namespace AASClient
{
    partial class AddRemindForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown提示价格 = new System.Windows.Forms.NumericUpDown();
            this.comboBox提示等级 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox启用 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.comboBox提示类型 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox证券代码 = new System.Windows.Forms.ComboBox();
            this.label证券名称 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown提示价格)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "当证券";
            // 
            // numericUpDown提示价格
            // 
            this.numericUpDown提示价格.DecimalPlaces = 3;
            this.numericUpDown提示价格.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown提示价格.Location = new System.Drawing.Point(329, 38);
            this.numericUpDown提示价格.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown提示价格.Name = "numericUpDown提示价格";
            this.numericUpDown提示价格.Size = new System.Drawing.Size(55, 21);
            this.numericUpDown提示价格.TabIndex = 2;
            // 
            // comboBox提示等级
            // 
            this.comboBox提示等级.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox提示等级.FormattingEnabled = true;
            this.comboBox提示等级.Location = new System.Drawing.Point(466, 39);
            this.comboBox提示等级.Name = "comboBox提示等级";
            this.comboBox提示等级.Size = new System.Drawing.Size(45, 20);
            this.comboBox提示等级.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(431, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "发出";
            // 
            // checkBox启用
            // 
            this.checkBox启用.AutoSize = true;
            this.checkBox启用.Checked = true;
            this.checkBox启用.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox启用.Location = new System.Drawing.Point(210, 102);
            this.checkBox启用.Name = "checkBox启用";
            this.checkBox启用.Size = new System.Drawing.Size(48, 16);
            this.checkBox启用.TabIndex = 4;
            this.checkBox启用.Text = "启用";
            this.checkBox启用.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(264, 98);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "添加此提示";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // comboBox提示类型
            // 
            this.comboBox提示类型.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox提示类型.FormattingEnabled = true;
            this.comboBox提示类型.Location = new System.Drawing.Point(264, 39);
            this.comboBox提示类型.Name = "comboBox提示类型";
            this.comboBox提示类型.Size = new System.Drawing.Size(59, 20);
            this.comboBox提示类型.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(390, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "元时,";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(517, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "色提示";
            // 
            // comboBox证券代码
            // 
            this.comboBox证券代码.FormattingEnabled = true;
            this.comboBox证券代码.Location = new System.Drawing.Point(111, 39);
            this.comboBox证券代码.Name = "comboBox证券代码";
            this.comboBox证券代码.Size = new System.Drawing.Size(147, 20);
            this.comboBox证券代码.TabIndex = 10;
            this.comboBox证券代码.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox证券代码_KeyDown);
            this.comboBox证券代码.Leave += new System.EventHandler(this.comboBox证券代码_Leave);
            // 
            // label证券名称
            // 
            this.label证券名称.AutoSize = true;
            this.label证券名称.Location = new System.Drawing.Point(109, 21);
            this.label证券名称.Name = "label证券名称";
            this.label证券名称.Size = new System.Drawing.Size(0, 12);
            this.label证券名称.TabIndex = 11;
            // 
            // AddRemindForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 148);
            this.Controls.Add(this.label证券名称);
            this.Controls.Add(this.comboBox证券代码);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBox提示类型);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox启用);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox提示等级);
            this.Controls.Add(this.numericUpDown提示价格);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AddRemindForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加价格提示";
            this.Activated += new System.EventHandler(this.AddRemindForm_Activated);
            this.Load += new System.EventHandler(this.AddRemindForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown提示价格)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown提示价格;
        private System.Windows.Forms.ComboBox comboBox提示等级;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox启用;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox comboBox提示类型;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox comboBox证券代码;
        private System.Windows.Forms.Label label证券名称;
    }
}