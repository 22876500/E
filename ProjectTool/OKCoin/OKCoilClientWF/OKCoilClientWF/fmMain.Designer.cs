namespace OKCoilClientWF
{
    partial class fmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelQuarterSell = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelQuarterBuy = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.labelThisWeekSell = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelOpenDiff = new System.Windows.Forms.Label();
            this.labelThisWeekBuy = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelCloseDiff = new System.Windows.Forms.Label();
            this.labelNextWeekSale = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.labelNextWeekBuy = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelQuarterSell);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.labelQuarterBuy);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(607, 59);
            this.panel1.TabIndex = 0;
            // 
            // labelQuarterSell
            // 
            this.labelQuarterSell.AutoSize = true;
            this.labelQuarterSell.Location = new System.Drawing.Point(220, 24);
            this.labelQuarterSell.Name = "labelQuarterSell";
            this.labelQuarterSell.Size = new System.Drawing.Size(11, 12);
            this.labelQuarterSell.TabIndex = 3;
            this.labelQuarterSell.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(156, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "季度卖一:";
            // 
            // labelQuarterBuy
            // 
            this.labelQuarterBuy.AutoSize = true;
            this.labelQuarterBuy.Location = new System.Drawing.Point(77, 24);
            this.labelQuarterBuy.Name = "labelQuarterBuy";
            this.labelQuarterBuy.Size = new System.Drawing.Size(11, 12);
            this.labelQuarterBuy.TabIndex = 1;
            this.labelQuarterBuy.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "季度买一:";
            // 
            // timerRefresh
            // 
            this.timerRefresh.Enabled = true;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 59);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(607, 292);
            this.panel2.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.labelNextWeekBuy);
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.labelNextWeekSale);
            this.tabPage1.Controls.Add(this.labelCloseDiff);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.labelThisWeekBuy);
            this.tabPage1.Controls.Add(this.labelOpenDiff);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.labelThisWeekSell);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(599, 266);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "BTC(次周-当周)";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // labelThisWeekSell
            // 
            this.labelThisWeekSell.AutoSize = true;
            this.labelThisWeekSell.Location = new System.Drawing.Point(370, 81);
            this.labelThisWeekSell.Name = "labelThisWeekSell";
            this.labelThisWeekSell.Size = new System.Drawing.Size(11, 12);
            this.labelThisWeekSell.TabIndex = 7;
            this.labelThisWeekSell.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(146, 151);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "开仓价位:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(304, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "本周卖一:";
            // 
            // labelOpenDiff
            // 
            this.labelOpenDiff.AutoSize = true;
            this.labelOpenDiff.Location = new System.Drawing.Point(210, 151);
            this.labelOpenDiff.Name = "labelOpenDiff";
            this.labelOpenDiff.Size = new System.Drawing.Size(11, 12);
            this.labelOpenDiff.TabIndex = 9;
            this.labelOpenDiff.Text = "0";
            // 
            // labelThisWeekBuy
            // 
            this.labelThisWeekBuy.AutoSize = true;
            this.labelThisWeekBuy.Location = new System.Drawing.Point(211, 82);
            this.labelThisWeekBuy.Name = "labelThisWeekBuy";
            this.labelThisWeekBuy.Size = new System.Drawing.Size(11, 12);
            this.labelThisWeekBuy.TabIndex = 5;
            this.labelThisWeekBuy.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(305, 151);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 12);
            this.label10.TabIndex = 10;
            this.label10.Text = "平仓价位:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(145, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "本周买一:";
            // 
            // labelCloseDiff
            // 
            this.labelCloseDiff.AutoSize = true;
            this.labelCloseDiff.Location = new System.Drawing.Point(369, 151);
            this.labelCloseDiff.Name = "labelCloseDiff";
            this.labelCloseDiff.Size = new System.Drawing.Size(11, 12);
            this.labelCloseDiff.TabIndex = 11;
            this.labelCloseDiff.Text = "0";
            // 
            // labelNextWeekSale
            // 
            this.labelNextWeekSale.AutoSize = true;
            this.labelNextWeekSale.Location = new System.Drawing.Point(370, 114);
            this.labelNextWeekSale.Name = "labelNextWeekSale";
            this.labelNextWeekSale.Size = new System.Drawing.Size(11, 12);
            this.labelNextWeekSale.TabIndex = 19;
            this.labelNextWeekSale.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(304, 115);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 12);
            this.label13.TabIndex = 18;
            this.label13.Text = "次周卖一:";
            // 
            // labelNextWeekBuy
            // 
            this.labelNextWeekBuy.AutoSize = true;
            this.labelNextWeekBuy.Location = new System.Drawing.Point(211, 115);
            this.labelNextWeekBuy.Name = "labelNextWeekBuy";
            this.labelNextWeekBuy.Size = new System.Drawing.Size(11, 12);
            this.labelNextWeekBuy.TabIndex = 17;
            this.labelNextWeekBuy.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(145, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "次周买一:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(607, 292);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 351);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fmMain";
            this.Text = "主界面";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelQuarterSell;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelQuarterBuy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelNextWeekBuy;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label labelNextWeekSale;
        private System.Windows.Forms.Label labelCloseDiff;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelThisWeekBuy;
        private System.Windows.Forms.Label labelOpenDiff;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelThisWeekSell;
    }
}

