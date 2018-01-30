namespace AASClient
{
    partial class SetHqServerForm
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
            this.comboBox行情服务器 = new System.Windows.Forms.ComboBox();
            this.button保存 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox行情服务器
            // 
            this.comboBox行情服务器.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox行情服务器.FormattingEnabled = true;
            this.comboBox行情服务器.Location = new System.Drawing.Point(132, 35);
            this.comboBox行情服务器.Name = "comboBox行情服务器";
            this.comboBox行情服务器.Size = new System.Drawing.Size(268, 20);
            this.comboBox行情服务器.TabIndex = 0;
            // 
            // button保存
            // 
            this.button保存.Location = new System.Drawing.Point(197, 82);
            this.button保存.Name = "button保存";
            this.button保存.Size = new System.Drawing.Size(102, 23);
            this.button保存.TabIndex = 1;
            this.button保存.Text = "保存";
            this.button保存.UseVisualStyleBackColor = true;
            this.button保存.Click += new System.EventHandler(this.button保存_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "当前行情服务器";
            // 
            // SetHqServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 136);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button保存);
            this.Controls.Add(this.comboBox行情服务器);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SetHqServerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设置行情服务器";
            this.Load += new System.EventHandler(this.SetHqServerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox行情服务器;
        private System.Windows.Forms.Button button保存;
        private System.Windows.Forms.Label label1;
    }
}