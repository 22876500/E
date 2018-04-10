using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using AASClient.AASServiceReference;

namespace AASClient.StockPosition
{
    public partial class PositionImport : Form
    {
        DataTable ImportData;

        public PositionImport()
        {
            InitializeComponent();
            dataGridViewImportData.AutoGenerateColumns = false;
        }

        private void buttonPositionImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Multiselect = false,
                CheckFileExists = true,
                Filter = "All Excel|*.xls;*.xlsx|Excel 97~2003|*.xls|Excel 2007/2010|*.xlsx",
            };
            DialogResult Result = fileDialog.ShowDialog(this);
            if (Result == DialogResult.OK || Result == DialogResult.Yes)
            {
                try
                {
                    if (ExcelUtils.IsExcel(fileDialog.FileName))
                    {
                        int headerIndex = (int)numericUpDownHeaderRow.Value - 1;
                        int contentIndex = (int)numericUpDownExcelSheet.Value - 1;

                        DataTable dt = ExcelUtils.ReadExcel(fileDialog.FileName, headerIndex, contentIndex);
                        if (dt != null)
                        {
                            if (dt.Columns.Contains("证券代码"))
                            {
                                foreach (DataRow dataRow in dt.Rows)
                                {
                                    dataRow["证券代码"] = UpdateStockCode(dataRow["证券代码"].ToString());
                                }
                            }
                            ImportData = dt;

                            for (int i = dt.Rows.Count -1; i > 0; i--)
                            {
                                var row = dt.Rows[i];
                                if ((row[0] + "").Length == 0 || (row[2] +"").Length == 0 || (row[2] + "").Length == 0)
                                {
                                    dt.Rows.RemoveAt(i);
                                }
                        }
                            this.dataGridViewImportData.DataSource = dt.DefaultView;
                        }
                        else
                        {
                            MessageBox.Show(string.Format("导入数据为空，请检查导入文件的第{0}个Sheet页是否包含数据", numericUpDownExcelSheet.Value));
                        }
                    }
                    else
                    {
                        MessageBox.Show("文件检测异常，并非正常的excel文件!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("导入文件异常! 错误信息 {0}", ex.Message));
                }
            }

        }

        private string UpdateStockCode(string v)
        {
            if (v.Length < 6)
            {
                for (int i = v.Length; i < 6; i++)
                {
                    v = "0" + v;
                }
            }
            return v;
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            //验证。。。如何验证？考虑首先验证是否有冲突项，如果有，提示是否需要覆盖保存，是，否，或者取消。
            if (ImportData != null && ImportData.Rows.Count > 0)
            {
                //首先验证列是否正常。
                if (!IsColumnValid(ImportData))
                {
                    return;
                }

                //其次确认是否需要删除旧数据
                if (checkBox删除旧数据.Checked == true)
                {
                    Program.AASServiceClient.DeleteAll可用仓位();
                }

                //再次验证是否存在冲突数据
                AASServiceReference.DbDataSet.可用仓位DataTable dtOld = Program.AASServiceClient.QueryTotalPosition();
                if (HasExistData(dtOld, ImportData))
                {
                    var result = MessageBox.Show("存在组合号及证券代码相同的旧数据，覆盖，不覆盖，还是取消操作?", "导入提示", MessageBoxButtons.YesNoCancel);
                    if (result != DialogResult.Cancel)
                    {
                        AddMutiPosition(ImportData, result == DialogResult.Yes);
                    }
                }
                else
                {
                    AddMutiPosition(ImportData, false);
                }
                this.Close();
            }
            

        }

        private bool HasExistData(AASServiceReference.DbDataSet.可用仓位DataTable dtOld, DataTable importData)
        {
            foreach (DataRow row in importData.Rows)
            {
                string name = row["组合号"].ToString();
                string stockCode = row["证券代码"].ToString();
                foreach (var item in dtOld)
                {
                    if (item.组合号 == name && item.证券代码 == stockCode)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsColumnValid(DataTable importData)
        {
            if (!importData.Columns.Contains("组合号"))
            {
                MessageBox.Show("缺少 组合号 列!");
                return false;
            }
            else if (!importData.Columns.Contains("证券代码"))
            {
                MessageBox.Show("缺少 证券代码 列!");
                return false;
            }
            else if (!importData.Columns.Contains("证券名称"))
            {
                MessageBox.Show("缺少 证券名称 列!");
                return false;
            }
            else if (!importData.Columns.Contains("总仓位"))
            {
                MessageBox.Show("缺少 总仓位 列!");
                return false;
            }
            return true;
        }

        private void AddMutiPosition(DataTable dt, bool isCoverExist)
        {
            Task.Run(() =>
            {
                try
                {
                    AASServiceReference.DbDataSet.可用仓位DataTable fixedData = ChangeToStandardDataTable(dt);
                    Program.AASServiceClient.AddMuti可用仓位(fixedData, isCoverExist);
                    this.Invoke(new Action(() => { MessageBox.Show(string.Format("保存成功!")); }));
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() => { MessageBox.Show(string.Format("保存异常!", ex.Message)); }));
                }
            });
        }

        private DbDataSet.可用仓位DataTable ChangeToStandardDataTable(DataTable dt)
        {
            var standardDT = new AASServiceReference.DbDataSet.可用仓位DataTable();
            foreach (DataRow item in dt.Rows)
            {
                var newRow = standardDT.New可用仓位Row();
                newRow.组合号 = item["组合号"].ToString();
                newRow.证券代码 = item["证券代码"].ToString();
                newRow.证券名称 = item["证券名称"].ToString();
                newRow.总仓位 = decimal.Parse(item["总仓位"].ToString());
                standardDT.Add可用仓位Row(newRow);
            }
            return standardDT;
        }
    }
}
