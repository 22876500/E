namespace AASClient.UC
{
    partial class ucUserAccountSet
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelLoading = new System.Windows.Forms.Label();
            this.listBoxAccounts = new System.Windows.Forms.ListBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridViewSelected = new System.Windows.Forms.DataGridView();
            this.Column已绑定交易员 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column选中 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridViewNotSelected = new System.Windows.Forms.DataGridView();
            this.Column未绑定交易员 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column是否绑定 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelected)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewNotSelected)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.labelLoading);
            this.panel1.Controls.Add(this.listBoxAccounts);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(842, 498);
            this.panel1.TabIndex = 0;
            // 
            // labelLoading
            // 
            this.labelLoading.AutoSize = true;
            this.labelLoading.Location = new System.Drawing.Point(42, 180);
            this.labelLoading.Name = "labelLoading";
            this.labelLoading.Size = new System.Drawing.Size(65, 12);
            this.labelLoading.TabIndex = 2;
            this.labelLoading.Text = "加载中……";
            // 
            // listBoxAccounts
            // 
            this.listBoxAccounts.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBoxAccounts.FormattingEnabled = true;
            this.listBoxAccounts.ItemHeight = 12;
            this.listBoxAccounts.Location = new System.Drawing.Point(0, 0);
            this.listBoxAccounts.Name = "listBoxAccounts";
            this.listBoxAccounts.Size = new System.Drawing.Size(139, 496);
            this.listBoxAccounts.TabIndex = 0;
            this.listBoxAccounts.SelectedIndexChanged += new System.EventHandler(this.listBoxTraders_SelectedIndexChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "AccountName";
            this.dataGridViewTextBoxColumn1.HeaderText = "组合号";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "IsSelected";
            this.dataGridViewCheckBoxColumn1.HeaderText = "已添加";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "交易员";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.HeaderText = "选中";
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(139, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(701, 10);
            this.panel2.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(139, 10);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(701, 486);
            this.splitContainer1.SplitterDistance = 242;
            this.splitContainer1.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridViewSelected);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(701, 242);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "已分配";
            // 
            // dataGridViewSelected
            // 
            this.dataGridViewSelected.AllowUserToAddRows = false;
            this.dataGridViewSelected.AllowUserToDeleteRows = false;
            this.dataGridViewSelected.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSelected.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column已绑定交易员,
            this.Column选中});
            this.dataGridViewSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSelected.Location = new System.Drawing.Point(3, 17);
            this.dataGridViewSelected.Name = "dataGridViewSelected";
            this.dataGridViewSelected.RowHeadersWidth = 20;
            this.dataGridViewSelected.RowTemplate.Height = 23;
            this.dataGridViewSelected.Size = new System.Drawing.Size(695, 222);
            this.dataGridViewSelected.TabIndex = 0;
            this.dataGridViewSelected.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSelected_CellContentClick);
            // 
            // Column已绑定交易员
            // 
            this.Column已绑定交易员.HeaderText = "交易员";
            this.Column已绑定交易员.Name = "Column已绑定交易员";
            // 
            // Column选中
            // 
            this.Column选中.DataPropertyName = "Selected";
            this.Column选中.HeaderText = "选中";
            this.Column选中.Name = "Column选中";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridViewNotSelected);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(701, 240);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "未分配";
            // 
            // dataGridViewNotSelected
            // 
            this.dataGridViewNotSelected.AllowUserToAddRows = false;
            this.dataGridViewNotSelected.AllowUserToDeleteRows = false;
            this.dataGridViewNotSelected.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewNotSelected.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column未绑定交易员,
            this.Column是否绑定});
            this.dataGridViewNotSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewNotSelected.Location = new System.Drawing.Point(3, 17);
            this.dataGridViewNotSelected.Name = "dataGridViewNotSelected";
            this.dataGridViewNotSelected.ReadOnly = true;
            this.dataGridViewNotSelected.RowHeadersWidth = 22;
            this.dataGridViewNotSelected.RowTemplate.Height = 23;
            this.dataGridViewNotSelected.Size = new System.Drawing.Size(695, 220);
            this.dataGridViewNotSelected.TabIndex = 1;
            this.dataGridViewNotSelected.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewNotSelected_CellContentClick);
            // 
            // Column未绑定交易员
            // 
            this.Column未绑定交易员.HeaderText = "交易员";
            this.Column未绑定交易员.Name = "Column未绑定交易员";
            this.Column未绑定交易员.ReadOnly = true;
            // 
            // Column是否绑定
            // 
            this.Column是否绑定.HeaderText = "选中";
            this.Column是否绑定.Name = "Column是否绑定";
            this.Column是否绑定.ReadOnly = true;
            // 
            // ucUserAccountSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "ucUserAccountSet";
            this.Size = new System.Drawing.Size(842, 498);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelected)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewNotSelected)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listBoxAccounts;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.Label labelLoading;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridViewSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column已绑定交易员;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column选中;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridViewNotSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column未绑定交易员;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column是否绑定;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
    }
}
