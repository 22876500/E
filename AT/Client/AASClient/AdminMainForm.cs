using AASClient.AASServiceReference;
using DataGridViewAutoFilter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AASClient
{
    public partial class AdminMainForm : Form
    {
        public AdminMainForm()
        {
            InitializeComponent();
        }

        private void AdminMainForm_Load(object sender, EventArgs e)
        {
           
            this.dataGridView用户.AutoGenerateColumns = false;
            this.dataGridView风控.AutoGenerateColumns = false;
            this.dataGridView已分配交易员.AutoGenerateColumns = false;
            this.dataGridView待分配交易员.AutoGenerateColumns = false;
            this.dataGridView券商帐户.AutoGenerateColumns = false;
            //this.dataGridView交易额度.AutoGenerateColumns = false;
            this.dataGridView普通用户.AutoGenerateColumns = false;
            this.dataGridViewMAC.AutoGenerateColumns = false;
            this.dataGridView恒生帐户.AutoGenerateColumns = false;
            dataGridViewShareLimitUser.AutoGenerateColumns = false;
            dataGridViewShareLimitStocks.AutoGenerateColumns = false;


          


            if (Program.Current平台用户.角色 == (int)角色.普通管理员)
            {
                this.tabPage2.Parent = null;
                this.tabPage5.Parent = null;
                this.tabPage6.Parent = null;
            }

            this.Text = string.Format("{0} {1}", ((角色)Program.Current平台用户.角色).ToString(), Program.Current平台用户.用户名);

            this.tabControl1_SelectedIndexChanged(this.tabControl1, new EventArgs());

            InitAyersAccount();

            InitShareLimit();
        }

        private void InitAyersAccount()
        {
            try
            {
                var acc = Program.AASServiceClient.GetAyersAccount();
                if (acc.StartsWith("1|") && acc.Length > 3)
                {
                    var xDoc = XDocument.Parse(acc.Substring(2));
                    if (xDoc != null)
                    {
                        textBoxServerIP.Text = xDoc.Root.Element("server_ip").Value;
                        textBoxPortNoEncryption.Text = xDoc.Root.Element("port_no_encryption").Value;
                        textBoxPortEncryption.Text = xDoc.Root.Element("port_encryption").Value;
                        textBoxEncryptionKey.Text = xDoc.Root.Element("encryption_key").Value;
                        textBoxMessageCompression.Text = xDoc.Root.Element("message_compression").Value;
                        textBoxApiLoginID.Text = xDoc.Root.Element("api_login_id").Value;
                        textBoxApiLogionPsw.Text = xDoc.Root.Element("api_login_psw").Value;
                        textBoxAyersSite.Text = xDoc.Root.Element("site").Value;
                        textBoxAyersStation.Text = xDoc.Root.Element("station").Value;
                        textBoxAyersType.Text = xDoc.Root.Element("type").Value;
                        textBoxClientID1.Text = xDoc.Root.Element("client_first").Element("id").Value;
                        textBoxClientID1Psw.Text = xDoc.Root.Element("client_first").Element("password").Value;
                        textBoxClientID2.Text = xDoc.Root.Element("client_second").Element("id").Value;
                        textBoxClientID2Psw.Text = xDoc.Root.Element("client_second").Element("password").Value;
                        textBoxClientIDUsing.Text = xDoc.Root.Element("client_using").Value;
                        textBoxPortUsing.Text = xDoc.Root.Element("port_using").Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogRunning("Ayers 账户信息获取异常! Message:{0}, StackTrace:{1}", ex.Message, ex.StackTrace);
            }
        }

        

        private void AdminMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.logger.LogRunning("正在关闭连接...");
            try
            {
                if (Program.AASServiceClient.State == CommunicationState.Opened)
                {
                    Program.AASServiceClient.Close();
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogRunning(ex.Message);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
                switch (this.tabControl1.SelectedTab.Text)
                {
                    case "平台用户管理":
                        //if (Program.Current平台用户.角色 == (int)角色.普通管理员 && Program.Current平台用户.分组 == (int)分组.ALL)
                        //{
                        //    this.bindingSource用户.DataSource = Tool.GetNewDataTable(Program.AASServiceClient.QueryUser(), string.Format("分组<>{0}", (int)分组.ALL));
                        //}
                        //else
                        //{
                        //    this.bindingSource用户.DataSource = Program.AASServiceClient.QueryUser();
                        //}
                        this.bindingSource用户.DataSource = Program.AASServiceClient.QueryUser();
                        this.dataGridView用户.DataSource = this.bindingSource用户;
                        break;
                    case "风控分配管理":
                        this.bindingSource风控.DataSource = Program.AASServiceClient.QueryFK();
                        this.dataGridView风控.DataSource = this.bindingSource风控;

                        this.dataGridView已分配交易员.DataSource = this.bindingSource已分配交易员;
                        this.dataGridView待分配交易员.DataSource = this.bindingSource待分配交易员;
                        break;
                    case "券商帐户管理":
                        this.bindingSource券商帐户.DataSource = Program.AASServiceClient.QueryQsAccount();
                        this.dataGridView券商帐户.DataSource = this.bindingSource券商帐户;
                        break;
                    case "股票分配管理":
                        this.bindingSource交易额度.DataSource = Program.AASServiceClient.QueryTradeLimit();
                        this.dataGridView交易额度.DataSource = this.bindingSource交易额度;
                        this.dataGridView交易额度.Columns["交易额度"].DefaultCellStyle.Format = "f0";
                        break;
                    case "MAC地址管理":
                        this.bindingSource普通用户.DataSource = Program.AASServiceClient.Query普通用户();
                        this.dataGridView普通用户.DataSource = this.bindingSource普通用户;

                        this.dataGridViewMAC.DataSource = this.bindingSourceMAC;


                        break;
                    case "恒生帐户管理":
                        this.bindingSource恒生帐户.DataSource = Program.AASServiceClient.QueryHsAccount();
                        this.dataGridView恒生帐户.DataSource = this.bindingSource恒生帐户;


                        break;
                    case "额度共享管理":
                        InitShareLimit();
                        break;
                    default:
                        break;
                }
        }

        #region 用户管理
        private void 导入用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult DialogResult1 = this.openFileDialog1.ShowDialog();
                if (DialogResult1 != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }



                string[] FileContent = File.ReadAllLines(this.openFileDialog1.FileName, Encoding.Default);


                for (int i = 1; i < FileContent.Length; i++)
                {
                    string[] Data = FileContent[i].Split(',');


                    角色 角色1 = (角色)Enum.Parse(typeof(角色), Data[2], false);
                    分组 分组1 = (分组)Enum.Parse(typeof(分组), Data[7], false);

                    Program.AASServiceClient.AddUser(Data[0], Data[1], 角色1, decimal.Parse(Data[3]), decimal.Parse(Data[4]), decimal.Parse(Data[5]), Data[6] == "是" ? true : false, 分组1);

                }

            }
            catch (Exception ex)
            {
                Exception ex1 = ex.InnerException == null ? ex : ex.InnerException;

                MessageBox.Show(ex1.Message + ex1.StackTrace);
            }

            this.bindingSource用户.DataSource = Program.AASServiceClient.QueryUser();

        }



        private void toolStripMenuItem重置密码_Click(object sender, EventArgs e)
        {
            AASClient.AASServiceReference.DbDataSet.平台用户Row DataRow1 = (this.bindingSource用户.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.平台用户Row;

            ResetPasswordForm ResetPasswordForm1 = new ResetPasswordForm(DataRow1);
            ResetPasswordForm1.ShowDialog();
        }


        private void 新建用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddUserForm AddUserForm1 = new AddUserForm();
            DialogResult DialogResult1 = AddUserForm1.ShowDialog();
            if (DialogResult1 != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            this.bindingSource用户.DataSource = Program.AASServiceClient.QueryUser();
        }

        private void dataGridView用户_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((sender as DataGridView).Columns[e.ColumnIndex].HeaderText == "角色")
            {
                int int1 = (int)e.Value;

                角色 角色1 = (角色)Enum.Parse(typeof(角色), int1.ToString(), false);
                e.Value = 角色1.ToString();
            }
            else if ((sender as DataGridView).Columns[e.ColumnIndex].HeaderText == "分组")
            {

                分组 分组1 = (分组)Enum.Parse(typeof(分组), e.Value.ToString(), false);
                e.Value = 分组1.ToString();
            }
        }



        private void dataGridView用户_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AASClient.AASServiceReference.DbDataSet.平台用户Row DataRow1 = (this.bindingSource用户.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.平台用户Row;



            ModifyUserForm ModifyUserForm1 = new ModifyUserForm(DataRow1);
            DialogResult DialogResult1 = ModifyUserForm1.ShowDialog();
            if (DialogResult1 != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            this.bindingSource用户.DataSource = Program.AASServiceClient.QueryUser();
            
            
        }
        private void toolStripMenuItem修改用户_Click(object sender, EventArgs e)
        {
            AASClient.AASServiceReference.DbDataSet.平台用户Row DataRow1 = (this.bindingSource用户.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.平台用户Row;



            ModifyUserForm ModifyUserForm1 = new ModifyUserForm(DataRow1);
            DialogResult DialogResult1 = ModifyUserForm1.ShowDialog();
            if (DialogResult1 != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            this.bindingSource用户.DataSource = Program.AASServiceClient.QueryUser();


        }


        private void 删除用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AASClient.AASServiceReference.DbDataSet.平台用户Row DataRow1 = (this.bindingSource用户.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.平台用户Row;

            Program.AASServiceClient.DeleteUser(DataRow1.用户名);

            this.bindingSource用户.DataSource = Program.AASServiceClient.QueryUser();
        }

        #endregion

        #region 风控分配管理


        private void dataGridView风控_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            AASClient.AASServiceReference.DbDataSet.平台用户Row DataRow1 = (this.bindingSource风控.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.平台用户Row;
            string 风控员 = DataRow1.用户名;


            this.bindingSource已分配交易员.DataSource = Program.AASServiceClient.QueryJyBelongFK(风控员);
         


            this.bindingSource待分配交易员.DataSource = Program.AASServiceClient.QueryJyNotBelongFK(风控员);
        }




        private void dataGridView已分配交易员_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                bool bool1 = false;
                if (this.dataGridView已分配交易员["选择交易员", e.RowIndex].Value != null)
                {
                    bool1 = (bool)this.dataGridView已分配交易员["选择交易员", e.RowIndex].Value;
                }


                this.dataGridView已分配交易员["选择交易员", e.RowIndex].Value = !bool1;
            }
        }


        private void dataGridView待分配交易员_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                bool bool1 = false;
                if (this.dataGridView待分配交易员["选择", e.RowIndex].Value != null)
                {
                    bool1 = (bool)this.dataGridView待分配交易员["选择", e.RowIndex].Value;
                }

                this.dataGridView待分配交易员["选择", e.RowIndex].Value = !bool1;
            }
        }



        private void 移入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.bindingSource风控.Current == null)
            {
                MessageBox.Show("必须先选定风控员才能分配交易员");
                return;
            }


            AASClient.AASServiceReference.DbDataSet.平台用户Row DataRow1 = (this.bindingSource风控.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.平台用户Row;
            string 风控员 = DataRow1.用户名;


            List<string> Traders = new List<string>();
            foreach (DataGridViewRow DataGridViewRow1 in this.dataGridView待分配交易员.Rows)
            {
                if (DataGridViewRow1.Cells["选择"].Value != null)
                {
                    bool bool1 = (bool)DataGridViewRow1.Cells["选择"].Value;
                    if (bool1)
                    {
                        Traders.Add(DataGridViewRow1.Cells[1].Value as string);
                    }
                }
            }



            Program.AASServiceClient.AddTraderToRC(风控员, Traders.ToArray());

            this.bindingSource已分配交易员.DataSource = Program.AASServiceClient.QueryJyBelongFK(风控员);

            this.bindingSource待分配交易员.DataSource = Program.AASServiceClient.QueryJyNotBelongFK(风控员);
        }

        private void 移出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.bindingSource风控.Current == null)
            {
                MessageBox.Show("必须先选定风控员才能分配交易员");
                return;
            }


            AASClient.AASServiceReference.DbDataSet.平台用户Row DataRow1 = (this.bindingSource风控.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.平台用户Row;
            string 风控员 = DataRow1.用户名;


            List<string> Traders = new List<string>();
            foreach (DataGridViewRow DataGridViewRow1 in this.dataGridView已分配交易员.Rows)
            {
                if (DataGridViewRow1.Cells["选择交易员"].Value != null)
                {
                    bool bool1 = (bool)DataGridViewRow1.Cells["选择交易员"].Value;
                    if (bool1)
                    {
                        Traders.Add(DataGridViewRow1.Cells[1].Value as string);
                    }
                }
            }



            Program.AASServiceClient.DeleteTraderFromRC(风控员, Traders.ToArray());

            this.bindingSource已分配交易员.DataSource = Program.AASServiceClient.QueryJyBelongFK(风控员);

            this.bindingSource待分配交易员.DataSource = Program.AASServiceClient.QueryJyNotBelongFK(风控员);
        }



        #endregion

        #region 券商交易帐户管理

        private void dataGridView券商帐户_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                AASClient.AASServiceReference.DbDataSet.券商帐户Row DataRow1 = (this.bindingSource券商帐户.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.券商帐户Row;

                Program.AASServiceClient.EnableQSAccount(DataRow1.名称, !DataRow1.启用);

                this.bindingSource券商帐户.DataSource = Program.AASServiceClient.QueryQsAccount();
            }
        }


        private void 添加券商帐号ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            AddQSAccountForm AddQSAccountForm1 = new AddQSAccountForm();
            DialogResult DialogResult1 = AddQSAccountForm1.ShowDialog();
            if (DialogResult1 != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            this.bindingSource券商帐户.DataSource = Program.AASServiceClient.QueryQsAccount();

        }


        private void dataGridView券商帐户_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            AASClient.AASServiceReference.DbDataSet.券商帐户Row DataRow1 = (this.bindingSource券商帐户.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.券商帐户Row;

            ModifyQSAccountForm ModifyQSAccountForm1 = new ModifyQSAccountForm(DataRow1);
            DialogResult DialogResult1 = ModifyQSAccountForm1.ShowDialog();
            if (DialogResult1 != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }


            this.bindingSource券商帐户.DataSource = Program.AASServiceClient.QueryQsAccount();
        }
        private void 修改券商帐户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AASClient.AASServiceReference.DbDataSet.券商帐户Row DataRow1 = (this.bindingSource券商帐户.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.券商帐户Row;


            ModifyQSAccountForm ModifyQSAccountForm1 = new ModifyQSAccountForm(DataRow1);
            DialogResult DialogResult1 = ModifyQSAccountForm1.ShowDialog();
            if (DialogResult1 != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }


            this.bindingSource券商帐户.DataSource = Program.AASServiceClient.QueryQsAccount();
        }

        private void 删除券商帐户ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AASClient.AASServiceReference.DbDataSet.券商帐户Row DataRow1 = (this.bindingSource券商帐户.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.券商帐户Row;


            Program.AASServiceClient.DeleteQSAccount(DataRow1.名称);

            this.bindingSource券商帐户.DataSource = Program.AASServiceClient.QueryQsAccount();
        }

        #endregion

        #region 交易额度管理

        private void 导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult DialogResult1 = this.openFileDialog1.ShowDialog();
            if (DialogResult1 != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }


            string[] FileContent = File.ReadAllLines(this.openFileDialog1.FileName, Encoding.Default);

            StringBuilder sbErr = new StringBuilder(128);
            Dictionary<string, int> dictIndex = new Dictionary<string, int>();
            Dictionary<string, List<string>> dictTraderStock = new Dictionary<string, List<string>>();
            for (int i = 0; i < FileContent.Length; i++)
            {
                var item = FileContent[i];
                string[] Data = FileContent[i].Split(',');
                if (dictTraderStock.ContainsKey(Data[0]))
                {
                    if (!dictTraderStock[Data[0]].Contains(Data[1]))
                    {
                        dictIndex.Add(Data[0] + "_" + Data[1], i);
                        dictTraderStock[Data[0]].Add(Data[1]);
                    }
                    else
                    {
                        var index = dictIndex[Data[0] + "_" + Data[1]];
                        sbErr.AppendFormat("交易员{0} 对应股票{1} 在{2}行数据有重复项, 与第{3}行数据发生冲突", Data[0], Data[1], i.ToString(), index).Append(Environment.NewLine);
                    }
                }
                else
                {
                    dictTraderStock.Add(Data[0], new List<string>() { Data[1] });
                    dictIndex.Add(Data[0] + "_" + Data[1], i);
                }
            }
            if (sbErr.Length > 0)
            {
                Program.logger.LogInfo(sbErr.ToString());
                sbErr.Append("请修改至无重复项再导入！");
                MessageBox.Show(sbErr.ToString());
            }
            else
            {
                AASClient.AASServiceReference.DbDataSet.平台用户DataTable 平台用户DataTable1 =  Program.AASServiceClient.QueryUser();
                for (int i = 1; i < FileContent.Length; i++)
                {
                    string[] Data = FileContent[i].Split(',');

                    买模式 买模式1 = (买模式)Enum.Parse(typeof(买模式), Data[6], false);
                    卖模式 卖模式1 = (卖模式)Enum.Parse(typeof(卖模式), Data[7], false);

                    try
                    {
                        foreach (AASClient.AASServiceReference.DbDataSet.平台用户Row 平台用户Row1 in 平台用户DataTable1.Where(r => r.用户名.Equals(Data[0].Trim())))
                        {
                            Program.AASServiceClient.AddTradeLimit(Data[0], Data[1], Data[2], byte.Parse(Data[3]), Data[4], Data[5], 买模式1, 卖模式1, int.Parse(Data[8]), decimal.Parse(Data[9]));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("导入第{0}条记录[{1} {2}]失败,{3}", i, Data[0], Data[1], ex.Message));
                        break;
                    }
                }

            }

            this.bindingSource交易额度.DataSource = Program.AASServiceClient.QueryTradeLimit();

        }
        private void 添加交易额度ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            AddTradeLimitForm AddTradeLimitForm1 = new AddTradeLimitForm();
            DialogResult DialogResult1= AddTradeLimitForm1.ShowDialog();
            if (DialogResult1 != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            this.bindingSource交易额度.DataSource = Program.AASServiceClient.QueryTradeLimit();
        }

        private void dataGridView交易额度_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((sender as DataGridView).Columns[e.ColumnIndex].Name == "市场")
            {
                byte byte1 = (byte)e.Value;


                e.Value = byte1 == 0 ? "深圳" : "上海";
            }
            else if ((sender as DataGridView).Columns[e.ColumnIndex].Name == "买模式")
            {
                买模式 买模式1 = (买模式)e.Value;
                e.Value = 买模式1.ToString();
            }
            else if ((sender as DataGridView).Columns[e.ColumnIndex].Name == "卖模式")
            {
                卖模式 卖模式1 = (卖模式)e.Value;
                e.Value = 卖模式1.ToString();
            }
        }

       

      



        private void dataGridView交易额度_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex==-1)
            {
                return;
            }



            AASClient.AASServiceReference.DbDataSet.额度分配Row DataRow1 = (this.bindingSource交易额度.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.额度分配Row;



            ModifyTradeLimitForm ModifyTradeLimitForm1 = new ModifyTradeLimitForm(DataRow1);
            DialogResult DialogResult1 = ModifyTradeLimitForm1.ShowDialog();
            if (DialogResult1 != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }


            this.bindingSource交易额度.DataSource = Program.AASServiceClient.QueryTradeLimit();
        }

        private void 修改交易额度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AASClient.AASServiceReference.DbDataSet.额度分配Row DataRow1 = (this.bindingSource交易额度.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.额度分配Row;



            ModifyTradeLimitForm ModifyTradeLimitForm1 = new ModifyTradeLimitForm(DataRow1);
            DialogResult DialogResult1 = ModifyTradeLimitForm1.ShowDialog();
            if (DialogResult1 != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }


            this.bindingSource交易额度.DataSource = Program.AASServiceClient.QueryTradeLimit();
        }

        private void 删除交易额度ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //AASClient.AASServiceReference.DbDataSet.额度分配Row DataRow1 = (this.bindingSource交易额度.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.额度分配Row;


            //Program.AASServiceClient.DeleteTradeLimit(DataRow1.交易员, DataRow1.证券代码);


            foreach(DataGridViewRow DataGridViewRow1 in this.dataGridView交易额度.SelectedRows)
            {
                Program.AASServiceClient.DeleteTradeLimit(DataGridViewRow1.Cells["Column13"].Value as string, DataGridViewRow1.Cells["Column14"].Value as string);//交易员 列 证券代码列
            }


            this.bindingSource交易额度.DataSource = Program.AASServiceClient.QueryTradeLimit();
        }

        private void 全部显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewAutoFilterColumnHeaderCell.ValueListForFilter.Clear();


            this.bindingSource交易额度.Filter = null;
        }
  




        #endregion

        #region MAC地址管理

        private void dataGridView普通用户_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            AASClient.AASServiceReference.DbDataSet.平台用户Row DataRow1 = (this.bindingSource普通用户.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.平台用户Row;




            this.bindingSourceMAC.DataSource = Program.AASServiceClient.QueryMACBelongUser(DataRow1.用户名);
        }


        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AASClient.AASServiceReference.DbDataSet.平台用户Row DataRow1 = (this.bindingSource普通用户.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.平台用户Row;


            AddMACForm AddMACForm1 = new AddMACForm(DataRow1.用户名);
            AddMACForm1.ShowDialog();

            this.bindingSourceMAC.DataSource = Program.AASServiceClient.QueryMACBelongUser(DataRow1.用户名);
        }

        private void 删除ToolStripMenuItem2_Click(object sender, EventArgs e)
        {

            AASClient.AASServiceReference.DbDataSet.MAC地址分配Row DataRow2 = (this.bindingSourceMAC.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.MAC地址分配Row;


            Program.AASServiceClient.DeleteMAC(DataRow2.用户名, DataRow2.MAC);

            this.bindingSourceMAC.DataSource = Program.AASServiceClient.QueryMACBelongUser(DataRow2.用户名);
        }


        #endregion

        #region 恒生帐户管理
        private void 添加帐户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddHsAccountForm AddHsAccountForm1 = new AddHsAccountForm();
            DialogResult DialogResult1 = AddHsAccountForm1.ShowDialog();
            if (DialogResult1 != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }


            this.bindingSource恒生帐户.DataSource = Program.AASServiceClient.QueryHsAccount();
        }

        private void dataGridView恒生帐户_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AASClient.AASServiceReference.DbDataSet.恒生帐户Row DataRow1 = (this.bindingSource恒生帐户.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.恒生帐户Row;

            ModifyHsForm ModifyHsForm1 = new ModifyHsForm(DataRow1);
            DialogResult DialogResult1 = ModifyHsForm1.ShowDialog();
            if (DialogResult1 != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }


            this.bindingSource恒生帐户.DataSource = Program.AASServiceClient.QueryHsAccount();
        }


        private void 修改ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AASClient.AASServiceReference.DbDataSet.恒生帐户Row DataRow1 = (this.bindingSource恒生帐户.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.恒生帐户Row;

            ModifyHsForm ModifyHsForm1 = new ModifyHsForm(DataRow1);
            DialogResult DialogResult1 = ModifyHsForm1.ShowDialog();
            if (DialogResult1 != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }


            this.bindingSource恒生帐户.DataSource = Program.AASServiceClient.QueryHsAccount();
        }

        private void 删除ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            AASClient.AASServiceReference.DbDataSet.恒生帐户Row DataRow1 = (this.bindingSource恒生帐户.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.恒生帐户Row;


            Program.AASServiceClient.DeleteHsAccount(DataRow1.名称);

            this.bindingSource恒生帐户.DataSource = Program.AASServiceClient.QueryHsAccount();
        }


        private void dataGridView恒生帐户_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                AASClient.AASServiceReference.DbDataSet.恒生帐户Row DataRow1 = (this.bindingSource恒生帐户.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.恒生帐户Row;

                Program.AASServiceClient.EnableHsAccount(DataRow1.名称, !DataRow1.启用);

                this.bindingSource恒生帐户.DataSource = Program.AASServiceClient.QueryHsAccount();
            }
        }



        #endregion

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                string serverIP = textBoxServerIP.Text.Trim();
                int port = int.Parse(textBoxPortNoEncryption.Text.Trim());
                int portEncry = int.Parse(textBoxPortEncryption.Text.Trim());
                string encryKey = textBoxEncryptionKey.Text.Trim();
                string messageCompression = textBoxMessageCompression.Text.Trim();
                string apiLoginID = textBoxApiLoginID.Text.Trim();
                string apiLoginPsw = textBoxApiLogionPsw.Text.Trim();

                string site = textBoxAyersSite.Text.Trim();
                string station = textBoxAyersStation.Text.Trim();
                string type = textBoxAyersType.Text.Trim();
                string clientID1 = textBoxClientID1.Text.Trim();
                string psw = textBoxClientID1Psw.Text.Trim();
                string clientID2 = textBoxClientID2.Text.Trim();
                string clientID2Psw = textBoxClientID2Psw.Text.Trim();
                string clientIdUsing = textBoxClientIDUsing.Text.Trim();
                int portUsing = int.Parse(textBoxPortUsing.Text.Trim());
                bool success = Program.AASServiceClient.UpdateAyersAccount(serverIP, port, portEncry, encryKey, messageCompression, 
                    apiLoginID, apiLoginPsw, site, station, type, clientID1, psw, clientID2, clientID2, clientIdUsing, portUsing);

                MessageBox.Show(string.Format("保存{0}！", success ? "成功": "失败"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("保存时发生异常，Message{0}！", ex.Message));
            }
        }

        #region 额度共享
        private void buttonShareLimitSearch_Click(object sender, EventArgs e)
        {
            //显示额度共享界面
            try
            {
                string groupName = comboBoxShareLimit.SelectedItem.ToString();
                if (ShareLimitGroups != null && ShareLimitGroups.Length > 0 && !string.IsNullOrEmpty(groupName))
                {
                    var groupItem = ShareLimitGroups.First(_=> _.GroupName == groupName);
                    BindingShareLimitData(groupItem);
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo("初始化额度共享异常：" + ex.Message);
            }
        }
        
        private void buttonShareGroupEdit_Click(object sender, EventArgs e)
        {
            if (ShareLimitGroups == null)
            {
                MessageBox.Show("额度共享分组为null，请检查配置项。");
                return;
            }
            ShareLimitGroupItem selectedGroup = null;
            if (comboBoxShareLimit.SelectedIndex > -1)
            {
                selectedGroup = ShareLimitGroups[comboBoxShareLimit.SelectedIndex];
            }
            else
            {
                selectedGroup = ShareLimitGroups[0];
            }

            List<LimitTrader> AllTradersGrouped = new List<LimitTrader>();
            ShareLimitGroups.ToList().ForEach(_ => AllTradersGrouped.AddRange(_.GroupTraderList));
            var win = new SharedLimitEdit();
            win.Init(selectedGroup.GroupName);
            win.ShowDialog();

            InitShareLimit();
        }

        private ShareLimitGroupItem[] ShareLimitGroups = null;
        private void InitShareLimit()
        {
            try
            {
                var groups = Program.AASServiceClient.ShareGroupQuery();
                if (groups != null && groups.Length > 0)
                {
                    comboBoxShareLimit.Items.Clear();
                    groups.ToList().ForEach(_ => comboBoxShareLimit.Items.Add(_.GroupName));
                    comboBoxShareLimit.SelectedIndex = 0;

                    var Group = groups.First();

                    BindingShareLimitData(Group);
                }
                ShareLimitGroups = groups;
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo("初始化额度共享异常：" + ex.Message);
            }

        }

        private void BindingShareLimitData(ShareLimitGroupItem Group)
        {
            bindingSourceShareLimitUser.DataSource = Group.GroupTraderList;
            dataGridViewShareLimitUser.DataSource = bindingSourceShareLimitUser;

            bindingSourceShareLimitStocks.DataSource = Group.GroupStockList;
            dataGridViewShareLimitStocks.DataSource = bindingSourceShareLimitStocks;
        }

        private void dataGridViewShareLimitStocks_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null) return;

            if ((sender as DataGridView).Columns[e.ColumnIndex].HeaderText == "买入方式")
            {
                var buyType = e.Value.ToString();

                switch (buyType)
                {
                    case "0":
                        e.Value = "买入";
                        break;
                    case "1":
                        e.Value = "融资买入";
                        break;
                    default:
                        break;
                }

            }
            else if ((sender as DataGridView).Columns[e.ColumnIndex].HeaderText == "卖出方式")
            {
                var saleType = e.Value.ToString();

                switch (saleType)
                {
                    case "0":
                        e.Value = "卖出";
                        break;
                    case "1":
                        e.Value = "融券卖出";
                        break;
                    default:
                        break;
                }
            }
        }

        private void textBoxShareLimitKeyWords_TextChanged(object sender, EventArgs e)
        {
            if (ShareLimitGroups.Length <= 0) return;

            var keyWords = textBoxShareLimitKeyWords.Text.Trim();
            var groupInfo = ShareLimitGroups[comboBoxShareLimit.SelectedIndex];
            if (keyWords.Length > 0)
            {
                if (Regex.IsMatch(keyWords, "^[0-9]+$"))
                {//说明是股票代码
                    this.bindingSourceShareLimitStocks.DataSource = groupInfo.GroupStockList.Where(_ => _.StockID.Contains(keyWords)).OrderBy(_ => _.GroupAccount).ThenBy(_ => _.StockID);
                }
                else if (Regex.IsMatch(keyWords, "^[A-Za-z][0-9]*"))
                {
                    this.bindingSourceShareLimitStocks.DataSource = groupInfo.GroupStockList.Where(_ => _.GroupAccount.Contains(keyWords)).OrderBy(_ => _.GroupAccount).ThenBy(_ => _.StockID);
                }
                else
                {
                    this.bindingSourceShareLimitStocks.DataSource = groupInfo.GroupStockList.Where(_ => _.StockName.Contains(keyWords)).OrderBy(_ => _.GroupAccount).ThenBy(_ => _.StockID);
                }
            }
            else
            {
                this.bindingSourceShareLimitStocks.DataSource = groupInfo.GroupStockList.OrderBy(_ => _.GroupAccount).ThenBy(_ => _.StockID);
            }
        }

        #endregion










    }
}
