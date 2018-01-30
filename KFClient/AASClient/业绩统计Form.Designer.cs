namespace AASClient
{
    partial class 业绩统计Form
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
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridView交易统计 = new System.Windows.Forms.DataGridView();
            this.证券代码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.证券名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.买入数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.卖出数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.买入金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.卖出金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.毛利 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.交易费用 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.净利润 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView交易统计)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView交易统计
            // 
            this.dataGridView交易统计.AllowUserToAddRows = false;
            this.dataGridView交易统计.AllowUserToDeleteRows = false;
            this.dataGridView交易统计.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView交易统计.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView交易统计.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.证券代码,
            this.证券名称,
            this.买入数量,
            this.卖出数量,
            this.买入金额,
            this.卖出金额,
            this.毛利,
            this.交易费用,
            this.净利润});
            this.dataGridView交易统计.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView交易统计.Location = new System.Drawing.Point(0, 0);
            this.dataGridView交易统计.Name = "dataGridView交易统计";
            this.dataGridView交易统计.ReadOnly = true;
            this.dataGridView交易统计.RowHeadersVisible = false;
            this.dataGridView交易统计.RowTemplate.Height = 23;
            this.dataGridView交易统计.Size = new System.Drawing.Size(911, 379);
            this.dataGridView交易统计.TabIndex = 1;
            this.dataGridView交易统计.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView交易统计_RowPrePaint);
            // 
            // 证券代码
            // 
            this.证券代码.DataPropertyName = "证券代码";
            this.证券代码.HeaderText = "证券代码";
            this.证券代码.Name = "证券代码";
            this.证券代码.ReadOnly = true;
            // 
            // 证券名称
            // 
            this.证券名称.DataPropertyName = "证券名称";
            this.证券名称.HeaderText = "证券名称";
            this.证券名称.Name = "证券名称";
            this.证券名称.ReadOnly = true;
            // 
            // 买入数量
            // 
            this.买入数量.DataPropertyName = "买入数量";
            this.买入数量.HeaderText = "买入数量";
            this.买入数量.Name = "买入数量";
            this.买入数量.ReadOnly = true;
            // 
            // 卖出数量
            // 
            this.卖出数量.DataPropertyName = "卖出数量";
            this.卖出数量.HeaderText = "卖出数量";
            this.卖出数量.Name = "卖出数量";
            this.卖出数量.ReadOnly = true;
            // 
            // 买入金额
            // 
            this.买入金额.DataPropertyName = "买入金额";
            this.买入金额.HeaderText = "买入金额";
            this.买入金额.Name = "买入金额";
            this.买入金额.ReadOnly = true;
            // 
            // 卖出金额
            // 
            this.卖出金额.DataPropertyName = "卖出金额";
            this.卖出金额.HeaderText = "卖出金额";
            this.卖出金额.Name = "卖出金额";
            this.卖出金额.ReadOnly = true;
            // 
            // 毛利
            // 
            this.毛利.DataPropertyName = "毛利";
            this.毛利.HeaderText = "毛利";
            this.毛利.Name = "毛利";
            this.毛利.ReadOnly = true;
            // 
            // 交易费用
            // 
            this.交易费用.DataPropertyName = "交易费用";
            this.交易费用.HeaderText = "交易费用";
            this.交易费用.Name = "交易费用";
            this.交易费用.ReadOnly = true;
            // 
            // 净利润
            // 
            this.净利润.DataPropertyName = "净利润";
            this.净利润.HeaderText = "净利润";
            this.净利润.Name = "净利润";
            this.净利润.ReadOnly = true;
            // 
            // 业绩统计Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 379);
            this.Controls.Add(this.dataGridView交易统计);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "业绩统计Form";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
            this.Text = "交易统计";
            this.Load += new System.EventHandler(this.交易统计Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView交易统计)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.DataGridView dataGridView交易统计;
        private System.Windows.Forms.DataGridViewTextBoxColumn 证券代码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 证券名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 买入数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 卖出数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 买入金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 卖出金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 毛利;
        private System.Windows.Forms.DataGridViewTextBoxColumn 交易费用;
        private System.Windows.Forms.DataGridViewTextBoxColumn 净利润;
    }
}