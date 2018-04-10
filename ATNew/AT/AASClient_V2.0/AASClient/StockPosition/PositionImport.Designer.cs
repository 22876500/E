namespace AASClient.StockPosition
{
    partial class PositionImport
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridViewImportData = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBox删除旧数据 = new System.Windows.Forms.CheckBox();
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.numericUpDownExcelSheet = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownHeaderRow = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonPositionImport = new System.Windows.Forms.Button();
            this.Column组合号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column证券代码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column证券名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column总仓位 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewImportData)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExcelSheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeaderRow)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.dataGridViewImportData);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(811, 335);
            this.panel1.TabIndex = 0;
            // 
            // dataGridViewImportData
            // 
            this.dataGridViewImportData.AllowUserToAddRows = false;
            this.dataGridViewImportData.AllowUserToDeleteRows = false;
            this.dataGridViewImportData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewImportData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column组合号,
            this.Column证券代码,
            this.Column证券名称,
            this.Column总仓位});
            this.dataGridViewImportData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewImportData.Location = new System.Drawing.Point(0, 67);
            this.dataGridViewImportData.Name = "dataGridViewImportData";
            this.dataGridViewImportData.RowTemplate.Height = 23;
            this.dataGridViewImportData.Size = new System.Drawing.Size(809, 266);
            this.dataGridViewImportData.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkBox删除旧数据);
            this.panel2.Controls.Add(this.buttonSubmit);
            this.panel2.Controls.Add(this.numericUpDownExcelSheet);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.numericUpDownHeaderRow);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.buttonPositionImport);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(809, 67);
            this.panel2.TabIndex = 0;
            // 
            // checkBox删除旧数据
            // 
            this.checkBox删除旧数据.AutoSize = true;
            this.checkBox删除旧数据.Location = new System.Drawing.Point(680, 27);
            this.checkBox删除旧数据.Name = "checkBox删除旧数据";
            this.checkBox删除旧数据.Size = new System.Drawing.Size(120, 16);
            this.checkBox删除旧数据.TabIndex = 6;
            this.checkBox删除旧数据.Text = "删除旧仓位并保存";
            this.checkBox删除旧数据.UseVisualStyleBackColor = true;
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Location = new System.Drawing.Point(599, 24);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(75, 23);
            this.buttonSubmit.TabIndex = 5;
            this.buttonSubmit.Text = "保存";
            this.buttonSubmit.UseVisualStyleBackColor = true;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
            // 
            // numericUpDownExcelSheet
            // 
            this.numericUpDownExcelSheet.Location = new System.Drawing.Point(316, 27);
            this.numericUpDownExcelSheet.Name = "numericUpDownExcelSheet";
            this.numericUpDownExcelSheet.Size = new System.Drawing.Size(60, 21);
            this.numericUpDownExcelSheet.TabIndex = 4;
            this.numericUpDownExcelSheet.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(236, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "导入Sheet页：";
            // 
            // numericUpDownHeaderRow
            // 
            this.numericUpDownHeaderRow.Location = new System.Drawing.Point(102, 27);
            this.numericUpDownHeaderRow.Name = "numericUpDownHeaderRow";
            this.numericUpDownHeaderRow.Size = new System.Drawing.Size(60, 21);
            this.numericUpDownHeaderRow.TabIndex = 2;
            this.numericUpDownHeaderRow.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "表头对应行：";
            // 
            // buttonPositionImport
            // 
            this.buttonPositionImport.Location = new System.Drawing.Point(390, 24);
            this.buttonPositionImport.Name = "buttonPositionImport";
            this.buttonPositionImport.Size = new System.Drawing.Size(111, 23);
            this.buttonPositionImport.TabIndex = 0;
            this.buttonPositionImport.Text = "可用仓位导入";
            this.buttonPositionImport.UseVisualStyleBackColor = true;
            this.buttonPositionImport.Click += new System.EventHandler(this.buttonPositionImport_Click);
            // 
            // Column组合号
            // 
            this.Column组合号.DataPropertyName = "组合号";
            this.Column组合号.HeaderText = "组合号";
            this.Column组合号.Name = "Column组合号";
            // 
            // Column证券代码
            // 
            this.Column证券代码.DataPropertyName = "证券代码";
            this.Column证券代码.HeaderText = "证券代码";
            this.Column证券代码.Name = "Column证券代码";
            // 
            // Column证券名称
            // 
            this.Column证券名称.DataPropertyName = "证券名称";
            this.Column证券名称.HeaderText = "证券名称";
            this.Column证券名称.Name = "Column证券名称";
            // 
            // Column总仓位
            // 
            this.Column总仓位.DataPropertyName = "总仓位";
            this.Column总仓位.HeaderText = "总仓位";
            this.Column总仓位.Name = "Column总仓位";
            // 
            // PositionImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 335);
            this.Controls.Add(this.panel1);
            this.Name = "PositionImport";
            this.Text = "可用仓位导入";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewImportData)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExcelSheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeaderRow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridViewImportData;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonPositionImport;
        private System.Windows.Forms.NumericUpDown numericUpDownHeaderRow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownExcelSheet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonSubmit;
        private System.Windows.Forms.CheckBox checkBox删除旧数据;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column组合号;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column证券代码;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column证券名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column总仓位;
    }
}