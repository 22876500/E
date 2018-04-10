namespace AASClient
{
    partial class SendOrderForm
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
            this.components = new System.ComponentModel.Container();
            this.comboBox交易方向 = new System.Windows.Forms.ComboBox();
            this.numericUpDown数量 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown价格 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox交易员 = new System.Windows.Forms.TextBox();
            this.textBox证券代码 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label证券名称 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown数量)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown价格)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox交易方向
            // 
            this.comboBox交易方向.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox交易方向.FormattingEnabled = true;
            this.comboBox交易方向.Items.AddRange(new object[] {
            "买入",
            "卖出"});
            this.comboBox交易方向.Location = new System.Drawing.Point(191, 115);
            this.comboBox交易方向.Name = "comboBox交易方向";
            this.comboBox交易方向.Size = new System.Drawing.Size(111, 20);
            this.comboBox交易方向.TabIndex = 13;
            // 
            // numericUpDown数量
            // 
            this.numericUpDown数量.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown数量.Location = new System.Drawing.Point(97, 143);
            this.numericUpDown数量.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDown数量.Name = "numericUpDown数量";
            this.numericUpDown数量.Size = new System.Drawing.Size(88, 21);
            this.numericUpDown数量.TabIndex = 12;
            // 
            // numericUpDown价格
            // 
            this.numericUpDown价格.DecimalPlaces = 2;
            this.numericUpDown价格.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown价格.Location = new System.Drawing.Point(97, 116);
            this.numericUpDown价格.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown价格.Name = "numericUpDown价格";
            this.numericUpDown价格.Size = new System.Drawing.Size(88, 21);
            this.numericUpDown价格.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "股数";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "价格";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(191, 139);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 24);
            this.button1.TabIndex = 8;
            this.button1.Text = "下单";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox交易员
            // 
            this.textBox交易员.Location = new System.Drawing.Point(97, 62);
            this.textBox交易员.Name = "textBox交易员";
            this.textBox交易员.ReadOnly = true;
            this.textBox交易员.Size = new System.Drawing.Size(88, 21);
            this.textBox交易员.TabIndex = 17;
            // 
            // textBox证券代码
            // 
            this.textBox证券代码.Location = new System.Drawing.Point(97, 89);
            this.textBox证券代码.Name = "textBox证券代码";
            this.textBox证券代码.ReadOnly = true;
            this.textBox证券代码.Size = new System.Drawing.Size(88, 21);
            this.textBox证券代码.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 19;
            this.label5.Text = "证券代码";
            // 
            // label证券名称
            // 
            this.label证券名称.AutoSize = true;
            this.label证券名称.Location = new System.Drawing.Point(201, 92);
            this.label证券名称.Name = "label证券名称";
            this.label证券名称.Size = new System.Drawing.Size(53, 12);
            this.label证券名称.TabIndex = 20;
            this.label证券名称.Text = "证券名称";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 22;
            this.label1.Text = "交易员";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // SendOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 225);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label证券名称);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox证券代码);
            this.Controls.Add(this.textBox交易员);
            this.Controls.Add(this.comboBox交易方向);
            this.Controls.Add(this.numericUpDown数量);
            this.Controls.Add(this.numericUpDown价格);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SendOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "下单";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SendOrderForm_FormClosing);
            this.Load += new System.EventHandler(this.SendOrderForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown数量)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown价格)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox交易方向;
        private System.Windows.Forms.NumericUpDown numericUpDown数量;
        private System.Windows.Forms.NumericUpDown numericUpDown价格;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox交易员;
        private System.Windows.Forms.TextBox textBox证券代码;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label证券名称;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
    }
}