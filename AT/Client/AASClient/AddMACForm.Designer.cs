namespace AASClient
{
    partial class AddMACForm
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
            this.textBox用户名 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxMAC = new System.Windows.Forms.TextBox();
            this.button添加 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名";
            // 
            // textBox用户名
            // 
            this.textBox用户名.Location = new System.Drawing.Point(108, 34);
            this.textBox用户名.Name = "textBox用户名";
            this.textBox用户名.ReadOnly = true;
            this.textBox用户名.Size = new System.Drawing.Size(100, 21);
            this.textBox用户名.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "MAC地址";
            // 
            // textBoxMAC
            // 
            this.textBoxMAC.Location = new System.Drawing.Point(108, 72);
            this.textBoxMAC.MaxLength = 17;
            this.textBoxMAC.Name = "textBoxMAC";
            this.textBoxMAC.Size = new System.Drawing.Size(100, 21);
            this.textBoxMAC.TabIndex = 3;
            this.textBoxMAC.Leave += new System.EventHandler(this.textBoxMAC_Leave);
            // 
            // button添加
            // 
            this.button添加.Location = new System.Drawing.Point(108, 117);
            this.button添加.Name = "button添加";
            this.button添加.Size = new System.Drawing.Size(75, 23);
            this.button添加.TabIndex = 4;
            this.button添加.Text = "添加";
            this.button添加.UseVisualStyleBackColor = true;
            this.button添加.Click += new System.EventHandler(this.button添加_Click);
            // 
            // AddMACForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 173);
            this.Controls.Add(this.button添加);
            this.Controls.Add(this.textBoxMAC);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox用户名);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AddMACForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加MAC地址";
            this.Activated += new System.EventHandler(this.AddMACForm_Activated);
            this.Load += new System.EventHandler(this.AddMACForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox用户名;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxMAC;
        private System.Windows.Forms.Button button添加;
    }
}