namespace AASClient
{
    partial class PubStocksForm
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
            this.dataGridView公共券池 = new System.Windows.Forms.DataGridView();
            this.StockCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CanSaleCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource公共券池 = new System.Windows.Forms.BindingSource(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxStockCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView公共券池)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource公共券池)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView公共券池
            // 
            this.dataGridView公共券池.AllowUserToAddRows = false;
            this.dataGridView公共券池.AllowUserToDeleteRows = false;
            this.dataGridView公共券池.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView公共券池.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView公共券池.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StockCode,
            this.StockName,
            this.CanSaleCount});
            this.dataGridView公共券池.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView公共券池.Location = new System.Drawing.Point(0, 0);
            this.dataGridView公共券池.Name = "dataGridView公共券池";
            this.dataGridView公共券池.RowTemplate.Height = 23;
            this.dataGridView公共券池.Size = new System.Drawing.Size(424, 397);
            this.dataGridView公共券池.TabIndex = 0;
            this.dataGridView公共券池.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView公共券池_CellDoubleClick);
            // 
            // StockCode
            // 
            this.StockCode.DataPropertyName = "StockCode";
            this.StockCode.HeaderText = "证券代码";
            this.StockCode.Name = "StockCode";
            // 
            // StockName
            // 
            this.StockName.DataPropertyName = "StockName";
            this.StockName.HeaderText = "证券名称";
            this.StockName.Name = "StockName";
            // 
            // CanSaleCount
            // 
            this.CanSaleCount.DataPropertyName = "CanSaleCount";
            this.CanSaleCount.HeaderText = "可卖数量";
            this.CanSaleCount.Name = "CanSaleCount";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBoxStockCode);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(424, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(292, 397);
            this.panel1.TabIndex = 1;
            // 
            // textBoxStockCode
            // 
            this.textBoxStockCode.Location = new System.Drawing.Point(115, 30);
            this.textBoxStockCode.Name = "textBoxStockCode";
            this.textBoxStockCode.Size = new System.Drawing.Size(117, 21);
            this.textBoxStockCode.TabIndex = 1;
            this.textBoxStockCode.TextChanged += new System.EventHandler(this.textBoxStockCode_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "股票代码:";
            // 
            // PubStocksForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 397);
            this.Controls.Add(this.dataGridView公共券池);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "PubStocksForm";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
            this.Text = "公共券池";
            this.Load += new System.EventHandler(this.PubStocks_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView公共券池)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource公共券池)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView公共券池;
        private System.Windows.Forms.BindingSource bindingSource公共券池;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn StockCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn StockName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CanSaleCount;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxStockCode;
        private System.Windows.Forms.Label label1;
    }
}