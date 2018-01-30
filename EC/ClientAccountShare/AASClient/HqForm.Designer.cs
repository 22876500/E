namespace AASClient
{
    partial class HqForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.btnSendOrder = new System.Windows.Forms.Button();
            this.comboBox买卖方向 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown数量 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown价格 = new System.Windows.Forms.NumericUpDown();
            this.panel4 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listView买盘 = new AASClient.ListViewNF();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView卖盘 = new AASClient.ListViewNF();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel3 = new System.Windows.Forms.Panel();
            this.listView逐笔成交 = new AASClient.ListViewNF();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label可买股数 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelETH = new System.Windows.Forms.Label();
            this.labelBTC = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.linkLabel默认股数 = new System.Windows.Forms.LinkLabel();
            this.labelUSDT = new System.Windows.Forms.Label();
            this.labelBNB = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox代码 = new System.Windows.Forms.ComboBox();
            this.label可用资金 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label可卖股数 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown数量)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown价格)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 19);
            this.label3.TabIndex = 1;
            this.label3.Text = "价格";
            // 
            // btnSendOrder
            // 
            this.btnSendOrder.Location = new System.Drawing.Point(167, 37);
            this.btnSendOrder.Name = "btnSendOrder";
            this.btnSendOrder.Size = new System.Drawing.Size(121, 30);
            this.btnSendOrder.TabIndex = 0;
            this.btnSendOrder.Text = "下单";
            this.btnSendOrder.UseVisualStyleBackColor = true;
            this.btnSendOrder.Click += new System.EventHandler(this.btnSendOrder_Click);
            // 
            // comboBox买卖方向
            // 
            this.comboBox买卖方向.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox买卖方向.FormattingEnabled = true;
            this.comboBox买卖方向.Items.AddRange(new object[] {
            "买入",
            "卖出"});
            this.comboBox买卖方向.Location = new System.Drawing.Point(167, 8);
            this.comboBox买卖方向.Name = "comboBox买卖方向";
            this.comboBox买卖方向.Size = new System.Drawing.Size(121, 27);
            this.comboBox买卖方向.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 19);
            this.label4.TabIndex = 2;
            this.label4.Text = "股数";
            // 
            // numericUpDown数量
            // 
            this.numericUpDown数量.DecimalPlaces = 4;
            this.numericUpDown数量.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numericUpDown数量.Location = new System.Drawing.Point(55, 40);
            this.numericUpDown数量.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDown数量.Name = "numericUpDown数量";
            this.numericUpDown数量.Size = new System.Drawing.Size(102, 25);
            this.numericUpDown数量.TabIndex = 4;
            this.numericUpDown数量.Click += new System.EventHandler(this.numericUpDown数量_Click);
            this.numericUpDown数量.Enter += new System.EventHandler(this.numericUpDown数量_Enter);
            // 
            // numericUpDown价格
            // 
            this.numericUpDown价格.DecimalPlaces = 8;
            this.numericUpDown价格.Increment = new decimal(new int[] {
            1,
            0,
            0,
            524288});
            this.numericUpDown价格.Location = new System.Drawing.Point(55, 9);
            this.numericUpDown价格.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown价格.Name = "numericUpDown价格";
            this.numericUpDown价格.Size = new System.Drawing.Size(102, 25);
            this.numericUpDown价格.TabIndex = 3;
            this.numericUpDown价格.Click += new System.EventHandler(this.numericUpDown价格_Click);
            this.numericUpDown价格.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown价格_KeyDown);
            this.numericUpDown价格.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericUpDown价格_KeyPress);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.splitContainer1);
            this.panel4.Controls.Add(this.panel1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(566, 393);
            this.panel4.TabIndex = 5;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 61);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView逐笔成交);
            this.splitContainer1.Panel2.Controls.Add(this.splitter1);
            this.splitContainer1.Size = new System.Drawing.Size(566, 332);
            this.splitContainer1.SplitterDistance = 329;
            this.splitContainer1.TabIndex = 3;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listView买盘);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listView卖盘);
            this.splitContainer2.Size = new System.Drawing.Size(329, 259);
            this.splitContainer2.SplitterDistance = 167;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer2_SplitterMoved);
            // 
            // listView买盘
            // 
            this.listView买盘.BackColor = System.Drawing.Color.White;
            this.listView买盘.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader8});
            this.listView买盘.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView买盘.FullRowSelect = true;
            this.listView买盘.Location = new System.Drawing.Point(0, 0);
            this.listView买盘.MultiSelect = false;
            this.listView买盘.Name = "listView买盘";
            this.listView买盘.OwnerDraw = true;
            this.listView买盘.Size = new System.Drawing.Size(167, 259);
            this.listView买盘.TabIndex = 0;
            this.listView买盘.UseCompatibleStateImageBehavior = false;
            this.listView买盘.View = System.Windows.Forms.View.Details;
            this.listView买盘.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.listView买盘_ColumnWidthChanged);
            this.listView买盘.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listView买盘_DrawColumnHeader);
            this.listView买盘.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listView买盘_DrawSubItem);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "价格";
            this.columnHeader1.Width = 86;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "数量";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader2.Width = 76;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "金额";
            this.columnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader8.Width = 70;
            // 
            // listView卖盘
            // 
            this.listView卖盘.BackColor = System.Drawing.Color.White;
            this.listView卖盘.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader9});
            this.listView卖盘.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView卖盘.FullRowSelect = true;
            this.listView卖盘.Location = new System.Drawing.Point(0, 0);
            this.listView卖盘.MultiSelect = false;
            this.listView卖盘.Name = "listView卖盘";
            this.listView卖盘.OwnerDraw = true;
            this.listView卖盘.Size = new System.Drawing.Size(158, 259);
            this.listView卖盘.TabIndex = 1;
            this.listView卖盘.UseCompatibleStateImageBehavior = false;
            this.listView卖盘.View = System.Windows.Forms.View.Details;
            this.listView卖盘.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.listView卖盘_ColumnWidthChanged);
            this.listView卖盘.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listView卖盘_DrawColumnHeader);
            this.listView卖盘.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listView卖盘_DrawSubItem);
            this.listView卖盘.DoubleClick += new System.EventHandler(this.listView卖盘_DoubleClick);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "价格";
            this.columnHeader3.Width = 78;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "数量";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader4.Width = 66;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "金额";
            this.columnHeader9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.btnSendOrder);
            this.panel3.Controls.Add(this.numericUpDown价格);
            this.panel3.Controls.Add(this.comboBox买卖方向);
            this.panel3.Controls.Add(this.numericUpDown数量);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 259);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(329, 73);
            this.panel3.TabIndex = 1;
            this.panel3.Visible = false;
            this.panel3.VisibleChanged += new System.EventHandler(this.panel3_VisibleChanged);
            // 
            // listView逐笔成交
            // 
            this.listView逐笔成交.BackColor = System.Drawing.Color.White;
            this.listView逐笔成交.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.listView逐笔成交.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView逐笔成交.FullRowSelect = true;
            this.listView逐笔成交.Location = new System.Drawing.Point(3, 0);
            this.listView逐笔成交.MultiSelect = false;
            this.listView逐笔成交.Name = "listView逐笔成交";
            this.listView逐笔成交.OwnerDraw = true;
            this.listView逐笔成交.Size = new System.Drawing.Size(230, 332);
            this.listView逐笔成交.TabIndex = 5;
            this.listView逐笔成交.UseCompatibleStateImageBehavior = false;
            this.listView逐笔成交.View = System.Windows.Forms.View.Details;
            this.listView逐笔成交.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.listView逐笔成交_ColumnWidthChanged);
            this.listView逐笔成交.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listView逐笔成交_DrawColumnHeader);
            this.listView逐笔成交.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listView逐笔成交_DrawSubItem);
            this.listView逐笔成交.DoubleClick += new System.EventHandler(this.listView逐笔成交_DoubleClick);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "价格";
            this.columnHeader5.Width = 78;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "数量";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader6.Width = 66;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "时间";
            this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader7.Width = 76;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 332);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.linkLabel1);
            this.panel1.Controls.Add(this.label可买股数);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.labelETH);
            this.panel1.Controls.Add(this.labelBTC);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.linkLabel默认股数);
            this.panel1.Controls.Add(this.labelUSDT);
            this.panel1.Controls.Add(this.labelBNB);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.comboBox代码);
            this.panel1.Controls.Add(this.label可用资金);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label可卖股数);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(566, 61);
            this.panel1.TabIndex = 2;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel1.Location = new System.Drawing.Point(457, 39);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(32, 17);
            this.linkLabel1.TabIndex = 34;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "刷新";
            this.linkLabel1.Click += new System.EventHandler(this.linkLabelRefreshCoinInfo_Click);
            // 
            // label可买股数
            // 
            this.label可买股数.AutoSize = true;
            this.label可买股数.Location = new System.Drawing.Point(66, 39);
            this.label可买股数.Name = "label可买股数";
            this.label可买股数.Size = new System.Drawing.Size(15, 17);
            this.label可买股数.TabIndex = 33;
            this.label可买股数.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 17);
            this.label2.TabIndex = 32;
            this.label2.Text = "可买股数:";
            // 
            // labelETH
            // 
            this.labelETH.AutoSize = true;
            this.labelETH.ForeColor = System.Drawing.Color.Green;
            this.labelETH.Location = new System.Drawing.Point(274, 22);
            this.labelETH.Name = "labelETH";
            this.labelETH.Size = new System.Drawing.Size(39, 17);
            this.labelETH.TabIndex = 31;
            this.labelETH.Text = "0.000";
            this.labelETH.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelBTC
            // 
            this.labelBTC.AutoSize = true;
            this.labelBTC.ForeColor = System.Drawing.Color.Red;
            this.labelBTC.Location = new System.Drawing.Point(274, 5);
            this.labelBTC.Name = "labelBTC";
            this.labelBTC.Size = new System.Drawing.Size(39, 17);
            this.labelBTC.TabIndex = 30;
            this.labelBTC.Text = "0.000";
            this.labelBTC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Green;
            this.label10.Location = new System.Drawing.Point(246, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 17);
            this.label10.TabIndex = 29;
            this.label10.Text = "ETH:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(245, 5);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(34, 17);
            this.label11.TabIndex = 28;
            this.label11.Text = "BTC:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkLabel默认股数
            // 
            this.linkLabel默认股数.AutoSize = true;
            this.linkLabel默认股数.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel默认股数.Location = new System.Drawing.Point(324, 40);
            this.linkLabel默认股数.Name = "linkLabel默认股数";
            this.linkLabel默认股数.Size = new System.Drawing.Size(127, 17);
            this.linkLabel默认股数.TabIndex = 27;
            this.linkLabel默认股数.TabStop = true;
            this.linkLabel默认股数.Text = "Shares: 0 MaxAmt: 0";
            this.linkLabel默认股数.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel默认股数_LinkClicked);
            // 
            // labelUSDT
            // 
            this.labelUSDT.AutoSize = true;
            this.labelUSDT.ForeColor = System.Drawing.Color.Green;
            this.labelUSDT.Location = new System.Drawing.Point(412, 22);
            this.labelUSDT.Name = "labelUSDT";
            this.labelUSDT.Size = new System.Drawing.Size(39, 17);
            this.labelUSDT.TabIndex = 26;
            this.labelUSDT.Text = "0.000";
            this.labelUSDT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelBNB
            // 
            this.labelBNB.AutoSize = true;
            this.labelBNB.ForeColor = System.Drawing.Color.Red;
            this.labelBNB.Location = new System.Drawing.Point(412, 5);
            this.labelBNB.Name = "labelBNB";
            this.labelBNB.Size = new System.Drawing.Size(39, 17);
            this.labelBNB.TabIndex = 25;
            this.labelBNB.Text = "0.000";
            this.labelBNB.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Green;
            this.label7.Location = new System.Drawing.Point(374, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 17);
            this.label7.TabIndex = 24;
            this.label7.Text = "USDT:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(380, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 17);
            this.label6.TabIndex = 23;
            this.label6.Text = "BNB:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBox代码
            // 
            this.comboBox代码.FormattingEnabled = true;
            this.comboBox代码.Location = new System.Drawing.Point(69, 12);
            this.comboBox代码.MaxLength = 100;
            this.comboBox代码.Name = "comboBox代码";
            this.comboBox代码.Size = new System.Drawing.Size(142, 25);
            this.comboBox代码.TabIndex = 18;
            this.comboBox代码.TextChanged += new System.EventHandler(this.comboBox代码_TextChanged);
            this.comboBox代码.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox代码_KeyDown);
            this.comboBox代码.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox代码_KeyPress);
            this.comboBox代码.Leave += new System.EventHandler(this.comboBox代码_Leave);
            // 
            // label可用资金
            // 
            this.label可用资金.AutoSize = true;
            this.label可用资金.Location = new System.Drawing.Point(494, 37);
            this.label可用资金.Name = "label可用资金";
            this.label可用资金.Size = new System.Drawing.Size(15, 17);
            this.label可用资金.TabIndex = 16;
            this.label可用资金.Text = "0";
            this.label可用资金.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "证券代码:";
            // 
            // label可卖股数
            // 
            this.label可卖股数.AutoSize = true;
            this.label可卖股数.Location = new System.Drawing.Point(228, 39);
            this.label可卖股数.Name = "label可卖股数";
            this.label可卖股数.Size = new System.Drawing.Size(15, 17);
            this.label可卖股数.TabIndex = 14;
            this.label可卖股数.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(174, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 17);
            this.label5.TabIndex = 13;
            this.label5.Text = "可卖股数:";
            // 
            // HqForm
            // 
            this.AcceptButton = this.btnSendOrder;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 393);
            this.Controls.Add(this.panel4);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.KeyPreview = true;
            this.Name = "HqForm";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Float;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "[]";
            this.DockStateChanged += new System.EventHandler(this.HqForm_DockStateChanged);
            this.Activated += new System.EventHandler(this.HqForm_Activated);
            this.Deactivate += new System.EventHandler(this.HqForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HqForm_FormClosing);
            this.Load += new System.EventHandler(this.HqForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HqForm_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HqForm_KeyPress);
            this.Leave += new System.EventHandler(this.HqForm_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown数量)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown价格)).EndInit();
            this.panel4.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private ListViewNF listView卖盘;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ComboBox comboBox买卖方向;
        private System.Windows.Forms.NumericUpDown numericUpDown数量;
        private System.Windows.Forms.NumericUpDown numericUpDown价格;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSendOrder;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private ListViewNF listView买盘;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.LinkLabel linkLabel默认股数;
        public System.Windows.Forms.ComboBox comboBox代码;
        private System.Windows.Forms.Label label可用资金;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label可卖股数;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Splitter splitter1;
        private ListViewNF listView逐笔成交;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.Label label可买股数;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelETH;
        private System.Windows.Forms.Label labelBTC;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelUSDT;
        private System.Windows.Forms.Label labelBNB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}