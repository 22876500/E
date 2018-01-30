namespace AASClient
{
    partial class TradeMainForm
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
            WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
            WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin1 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient1 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient2 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient3 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient4 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient5 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient3 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient6 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient7 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.行情服务器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.交易快捷键ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.报价窗口数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.交易额度ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.共享额度ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.价格提示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.界面设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.订阅设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.订阅模式设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.价格提示设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.预警显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel时间 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelConnect = new System.Windows.Forms.ToolStripStatusLabel();
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.显示所有窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker行情 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker报价 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlWaiting = new System.Windows.Forms.Panel();
            this.lblWaiting = new System.Windows.Forms.Label();
            this.timerTestConnect = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.pnlWaiting.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem,
            this.查询ToolStripMenuItem,
            this.价格提示ToolStripMenuItem,
            this.界面设置ToolStripMenuItem,
            this.订阅设置ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(984, 27);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.行情服务器ToolStripMenuItem,
            this.交易快捷键ToolStripMenuItem,
            this.报价窗口数ToolStripMenuItem});
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem.Text = "设置";
            // 
            // 行情服务器ToolStripMenuItem
            // 
            this.行情服务器ToolStripMenuItem.Name = "行情服务器ToolStripMenuItem";
            this.行情服务器ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.行情服务器ToolStripMenuItem.Text = "行情服务器";
            this.行情服务器ToolStripMenuItem.Visible = false;
            this.行情服务器ToolStripMenuItem.Click += new System.EventHandler(this.行情服务器ToolStripMenuItem_Click);
            // 
            // 交易快捷键ToolStripMenuItem
            // 
            this.交易快捷键ToolStripMenuItem.Name = "交易快捷键ToolStripMenuItem";
            this.交易快捷键ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.交易快捷键ToolStripMenuItem.Text = "交易快捷键";
            this.交易快捷键ToolStripMenuItem.Click += new System.EventHandler(this.交易快捷键ToolStripMenuItem_Click);
            // 
            // 报价窗口数ToolStripMenuItem
            // 
            this.报价窗口数ToolStripMenuItem.Name = "报价窗口数ToolStripMenuItem";
            this.报价窗口数ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.报价窗口数ToolStripMenuItem.Text = "报价窗口数";
            this.报价窗口数ToolStripMenuItem.Click += new System.EventHandler(this.报价窗口数ToolStripMenuItem_Click);
            // 
            // 查询ToolStripMenuItem
            // 
            this.查询ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.交易额度ToolStripMenuItem,
            this.共享额度ToolStripMenuItem});
            this.查询ToolStripMenuItem.Name = "查询ToolStripMenuItem";
            this.查询ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.查询ToolStripMenuItem.Text = "查询";
            // 
            // 交易额度ToolStripMenuItem
            // 
            this.交易额度ToolStripMenuItem.Name = "交易额度ToolStripMenuItem";
            this.交易额度ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.交易额度ToolStripMenuItem.Text = "交易额度";
            this.交易额度ToolStripMenuItem.Click += new System.EventHandler(this.交易额度ToolStripMenuItem_Click);
            // 
            // 共享额度ToolStripMenuItem
            // 
            this.共享额度ToolStripMenuItem.Name = "共享额度ToolStripMenuItem";
            this.共享额度ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.共享额度ToolStripMenuItem.Text = "共享额度";
            this.共享额度ToolStripMenuItem.Click += new System.EventHandler(this.共享额度ToolStripMenuItem_Click);
            // 
            // 价格提示ToolStripMenuItem
            // 
            this.价格提示ToolStripMenuItem.Name = "价格提示ToolStripMenuItem";
            this.价格提示ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.价格提示ToolStripMenuItem.Text = "价格提示";
            this.价格提示ToolStripMenuItem.Click += new System.EventHandler(this.价格提示ToolStripMenuItem_Click);
            // 
            // 界面设置ToolStripMenuItem
            // 
            this.界面设置ToolStripMenuItem.Name = "界面设置ToolStripMenuItem";
            this.界面设置ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.界面设置ToolStripMenuItem.Text = "界面设置";
            this.界面设置ToolStripMenuItem.Click += new System.EventHandler(this.界面设置ToolStripMenuItem_Click);
            // 
            // 订阅设置ToolStripMenuItem
            // 
            this.订阅设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.订阅模式设置ToolStripMenuItem,
            this.价格提示设置ToolStripMenuItem,
            this.预警显示ToolStripMenuItem});
            this.订阅设置ToolStripMenuItem.Name = "订阅设置ToolStripMenuItem";
            this.订阅设置ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.订阅设置ToolStripMenuItem.Text = "预警设置";
            this.订阅设置ToolStripMenuItem.Visible = false;
            // 
            // 订阅模式设置ToolStripMenuItem
            // 
            this.订阅模式设置ToolStripMenuItem.Name = "订阅模式设置ToolStripMenuItem";
            this.订阅模式设置ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.订阅模式设置ToolStripMenuItem.Text = "订阅模式设置";
            this.订阅模式设置ToolStripMenuItem.Click += new System.EventHandler(this.订阅模式设置ToolStripMenuItem_Click);
            // 
            // 价格提示设置ToolStripMenuItem
            // 
            this.价格提示设置ToolStripMenuItem.Name = "价格提示设置ToolStripMenuItem";
            this.价格提示设置ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.价格提示设置ToolStripMenuItem.Text = "预警设置";
            this.价格提示设置ToolStripMenuItem.Click += new System.EventHandler(this.价格提示设置ToolStripMenuItem_Click);
            // 
            // 预警显示ToolStripMenuItem
            // 
            this.预警显示ToolStripMenuItem.Name = "预警显示ToolStripMenuItem";
            this.预警显示ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.预警显示ToolStripMenuItem.Text = "预警显示";
            this.预警显示ToolStripMenuItem.Click += new System.EventHandler(this.预警显示ToolStripMenuItem_Click);
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
            this.toolStripStatusLabel时间.ForeColor = System.Drawing.Color.Red;
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
            // dockPanel1
            // 
            this.dockPanel1.ContextMenuStrip = this.contextMenuStrip1;
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.DockBackColor = System.Drawing.SystemColors.AppWorkspace;
            this.dockPanel1.Location = new System.Drawing.Point(0, 27);
            this.dockPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(984, 531);
            dockPanelGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
            tabGradient1.EndColor = System.Drawing.SystemColors.Control;
            tabGradient1.StartColor = System.Drawing.SystemColors.Control;
            tabGradient1.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin1.TabGradient = tabGradient1;
            autoHideStripSkin1.TextFont = new System.Drawing.Font("微软雅黑", 9F);
            dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
            tabGradient2.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
            dockPanelGradient2.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient2.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
            tabGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
            dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
            dockPaneStripSkin1.TextFont = new System.Drawing.Font("微软雅黑", 9F);
            tabGradient4.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient4.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient4.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
            tabGradient5.EndColor = System.Drawing.SystemColors.Control;
            tabGradient5.StartColor = System.Drawing.SystemColors.Control;
            tabGradient5.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
            dockPanelGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
            tabGradient6.EndColor = System.Drawing.SystemColors.InactiveCaption;
            tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient6.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient6.TextColor = System.Drawing.SystemColors.InactiveCaptionText;
            dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
            tabGradient7.EndColor = System.Drawing.Color.Transparent;
            tabGradient7.StartColor = System.Drawing.Color.Transparent;
            tabGradient7.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
            dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
            dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
            this.dockPanel1.Skin = dockPanelSkin1;
            this.dockPanel1.TabIndex = 2;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示所有窗口ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 26);
            // 
            // 显示所有窗口ToolStripMenuItem
            // 
            this.显示所有窗口ToolStripMenuItem.Name = "显示所有窗口ToolStripMenuItem";
            this.显示所有窗口ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.显示所有窗口ToolStripMenuItem.Text = "显示所有窗口";
            this.显示所有窗口ToolStripMenuItem.Click += new System.EventHandler(this.显示所有窗口ToolStripMenuItem_Click);
            // 
            // backgroundWorker行情
            // 
            this.backgroundWorker行情.WorkerReportsProgress = true;
            this.backgroundWorker行情.WorkerSupportsCancellation = true;
            this.backgroundWorker行情.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker行情_DoWork);
            this.backgroundWorker行情.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker行情_ProgressChanged);
            this.backgroundWorker行情.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker行情_RunWorkerCompleted);
            // 
            // backgroundWorker报价
            // 
            this.backgroundWorker报价.WorkerReportsProgress = true;
            this.backgroundWorker报价.WorkerSupportsCancellation = true;
            this.backgroundWorker报价.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker报价_DoWork);
            this.backgroundWorker报价.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker报价_ProgressChanged);
            this.backgroundWorker报价.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker报价_RunWorkerCompleted);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pnlWaiting
            // 
            this.pnlWaiting.Controls.Add(this.lblWaiting);
            this.pnlWaiting.Location = new System.Drawing.Point(367, 228);
            this.pnlWaiting.Name = "pnlWaiting";
            this.pnlWaiting.Padding = new System.Windows.Forms.Padding(4);
            this.pnlWaiting.Size = new System.Drawing.Size(200, 33);
            this.pnlWaiting.TabIndex = 5;
            this.pnlWaiting.Visible = false;
            // 
            // lblWaiting
            // 
            this.lblWaiting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWaiting.Location = new System.Drawing.Point(4, 4);
            this.lblWaiting.Name = "lblWaiting";
            this.lblWaiting.Size = new System.Drawing.Size(192, 25);
            this.lblWaiting.TabIndex = 0;
            this.lblWaiting.Text = "正在退出系统……";
            this.lblWaiting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerTestConnect
            // 
            this.timerTestConnect.Enabled = true;
            this.timerTestConnect.Interval = 2000;
            this.timerTestConnect.Tick += new System.EventHandler(this.timerTestConnect_Tick);
            // 
            // TradeMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 580);
            this.Controls.Add(this.pnlWaiting);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TradeMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "交易员";
            this.Load += new System.EventHandler(this.TradeMainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.pnlWaiting.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 行情服务器ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel时间;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 显示所有窗口ToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker行情;
        private System.ComponentModel.BackgroundWorker backgroundWorker报价;
        private System.Windows.Forms.ToolStripMenuItem 查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 交易额度ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 交易快捷键ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 报价窗口数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 价格提示ToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem 界面设置ToolStripMenuItem;
        private System.Windows.Forms.Panel pnlWaiting;
        private System.Windows.Forms.Label lblWaiting;
        private System.Windows.Forms.ToolStripMenuItem 订阅设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 订阅模式设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 价格提示设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 预警显示ToolStripMenuItem;
        private System.Windows.Forms.Timer timerTestConnect;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelConnect;
        private System.Windows.Forms.ToolStripMenuItem 共享额度ToolStripMenuItem;
    }
}