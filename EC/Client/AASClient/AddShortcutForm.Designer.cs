namespace AASClient
{
    partial class AddShortcutForm
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
            this.textBox键名 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox价格 = new System.Windows.Forms.ComboBox();
            this.comboBox价差模式 = new System.Windows.Forms.ComboBox();
            this.numericUpDown价差数值 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox股数模式 = new System.Windows.Forms.ComboBox();
            this.numericUpDown股数数值 = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox方向 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown价差数值)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown股数数值)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(123, 56);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "键名";
            // 
            // textBox键名
            // 
            this.textBox键名.Location = new System.Drawing.Point(200, 51);
            this.textBox键名.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox键名.Name = "textBox键名";
            this.textBox键名.Size = new System.Drawing.Size(95, 29);
            this.textBox键名.TabIndex = 1;
            this.textBox键名.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox键名_KeyDown);
            this.textBox键名.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox键名_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(123, 151);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "价格";
            // 
            // comboBox价格
            // 
            this.comboBox价格.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox价格.FormattingEnabled = true;
            this.comboBox价格.Location = new System.Drawing.Point(200, 146);
            this.comboBox价格.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBox价格.Name = "comboBox价格";
            this.comboBox价格.Size = new System.Drawing.Size(95, 29);
            this.comboBox价格.TabIndex = 3;
            // 
            // comboBox价差模式
            // 
            this.comboBox价差模式.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox价差模式.FormattingEnabled = true;
            this.comboBox价差模式.Location = new System.Drawing.Point(309, 146);
            this.comboBox价差模式.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBox价差模式.Name = "comboBox价差模式";
            this.comboBox价差模式.Size = new System.Drawing.Size(104, 29);
            this.comboBox价差模式.TabIndex = 5;
            // 
            // numericUpDown价差数值
            // 
            this.numericUpDown价差数值.DecimalPlaces = 2;
            this.numericUpDown价差数值.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown价差数值.Location = new System.Drawing.Point(424, 147);
            this.numericUpDown价差数值.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDown价差数值.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown价差数值.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDown价差数值.Name = "numericUpDown价差数值";
            this.numericUpDown价差数值.Size = new System.Drawing.Size(127, 29);
            this.numericUpDown价差数值.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(123, 196);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 21);
            this.label4.TabIndex = 7;
            this.label4.Text = "股数";
            // 
            // comboBox股数模式
            // 
            this.comboBox股数模式.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox股数模式.FormattingEnabled = true;
            this.comboBox股数模式.Location = new System.Drawing.Point(200, 190);
            this.comboBox股数模式.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBox股数模式.Name = "comboBox股数模式";
            this.comboBox股数模式.Size = new System.Drawing.Size(213, 29);
            this.comboBox股数模式.TabIndex = 8;
            // 
            // numericUpDown股数数值
            // 
            this.numericUpDown股数数值.DecimalPlaces = 2;
            this.numericUpDown股数数值.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown股数数值.Location = new System.Drawing.Point(424, 189);
            this.numericUpDown股数数值.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDown股数数值.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDown股数数值.Name = "numericUpDown股数数值";
            this.numericUpDown股数数值.Size = new System.Drawing.Size(127, 29);
            this.numericUpDown股数数值.TabIndex = 9;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(200, 252);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 41);
            this.button1.TabIndex = 10;
            this.button1.Text = "确定添加";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox方向
            // 
            this.comboBox方向.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox方向.FormattingEnabled = true;
            this.comboBox方向.Location = new System.Drawing.Point(200, 100);
            this.comboBox方向.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBox方向.Name = "comboBox方向";
            this.comboBox方向.Size = new System.Drawing.Size(95, 29);
            this.comboBox方向.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(123, 105);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 21);
            this.label3.TabIndex = 12;
            this.label3.Text = "方向";
            // 
            // AddShortcutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 356);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox方向);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.numericUpDown股数数值);
            this.Controls.Add(this.comboBox股数模式);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDown价差数值);
            this.Controls.Add(this.comboBox价差模式);
            this.Controls.Add(this.comboBox价格);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox键名);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "AddShortcutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加快捷键";
            this.Load += new System.EventHandler(this.AddShortcutForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown价差数值)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown股数数值)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox键名;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox价格;
        private System.Windows.Forms.ComboBox comboBox价差模式;
        private System.Windows.Forms.NumericUpDown numericUpDown价差数值;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox股数模式;
        private System.Windows.Forms.NumericUpDown numericUpDown股数数值;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox方向;
        private System.Windows.Forms.Label label3;
    }
}