using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoStock
{
    public partial class MainForm : Form
    {
        private DataTable originalDataTable = null;
        private DataTable oldDataTable = null;
        private DataTable newDataTable = null;
        private DataTable stockDataTable = null;

        private List<string> baseAllCode = null;
        private Dictionary<string, List<int>> changedAllCode = null;
        public MainForm()
        {
            this.StartPosition = FormStartPosition.CenterScreen; 
            InitializeComponent();
            Program.logger.Init(this.listView1);
            Program.logger.LogInfo("程序已启动");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.ShowDialog();

            //得到上传文件的完整名
            string loadFullName = ofd.FileName.ToString();

            this.textBox1.Text = loadFullName;

            if (string.IsNullOrEmpty(loadFullName))
            {
                MessageBox.Show("请选择导入文件");
                return;
            }

            if (loadFullName.EndsWith(".xls") || loadFullName.EndsWith(".xlsx"))
            {
                originalDataTable = ExcelHelper.ImportExcel(loadFullName, 0, 0);
            }
            else if (loadFullName.EndsWith(".csv"))
            {
                originalDataTable = CSVFileHelper.OpenCSV(this.textBox1.Text.Trim(), 0);
            }
            else
            {
                Program.logger.LogInfo("无法识别的文件格式，请选择excel或者csv文件");
                return;
            }

            if (originalDataTable == null || originalDataTable.Rows.Count == 0)
            {
                MessageBox.Show("总交易额度导入数据为0条");
                return;
            }
            MessageBox.Show(string.Format("导入成功,共{0}条", originalDataTable.Rows.Count));
            Program.logger.LogInfo(string.Format("总交易额度表导入成功，共{0}条", originalDataTable.Rows.Count));
        }


        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd1 = new OpenFileDialog();

            ofd1.ShowDialog();

            //得到上传文件的完整名
            string loadFullName = ofd1.FileName.ToString();

            this.textBox2.Text = loadFullName;

            if (string.IsNullOrEmpty(loadFullName))
            {
                MessageBox.Show("请选择导入文件");
                return;
            }

            if (loadFullName.EndsWith(".xls") || loadFullName.EndsWith(".xlsx"))
            {
                oldDataTable = ExcelHelper.ImportExcel(loadFullName, 0, 1);
            }
            else if (loadFullName.EndsWith(".csv"))
            {
                oldDataTable = CSVFileHelper.OpenCSV(this.textBox2.Text.Trim(), 1);
            }
            else
            {
                Program.logger.LogInfo("无法识别的文件格式，请选择excel或者csv文件");
                return;
            }

            if (oldDataTable == null || oldDataTable.Rows.Count == 0)
            {
                MessageBox.Show("已分配额度导入数据为0条");
                return;
            }
            MessageBox.Show(string.Format("导入成功,共{0}条", oldDataTable.Rows.Count));
            Program.logger.LogInfo(string.Format("已分配额度表导入成功，共{0}条", oldDataTable.Rows.Count));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sbNotCodeInbase = new StringBuilder();
                StringBuilder sbCodeInBase = new StringBuilder();
                Program.logger.LogInfo("自动检测开始...");
                string filePath = Application.StartupPath + @"\股票列表.xlsx";
                stockDataTable = ExcelHelper.ImportExcel(filePath, 0, 3);
                Program.logger.LogInfo(string.Format("加载股票列表excel文件结束，共{0}条", stockDataTable.Rows.Count));
                this.button6.Visible = false;
                if (originalDataTable == null || originalDataTable.Rows.Count == 0)
                {
                    MessageBox.Show("总交易额度数据为0条");
                    Program.logger.LogInfo("自动检测异常结束,请重新上传总交易额度表");
                    return;
                }
                if (oldDataTable == null || oldDataTable.Rows.Count == 0)
                {
                    MessageBox.Show("已分配额度数据为0条");
                    Program.logger.LogInfo("自动检测异常结束,请重新上传已分配额度表");
                    return;
                }
                newDataTable = new DataTable();
                baseAllCode = new List<string>();
                changedAllCode = new Dictionary<string, List<int>>();
                //string[] heads = new string[] { "交易员", "证券代码", "组合号", "市场", "证券名称", "拼音缩写", "买模式", "卖模式", "交易额度", "手续费率" };
                foreach (var item in CommonUtils.heads)
                {
                    DataColumn dc = new DataColumn(item);
                    newDataTable.Columns.Add(dc);
                }

                foreach (DataRow dr in originalDataTable.Rows)
                {
                    string code = dr[0].ToString().Trim();
                    int origNum = int.Parse(dr[1].ToString().Trim());
                    if (origNum <= 100)
                    {
                        Program.logger.LogInfo(string.Format("总交易额度表中证券代码{0}的额度小于100，此数据被过滤掉", code));
                        continue;
                    }
                    int currentNum = 0;
                    if (baseAllCode.Contains(code)) { continue; }
                    //if (code.Equals("600016"))
                    //{
                    //    string str = "1";
                    //    Program.logger.LogInfo(string.Format("在已分配额度表中没有找到证券代码为{0}的记录", code));
                    //}
                    baseAllCode.Add(code);
                    DataRow[] findRows = oldDataTable.Select(string.Format("证券代码='{0}'", code));
                    
                    if (findRows == null || findRows.Length == 0)
                    {
                        Program.logger.LogInfo(string.Format("在已分配额度表中没有找到证券代码为{0}的记录", code));
                        continue;
                    }

                    foreach (var item in findRows)
                    {
                        currentNum += int.Parse(item[8].ToString().Trim());
                    }

                    if (currentNum > origNum)
                    {
                        int resultNum = currentNum;
                        Dictionary<string, int> dict = new Dictionary<string, int>();
                        while (resultNum > origNum)
                        {
                            foreach (var item in findRows)
                            {
                                string key = string.Format("{0},{1},{2}", item[0], item[1], item[2]);
                                //resultNum = int.Parse(item["交易额度"].ToString().Trim()) - 100;
                                if (dict.ContainsKey(key))
                                {
                                    if (dict[key] >= 100)
                                    {
                                        dict[key] = dict[key] - 100;
                                        resultNum = resultNum - 100;
                                    }
                                    else
                                    {
                                        //判断所有Key对应的额度已经全部是0
                                        int num = 0;
                                        foreach (var itemKey in dict.Keys)
                                        {
                                            num += dict[itemKey];
                                        }
                                        if (num == 0)
                                        {
                                            int iNum = origNum/dict.Keys.Count;
                                            int a = 2000 / 3;
                                            int b = a;
                                            String[] keyArr = dict.Keys.ToArray<String>();
                                            for (int i = 0; i < keyArr.Length; i++)
                                            {
                                                dict[keyArr[i]] = iNum;
                                            } 
                                            resultNum = origNum;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (int.Parse(item[8].ToString().Trim()) >= 100)
                                    {
                                        dict.Add(key, int.Parse(item[8].ToString().Trim()) - 100);
                                        resultNum = resultNum - 100;
                                    }
                                    else
                                    {
                                        dict.Add(key, int.Parse(item[8].ToString().Trim()));
                                    }
                                }
                                if (resultNum <= origNum)
                                {
                                    break;
                                }
                            }
                        }

                        foreach (var key in dict.Keys)
                        {
                            var arry = key.Split(',');
                            DataRow[] drs = oldDataTable.Select(string.Format("交易员='{0}' and 证券代码='{1}' and 组合号='{2}'", arry[0], arry[1], arry[2]));
                            foreach (var item in drs)
                            {
                                string strTrader = item[0].ToString().Trim();
                                string strCode = item[1].ToString().Trim().PadLeft(6, '0');
                                string strGroup = item[2].ToString().Trim();
                                string strMatket = item[3].ToString().Trim();
                                string strCodeName = item[4].ToString().Trim();
                                string strPinY = item[5].ToString().Trim();
                                string strBuy = item[6].ToString().Trim();
                                string strSell = item[7].ToString().Trim();
                                string strRate = item[9].ToString().Trim();

                                if (string.IsNullOrEmpty(strMatket))
                                {
                                    strMatket = strCode.StartsWith("6") == true ? "1" : "0";
                                }
                                if (string.IsNullOrEmpty(strCodeName))
                                {
                                    foreach (var stockItem in stockDataTable.Select(string.Format("代码='{0}'", strCode)))
                                    {
                                        strCodeName = stockItem[1].ToString().Trim();
                                    }
                                }
                                if (string.IsNullOrEmpty(strPinY))
                                {
                                    foreach (var stockItem in stockDataTable.Select(string.Format("代码='{0}'", strCode)))
                                    {
                                        strPinY = stockItem[2].ToString().Trim();
                                    }
                                }
                                if (string.IsNullOrEmpty(strBuy))
                                {
                                    strBuy = "现金买入";
                                }
                                if (string.IsNullOrEmpty(strSell))
                                {
                                    strSell = "现券卖出";
                                }

                                if (string.IsNullOrEmpty(strTrader) && string.IsNullOrEmpty(strRate))
                                {
                                    strRate = this.textBox3.Text.Trim();
                                }

                                if (string.IsNullOrEmpty(strRate))
                                {
                                    strRate = this.textBox4.Text.Trim();
                                }

                                DataRow newDr = newDataTable.NewRow();
                                newDataTable.Rows.Add(newDr);

                                newDr[0] = strTrader;
                                newDr[1] = strCode;
                                newDr[2] = strGroup;
                                newDr[3] = strMatket;
                                newDr[4] = strCodeName;
                                newDr[5] = strPinY;
                                newDr[6] = strBuy;
                                newDr[7] = strSell;
                                newDr[8] = dict[key];
                                newDr[9] = strRate;
                            }

                        }

                        if (!changedAllCode.ContainsKey(code))
                        {
                            changedAllCode.Add(code, new List<int> { currentNum, origNum });
                        }

                    }
                    else
                    {
                        foreach (var item in findRows)
                        {
                            string strTrader = item[0].ToString().Trim();
                            string strCode = item[1].ToString().Trim().PadLeft(6, '0');
                            string strGroup = item[2].ToString().Trim();
                            string strMatket = item[3].ToString().Trim();
                            string strCodeName = item[4].ToString().Trim();
                            string strPinY = item[5].ToString().Trim();
                            string strBuy = item[6].ToString().Trim();
                            string strSell = item[7].ToString().Trim();
                            string strNum = item[8].ToString().Trim();
                            string strRate = item[9].ToString().Trim();

                            if (string.IsNullOrEmpty(strMatket))
                            {
                                strMatket = strCode.StartsWith("6") == true ? "1" : "0";
                            }
                            if (string.IsNullOrEmpty(strCodeName))
                            {
                                foreach (var stockItem in stockDataTable.Select(string.Format("代码='{0}'", strCode)))
                                {
                                    strCodeName = stockItem[1].ToString().Trim();
                                }
                            }
                            if (string.IsNullOrEmpty(strPinY))
                            {
                                foreach (var stockItem in stockDataTable.Select(string.Format("代码='{0}'", strCode)))
                                {
                                    strPinY = stockItem[2].ToString().Trim();
                                }
                            }
                            if (string.IsNullOrEmpty(strBuy))
                            {
                                strBuy = "现金买入";
                            }
                            if (string.IsNullOrEmpty(strSell))
                            {
                                strSell = "现券卖出";
                            }

                            if (string.IsNullOrEmpty(strTrader) && string.IsNullOrEmpty(strRate))
                            {
                                strRate = this.textBox3.Text.Trim();
                            }

                            if (string.IsNullOrEmpty(strRate))
                            {
                                strRate = this.textBox4.Text.Trim();
                            }

                            DataRow newDr = newDataTable.NewRow();
                            newDataTable.Rows.Add(newDr);

                            newDr[0] = strTrader;
                            newDr[1] = strCode;
                            newDr[2] = strGroup;
                            newDr[3] = strMatket;
                            newDr[4] = strCodeName;
                            newDr[5] = strPinY;
                            newDr[6] = strBuy;
                            newDr[7] = strSell;
                            newDr[8] = strNum;
                            newDr[9] = strRate;
                        }
                    }


                }

                this.button6.Visible = true;

                Program.logger.LogInfo(string.Format("自动检测已结束,生成{0}条记录", newDataTable.Rows.Count));

                foreach (var strCode in changedAllCode.Keys)
                {
                    //List<int> items = changedAllCode[strCode];
                    //int i = items[0];
                    //int j = items[1];
                    Program.logger.LogInfo(string.Format("已校对{0},总券池额度为{1},已分配额度为{2},校正后额度为{3}", strCode, changedAllCode[strCode][1], changedAllCode[strCode][0], changedAllCode[strCode][1]));
                }

                foreach (DataRow diffRow in oldDataTable.Rows)
                {
                    if(diffRow[1].ToString().Equals("2020"))
                    {
                        string str = "";
                    }
                    if (baseAllCode.Contains(diffRow[1].ToString())) { continue; }

                    //在总券池中没有找到对应的证券代码
                    sbNotCodeInbase.Append(diffRow[1].ToString());
                    sbNotCodeInbase.Append(",");

                }
                if (sbNotCodeInbase.ToString().Length > 0) { Program.logger.LogInfo(string.Format("总交易额度列表中没有找到以下证券代码:{0}", sbNotCodeInbase.ToString().TrimEnd(','))); }
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo("自动检测异常结束,错误信息:{0}", ex.Message);
            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string fileName = string.Format("券单导入_{0}", DateTime.Now.ToString("yyyyMMdd"));
            CSVFileHelper.SaveCSV(newDataTable, fileName);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView listview = (ListView)sender;
            ListViewItem lstrow = listview.GetItemAt(e.X, e.Y);
            System.Windows.Forms.ListViewItem.ListViewSubItem lstcol = lstrow.GetSubItemAt(e.X, e.Y);
            string strText = lstcol.Text;
            try
            {
                Clipboard.SetDataObject(strText);
                //notifyIcon1.Visible = true;
                //string info = string.Format("内容【{0}】已经复制到剪贴板", strText);
                //notifyIcon1.ShowBalloonTip(1500, "提示", info, ToolTipIcon.Info);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }

    }
}
