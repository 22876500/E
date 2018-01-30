namespace AASClient
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox密码 = new System.Windows.Forms.TextBox();
            this.button登录 = new System.Windows.Forms.Button();
            this.textBox服务器 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox记住密码 = new System.Windows.Forms.CheckBox();
            this.comboBox用户名 = new System.Windows.Forms.ComboBox();
            this.comboBoxServer = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(78, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "密码";
            // 
            // textBox密码
            // 
            this.textBox密码.Location = new System.Drawing.Point(138, 137);
            this.textBox密码.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.textBox密码.Name = "textBox密码";
            this.textBox密码.PasswordChar = '*';
            this.textBox密码.Size = new System.Drawing.Size(132, 26);
            this.textBox密码.TabIndex = 2;
            this.textBox密码.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyEnterDown);
            // 
            // button登录
            // 
            this.button登录.Location = new System.Drawing.Point(138, 220);
            this.button登录.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.button登录.Name = "button登录";
            this.button登录.Size = new System.Drawing.Size(99, 39);
            this.button登录.TabIndex = 4;
            this.button登录.Text = "登录";
            this.button登录.UseVisualStyleBackColor = true;
            this.button登录.Click += new System.EventHandler(this.button登录_Click);
            // 
            // textBox服务器
            // 
            this.textBox服务器.Location = new System.Drawing.Point(138, 65);
            this.textBox服务器.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.textBox服务器.Name = "textBox服务器";
            this.textBox服务器.Size = new System.Drawing.Size(132, 26);
            this.textBox服务器.TabIndex = 0;
            this.textBox服务器.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyEnterDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "服务器";
            // 
            // checkBox记住密码
            // 
            this.checkBox记住密码.AutoSize = true;
            this.checkBox记住密码.Checked = true;
            this.checkBox记住密码.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox记住密码.Location = new System.Drawing.Point(138, 182);
            this.checkBox记住密码.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.checkBox记住密码.Name = "checkBox记住密码";
            this.checkBox记住密码.Size = new System.Drawing.Size(84, 24);
            this.checkBox记住密码.TabIndex = 3;
            this.checkBox记住密码.Text = "记住密码";
            this.checkBox记住密码.UseVisualStyleBackColor = true;
            this.checkBox记住密码.CheckedChanged += new System.EventHandler(this.checkBox记住密码_CheckedChanged);
            // 
            // comboBox用户名
            // 
            this.comboBox用户名.FormattingEnabled = true;
            this.comboBox用户名.Location = new System.Drawing.Point(138, 101);
            this.comboBox用户名.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.comboBox用户名.Name = "comboBox用户名";
            this.comboBox用户名.Size = new System.Drawing.Size(132, 28);
            this.comboBox用户名.TabIndex = 1;
            this.comboBox用户名.SelectedIndexChanged += new System.EventHandler(this.comboBox用户名_SelectedIndexChanged);
            // 
            // comboBoxServer
            // 
            this.comboBoxServer.FormattingEnabled = true;
            this.comboBoxServer.Location = new System.Drawing.Point(138, 29);
            this.comboBoxServer.Name = "comboBoxServer";
            this.comboBoxServer.Size = new System.Drawing.Size(132, 28);
            this.comboBoxServer.TabIndex = 7;
            this.comboBoxServer.SelectedIndexChanged += new System.EventHandler(this.comboBoxServer_SelectedIndexChanged);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 304);
            this.Controls.Add(this.comboBoxServer);
            this.Controls.Add(this.comboBox用户名);
            this.Controls.Add(this.checkBox记住密码);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox服务器);
            this.Controls.Add(this.button登录);
            this.Controls.Add(this.textBox密码);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.Activated += new System.EventHandler(this.LoginForm_Activated);
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox密码;
        private System.Windows.Forms.Button button登录;
        private System.Windows.Forms.TextBox textBox服务器;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox记住密码;
        private System.Windows.Forms.ComboBox comboBox用户名;
        private System.Windows.Forms.ComboBox comboBoxServer;
    }
}