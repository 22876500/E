using DataModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    public partial class PrewarningShowForm : Form
    {
        public const string PreWarningInfoDir = "PreWarning";

        #region Properties
        static string WarningFilePath
        {
            get
            {
                return PreWarningInfoDir + "\\PreWarningInfo" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
            }
        } 
        #endregion

        #region Delegates
        public Action<string> OnWarningClick; 
        #endregion

        #region Members
        public Model.WarningFormulaOne Formula;
        System.Windows.Forms.Timer timer;

        static ConcurrentQueue<AASClient.Model.WarningEntity> WarningHistory = new ConcurrentQueue<Model.WarningEntity>();
        static Thread saveThread = null;
        #endregion

        #region Init
        public PrewarningShowForm(Model.WarningFormulaOne formula)
        {
            InitializeComponent();
            this.Formula = formula;
            this.Text = formula.计算方法;
            Init();
        }

        private void Init()
        {
            this.Load += PrewarningShowForm_Load;
            this.FormClosed += PrewarningShowForm_FormClosed;
            var dir = WarningFilePath.Substring(0, WarningFilePath.IndexOf('\\'));
            if (!System.IO.Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (saveThread == null)
            {
                saveThread = new Thread(new ThreadStart(SaveWarningInfo)) { IsBackground = true };
                saveThread.Start();
            }
        }

        private static void SaveWarningInfo()
        {
            while (true)
            {
                if (WarningHistory.Count == 0)
                {
                    Thread.Sleep(2000);
                }
                else
                {
                    try
                    {
                        Model.WarningEntity we = null;
                        Queue<string> queue = new Queue<string>();
                        for (int i = WarningHistory.Count; i > -1; i--)
                        {
                            if (WarningHistory.Count > 0 && WarningHistory.TryDequeue(out we))
                            {
                                var s = string.Format("{0} {1}, {2}", DateTime.Today.ToShortDateString(), we.DataTime, we.WarningDescription);
                                queue.Enqueue(s);
                            }
                        }
                        File.AppendAllLines(WarningFilePath, queue, Encoding.UTF8);
                    }
                    catch (Exception ex)
                    {
                        Program.logger.LogRunning("预警记录出错！", ex.Message);
                    }
                }
            }
        }
        #endregion

        #region Events
        void PrewarningShowForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Stop();
            //if (testDataThread != null && testDataThread.ThreadState != ThreadState.Stopped)
            //{
            //    try
            //    {
            //        testDataThread.Abort();
            //    }
            //    catch (Exception ex)
            //    {
            //        Program.logger.LogRunning("预警显示界面，关闭测试数据生成线程异常\r\n  Message:{0}\r\n  StackTrace:{1}", ex.Message, ex.StackTrace);
            //    }
            //}
        }

        void PrewarningShowForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 20; i++)
            {
                ListViewItem ListViewItem3 = new ListViewItem(new string[] { "", "    ", "    ", "    ", "    ", "    ", "    ", "    ", "    ", "    " });

                this.listViewNFWarning.Items.Add(ListViewItem3);
            }

            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(TimerEvent);
            timer.Interval = 200;
            timer.Start();
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            try
            {
                int index = listViewNFWarning.Items.Count - 1;
                Model.WarningEntity warning = null;

                if (!Program.Warnings.ContainsKey(this.Formula.ID) || Program.Warnings[this.Formula.ID].Count == 0)
                {
                    return;
                }

                var queue = Program.Warnings[this.Formula.ID];
                for (int i = 0; i < 20; i++)
                {
                    if (queue.Count > 0 && queue.TryDequeue(out warning))
                    {
                        if (index > -1)
                            this.listViewNFWarning.Items.RemoveAt(index);

                        WarningHistory.Enqueue(warning);

                        ListViewItem listViewItemAdded = new ListViewItem(new string[] { "1", warning.Code, warning.DataTime, Math.Round((decimal)warning.Match / 10000, 2).ToString("#0.00"), warning.SubValue.ToString("#0.000"), warning.SubType, warning.WarningCount.ToString(), warning.LargeVolume, warning.LargeVolumnCost, warning.LargeVolumnFlag });
                        listViewItemAdded.BackColor = warning.Background;
                        listViewItemAdded.Tag = warning;


                        this.listViewNFWarning.Items.Insert(0, listViewItemAdded);
                        foreach (ListViewItem item in this.listViewNFWarning.Items)
                        {
                            if (item.Tag != null) item.SubItems[0].Text = (item.Index + 1).ToString();
                            else break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogRunning("预警功能定时出错：{0}\r\n  StackTrace:{1} ", ex.Message, ex.StackTrace);
            }
        }

        

        private void listViewNFWarning_Click(object sender, EventArgs e)
        {
            if (listViewNFWarning.SelectedItems.Count > 0)
            {
                var selectItem = listViewNFWarning.SelectedItems[0].Tag as AASClient.Model.WarningEntity;
                if (selectItem != null && OnWarningClick != null)
                {
                    OnWarningClick.Invoke(selectItem.Code);
                }
            }
        } 
        #endregion

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog sfd = new SaveFileDialog() { Filter = "文本文件|*.txt"};
            var dialog = sfd.ShowDialog();
            if (dialog == System.Windows.Forms.DialogResult.OK || dialog == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    //AutoSave();
                    var text = File.ReadAllText(WarningFilePath);
                    File.AppendAllText(sfd.FileName, text, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    Program.logger.LogRunning("导出预警记录异常\r\n  Message:{0}\r\n  StackTrace:{1}", ex.Message, ex.StackTrace);
                }
            }
        }


        private void 公式编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var win = new PrewarningAddForm(this.Formula);
            win.ShowDialog();
            this.Text = this.Formula.计算方法;
        }

    }
}
