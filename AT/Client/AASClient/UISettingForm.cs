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
    public partial class UISettingForm : Form
    {
        FontDialog fontDialog1 = null;
        ColorDialog colorDialog1 = null;
        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = null;
        public UISettingForm()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            fontDialog1 = new FontDialog();
            colorDialog1 = new ColorDialog();
            binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            
            List<UISettingEntity> lstSetting = new List<UISettingEntity>();
            
            List<UISettingEntity> lstBuyArr = new List<UISettingEntity>();
            List<UISettingEntity> lstSaleArr = new List<UISettingEntity>();

            var strBuyFont = Program.accountDataSet.参数.GetParaValue("买盘字体", string.Empty);
            var strSaleFont = Program.accountDataSet.参数.GetParaValue("卖盘字体", string.Empty);
            var strTradeFont = Program.accountDataSet.参数.GetParaValue("字体", string.Empty);

            lstSetting.Add(new UISettingEntity() { Setting = "买盘字体", SettingName = "买盘字体", SettingValue = strBuyFont });
            lstSetting.Add(new UISettingEntity() { Setting = "卖盘字体", SettingName = "卖盘字体", SettingValue = strSaleFont });
            lstSetting.Add(new UISettingEntity() { Setting = "逐笔成交字体", SettingName = "字体", SettingValue = strTradeFont });
            for (int i = 0; i < 10; i++)
            {
                string strBuyName = string.Format("买{0}颜色", i);
                string strSaleName = string.Format("卖{0}颜色", i);
                string strBuyValue = Program.accountDataSet.参数.GetParaValue(strBuyName, string.Empty);
                string strSaleValue = Program.accountDataSet.参数.GetParaValue(strSaleName, string.Empty);
                lstBuyArr.Add(new UISettingEntity() { Setting = string.Format("买{0}颜色", i + 1), SettingName = strBuyName, SettingValue = strBuyValue });
                lstSaleArr.Add(new UISettingEntity() { Setting = string.Format("卖{0}颜色", i + 1), SettingName = strSaleName, SettingValue = strSaleValue });
            }
            lstSetting.AddRange(lstBuyArr);
            lstSetting.AddRange(lstSaleArr);

            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.DataSource = lstSetting;
            this.dataGridView1.CellClick += dataGridView1_CellClick;
        }

        void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "SetCol")
            {
                var data = (dataGridView1.DataSource as List<UISettingEntity>)[e.RowIndex];
                switch (data.SettingType)
                {
                    case "字体":
                        SetFont(data);
                        break;
                    case "颜色":
                        SetColor(data);
                        break;
                    default:
                        break;
                }
            }
        }

        private void SetColor(UISettingEntity data)
        {
            DialogResult DialogResult1 = this.colorDialog1.ShowDialog();
            if (DialogResult1 != DialogResult.OK)
            {
                return;
            }

            Program.accountDataSet.参数.SetParaValue(data.SettingName, this.colorDialog1.Color.ToArgb().ToString());
        }

        private void SetFont(UISettingEntity data)
        {
            DialogResult DialogResult1 = this.fontDialog1.ShowDialog();
            if (DialogResult1 != DialogResult.OK)
            {
                return;
            }

            using (System.IO.MemoryStream MemoryStream1 = new System.IO.MemoryStream())
            {
                this.binaryFormatter.Serialize(MemoryStream1, this.fontDialog1.Font);
                Program.accountDataSet.参数.SetParaValue(data.SettingName, Convert.ToBase64String(MemoryStream1.ToArray()));
            }
        }

        class UISettingEntity
        {
            static Regex reg = new Regex("颜色|字体");
            public string Setting { get; set; }

            public string UIName { get { return reg.Replace(Setting, ""); } }

            public string SettingName { get; set; }

            public string SettingType { get { return reg.Match(Setting).Value; } }

            public string SettingValue { get; set; }
        }
    }
}
