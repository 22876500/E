namespace AASClient.StockPosition
{
    partial class StockPositionEdit
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxStockID = new System.Windows.Forms.TextBox();
            this.comboBox组合号 = new System.Windows.Forms.ComboBox();
            this.textBoxStockName = new System.Windows.Forms.TextBox();
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.numericUpDown总仓位 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown总仓位)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "组合号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "证券代码";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(73, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "总仓位";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(61, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "证券名称";
            // 
            // textBoxStockID
            // 
            this.textBoxStockID.Location = new System.Drawing.Point(121, 67);
            this.textBoxStockID.Name = "textBoxStockID";
            this.textBoxStockID.Size = new System.Drawing.Size(100, 21);
            this.textBoxStockID.TabIndex = 2;
            this.textBoxStockID.Leave += new System.EventHandler(this.textBoxStockID_Leave);
            // 
            // comboBox组合号
            // 
            this.comboBox组合号.FormattingEnabled = true;
            this.comboBox组合号.Location = new System.Drawing.Point(120, 36);
            this.comboBox组合号.Name = "comboBox组合号";
            this.comboBox组合号.Size = new System.Drawing.Size(101, 20);
            this.comboBox组合号.TabIndex = 1;
            // 
            // textBoxStockName
            // 
            this.textBoxStockName.Location = new System.Drawing.Point(121, 97);
            this.textBoxStockName.Name = "textBoxStockName";
            this.textBoxStockName.Size = new System.Drawing.Size(100, 21);
            this.textBoxStockName.TabIndex = 3;
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Location = new System.Drawing.Point(120, 173);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(75, 23);
            this.buttonSubmit.TabIndex = 5;
            this.buttonSubmit.Text = "确定";
            this.buttonSubmit.UseVisualStyleBackColor = true;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
            // 
            // numericUpDown总仓位
            // 
            this.numericUpDown总仓位.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown总仓位.Location = new System.Drawing.Point(120, 129);
            this.numericUpDown总仓位.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numericUpDown总仓位.Name = "numericUpDown总仓位";
            this.numericUpDown总仓位.Size = new System.Drawing.Size(101, 21);
            this.numericUpDown总仓位.TabIndex = 4;
            // 
            // StockPositionEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 208);
            this.Controls.Add(this.numericUpDown总仓位);
            this.Controls.Add(this.buttonSubmit);
            this.Controls.Add(this.textBoxStockName);
            this.Controls.Add(this.comboBox组合号);
            this.Controls.Add(this.textBoxStockID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "StockPositionEdit";
            this.Text = "可用仓位新增";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown总仓位)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxStockID;
        private System.Windows.Forms.ComboBox comboBox组合号;
        private System.Windows.Forms.TextBox textBoxStockName;
        private System.Windows.Forms.Button buttonSubmit;
        private System.Windows.Forms.NumericUpDown numericUpDown总仓位;
    }
}