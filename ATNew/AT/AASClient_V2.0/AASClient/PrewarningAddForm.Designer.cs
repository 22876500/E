namespace AASClient
{
    partial class PrewarningAddForm
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
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.comboBoxCodesType = new System.Windows.Forms.ComboBox();
            this.rtbCodes = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownFrequency = new System.Windows.Forms.NumericUpDown();
            this.comboBoxCalType = new System.Windows.Forms.ComboBox();
            this.comboBoxLevel = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.numericUpDownParam = new System.Windows.Forms.NumericUpDown();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBoxCompare = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panelTop = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDownBig = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.panelCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownParam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBig)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelCenter);
            this.panelMain.Controls.Add(this.panelTop);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(582, 396);
            this.panelMain.TabIndex = 0;
            // 
            // panelCenter
            // 
            this.panelCenter.Controls.Add(this.label8);
            this.panelCenter.Controls.Add(this.numericUpDownBig);
            this.panelCenter.Controls.Add(this.label7);
            this.panelCenter.Controls.Add(this.comboBoxCodesType);
            this.panelCenter.Controls.Add(this.rtbCodes);
            this.panelCenter.Controls.Add(this.label6);
            this.panelCenter.Controls.Add(this.label5);
            this.panelCenter.Controls.Add(this.label2);
            this.panelCenter.Controls.Add(this.numericUpDownFrequency);
            this.panelCenter.Controls.Add(this.comboBoxCalType);
            this.panelCenter.Controls.Add(this.comboBoxLevel);
            this.panelCenter.Controls.Add(this.label4);
            this.panelCenter.Controls.Add(this.label3);
            this.panelCenter.Controls.Add(this.btnSubmit);
            this.panelCenter.Controls.Add(this.numericUpDownParam);
            this.panelCenter.Controls.Add(this.comboBox3);
            this.panelCenter.Controls.Add(this.comboBoxCompare);
            this.panelCenter.Controls.Add(this.comboBox2);
            this.panelCenter.Controls.Add(this.label1);
            this.panelCenter.Controls.Add(this.comboBox1);
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.Location = new System.Drawing.Point(0, 27);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(582, 369);
            this.panelCenter.TabIndex = 1;
            // 
            // comboBoxCodesType
            // 
            this.comboBoxCodesType.FormattingEnabled = true;
            this.comboBoxCodesType.Location = new System.Drawing.Point(114, 152);
            this.comboBoxCodesType.Name = "comboBoxCodesType";
            this.comboBoxCodesType.Size = new System.Drawing.Size(80, 20);
            this.comboBoxCodesType.TabIndex = 17;
            this.comboBoxCodesType.SelectedIndexChanged += new System.EventHandler(this.comboBoxCodesType_SelectedIndexChanged);
            // 
            // rtbCodes
            // 
            this.rtbCodes.Location = new System.Drawing.Point(114, 191);
            this.rtbCodes.Name = "rtbCodes";
            this.rtbCodes.Size = new System.Drawing.Size(363, 112);
            this.rtbCodes.TabIndex = 16;
            this.rtbCodes.Text = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(48, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "预警列表:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(197, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "秒";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "预警频率:";
            // 
            // numericUpDownFrequency
            // 
            this.numericUpDownFrequency.Location = new System.Drawing.Point(114, 78);
            this.numericUpDownFrequency.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.numericUpDownFrequency.Name = "numericUpDownFrequency";
            this.numericUpDownFrequency.Size = new System.Drawing.Size(80, 21);
            this.numericUpDownFrequency.TabIndex = 12;
            this.numericUpDownFrequency.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // comboBoxCalType
            // 
            this.comboBoxCalType.FormattingEnabled = true;
            this.comboBoxCalType.Location = new System.Drawing.Point(432, 12);
            this.comboBoxCalType.Name = "comboBoxCalType";
            this.comboBoxCalType.Size = new System.Drawing.Size(36, 20);
            this.comboBoxCalType.TabIndex = 11;
            // 
            // comboBoxLevel
            // 
            this.comboBoxLevel.FormattingEnabled = true;
            this.comboBoxLevel.Location = new System.Drawing.Point(114, 46);
            this.comboBoxLevel.Name = "comboBoxLevel";
            this.comboBoxLevel.Size = new System.Drawing.Size(80, 20);
            this.comboBoxLevel.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "预警级别:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(49, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "预警公式:";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(252, 331);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 7;
            this.btnSubmit.Text = "提交";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // numericUpDownParam
            // 
            this.numericUpDownParam.DecimalPlaces = 3;
            this.numericUpDownParam.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownParam.Location = new System.Drawing.Point(476, 11);
            this.numericUpDownParam.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownParam.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numericUpDownParam.Name = "numericUpDownParam";
            this.numericUpDownParam.Size = new System.Drawing.Size(60, 21);
            this.numericUpDownParam.TabIndex = 6;
            this.numericUpDownParam.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(360, 12);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(64, 20);
            this.comboBox3.TabIndex = 4;
            // 
            // comboBoxCompare
            // 
            this.comboBoxCompare.FormattingEnabled = true;
            this.comboBoxCompare.Location = new System.Drawing.Point(308, 12);
            this.comboBoxCompare.Name = "comboBoxCompare";
            this.comboBoxCompare.Size = new System.Drawing.Size(38, 20);
            this.comboBoxCompare.TabIndex = 3;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(212, 12);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(80, 20);
            this.comboBox2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(197, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "-";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(114, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(80, 20);
            this.comboBox1.TabIndex = 0;
            // 
            // panelTop
            // 
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(582, 27);
            this.panelTop.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(48, 118);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 18;
            this.label7.Text = "大单设置:";
            // 
            // numericUpDownBig
            // 
            this.numericUpDownBig.Location = new System.Drawing.Point(114, 114);
            this.numericUpDownBig.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.numericUpDownBig.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownBig.Name = "numericUpDownBig";
            this.numericUpDownBig.Size = new System.Drawing.Size(80, 21);
            this.numericUpDownBig.TabIndex = 19;
            this.numericUpDownBig.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(197, 119);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 12);
            this.label8.TabIndex = 20;
            this.label8.Text = "股";
            // 
            // PrewarningAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 396);
            this.Controls.Add(this.panelMain);
            this.Name = "PrewarningAddForm";
            this.Load += new System.EventHandler(this.PrewarningAddForm_Load);
            this.panelMain.ResumeLayout(false);
            this.panelCenter.ResumeLayout(false);
            this.panelCenter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownParam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBig)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.NumericUpDown numericUpDownParam;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBoxCompare;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.ComboBox comboBoxLevel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxCalType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownFrequency;
        private System.Windows.Forms.RichTextBox rtbCodes;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxCodesType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDownBig;
        private System.Windows.Forms.Label label7;
    }
}