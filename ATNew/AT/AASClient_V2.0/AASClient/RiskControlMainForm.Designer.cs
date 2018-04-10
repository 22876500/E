namespace AASClient
{
    partial class RiskControlMainForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip交易额度 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.显示全部ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip交易日志 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.刷新交易日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker报价 = new System.ComponentModel.BackgroundWorker();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel时间 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelConnect = new System.Windows.Forms.ToolStripStatusLabel();
            this.backgroundWorker行情 = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.行情服务器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.当日成交ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.委托记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.当日平仓ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timerConnector = new System.Windows.Forms.Timer(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPage监控对象 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView交易员 = new System.Windows.Forms.DataGridView();
            this.用户名 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.仓位限制 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.亏损限制 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView交易额度 = new System.Windows.Forms.DataGridView();
            this.tabPage当前仓位 = new System.Windows.Forms.TabPage();
            this.dataGridView当前仓位 = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label实现盈亏 = new System.Windows.Forms.Label();
            this.label浮动盈亏 = new System.Windows.Forms.Label();
            this.label市值合计 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage当前委托 = new System.Windows.Forms.TabPage();
            this.dataGridView当前委托 = new System.Windows.Forms.DataGridView();
            this.tabPage业绩统计 = new System.Windows.Forms.TabPage();
            this.dataGridView业绩统计 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton按组合号 = new System.Windows.Forms.RadioButton();
            this.radioButton按交易员 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button统计 = new System.Windows.Forms.Button();
            this.dateTimePicker结束日期 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker开始日期 = new System.Windows.Forms.DateTimePicker();
            this.tabPage组合仓位 = new System.Windows.Forms.TabPage();
            this.dataGridView组合仓位 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage额度共享 = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.dataGridView额度分配交易员 = new System.Windows.Forms.DataGridView();
            this.ColumnTraderAccount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView额度分配股票 = new System.Windows.Forms.DataGridView();
            this.ColumnGroupAccount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStockID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStockName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLimitCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCommission = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnBuyType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSaleType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonSearchShareLimit = new System.Windows.Forms.Button();
            this.comboBoxShareLimitGroup = new System.Windows.Forms.ComboBox();
            this.tabPage可用仓位 = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.ucPositionShow1 = new AASClient.UC.ucPositionShow();
            this.tabPage交易员配置 = new System.Windows.Forms.TabPage();
            this.ucUserAccountSet1 = new AASClient.UC.ucUserAccountSet();
            this.tabPage额度管理 = new System.Windows.Forms.TabPage();
            this.ucStockLimitManage1 = new AASClient.UC.ucStockLimitManage();
            this.tabPage交易员管理 = new System.Windows.Forms.TabPage();
            this.ucTraderManager1 = new AASClient.UC.ucTraderManager();
            this.tabPage运行日志 = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dataGridView运行日志 = new System.Windows.Forms.DataGridView();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.bindingSource交易员 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource当前仓位 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource当前委托 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource当前成交 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource当日统计 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource委托记录 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource交易额度 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource组合仓位 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource运行日志 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource额度共享股票 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource额度共享交易员 = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip当前仓位 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip交易额度.SuspendLayout();
            this.contextMenuStrip交易日志.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPage监控对象.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView交易员)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView交易额度)).BeginInit();
            this.tabPage当前仓位.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView当前仓位)).BeginInit();
            this.panel2.SuspendLayout();
            this.tabPage当前委托.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView当前委托)).BeginInit();
            this.tabPage业绩统计.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView业绩统计)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabPage组合仓位.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView组合仓位)).BeginInit();
            this.tabPage额度共享.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView额度分配交易员)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView额度分配股票)).BeginInit();
            this.panel3.SuspendLayout();
            this.tabPage可用仓位.SuspendLayout();
            this.panel6.SuspendLayout();
            this.tabPage交易员配置.SuspendLayout();
            this.tabPage额度管理.SuspendLayout();
            this.tabPage交易员管理.SuspendLayout();
            this.tabPage运行日志.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView运行日志)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource交易员)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource当前仓位)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource当前委托)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource当前成交)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource当日统计)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource委托记录)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource交易额度)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource组合仓位)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource运行日志)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource额度共享股票)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource额度共享交易员)).BeginInit();
            this.contextMenuStrip当前仓位.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip交易额度
            // 
            this.contextMenuStrip交易额度.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示全部ToolStripMenuItem});
            this.contextMenuStrip交易额度.Name = "contextMenuStrip2";
            this.contextMenuStrip交易额度.Size = new System.Drawing.Size(125, 26);
            // 
            // 显示全部ToolStripMenuItem
            // 
            this.显示全部ToolStripMenuItem.Name = "显示全部ToolStripMenuItem";
            this.显示全部ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.显示全部ToolStripMenuItem.Text = "显示全部";
            this.显示全部ToolStripMenuItem.Click += new System.EventHandler(this.显示全部ToolStripMenuItem_Click);
            // 
            // contextMenuStrip交易日志
            // 
            this.contextMenuStrip交易日志.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.刷新交易日志ToolStripMenuItem});
            this.contextMenuStrip交易日志.Name = "contextMenuStrip1";
            this.contextMenuStrip交易日志.Size = new System.Drawing.Size(149, 26);
            // 
            // 刷新交易日志ToolStripMenuItem
            // 
            this.刷新交易日志ToolStripMenuItem.Name = "刷新交易日志ToolStripMenuItem";
            this.刷新交易日志ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.刷新交易日志ToolStripMenuItem.Text = "刷新交易日志";
            this.刷新交易日志ToolStripMenuItem.Click += new System.EventHandler(this.刷新交易日志ToolStripMenuItem_Click);
            // 
            // backgroundWorker报价
            // 
            this.backgroundWorker报价.WorkerReportsProgress = true;
            this.backgroundWorker报价.WorkerSupportsCancellation = true;
            this.backgroundWorker报价.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker报价_DoWork);
            this.backgroundWorker报价.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker报价_ProgressChanged);
            this.backgroundWorker报价.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker报价_RunWorkerCompleted);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel时间,
            this.toolStripStatusLabelConnect});
            this.statusStrip1.Location = new System.Drawing.Point(0, 558);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(984, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel时间
            // 
            this.toolStripStatusLabel时间.Name = "toolStripStatusLabel时间";
            this.toolStripStatusLabel时间.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabel时间.Text = "00:00:00";
            // 
            // toolStripStatusLabelConnect
            // 
            this.toolStripStatusLabelConnect.Name = "toolStripStatusLabelConnect";
            this.toolStripStatusLabelConnect.Size = new System.Drawing.Size(911, 17);
            this.toolStripStatusLabelConnect.Spring = true;
            this.toolStripStatusLabelConnect.Text = "连接状态：已连接";
            this.toolStripStatusLabelConnect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // backgroundWorker行情
            // 
            this.backgroundWorker行情.WorkerReportsProgress = true;
            this.backgroundWorker行情.WorkerSupportsCancellation = true;
            this.backgroundWorker行情.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker行情_DoWork);
            this.backgroundWorker行情.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker行情_ProgressChanged);
            this.backgroundWorker行情.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker行情_RunWorkerCompleted);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem,
            this.数据查询ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(984, 27);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.行情服务器ToolStripMenuItem});
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem.Text = "设置";
            // 
            // 行情服务器ToolStripMenuItem
            // 
            this.行情服务器ToolStripMenuItem.Name = "行情服务器ToolStripMenuItem";
            this.行情服务器ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.行情服务器ToolStripMenuItem.Text = "行情服务器";
            this.行情服务器ToolStripMenuItem.Click += new System.EventHandler(this.行情服务器ToolStripMenuItem_Click);
            // 
            // 数据查询ToolStripMenuItem
            // 
            this.数据查询ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.当日成交ToolStripMenuItem,
            this.委托记录ToolStripMenuItem,
            this.当日平仓ToolStripMenuItem});
            this.数据查询ToolStripMenuItem.Name = "数据查询ToolStripMenuItem";
            this.数据查询ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.数据查询ToolStripMenuItem.Text = "数据查询";
            // 
            // 当日成交ToolStripMenuItem
            // 
            this.当日成交ToolStripMenuItem.Name = "当日成交ToolStripMenuItem";
            this.当日成交ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.当日成交ToolStripMenuItem.Text = "当前成交";
            this.当日成交ToolStripMenuItem.Click += new System.EventHandler(this.当日成交ToolStripMenuItem_Click);
            // 
            // 委托记录ToolStripMenuItem
            // 
            this.委托记录ToolStripMenuItem.Name = "委托记录ToolStripMenuItem";
            this.委托记录ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.委托记录ToolStripMenuItem.Text = "委托记录";
            this.委托记录ToolStripMenuItem.Click += new System.EventHandler(this.委托记录ToolStripMenuItem_Click);
            // 
            // 当日平仓ToolStripMenuItem
            // 
            this.当日平仓ToolStripMenuItem.Name = "当日平仓ToolStripMenuItem";
            this.当日平仓ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.当日平仓ToolStripMenuItem.Text = "当日平仓";
            this.当日平仓ToolStripMenuItem.Click += new System.EventHandler(this.当日平仓ToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timerConnector
            // 
            this.timerConnector.Enabled = true;
            this.timerConnector.Interval = 3000;
            this.timerConnector.Tick += new System.EventHandler(this.timerConnector_Tick);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 27);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControlMain);
            this.splitContainer2.Panel2Collapsed = true;
            this.splitContainer2.Size = new System.Drawing.Size(984, 531);
            this.splitContainer2.SplitterDistance = 303;
            this.splitContainer2.SplitterWidth = 6;
            this.splitContainer2.TabIndex = 1;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPage监控对象);
            this.tabControlMain.Controls.Add(this.tabPage当前仓位);
            this.tabControlMain.Controls.Add(this.tabPage当前委托);
            this.tabControlMain.Controls.Add(this.tabPage业绩统计);
            this.tabControlMain.Controls.Add(this.tabPage组合仓位);
            this.tabControlMain.Controls.Add(this.tabPage额度共享);
            this.tabControlMain.Controls.Add(this.tabPage可用仓位);
            this.tabControlMain.Controls.Add(this.tabPage交易员配置);
            this.tabControlMain.Controls.Add(this.tabPage额度管理);
            this.tabControlMain.Controls.Add(this.tabPage交易员管理);
            this.tabControlMain.Controls.Add(this.tabPage运行日志);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(984, 531);
            this.tabControlMain.TabIndex = 0;
            this.tabControlMain.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage监控对象
            // 
            this.tabPage监控对象.Controls.Add(this.splitContainer1);
            this.tabPage监控对象.Location = new System.Drawing.Point(4, 26);
            this.tabPage监控对象.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage监控对象.Name = "tabPage监控对象";
            this.tabPage监控对象.Size = new System.Drawing.Size(976, 501);
            this.tabPage监控对象.TabIndex = 5;
            this.tabPage监控对象.Text = "监控对象";
            this.tabPage监控对象.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView交易员);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView交易额度);
            this.splitContainer1.Size = new System.Drawing.Size(976, 501);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 1;
            // 
            // dataGridView交易员
            // 
            this.dataGridView交易员.AllowUserToAddRows = false;
            this.dataGridView交易员.AllowUserToDeleteRows = false;
            this.dataGridView交易员.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView交易员.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView交易员.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.用户名,
            this.仓位限制,
            this.亏损限制});
            this.dataGridView交易员.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView交易员.Location = new System.Drawing.Point(0, 0);
            this.dataGridView交易员.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView交易员.Name = "dataGridView交易员";
            this.dataGridView交易员.ReadOnly = true;
            this.dataGridView交易员.RowHeadersVisible = false;
            this.dataGridView交易员.RowTemplate.Height = 23;
            this.dataGridView交易员.Size = new System.Drawing.Size(300, 501);
            this.dataGridView交易员.TabIndex = 0;
            this.dataGridView交易员.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView交易员_CellClick);
            this.dataGridView交易员.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView交易员_CellDoubleClick);
            this.dataGridView交易员.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView交易员_CellFormatting);
            // 
            // 用户名
            // 
            this.用户名.DataPropertyName = "用户名";
            this.用户名.HeaderText = "用户名";
            this.用户名.Name = "用户名";
            this.用户名.ReadOnly = true;
            this.用户名.Width = 69;
            // 
            // 仓位限制
            // 
            this.仓位限制.DataPropertyName = "仓位限制";
            this.仓位限制.HeaderText = "仓位限制";
            this.仓位限制.Name = "仓位限制";
            this.仓位限制.ReadOnly = true;
            this.仓位限制.Width = 81;
            // 
            // 亏损限制
            // 
            this.亏损限制.DataPropertyName = "亏损限制";
            this.亏损限制.HeaderText = "亏损限制";
            this.亏损限制.Name = "亏损限制";
            this.亏损限制.ReadOnly = true;
            this.亏损限制.Width = 81;
            // 
            // dataGridView交易额度
            // 
            this.dataGridView交易额度.AllowUserToAddRows = false;
            this.dataGridView交易额度.AllowUserToDeleteRows = false;
            this.dataGridView交易额度.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView交易额度.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView交易额度.ContextMenuStrip = this.contextMenuStrip交易额度;
            this.dataGridView交易额度.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView交易额度.Location = new System.Drawing.Point(0, 0);
            this.dataGridView交易额度.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView交易额度.Name = "dataGridView交易额度";
            this.dataGridView交易额度.ReadOnly = true;
            this.dataGridView交易额度.RowHeadersVisible = false;
            this.dataGridView交易额度.RowTemplate.Height = 23;
            this.dataGridView交易额度.Size = new System.Drawing.Size(671, 501);
            this.dataGridView交易额度.TabIndex = 1;
            this.dataGridView交易额度.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView交易额度_CellDoubleClick);
            this.dataGridView交易额度.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView交易额度_CellFormatting);
            // 
            // tabPage当前仓位
            // 
            this.tabPage当前仓位.Controls.Add(this.dataGridView当前仓位);
            this.tabPage当前仓位.Controls.Add(this.panel2);
            this.tabPage当前仓位.Location = new System.Drawing.Point(4, 26);
            this.tabPage当前仓位.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage当前仓位.Name = "tabPage当前仓位";
            this.tabPage当前仓位.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage当前仓位.Size = new System.Drawing.Size(976, 501);
            this.tabPage当前仓位.TabIndex = 0;
            this.tabPage当前仓位.Text = "当前仓位";
            this.tabPage当前仓位.UseVisualStyleBackColor = true;
            // 
            // dataGridView当前仓位
            // 
            this.dataGridView当前仓位.AllowUserToAddRows = false;
            this.dataGridView当前仓位.AllowUserToDeleteRows = false;
            this.dataGridView当前仓位.AllowUserToResizeRows = false;
            this.dataGridView当前仓位.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView当前仓位.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView当前仓位.ContextMenuStrip = this.contextMenuStrip当前仓位;
            this.dataGridView当前仓位.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView当前仓位.Location = new System.Drawing.Point(3, 36);
            this.dataGridView当前仓位.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView当前仓位.Name = "dataGridView当前仓位";
            this.dataGridView当前仓位.ReadOnly = true;
            this.dataGridView当前仓位.RowHeadersVisible = false;
            this.dataGridView当前仓位.RowTemplate.Height = 23;
            this.dataGridView当前仓位.Size = new System.Drawing.Size(970, 461);
            this.dataGridView当前仓位.TabIndex = 0;
            this.dataGridView当前仓位.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView当前仓位_CellDoubleClick);
            this.dataGridView当前仓位.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView当前仓位_CellFormatting);
            this.dataGridView当前仓位.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView当前仓位_DataError);
            this.dataGridView当前仓位.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView当前仓位_RowPrePaint);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label实现盈亏);
            this.panel2.Controls.Add(this.label浮动盈亏);
            this.panel2.Controls.Add(this.label市值合计);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 4);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(970, 32);
            this.panel2.TabIndex = 1;
            // 
            // label实现盈亏
            // 
            this.label实现盈亏.AutoSize = true;
            this.label实现盈亏.Location = new System.Drawing.Point(545, 7);
            this.label实现盈亏.Name = "label实现盈亏";
            this.label实现盈亏.Size = new System.Drawing.Size(15, 17);
            this.label实现盈亏.TabIndex = 11;
            this.label实现盈亏.Text = "0";
            // 
            // label浮动盈亏
            // 
            this.label浮动盈亏.AutoSize = true;
            this.label浮动盈亏.Location = new System.Drawing.Point(338, 7);
            this.label浮动盈亏.Name = "label浮动盈亏";
            this.label浮动盈亏.Size = new System.Drawing.Size(15, 17);
            this.label浮动盈亏.TabIndex = 10;
            this.label浮动盈亏.Text = "0";
            // 
            // label市值合计
            // 
            this.label市值合计.AutoSize = true;
            this.label市值合计.Location = new System.Drawing.Point(101, 7);
            this.label市值合计.Name = "label市值合计";
            this.label市值合计.Size = new System.Drawing.Size(15, 17);
            this.label市值合计.TabIndex = 9;
            this.label市值合计.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(462, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "实现盈亏";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(255, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "浮动盈亏";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 17);
            this.label6.TabIndex = 6;
            this.label6.Text = "市值合计";
            // 
            // tabPage当前委托
            // 
            this.tabPage当前委托.Controls.Add(this.dataGridView当前委托);
            this.tabPage当前委托.Location = new System.Drawing.Point(4, 26);
            this.tabPage当前委托.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage当前委托.Name = "tabPage当前委托";
            this.tabPage当前委托.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage当前委托.Size = new System.Drawing.Size(976, 501);
            this.tabPage当前委托.TabIndex = 1;
            this.tabPage当前委托.Text = "当前委托";
            this.tabPage当前委托.UseVisualStyleBackColor = true;
            // 
            // dataGridView当前委托
            // 
            this.dataGridView当前委托.AllowUserToAddRows = false;
            this.dataGridView当前委托.AllowUserToDeleteRows = false;
            this.dataGridView当前委托.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView当前委托.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView当前委托.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView当前委托.Location = new System.Drawing.Point(3, 4);
            this.dataGridView当前委托.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView当前委托.Name = "dataGridView当前委托";
            this.dataGridView当前委托.ReadOnly = true;
            this.dataGridView当前委托.RowHeadersVisible = false;
            this.dataGridView当前委托.RowTemplate.Height = 23;
            this.dataGridView当前委托.Size = new System.Drawing.Size(970, 493);
            this.dataGridView当前委托.TabIndex = 1;
            this.dataGridView当前委托.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView当前委托_CellDoubleClick);
            this.dataGridView当前委托.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView当前委托_CellFormatting);
            this.dataGridView当前委托.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView当前委托_RowPrePaint);
            // 
            // tabPage业绩统计
            // 
            this.tabPage业绩统计.Controls.Add(this.dataGridView业绩统计);
            this.tabPage业绩统计.Controls.Add(this.panel1);
            this.tabPage业绩统计.Location = new System.Drawing.Point(4, 26);
            this.tabPage业绩统计.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage业绩统计.Name = "tabPage业绩统计";
            this.tabPage业绩统计.Size = new System.Drawing.Size(976, 501);
            this.tabPage业绩统计.TabIndex = 6;
            this.tabPage业绩统计.Text = "业绩统计";
            this.tabPage业绩统计.UseVisualStyleBackColor = true;
            // 
            // dataGridView业绩统计
            // 
            this.dataGridView业绩统计.AllowUserToAddRows = false;
            this.dataGridView业绩统计.AllowUserToDeleteRows = false;
            this.dataGridView业绩统计.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView业绩统计.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView业绩统计.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView业绩统计.Location = new System.Drawing.Point(0, 0);
            this.dataGridView业绩统计.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView业绩统计.Name = "dataGridView业绩统计";
            this.dataGridView业绩统计.ReadOnly = true;
            this.dataGridView业绩统计.RowHeadersVisible = false;
            this.dataGridView业绩统计.RowTemplate.Height = 23;
            this.dataGridView业绩统计.Size = new System.Drawing.Size(698, 501);
            this.dataGridView业绩统计.TabIndex = 1;
            this.dataGridView业绩统计.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView业绩统计_CellFormatting);
            this.dataGridView业绩统计.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView业绩统计_RowPrePaint);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton按组合号);
            this.panel1.Controls.Add(this.radioButton按交易员);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button统计);
            this.panel1.Controls.Add(this.dateTimePicker结束日期);
            this.panel1.Controls.Add(this.dateTimePicker开始日期);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(698, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(278, 501);
            this.panel1.TabIndex = 0;
            // 
            // radioButton按组合号
            // 
            this.radioButton按组合号.AutoSize = true;
            this.radioButton按组合号.Location = new System.Drawing.Point(145, 101);
            this.radioButton按组合号.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton按组合号.Name = "radioButton按组合号";
            this.radioButton按组合号.Size = new System.Drawing.Size(74, 21);
            this.radioButton按组合号.TabIndex = 10;
            this.radioButton按组合号.Text = "按组合号";
            this.radioButton按组合号.UseVisualStyleBackColor = true;
            // 
            // radioButton按交易员
            // 
            this.radioButton按交易员.AutoSize = true;
            this.radioButton按交易员.Checked = true;
            this.radioButton按交易员.Location = new System.Drawing.Point(30, 101);
            this.radioButton按交易员.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton按交易员.Name = "radioButton按交易员";
            this.radioButton按交易员.Size = new System.Drawing.Size(74, 21);
            this.radioButton按交易员.TabIndex = 9;
            this.radioButton按交易员.TabStop = true;
            this.radioButton按交易员.Text = "按交易员";
            this.radioButton按交易员.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "结束日期";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "开始日期";
            // 
            // button统计
            // 
            this.button统计.Location = new System.Drawing.Point(99, 157);
            this.button统计.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button统计.Name = "button统计";
            this.button统计.Size = new System.Drawing.Size(87, 33);
            this.button统计.TabIndex = 6;
            this.button统计.Text = "统计";
            this.button统计.UseVisualStyleBackColor = true;
            this.button统计.Click += new System.EventHandler(this.button统计_Click);
            // 
            // dateTimePicker结束日期
            // 
            this.dateTimePicker结束日期.Location = new System.Drawing.Point(99, 59);
            this.dateTimePicker结束日期.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePicker结束日期.Name = "dateTimePicker结束日期";
            this.dateTimePicker结束日期.Size = new System.Drawing.Size(157, 23);
            this.dateTimePicker结束日期.TabIndex = 5;
            // 
            // dateTimePicker开始日期
            // 
            this.dateTimePicker开始日期.Location = new System.Drawing.Point(99, 10);
            this.dateTimePicker开始日期.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePicker开始日期.Name = "dateTimePicker开始日期";
            this.dateTimePicker开始日期.Size = new System.Drawing.Size(157, 23);
            this.dateTimePicker开始日期.TabIndex = 4;
            // 
            // tabPage组合仓位
            // 
            this.tabPage组合仓位.Controls.Add(this.dataGridView组合仓位);
            this.tabPage组合仓位.Location = new System.Drawing.Point(4, 26);
            this.tabPage组合仓位.Name = "tabPage组合仓位";
            this.tabPage组合仓位.Size = new System.Drawing.Size(976, 501);
            this.tabPage组合仓位.TabIndex = 7;
            this.tabPage组合仓位.Text = "组合仓位";
            this.tabPage组合仓位.UseVisualStyleBackColor = true;
            // 
            // dataGridView组合仓位
            // 
            this.dataGridView组合仓位.AllowUserToAddRows = false;
            this.dataGridView组合仓位.AllowUserToDeleteRows = false;
            this.dataGridView组合仓位.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView组合仓位.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView组合仓位.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            this.dataGridView组合仓位.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView组合仓位.Location = new System.Drawing.Point(0, 0);
            this.dataGridView组合仓位.Name = "dataGridView组合仓位";
            this.dataGridView组合仓位.ReadOnly = true;
            this.dataGridView组合仓位.RowHeadersVisible = false;
            this.dataGridView组合仓位.RowTemplate.Height = 23;
            this.dataGridView组合仓位.Size = new System.Drawing.Size(976, 501);
            this.dataGridView组合仓位.TabIndex = 0;
            this.dataGridView组合仓位.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView组合仓位_CellFormatting);
            this.dataGridView组合仓位.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView组合仓位_RowPrePaint);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "组合号";
            this.Column1.HeaderText = "组合号";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 69;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "证券代码";
            this.Column2.HeaderText = "证券代码";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 81;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "证券名称";
            this.Column3.HeaderText = "证券名称";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 81;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "开仓类别";
            this.Column4.HeaderText = "开仓类别";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 81;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "已开数量";
            dataGridViewCellStyle4.Format = "f0";
            this.Column5.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column5.HeaderText = "已开数量";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 81;
            // 
            // tabPage额度共享
            // 
            this.tabPage额度共享.Controls.Add(this.panel4);
            this.tabPage额度共享.Controls.Add(this.panel3);
            this.tabPage额度共享.Location = new System.Drawing.Point(4, 26);
            this.tabPage额度共享.Name = "tabPage额度共享";
            this.tabPage额度共享.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage额度共享.Size = new System.Drawing.Size(976, 501);
            this.tabPage额度共享.TabIndex = 8;
            this.tabPage额度共享.Text = "共享额度";
            this.tabPage额度共享.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.splitContainer4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 59);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(970, 439);
            this.panel4.TabIndex = 1;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.dataGridView额度分配交易员);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.dataGridView额度分配股票);
            this.splitContainer4.Size = new System.Drawing.Size(970, 439);
            this.splitContainer4.SplitterDistance = 167;
            this.splitContainer4.TabIndex = 0;
            // 
            // dataGridView额度分配交易员
            // 
            this.dataGridView额度分配交易员.AllowUserToAddRows = false;
            this.dataGridView额度分配交易员.AllowUserToDeleteRows = false;
            this.dataGridView额度分配交易员.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView额度分配交易员.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTraderAccount});
            this.dataGridView额度分配交易员.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView额度分配交易员.Location = new System.Drawing.Point(0, 0);
            this.dataGridView额度分配交易员.Name = "dataGridView额度分配交易员";
            this.dataGridView额度分配交易员.RowTemplate.Height = 23;
            this.dataGridView额度分配交易员.Size = new System.Drawing.Size(167, 439);
            this.dataGridView额度分配交易员.TabIndex = 0;
            // 
            // ColumnTraderAccount
            // 
            this.ColumnTraderAccount.DataPropertyName = "TraderAccount";
            this.ColumnTraderAccount.HeaderText = "用户名";
            this.ColumnTraderAccount.Name = "ColumnTraderAccount";
            // 
            // dataGridView额度分配股票
            // 
            this.dataGridView额度分配股票.AllowUserToAddRows = false;
            this.dataGridView额度分配股票.AllowUserToDeleteRows = false;
            this.dataGridView额度分配股票.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView额度分配股票.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnGroupAccount,
            this.ColumnStockID,
            this.ColumnStockName,
            this.ColumnLimitCount,
            this.ColumnCommission,
            this.ColumnBuyType,
            this.ColumnSaleType});
            this.dataGridView额度分配股票.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView额度分配股票.Location = new System.Drawing.Point(0, 0);
            this.dataGridView额度分配股票.Name = "dataGridView额度分配股票";
            this.dataGridView额度分配股票.RowTemplate.Height = 23;
            this.dataGridView额度分配股票.Size = new System.Drawing.Size(799, 439);
            this.dataGridView额度分配股票.TabIndex = 0;
            this.dataGridView额度分配股票.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView额度分配股票_CellFormatting);
            // 
            // ColumnGroupAccount
            // 
            this.ColumnGroupAccount.DataPropertyName = "GroupAccount";
            this.ColumnGroupAccount.HeaderText = "组合号";
            this.ColumnGroupAccount.Name = "ColumnGroupAccount";
            // 
            // ColumnStockID
            // 
            this.ColumnStockID.DataPropertyName = "StockID";
            this.ColumnStockID.HeaderText = "股票代码";
            this.ColumnStockID.Name = "ColumnStockID";
            // 
            // ColumnStockName
            // 
            this.ColumnStockName.DataPropertyName = "StockName";
            this.ColumnStockName.HeaderText = "股票名称";
            this.ColumnStockName.Name = "ColumnStockName";
            // 
            // ColumnLimitCount
            // 
            this.ColumnLimitCount.DataPropertyName = "LimitCount";
            this.ColumnLimitCount.HeaderText = "额度";
            this.ColumnLimitCount.Name = "ColumnLimitCount";
            // 
            // ColumnCommission
            // 
            this.ColumnCommission.DataPropertyName = "Commission";
            this.ColumnCommission.HeaderText = "手续费率";
            this.ColumnCommission.Name = "ColumnCommission";
            // 
            // ColumnBuyType
            // 
            this.ColumnBuyType.DataPropertyName = "BuyType";
            this.ColumnBuyType.HeaderText = "买方式";
            this.ColumnBuyType.Name = "ColumnBuyType";
            // 
            // ColumnSaleType
            // 
            this.ColumnSaleType.DataPropertyName = "SaleType";
            this.ColumnSaleType.HeaderText = "卖方式";
            this.ColumnSaleType.Name = "ColumnSaleType";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.buttonSearchShareLimit);
            this.panel3.Controls.Add(this.comboBoxShareLimitGroup);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(970, 56);
            this.panel3.TabIndex = 0;
            // 
            // buttonSearchShareLimit
            // 
            this.buttonSearchShareLimit.Location = new System.Drawing.Point(157, 14);
            this.buttonSearchShareLimit.Name = "buttonSearchShareLimit";
            this.buttonSearchShareLimit.Size = new System.Drawing.Size(75, 23);
            this.buttonSearchShareLimit.TabIndex = 1;
            this.buttonSearchShareLimit.Text = "查询";
            this.buttonSearchShareLimit.UseVisualStyleBackColor = true;
            this.buttonSearchShareLimit.Click += new System.EventHandler(this.buttonSearchShareLimit_Click);
            // 
            // comboBoxShareLimitGroup
            // 
            this.comboBoxShareLimitGroup.FormattingEnabled = true;
            this.comboBoxShareLimitGroup.Location = new System.Drawing.Point(20, 14);
            this.comboBoxShareLimitGroup.Name = "comboBoxShareLimitGroup";
            this.comboBoxShareLimitGroup.Size = new System.Drawing.Size(121, 25);
            this.comboBoxShareLimitGroup.TabIndex = 0;
            // 
            // tabPage可用仓位
            // 
            this.tabPage可用仓位.Controls.Add(this.panel6);
            this.tabPage可用仓位.Location = new System.Drawing.Point(4, 26);
            this.tabPage可用仓位.Name = "tabPage可用仓位";
            this.tabPage可用仓位.Size = new System.Drawing.Size(976, 501);
            this.tabPage可用仓位.TabIndex = 10;
            this.tabPage可用仓位.Text = "可用仓位";
            this.tabPage可用仓位.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.ucPositionShow1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(976, 501);
            this.panel6.TabIndex = 2;
            // 
            // ucPositionShow1
            // 
            this.ucPositionShow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucPositionShow1.Location = new System.Drawing.Point(0, 0);
            this.ucPositionShow1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.ucPositionShow1.Name = "ucPositionShow1";
            this.ucPositionShow1.Size = new System.Drawing.Size(976, 501);
            this.ucPositionShow1.TabIndex = 0;
            // 
            // tabPage交易员配置
            // 
            this.tabPage交易员配置.Controls.Add(this.ucUserAccountSet1);
            this.tabPage交易员配置.Location = new System.Drawing.Point(4, 26);
            this.tabPage交易员配置.Name = "tabPage交易员配置";
            this.tabPage交易员配置.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage交易员配置.Size = new System.Drawing.Size(976, 501);
            this.tabPage交易员配置.TabIndex = 11;
            this.tabPage交易员配置.Text = "组合号交易员配置";
            this.tabPage交易员配置.UseVisualStyleBackColor = true;
            // 
            // ucUserAccountSet1
            // 
            this.ucUserAccountSet1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucUserAccountSet1.Location = new System.Drawing.Point(3, 3);
            this.ucUserAccountSet1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucUserAccountSet1.Name = "ucUserAccountSet1";
            this.ucUserAccountSet1.Size = new System.Drawing.Size(970, 495);
            this.ucUserAccountSet1.TabIndex = 0;
            // 
            // tabPage额度管理
            // 
            this.tabPage额度管理.Controls.Add(this.ucStockLimitManage1);
            this.tabPage额度管理.Location = new System.Drawing.Point(4, 26);
            this.tabPage额度管理.Name = "tabPage额度管理";
            this.tabPage额度管理.Size = new System.Drawing.Size(976, 501);
            this.tabPage额度管理.TabIndex = 12;
            this.tabPage额度管理.Text = "额度管理";
            this.tabPage额度管理.UseVisualStyleBackColor = true;
            // 
            // ucStockLimitManage1
            // 
            this.ucStockLimitManage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucStockLimitManage1.Location = new System.Drawing.Point(0, 0);
            this.ucStockLimitManage1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucStockLimitManage1.Name = "ucStockLimitManage1";
            this.ucStockLimitManage1.Size = new System.Drawing.Size(976, 501);
            this.ucStockLimitManage1.TabIndex = 0;
            // 
            // tabPage交易员管理
            // 
            this.tabPage交易员管理.Controls.Add(this.ucTraderManager1);
            this.tabPage交易员管理.Location = new System.Drawing.Point(4, 26);
            this.tabPage交易员管理.Name = "tabPage交易员管理";
            this.tabPage交易员管理.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage交易员管理.Size = new System.Drawing.Size(976, 501);
            this.tabPage交易员管理.TabIndex = 13;
            this.tabPage交易员管理.Text = "交易员管理";
            this.tabPage交易员管理.UseVisualStyleBackColor = true;
            // 
            // ucTraderManager1
            // 
            this.ucTraderManager1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucTraderManager1.Location = new System.Drawing.Point(3, 3);
            this.ucTraderManager1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucTraderManager1.Name = "ucTraderManager1";
            this.ucTraderManager1.Size = new System.Drawing.Size(970, 495);
            this.ucTraderManager1.TabIndex = 0;
            // 
            // tabPage运行日志
            // 
            this.tabPage运行日志.Controls.Add(this.panel5);
            this.tabPage运行日志.Location = new System.Drawing.Point(4, 26);
            this.tabPage运行日志.Name = "tabPage运行日志";
            this.tabPage运行日志.Size = new System.Drawing.Size(976, 501);
            this.tabPage运行日志.TabIndex = 9;
            this.tabPage运行日志.Text = "运行日志";
            this.tabPage运行日志.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.dataGridView运行日志);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(976, 501);
            this.panel5.TabIndex = 0;
            // 
            // dataGridView运行日志
            // 
            this.dataGridView运行日志.AllowUserToAddRows = false;
            this.dataGridView运行日志.AllowUserToDeleteRows = false;
            this.dataGridView运行日志.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView运行日志.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView运行日志.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView运行日志.Location = new System.Drawing.Point(0, 0);
            this.dataGridView运行日志.Name = "dataGridView运行日志";
            this.dataGridView运行日志.ReadOnly = true;
            this.dataGridView运行日志.RowHeadersVisible = false;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView运行日志.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView运行日志.RowTemplate.Height = 23;
            this.dataGridView运行日志.Size = new System.Drawing.Size(974, 499);
            this.dataGridView运行日志.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "用户名";
            this.dataGridViewTextBoxColumn1.HeaderText = "用户名";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 66;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "仓位限制";
            this.dataGridViewTextBoxColumn2.HeaderText = "仓位限制";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 78;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "亏损限制";
            this.dataGridViewTextBoxColumn3.HeaderText = "亏损限制";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 78;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "组合号";
            this.dataGridViewTextBoxColumn4.HeaderText = "组合号";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 66;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "证券代码";
            this.dataGridViewTextBoxColumn5.HeaderText = "证券代码";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 78;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "证券名称";
            this.dataGridViewTextBoxColumn6.HeaderText = "证券名称";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 78;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "开仓类别";
            this.dataGridViewTextBoxColumn7.HeaderText = "开仓类别";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 78;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "已开数量";
            dataGridViewCellStyle6.Format = "f0";
            this.dataGridViewTextBoxColumn8.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn8.HeaderText = "已开数量";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Width = 78;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "TraderAccount";
            this.dataGridViewTextBoxColumn9.HeaderText = "用户名";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "GroupAccount";
            this.dataGridViewTextBoxColumn10.HeaderText = "组合号";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "StockID";
            this.dataGridViewTextBoxColumn11.HeaderText = "股票代码";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "StockName";
            this.dataGridViewTextBoxColumn12.HeaderText = "股票名称";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "LimitCount";
            this.dataGridViewTextBoxColumn13.HeaderText = "额度";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "Commission";
            this.dataGridViewTextBoxColumn14.HeaderText = "手续费率";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.DataPropertyName = "BuyType";
            this.dataGridViewTextBoxColumn15.HeaderText = "买方式";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.DataPropertyName = "SaleType";
            this.dataGridViewTextBoxColumn16.HeaderText = "卖方式";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            // 
            // contextMenuStrip当前仓位
            // 
            this.contextMenuStrip当前仓位.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.刷新ToolStripMenuItem});
            this.contextMenuStrip当前仓位.Name = "contextMenuStrip当前仓位";
            this.contextMenuStrip当前仓位.Size = new System.Drawing.Size(101, 26);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // RiskControlMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 580);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "RiskControlMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "风控员";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RiskControlMainForm_FormClosing);
            this.Load += new System.EventHandler(this.RiskControlMainForm_Load);
            this.contextMenuStrip交易额度.ResumeLayout(false);
            this.contextMenuStrip交易日志.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabPage监控对象.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView交易员)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView交易额度)).EndInit();
            this.tabPage当前仓位.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView当前仓位)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabPage当前委托.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView当前委托)).EndInit();
            this.tabPage业绩统计.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView业绩统计)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage组合仓位.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView组合仓位)).EndInit();
            this.tabPage额度共享.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView额度分配交易员)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView额度分配股票)).EndInit();
            this.panel3.ResumeLayout(false);
            this.tabPage可用仓位.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.tabPage交易员配置.ResumeLayout(false);
            this.tabPage额度管理.ResumeLayout(false);
            this.tabPage交易员管理.ResumeLayout(false);
            this.tabPage运行日志.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView运行日志)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource交易员)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource当前仓位)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource当前委托)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource当前成交)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource当日统计)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource委托记录)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource交易额度)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource组合仓位)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource运行日志)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource额度共享股票)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource额度共享交易员)).EndInit();
            this.contextMenuStrip当前仓位.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView交易员;
        private System.Windows.Forms.BindingSource bindingSource交易员;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPage当前仓位;
        private System.Windows.Forms.DataGridView dataGridView当前仓位;
        private System.Windows.Forms.TabPage tabPage当前委托;
        private System.Windows.Forms.DataGridView dataGridView当前委托;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.ComponentModel.BackgroundWorker backgroundWorker报价;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel时间;
        private System.Windows.Forms.BindingSource bindingSource当前仓位;
        private System.Windows.Forms.BindingSource bindingSource当前委托;
        private System.Windows.Forms.BindingSource bindingSource当前成交;
        private System.Windows.Forms.BindingSource bindingSource当日统计;
        private System.Windows.Forms.BindingSource bindingSource委托记录;
        private System.ComponentModel.BackgroundWorker backgroundWorker行情;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 行情服务器ToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage监控对象;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView交易额度;
        private System.Windows.Forms.BindingSource bindingSource交易额度;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip交易日志;
        private System.Windows.Forms.ToolStripMenuItem 刷新交易日志ToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage业绩统计;
        private System.Windows.Forms.DataGridView dataGridView业绩统计;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button统计;
        private System.Windows.Forms.DateTimePicker dateTimePicker结束日期;
        private System.Windows.Forms.DateTimePicker dateTimePicker开始日期;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label实现盈亏;
        private System.Windows.Forms.Label label浮动盈亏;
        private System.Windows.Forms.Label label市值合计;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.RadioButton radioButton按组合号;
        private System.Windows.Forms.RadioButton radioButton按交易员;
        private System.Windows.Forms.DataGridViewTextBoxColumn 用户名;
        private System.Windows.Forms.DataGridViewTextBoxColumn 仓位限制;
        private System.Windows.Forms.DataGridViewTextBoxColumn 亏损限制;
        private System.Windows.Forms.TabPage tabPage组合仓位;
        private System.Windows.Forms.DataGridView dataGridView组合仓位;
        private System.Windows.Forms.BindingSource bindingSource组合仓位;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip交易额度;
        private System.Windows.Forms.ToolStripMenuItem 显示全部ToolStripMenuItem;
        private System.Windows.Forms.BindingSource bindingSource运行日志;
        private System.Windows.Forms.Timer timerConnector;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelConnect;
        private System.Windows.Forms.TabPage tabPage额度共享;
        private System.Windows.Forms.BindingSource bindingSource额度共享股票;
        private System.Windows.Forms.BindingSource bindingSource额度共享交易员;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox comboBoxShareLimitGroup;
        private System.Windows.Forms.Button buttonSearchShareLimit;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.DataGridView dataGridView额度分配交易员;
        private System.Windows.Forms.DataGridView dataGridView额度分配股票;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTraderAccount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGroupAccount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStockID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStockName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLimitCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCommission;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnBuyType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSaleType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.TabPage tabPage运行日志;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.DataGridView dataGridView运行日志;
        private System.Windows.Forms.TabPage tabPage可用仓位;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.TabPage tabPage交易员配置;
        private UC.ucPositionShow ucPositionShow1;
        private UC.ucUserAccountSet ucUserAccountSet1;
        private System.Windows.Forms.TabPage tabPage额度管理;
        private UC.ucStockLimitManage ucStockLimitManage1;
        private System.Windows.Forms.TabPage tabPage交易员管理;
        private UC.ucTraderManager ucTraderManager1;
        private System.Windows.Forms.ToolStripMenuItem 数据查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 当日成交ToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripMenuItem 委托记录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 当日平仓ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip当前仓位;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
    }
}