namespace AASClient
{
    partial class ShareLimitStockItemEdit
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
            this.comboBoxSale = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxBuy = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownCommission = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxGroups = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownLimit = new System.Windows.Forms.NumericUpDown();
            this.textBoxStockName = new System.Windows.Forms.TextBox();
            this.textBoxStockCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCommission)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxSale
            // 
            this.comboBoxSale.FormattingEnabled = true;
            this.comboBoxSale.Items.AddRange(new object[] {
            "卖出",
            "融券卖出"});
            this.comboBoxSale.Location = new System.Drawing.Point(121, 202);
            this.comboBoxSale.Name = "comboBoxSale";
            this.comboBoxSale.Size = new System.Drawing.Size(100, 20);
            this.comboBoxSale.TabIndex = 29;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(63, 206);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 28;
            this.label7.Text = "卖出方式：";
            // 
            // comboBoxBuy
            // 
            this.comboBoxBuy.FormattingEnabled = true;
            this.comboBoxBuy.Items.AddRange(new object[] {
            "买入",
            "融资买入"});
            this.comboBoxBuy.Location = new System.Drawing.Point(121, 174);
            this.comboBoxBuy.Name = "comboBoxBuy";
            this.comboBoxBuy.Size = new System.Drawing.Size(100, 20);
            this.comboBoxBuy.TabIndex = 27;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(63, 178);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 26;
            this.label6.Text = "买入方式：";
            // 
            // numericUpDownCommission
            // 
            this.numericUpDownCommission.DecimalPlaces = 5;
            this.numericUpDownCommission.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numericUpDownCommission.Location = new System.Drawing.Point(121, 143);
            this.numericUpDownCommission.Name = "numericUpDownCommission";
            this.numericUpDownCommission.Size = new System.Drawing.Size(100, 21);
            this.numericUpDownCommission.TabIndex = 25;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(62, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 24;
            this.label5.Text = "手续费率：";
            // 
            // comboBoxGroups
            // 
            this.comboBoxGroups.FormattingEnabled = true;
            this.comboBoxGroups.Location = new System.Drawing.Point(121, 28);
            this.comboBoxGroups.Name = "comboBoxGroups";
            this.comboBoxGroups.Size = new System.Drawing.Size(100, 20);
            this.comboBoxGroups.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(73, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 22;
            this.label4.Text = "组合号：";
            // 
            // numericUpDownLimit
            // 
            this.numericUpDownLimit.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownLimit.Location = new System.Drawing.Point(121, 114);
            this.numericUpDownLimit.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDownLimit.Name = "numericUpDownLimit";
            this.numericUpDownLimit.Size = new System.Drawing.Size(100, 21);
            this.numericUpDownLimit.TabIndex = 21;
            // 
            // textBoxStockName
            // 
            this.textBoxStockName.Location = new System.Drawing.Point(121, 83);
            this.textBoxStockName.Name = "textBoxStockName";
            this.textBoxStockName.Size = new System.Drawing.Size(100, 21);
            this.textBoxStockName.TabIndex = 20;
            // 
            // textBoxStockCode
            // 
            this.textBoxStockCode.Location = new System.Drawing.Point(121, 55);
            this.textBoxStockCode.Name = "textBoxStockCode";
            this.textBoxStockCode.Size = new System.Drawing.Size(100, 21);
            this.textBoxStockCode.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(62, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "分配额度：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(62, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "股票名称：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 16;
            this.label1.Text = "股票代码：";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(117, 241);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 30;
            this.button1.Text = "保存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ShareLimitStockItemEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 282);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBoxSale);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBoxBuy);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numericUpDownCommission);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxGroups);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDownLimit);
            this.Controls.Add(this.textBoxStockName);
            this.Controls.Add(this.textBoxStockCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ShareLimitStockItemEdit";
            this.Text = "共享额度编辑";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCommission)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxSale;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxBuy;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownCommission;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxGroups;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownLimit;
        private System.Windows.Forms.TextBox textBoxStockName;
        private System.Windows.Forms.TextBox textBoxStockCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}