using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    public partial class PrewarningAddForm : Form
    {
        string numbersZhCn = "一二三四五六七八九十";
        private Model.WarningFormulaOne Formula;
        public PrewarningAddForm()
        {
            InitializeComponent();
            this.Text = "预警公式新增";
        }

        public PrewarningAddForm(Model.WarningFormulaOne formula)
        {
            InitializeComponent();
            this.Text = "预警公式编辑";
            Formula = formula as Model.WarningFormulaOne;
        }

        void PrewarningAddForm_Load(object sender, EventArgs e)
        {
            if (this.Formula == null)
            {
                Init();
            }
            else
            {
                Init(Formula);
            }
        }

        private void Init()
        {
            var values = Enum.GetValues(typeof(Model.CompareType));
            foreach (var item in values)
            {
                this.comboBoxCompare.Items.Add(CommonUtils.CompareString((Model.CompareType)item));
            }
            this.comboBoxCompare.SelectedIndex = 0;

            string[] prices = new string[20];
            for (int i = 0; i < numbersZhCn.Length; i++)
            {
                prices[i] = string.Format("买{0}价", numbersZhCn[i]);
                prices[i + 10] = string.Format("卖{0}价", numbersZhCn[i]);
            }

            foreach (var item in prices)
            {
                this.comboBox1.Items.Add(item);
                this.comboBox2.Items.Add(item);
                this.comboBox3.Items.Add(item);
            }
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 10;
            this.comboBox3.SelectedIndex = 10;

            this.comboBoxCalType.DataSource = new char[] { '+', '-', '*', '/' };
            this.comboBoxCalType.SelectedIndex = 2;

            this.comboBoxLevel.DataSource = new string[] { "黄色预警", "红色预警" };
            this.comboBoxLevel.SelectedIndex = 0;

            this.comboBoxCodesType.DataSource = new string[] { "全订阅", "自定义" };
            comboBoxCodesType.SelectedIndex = 0;

            this.numericUpDownFrequency.Value = 60;
            //this.numericUpDownParam.Value = (decimal)0.01;
        }

        private void Init(Model.WarningFormulaOne wfo)
        {
            var values = Enum.GetValues(typeof(Model.CompareType));
            foreach (var item in values)
            {
                this.comboBoxCompare.Items.Add(CommonUtils.CompareString((Model.CompareType)item));
            }
            this.comboBoxCompare.SelectedIndex = (int)wfo.CompareTypeInfo;

            string[] prices = new string[20];
            for (int i = 0; i < numbersZhCn.Length; i++)
            {
                prices[i] = string.Format("买{0}价", numbersZhCn[i]);
                prices[i + 10] = string.Format("卖{0}价", numbersZhCn[i]);
            }

            foreach (var item in prices)
            {
                this.comboBox1.Items.Add(item);
                this.comboBox2.Items.Add(item);
                this.comboBox3.Items.Add(item);
            }
            this.comboBox1.SelectedIndex = wfo.FirstSetting.Index + wfo.FirstSetting.Direction * 10;
            this.comboBox2.SelectedIndex = wfo.SecondSetting.Index + wfo.SecondSetting.Direction * 10;
            this.comboBox3.SelectedIndex = wfo.ThirdSetting.Index + wfo.ThirdSetting.Direction * 10;

            this.comboBoxCalType.DataSource = new char[] { '+', '-', '*', '/' };
            this.comboBoxCalType.SelectedIndex = (int)wfo.ParamCalType;

            this.comboBoxLevel.DataSource = new string[] { "黄色预警", "红色预警" };
            this.comboBoxLevel.SelectedIndex = (int)wfo.Level;

            this.comboBoxCodesType.DataSource = new string[] { "全订阅", "自定义" }; ;
            if (wfo.IsSubAll)
            {
                this.comboBoxCodesType.SelectedIndex = 0;
                this.rtbCodes.ReadOnly = true;
            }
            else
            {
                this.comboBoxCodesType.SelectedIndex = 1;
                if (wfo.CodeList != null)
                    this.rtbCodes.Text = string.Join(",", wfo.CodeList);
            }
            this.numericUpDownBig.Value = wfo.LargeVolume;
            this.numericUpDownParam.Value = wfo.Param;
            this.numericUpDownFrequency.Value = wfo.Frequency; //预警显示频率
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Model.WarningFormulaOne entity = this.Formula ?? new Model.WarningFormulaOne() { ID = Guid.NewGuid().ToString() };

            if (comboBoxCodesType.SelectedIndex != 0)
            {
                entity.IsSubAll = false;
                List<string> codes = GetCodes();
                if (codes.Count < 1)
                {
                    rtbCodes.Focus();
                    MessageBox.Show("预警列表不能为空。");
                    return;
                }

                entity.CodeList = codes;
            }
            else
            {
                entity.IsSubAll = true;
                entity.CodeList = new List<string>();
            }

            var s1 = comboBox1.SelectedItem.ToString();
            entity.FirstSetting = new Model.BidAskItem() { Index = GetIndex(s1), Direction = GetDerection(s1) };

            var s2 = comboBox2.SelectedItem.ToString();
            entity.SecondSetting = new Model.BidAskItem() { Index = GetIndex(s2), Direction = GetDerection(s2) };

            var s3 = comboBox3.SelectedItem.ToString();
            entity.ThirdSetting = new Model.BidAskItem() { Index = GetIndex(s3), Direction = GetDerection(s3) };

            entity.Param = this.numericUpDownParam.Value;

            entity.CompareTypeInfo = (Model.CompareType)comboBoxCompare.SelectedIndex;

            entity.Level = (Model.WarningLevel)comboBoxLevel.SelectedIndex;

            entity.LargeVolume = this.numericUpDownBig.Value;

            entity.ParamCalType = (Model.CalculateType)comboBoxCalType.SelectedIndex;

            entity.Frequency = this.numericUpDownFrequency.Value;

            if (!Program.WarningFormulas.Contains(entity))
            {
                Program.WarningFormulas.Add(entity);
            }

            var strPreWarning = Cryptor.MD5Encrypt(Program.WarningFormulas.ToJSON());
            Program.accountDataSet.参数.SetParaValue("预警列表",strPreWarning);
            if (this.Formula == null)
            {
                var fm = new PrewarningShowForm(entity);
                fm.Show();

                if (Program.fmPreWarnings == null)
                {
                    Program.fmPreWarnings = new List<PrewarningShowForm>();
                }
                Program.fmPreWarnings.Add(fm);
                
            }
            this.Close();
        }

        private List<string> GetCodes()
        {
            List<string> codes = new List<string>();
            var strArr = Regex.Split(rtbCodes.Text, "[^0-9]+");
            foreach (var item in strArr)
            {
                if (CommonUtils.IsCode(item))
                {
                    codes.Add(item);
                }
            }
            return codes;
        }

        int GetIndex(string s)
        {
            int index = -1;
            for (int i = 0; i < numbersZhCn.Length; i++)
            {
                if (s.IndexOf(numbersZhCn[i]) > -1)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        byte GetDerection(string s)
        {
            return s.IndexOf("买") > -1 ? (byte)0 : (byte)1;
        }

        private void comboBoxCodesType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxCodesType != null)
            {
                this.rtbCodes.ReadOnly = comboBoxCodesType.SelectedIndex == 0;
            }
        }
    }
}
