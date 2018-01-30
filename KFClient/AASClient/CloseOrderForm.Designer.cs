namespace AASClient
{
    partial class CloseOrderForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.textBox证券代码 = new System.Windows.Forms.TextBox();
            this.textBox证券名称 = new System.Windows.Forms.TextBox();
            this.numericUpDown平仓价位 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown开仓数量 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDown开仓价位 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown平仓数量 = new System.Windows.Forms.NumericUpDown();
            this.comboBox开仓类别 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox交易员 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown平仓价位)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown开仓数量)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown开仓价位)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown平仓数量)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "证券代码";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(64, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "证券名称";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 193);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "平仓数量";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(64, 219);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "平仓价位";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(123, 254);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "平仓";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox证券代码
            // 
            this.textBox证券代码.Location = new System.Drawing.Point(123, 44);
            this.textBox证券代码.Name = "textBox证券代码";
            this.textBox证券代码.ReadOnly = true;
            this.textBox证券代码.Size = new System.Drawing.Size(100, 21);
            this.textBox证券代码.TabIndex = 5;
            // 
            // textBox证券名称
            // 
            this.textBox证券名称.Location = new System.Drawing.Point(123, 71);
            this.textBox证券名称.Name = "textBox证券名称";
            this.textBox证券名称.ReadOnly = true;
            this.textBox证券名称.Size = new System.Drawing.Size(100, 21);
            this.textBox证券名称.TabIndex = 6;
            // 
            // numericUpDown平仓价位
            // 
            this.numericUpDown平仓价位.DecimalPlaces = 3;
            this.numericUpDown平仓价位.Location = new System.Drawing.Point(123, 217);
            this.numericUpDown平仓价位.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDown平仓价位.Name = "numericUpDown平仓价位";
            this.numericUpDown平仓价位.Size = new System.Drawing.Size(100, 21);
            this.numericUpDown平仓价位.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(64, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "开仓类别";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(64, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "开仓数量";
            // 
            // numericUpDown开仓数量
            // 
            this.numericUpDown开仓数量.Enabled = false;
            this.numericUpDown开仓数量.Location = new System.Drawing.Point(123, 125);
            this.numericUpDown开仓数量.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDown开仓数量.Name = "numericUpDown开仓数量";
            this.numericUpDown开仓数量.Size = new System.Drawing.Size(100, 21);
            this.numericUpDown开仓数量.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(66, 155);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "开仓价位";
            // 
            // numericUpDown开仓价位
            // 
            this.numericUpDown开仓价位.DecimalPlaces = 3;
            this.numericUpDown开仓价位.Enabled = false;
            this.numericUpDown开仓价位.Location = new System.Drawing.Point(123, 152);
            this.numericUpDown开仓价位.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDown开仓价位.Name = "numericUpDown开仓价位";
            this.numericUpDown开仓价位.Size = new System.Drawing.Size(100, 21);
            this.numericUpDown开仓价位.TabIndex = 14;
            // 
            // numericUpDown平仓数量
            // 
            this.numericUpDown平仓数量.Enabled = false;
            this.numericUpDown平仓数量.Location = new System.Drawing.Point(123, 190);
            this.numericUpDown平仓数量.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDown平仓数量.Name = "numericUpDown平仓数量";
            this.numericUpDown平仓数量.Size = new System.Drawing.Size(100, 21);
            this.numericUpDown平仓数量.TabIndex = 15;
            // 
            // comboBox开仓类别
            // 
            this.comboBox开仓类别.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox开仓类别.Enabled = false;
            this.comboBox开仓类别.FormattingEnabled = true;
            this.comboBox开仓类别.Items.AddRange(new object[] {
            "买",
            "卖",
            "融资买入",
            "融券卖出"});
            this.comboBox开仓类别.Location = new System.Drawing.Point(123, 98);
            this.comboBox开仓类别.Name = "comboBox开仓类别";
            this.comboBox开仓类别.Size = new System.Drawing.Size(100, 20);
            this.comboBox开仓类别.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(76, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 17;
            this.label8.Text = "交易员";
            // 
            // textBox交易员
            // 
            this.textBox交易员.Location = new System.Drawing.Point(123, 17);
            this.textBox交易员.Name = "textBox交易员";
            this.textBox交易员.ReadOnly = true;
            this.textBox交易员.Size = new System.Drawing.Size(100, 21);
            this.textBox交易员.TabIndex = 18;
            // 
            // CloseOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 294);
            this.Controls.Add(this.textBox交易员);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboBox开仓类别);
            this.Controls.Add(this.numericUpDown平仓数量);
            this.Controls.Add(this.numericUpDown开仓价位);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numericUpDown开仓数量);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericUpDown平仓价位);
            this.Controls.Add(this.textBox证券名称);
            this.Controls.Add(this.textBox证券代码);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "CloseOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "平仓碎股";
            this.Load += new System.EventHandler(this.CloseOrderForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown平仓价位)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown开仓数量)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown开仓价位)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown平仓数量)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox证券代码;
        private System.Windows.Forms.TextBox textBox证券名称;
        private System.Windows.Forms.NumericUpDown numericUpDown平仓价位;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown开仓数量;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericUpDown开仓价位;
        private System.Windows.Forms.NumericUpDown numericUpDown平仓数量;
        private System.Windows.Forms.ComboBox comboBox开仓类别;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox交易员;
    }
}