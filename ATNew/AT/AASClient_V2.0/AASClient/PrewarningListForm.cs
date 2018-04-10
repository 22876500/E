using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    public partial class PrewarningListForm : Form
    {
        public PrewarningListForm()
        {
            InitializeComponent();
            this.Load += RemindSettingForm_Load;
        }

        #region Events
        void RemindSettingForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.bindingSourceSettings.DataSource = Program.WarningFormulas;
                this.dataGridViewSettings.DataSource = this.bindingSourceSettings;
                foreach (DataGridViewColumn col in dataGridViewSettings.Columns)
                {
                    if (col.Name == "计算方法" || col.Name == "预警级别")
                    {
                        col.Visible = true;
                    }
                    else
                    {
                        col.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = this.bindingSourceSettings.Current as Model.WarningFormulaOne;

            Program.WarningFormulas.Remove(item);

            var strPreWarning = Cryptor.MD5Encrypt(Program.WarningFormulas.ToJSON());
            Program.accountDataSet.参数.SetParaValue("预警列表", strPreWarning);

            this.bindingSourceSettings.DataSource = Program.WarningFormulas.ToList<Model.IWarningFormula>();
            if (Program.fmPreWarnings != null)
            {
                for (int i = 0; i < Program.fmPreWarnings.Count; i++)
                {
                    var fm = Program.fmPreWarnings[i];
                    if (fm != null && fm.Formula.ID == item.ID && !fm.IsDisposed)
                    {
                        Program.fmPreWarnings.Remove(fm);
                        fm.Close();
                    }
                }
            }
        }

        private void 新增ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new PrewarningAddForm();
            frm.ShowDialog();

            this.bindingSourceSettings.DataSource = Program.WarningFormulas.ToList<Model.IWarningFormula>();
        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormulaEdit();
        }

        private void dataGridViewSettings_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                FormulaEdit();
            }
        }
        #endregion

        private void FormulaEdit()
        {
            var item = this.bindingSourceSettings.Current as Model.WarningFormulaOne;

            var frm = new PrewarningAddForm(item);
            frm.ShowDialog();
            this.bindingSourceSettings.DataSource = Program.WarningFormulas.ToList<Model.IWarningFormula>();
        }

        
    }
}
