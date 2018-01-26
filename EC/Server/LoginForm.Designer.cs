namespace Server
{
    partial class LoginForm
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
            this.textBox版本号 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox交易服务器 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox交易密码 = new System.Windows.Forms.TextBox();
            this.button登录 = new System.Windows.Forms.Button();
            this.checkBox记住密码 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox版本号
            // 
            this.textBox版本号.Location = new System.Drawing.Point(230, 34);
            this.textBox版本号.Name = "textBox版本号";
            this.textBox版本号.Size = new System.Drawing.Size(58, 21);
            this.textBox版本号.TabIndex = 1;
            this.textBox版本号.Text = "6.00";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(228, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 29;
            this.label7.Text = "版本号";
            // 
            // comboBox交易服务器
            // 
            this.comboBox交易服务器.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox交易服务器.FormattingEnabled = true;
            this.comboBox交易服务器.Location = new System.Drawing.Point(45, 35);
            this.comboBox交易服务器.Name = "comboBox交易服务器";
            this.comboBox交易服务器.Size = new System.Drawing.Size(171, 20);
            this.comboBox交易服务器.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "密码";
            // 
            // textBox交易密码
            // 
            this.textBox交易密码.Location = new System.Drawing.Point(45, 86);
            this.textBox交易密码.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox交易密码.Name = "textBox交易密码";
            this.textBox交易密码.PasswordChar = '*';
            this.textBox交易密码.Size = new System.Drawing.Size(171, 21);
            this.textBox交易密码.TabIndex = 4;
            // 
            // button登录
            // 
            this.button登录.Location = new System.Drawing.Point(168, 168);
            this.button登录.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button登录.Name = "button登录";
            this.button登录.Size = new System.Drawing.Size(87, 33);
            this.button登录.TabIndex = 6;
            this.button登录.Text = "登录";
            this.button登录.UseVisualStyleBackColor = true;
            this.button登录.Click += new System.EventHandler(this.button登录_Click);
            // 
            // checkBox记住密码
            // 
            this.checkBox记住密码.AutoSize = true;
            this.checkBox记住密码.Checked = true;
            this.checkBox记住密码.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox记住密码.Location = new System.Drawing.Point(90, 177);
            this.checkBox记住密码.Name = "checkBox记住密码";
            this.checkBox记住密码.Size = new System.Drawing.Size(72, 16);
            this.checkBox记住密码.TabIndex = 30;
            this.checkBox记住密码.Text = "记住密码";
            this.checkBox记住密码.UseVisualStyleBackColor = true;
            this.checkBox记住密码.CheckedChanged += new System.EventHandler(this.checkBox记住密码_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox交易密码);
            this.groupBox1.Controls.Add(this.textBox版本号);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.comboBox交易服务器);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(339, 135);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "凭据";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "交易服务器";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 226);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBox记住密码);
            this.Controls.Add(this.button登录);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.Activated += new System.EventHandler(this.LoginForm_Activated);
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox版本号;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox交易服务器;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox交易密码;
        private System.Windows.Forms.Button button登录;
        private System.Windows.Forms.CheckBox checkBox记住密码;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
    }
}