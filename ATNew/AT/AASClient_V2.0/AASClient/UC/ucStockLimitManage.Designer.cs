namespace AASClient.UC
{
    partial class ucStockLimitManage
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelLoading = new System.Windows.Forms.Label();
            this.dataGridView交易额度 = new System.Windows.Forms.DataGridView();
            this.Column交易员 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column证券代码 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column组合号 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column市场 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column证券名称 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column拼音缩写 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column买模式 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column卖模式 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column交易额度 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column手续费率 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.contextMenuStrip交易额度 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全部显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonFilte = new System.Windows.Forms.Button();
            this.textBox交易员 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox证券名称 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox证券代码 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox组合号 = new System.Windows.Forms.ComboBox();
            this.bindingSource交易额度 = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewAutoFilterTextBoxColumn1 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.dataGridViewAutoFilterTextBoxColumn2 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.dataGridViewAutoFilterTextBoxColumn3 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.dataGridViewAutoFilterTextBoxColumn4 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.dataGridViewAutoFilterTextBoxColumn5 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.dataGridViewAutoFilterTextBoxColumn6 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.dataGridViewAutoFilterTextBoxColumn7 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.dataGridViewAutoFilterTextBoxColumn8 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.dataGridViewAutoFilterTextBoxColumn9 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.dataGridViewAutoFilterTextBoxColumn10 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView交易额度)).BeginInit();
            this.contextMenuStrip交易额度.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource交易额度)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.labelLoading);
            this.panel1.Controls.Add(this.dataGridView交易额度);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(885, 461);
            this.panel1.TabIndex = 0;
            // 
            // labelLoading
            // 
            this.labelLoading.AutoSize = true;
            this.labelLoading.Location = new System.Drawing.Point(364, 190);
            this.labelLoading.Name = "labelLoading";
            this.labelLoading.Size = new System.Drawing.Size(65, 12);
            this.labelLoading.TabIndex = 3;
            this.labelLoading.Text = "加载中……";
            // 
            // dataGridView交易额度
            // 
            this.dataGridView交易额度.AllowUserToAddRows = false;
            this.dataGridView交易额度.AllowUserToDeleteRows = false;
            this.dataGridView交易额度.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView交易额度.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column交易员,
            this.Column证券代码,
            this.Column组合号,
            this.Column市场,
            this.Column证券名称,
            this.Column拼音缩写,
            this.Column买模式,
            this.Column卖模式,
            this.Column交易额度,
            this.Column手续费率});
            this.dataGridView交易额度.ContextMenuStrip = this.contextMenuStrip交易额度;
            this.dataGridView交易额度.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView交易额度.Location = new System.Drawing.Point(0, 50);
            this.dataGridView交易额度.Name = "dataGridView交易额度";
            this.dataGridView交易额度.ReadOnly = true;
            this.dataGridView交易额度.RowHeadersVisible = false;
            this.dataGridView交易额度.RowTemplate.Height = 23;
            this.dataGridView交易额度.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView交易额度.Size = new System.Drawing.Size(883, 409);
            this.dataGridView交易额度.TabIndex = 2;
            this.dataGridView交易额度.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView交易额度_CellDoubleClick);
            this.dataGridView交易额度.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView交易额度_CellFormatting);
            // 
            // Column交易员
            // 
            this.Column交易员.DataPropertyName = "交易员";
            this.Column交易员.HeaderText = "交易员";
            this.Column交易员.Name = "Column交易员";
            this.Column交易员.ReadOnly = true;
            this.Column交易员.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column交易员.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // Column证券代码
            // 
            this.Column证券代码.DataPropertyName = "证券代码";
            this.Column证券代码.HeaderText = "证券代码";
            this.Column证券代码.Name = "Column证券代码";
            this.Column证券代码.ReadOnly = true;
            this.Column证券代码.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column证券代码.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // Column组合号
            // 
            this.Column组合号.DataPropertyName = "组合号";
            this.Column组合号.HeaderText = "组合号";
            this.Column组合号.Name = "Column组合号";
            this.Column组合号.ReadOnly = true;
            this.Column组合号.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column组合号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // Column市场
            // 
            this.Column市场.DataPropertyName = "市场";
            this.Column市场.HeaderText = "市场";
            this.Column市场.Name = "Column市场";
            this.Column市场.ReadOnly = true;
            this.Column市场.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column市场.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // Column证券名称
            // 
            this.Column证券名称.DataPropertyName = "证券名称";
            this.Column证券名称.HeaderText = "证券名称";
            this.Column证券名称.Name = "Column证券名称";
            this.Column证券名称.ReadOnly = true;
            this.Column证券名称.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column证券名称.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // Column拼音缩写
            // 
            this.Column拼音缩写.DataPropertyName = "拼音缩写";
            this.Column拼音缩写.HeaderText = "拼音缩写";
            this.Column拼音缩写.Name = "Column拼音缩写";
            this.Column拼音缩写.ReadOnly = true;
            this.Column拼音缩写.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column拼音缩写.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // Column买模式
            // 
            this.Column买模式.DataPropertyName = "买模式";
            this.Column买模式.HeaderText = "买模式";
            this.Column买模式.Name = "Column买模式";
            this.Column买模式.ReadOnly = true;
            this.Column买模式.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column买模式.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // Column卖模式
            // 
            this.Column卖模式.DataPropertyName = "卖模式";
            this.Column卖模式.HeaderText = "卖模式";
            this.Column卖模式.Name = "Column卖模式";
            this.Column卖模式.ReadOnly = true;
            this.Column卖模式.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column卖模式.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // Column交易额度
            // 
            this.Column交易额度.DataPropertyName = "交易额度";
            this.Column交易额度.HeaderText = "交易额度";
            this.Column交易额度.Name = "Column交易额度";
            this.Column交易额度.ReadOnly = true;
            this.Column交易额度.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column交易额度.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // Column手续费率
            // 
            this.Column手续费率.DataPropertyName = "手续费率";
            this.Column手续费率.HeaderText = "手续费率";
            this.Column手续费率.Name = "Column手续费率";
            this.Column手续费率.ReadOnly = true;
            this.Column手续费率.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column手续费率.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // contextMenuStrip交易额度
            // 
            this.contextMenuStrip交易额度.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.删除ToolStripMenuItem,
            this.修改ToolStripMenuItem,
            this.全部显示ToolStripMenuItem});
            this.contextMenuStrip交易额度.Name = "contextMenuStrip交易额度";
            this.contextMenuStrip交易额度.Size = new System.Drawing.Size(125, 92);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem1.Text = "新增";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.添加交易额度ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除交易额度ToolStripMenuItem_Click);
            // 
            // 修改ToolStripMenuItem
            // 
            this.修改ToolStripMenuItem.Name = "修改ToolStripMenuItem";
            this.修改ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.修改ToolStripMenuItem.Text = "修改";
            this.修改ToolStripMenuItem.Click += new System.EventHandler(this.修改交易额度ToolStripMenuItem_Click);
            // 
            // 全部显示ToolStripMenuItem
            // 
            this.全部显示ToolStripMenuItem.Name = "全部显示ToolStripMenuItem";
            this.全部显示ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.全部显示ToolStripMenuItem.Text = "全部显示";
            this.全部显示ToolStripMenuItem.Click += new System.EventHandler(this.全部显示ToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonFilte);
            this.groupBox1.Controls.Add(this.textBox交易员);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox证券名称);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox证券代码);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBox组合号);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(883, 50);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "过滤";
            // 
            // buttonFilte
            // 
            this.buttonFilte.Location = new System.Drawing.Point(662, 17);
            this.buttonFilte.Name = "buttonFilte";
            this.buttonFilte.Size = new System.Drawing.Size(75, 23);
            this.buttonFilte.TabIndex = 5;
            this.buttonFilte.Text = "过滤";
            this.buttonFilte.UseVisualStyleBackColor = true;
            this.buttonFilte.Click += new System.EventHandler(this.buttonFilte_Click);
            // 
            // textBox交易员
            // 
            this.textBox交易员.Location = new System.Drawing.Point(524, 19);
            this.textBox交易员.Name = "textBox交易员";
            this.textBox交易员.Size = new System.Drawing.Size(100, 21);
            this.textBox交易员.TabIndex = 4;
            this.textBox交易员.TextChanged += new System.EventHandler(this.textBox交易员_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(479, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "交易员";
            // 
            // textBox证券名称
            // 
            this.textBox证券名称.Location = new System.Drawing.Point(364, 19);
            this.textBox证券名称.Name = "textBox证券名称";
            this.textBox证券名称.Size = new System.Drawing.Size(100, 21);
            this.textBox证券名称.TabIndex = 3;
            this.textBox证券名称.TextChanged += new System.EventHandler(this.textBox证券名称_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(308, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "证券名称";
            // 
            // textBox证券代码
            // 
            this.textBox证券代码.Location = new System.Drawing.Point(191, 19);
            this.textBox证券代码.Name = "textBox证券代码";
            this.textBox证券代码.Size = new System.Drawing.Size(100, 21);
            this.textBox证券代码.TabIndex = 2;
            this.textBox证券代码.TextChanged += new System.EventHandler(this.textBox证券代码_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(135, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "证券代码";
            // 
            // comboBox组合号
            // 
            this.comboBox组合号.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox组合号.FormattingEnabled = true;
            this.comboBox组合号.Location = new System.Drawing.Point(18, 19);
            this.comboBox组合号.Name = "comboBox组合号";
            this.comboBox组合号.Size = new System.Drawing.Size(95, 20);
            this.comboBox组合号.TabIndex = 1;
            this.comboBox组合号.SelectedIndexChanged += new System.EventHandler(this.comboBox组合号_SelectedIndexChanged);
            // 
            // dataGridViewAutoFilterTextBoxColumn1
            // 
            this.dataGridViewAutoFilterTextBoxColumn1.DataPropertyName = "交易员";
            this.dataGridViewAutoFilterTextBoxColumn1.HeaderText = "交易员";
            this.dataGridViewAutoFilterTextBoxColumn1.Name = "dataGridViewAutoFilterTextBoxColumn1";
            this.dataGridViewAutoFilterTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAutoFilterTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // dataGridViewAutoFilterTextBoxColumn2
            // 
            this.dataGridViewAutoFilterTextBoxColumn2.DataPropertyName = "证券代码";
            this.dataGridViewAutoFilterTextBoxColumn2.HeaderText = "证券代码";
            this.dataGridViewAutoFilterTextBoxColumn2.Name = "dataGridViewAutoFilterTextBoxColumn2";
            this.dataGridViewAutoFilterTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAutoFilterTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // dataGridViewAutoFilterTextBoxColumn3
            // 
            this.dataGridViewAutoFilterTextBoxColumn3.DataPropertyName = "组合号";
            this.dataGridViewAutoFilterTextBoxColumn3.HeaderText = "组合号";
            this.dataGridViewAutoFilterTextBoxColumn3.Name = "dataGridViewAutoFilterTextBoxColumn3";
            this.dataGridViewAutoFilterTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAutoFilterTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // dataGridViewAutoFilterTextBoxColumn4
            // 
            this.dataGridViewAutoFilterTextBoxColumn4.DataPropertyName = "市场";
            this.dataGridViewAutoFilterTextBoxColumn4.HeaderText = "市场";
            this.dataGridViewAutoFilterTextBoxColumn4.Name = "dataGridViewAutoFilterTextBoxColumn4";
            this.dataGridViewAutoFilterTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAutoFilterTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // dataGridViewAutoFilterTextBoxColumn5
            // 
            this.dataGridViewAutoFilterTextBoxColumn5.DataPropertyName = "证券名称";
            this.dataGridViewAutoFilterTextBoxColumn5.HeaderText = "证券名称";
            this.dataGridViewAutoFilterTextBoxColumn5.Name = "dataGridViewAutoFilterTextBoxColumn5";
            this.dataGridViewAutoFilterTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAutoFilterTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // dataGridViewAutoFilterTextBoxColumn6
            // 
            this.dataGridViewAutoFilterTextBoxColumn6.DataPropertyName = "拼音缩写";
            this.dataGridViewAutoFilterTextBoxColumn6.HeaderText = "拼音缩写";
            this.dataGridViewAutoFilterTextBoxColumn6.Name = "dataGridViewAutoFilterTextBoxColumn6";
            this.dataGridViewAutoFilterTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAutoFilterTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // dataGridViewAutoFilterTextBoxColumn7
            // 
            this.dataGridViewAutoFilterTextBoxColumn7.DataPropertyName = "买模式";
            this.dataGridViewAutoFilterTextBoxColumn7.HeaderText = "买模式";
            this.dataGridViewAutoFilterTextBoxColumn7.Name = "dataGridViewAutoFilterTextBoxColumn7";
            this.dataGridViewAutoFilterTextBoxColumn7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAutoFilterTextBoxColumn7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // dataGridViewAutoFilterTextBoxColumn8
            // 
            this.dataGridViewAutoFilterTextBoxColumn8.DataPropertyName = "卖模式";
            this.dataGridViewAutoFilterTextBoxColumn8.HeaderText = "卖模式";
            this.dataGridViewAutoFilterTextBoxColumn8.Name = "dataGridViewAutoFilterTextBoxColumn8";
            this.dataGridViewAutoFilterTextBoxColumn8.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAutoFilterTextBoxColumn8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // dataGridViewAutoFilterTextBoxColumn9
            // 
            this.dataGridViewAutoFilterTextBoxColumn9.DataPropertyName = "交易额度";
            this.dataGridViewAutoFilterTextBoxColumn9.HeaderText = "交易额度";
            this.dataGridViewAutoFilterTextBoxColumn9.Name = "dataGridViewAutoFilterTextBoxColumn9";
            this.dataGridViewAutoFilterTextBoxColumn9.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAutoFilterTextBoxColumn9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // dataGridViewAutoFilterTextBoxColumn10
            // 
            this.dataGridViewAutoFilterTextBoxColumn10.DataPropertyName = "手续费率";
            this.dataGridViewAutoFilterTextBoxColumn10.HeaderText = "手续费率";
            this.dataGridViewAutoFilterTextBoxColumn10.Name = "dataGridViewAutoFilterTextBoxColumn10";
            this.dataGridViewAutoFilterTextBoxColumn10.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAutoFilterTextBoxColumn10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ucStockLimitManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "ucStockLimitManage";
            this.Size = new System.Drawing.Size(885, 461);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView交易额度)).EndInit();
            this.contextMenuStrip交易额度.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource交易额度)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView交易额度;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox交易员;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox证券名称;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox证券代码;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox组合号;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column交易员;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column证券代码;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column组合号;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column市场;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column证券名称;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column拼音缩写;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column买模式;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column卖模式;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column交易额度;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column手续费率;
        private System.Windows.Forms.Button buttonFilte;
        private System.Windows.Forms.BindingSource bindingSource交易额度;
        private System.Windows.Forms.Label labelLoading;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip交易额度;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 全部显示ToolStripMenuItem;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn dataGridViewAutoFilterTextBoxColumn1;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn dataGridViewAutoFilterTextBoxColumn2;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn dataGridViewAutoFilterTextBoxColumn3;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn dataGridViewAutoFilterTextBoxColumn4;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn dataGridViewAutoFilterTextBoxColumn5;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn dataGridViewAutoFilterTextBoxColumn6;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn dataGridViewAutoFilterTextBoxColumn7;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn dataGridViewAutoFilterTextBoxColumn8;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn dataGridViewAutoFilterTextBoxColumn9;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn dataGridViewAutoFilterTextBoxColumn10;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}
