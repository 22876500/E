namespace AASClient.StockPosition
{
    partial class PositionList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelLoading = new System.Windows.Forms.Label();
            this.dataGridView可用仓位 = new System.Windows.Forms.DataGridView();
            this.Column组合号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column证券代码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column证券名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column剩余数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column剩余仓位 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column昨收 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column操作 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button刷新 = new System.Windows.Forms.Button();
            this.textBox证券名称 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox证券代码 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewButtonColumn1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView可用仓位)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.labelLoading);
            this.panel1.Controls.Add(this.dataGridView可用仓位);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(864, 377);
            this.panel1.TabIndex = 0;
            // 
            // labelLoading
            // 
            this.labelLoading.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelLoading.AutoSize = true;
            this.labelLoading.Location = new System.Drawing.Point(399, 155);
            this.labelLoading.Name = "labelLoading";
            this.labelLoading.Size = new System.Drawing.Size(65, 12);
            this.labelLoading.TabIndex = 2;
            this.labelLoading.Text = "加载中……";
            // 
            // dataGridView可用仓位
            // 
            this.dataGridView可用仓位.AllowUserToAddRows = false;
            this.dataGridView可用仓位.AllowUserToOrderColumns = true;
            this.dataGridView可用仓位.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView可用仓位.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column组合号,
            this.Column证券代码,
            this.Column证券名称,
            this.Column剩余数量,
            this.Column剩余仓位,
            this.Column昨收,
            this.Column操作});
            this.dataGridView可用仓位.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView可用仓位.Location = new System.Drawing.Point(0, 54);
            this.dataGridView可用仓位.Name = "dataGridView可用仓位";
            this.dataGridView可用仓位.ReadOnly = true;
            this.dataGridView可用仓位.RowHeadersVisible = false;
            this.dataGridView可用仓位.RowTemplate.Height = 23;
            this.dataGridView可用仓位.Size = new System.Drawing.Size(862, 321);
            this.dataGridView可用仓位.TabIndex = 1;
            this.dataGridView可用仓位.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView可用仓位_CellContentClick);
            this.dataGridView可用仓位.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView可用仓位_CellMouseDoubleClick);
            this.dataGridView可用仓位.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridView可用仓位_SortCompare);
            // 
            // Column组合号
            // 
            this.Column组合号.DataPropertyName = "组合号";
            this.Column组合号.HeaderText = "组合号";
            this.Column组合号.Name = "Column组合号";
            this.Column组合号.ReadOnly = true;
            this.Column组合号.Width = 70;
            // 
            // Column证券代码
            // 
            this.Column证券代码.DataPropertyName = "证券代码";
            this.Column证券代码.HeaderText = "证券代码";
            this.Column证券代码.Name = "Column证券代码";
            this.Column证券代码.ReadOnly = true;
            this.Column证券代码.Width = 80;
            // 
            // Column证券名称
            // 
            this.Column证券名称.DataPropertyName = "证券名称";
            this.Column证券名称.HeaderText = "证券名称";
            this.Column证券名称.Name = "Column证券名称";
            this.Column证券名称.ReadOnly = true;
            this.Column证券名称.Width = 80;
            // 
            // Column剩余数量
            // 
            this.Column剩余数量.DataPropertyName = "剩余数量";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N0";
            this.Column剩余数量.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column剩余数量.HeaderText = "剩余股数";
            this.Column剩余数量.Name = "Column剩余数量";
            this.Column剩余数量.ReadOnly = true;
            this.Column剩余数量.Width = 90;
            // 
            // Column剩余仓位
            // 
            this.Column剩余仓位.DataPropertyName = "剩余市值";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            this.Column剩余仓位.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column剩余仓位.HeaderText = "市值(元)";
            this.Column剩余仓位.Name = "Column剩余仓位";
            this.Column剩余仓位.ReadOnly = true;
            // 
            // Column昨收
            // 
            this.Column昨收.DataPropertyName = "昨收";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            this.Column昨收.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column昨收.HeaderText = "昨收(元)";
            this.Column昨收.Name = "Column昨收";
            this.Column昨收.ReadOnly = true;
            // 
            // Column操作
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            this.Column操作.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column操作.HeaderText = "操作";
            this.Column操作.Name = "Column操作";
            this.Column操作.ReadOnly = true;
            this.Column操作.Text = "锁定额度";
            this.Column操作.UseColumnTextForButtonValue = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button刷新);
            this.groupBox1.Controls.Add(this.textBox证券名称);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox证券代码);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(862, 54);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "过滤";
            // 
            // button刷新
            // 
            this.button刷新.Location = new System.Drawing.Point(277, 19);
            this.button刷新.Name = "button刷新";
            this.button刷新.Size = new System.Drawing.Size(75, 23);
            this.button刷新.TabIndex = 4;
            this.button刷新.Text = "刷新";
            this.button刷新.UseVisualStyleBackColor = true;
            this.button刷新.Click += new System.EventHandler(this.button刷新_Click);
            // 
            // textBox证券名称
            // 
            this.textBox证券名称.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox证券名称.Location = new System.Drawing.Point(198, 20);
            this.textBox证券名称.Name = "textBox证券名称";
            this.textBox证券名称.Size = new System.Drawing.Size(65, 21);
            this.textBox证券名称.TabIndex = 3;
            this.textBox证券名称.TextChanged += new System.EventHandler(this.textBox证券名称_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(140, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "证券名称";
            // 
            // textBox证券代码
            // 
            this.textBox证券代码.Location = new System.Drawing.Point(64, 20);
            this.textBox证券代码.Name = "textBox证券代码";
            this.textBox证券代码.Size = new System.Drawing.Size(62, 21);
            this.textBox证券代码.TabIndex = 1;
            this.textBox证券代码.TextChanged += new System.EventHandler(this.textBox证券代码_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "证券代码";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "组合号";
            this.dataGridViewTextBoxColumn1.HeaderText = "组合号";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 80;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "证券代码";
            this.dataGridViewTextBoxColumn2.HeaderText = "证券代码";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 80;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "证券名称";
            this.dataGridViewTextBoxColumn3.HeaderText = "证券名称";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 80;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "总仓位";
            dataGridViewCellStyle5.Format = "N0";
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn4.HeaderText = "总仓位";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 120;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "已用仓位";
            dataGridViewCellStyle6.Format = "N2";
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn5.HeaderText = "已用仓位";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 90;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "总仓位 - 已用数量";
            dataGridViewCellStyle7.Format = "N2";
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewTextBoxColumn6.HeaderText = "剩余数量";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.dataGridViewTextBoxColumn6.Width = 90;
            // 
            // dataGridViewButtonColumn1
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black;
            this.dataGridViewButtonColumn1.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewButtonColumn1.HeaderText = "操作";
            this.dataGridViewButtonColumn1.Name = "dataGridViewButtonColumn1";
            this.dataGridViewButtonColumn1.ReadOnly = true;
            this.dataGridViewButtonColumn1.Text = "锁定额度";
            this.dataGridViewButtonColumn1.UseColumnTextForButtonValue = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "剩余仓位";
            dataGridViewCellStyle9.Format = "N2";
            this.dataGridViewTextBoxColumn7.DefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridViewTextBoxColumn7.HeaderText = "剩余仓位";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "昨收";
            dataGridViewCellStyle10.Format = "N2";
            this.dataGridViewTextBoxColumn8.DefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridViewTextBoxColumn8.HeaderText = "昨收(元)";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // PositionList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 377);
            this.Controls.Add(this.panel1);
            this.Name = "PositionList";
            this.Text = "可用仓位列表";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView可用仓位)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView可用仓位;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox证券名称;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox证券代码;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelLoading;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn1;
        private System.Windows.Forms.Button button刷新;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column组合号;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column证券代码;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column证券名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column剩余数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column剩余仓位;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column昨收;
        private System.Windows.Forms.DataGridViewButtonColumn Column操作;
    }
}