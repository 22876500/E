namespace AASClient.UC
{
    partial class ucPositionShow
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.textBox证券代码过滤 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox券商账户过滤 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridView可用仓位 = new System.Windows.Forms.DataGridView();
            this.Column组合号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column证券代码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column证券名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column总仓位 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column已用数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column剩余数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column剩余市值 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column昨收 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column操作 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.contextMenuStrip可用仓位 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新增ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bindingSource可用仓位 = new System.Windows.Forms.BindingSource(this.components);
            this.labelPositionLoading = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewButtonColumn1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView可用仓位)).BeginInit();
            this.contextMenuStrip可用仓位.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource可用仓位)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonAdd);
            this.groupBox1.Controls.Add(this.buttonEdit);
            this.groupBox1.Controls.Add(this.buttonDelete);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.buttonSearch);
            this.groupBox1.Controls.Add(this.textBox证券代码过滤);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBox券商账户过滤);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(743, 59);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(573, 22);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(48, 23);
            this.buttonAdd.TabIndex = 8;
            this.buttonAdd.Text = "新增";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Location = new System.Drawing.Point(519, 22);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(48, 23);
            this.buttonEdit.TabIndex = 7;
            this.buttonEdit.Text = "修改";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(464, 22);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(48, 23);
            this.buttonDelete.TabIndex = 6;
            this.buttonDelete.Text = "删除";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(627, 22);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(60, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "导入";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(410, 22);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(48, 23);
            this.buttonSearch.TabIndex = 4;
            this.buttonSearch.Text = "过滤";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonFilte_Click);
            // 
            // textBox证券代码过滤
            // 
            this.textBox证券代码过滤.Location = new System.Drawing.Point(182, 23);
            this.textBox证券代码过滤.Name = "textBox证券代码过滤";
            this.textBox证券代码过滤.Size = new System.Drawing.Size(58, 21);
            this.textBox证券代码过滤.TabIndex = 3;
            this.textBox证券代码过滤.TextChanged += new System.EventHandler(this.textBox证券代码过滤_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(128, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "证券代码";
            // 
            // textBox券商账户过滤
            // 
            this.textBox券商账户过滤.Location = new System.Drawing.Point(61, 23);
            this.textBox券商账户过滤.Name = "textBox券商账户过滤";
            this.textBox券商账户过滤.Size = new System.Drawing.Size(49, 21);
            this.textBox券商账户过滤.TabIndex = 1;
            this.textBox券商账户过滤.TextChanged += new System.EventHandler(this.textBox券商账户过滤_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "券商账户";
            // 
            // dataGridView可用仓位
            // 
            this.dataGridView可用仓位.AllowUserToAddRows = false;
            this.dataGridView可用仓位.AllowUserToDeleteRows = false;
            this.dataGridView可用仓位.AllowUserToResizeRows = false;
            this.dataGridView可用仓位.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView可用仓位.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column组合号,
            this.Column证券代码,
            this.Column证券名称,
            this.Column总仓位,
            this.Column已用数量,
            this.Column剩余数量,
            this.Column剩余市值,
            this.Column昨收,
            this.Column操作});
            this.dataGridView可用仓位.ContextMenuStrip = this.contextMenuStrip可用仓位;
            this.dataGridView可用仓位.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView可用仓位.Location = new System.Drawing.Point(0, 59);
            this.dataGridView可用仓位.Name = "dataGridView可用仓位";
            this.dataGridView可用仓位.ReadOnly = true;
            this.dataGridView可用仓位.RowHeadersVisible = false;
            this.dataGridView可用仓位.RowTemplate.Height = 23;
            this.dataGridView可用仓位.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView可用仓位.Size = new System.Drawing.Size(743, 410);
            this.dataGridView可用仓位.TabIndex = 3;
            this.dataGridView可用仓位.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView可用仓位_CellContentClick);
            this.dataGridView可用仓位.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView可用仓位_CellMouseDoubleClick);
            // 
            // Column组合号
            // 
            this.Column组合号.DataPropertyName = "组合号";
            this.Column组合号.HeaderText = "组合号";
            this.Column组合号.Name = "Column组合号";
            this.Column组合号.ReadOnly = true;
            // 
            // Column证券代码
            // 
            this.Column证券代码.DataPropertyName = "证券代码";
            this.Column证券代码.HeaderText = "证券代码";
            this.Column证券代码.Name = "Column证券代码";
            this.Column证券代码.ReadOnly = true;
            // 
            // Column证券名称
            // 
            this.Column证券名称.DataPropertyName = "证券名称";
            this.Column证券名称.HeaderText = "证券名称";
            this.Column证券名称.Name = "Column证券名称";
            this.Column证券名称.ReadOnly = true;
            // 
            // Column总仓位
            // 
            this.Column总仓位.DataPropertyName = "总仓位";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N0";
            this.Column总仓位.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column总仓位.HeaderText = "总仓位";
            this.Column总仓位.Name = "Column总仓位";
            this.Column总仓位.ReadOnly = true;
            // 
            // Column已用数量
            // 
            this.Column已用数量.DataPropertyName = "已用数量";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N0";
            this.Column已用数量.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column已用数量.HeaderText = "已用数量";
            this.Column已用数量.Name = "Column已用数量";
            this.Column已用数量.ReadOnly = true;
            // 
            // Column剩余数量
            // 
            this.Column剩余数量.DataPropertyName = "剩余数量";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N0";
            this.Column剩余数量.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column剩余数量.HeaderText = "剩余数量";
            this.Column剩余数量.Name = "Column剩余数量";
            this.Column剩余数量.ReadOnly = true;
            // 
            // Column剩余市值
            // 
            this.Column剩余市值.DataPropertyName = "剩余市值";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            this.Column剩余市值.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column剩余市值.HeaderText = "剩余市值";
            this.Column剩余市值.Name = "Column剩余市值";
            this.Column剩余市值.ReadOnly = true;
            // 
            // Column昨收
            // 
            this.Column昨收.DataPropertyName = "昨收";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            this.Column昨收.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column昨收.HeaderText = "昨收";
            this.Column昨收.Name = "Column昨收";
            this.Column昨收.ReadOnly = true;
            // 
            // Column操作
            // 
            this.Column操作.HeaderText = "操作";
            this.Column操作.Name = "Column操作";
            this.Column操作.ReadOnly = true;
            this.Column操作.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column操作.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column操作.Text = "已用仓位详情";
            this.Column操作.UseColumnTextForButtonValue = true;
            // 
            // contextMenuStrip可用仓位
            // 
            this.contextMenuStrip可用仓位.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem,
            this.新增ToolStripMenuItem,
            this.修改ToolStripMenuItem,
            this.刷新ToolStripMenuItem});
            this.contextMenuStrip可用仓位.Name = "contextMenuStrip可用仓位";
            this.contextMenuStrip可用仓位.Size = new System.Drawing.Size(101, 92);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // 新增ToolStripMenuItem
            // 
            this.新增ToolStripMenuItem.Name = "新增ToolStripMenuItem";
            this.新增ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.新增ToolStripMenuItem.Text = "新增";
            this.新增ToolStripMenuItem.Click += new System.EventHandler(this.新增ToolStripMenuItem_Click);
            // 
            // 修改ToolStripMenuItem
            // 
            this.修改ToolStripMenuItem.Name = "修改ToolStripMenuItem";
            this.修改ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.修改ToolStripMenuItem.Text = "修改";
            this.修改ToolStripMenuItem.Click += new System.EventHandler(this.修改ToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // labelPositionLoading
            // 
            this.labelPositionLoading.AutoSize = true;
            this.labelPositionLoading.Location = new System.Drawing.Point(333, 228);
            this.labelPositionLoading.Name = "labelPositionLoading";
            this.labelPositionLoading.Size = new System.Drawing.Size(77, 12);
            this.labelPositionLoading.TabIndex = 4;
            this.labelPositionLoading.Text = "正在加载……";
            this.labelPositionLoading.Visible = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "组合号";
            this.dataGridViewTextBoxColumn1.HeaderText = "组合号";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "证券代码";
            this.dataGridViewTextBoxColumn2.HeaderText = "证券代码";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "证券名称";
            this.dataGridViewTextBoxColumn3.HeaderText = "证券名称";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "总仓位";
            this.dataGridViewTextBoxColumn4.HeaderText = "总仓位";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "已用数量";
            this.dataGridViewTextBoxColumn5.HeaderText = "已用数量";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "剩余数量";
            this.dataGridViewTextBoxColumn6.HeaderText = "剩余数量";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "剩余市值";
            this.dataGridViewTextBoxColumn7.HeaderText = "剩余市值";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "昨收";
            this.dataGridViewTextBoxColumn8.HeaderText = "昨收";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewButtonColumn1
            // 
            this.dataGridViewButtonColumn1.HeaderText = "操作";
            this.dataGridViewButtonColumn1.Name = "dataGridViewButtonColumn1";
            this.dataGridViewButtonColumn1.ReadOnly = true;
            this.dataGridViewButtonColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewButtonColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewButtonColumn1.Text = "查看";
            this.dataGridViewButtonColumn1.UseColumnTextForButtonValue = true;
            // 
            // ucPositionShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelPositionLoading);
            this.Controls.Add(this.dataGridView可用仓位);
            this.Controls.Add(this.groupBox1);
            this.Name = "ucPositionShow";
            this.Size = new System.Drawing.Size(743, 469);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView可用仓位)).EndInit();
            this.contextMenuStrip可用仓位.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource可用仓位)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.TextBox textBox证券代码过滤;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox券商账户过滤;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridView可用仓位;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip可用仓位;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.BindingSource bindingSource可用仓位;
        private System.Windows.Forms.Label labelPositionLoading;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn1;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.ToolStripMenuItem 新增ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改ToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column组合号;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column证券代码;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column证券名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column总仓位;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column已用数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column剩余数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column剩余市值;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column昨收;
        private System.Windows.Forms.DataGridViewButtonColumn Column操作;
    }
}
