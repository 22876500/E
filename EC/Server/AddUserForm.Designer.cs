namespace Server
{
    partial class AddUserForm
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
            this.button确定 = new System.Windows.Forms.Button();
            this.textBox用户名 = new System.Windows.Forms.TextBox();
            this.textBox密码 = new System.Windows.Forms.TextBox();
            this.textBox确认密码 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button确定
            // 
            this.button确定.Location = new System.Drawing.Point(143, 140);
            this.button确定.Name = "button确定";
            this.button确定.Size = new System.Drawing.Size(75, 23);
            this.button确定.TabIndex = 0;
            this.button确定.Text = "确定";
            this.button确定.UseVisualStyleBackColor = true;
            this.button确定.Click += new System.EventHandler(this.button确定_Click);
            // 
            // textBox用户名
            // 
            this.textBox用户名.Location = new System.Drawing.Point(143, 47);
            this.textBox用户名.Name = "textBox用户名";
            this.textBox用户名.Size = new System.Drawing.Size(100, 21);
            this.textBox用户名.TabIndex = 1;
            this.textBox用户名.Text = "admin";
            // 
            // textBox密码
            // 
            this.textBox密码.Location = new System.Drawing.Point(143, 74);
            this.textBox密码.Name = "textBox密码";
            this.textBox密码.PasswordChar = '*';
            this.textBox密码.Size = new System.Drawing.Size(100, 21);
            this.textBox密码.TabIndex = 2;
            this.textBox密码.Text = "admin";
            // 
            // textBox确认密码
            // 
            this.textBox确认密码.Location = new System.Drawing.Point(143, 101);
            this.textBox确认密码.Name = "textBox确认密码";
            this.textBox确认密码.PasswordChar = '*';
            this.textBox确认密码.Size = new System.Drawing.Size(100, 21);
            this.textBox确认密码.TabIndex = 3;
            this.textBox确认密码.Text = "admin";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(76, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "用户名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(88, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "密码";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "确认密码";
            // 
            // AddUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 196);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox确认密码);
            this.Controls.Add(this.textBox密码);
            this.Controls.Add(this.textBox用户名);
            this.Controls.Add(this.button确定);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AddUserForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设定初始的超级管理员的用户名和密码";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button确定;
        private System.Windows.Forms.TextBox textBox用户名;
        private System.Windows.Forms.TextBox textBox密码;
        private System.Windows.Forms.TextBox textBox确认密码;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}