namespace AASClient
{
    partial class LogForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView交易日志 = new System.Windows.Forms.DataGridView();
            this.证券代码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.证券名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.买卖方向 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.委托数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.委托价格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.信息 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.操作员 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView交易日志)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView交易日志
            // 
            this.dataGridView交易日志.AllowUserToAddRows = false;
            this.dataGridView交易日志.AllowUserToDeleteRows = false;
            this.dataGridView交易日志.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView交易日志.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView交易日志.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView交易日志.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.证券代码,
            this.证券名称,
            this.买卖方向,
            this.委托数量,
            this.委托价格,
            this.信息,
            this.操作员,
            this.时间});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView交易日志.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView交易日志.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView交易日志.Location = new System.Drawing.Point(0, 0);
            this.dataGridView交易日志.Name = "dataGridView交易日志";
            this.dataGridView交易日志.ReadOnly = true;
            this.dataGridView交易日志.RowHeadersVisible = false;
            this.dataGridView交易日志.RowTemplate.Height = 23;
            this.dataGridView交易日志.Size = new System.Drawing.Size(507, 500);
            this.dataGridView交易日志.TabIndex = 1;
            this.dataGridView交易日志.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView交易日志_CellFormatting);
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
            // 买卖方向
            // 
            this.买卖方向.DataPropertyName = "买卖方向";
            this.买卖方向.HeaderText = "买卖方向";
            this.买卖方向.Name = "买卖方向";
            this.买卖方向.ReadOnly = true;
            // 
            // 委托数量
            // 
            this.委托数量.DataPropertyName = "委托数量";
            this.委托数量.HeaderText = "委托数量";
            this.委托数量.Name = "委托数量";
            this.委托数量.ReadOnly = true;
            // 
            // 委托价格
            // 
            this.委托价格.DataPropertyName = "委托价格";
            this.委托价格.HeaderText = "委托价格";
            this.委托价格.Name = "委托价格";
            this.委托价格.ReadOnly = true;
            // 
            // 信息
            // 
            this.信息.DataPropertyName = "信息";
            this.信息.HeaderText = "信息";
            this.信息.Name = "信息";
            this.信息.ReadOnly = true;
            // 
            // 操作员
            // 
            this.操作员.DataPropertyName = "交易员";
            this.操作员.HeaderText = "操作员";
            this.操作员.Name = "操作员";
            this.操作员.ReadOnly = true;
            // 
            // 时间
            // 
            this.时间.DataPropertyName = "时间";
            this.时间.HeaderText = "时间";
            this.时间.Name = "时间";
            this.时间.ReadOnly = true;
            // 
            // LogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 500);
            this.Controls.Add(this.dataGridView交易日志);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "LogForm";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
            this.Text = "日志";
            this.Load += new System.EventHandler(this.LogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView交易日志)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView交易日志;
        private System.Windows.Forms.DataGridViewTextBoxColumn 证券代码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 证券名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 买卖方向;
        private System.Windows.Forms.DataGridViewTextBoxColumn 委托数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 委托价格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 信息;
        private System.Windows.Forms.DataGridViewTextBoxColumn 操作员;
        private System.Windows.Forms.DataGridViewTextBoxColumn 时间;
        private System.Windows.Forms.BindingSource bindingSource1;

    }
}