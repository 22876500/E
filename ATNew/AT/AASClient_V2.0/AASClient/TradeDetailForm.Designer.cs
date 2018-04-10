namespace AASClient
{
    partial class TradeDetailForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView当前成交 = new System.Windows.Forms.DataGridView();
            this.交易员 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.组合号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.证券代码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.证券名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.买卖方向 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.成交价格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.成交数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.成交金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.成交时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.成交编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.委托编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource当前成交 = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.numericUpDownPageSize = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownPageTotal = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownPageIndex = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
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
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView当前成交)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource当前成交)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPageSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPageTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPageIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(926, 75);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "辅助功能";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(43, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "导出Excel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView当前成交
            // 
            this.dataGridView当前成交.AllowUserToAddRows = false;
            this.dataGridView当前成交.AllowUserToDeleteRows = false;
            this.dataGridView当前成交.AllowUserToResizeRows = false;
            this.dataGridView当前成交.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView当前成交.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.交易员,
            this.组合号,
            this.证券代码,
            this.证券名称,
            this.买卖方向,
            this.成交价格,
            this.成交数量,
            this.成交金额,
            this.成交时间,
            this.成交编号,
            this.委托编号});
            this.dataGridView当前成交.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView当前成交.Location = new System.Drawing.Point(0, 75);
            this.dataGridView当前成交.Name = "dataGridView当前成交";
            this.dataGridView当前成交.RowHeadersVisible = false;
            this.dataGridView当前成交.RowTemplate.Height = 23;
            this.dataGridView当前成交.Size = new System.Drawing.Size(926, 487);
            this.dataGridView当前成交.TabIndex = 1;
            this.dataGridView当前成交.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView当前成交_DataError);
            this.dataGridView当前成交.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView当前成交_RowPrePaint);
            // 
            // 交易员
            // 
            this.交易员.DataPropertyName = "交易员";
            this.交易员.HeaderText = "交易员";
            this.交易员.Name = "交易员";
            this.交易员.Width = 80;
            // 
            // 组合号
            // 
            this.组合号.DataPropertyName = "组合号";
            this.组合号.HeaderText = "组合号";
            this.组合号.Name = "组合号";
            this.组合号.Width = 80;
            // 
            // 证券代码
            // 
            this.证券代码.DataPropertyName = "证券代码";
            this.证券代码.HeaderText = "证券代码";
            this.证券代码.Name = "证券代码";
            this.证券代码.Width = 80;
            // 
            // 证券名称
            // 
            this.证券名称.DataPropertyName = "证券名称";
            this.证券名称.HeaderText = "证券名称";
            this.证券名称.Name = "证券名称";
            this.证券名称.Width = 80;
            // 
            // 买卖方向
            // 
            this.买卖方向.DataPropertyName = "买卖方向";
            this.买卖方向.HeaderText = "买卖方向";
            this.买卖方向.Name = "买卖方向";
            this.买卖方向.Width = 80;
            // 
            // 成交价格
            // 
            this.成交价格.DataPropertyName = "成交价格";
            this.成交价格.HeaderText = "成交价格";
            this.成交价格.Name = "成交价格";
            this.成交价格.Width = 80;
            // 
            // 成交数量
            // 
            this.成交数量.DataPropertyName = "成交数量";
            this.成交数量.HeaderText = "成交数量";
            this.成交数量.Name = "成交数量";
            this.成交数量.Width = 80;
            // 
            // 成交金额
            // 
            this.成交金额.DataPropertyName = "成交金额";
            this.成交金额.HeaderText = "成交金额";
            this.成交金额.Name = "成交金额";
            this.成交金额.Width = 80;
            // 
            // 成交时间
            // 
            this.成交时间.DataPropertyName = "成交时间";
            this.成交时间.HeaderText = "成交时间";
            this.成交时间.Name = "成交时间";
            this.成交时间.Width = 80;
            // 
            // 成交编号
            // 
            this.成交编号.DataPropertyName = "成交编号";
            this.成交编号.HeaderText = "成交编号";
            this.成交编号.Name = "成交编号";
            this.成交编号.Width = 80;
            // 
            // 委托编号
            // 
            this.委托编号.DataPropertyName = "委托编号";
            this.委托编号.HeaderText = "委托编号";
            this.委托编号.Name = "委托编号";
            this.委托编号.Width = 80;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.numericUpDownPageSize);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.numericUpDownPageTotal);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.numericUpDownPageIndex);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 562);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(926, 54);
            this.panel1.TabIndex = 1;
            // 
            // numericUpDownPageSize
            // 
            this.numericUpDownPageSize.Location = new System.Drawing.Point(294, 15);
            this.numericUpDownPageSize.Name = "numericUpDownPageSize";
            this.numericUpDownPageSize.Size = new System.Drawing.Size(55, 21);
            this.numericUpDownPageSize.TabIndex = 7;
            this.numericUpDownPageSize.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownPageSize.ValueChanged += new System.EventHandler(this.numericUpDownPageSize_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(238, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "每页条数";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(211, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "页";
            // 
            // numericUpDownPageTotal
            // 
            this.numericUpDownPageTotal.Enabled = false;
            this.numericUpDownPageTotal.Location = new System.Drawing.Point(152, 17);
            this.numericUpDownPageTotal.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownPageTotal.Name = "numericUpDownPageTotal";
            this.numericUpDownPageTotal.ReadOnly = true;
            this.numericUpDownPageTotal.Size = new System.Drawing.Size(52, 21);
            this.numericUpDownPageTotal.TabIndex = 4;
            this.numericUpDownPageTotal.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(131, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "共";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(103, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "页";
            // 
            // numericUpDownPageIndex
            // 
            this.numericUpDownPageIndex.Location = new System.Drawing.Point(43, 17);
            this.numericUpDownPageIndex.Name = "numericUpDownPageIndex";
            this.numericUpDownPageIndex.Size = new System.Drawing.Size(54, 21);
            this.numericUpDownPageIndex.TabIndex = 1;
            this.numericUpDownPageIndex.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPageIndex.ValueChanged += new System.EventHandler(this.numericUpDownPageIndex_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "第";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "交易员";
            this.dataGridViewTextBoxColumn1.HeaderText = "交易员";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 80;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "组合号";
            this.dataGridViewTextBoxColumn2.HeaderText = "组合号";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 80;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "证券代码";
            this.dataGridViewTextBoxColumn3.HeaderText = "证券代码";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 80;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "证券名称";
            this.dataGridViewTextBoxColumn4.HeaderText = "证券名称";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 80;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "买卖方向";
            this.dataGridViewTextBoxColumn5.HeaderText = "买卖方向";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 80;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "成交价格";
            this.dataGridViewTextBoxColumn6.HeaderText = "成交价格";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 80;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "成交数量";
            this.dataGridViewTextBoxColumn7.HeaderText = "成交数量";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Width = 80;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "成交金额";
            this.dataGridViewTextBoxColumn8.HeaderText = "成交金额";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Width = 80;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "成交时间";
            this.dataGridViewTextBoxColumn9.HeaderText = "成交时间";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Width = 80;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "成交编号";
            this.dataGridViewTextBoxColumn10.HeaderText = "成交编号";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.Width = 80;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "委托编号";
            this.dataGridViewTextBoxColumn11.HeaderText = "委托编号";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.Width = 80;
            // 
            // TradeDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 616);
            this.Controls.Add(this.dataGridView当前成交);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "TradeDetailForm";
            this.Text = "当前成交";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView当前成交)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource当前成交)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPageSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPageTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPageIndex)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView当前成交;
        private System.Windows.Forms.BindingSource bindingSource当前成交;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown numericUpDownPageSize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownPageTotal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownPageIndex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 交易员;
        private System.Windows.Forms.DataGridViewTextBoxColumn 组合号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 证券代码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 证券名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 买卖方向;
        private System.Windows.Forms.DataGridViewTextBoxColumn 成交价格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 成交数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 成交金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 成交时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 成交编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 委托编号;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
    }
}