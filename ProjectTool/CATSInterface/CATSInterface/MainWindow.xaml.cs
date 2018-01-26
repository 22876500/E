using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CATSInterface
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        bool systemClosing = false;
        

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.Closed += Window_Closed;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitCheck();

            BindData();

            Thread autoCloseThread = new Thread(new ThreadStart(AutoRun)) { IsBackground = true };
            autoCloseThread.Start();
        }

        /// <summary>
        /// 自动开启扫单功能之前，预先检测是否存在旧数据，存在则清除，不存在则
        /// </summary>
        private void InitCheck()
        {
            //if (ConfigData.CHECK_OPPO)
            //{

            //}
            this.Title = string.Format("CATSInterface 启动时间:{0}", DateTime.Now.ToString());
            rtbMainInfo.AppendText(ConfigData.CHECK_OPPO ? "已开启自成交检测。\r" : "未开启自成交检测。\r");


            if (DateTime.Now.Hour < ConfigData.AutoStartHour)
            {
                if (CATSAdapter.Instance.ExistDBFData())
                {
                    string errorValue = CATSAdapter.Instance.ClearDataFile(false);
                    if (!string.IsNullOrEmpty(errorValue))
                    {
                        rtbMainInfo.AppendText(string.Format("{0}:  {1}\r", DateTime.Now.ToString(), errorValue));
                    }
                    else
                    {
                        rtbMainInfo.AppendText("清除文件成功. \r");
                    }
                }
                else
                {
                    rtbMainInfo.AppendText(string.Format("{0}:  已检测下单指令表及委托表为空表，无需清除文件. \r", DateTime.Now.ToString()));
                }
            }
            else
            {
                rtbMainInfo.AppendText(string.Format("自动启动时间已过，启动将不清除文件内容，避免删除正常下单信息!"));
            }
        }

        private void AutoRun()
        {
            bool hasAutoStarted = false;
            while (true)
            {
                if (DateTime.Now.Hour >= ConfigData.AutoCloseHour)
                {
                    Utils.logger.LogInfo("自动关闭扫单工具CATSInterface开始……");

                    DeleteFiles(true);

                    systemClosing = true;

                    Dispatcher.Invoke(() => { this.Close(); });

                }
                else if (DateTime.Now.Hour > ConfigData.AutoStopHour)
                {
                    Thread.Sleep(10000);//停止扫单
                    CATSAdapter.Instance.Stop();
                }
                else if (DateTime.Now.Hour >= ConfigData.AutoStartHour && !CATSAdapter.Instance.IsStarted && !hasAutoStarted)
                {
                    CATSAdapter.Instance.Start();
                }
                else
                {
                    Thread.Sleep(10000);
                }
            }
        }

        private void DeleteFiles(bool isBakFile)
        {
            try
            {
                if (CATSAdapter.Instance.ExistDBFData())
                {
                    CATSAdapter.Instance.ClearDataFile(isBakFile);
                }
            }
            catch (Exception ex)
            {
                Utils.logger.LogInfo("删除文件异常:{0}", ex.Message);
            }
        }

        private void MenuItem_Path_Click(object sender, RoutedEventArgs e)
        {
            //显示默认路径配置，可修改
            var win = new wCATSConfigEdit();
            win.ShowDialog();
        }

        private void StartMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //开始扫数据
            CATSAdapter.Instance.Start();
        }

        private void BindData()
        {
            //miStartScan.IsEnabled = new Binding();
            this.DataContext = CATSAdapter.Instance;
            dgOrder.ItemsSource = CATSAdapter.Instance.StandardOrderDict.Values.ToList();
            dgInstruction.ItemsSource = CATSAdapter.Instance.AddInstructionDict.Values.ToList();
            CATSAdapter.Instance.OnOrderChange += new Action(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    dgOrder.ItemsSource = CATSAdapter.Instance.dtOrderNewst.DefaultView;
                    //dgOrder.ItemsSource = CATSAdapter.Instance.StandardOrderDict.Values.OrderBy(_ => _.OrderTime);
                });
            });
            CATSAdapter.Instance.OnAddInstructionChange += new Action(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    dgInstruction.ItemsSource = CATSAdapter.Instance.AddInstructionDict.Values.ToList();
                });
            });
            //CATSAdapter.Instance.OnCancelInstructionChange += new Action(() => 
            //{
            //    this.Dispatcher.Invoke(() => 
            //    {
            //        dgCancelInstruction.ItemsSource = CATSAdapter.Instance.CancelInstructionDict.Values.ToList();
            //    });
            //});
        }

        private void StopMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //停止读数据
            CATSAdapter.Instance.Stop();
        }

        //运行时间配置
        private void RunTimeMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Utils.logger.LogInfo("扫单工具CATSInterface关闭完成。");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!systemClosing)
            {
                var result = MessageBox.Show("确认关闭CATSInterface吗，将不执行接收下单命令并写入扫单文件的行为？", "关闭提示", MessageBoxButton.OKCancel);
                if (result != MessageBoxResult.OK)
                {
                    e.Cancel = true;
                }
                else if (DateTime.Now.Hour >= 15)
                {
                    DeleteFiles(true);
                }
            }
        }

        private void Button_CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null && btn.DataContext is Entities.StandardOrderEntity)
            {
                var order = btn.DataContext as Entities.StandardOrderEntity;
                //如何撤单、、
                if (string.IsNullOrWhiteSpace(order.Account))
                {
                    MessageBox.Show("委托账户为空，无法撤单!");
                }
                else if (string.IsNullOrWhiteSpace(order.OrderNo))
                {
                    MessageBox.Show("委托编号为空，无法撤单!");
                }
                else if (order.OrderQty == (order.FilledQty + order.CancelQty))
                {
                    MessageBox.Show(string.Format("委托数量({0})等于成交数({1})加撤单数({2})，无法撤单!", order.OrderQty, order.FilledQty, order.CancelQty));
                }
                else
                {
                    if (MessageBox.Show("确认撤单吗? ", "撤单操作确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        CATSAdapter.Instance.CancelOrder(order.Account, order.OrderNo);
                    }
                }
                
            }
        }

        private void Button_SetExceptionOrder_Click(object sender, RoutedEventArgs e)
        {
            
            var btn = sender as Button;
            if (btn != null && btn.DataContext is Entities.StandardOrderEntity)
            {
                var order = btn.DataContext as Entities.StandardOrderEntity;

                if (order.OrderQty > 0 && order.CancelQty == 0 && order.FilledQty == 0)
                {
                    try
                    {
                        CATSAdapter.Instance.SetExceptionOrder(order);
                        dgSignedErrorOrder.ItemsSource = CATSAdapter.Instance.ExceptionOrderDict.Values.OrderBy(_ => _.OrderTime);
                        tbException.IsSelected = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("设置废单异常, {0}", ex.Message));
                    }
                }
                else
                {
                    MessageBox.Show("废单设置失败，废单必须为委托数大于0，撤单数等于0，成交股数等于0");
                }

                
            }
        }

        private void Button_CancelExceptionOrder_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null && btn.DataContext is Entities.StandardOrderEntity)
            {
                var order = btn.DataContext as Entities.StandardOrderEntity;
                Entities.StandardOrderEntity orderValue;
                if (CATSAdapter.Instance.ExceptionOrderDict.TryRemove(order.OrderNo, out orderValue))
                {
                    dgSignedErrorOrder.ItemsSource = CATSAdapter.Instance.ExceptionOrderDict.Values.OrderBy(_ => _.OrderTime);
                }
                else
                {
                    MessageBox.Show("撤销废单操作失败, 请稍后再试。");
                }
            }
            
        }

    }
}
