namespace AASClient
{
    partial class ShareLimitListForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBoxGroup = new System.Windows.Forms.ComboBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridViewStock = new System.Windows.Forms.DataGridView();
            this.ColumnGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStockID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column股票名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column额度 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSourceTrader = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceStock = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTrader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceStock)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBoxGroup);
            this.panel1.Controls.Add(this.buttonSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(799, 70);
            this.panel1.TabIndex = 0;
            // 
            // comboBoxGroup
            // 
            this.comboBoxGroup.FormattingEnabled = true;
            this.comboBoxGroup.Location = new System.Drawing.Point(25, 26);
            this.comboBoxGroup.Name = "comboBoxGroup";
            this.comboBoxGroup.Size = new System.Drawing.Size(121, 20);
            this.comboBoxGroup.TabIndex = 1;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(152, 23);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 0;
            this.buttonSearch.Text = "查询";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridViewStock);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 70);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(799, 329);
            this.panel2.TabIndex = 1;
            // 
            // dataGridViewStock
            // 
            this.dataGridViewStock.AllowUserToAddRows = false;
            this.dataGridViewStock.AllowUserToDeleteRows = false;
            this.dataGridViewStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStock.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnGroup,
            this.ColumnStockID,
            this.Column股票名称,
            this.Column额度});
            this.dataGridViewStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewStock.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewStock.Name = "dataGridViewStock";
            this.dataGridViewStock.RowTemplate.Height = 23;
            this.dataGridViewStock.Size = new System.Drawing.Size(799, 329);
            this.dataGridViewStock.TabIndex = 0;
            // 
            // ColumnGroup
            // 
            this.ColumnGroup.DataPropertyName = "GroupAccount";
            this.ColumnGroup.HeaderText = "组合号";
            this.ColumnGroup.Name = "ColumnGroup";
            // 
            // ColumnStockID
            // 
            this.ColumnStockID.DataPropertyName = "StockID";
            this.ColumnStockID.HeaderText = "股票代码";
            this.ColumnStockID.Name = "ColumnStockID";
            // 
            // Column股票名称
            // 
            this.Column股票名称.DataPropertyName = "StockName";
            this.Column股票名称.HeaderText = "股票名称";
            this.Column股票名称.Name = "Column股票名称";
            // 
            // Column额度
            // 
            this.Column额度.DataPropertyName = "LimitCount";
            this.Column额度.HeaderText = "股票额度";
            this.Column额度.Name = "Column额度";
            // 
            // ShareLimitListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 399);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ShareLimitListForm";
            this.Text = "ShareLimitListForm";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTrader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceStock)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBoxGroup;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridViewStock;
        private System.Windows.Forms.BindingSource bindingSourceTrader;
        private System.Windows.Forms.BindingSource bindingSourceStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStockID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column股票名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column额度;
    }
}