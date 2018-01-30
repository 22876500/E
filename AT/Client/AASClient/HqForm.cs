using AASClient.TDFData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;
using AASTrader.Client;
using DataModel;
using System.Threading;
using AASClient.AASServiceReference;

namespace AASClient
{
    public partial class HqForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private delegate void FlushClient();//代理

        #region TDF Source Control Info
        MarketData lastTdfMarket;
        HKMarketData lastHKMarket;
        MarketTransaction lastTran;

        ConcurrentQueue<MarketData> marketQueue = new ConcurrentQueue<MarketData>();
        ConcurrentQueue<MarketTransaction> transQueue = new ConcurrentQueue<MarketTransaction>();
        System.Threading.Thread _refreshPageThread = null;
        Queue<MarketTransaction> transCache = new Queue<MarketTransaction>(128);
        List<MarketTranEntity> tranEntityCache = new List<MarketTranEntity>();
        #endregion

        bool CodeFocus = true;
        private static object sync = new object();


        decimal 证券仓位 = 0;
        decimal 可卖股数 = 0;
        decimal 可买股数 = 0;

        int index;

        string[] N = { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };



        string Last委托编号;

        TradeMainForm tradeMainForm;


        BinaryFormatter binaryFormatter = new BinaryFormatter();

        public string Zqdm
        {
            get
            {
                return Program.accountDataSet.参数.GetParaValue("证券代码" + this.index.ToString(), "000001");
            }
        }

        decimal? MaxNum
        {
            get
            {
                if (this.numericUpDownTransMax.Value > 0)
                {
                    return this.numericUpDownTransMax.Value;
                }
                return null;
            }
            set
            {
                if (value.HasValue && value.Value >= 0 && this.numericUpDownTransMax.Value != value)
                {
                    this.numericUpDownTransMax.Value = value.Value;
                }
            }
        }

        decimal? MinNum
        {
            get
            {
                if (this.numericUpDownTransMin.Value > 0)
                {
                    return this.numericUpDownTransMin.Value;
                }
                return null;
            }
            set
            {
                if (value.HasValue && value.Value >= 0 && this.numericUpDownTransMin.Value != value)
                {
                    this.numericUpDownTransMin.Value = value.Value;
                }
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
            this.splitContainer1.SplitterDistance = int.Parse(Program.accountDataSet.参数.GetParaValue(ParaName1, "299"));

            string ParaName2 = string.Format("{0} splitContainer2 SplitterDistance", this.index);
            this.splitContainer2.SplitterDistance = int.Parse(Program.accountDataSet.参数.GetParaValue(ParaName2, "149"));

            var isShowFilteTran = Program.accountDataSet.参数.GetParaValue(index + "逐笔过滤显示", "0");
            if (isShowFilteTran == "1")
            {
                this.splitContainer3.Panel2Collapsed = false;
                this.pictureBox1.BackgroundImage = AASClient.Properties.Resources.next;
                panelFilter.Visible = true;
            }
            else
            {
                this.splitContainer3.Panel2Collapsed = true;
                this.pictureBox1.BackgroundImage = AASClient.Properties.Resources.previous;
                panelFilter.Visible = false;
            }
            string ParaName3 = string.Format("{0} splitContainer3 SplitterDistance", this.index);
            splitContainer3.SplitterDistance = int.Parse(Program.accountDataSet.参数.GetParaValue(ParaName3, "233"));
            splitContainer3.SplitterMoved += splitContainer3_SplitterMoved;

            for (int i = 0; i < 10; i++)
            {
                ListViewItem ListViewItem1 = new ListViewItem(new string[] { "        ", "        " });
                ListViewItem1.SubItems[1].ForeColor = Color.Black;
                this.listView买盘.Items.Add(ListViewItem1);


                ListViewItem ListViewItem2 = new ListViewItem(new string[] { "        ", "        " });
                ListViewItem2.SubItems[1].ForeColor = Color.Black;
                this.listView卖盘.Items.Add(ListViewItem2);

            }

            for (int i = 0; i < 50; i++)
            {
                ListViewItem ListViewItem3 = new ListViewItem(new string[] { "        ", "        ", "        " });
                ListViewItem3.SubItems[2].ForeColor = Color.Black;
                this.listView逐笔成交.Items.Add(ListViewItem3);

                ListViewItem ListViewItem4 = new ListViewItem(new string[] { "        ", "        ", "        " });
                ListViewItem4.SubItems[2].ForeColor = Color.Black;
                this.listView逐笔成交Filte.Items.Add(ListViewItem4);
            }



            for (int i = 0; i < this.listView买盘.Columns.Count; i++)
            {
                string ParaName = string.Format("{0}买盘列{1}宽度", this.index, i);
                this.listView买盘.Columns[i].Width = int.Parse(Program.accountDataSet.参数.GetParaValue(ParaName, "60"));
            }
            for (int i = 0; i < this.listView卖盘.Columns.Count; i++)
            {
                string ParaName = string.Format("{0}卖盘列{1}宽度", this.index, i);
                this.listView卖盘.Columns[i].Width = int.Parse(Program.accountDataSet.参数.GetParaValue(ParaName, "60"));
            }
            for (int i = 0; i < this.listView逐笔成交.Columns.Count; i++)
            {
                string ParaName = string.Format("{0}逐笔成交列{1}宽度", this.index, i);
                this.listView逐笔成交.Columns[i].Width = int.Parse(Program.accountDataSet.参数.GetParaValue(ParaName, i < 2 ? "60" : "76"));
            }

            for (int i = 0; i < this.listView逐笔成交Filte.Columns.Count; i++)
            {
                string ParaName = string.Format("{0}逐笔成交Filte列{1}宽度", this.index, i);
                this.listView逐笔成交Filte.Columns[i].Width = int.Parse(Program.accountDataSet.参数.GetParaValue(ParaName, i < 2 ? "60" : "76"));
            }
            var min = int.Parse(Program.accountDataSet.参数.GetParaValue(index + "逐笔过滤最小值", "0"));
            if (min >= 0)
                MinNum = min;

            var max = int.Parse(Program.accountDataSet.参数.GetParaValue(index + "逐笔过滤最大值", "0"));
            if (max > 0 && max >= min)
                MaxNum = max;

            this.comboBox代码.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.comboBox代码.AutoCompleteMode = AutoCompleteMode.SuggestAppend;


        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0014) // 禁掉清除背景消息
                return;
            base.WndProc(ref m);
        }

        private void HqForm_Load(object sender, EventArgs e)
        {
            this.comboBox代码.Text = this.Zqdm;
            //this.guid = Guid.NewGuid();
            SubData();
        }

        #region TDF Data Refresh
        private void SubData()
        {
            if (TDFData.DataSourceConfig.IsUseTDFData)
            {
                //DataCache.GetInstance().SubMarket();
                //DataCache.GetInstance().SubTrans();
                DataCache.GetInstance().UpdateMarketData += InsertMarketData;
                DataCache.GetInstance().UpdateTransaction += InsertTransData;
                //TDFSourceRefreshRun();
            }
        }

        //private void TDFSourceRefreshRun()
        //{
        //    if (_refreshPageThread == null)
        //    {
        //        lock (sync)
        //        {
        //            if (_refreshPageThread == null)
        //            {
        //                _refreshPageThread = new System.Threading.Thread(new ThreadStart(UpdateFormData)) { IsBackground = true };
        //                _refreshPageThread.Start();
        //            }
        //        }
        //    }
        //}

        //private void UpdateFormData()
        //{
        //    while (true)
        //    {
        //        try
        //        {
        //            if (marketQueue.Count > 0)
        //            {
        //                MarketData md = null;
        //                foreach (var item in marketQueue)
        //                {
        //                    if (marketQueue.TryDequeue(out md))
        //                        UpdateMarketData(md);
        //                }
        //            }
        //            if (transQueue.Count > 0)
        //            {
        //                MarketTransaction mt = null;
        //                foreach (var item in transQueue)
        //                {
        //                    if (transQueue.TryDequeue(out mt))
        //                        UpdateTransaction(mt);
        //                }
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            Program.logger.LogRunning("TDF数据源更新出错：{0}", ex.Message);
        //        }
        //        System.Threading.Thread.Sleep(50);
        //    }
        //}

        private void InsertTransData(MarketTransaction obj)
        {
            if (obj.Code == this.Zqdm)
            {
                //transQueue.Enqueue(obj);\
                UpdateTransaction(obj);
            }
        }

        private void InsertMarketData(MarketData obj)
        {
            if (obj.Code == this.Zqdm)
            {
                //marketQueue.Enqueue(obj);
                UpdateMarketData(obj);
            }
        }
        #endregion

        private void HqForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (DataSourceConfig.IsUseTDXData)
                {
                    //if (DataSourceConfig.IsUseVipCodes && DataSourceConfig.VipCodes.Contains(this.Zqdm))
                    //    return;
                }
                DataCache.GetInstance().UpdateMarketData -= InsertMarketData;
                DataCache.GetInstance().UpdateTransaction -= InsertTransData;
                _refreshPageThread.Abort();
            }
            catch (Exception)
            {

            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < this.listView买盘.Items.Count; i++)
            {
                ListViewItem ListViewItem1 = this.listView买盘.Items[i];
                ListViewItem ListViewItem2 = this.listView卖盘.Items[i];


                for (int j = 0; j < ListViewItem1.SubItems.Count; j++)
                {
                    string ParaName = string.Format("买{0}颜色", i);
                    int ColorValue = int.Parse(Program.accountDataSet.参数.GetParaValue(ParaName, Color.White.ToArgb().ToString()));
                    if (ListViewItem1.SubItems[j].BackColor != Color.FromArgb(ColorValue))
                    {
                        ListViewItem1.SubItems[j].BackColor = Color.FromArgb(ColorValue);
                    }
                }


                for (int j = 0; j < ListViewItem2.SubItems.Count; j++)
                {
                    string ParaName = string.Format("卖{0}颜色", i);
                    int ColorValue = int.Parse(Program.accountDataSet.参数.GetParaValue(ParaName, Color.White.ToArgb().ToString()));
                    if (ListViewItem2.SubItems[j].BackColor != Color.FromArgb(ColorValue))
                    {
                        ListViewItem2.SubItems[j].BackColor = Color.FromArgb(ColorValue);
                    }
                }
            }

            RefreshTitle();

            this.linkLabel默认股数.Text = string.Format("Shares: {0} MaxAmt: {1}", Program.accountDataSet.参数.GetParaValue(this.Zqdm + "默认股数", "0"), Program.accountDataSet.参数.GetParaValue(this.Zqdm + "最大金额", "0"));



            RefreshFontInfo();

            RefreshMarketBuySaleData();

            RefreshTransaction();


            this.证券仓位 = Program.serverDb.订单.Get证券仓位(this.Zqdm);



            decimal 已用仓位 = Program.jyDataSet.委托.Get已用仓位();
            this.label可用资金.Text = (Program.Current平台用户.仓位限制 - 已用仓位).ToString("f2");





            AASClient.AASServiceReference.DbDataSet.额度分配Row 交易额度Row1 = Program.serverDb.额度分配.FirstOrDefault(r => r.证券代码 == this.Zqdm);
            if (交易额度Row1 != null)
            {
                decimal 已买股数 = 0;
                decimal 已卖股数 = 0;
                Program.jyDataSet.委托.Get已买卖股数(this.Zqdm, out 已买股数, out 已卖股数);


                this.可买股数 = 交易额度Row1.交易额度 - 已买股数;
                this.可卖股数 = 交易额度Row1.交易额度 - 已卖股数;
            }
            else
            {
                if (PublicStock != null && this.Zqdm == PublicStock.StockCode)
                {
                    if (!PubStocksForm.PublicStockList.Contains(PublicStock))
                    {
                        PublicStock.CanSaleCount = 0;
                    }
                    //var pubStockItem = PubStocksForm.PublicStockList.FirstOrDefault(_ => _.StockCode == this.Zqdm);

                    //if (pubStockItem != null && PublicStock.CanSaleCount != pubStockItem.CanSaleCount)
                    //{
                    //    PublicStock.CanSaleCount = pubStockItem.CanSaleCount;
                    //}

                    decimal canUseMoney = decimal.Parse(this.label可用资金.Text);
                    decimal HighestPrice = decimal.Parse(this.label涨停价.Text);
                    if (HighestPrice > 0)
                    {
                        var canBuyCount = Math.Floor(Math.Min(canUseMoney / HighestPrice, PublicStock.CanSaleCount) / 100) * 100;
                        //var canBuyCount = Math.Floor(canUseMoney / (100 * HighestPrice)) * 100;
                        this.label可用股数.Text = canBuyCount.ToString();
                        this.可买股数 = canBuyCount;
                        this.可卖股数 = canBuyCount;
                    }
                }
                else
                {
                    this.可买股数 = 0;
                    this.可卖股数 = 0;
                }
            }

            this.label可用股数.Text = this.可卖股数.ToString("f0");

            
        }


        public void RefreshTitle()
        {
            string subInfo = string.Empty;
            if (TDFData.DataSourceConfig.IsUseTDFData && TDFData.DataCache.GetInstance().IsConnected && (DateTime.Now - MarketUpdateTime).TotalSeconds < 30)
            {
                subInfo = " - 已连接";
            }

            if (Program.证券名称.Count == 0)
            {
                this.Text = string.Format("[{0}]{1}", Manager.StockCodeManager.GetNameByCode(this.Zqdm), subInfo);
            }
            else
            {
                this.Text = string.Format("[{0}]{1}", L2Api.Get名称(this.Zqdm), subInfo);
            }
        }

        private void RefreshFontInfo()
        {
            Font 逐笔成交Font = null;
            string 字体Base64String = Program.accountDataSet.参数.GetParaValue("字体", null);
            if (字体Base64String != null)
            {
                using (MemoryStream MemoryStream1 = new MemoryStream(Convert.FromBase64String(字体Base64String)))
                {
                    逐笔成交Font = this.binaryFormatter.Deserialize(MemoryStream1) as Font;
                }
            }

            Font 买盘Font = null;
            字体Base64String = Program.accountDataSet.参数.GetParaValue("买盘字体", null);
            if (字体Base64String != null)
            {
                using (MemoryStream MemoryStream1 = new MemoryStream(Convert.FromBase64String(字体Base64String)))
                {
                    买盘Font = this.binaryFormatter.Deserialize(MemoryStream1) as Font;
                }
            }

            Font 卖盘Font = null;
            字体Base64String = Program.accountDataSet.参数.GetParaValue("卖盘字体", null);
            if (字体Base64String != null)
            {
                using (MemoryStream MemoryStream1 = new MemoryStream(Convert.FromBase64String(字体Base64String)))
                {
                    卖盘Font = this.binaryFormatter.Deserialize(MemoryStream1) as Font;
                }
            }

            if (买盘Font != null)
            {
                if (this.listView买盘.Font != 买盘Font)
                {
                    this.listView买盘.Font = 买盘Font;
                }
            }

            if (卖盘Font != null)
            {
                if (this.listView卖盘.Font != 卖盘Font)
                {
                    this.listView卖盘.Font = 卖盘Font;
                }
            }

            if (逐笔成交Font != null)
            {
                if (this.listView逐笔成交.Font != 逐笔成交Font)
                {
                    this.listView逐笔成交.Font = 逐笔成交Font;
                }
            }
        }

        #region 逐笔成交内容刷新

        private void RefreshTransaction()
        {
            if (DataSourceConfig.IsUseTDXData)
            {
                //if (DataSourceConfig.IsUseVipCodes && DataSourceConfig.VipCodes.Contains(this.Zqdm))
                //    return;
                if (DataSourceConfig.IsUseTDFData)
                {
                    if ((DateTime.Now - MarketUpdateTime).TotalSeconds < 10)
                    {
                        return;
                    }
                }

                DataTable DataTable1 = null;
                if (this.Zqdm.Length == 6 && Program.TransactionDataTable.ContainsKey(this.Zqdm))
                {
                    DataTable1 = Program.TransactionDataTable[this.Zqdm];
                }
                else if (this.Zqdm.Length == 5 && CommonUtils.EnableTdxHKApi)
                {
                    DataTable1 = L2HkApi.Instance.GetTran(this.Zqdm);
                }

                if (DataTable1 == null)
                {
                    return;
                }
                    
                int filterIndex = 0;

                for (int i = 0; i < DataTable1.Rows.Count && i < 50; i++)
                {
                    #region 刷新逐笔成交
                    DataRow DataRow1 = DataTable1.Rows[DataTable1.Rows.Count - 1 - i];

                    decimal Price = decimal.Parse(DataRow1[DataTable1.Columns.Contains("价格") ?  "价格" :"现价"] as string);
                    decimal Volumn = 0;
                    string Time = string.Empty;
                    string BS = BS = DataRow1["性质"] as string; ;

                    if (this.Zqdm.Length == 5)
                    {
                        Price = Price / 1000;
                        Volumn = decimal.Parse(DataRow1["现量"] as string);
                        Time = DataRow1["时间"] as string;
                    }
                    else
                    {
                        Volumn = decimal.Parse(DataRow1["成交量"] as string);
                        Time = DataRow1["成交时间"] as string;
                    }
                    

                    Refresh逐笔成交Item(Price, Volumn.ToString("#0"), Time, BS, listView逐笔成交.Items[i]);

                    if (ValidateVolumnLimit(Volumn))
                    {
                        Refresh逐笔成交Item(Price, Volumn.ToString("#0"), Time, BS, listView逐笔成交Filte.Items[filterIndex++]);
                    }
                    #endregion
                }
                
            }
        }

        private void Refresh逐笔成交Item(decimal Price, string Volumn, string Time, string BS, ListViewItem item)
        {
            if (item.SubItems[0].Text != Price.ToString(L2Api.PriceFormat(this.Zqdm)))
                item.SubItems[0].Text = Price.ToString(L2Api.PriceFormat(this.Zqdm));

            if (item.SubItems[1].Text != Volumn)
                item.SubItems[1].Text = Volumn;

            if (item.SubItems[2].Text != Time)
                item.SubItems[2].Text = Time;

            Color color = GetTransColor(BS);

            if (item.SubItems[1].ForeColor != color)
            {
                item.SubItems[0].ForeColor = color;
                item.SubItems[1].ForeColor = color;
                item.SubItems[2].ForeColor = color;
            }
        }

        private void UpdateTransaction(MarketTransaction obj)
        {
            if (this.IsHandleCreated)
            {
                try
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        decimal Price = (decimal)obj.Price / 10000;
                        decimal Volumn = Math.Round((decimal)obj.Volume / 100, 2);
                        string Time = obj.Time.DateTimeFormat();
                        string BS = ((char)obj.Flag).ToString();
                        Color c = GetTransColor(BS);

                        this.listView逐笔成交.Items.RemoveAt(listView逐笔成交.Items.Count - 1);

                        ListViewItem listViewItemAdded = new ListViewItem(new string[] { Price.ToString("#0.00"), Volumn.ToString("#0"), Time });
                        this.listView逐笔成交.Items.Insert(0, listViewItemAdded);

                        listViewItemAdded.SubItems[0].ForeColor = c;
                        listViewItemAdded.SubItems[1].ForeColor = c;
                        listViewItemAdded.SubItems[2].ForeColor = c;

                        if (ValidateVolumnLimit(Volumn))
                        {
                            listView逐笔成交Filte.Items.RemoveAt(listView逐笔成交.Items.Count - 1);
                            var filterItem = new ListViewItem(new string[] { Price.ToString("#0.00"), Volumn.ToString("#0"), Time });
                            this.listView逐笔成交Filte.Items.Insert(0, filterItem);
                            filterItem.SubItems[0].ForeColor = c;
                            filterItem.SubItems[1].ForeColor = c;
                            filterItem.SubItems[2].ForeColor = c;

                            //transCache.Insert(0, obj);
                            //if (transCache.Count >= 100)
                            //    transCache = transCache.GetRange(0, 50);
                        }
                        lastTran = obj;
                    });

                }
                catch (Exception ex)
                {
                    Program.logger.LogRunning("错误信息：{0}", ex.Message);
                }
            }
        }

        private static Color GetTransColor(string BS)
        {
            Color c;
            switch (BS)
            {
                case "B":
                    c = Color.Red;
                    break;
                case "S":
                    c = Color.Green;
                    break;
                default:
                    c = Color.Black;
                    break;
            }
            return c;
        }

        #endregion

        #region 买卖盘内容刷新

        private void RefreshMarketBuySaleData()
        {
            if (DataSourceConfig.IsUseTDXData)
            {
                var code = this.comboBox代码.Text;
                //if (DataSourceConfig.IsUseVipCodes && DataSourceConfig.VipCodes.Contains(this.Zqdm))
                //    return;
                if (DataSourceConfig.IsUseTDFData)
                {
                    if ((DateTime.Now - MarketUpdateTime).TotalSeconds < 10)
                    {
                        return;
                    }
                }

                //if (Zqdm.Length == 5 && CommonUtils.EnableTdxHKApi)
                //{
                //    RefreshHKMarketData();
                //    return;
                //}

                DataTable DataTable1 = null;

                if (this.Zqdm.Length == 6 && Program.HqDataTable.ContainsKey(this.Zqdm))
                {
                    DataTable1 = Program.HqDataTable[this.Zqdm];
                }
                if (DataTable1 == null)
                {
                    return;
                }
                DataRow DataRow1 = DataTable1.Rows[0];
                bool isHKData = this.Zqdm.Length == 5 && DataTable1.Columns.Contains("代码") && (DataRow1["代码"] as string).Length == 5;

                decimal ZS = 0;
                for (int i = 0; i < 10; i++)
                {
                    #region 刷新十档行情
                    decimal BJW = decimal.Parse((DataRow1["买" + this.N[i] + "价"] as string));
                    string BSL = DataRow1["买" + this.N[i] + "量"] as string;

                    decimal SJW = decimal.Parse((DataRow1["卖" + this.N[i] + "价"] as string));
                    string SSL = DataRow1["卖" + this.N[i] + "量"] as string;

                    //if (isHKData)
                    //{
                    //    BSL = int.Parse(BSL) / 100 + "";
                    //    SSL = int.Parse(SSL) / 100 + "";
                    //}

                    ZS = decimal.Parse((DataRow1["昨收"] as string));
                    decimal XJ = decimal.Parse((DataRow1["现价"] as string));
                    string ZF = (XJ == 0 ? "0.00%" : ((XJ - ZS) / ZS).ToString("P"));



                    string ZT = Math.Round(ZS * (1 + 0.1m), L2Api.Get精度(this.Zqdm), MidpointRounding.AwayFromZero).ToString();
                    string DT = Math.Round(ZS * (1 - 0.1m), L2Api.Get精度(this.Zqdm), MidpointRounding.AwayFromZero).ToString();



                    decimal High = decimal.Parse((DataRow1["最高"] as string));
                    decimal Low = decimal.Parse((DataRow1["最低"] as string));
                    if (this.label最高价.Text != High.ToString(L2Api.PriceFormat(this.Zqdm)))
                    {
                        this.label最高价.Text = High.ToString(L2Api.PriceFormat(this.Zqdm));
                    }
                    if (this.label最低价.Text != Low.ToString(L2Api.PriceFormat(this.Zqdm)))
                    {
                        this.label最低价.Text = Low.ToString(L2Api.PriceFormat(this.Zqdm));
                    }


                    if (this.label涨停价.Text != ZT)
                    {
                        this.label涨停价.Text = ZT;
                    }
                    if (this.label跌停价.Text != DT)
                    {
                        this.label跌停价.Text = DT;
                    }



                    if (this.listView买盘.Items[i].SubItems[0].Text != BJW.ToString(L2Api.PriceFormat(this.Zqdm)))
                    {
                        this.listView买盘.Items[i].SubItems[0].Text = BJW.ToString(L2Api.PriceFormat(this.Zqdm));
                    }

                    if (this.listView买盘.Items[i].SubItems[1].Text != BSL)
                    {
                        this.listView买盘.Items[i].SubItems[1].Text = BSL;
                    }

                    if (this.listView卖盘.Items[i].SubItems[0].Text != SJW.ToString(L2Api.PriceFormat(this.Zqdm)))
                    {
                        this.listView卖盘.Items[i].SubItems[0].Text = SJW.ToString(L2Api.PriceFormat(this.Zqdm));
                    }

                    if (this.listView卖盘.Items[i].SubItems[1].Text != SSL)
                    {
                        this.listView卖盘.Items[i].SubItems[1].Text = SSL;
                    }


                    if (this.label最新价.Text != XJ.ToString("#0.00"))
                    {
                        this.label最新价.Text = XJ.ToString("#0.00");
                    }

                    if (this.label涨幅.Text != ZF)
                    {
                        this.label涨幅.Text = ZF;
                    }



                    if (XJ > ZS)
                    {
                        this.label最新价字.ForeColor = Color.Red;
                        this.label最新价.ForeColor = Color.Red;
                        this.label涨幅字.ForeColor = Color.Red;
                        this.label涨幅.ForeColor = Color.Red;
                    }
                    else if (XJ < ZS)
                    {
                        this.label最新价字.ForeColor = Color.Green;
                        this.label最新价.ForeColor = Color.Green;
                        this.label涨幅字.ForeColor = Color.Green;
                        this.label涨幅.ForeColor = Color.Green;
                    }
                    else
                    {
                        this.label最新价字.ForeColor = Color.Black;
                        this.label最新价.ForeColor = Color.Black;
                        this.label涨幅字.ForeColor = Color.Black;
                        this.label涨幅.ForeColor = Color.Black;
                    }


                    #endregion
                }

                if (this.PublicStock != null)
                {
                    if (this.Zqdm == this.PublicStock.StockCode)
                    {
                        SetHighestPrice();
                    }
                        
                }
               
            }
        }

        private void RefreshHKMarketData()
        {
            var obj = L2HkApi.Instance.GetMarket(this.Zqdm);
            if (obj == null)
                return;
            lastHKMarket = obj;

            for (int i = 0; i < 10; i++)
            {
                #region 刷新十档行情
                decimal BJW = obj.PriceB[i];
                string BSL = obj.QtyB[i].ToString();

                decimal SJW = obj.PriceS[i];
                string SSL = obj.QtyS[i].ToString();

                var ZS = obj.PricePRE;
                decimal XJ = obj.Price;
                string ZF = (XJ == 0 ? "0.00%" : ((XJ - ZS) / ZS).ToString("P"));

                string ZT = Math.Round(ZS * (1 + 0.1m), L2Api.Get精度(this.Zqdm), MidpointRounding.AwayFromZero).ToString();
                string DT = Math.Round(ZS * (1 - 0.1m), L2Api.Get精度(this.Zqdm), MidpointRounding.AwayFromZero).ToString();

                decimal High = obj.Highest;
                decimal Low = obj.Lowest;
                if (this.label最高价.Text != High.ToString(L2Api.PriceFormat(this.Zqdm)))
                {
                    this.label最高价.Text = High.ToString(L2Api.PriceFormat(this.Zqdm));
                }
                if (this.label最低价.Text != Low.ToString(L2Api.PriceFormat(this.Zqdm)))
                {
                    this.label最低价.Text = Low.ToString(L2Api.PriceFormat(this.Zqdm));
                }

                if (this.label涨停价.Text != ZT)
                {
                    this.label涨停价.Text = ZT;
                }
                if (this.label跌停价.Text != DT)
                {
                    this.label跌停价.Text = DT;
                }

                if (this.listView买盘.Items[i].SubItems[0].Text != BJW.ToString(L2Api.PriceFormat(this.Zqdm)))
                {
                    this.listView买盘.Items[i].SubItems[0].Text = BJW.ToString(L2Api.PriceFormat(this.Zqdm));
                }

                if (this.listView买盘.Items[i].SubItems[1].Text != BSL)
                {
                    this.listView买盘.Items[i].SubItems[1].Text = BSL;
                }

                if (this.listView卖盘.Items[i].SubItems[0].Text != SJW.ToString(L2Api.PriceFormat(this.Zqdm)))
                {
                    this.listView卖盘.Items[i].SubItems[0].Text = SJW.ToString(L2Api.PriceFormat(this.Zqdm));

                }

                if (this.listView卖盘.Items[i].SubItems[1].Text != SSL)
                {
                    this.listView卖盘.Items[i].SubItems[1].Text = SSL;
                }

                if (this.label最新价.Text != XJ.ToString("#0.00"))
                {
                    this.label最新价.Text = XJ.ToString("#0.00");
                }

                if (this.label涨幅.Text != ZF)
                {
                    this.label涨幅.Text = ZF;
                }

                Color foreColor = Color.Black;

                if (XJ > ZS)
                {
                    foreColor = Color.Red;
                }
                else if (XJ < ZS)
                {
                    foreColor = Color.Green;
                }
                this.label最新价字.ForeColor = foreColor;
                this.label最新价.ForeColor = foreColor;
                this.label涨幅字.ForeColor = foreColor;
                this.label涨幅.ForeColor = foreColor;
                #endregion
            }
        }

        DateTime MarketUpdateTime = DateTime.Now;
        private void UpdateMarketData(MarketData obj)
        {
            try
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    MarketUpdateTime = DateTime.Now;
                    var len = Math.Min(10, obj.AskPrice.Length);

                    for (int i = 0; i < len; i++)
                    {
                        #region 刷新十档行情
                        decimal BJW = (decimal)obj.BidPrice[i] / 10000;
                        string BSL = Math.Ceiling((decimal)obj.BidVol[i] / 100).ToString();

                        decimal SJW = (decimal)obj.AskPrice[i] / 10000;
                        string SSL = Math.Round((decimal)obj.AskVol[i] / 100).ToString();

                        var ZS = (decimal)obj.PreClose / 10000;
                        decimal XJ = (decimal)obj.Match / 10000;
                        string ZF = (XJ == 0 ? "0.00%" : ((XJ - ZS) / ZS).ToString("P"));

                        string ZT = Math.Round(ZS * (1 + 0.1m), L2Api.Get精度(this.Zqdm), MidpointRounding.AwayFromZero).ToString();
                        string DT = Math.Round(ZS * (1 - 0.1m), L2Api.Get精度(this.Zqdm), MidpointRounding.AwayFromZero).ToString();

                        decimal High = (decimal)obj.High / 10000;
                        decimal Low = (decimal)obj.Low / 10000;
                        if (this.label最高价.Text != High.ToString(L2Api.PriceFormat(this.Zqdm)))
                        {
                            this.label最高价.Text = High.ToString(L2Api.PriceFormat(this.Zqdm));
                        }
                        if (this.label最低价.Text != Low.ToString(L2Api.PriceFormat(this.Zqdm)))
                        {
                            this.label最低价.Text = Low.ToString(L2Api.PriceFormat(this.Zqdm));
                        }

                        if (this.label涨停价.Text != ZT)
                        {
                            this.label涨停价.Text = ZT;
                        }
                        if (this.label跌停价.Text != DT)
                        {
                            this.label跌停价.Text = DT;
                        }

                        if (this.listView买盘.Items[i].SubItems[0].Text != BJW.ToString(L2Api.PriceFormat(this.Zqdm)))
                        {
                            this.listView买盘.Items[i].SubItems[0].Text = BJW.ToString(L2Api.PriceFormat(this.Zqdm));
                        }

                        if (this.listView买盘.Items[i].SubItems[1].Text != BSL)
                        {
                            this.listView买盘.Items[i].SubItems[1].Text = BSL;
                        }

                        if (this.listView卖盘.Items[i].SubItems[0].Text != SJW.ToString(L2Api.PriceFormat(this.Zqdm)))
                        {
                            this.listView卖盘.Items[i].SubItems[0].Text = SJW.ToString(L2Api.PriceFormat(this.Zqdm));

                        }

                        if (this.listView卖盘.Items[i].SubItems[1].Text != SSL)
                        {
                            this.listView卖盘.Items[i].SubItems[1].Text = SSL;
                        }

                        if (this.label最新价.Text != XJ.ToString("#0.00"))
                        {
                            this.label最新价.Text = XJ.ToString("#0.00");
                        }

                        if (this.label涨幅.Text != ZF)
                        {
                            this.label涨幅.Text = ZF;
                        }

                        Color foreColor = Color.Black;

                        if (XJ > ZS)
                        {
                            foreColor = Color.Red;
                        }
                        else if (XJ < ZS)
                        {
                            foreColor = Color.Green;
                        }
                        this.label最新价字.ForeColor = foreColor;
                        this.label最新价.ForeColor = foreColor;
                        this.label涨幅字.ForeColor = foreColor;
                        this.label涨幅.ForeColor = foreColor;
                        #endregion
                    }
                    lastTdfMarket = obj;
                    if (PublicStock != null && obj.Code == PublicStock.StockCode)
                    {
                        SetHighestPrice();
                    }
                });

            }
            catch (Exception ex)
            {
                Program.logger.LogRunning("错误信息：{0}", ex.Message);
            }
        }

        bool IsPossessed = false;
        private void SetHighestPrice()
        {

            decimal canUseMoney = decimal.Parse(this.label可用资金.Text);
            if (canUseMoney > 0 && !IsPossessed)
            {
                IsPossessed = true;
                decimal HighestPrice = decimal.Parse(this.label涨停价.Text);
                try
                {
                    var canBuyCount = Math.Floor(canUseMoney / (100 * HighestPrice)) * 100;
                    
                    this.comboBox买卖方向.SelectedIndex = 3;
                    this.numericUpDown价格.Value = HighestPrice;
                    this.numericUpDown数量.Value = Math.Min(canBuyCount, Math.Floor((decimal)PublicStock.CanSaleCount / 100) * 100);
                }
                catch (Exception ex)
                {
                    Program.logger.LogRunning("公共券池股票自动占有出错\r\n  Message{0}\r\n  Trace{1}", ex.Message, ex.StackTrace);
                }
                this.panel3.Visible = true;
            }
        }
        #endregion


        private void listView买盘_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView买盘_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView卖盘_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView卖盘_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView逐笔成交_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView逐笔成交_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void HqForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Program.serverDb.额度分配.Any(r => r.证券代码 == this.Zqdm) )
            {
                if (PubStocksForm.PublicStockList == null || PubStocksForm.PublicStockList.FirstOrDefault(_ => _.StockCode == this.Zqdm) == null)
                {
                    if (this.Zqdm.Length != 5)
                        return;
                }
            }

            //Keys key = e.KeyCode;
            // if (PubStocksForm.PublicStockList != null && comboBox买卖方向.SelectedIndex > 1)
            //{
            //    var pubStockItem = PubStocksForm.PublicStockList.FirstOrDefault(_ => _.StockCode == this.Zqdm);

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
                if (this.Last委托编号 != null && this.Last委托编号.Length == 36)
                {
                    string orderID = Program.AASServiceClient.GetOrderIDByReference(Program.Current平台用户.用户名, this.Last委托编号);
                    if (!string.IsNullOrEmpty(orderID))
                    {
                        this.Last委托编号 = orderID;
                    }
                }
                if (this.Last委托编号 != null && this.Last委托编号.Length < 36)
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

                

                if (lastTdfMarket != null)
                {
                    SetInfoTdfMarket(快捷键Row1);
                }
                else if (lastHKMarket != null)
                {
                    SetInfoHKMarket(快捷键Row1);
                }
                else if (Program.HqDataTable.ContainsKey(this.Zqdm))
                {
                    SetInfo(快捷键Row1);
                }


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
                        int int1 = (int)(可用股数 * 快捷键Row1.股数数值 / 100);
                        this.numericUpDown数量.Value = (int1 / 100) * 100;
                        break;
                    case 股数模式.仓位的百分之:
                        int1 = (int)(this.证券仓位 * 快捷键Row1.股数数值 / 100);
                        this.numericUpDown数量.Value = (int1 / 100) * 100;
                        break;
                    case 股数模式.默认值的百分之:
                        int1 = (int)(decimal.Parse(Program.accountDataSet.参数.GetParaValue(this.Zqdm + "默认股数", "0")) * 快捷键Row1.股数数值 / 100);
                        this.numericUpDown数量.Value = (int1 / 100) * 100;
                        break;
                    case 股数模式.股数数值:
                        this.numericUpDown数量.Value = 快捷键Row1.股数数值;
                        break;
                    case 股数模式.不处理:
                        //this.numericUpDown数量.Value = 0;
                        break;
                }
            }
        }

        private void SetInfoHKMarket(AccountDataSet.快捷键Row 快捷键Row1)
        {
            var o = lastHKMarket;
            价格模式 价格模式1 = (价格模式)快捷键Row1.价格模式;
            decimal BasePrice = GetPrice(价格模式1, listView买盘, listView卖盘, o.Highest, o.Lowest, o.Price);
            if (价格模式1 == 价格模式.不处理)
            {
                this.numericUpDown价格.Select(0, this.numericUpDown价格.Value.ToString().Length);
            }
            else
            {
                价差模式 价差模式1 = (价差模式)快捷键Row1.价差模式;
                switch (价差模式1)
                {
                    case 价差模式.百分之:
                        this.numericUpDown价格.Value = Math.Round(BasePrice * (1 + 快捷键Row1.价差数值 / 100), L2Api.Get精度(this.Zqdm), MidpointRounding.AwayFromZero);
                        break;
                    case 价差模式.数值:
                        this.numericUpDown价格.Value = Math.Round(BasePrice + 快捷键Row1.价差数值, L2Api.Get精度(this.Zqdm), MidpointRounding.AwayFromZero);
                        break;
                }

                this.numericUpDown价格.Select(0, this.numericUpDown价格.Value.ToString().Length);
            }
        }

        private void SetInfoTdfMarket(AccountDataSet.快捷键Row 快捷键Row1)
        {
            var marketData = lastTdfMarket;

            decimal BasePrice = 0;
            价格模式 价格模式1 = (价格模式)快捷键Row1.价格模式;
            switch (价格模式1)
            {
                case 价格模式.卖五价:
                case 价格模式.卖四价:
                case 价格模式.卖三价:
                case 价格模式.卖二价:
                case 价格模式.卖一价:
                case 价格模式.买一价:
                case 价格模式.买二价:
                case 价格模式.买三价:
                case 价格模式.买四价:
                case 价格模式.买五价:
                case 价格模式.现价:
                    BasePrice = GetPrice(价格模式1, listView买盘, listView卖盘, marketData.HighLimited / 10000, marketData.LowLimited/ 10000 , marketData.Match);
                    break;
                case 价格模式.涨停价:
                    decimal ZS = (decimal)marketData.PreClose / 10000;
                    BasePrice = Math.Round(ZS * (1 + 0.1m), L2Api.Get精度(this.Zqdm), MidpointRounding.AwayFromZero);
                    break;
                case 价格模式.跌停价:
                    ZS = (decimal)marketData.PreClose / 10000;
                    BasePrice = Math.Round(ZS * (1 - 0.1m), L2Api.Get精度(this.Zqdm), MidpointRounding.AwayFromZero);
                    break;
                default:
                    break;
            }
            if (价格模式1 == 价格模式.不处理)
            {
                //this.numericUpDown价格.Value = 0;
                this.numericUpDown价格.Select(0, this.numericUpDown价格.Value.ToString().Length);
            }
            else
            {
                价差模式 价差模式1 = (价差模式)快捷键Row1.价差模式;
                switch (价差模式1)
                {
                    case 价差模式.百分之:
                        this.numericUpDown价格.Value = Math.Round(BasePrice * (1 + 快捷键Row1.价差数值 / 100), L2Api.Get精度(this.Zqdm), MidpointRounding.AwayFromZero);
                        break;
                    case 价差模式.数值:
                        this.numericUpDown价格.Value = Math.Round(BasePrice + 快捷键Row1.价差数值, L2Api.Get精度(this.Zqdm), MidpointRounding.AwayFromZero);
                        break;
                }

                this.numericUpDown价格.Select(0, this.numericUpDown价格.Value.ToString().Length);
            }
        }

        //decimal GetPrice(价格模式 价格模式1, ListViewNF lvBuy, ListViewNF lvSale, MarketData marketData)
        //{
        //    decimal price = 0;
        //    switch (价格模式1)
        //    {
        //        case 价格模式.卖五价:
        //            decimal.TryParse(lvSale.Items[4].SubItems[0].Text, out price);
        //            break;
        //        case 价格模式.卖四价:
        //            decimal.TryParse(lvSale.Items[3].SubItems[0].Text, out price);
        //            break;
        //        case 价格模式.卖三价:
        //            decimal.TryParse(lvSale.Items[2].SubItems[0].Text, out price);
        //            break;
        //        case 价格模式.卖二价:
        //            decimal.TryParse(lvSale.Items[1].SubItems[0].Text, out price);
        //            break;
        //        case 价格模式.卖一价:
        //            decimal.TryParse(lvSale.Items[0].SubItems[0].Text, out price);
        //            break;
        //        case 价格模式.买一价:
        //            decimal.TryParse(lvBuy.Items[0].SubItems[0].Text, out price);
        //            break;
        //        case 价格模式.买二价:
        //            decimal.TryParse(lvBuy.Items[1].SubItems[0].Text, out price);
        //            break;
        //        case 价格模式.买三价:
        //            decimal.TryParse(lvBuy.Items[2].SubItems[0].Text, out price);
        //            break;
        //        case 价格模式.买四价:
        //            decimal.TryParse(lvBuy.Items[3].SubItems[0].Text, out price);
        //            break;
        //        case 价格模式.买五价:
        //            decimal.TryParse(lvBuy.Items[4].SubItems[0].Text, out price);
        //            break;
        //        case 价格模式.涨停价:
        //            return marketData.HighLimited / 10000;
        //        case 价格模式.跌停价:
        //            return marketData.LowLimited / 10000;
        //        case 价格模式.现价:
        //        default:
        //            return (decimal)marketData.Match;
        //    }
        //    return price;
        //}

        decimal GetPrice(价格模式 价格模式1, ListViewNF lvBuy, ListViewNF lvSale, decimal High, decimal Low, decimal NowPrice)
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
                case 价格模式.涨停价:
                    return High;
                case 价格模式.跌停价:
                    return Low;
                case 价格模式.现价:
                default:
                    return NowPrice;
            }
            return price;
        }

        private void SetInfo(AASClient.AccountDataSet.快捷键Row 快捷键Row1)
        {
            DataTable DataTable1 = Program.HqDataTable[this.Zqdm];
            DataRow DataRow1 = DataTable1.Rows[0];
            decimal BasePrice = 0;
            价格模式 价格模式1 = (价格模式)快捷键Row1.价格模式;
            switch (价格模式1)
            {
                case 价格模式.卖五价:
                case 价格模式.卖四价:
                case 价格模式.卖三价:
                case 价格模式.卖二价:
                case 价格模式.卖一价:
                case 价格模式.买一价:
                case 价格模式.买二价:
                case 价格模式.买三价:
                case 价格模式.买四价:
                case 价格模式.买五价:
                case 价格模式.现价:
                    BasePrice = decimal.Parse((DataRow1[价格模式1.ToString()] as string));
                    break;
                case 价格模式.涨停价:
                    decimal ZS = decimal.Parse((DataRow1["昨收"] as string));
                    BasePrice = Math.Round(ZS * (1 + 0.1m), L2Api.Get精度(this.Zqdm), MidpointRounding.AwayFromZero);
                    break;
                case 价格模式.跌停价:
                    ZS = decimal.Parse((DataRow1["昨收"] as string));
                    BasePrice = Math.Round(ZS * (1 - 0.1m), L2Api.Get精度(this.Zqdm), MidpointRounding.AwayFromZero);
                    break;
                default:
                    break;
            }
            if (价格模式1 == 价格模式.不处理)
            {
                //this.numericUpDown价格.Value = 0;
                this.numericUpDown价格.Select(0, this.numericUpDown价格.Value.ToString().Length);
            }
            else
            {
                价差模式 价差模式1 = (价差模式)快捷键Row1.价差模式;
                switch (价差模式1)
                {
                    case 价差模式.百分之:
                        this.numericUpDown价格.Value = Math.Round(BasePrice * (1 + 快捷键Row1.价差数值 / 100), L2Api.Get精度(this.Zqdm), MidpointRounding.AwayFromZero);
                        break;
                    case 价差模式.数值:
                        this.numericUpDown价格.Value = Math.Round(BasePrice + 快捷键Row1.价差数值, L2Api.Get精度(this.Zqdm), MidpointRounding.AwayFromZero);
                        break;
                }

                this.numericUpDown价格.Select(0, this.numericUpDown价格.Value.ToString().Length);
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (PubStocksForm.PublicStockList != null && comboBox买卖方向.SelectedIndex > 1)
            {
                var pubStockItem = PubStocksForm.PublicStockList.FirstOrDefault(_ => _.StockCode == this.Zqdm);
                if (pubStockItem == null)
                {
                    MessageBox.Show("公共券池不包含该股票!");
                }
                else if (this.numericUpDown数量.Value < 100)
                {
                    MessageBox.Show("下单数量不能小于100股!");
                }
                else
                {
                    SendPubOrder(PublicStock);
                }
            }
            else
            {
                if (!Program.serverDb.额度分配.Any(r => r.证券代码 == this.Zqdm))
                {
                    return;
                }
                else
                {
                    int qty = -1;//如果长度为5，且下单数量非一手整数倍，则提示
                    if (this.Zqdm.Length == 5 && L2HkApi.Instance.GetQty(this.Zqdm, out qty) && qty > 0 && numericUpDown数量.Value % qty != 0)
                    {
                        MessageBox.Show(string.Format("下单数量应为一手({0})的整数倍!,", qty));
                    }
                    else if (!string.IsNullOrWhiteSpace(label涨停价.Text) && numericUpDown价格.Value > decimal.Parse(label涨停价.Text))
                    {
                        MessageBox.Show(string.Format("欲挂单价格{0}高于涨停价{1}", numericUpDown价格.Value.ToString(), label涨停价.Text));
                    }
                    else if (!string.IsNullOrWhiteSpace(label跌停价.Text) && numericUpDown价格.Value < decimal.Parse(label跌停价.Text))
                    {
                        MessageBox.Show(string.Format("欲挂单价格{0}低于跌停价{1}", numericUpDown价格.Value.ToString(), label跌停价.Text));
                    }
                    else
                    {
                        SendOrder();
                    }
                }
            }

        }

        int GetPubOrderSide()
        {
            switch (this.comboBox买卖方向.SelectedIndex)
            {
                case 2:
                    return 2;//公共券池资源，融资买入
                case 3:
                    return 3;//公共券池资源，融券卖出
                case 4:
                    return 0;//公共券池资源，现金买入
                default:
                    return this.comboBox买卖方向.SelectedIndex;
            }
        }

        private void SendPubOrder(Model.PublicStock pubStockItem)
        {
            this.btnSendOrder.Enabled = false;

            decimal 委托价格 = Math.Round(this.numericUpDown价格.Value, L2Api.Get精度(this.Zqdm), MidpointRounding.AwayFromZero);
            decimal 委托数量 = Math.Round(this.numericUpDown数量.Value, 0, MidpointRounding.AwayFromZero);
            decimal highestPrice = decimal.Parse(label涨停价.Text);


            int tradeType = GetPubOrderSide();

            WaitCallback callback = i =>
            {
                DateTime dt = DateTime.Now;
                try
                {
                    string Ret = Program.AASServiceClient.SendPubStock(Program.Current平台用户.用户名, pubStockItem.StockCode, pubStockItem.StockName, tradeType, 委托数量, 委托价格, highestPrice, "");
                    string[] Data = Ret.Split('|');
                    if (Data[1] == string.Empty)
                    {
                        this.Last委托编号 = Data[0];
                        Program.logger.LogJy(Program.Current平台用户.用户名, pubStockItem.StockCode, pubStockItem.StockName, Data[0], tradeType, 委托数量, 委托价格,
                            "公共券池股票下单成功," + "耗时：" + (DateTime.Now - dt).TotalSeconds);
                    }
                    else
                    {
                        Program.logger.LogJy(Program.Current平台用户.用户名, pubStockItem.StockCode, pubStockItem.StockName, string.Empty, tradeType, 委托数量, 委托价格,
                            "公共券池股票下单失败, {0}", Data[1]);
                    }
                    this.Invoke(new FlushClient(() => { this.panel3.Visible = false; btnSendOrder.Enabled = true; }));
                }
                catch (Exception ex)
                {
                    Program.logger.LogRunning("交易员{0}公共券池股票{1}下单异常：\r\n  Message:{2}\r\n  StackTrace:{3}", Program.Current平台用户.用户名, this.Zqdm, ex.Message, ex.StackTrace);
                    this.Invoke(new FlushClient(() => { this.panel3.Visible = false; btnSendOrder.Enabled = true; }));
                }

            };
            ThreadPool.QueueUserWorkItem(callback, 1);
        }


        private void SendOrder()
        {
            this.btnSendOrder.Enabled = false;
            AASClient.AASServiceReference.DbDataSet.额度分配Row 交易额度Row1 = Program.serverDb.额度分配.FirstOrDefault(r => r.证券代码 == this.Zqdm);

            decimal 委托价格 = Math.Round(this.numericUpDown价格.Value, L2Api.Get精度(this.Zqdm), MidpointRounding.AwayFromZero);
            decimal 委托数量 = Math.Round(this.numericUpDown数量.Value, 0, MidpointRounding.AwayFromZero);

            //decimal Wtje = 委托价格 * 委托数量;
            //decimal WtjeLimit = decimal.Parse(Program.accountDataSet.参数.GetParaValue(this.Zqdm + "最大金额", "0"));

            SendingSync(交易额度Row1 == null ? "" : 交易额度Row1.证券名称, 委托价格, 委托数量);
            this.btnSendOrder.Enabled = true;
        }

        private void SendingSync(string 证券名称, decimal 委托价格, decimal 委托数量)
        {
            string code = this.Zqdm;
            int tradeType = this.comboBox买卖方向.SelectedIndex;
            WaitCallback callback = i =>
            {
                DateTime dt = DateTime.Now;
                //var order = new SendingEntity() { Price = 委托价格, Quatity = 委托数量, Zqdm = code, TradeType = tradeType, IsSending = true };
                //CommonUtils.SendingCache.Add(order);
                try
                {
                    string Ret;
                    if (code.Length == 6)
                    {
                        Ret = Program.AASServiceClient.SendOrder(Program.Current平台用户.用户名, code, tradeType, 委托数量, 委托价格);
                        string[] Data = Ret.Split('|');
                        if (Data[1] == string.Empty)
                        {
                            this.Last委托编号 = Data[0];
                            Program.logger.LogJy(Program.Current平台用户.用户名, code, 证券名称, Data[0], tradeType, 委托数量, 委托价格, "下单成功," + "耗时：" + (DateTime.Now - dt).TotalSeconds);
                            this.Invoke(new FlushClient(() => { this.panel3.Visible = false; }));
                        }
                        else
                        {
                            Program.logger.LogJy(Program.Current平台用户.用户名, code, 证券名称, string.Empty, tradeType, 委托数量, 委托价格, "下单失败, {0}", Data[1]);
                        }
                    }
                    else
                    {
                        Ret = Program.AASServiceClient.SendAyersOrder(Program.Current平台用户.用户名, code, 证券名称, tradeType, (int)委托数量, 委托价格);
                        string[] Data = Ret.Split('|');
                        if (Data[1] == string.Empty)
                        {
                            this.Last委托编号 = Data[0];

                            Program.logger.LogJy(Program.Current平台用户.用户名, code, 证券名称, string.Empty, tradeType, 委托数量, 委托价格, "{0}", "订单已发送");
                        }
                        else
                        {
                            Program.logger.LogJy(Program.Current平台用户.用户名, code, 证券名称, string.Empty, tradeType, 委托数量, 委托价格, "下单失败, {0}", Data[1]);
                        }
                    }


                   
                }
                catch (Exception ex)
                {
                    Program.logger.LogRunning("交易员{0}下单异常：\r\n  Message:{1}\r\n  StackTrace:{2}", Program.Current平台用户.用户名, ex.Message, ex.StackTrace);
                    this.Invoke(new FlushClient(() => { this.panel3.Visible = false; }));
                }
                //CommonUtils.SendingCache.Remove(order);
            };
            ThreadPool.QueueUserWorkItem(callback, 1);
        }

        private void comboBox代码_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Regex Regex1 = new Regex("[0-9]{6}");
                if (Regex1.IsMatch(this.comboBox代码.Text))
                {
                    string Zqdm = Regex1.Match(this.comboBox代码.Text).Value;
                    this.comboBox代码.Text = Zqdm;

                    Program.accountDataSet.参数.SetParaValue("证券代码" + this.index.ToString(), Zqdm);
                    DataCache.GetInstance().AddSub(Zqdm);
                    transCache.Clear();
                    CompletionByWhiteSpace(0);
                }
            }
        }

        private void comboBox代码_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(this.comboBox代码.Text, "^[0-9]{5,6}$") && (this.comboBox代码.Text.Length == 5 || this.comboBox代码.Text.Length == 6))
            {
                Program.accountDataSet.参数.SetParaValue("证券代码" + this.index.ToString(), this.comboBox代码.Text);
                CompletionByWhiteSpace(0);

                if (this.comboBox代码.Text.Length == 6)
                {
                    DataCache.GetInstance().AddSub(this.Zqdm);
                    transCache.Clear();
                    IsPublicStockCheck();
                }
            }

            if (this.panel3.Visible)
            {
                this.panel3.Visible = false;
            }
        }

        private void IsPublicStockCheck()
        {
            if (PubStocksForm.PublicStockList != null)
            {
                var pubItem = PubStocksForm.PublicStockList.FirstOrDefault(_ => _.StockCode == this.Zqdm);
                if (pubItem == null)
                {
                    if (PublicStock != null)
                    {
                        PublicStock = null;
                        IsPossessed = false;
                    }
                }
                else
                {
                    if (PublicStock == null)
                    {
                        PublicStock = pubItem;
                    }
                    else if (PublicStock.StockCode != pubItem.StockCode)
                    {
                        PublicStock = pubItem;
                    }
                }
            }
        }

        private void comboBox代码_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)96)//屏蔽~键，因为做了快捷键
            //{
            //    e.Handled = true;
            //}

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
            catch
            {

            }


        }


        private void comboBox代码_Leave(object sender, EventArgs e)
        {
            this.comboBox代码.Text = this.Zqdm;
            if (!this.panel3.Visible && CodeFocus)
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


        private void comboBox代码_Click(object sender, EventArgs e)
        {
            this.comboBox代码.SelectAll();
        }




        private void numericUpDown数量_Enter(object sender, EventArgs e)
        {
            this.numericUpDown数量.Select(0, this.numericUpDown数量.Value.ToString().Length);
        }



        private void HqForm_Leave(object sender, EventArgs e)
        {

        }



        private void HqForm_Deactivate(object sender, EventArgs e)
        {
            this.comboBox代码.Text = this.Zqdm;
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



        private void HqForm_DockStateChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown价格_Click(object sender, EventArgs e)
        {
            this.numericUpDown价格.Select(0, this.numericUpDown价格.Value.ToString().Length);
        }

        private void numericUpDown数量_Click(object sender, EventArgs e)
        {
            this.numericUpDown数量.Select(0, this.numericUpDown数量.Value.ToString().Length);
        }

        private void linkLabel默认股数_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SetDefaultQuantityForm SetDefaultQuantityForm1 = new SetDefaultQuantityForm(this.Zqdm);

            SetDefaultQuantityForm1.ShowDialog();
        }

        private void HqForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void comboBox代码_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {


        }

        private void listView买盘_DoubleClick(object sender, EventArgs e)
        {
            if (this.listView买盘.SelectedItems.Count > 0)
            {
                this.colorDialog1.Color = this.listView买盘.SelectedItems[0].SubItems[0].BackColor;
                DialogResult DialogResult1 = this.colorDialog1.ShowDialog();
                if (DialogResult1 != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
                for (int i = 0; i < this.listView买盘.SelectedItems[0].SubItems.Count; i++)
                {
                    this.listView买盘.SelectedItems[0].SubItems[i].BackColor = this.colorDialog1.Color;
                }

                string ParaName = string.Format("买{0}颜色", this.listView买盘.SelectedItems[0].Index);
                Program.accountDataSet.参数.SetParaValue(ParaName, this.colorDialog1.Color.ToArgb().ToString());
            }
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

        private void numericUpDown价格_KeyDown(object sender, KeyEventArgs e)
        {
            string KeyCodeString = e.KeyCode.ToString();
            if (KeyCodeString == "Left")
            {
                this.numericUpDown价格.Value = Math.Max(this.numericUpDown价格.Minimum, this.numericUpDown价格.Value - 0.05m);
                //this.numericUpDown价格.Select(0, this.numericUpDown价格.Value.ToString().Length);
                e.Handled = true;
            }
            else if (KeyCodeString == "Right")
            {
                this.numericUpDown价格.Value = Math.Min(this.numericUpDown价格.Maximum, this.numericUpDown价格.Value + 0.05m);
                //this.numericUpDown价格.Select(0, this.numericUpDown价格.Value.ToString().Length);
                e.Handled = true;
            }

        }

        private void numericUpDown价格_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void numericUpDown价格_ValueChanged(object sender, EventArgs e)
        {

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

        private void listView买盘_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            string ParaName = string.Format("{0}买盘列{1}宽度", this.index, e.ColumnIndex);
            Program.accountDataSet.参数.SetParaValue(ParaName, this.listView买盘.Columns[e.ColumnIndex].Width.ToString());
        }

        private void listView卖盘_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            string ParaName = string.Format("{0}卖盘列{1}宽度", this.index, e.ColumnIndex);
            Program.accountDataSet.参数.SetParaValue(ParaName, this.listView卖盘.Columns[e.ColumnIndex].Width.ToString());
        }

        private void listView逐笔成交_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            string ParaName = string.Format("{0}逐笔成交列{1}宽度", this.index, e.ColumnIndex);
            Program.accountDataSet.参数.SetParaValue(ParaName, this.listView逐笔成交.Columns[e.ColumnIndex].Width.ToString());
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

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        #region 逐笔过滤 Events
        private void numericUpDownTransMax_ValueChanged(object sender, EventArgs e)
        {
            //保存最大值设置
            var ParaName = string.Format("{0}逐笔过滤最大值", this.index);
            var value = Decimal.Parse(Program.accountDataSet.参数.GetParaValue(ParaName, "0"));
            var newValue = this.numericUpDownTransMax.Value;
            if (newValue != value && newValue >= 0)
            {
                if (MinNum.HasValue && MinNum.Value < newValue)
                {
                    this.numericUpDownTransMin.Maximum = newValue;
                }
                //如果比最小值大，则设置最小值控件的最大值为此值
                Program.accountDataSet.参数.SetParaValue(ParaName, newValue.ToString());
                CompletionByWhiteSpace(0);
                //ResetTranByCache();
            }
        }

        private void numericUpDownTransMin_ValueChanged(object sender, EventArgs e)
        {
            //保存最小值设置
            var ParaName = string.Format("{0}逐笔过滤最小值", this.index);
            var value = Decimal.Parse(Program.accountDataSet.参数.GetParaValue(ParaName, "0"));
            var newValue = this.numericUpDownTransMin.Value;
            if (newValue != value)
            {
                if (MaxNum.HasValue && MaxNum.Value > newValue)
                {
                    this.numericUpDownTransMax.Minimum = newValue;
                }
                //如果比最大值小，则设置最大值控件的最小值为此值。
                Program.accountDataSet.参数.SetParaValue(ParaName, newValue.ToString());
                CompletionByWhiteSpace(0);
                //ResetTranByCache();
            }

        }

        private void ResetTranByCache()
        {//从缓存中获取数据并更新。
            if (transCache.Count > 0)
            {
                var tempTransCache = transCache.Where(_ => ValidateVolumnLimit((decimal)_.Volume / 100));
                transCache.Clear();
                int i = 0;
                lock (sync)
                {
                    foreach (var item in tempTransCache)
                    {
                        transCache.Enqueue(item);
                        decimal Price = (decimal)item.Price / 10000;
                        string Volumn = Math.Round((decimal)item.Volume / 100, 2).ToString("#0");
                        string Time = item.Time.DateTimeFormat();
                        string BS = ((char)item.Flag).ToString();
                        Refresh逐笔成交Item(Price, Volumn, Time, BS, listView逐笔成交Filte.Items[i++]);
                    }
                }

            }
            else if (tranEntityCache.Count > 0)
            {
                tranEntityCache = tranEntityCache.Where(_ => ValidateVolumnLimit(_.Volumn)).ToList();
            }
        }

        private void numericUpDownTransLimit_Move(object sender, EventArgs e)
        {
            if (CodeFocus)
            {
                CodeFocus = false;
            }
        }

        private void numericUpDownTrans_Click(object sender, EventArgs e)
        {
            if (CodeFocus)
            {
                CodeFocus = false;
            }
        }

        private void numericUpDownTrans_Leave(object sender, EventArgs e)
        {
            if (!CodeFocus)
            {
                CodeFocus = true;
            }
        }

        private void numericUpDownTransMin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                this.numericUpDownTransMin.Value = Math.Max(this.numericUpDownTransMin.Minimum, this.numericUpDownTransMin.Value - 1);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Right)
            {
                this.numericUpDownTransMin.Value = Math.Min(this.numericUpDownTransMin.Maximum, this.numericUpDownTransMin.Value + 1);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void numericUpDownTransMax_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                this.numericUpDownTransMax.Value = Math.Max(this.numericUpDownTransMax.Minimum, this.numericUpDownTransMax.Value - 1);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Right)
            {
                this.numericUpDownTransMax.Value = Math.Min(this.numericUpDownTransMax.Maximum, this.numericUpDownTransMax.Value + 1);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void listView逐笔成交Filte_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            string ParaName = string.Format("{0}逐笔成交Filte列{1}宽度", this.index, e.ColumnIndex);
            Program.accountDataSet.参数.SetParaValue(ParaName, this.listView逐笔成交Filte.Columns[e.ColumnIndex].Width.ToString());
        }

        private void listView逐笔成交Filte_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView逐笔成交Filte_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }


        private void CompletionByWhiteSpace(int index)
        {
            for (var i = index; i < 50; i++)
            {
                var item = listView逐笔成交Filte.Items[i];
                item.SubItems[0].Text = "        ";
                item.SubItems[1].Text = "        ";
                item.SubItems[2].Text = "        ";

                var item2 = listView逐笔成交.Items[i];
                item2.SubItems[0].Text = "        ";
                item2.SubItems[1].Text = "        ";
                item2.SubItems[2].Text = "        ";
            }
            for (int i = 0; i < 10; i++)
            {
                var item = listView买盘.Items[i];
                item.SubItems[0].Text = "        ";
                item.SubItems[1].Text = "        ";


                var item1 = listView卖盘.Items[i];
                item1.SubItems[0].Text = "        ";
                item1.SubItems[1].Text = "        ";


            }

        }

        bool ValidateVolumnLimit(decimal volumn)
        {
            if (MinNum.HasValue && volumn < MinNum)
            {
                return false;
            }
            if (MaxNum.HasValue && volumn > MaxNum)
            {
                return false;
            }
            return true;
        }
        #endregion

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (splitContainer3.Panel2Collapsed)
            {
                panelFilter.Visible = true;
                splitContainer3.Panel2Collapsed = false;
                pictureBox1.BackgroundImage = AASClient.Properties.Resources.next;
                Program.accountDataSet.参数.SetParaValue(index + "逐笔过滤显示", "1");
            }
            else
            {
                panelFilter.Visible = false;
                splitContainer3.Panel2Collapsed = true;
                pictureBox1.BackgroundImage = AASClient.Properties.Resources.previous;
                Program.accountDataSet.参数.SetParaValue(index + "逐笔过滤显示", "0");
            }
        }

        private void splitContainer3_SplitterMoved(object sender, SplitterEventArgs e)
        {
            string ParaName3 = string.Format("{0} splitContainer3 SplitterDistance", this.index);
            Program.accountDataSet.参数.SetParaValue(ParaName3, this.splitContainer3.SplitterDistance.ToString());
        }



        AASClient.Model.PublicStock PublicStock = null;
        internal void PossessPublicStock(Model.PublicStock stock)
        {
            PublicStock = stock;
            this.comboBox代码.Text = stock.StockCode;
            //以最高价迅速卖出需要实时价格。

        }
    }

    class MarketTranEntity : IEquatable<MarketTranEntity>
    {
        public string Code { get; set; }

        public string Time { get; set; }

        /// <summary>
        /// 性质：B -> Buy, S-> Sale
        /// </summary>
        public string BS { get; set; }

        public decimal Price { get; set; }

        public decimal Volumn { get; set; }

        public bool Equals(MarketTranEntity other)
        {
            if (Time == other.Time && BS == other.BS && Price == other.Price && Volumn == other.Volumn)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public static class DataUtils
    {
        public static string DateTimeFormat(this int dt)
        {
            if (dt < 10000000)
                return dt.ToString();
            var str = dt.ToString();

            var h = str.Substring(0, str.Length == 9 ? 2 : 1);
            var m = str.Substring(str.Length - 7, 2);
            var s = str.Substring(str.Length - 5, 2);

            return h + ":" + m + ":" + s;
        }
    }
}
