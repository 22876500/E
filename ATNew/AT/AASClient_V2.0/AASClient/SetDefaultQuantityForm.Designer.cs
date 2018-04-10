namespace AASClient
{
    partial class SetDefaultQuantityForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown最大委托金额 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.numericUpDown默认委托股数 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown最大委托金额)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown默认委托股数)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "最大委托金额:";
            // 
            // numericUpDown最大委托金额
            // 
            this.numericUpDown最大委托金额.Increment = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown最大委托金额.Location = new System.Drawing.Point(147, 60);
            this.numericUpDown最大委托金额.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numericUpDown最大委托金额.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDown最大委托金额.Name = "numericUpDown最大委托金额";
            this.numericUpDown最大委托金额.Size = new System.Drawing.Size(140, 21);
            this.numericUpDown最大委托金额.TabIndex = 6;
            this.numericUpDown最大委托金额.Enter += new System.EventHandler(this.numericUpDown最大委托金额_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "默认委托股数:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(147, 100);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 33);
            this.button1.TabIndex = 8;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // numericUpDown默认委托股数
            // 
            this.numericUpDown默认委托股数.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown默认委托股数.Location = new System.Drawing.Point(147, 26);
            this.numericUpDown默认委托股数.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numericUpDown默认委托股数.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDown默认委托股数.Name = "numericUpDown默认委托股数";
            this.numericUpDown默认委托股数.Size = new System.Drawing.Size(140, 21);
            this.numericUpDown默认委托股数.TabIndex = 5;
            this.numericUpDown默认委托股数.Enter += new System.EventHandler(this.numericUpDown默认委托股数_Enter);
            // 
            // SetDefaultQuantityForm
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 159);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown最大委托金额);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.numericUpDown默认委托股数);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SetDefaultQuantityForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设置默认股数";
            this.Load += new System.EventHandler(this.SetDefaultQuantityForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown最大委托金额)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown默认委托股数)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.NumericUpDown numericUpDown最大委托金额;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.NumericUpDown numericUpDown默认委托股数;
    }
}