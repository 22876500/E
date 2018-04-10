using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    public partial class TradeDetailForm : Form
    {
        public TradeDetailForm()
        {
            InitializeComponent();
            this.Load += TradeDetailForm_Load;
        }

        private void TradeDetailForm_Load(object sender, EventArgs e)
        {
            //this.bindingSource当前成交.DataSource = Program.jyDataSet;
            //this.bindingSource当前成交.DataMember = "成交";
            dataGridView当前成交.AutoGenerateColumns = false;
            //dataGridView当前成交.Columns.Add("交易员", "交易员");
            //dataGridView当前成交.Columns.Add("组合号", "组合号");
            //dataGridView当前成交.Columns.Add("证券代码", "证券代码");
            //dataGridView当前成交.Columns.Add("证券名称");
            //dataGridView当前成交.Columns.Add("买卖方向");
            //dataGridView当前成交.Columns.Add("成交价格");
            //dataGridView当前成交.Columns.Add("成交数量");
            //dataGridView当前成交.Columns.Add("成交金额");
            //dataGridView当前成交.Columns.Add("成交时间");
            //dataGridView当前成交.Columns.Add("成交编号");
            //dataGridView当前成交.Columns.Add("委托编号");

            try
            {
                if (Program.jyDataSet != null && Program.jyDataSet.成交 != null || Program.jyDataSet.成交.Count > 0)
                {
                    numericUpDownPageTotal.Value = Math.Ceiling((decimal)Program.jyDataSet.成交.Count / 20);
                    numericUpDownPageIndex.Maximum = numericUpDownPageTotal.Value;
                    this.bindingSource当前成交.DataSource = Program.jyDataSet.成交.Take(20);
                }

                numericUpDownPageIndex.Value = 1;
                numericUpDownPageSize.Value = 20;
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo(ex.Message);
            }

            this.dataGridView当前成交.DataSource = this.bindingSource当前成交;
            this.dataGridView当前成交.Columns["市场代码"].Visible = false;
            this.dataGridView当前成交.Columns["成交时间"].DisplayIndex = 8;
            this.dataGridView当前成交.Columns["成交数量"].DefaultCellStyle.Format = "f0";
        }

        private void dataGridView当前成交_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                var bsFlag = this.dataGridView当前成交["买卖方向", e.RowIndex].Value + "";
                
                if (bsFlag == "0")
                {
                    this.dataGridView当前成交.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                }
                else if (bsFlag == "1")
                {
                    this.dataGridView当前成交.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                }
                else
                {
                    //this.dataGridView当前成交.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        private void numericUpDownPageSize_ValueChanged(object sender, EventArgs e)
        {
            BindingChange();
        }

        private void numericUpDownPageIndex_ValueChanged(object sender, EventArgs e)
        {
            BindingChange();
        }

        private void BindingChange()
        {
            int pageIndex = (int)numericUpDownPageIndex.Value;
            int pageSize = (int)numericUpDownPageSize.Value;

            try
            {
                if (Program.jyDataSet != null && Program.jyDataSet.成交 != null || Program.jyDataSet.成交.Count > 0)
                {
                    if (Program.jyDataSet.成交.Count > pageSize)
                    {
                        this.bindingSource当前成交.DataSource = Program.jyDataSet.成交.Take(pageSize * pageIndex).Skip((pageIndex - 1) * pageSize);
                    }
                    else
                    {
                        this.bindingSource当前成交.DataSource = Program.jyDataSet.成交.Copy();
                    }
                    numericUpDownPageTotal.Value = Math.Ceiling((decimal)Program.jyDataSet.成交.Count / 20);
                    numericUpDownPageIndex.Maximum = numericUpDownPageTotal.Value;
                    numericUpDownPageIndex.Minimum = 1;
                }
            }
            catch (Exception)
            {
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog() { Filter = "Excel 97~2003|*.xls" };
            if (s.ShowDialog() ==  DialogResult.OK && !string.IsNullOrEmpty(s.FileName))
            {
                using (var stream = File.OpenWrite(s.FileName))
                {
                    var workbook = ExcelUtils.RenderToExcel(Program.jyDataSet.成交);
                    workbook.Write(stream);
                }
            }
        }

        private void dataGridView当前成交_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Program.logger.LogInfo("dataGridView当前成交_DataError: " + e.Exception.Message);
            e.ThrowException = false;
            e.Cancel = true;
        }
    }
}
