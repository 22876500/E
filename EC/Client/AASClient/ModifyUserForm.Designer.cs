namespace AASClient
{
    partial class ModifyUserForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button确定 = new System.Windows.Forms.Button();
            this.textBox用户名 = new System.Windows.Forms.TextBox();
            this.comboBox角色 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown手续费率 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown亏损限制 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown仓位限制 = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox允许删除碎股订单 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox分组 = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown手续费率)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown亏损限制)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown仓位限制)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(59, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "角色";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 16;
            this.label1.Text = "用户名";
            // 
            // button确定
            // 
            this.button确定.Location = new System.Drawing.Point(106, 148);
            this.button确定.Name = "button确定";
            this.button确定.Size = new System.Drawing.Size(75, 23);
            this.button确定.TabIndex = 15;
            this.button确定.Text = "确定";
            this.button确定.UseVisualStyleBackColor = true;
            this.button确定.Click += new System.EventHandler(this.button确定_Click);
            // 
            // textBox用户名
            // 
            this.textBox用户名.Location = new System.Drawing.Point(97, 39);
            this.textBox用户名.Name = "textBox用户名";
            this.textBox用户名.ReadOnly = true;
            this.textBox用户名.Size = new System.Drawing.Size(100, 21);
            this.textBox用户名.TabIndex = 12;
            // 
            // comboBox角色
            // 
            this.comboBox角色.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox角色.Enabled = false;
            this.comboBox角色.FormattingEnabled = true;
            this.comboBox角色.Items.AddRange(new object[] {
            "管理员",
            "风控员",
            "交易员"});
            this.comboBox角色.Location = new System.Drawing.Point(97, 73);
            this.comboBox角色.Name = "comboBox角色";
            this.comboBox角色.Size = new System.Drawing.Size(100, 20);
            this.comboBox角色.TabIndex = 11;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.numericUpDown手续费率);
            this.groupBox1.Controls.Add(this.numericUpDown亏损限制);
            this.groupBox1.Controls.Add(this.numericUpDown仓位限制);
            this.groupBox1.Location = new System.Drawing.Point(239, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(222, 119);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "交易员设置";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(188, 52);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 18;
            this.label10.Text = "元";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(188, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 12);
            this.label9.TabIndex = 17;
            this.label9.Text = "元";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 79);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 16;
            this.label8.Text = "手续费率";
            this.label8.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "亏损限制";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "仓位限制";
            // 
            // numericUpDown手续费率
            // 
            this.numericUpDown手续费率.DecimalPlaces = 5;
            this.numericUpDown手续费率.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numericUpDown手续费率.Location = new System.Drawing.Point(81, 77);
            this.numericUpDown手续费率.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown手续费率.Name = "numericUpDown手续费率";
            this.numericUpDown手续费率.Size = new System.Drawing.Size(100, 21);
            this.numericUpDown手续费率.TabIndex = 13;
            this.numericUpDown手续费率.Visible = false;
            // 
            // numericUpDown亏损限制
            // 
            this.numericUpDown亏损限制.Increment = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown亏损限制.Location = new System.Drawing.Point(81, 50);
            this.numericUpDown亏损限制.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.numericUpDown亏损限制.Name = "numericUpDown亏损限制";
            this.numericUpDown亏损限制.Size = new System.Drawing.Size(100, 21);
            this.numericUpDown亏损限制.TabIndex = 12;
            // 
            // numericUpDown仓位限制
            // 
            this.numericUpDown仓位限制.Increment = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown仓位限制.Location = new System.Drawing.Point(81, 23);
            this.numericUpDown仓位限制.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.numericUpDown仓位限制.Name = "numericUpDown仓位限制";
            this.numericUpDown仓位限制.Size = new System.Drawing.Size(100, 21);
            this.numericUpDown仓位限制.TabIndex = 11;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox允许删除碎股订单);
            this.groupBox2.Location = new System.Drawing.Point(239, 154);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(222, 43);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "普通风控员设置";
            // 
            // checkBox允许删除碎股订单
            // 
            this.checkBox允许删除碎股订单.AutoSize = true;
            this.checkBox允许删除碎股订单.Location = new System.Drawing.Point(19, 21);
            this.checkBox允许删除碎股订单.Name = "checkBox允许删除碎股订单";
            this.checkBox允许删除碎股订单.Size = new System.Drawing.Size(120, 16);
            this.checkBox允许删除碎股订单.TabIndex = 0;
            this.checkBox允许删除碎股订单.Text = "允许删除碎股订单";
            this.checkBox允许删除碎股订单.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 25;
            this.label2.Text = "分组";
            // 
            // comboBox分组
            // 
            this.comboBox分组.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox分组.FormattingEnabled = true;
            this.comboBox分组.Items.AddRange(new object[] {
            "管理员",
            "风控员",
            "交易员"});
            this.comboBox分组.Location = new System.Drawing.Point(98, 107);
            this.comboBox分组.Name = "comboBox分组";
            this.comboBox分组.Size = new System.Drawing.Size(100, 20);
            this.comboBox分组.TabIndex = 24;
            // 
            // ModifyUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 253);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox分组);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button确定);
            this.Controls.Add(this.textBox用户名);
            this.Controls.Add(this.comboBox角色);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ModifyUserForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "修改用户";
            this.Load += new System.EventHandler(this.ModifyUserForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown手续费率)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown亏损限制)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown仓位限制)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button确定;
        private System.Windows.Forms.TextBox textBox用户名;
        private System.Windows.Forms.ComboBox comboBox角色;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown手续费率;
        private System.Windows.Forms.NumericUpDown numericUpDown亏损限制;
        private System.Windows.Forms.NumericUpDown numericUpDown仓位限制;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox允许删除碎股订单;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox分组;
    }
}