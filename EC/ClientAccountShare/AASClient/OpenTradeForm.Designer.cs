namespace AASClient
{
    partial class OpenTradeForm
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
            this.dataGridView订单 = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.hlRefresh = new System.Windows.Forms.LinkLabel();
            this.label实现盈亏 = new System.Windows.Forms.Label();
            this.label浮动盈亏 = new System.Windows.Forms.Label();
            this.label市值合计 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView订单)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView订单
            // 
            this.dataGridView订单.AllowUserToAddRows = false;
            this.dataGridView订单.AllowUserToDeleteRows = false;
            this.dataGridView订单.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView订单.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView订单.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView订单.Location = new System.Drawing.Point(0, 39);
            this.dataGridView订单.Name = "dataGridView订单";
            this.dataGridView订单.ReadOnly = true;
            this.dataGridView订单.RowHeadersVisible = false;
            this.dataGridView订单.RowTemplate.Height = 23;
            this.dataGridView订单.Size = new System.Drawing.Size(650, 421);
            this.dataGridView订单.TabIndex = 0;
            this.dataGridView订单.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView订单_CellFormatting);
            this.dataGridView订单.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView订单_DataError);
            this.dataGridView订单.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView订单_RowPrePaint);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridView订单);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(650, 460);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.hlRefresh);
            this.panel2.Controls.Add(this.label实现盈亏);
            this.panel2.Controls.Add(this.label浮动盈亏);
            this.panel2.Controls.Add(this.label市值合计);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(650, 39);
            this.panel2.TabIndex = 1;
            // 
            // hlRefresh
            // 
            this.hlRefresh.AutoSize = true;
            this.hlRefresh.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.hlRefresh.Location = new System.Drawing.Point(541, 9);
            this.hlRefresh.Name = "hlRefresh";
            this.hlRefresh.Size = new System.Drawing.Size(37, 20);
            this.hlRefresh.TabIndex = 6;
            this.hlRefresh.TabStop = true;
            this.hlRefresh.Text = "刷新";
            this.hlRefresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.hlRefresh_LinkClicked);
            // 
            // label实现盈亏
            // 
            this.label实现盈亏.AutoSize = true;
            this.label实现盈亏.Location = new System.Drawing.Point(463, 9);
            this.label实现盈亏.Name = "label实现盈亏";
            this.label实现盈亏.Size = new System.Drawing.Size(17, 20);
            this.label实现盈亏.TabIndex = 5;
            this.label实现盈亏.Text = "0";
            // 
            // label浮动盈亏
            // 
            this.label浮动盈亏.AutoSize = true;
            this.label浮动盈亏.Location = new System.Drawing.Point(286, 9);
            this.label浮动盈亏.Name = "label浮动盈亏";
            this.label浮动盈亏.Size = new System.Drawing.Size(17, 20);
            this.label浮动盈亏.TabIndex = 4;
            this.label浮动盈亏.Text = "0";
            // 
            // label市值合计
            // 
            this.label市值合计.AutoSize = true;
            this.label市值合计.Location = new System.Drawing.Point(83, 9);
            this.label市值合计.Name = "label市值合计";
            this.label市值合计.Size = new System.Drawing.Size(17, 20);
            this.label市值合计.TabIndex = 3;
            this.label市值合计.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(392, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "实现盈亏";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(215, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "浮动盈亏";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "市值合计";
            // 
            // OpenTradeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 460);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Name = "OpenTradeForm";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
            this.Text = "未平仓交易";
            this.Load += new System.EventHandler(this.OpenTradeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView订单)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView订单;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.BindingSource bindingSource2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label实现盈亏;
        private System.Windows.Forms.Label label浮动盈亏;
        private System.Windows.Forms.Label label市值合计;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel hlRefresh;
    }
}