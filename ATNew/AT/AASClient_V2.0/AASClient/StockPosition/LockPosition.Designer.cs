namespace AASClient.StockPosition
{
    partial class LockPosition
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
            this.textBox组合号 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox证券代码 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDown请求数量 = new System.Windows.Forms.NumericUpDown();
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.textBox总仓位 = new System.Windows.Forms.TextBox();
            this.textBox已用数量 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox证券名称 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox缩写 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxSelfLockQty = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown请求数量)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(123, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "组合号";
            // 
            // textBox组合号
            // 
            this.textBox组合号.Location = new System.Drawing.Point(168, 32);
            this.textBox组合号.Name = "textBox组合号";
            this.textBox组合号.ReadOnly = true;
            this.textBox组合号.Size = new System.Drawing.Size(120, 21);
            this.textBox组合号.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(110, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "证券代码";
            // 
            // textBox证券代码
            // 
            this.textBox证券代码.Location = new System.Drawing.Point(168, 64);
            this.textBox证券代码.Name = "textBox证券代码";
            this.textBox证券代码.ReadOnly = true;
            this.textBox证券代码.Size = new System.Drawing.Size(120, 21);
            this.textBox证券代码.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(123, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "总仓位";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(111, 163);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "锁定总数";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(111, 223);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "申请数量";
            // 
            // numericUpDown请求数量
            // 
            this.numericUpDown请求数量.Location = new System.Drawing.Point(168, 219);
            this.numericUpDown请求数量.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericUpDown请求数量.Name = "numericUpDown请求数量";
            this.numericUpDown请求数量.Size = new System.Drawing.Size(120, 21);
            this.numericUpDown请求数量.TabIndex = 1;
            this.numericUpDown请求数量.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown请求数量_KeyDown);
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Location = new System.Drawing.Point(168, 301);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(75, 23);
            this.buttonSubmit.TabIndex = 3;
            this.buttonSubmit.Text = "确定";
            this.buttonSubmit.UseVisualStyleBackColor = true;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
            // 
            // textBox总仓位
            // 
            this.textBox总仓位.Location = new System.Drawing.Point(168, 128);
            this.textBox总仓位.Name = "textBox总仓位";
            this.textBox总仓位.ReadOnly = true;
            this.textBox总仓位.Size = new System.Drawing.Size(120, 21);
            this.textBox总仓位.TabIndex = 0;
            // 
            // textBox已用数量
            // 
            this.textBox已用数量.Location = new System.Drawing.Point(168, 160);
            this.textBox已用数量.Name = "textBox已用数量";
            this.textBox已用数量.ReadOnly = true;
            this.textBox已用数量.Size = new System.Drawing.Size(120, 21);
            this.textBox已用数量.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(110, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "证券名称";
            // 
            // textBox证券名称
            // 
            this.textBox证券名称.Location = new System.Drawing.Point(168, 96);
            this.textBox证券名称.Name = "textBox证券名称";
            this.textBox证券名称.ReadOnly = true;
            this.textBox证券名称.Size = new System.Drawing.Size(120, 21);
            this.textBox证券名称.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(111, 256);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "拼音缩写";
            // 
            // textBox缩写
            // 
            this.textBox缩写.Location = new System.Drawing.Point(168, 253);
            this.textBox缩写.Name = "textBox缩写";
            this.textBox缩写.Size = new System.Drawing.Size(120, 21);
            this.textBox缩写.TabIndex = 2;
            this.textBox缩写.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox缩写_KeyDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(75, 193);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 12);
            this.label8.TabIndex = 17;
            this.label8.Text = "本人已锁定额度";
            // 
            // textBoxSelfLockQty
            // 
            this.textBoxSelfLockQty.Location = new System.Drawing.Point(168, 189);
            this.textBoxSelfLockQty.Name = "textBoxSelfLockQty";
            this.textBoxSelfLockQty.ReadOnly = true;
            this.textBoxSelfLockQty.Size = new System.Drawing.Size(120, 21);
            this.textBoxSelfLockQty.TabIndex = 0;
            // 
            // LockPosition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 350);
            this.Controls.Add(this.textBoxSelfLockQty);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox缩写);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox证券名称);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox已用数量);
            this.Controls.Add(this.textBox总仓位);
            this.Controls.Add(this.buttonSubmit);
            this.Controls.Add(this.numericUpDown请求数量);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox证券代码);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox组合号);
            this.Controls.Add(this.label1);
            this.Name = "LockPosition";
            this.Text = "仓位锁定";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown请求数量)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox组合号;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox证券代码;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDown请求数量;
        private System.Windows.Forms.Button buttonSubmit;
        private System.Windows.Forms.TextBox textBox总仓位;
        private System.Windows.Forms.TextBox textBox已用数量;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox证券名称;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox缩写;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxSelfLockQty;
    }
}