using Binance.API.Csharp.Client.Models.WebSocket;
using DataModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    public partial class HqForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private static object sync = new object();
        private static readonly DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
        const int BidRowCount = 20;

        //decimal 证券仓位 = 0;
        decimal 可卖股数 = 0;
        decimal 可买股数 = 0;
        int index;
        string Last委托编号;
        DateTime lastQuickKeyTime = DateTime.MinValue;
        TradeMainForm tradeMainForm;
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        public string Zqdm
        {
            get
            {
                return Program.accountDataSet.参数.GetParaValue("证券代码" + this.index.ToString(), "ETHBTC");
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        public HqForm(int Index, TradeMainForm TradeMainForm1)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲

            this.index = Index;
            this.tradeMainForm = TradeMainForm1;

            string ParaName1 = string.Format("{0} splitContainer1 SplitterDistance", this.index);
            this.splitContainer1.SplitterDistance = int.Parse(Program.accountDataSet.参数.GetParaValue(ParaName1, "300"));

            string ParaName2 = string.Format("{0} splitContainer2 SplitterDistance", this.index);
            this.splitContainer2.SplitterDistance = int.Parse(Program.accountDataSet.参数.GetParaValue(ParaName2, "149"));
            
            for (int i = 0; i < BidRowCount; i++)
            {
                this.listView买盘.Items.Add(new ListViewItem(new string[] { "        ", "        ", "        " }));
                this.listView卖盘.Items.Add(new ListViewItem(new string[] { "        ", "        ", "        " }));
            }

            for (int i = 0; i < 50; i++)
            {
                this.listView逐笔成交.Items.Add(new ListViewItem(new string[] { "        ", "        ", "        " }));
            }
            
            for (int i = 0; i < 3; i++)
            {
                string ParaName = string.Format("{0}买盘列{1}宽度", this.index, i);
                this.listView买盘.Columns[i].Width = int.Parse(Program.accountDataSet.参数.GetParaValue(ParaName, "60"));

                ParaName = string.Format("{0}卖盘列{1}宽度", this.index, i);
                this.listView卖盘.Columns[i].Width = int.Parse(Program.accountDataSet.参数.GetParaValue(ParaName, "60"));

                ParaName = string.Format("{0}逐笔成交列{1}宽度", this.index, i);
                this.listView逐笔成交.Columns[i].Width = int.Parse(Program.accountDataSet.参数.GetParaValue(ParaName, "60"));
            }
            
            this.comboBox代码.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.comboBox代码.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            SubData();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0014) // 禁掉清除背景消息
                return;
            base.WndProc(ref m);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < this.listView买盘.Items.Count; i++)
            {
                ListViewItem ListViewItem1 = this.listView买盘.Items[i];
                ListViewItem ListViewItem2 = this.listView卖盘.Items[i];

                string ParaName = string.Format("买{0}颜色", i);
                int ColorValue = int.Parse(Program.accountDataSet.参数.GetParaValue(ParaName, Color.White.ToArgb().ToString()));
                for (int j = 0; j < ListViewItem1.SubItems.Count; j++)
                {
                    if (ListViewItem1.SubItems[j].BackColor != Color.FromArgb(ColorValue))
                    {
                        ListViewItem1.SubItems[j].BackColor = Color.FromArgb(ColorValue);
                    }
                }

                ParaName = string.Format("卖{0}颜色", i);
                ColorValue = int.Parse(Program.accountDataSet.参数.GetParaValue(ParaName, Color.White.ToArgb().ToString()));
                for (int j = 0; j < ListViewItem2.SubItems.Count; j++)
                {
                    if (ListViewItem2.SubItems[j].BackColor != Color.FromArgb(ColorValue))
                    {
                        ListViewItem2.SubItems[j].BackColor = Color.FromArgb(ColorValue);
                    }
                }
            }

            this.linkLabel默认股数.Text = string.Format("Shares: {0} MaxAmt: {1}", Program.accountDataSet.参数.GetParaValue(this.Zqdm + "默认股数", "0"), Program.accountDataSet.参数.GetParaValue(this.Zqdm + "最大金额", "0"));


            Font 卖盘Font = GetFont("卖盘字体");
            Font 买盘Font = GetFont("买盘字体");
            Font 逐笔成交Font = GetFont("字体");

            if (买盘Font != null && this.listView买盘.Font != 买盘Font)
            {
                listView买盘.Font = 买盘Font;
            }

            if (卖盘Font != null && listView卖盘.Font != 卖盘Font)
            {
                listView卖盘.Font = 卖盘Font;
            }

            if (逐笔成交Font != null && listView逐笔成交.Font != 逐笔成交Font)
            {
                listView逐笔成交.Font = 逐笔成交Font;
            }

            var kv = MarketAdapter.BinanceUtils.GetBinanceCoinInfo(Zqdm);
            if (tradeMainForm.dictAccountCoin.ContainsKey(kv.Key))
            {
                label可卖股数.Text = MarketAdapter.BinanceUtils.GetShortStr(tradeMainForm.dictAccountCoin[kv.Key].Freek__BackingField);
            }
            if (tradeMainForm.dictAccountCoin.ContainsKey(kv.Value))
            {
                label可用资金.Text = MarketAdapter.BinanceUtils.GetShortStr(tradeMainForm.dictAccountCoin[kv.Value].Freek__BackingField);
            }
            if (tradeMainForm.dictAccountCoin.Count > 0)
            {
                labelBNB.Text = MarketAdapter.BinanceUtils.GetShortStr(tradeMainForm.dictAccountCoin["BNB"].Freek__BackingField);
                labelBTC.Text = MarketAdapter.BinanceUtils.GetShortStr(tradeMainForm.dictAccountCoin["BTC"].Freek__BackingField);
                labelETH.Text = MarketAdapter.BinanceUtils.GetShortStr(tradeMainForm.dictAccountCoin["ETH"].Freek__BackingField);
                labelUSDT.Text = MarketAdapter.BinanceUtils.GetShortStr(tradeMainForm.dictAccountCoin["USDT"].Freek__BackingField);
            }
        }

        private Font GetFont(string fontConfigName)
        {
            Font font = null;
            var 字体Base64String = Program.accountDataSet.参数.GetParaValue(fontConfigName, null);
            if (字体Base64String != null)
            {
                using (MemoryStream MemoryStream1 = new MemoryStream(Convert.FromBase64String(字体Base64String)))
                {
                    font = this.binaryFormatter.Deserialize(MemoryStream1) as Font;
                }
            }
            return font;
        }

        #region 行情数据
        private void Subscribe()
        {
            var text = this.comboBox代码.Text.ToUpper();
            if (text.Length >= 5 &&
                (text.EndsWith("BNB", StringComparison.CurrentCultureIgnoreCase)
                || text.EndsWith("BTC", StringComparison.CurrentCultureIgnoreCase)
                || text.EndsWith("ETH", StringComparison.CurrentCultureIgnoreCase)
                || text.EndsWith("USDT", StringComparison.CurrentCultureIgnoreCase))
                && Regex.IsMatch(text, "[a-zA-Z]")
                )
            {
                this.comboBox代码.Text = text;
                Program.accountDataSet.参数.SetParaValue("证券代码" + this.index.ToString(), text);
                CompletionByWhiteSpace(0);
                MarketAdapter.BinanceAdapter.Instance.Subscribe(text.ToLower());
            }
        }

        private void SubData()
        {
            //后期应加入：根据选择的交易站点，选择绑定对应的对象
            MarketAdapter.BinanceAdapter.Instance.MCallBack += DepthCallBack;
            MarketAdapter.BinanceAdapter.Instance.TCallBack += TradeCallBack;
        }
        
        private void DepthCallBack(string symble, PartialDepthMessage message)
        {
            if (this.Zqdm.Equals(symble, StringComparison.CurrentCultureIgnoreCase))
            {
                int priceRound, qtyRound, notionRound;
                MarketAdapter.BinanceAdapter.Instance.GetRoundNum(symble, out priceRound, out qtyRound, out notionRound);
                this.BeginInvoke(new Action(() => {
                    int i = 0;
                    foreach (var bidItem in message.Bids)
                    {
                        if (i < listView买盘.Items.Count)
                        {
                            
                            ListViewItem bidViewItem = listView买盘.Items[i++];
                            bidViewItem.SubItems[0].Text = Math.Round(bidItem.Price, priceRound).ToString();
                            bidViewItem.SubItems[1].Text = Math.Round(bidItem.Quantity, qtyRound).ToString();
                            bidViewItem.SubItems[2].Text = Math.Round(bidItem.Quantity * bidItem.Price, notionRound).ToString();
                        }
                    }

                    i = 0;
                    foreach (var item in message.Asks)
                    {
                        if (i < listView卖盘.Items.Count)
                        {
                            ListViewItem askViewItem = listView卖盘.Items[i++];
                            askViewItem.SubItems[0].Text = Math.Round(item.Price, priceRound) + "";
                            askViewItem.SubItems[1].Text = Math.Round(item.Quantity, qtyRound) + "";
                            askViewItem.SubItems[2].Text = Math.Round(item.Quantity * item.Price, notionRound) + "";
                        }
                    }
                    var qtyMin = MarketAdapter.BinanceAdapter.Instance.GetMinQty(Zqdm);
                    label可买股数.Text = Math.Round(decimal.Parse(label可用资金.Text) / message.Asks.First().Price, MarketAdapter.BinanceUtils.GetDigit(qtyMin)) + "";
                }));
            }
        }
        
        private void TradeCallBack(string symble, AggregateTradeMessage message)
        {
            if (symble == this.Zqdm.ToUpper())
            {
                int priceRound, qtyRound, notionRound;
                MarketAdapter.BinanceAdapter.Instance.GetRoundNum(symble, out priceRound, out qtyRound, out notionRound);
                this.Invoke(new Action(()=> {
                    this.listView逐笔成交.Items.RemoveAt(listView逐笔成交.Items.Count - 1);
                    ListViewItem listViewItemAdded = new ListViewItem(new string[] { Math.Round(message.Price, priceRound) + "", Math.Round(message.Quantity, qtyRound) + "", dateStart.AddMilliseconds(message.TradeTime).ToString("HH:mm:ss") });
                    for (int i = 0; i < listViewItemAdded.SubItems.Count; i++)
                    {
                        listViewItemAdded.SubItems[i].ForeColor = message.BuyerIsMaker ? Color.Red : Color.Green;
                    }
                    this.listView逐笔成交.Items.Insert(0, listViewItemAdded);
                }));
            }
        }
        #endregion

        #region Event 买盘
        private void listView买盘_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView买盘_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView买盘_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            string ParaName = string.Format("{0}买盘列{1}宽度", this.index, e.ColumnIndex);
            Program.accountDataSet.参数.SetParaValue(ParaName, this.listView买盘.Columns[e.ColumnIndex].Width.ToString());
        }
        #endregion

        #region Event 卖盘
        private void listView卖盘_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView卖盘_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView卖盘_DoubleClick(object sender, EventArgs e)
        {
            if (this.listView卖盘.SelectedItems.Count > 0)
            {
                this.colorDialog1.Color = this.listView卖盘.SelectedItems[0].SubItems[0].BackColor;
                DialogResult DialogResult1 = this.colorDialog1.ShowDialog();
                if (DialogResult1 != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
                for (int i = 0; i < this.listView卖盘.SelectedItems[0].SubItems.Count; i++)
                {
                    this.listView卖盘.SelectedItems[0].SubItems[i].BackColor = this.colorDialog1.Color;
                }
                string ParaName = string.Format("卖{0}颜色", this.listView卖盘.SelectedItems[0].Index);
                Program.accountDataSet.参数.SetParaValue(ParaName, this.colorDialog1.Color.ToArgb().ToString());
            }
        }

        private void listView卖盘_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            string ParaName = string.Format("{0}卖盘列{1}宽度", this.index, e.ColumnIndex);
            Program.accountDataSet.参数.SetParaValue(ParaName, this.listView卖盘.Columns[e.ColumnIndex].Width.ToString());
        }
        #endregion

        #region Event 逐笔成交
        private void listView逐笔成交_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView逐笔成交_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView逐笔成交_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            string ParaName = string.Format("{0}逐笔成交列{1}宽度", this.index, e.ColumnIndex);
            Program.accountDataSet.参数.SetParaValue(ParaName, this.listView逐笔成交.Columns[e.ColumnIndex].Width.ToString());
        }

        private void listView逐笔成交_DoubleClick(object sender, EventArgs e)
        {
            this.fontDialog1.Font = this.listView逐笔成交.Font;
            DialogResult DialogResult1 = this.fontDialog1.ShowDialog();
            if (DialogResult1 != DialogResult.OK)
            {
                return;
            }
            using (MemoryStream MemoryStream1 = new MemoryStream())
            {
                this.binaryFormatter.Serialize(MemoryStream1, this.fontDialog1.Font);
                Program.accountDataSet.参数.SetParaValue("字体", Convert.ToBase64String(MemoryStream1.ToArray()));
            }
        }
        #endregion

        #region Event HqForm
        private void HqForm_Load(object sender, EventArgs e)
        {
            this.comboBox代码.Text = this.Zqdm;
        }

        private void HqForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }


        private void HqForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && this.panel3.Visible || (DateTime.Now - lastQuickKeyTime).TotalSeconds < 3)
            {
                panel3.Visible = false;
                return;
            }

            AASClient.AccountDataSet.快捷键Row 快捷键Row1 = Program.accountDataSet.快捷键.FirstOrDefault(r => r.键名 == e.KeyCode.ToString());
            if (快捷键Row1 == null)
            {
                return;
            }


            交易方向 交易方向1 = (交易方向)快捷键Row1.方向;



            if (交易方向1 == 交易方向.修改默认)
            {
                var SetDefaultQuantityForm1 = new SetDefaultQuantityForm(this.Zqdm);
                SetDefaultQuantityForm1.ShowDialog();
            }
            else if (交易方向1 == 交易方向.取消)
            {
                this.panel3.Visible = false;
            }
            else if (交易方向1 == 交易方向.撤单)
            {
                AASClient.AASServiceReference.JyDataSet.委托Row DataRow1 = Program.jyDataSet.委托.FirstOrDefault(r => r.委托编号 == this.Last委托编号);
                if (DataRow1.证券代码 == this.Zqdm)
                {
                    string Ret = Program.AASServiceClient.CancelOrder(Program.Current平台用户.用户名, DataRow1.组合号, DataRow1.证券代码, DataRow1.证券名称, DataRow1.市场代码, DataRow1.委托编号, DataRow1.买卖方向, DataRow1.委托数量, DataRow1.委托价格);
                    string[] Data = Ret.Split('|');
                    if (Data[1] != string.Empty)
                    {
                        Program.logger.LogJy(Program.Current平台用户.用户名, DataRow1.证券代码, DataRow1.证券名称, DataRow1.委托编号, DataRow1.买卖方向, DataRow1.委托数量, DataRow1.委托价格, "撤单失败: {0}", Data[1]);
                    }
                    else
                    {
                        Program.logger.LogJy(Program.Current平台用户.用户名, DataRow1.证券代码, DataRow1.证券名称, DataRow1.委托编号, DataRow1.买卖方向, DataRow1.委托数量, DataRow1.委托价格, "撤单成功");
                    }
                }
            }
            else if (交易方向1 == 交易方向.全撤)
            {
                foreach (AASClient.AASServiceReference.JyDataSet.委托Row DataRow1 in Program.jyDataSet.委托)
                {
                    if (DataRow1.证券代码 == this.Zqdm && DataRow1.委托数量 > DataRow1.成交数量 + DataRow1.撤单数量)
                    {

                        string Ret = Program.AASServiceClient.CancelOrder(Program.Current平台用户.用户名, DataRow1.组合号, DataRow1.证券代码, DataRow1.证券名称, DataRow1.市场代码, DataRow1.委托编号, DataRow1.买卖方向, DataRow1.委托数量, DataRow1.委托价格);
                        string[] Data = Ret.Split('|');
                        if (Data[1] != string.Empty)
                        {
                            Program.logger.LogJy(Program.Current平台用户.用户名, DataRow1.证券代码, DataRow1.证券名称, DataRow1.委托编号, DataRow1.买卖方向, DataRow1.委托数量, DataRow1.委托价格, "撤单失败: {0}", Data[1]);
                        }
                        else
                        {
                            Program.logger.LogJy(Program.Current平台用户.用户名, DataRow1.证券代码, DataRow1.证券名称, DataRow1.委托编号, DataRow1.买卖方向, DataRow1.委托数量, DataRow1.委托价格, "撤单成功");
                        }
                    }
                }
            }
            else
            {
                this.panel3.Visible = true;

                switch (交易方向1)
                {
                    case 交易方向.买入:
                    case 交易方向.卖出:
                        this.comboBox买卖方向.SelectedIndex = 快捷键Row1.方向;
                        break;
                    case 交易方向.券池融资买入:
                        this.comboBox买卖方向.SelectedIndex = 2;
                        break;
                    case 交易方向.券池融券卖出:
                        this.comboBox买卖方向.SelectedIndex = 3;
                        break;
                    case 交易方向.券池现金买入:
                        this.comboBox买卖方向.SelectedIndex = 4;
                        break;
                }



                SetInfo(快捷键Row1);


                股数模式 股数模式1 = (股数模式)快捷键Row1.股数模式;
                switch (股数模式1)
                {
                    case 股数模式.可用股数的百分之:
                        decimal 可用股数 = 0;
                        if (交易方向1 == 交易方向.买入 || 交易方向1 == 交易方向.券池融资买入)
                        {
                            可用股数 = this.可买股数;
                        }
                        else if (交易方向1 == 交易方向.卖出 || 交易方向1 == 交易方向.券池融券卖出)
                        {
                            可用股数 = this.可卖股数;
                        }
                        decimal qty = (可用股数 * 快捷键Row1.股数数值/ 100);
                        this.numericUpDown数量.Value = qty;
                        break;
                    case 股数模式.仓位的百分之:
                        qty = GetCoinQty(快捷键Row1);
                        this.numericUpDown数量.Value = qty;
                        break;
                    case 股数模式.默认值的百分之:
                        qty = (decimal.Parse(Program.accountDataSet.参数.GetParaValue(this.Zqdm + "默认股数", "0")) * 快捷键Row1.股数数值 / 100);
                        this.numericUpDown数量.Value = qty;
                        break;
                    case 股数模式.股数数值:
                        this.numericUpDown数量.Value = 快捷键Row1.股数数值;
                        break;
                    case 股数模式.不处理:
                        this.numericUpDown数量.Value = 0;
                        break;
                }

                decimal qtyMin, priceMin, notionMin;
                MarketAdapter.BinanceAdapter.Instance.GetFilter(Zqdm, out priceMin, out qtyMin, out notionMin);
                
                numericUpDown价格.DecimalPlaces = MarketAdapter.BinanceUtils.GetDigit(priceMin);
                numericUpDown价格.Increment = priceMin;
                numericUpDown价格.Value = numericUpDown价格.Value - numericUpDown价格.Value % priceMin;

                numericUpDown数量.DecimalPlaces = MarketAdapter.BinanceUtils.GetDigit(qtyMin);
                numericUpDown数量.Increment = qtyMin;
                numericUpDown数量.Value = numericUpDown数量.Value - numericUpDown数量.Value % qtyMin;
            }
        }

        DateTime lastQeuryQty = DateTime.MinValue;
        private decimal GetCoinQty(AccountDataSet.快捷键Row 快捷键Row1)
        {
            decimal qty = 0;
            var kv = MarketAdapter.BinanceUtils.GetBinanceCoinInfo(Zqdm);
            if ((DateTime.Now - lastQeuryQty).TotalSeconds > 3)
            {
                var CoinQtyFree = Program.AASServiceClient.QueryCoinQty(Program.Current平台用户.用户名, kv.Key);
                qty = (CoinQtyFree * 快捷键Row1.股数数值 / 100);
                if (tradeMainForm.dictAccountCoin.ContainsKey(kv.Key))
                {
                    tradeMainForm.dictAccountCoin[kv.Key].Freek__BackingField = CoinQtyFree;
                }
                
                lastQeuryQty = DateTime.Now;
            }
            else if(tradeMainForm.dictAccountCoin.ContainsKey(kv.Key))
            {
                qty = tradeMainForm.dictAccountCoin[kv.Key].Freek__BackingField;
            }
            return qty;
        }

        private void HqForm_Leave(object sender, EventArgs e)
        {

        }

        private void HqForm_Deactivate(object sender, EventArgs e)
        {
            this.comboBox代码.Text = this.Zqdm;
        }

        private void HqForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void HqForm_DockStateChanged(object sender, EventArgs e)
        {

        }

        private void HqForm_Activated(object sender, EventArgs e)
        {
            if (!this.panel3.Visible)
            {
                this.comboBox代码.Focus();
            }
            else
            {
                this.numericUpDown价格.Focus();
            }
        }
        #endregion

        #region Event 数量
        private void numericUpDown数量_Enter(object sender, EventArgs e)
        {
            this.numericUpDown数量.Select(0, this.numericUpDown数量.Value.ToString().Length);
        }

        private void numericUpDown数量_Click(object sender, EventArgs e)
        {
            this.numericUpDown数量.Select(0, this.numericUpDown数量.Value.ToString().Length);
        }
        #endregion

        #region Event 价格
        private void numericUpDown价格_Click(object sender, EventArgs e)
        {
            //this.numericUpDown价格.Select(0, this.numericUpDown价格.Value.ToString().Length);
        }

        private void numericUpDown价格_KeyDown(object sender, KeyEventArgs e)
        {
            string KeyCodeString = e.KeyCode.ToString();
            if (KeyCodeString == "Left")
            {
                this.numericUpDown价格.Value = Math.Max(this.numericUpDown价格.Minimum, this.numericUpDown价格.Value - this.numericUpDown价格.Increment * 10);
                e.Handled = true;
            }
            else if (KeyCodeString == "Right")
            {
                this.numericUpDown价格.Value = Math.Min(this.numericUpDown价格.Maximum, this.numericUpDown价格.Value + this.numericUpDown价格.Increment * 10);
                e.Handled = true;
            }

        }

        private void numericUpDown价格_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        #endregion

        #region Event ComboBox代码
        private void comboBox代码_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Subscribe();
            }
        }

        private void comboBox代码_TextChanged(object sender, EventArgs e)
        {
            Subscribe();

            if (this.panel3.Visible)
            {
                this.panel3.Visible = false;
            }
        }

        private void comboBox代码_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                KeysConverter kc = new KeysConverter();
                Keys Keys1 = (Keys)kc.ConvertFromString(e.KeyChar.ToString());
                AASClient.AccountDataSet.快捷键Row 快捷键Row1 = Program.accountDataSet.快捷键.FirstOrDefault(r => r.键名 == Keys1.ToString());
                if (快捷键Row1 != null)
                {
                    e.Handled = true;
                }
            }
            catch{ }
        }

        private void comboBox代码_Leave(object sender, EventArgs e)
        {
            this.comboBox代码.Text = this.Zqdm;
            if (!this.panel3.Visible)
            {
                bool NoComboBoxFocused = true;//没有其他combo有焦点，就把这个combo设为有焦点
                for (int i = 0; i < this.tradeMainForm.hqFormCount; i++)
                {
                    if (this.tradeMainForm.hqForm[i].comboBox代码.Focused)
                    {
                        NoComboBoxFocused = false;
                    }
                }

                if (NoComboBoxFocused)
                {
                    this.comboBox代码.Focus();
                }
            }
        }
        #endregion

        decimal GetPrice(价格模式 价格模式1, ListViewNF lvBuy, ListViewNF lvSale)
        {
            decimal price = 0;
            switch (价格模式1)
            {
                case 价格模式.卖五价:
                    decimal.TryParse(lvSale.Items[4].SubItems[0].Text, out price);
                    break;
                case 价格模式.卖四价:
                    decimal.TryParse(lvSale.Items[3].SubItems[0].Text, out price);
                    break;
                case 价格模式.卖三价:
                    decimal.TryParse(lvSale.Items[2].SubItems[0].Text, out price);
                    break;
                case 价格模式.卖二价:
                    decimal.TryParse(lvSale.Items[1].SubItems[0].Text, out price);
                    break;
                case 价格模式.卖一价:
                    decimal.TryParse(lvSale.Items[0].SubItems[0].Text, out price);
                    break;
                case 价格模式.买一价:
                    decimal.TryParse(lvBuy.Items[0].SubItems[0].Text, out price);
                    break;
                case 价格模式.买二价:
                    decimal.TryParse(lvBuy.Items[1].SubItems[0].Text, out price);
                    break;
                case 价格模式.买三价:
                    decimal.TryParse(lvBuy.Items[2].SubItems[0].Text, out price);
                    break;
                case 价格模式.买四价:
                    decimal.TryParse(lvBuy.Items[3].SubItems[0].Text, out price);
                    break;
                case 价格模式.买五价:
                    decimal.TryParse(lvBuy.Items[4].SubItems[0].Text, out price);
                    break;
            }
            return price;
        }

        private void SetInfo(AASClient.AccountDataSet.快捷键Row 快捷键Row1)
        {
            decimal BasePrice = 0;
            价格模式 价格模式1 = (价格模式)快捷键Row1.价格模式;
            BasePrice = GetPrice(价格模式1, listView买盘, listView卖盘);
            if (价格模式1 == 价格模式.不处理)
            {
                this.numericUpDown价格.Value = 0;
            }
            else
            {
                价差模式 价差模式1 = (价差模式)快捷键Row1.价差模式;
                switch (价差模式1)
                {
                    case 价差模式.百分之:
                        this.numericUpDown价格.Value = Math.Round(BasePrice * (1 + 快捷键Row1.价差数值 / 100), 8, MidpointRounding.AwayFromZero);
                        break;
                    case 价差模式.数值:
                        this.numericUpDown价格.Value = Math.Round(BasePrice + 快捷键Row1.价差数值, 8, MidpointRounding.AwayFromZero);
                        break;
                }
            }
        }

        private void btnSendOrder_Click(object sender, EventArgs e)
        {
            decimal qtyMin, priceMin, notionMin;
            MarketAdapter.BinanceAdapter.Instance.GetFilter(Zqdm, out priceMin, out qtyMin, out notionMin);

            if (numericUpDown数量.Value % qtyMin > 0)
            {
                Program.logger.LogInfo(string.Format("下单数量{0}与数量最小单位{1}余数为{2},请修正后再下单!", numericUpDown数量.Value, qtyMin, numericUpDown数量.Value % qtyMin));
            }
            if (numericUpDown价格.Value % priceMin > 0)
            {
                Program.logger.LogInfo(string.Format("下单价格{0}与价格最小单位{1}余数为{2},请修正后再下单!", numericUpDown价格.Value, priceMin, numericUpDown价格.Value % priceMin));
            }
            //if ((numericUpDown数量.Value * numericUpDown价格.Value) % notionMin > 0)
            //{
            //    Program.logger.LogInfo(string.Format("价格与数量乘积与金额最小值余数不为0,价格{0}* 数量{1} = {2},金额最小值{3}",
            //        numericUpDown价格.Value, numericUpDown数量.Value, numericUpDown价格.Value * numericUpDown数量.Value, notionMin));
            //}
            if (this.btnSendOrder.Enabled)
            {
                lock (sync)
                {
                    if (this.btnSendOrder.Enabled)
                    {
                        this.btnSendOrder.Enabled = false;
                        this.btnSendOrder.Text = "下单中……";
                        AASClient.AASServiceReference.DbDataSet.额度分配Row 交易额度Row1 = Program.serverDb.额度分配.FirstOrDefault(r => r.证券代码 == this.Zqdm);

                        decimal 委托价格 = Math.Round(this.numericUpDown价格.Value, 8, MidpointRounding.AwayFromZero);
                        decimal 委托数量 = Math.Round(this.numericUpDown数量.Value, 4, MidpointRounding.AwayFromZero);

                        SendOrder(交易额度Row1 == null ? "" : 交易额度Row1.证券名称, 委托价格, 委托数量);
                    }
                }
            }
        }
        
        private void SendOrder(string 证券名称, decimal 委托价格, decimal 委托数量)
        {
            string code = this.Zqdm;
            int tradeType = this.comboBox买卖方向.SelectedIndex;
            DateTime dt = DateTime.Now;
            Task.Run(()=> {
                try
                {
                    Program.logger.LogJy(Program.Current平台用户.用户名, code, 证券名称, "-", tradeType, 委托数量, 委托价格, "开始下单");
                    string Ret = Program.AASServiceClient.SendECoinOrder(Program.Current平台用户.用户名, code, tradeType, 委托数量, 委托价格);
                    string[] Data = Ret.Split('|');
                    if (Data[1] == string.Empty)
                    {
                        this.Last委托编号 = Data[0];
                        Program.logger.LogJy(Program.Current平台用户.用户名, code, 证券名称, Data[0], tradeType, 委托数量, 委托价格, "下单成功," + "耗时：" + (DateTime.Now - dt).TotalSeconds);
                    }
                    else
                    {
                        Program.logger.LogJy(Program.Current平台用户.用户名, code, 证券名称, string.Empty, tradeType, 委托数量, 委托价格, "下单失败, {0}", Data[1]);
                    }
                }
                catch (Exception ex)
                {
                    Program.logger.LogJy(Program.Current平台用户.用户名, code, 证券名称, string.Empty, tradeType, 委托数量, 委托价格, "下单异常, {0}",(ex.InnerException ?? ex).Message);
                }
                this.BeginInvoke(new Action(()=> {
                    this.btnSendOrder.Text = "下单";
                    this.btnSendOrder.Enabled = true;
                    this.panel3.Visible = false;
                }));
            });
        }

        private void panel3_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.panel3.Visible)
            {
                this.comboBox代码.Focus();
            }
            else
            {
                this.numericUpDown价格.Focus();
            }
        }

        private void linkLabel默认股数_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SetDefaultQuantityForm SetDefaultQuantityForm1 = new SetDefaultQuantityForm(this.Zqdm);

            SetDefaultQuantityForm1.ShowDialog();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            string ParaName = string.Format("{0} splitContainer1 SplitterDistance", this.index);
            Program.accountDataSet.参数.SetParaValue(ParaName, this.splitContainer1.SplitterDistance.ToString());
        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            string ParaName = string.Format("{0} splitContainer2 SplitterDistance", this.index);
            Program.accountDataSet.参数.SetParaValue(ParaName, this.splitContainer2.SplitterDistance.ToString());
        }
        
        private void CompletionByWhiteSpace(int index)
        {
            for (var i = index; i < Math.Max(listView逐笔成交.Items.Count, 50); i++)
            {
                var item2 = listView逐笔成交.Items[i];
                item2.SubItems[0].Text = "        ";
                item2.SubItems[1].Text = "        ";
                item2.SubItems[2].Text = "        ";
            }
            for (int i = 0; i < Math.Max(listView买盘.Items.Count, 20); i++)
            {
                var item = listView买盘.Items[i];
                item.SubItems[0].Text = "        ";
                item.SubItems[1].Text = "        ";
                
                var item1 = listView卖盘.Items[i];
                item1.SubItems[0].Text = "        ";
                item1.SubItems[1].Text = "        ";
            }

        }

        private void linkLabelRefreshCoinInfo_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (var item in tradeMainForm.hqForm)
            {
                var kv = MarketAdapter.BinanceUtils.GetBinanceCoinInfo(item.Zqdm);
                list.Add(kv.Key);
                list.Add(kv.Value);
            }

            var bnbCount = Program.AASServiceClient.QueryCoinQty(Program.Current平台用户.用户名, "BTC");
            tradeMainForm.dictAccountCoin["BNB"].Freek__BackingField = bnbCount;
            tradeMainForm.RefreshCoinQty();
        }
    }

}
