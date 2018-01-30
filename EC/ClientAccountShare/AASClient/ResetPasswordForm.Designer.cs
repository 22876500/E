namespace AASClient
{
    partial class ResetPasswordForm
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
            this.button修改密码 = new System.Windows.Forms.Button();
            this.textBox确认密码 = new System.Windows.Forms.TextBox();
            this.textBox密码 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox用户名 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button修改密码
            // 
            this.button修改密码.Location = new System.Drawing.Point(132, 114);
            this.button修改密码.Name = "button修改密码";
            this.button修改密码.Size = new System.Drawing.Size(75, 23);
            this.button修改密码.TabIndex = 11;
            this.button修改密码.Text = "修改密码";
            this.button修改密码.UseVisualStyleBackColor = true;
            this.button修改密码.Click += new System.EventHandler(this.button修改密码_Click);
            // 
            // textBox确认密码
            // 
            this.textBox确认密码.Location = new System.Drawing.Point(132, 75);
            this.textBox确认密码.Name = "textBox确认密码";
            this.textBox确认密码.PasswordChar = '*';
            this.textBox确认密码.Size = new System.Drawing.Size(100, 21);
            this.textBox确认密码.TabIndex = 10;
            // 
            // textBox密码
            // 
            this.textBox密码.Location = new System.Drawing.Point(132, 48);
            this.textBox密码.Name = "textBox密码";
            this.textBox密码.PasswordChar = '*';
            this.textBox密码.Size = new System.Drawing.Size(100, 21);
            this.textBox密码.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "确认密码";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "新密码";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(67, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "用户名";
            // 
            // textBox用户名
            // 
            this.textBox用户名.Location = new System.Drawing.Point(132, 21);
            this.textBox用户名.Name = "textBox用户名";
            this.textBox用户名.ReadOnly = true;
            this.textBox用户名.Size = new System.Drawing.Size(100, 21);
            this.textBox用户名.TabIndex = 13;
            // 
            // ResetPasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 171);
            this.Controls.Add(this.textBox用户名);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button修改密码);
            this.Controls.Add(this.textBox确认密码);
            this.Controls.Add(this.textBox密码);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ResetPasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "重置用户密码";
            this.Load += new System.EventHandler(this.ResetPasswordForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button修改密码;
        private System.Windows.Forms.TextBox textBox确认密码;
        private System.Windows.Forms.TextBox textBox密码;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox用户名;
    }
}