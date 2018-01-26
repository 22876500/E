namespace AASClient
{
    partial class ModifyTradeLimitForm
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
            this.button修改交易额度 = new System.Windows.Forms.Button();
            this.textBox证券名称 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox市场 = new System.Windows.Forms.ComboBox();
            this.comboBox组合号 = new System.Windows.Forms.ComboBox();
            this.textBox拼音缩写 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDown交易额度 = new System.Windows.Forms.NumericUpDown();
            this.textBox证券代码 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox交易员 = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox卖模式 = new System.Windows.Forms.ComboBox();
            this.comboBox买模式 = new System.Windows.Forms.ComboBox();
            this.numericUpDown手续费率 = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown交易额度)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown手续费率)).BeginInit();
            this.SuspendLayout();
            // 
            // button修改交易额度
            // 
            this.button修改交易额度.Location = new System.Drawing.Point(118, 307);
            this.button修改交易额度.Name = "button修改交易额度";
            this.button修改交易额度.Size = new System.Drawing.Size(100, 23);
            this.button修改交易额度.TabIndex = 18;
            this.button修改交易额度.Text = "修改交易额度";
            this.button修改交易额度.UseVisualStyleBackColor = true;
            this.button修改交易额度.Click += new System.EventHandler(this.button修改交易额度_Click);
            // 
            // textBox证券名称
            // 
            this.textBox证券名称.Location = new System.Drawing.Point(121, 142);
            this.textBox证券名称.MaxLength = 10;
            this.textBox证券名称.Name = "textBox证券名称";
            this.textBox证券名称.Size = new System.Drawing.Size(100, 21);
            this.textBox证券名称.TabIndex = 46;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(52, 146);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 56;
            this.label7.Text = "证券名称";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(76, 117);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 55;
            this.label6.Text = "市场";
            // 
            // comboBox市场
            // 
            this.comboBox市场.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox市场.FormattingEnabled = true;
            this.comboBox市场.Items.AddRange(new object[] {
            "深圳",
            "上海"});
            this.comboBox市场.Location = new System.Drawing.Point(121, 114);
            this.comboBox市场.Name = "comboBox市场";
            this.comboBox市场.Size = new System.Drawing.Size(101, 20);
            this.comboBox市场.TabIndex = 44;
            // 
            // comboBox组合号
            // 
            this.comboBox组合号.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox组合号.FormattingEnabled = true;
            this.comboBox组合号.Location = new System.Drawing.Point(121, 88);
            this.comboBox组合号.Name = "comboBox组合号";
            this.comboBox组合号.Size = new System.Drawing.Size(100, 20);
            this.comboBox组合号.TabIndex = 43;
            // 
            // textBox拼音缩写
            // 
            this.textBox拼音缩写.Location = new System.Drawing.Point(121, 169);
            this.textBox拼音缩写.MaxLength = 10;
            this.textBox拼音缩写.Name = "textBox拼音缩写";
            this.textBox拼音缩写.Size = new System.Drawing.Size(100, 21);
            this.textBox拼音缩写.TabIndex = 47;
            this.textBox拼音缩写.Leave += new System.EventHandler(this.textBox拼音缩写_Leave);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(52, 172);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 54;
            this.label8.Text = "拼音缩写";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(228, 253);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 53;
            this.label5.Text = "股";
            // 
            // numericUpDown交易额度
            // 
            this.numericUpDown交易额度.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown交易额度.Location = new System.Drawing.Point(118, 251);
            this.numericUpDown交易额度.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDown交易额度.Name = "numericUpDown交易额度";
            this.numericUpDown交易额度.Size = new System.Drawing.Size(100, 21);
            this.numericUpDown交易额度.TabIndex = 48;
            // 
            // textBox证券代码
            // 
            this.textBox证券代码.Location = new System.Drawing.Point(121, 60);
            this.textBox证券代码.MaxLength = 6;
            this.textBox证券代码.Name = "textBox证券代码";
            this.textBox证券代码.ReadOnly = true;
            this.textBox证券代码.Size = new System.Drawing.Size(100, 21);
            this.textBox证券代码.TabIndex = 45;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(52, 253);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 52;
            this.label4.Text = "交易额度";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(52, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 51;
            this.label3.Text = "证券代码";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 50;
            this.label2.Text = "组合号";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 49;
            this.label1.Text = "交易员";
            // 
            // comboBox交易员
            // 
            this.comboBox交易员.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox交易员.Enabled = false;
            this.comboBox交易员.FormattingEnabled = true;
            this.comboBox交易员.Location = new System.Drawing.Point(122, 35);
            this.comboBox交易员.Name = "comboBox交易员";
            this.comboBox交易员.Size = new System.Drawing.Size(97, 20);
            this.comboBox交易员.TabIndex = 57;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(62, 225);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 61;
            this.label10.Text = "卖模式";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(62, 199);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 60;
            this.label9.Text = "买模式";
            // 
            // comboBox卖模式
            // 
            this.comboBox卖模式.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox卖模式.FormattingEnabled = true;
            this.comboBox卖模式.Location = new System.Drawing.Point(119, 222);
            this.comboBox卖模式.Name = "comboBox卖模式";
            this.comboBox卖模式.Size = new System.Drawing.Size(100, 20);
            this.comboBox卖模式.TabIndex = 59;
            // 
            // comboBox买模式
            // 
            this.comboBox买模式.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox买模式.FormattingEnabled = true;
            this.comboBox买模式.Location = new System.Drawing.Point(119, 196);
            this.comboBox买模式.Name = "comboBox买模式";
            this.comboBox买模式.Size = new System.Drawing.Size(99, 20);
            this.comboBox买模式.TabIndex = 58;
            // 
            // numericUpDown手续费率
            // 
            this.numericUpDown手续费率.DecimalPlaces = 5;
            this.numericUpDown手续费率.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numericUpDown手续费率.Location = new System.Drawing.Point(118, 278);
            this.numericUpDown手续费率.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDown手续费率.Name = "numericUpDown手续费率";
            this.numericUpDown手续费率.Size = new System.Drawing.Size(100, 21);
            this.numericUpDown手续费率.TabIndex = 63;
            this.numericUpDown手续费率.Value = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(52, 280);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 62;
            this.label11.Text = "手续费率";
            // 
            // ModifyTradeLimitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 343);
            this.Controls.Add(this.numericUpDown手续费率);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboBox卖模式);
            this.Controls.Add(this.comboBox买模式);
            this.Controls.Add(this.comboBox交易员);
            this.Controls.Add(this.textBox证券名称);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBox市场);
            this.Controls.Add(this.comboBox组合号);
            this.Controls.Add(this.textBox拼音缩写);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericUpDown交易额度);
            this.Controls.Add(this.textBox证券代码);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button修改交易额度);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ModifyTradeLimitForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "修改交易额度";
            this.Load += new System.EventHandler(this.ModifyTradeLimitForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown交易额度)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown手续费率)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button修改交易额度;
        private System.Windows.Forms.TextBox textBox证券名称;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox市场;
        private System.Windows.Forms.ComboBox comboBox组合号;
        private System.Windows.Forms.TextBox textBox拼音缩写;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDown交易额度;
        private System.Windows.Forms.TextBox textBox证券代码;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox交易员;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox卖模式;
        private System.Windows.Forms.ComboBox comboBox买模式;
        private System.Windows.Forms.NumericUpDown numericUpDown手续费率;
        private System.Windows.Forms.Label label11;
    }
}