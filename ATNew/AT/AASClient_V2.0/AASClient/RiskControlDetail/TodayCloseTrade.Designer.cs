namespace AASClient.RiskControlDetail
{
    partial class TodayCloseTrade
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView当日平仓 = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bindingSource当日统计 = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView当日平仓)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource当日统计)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.dataGridView当日平仓);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1083, 688);
            this.panel1.TabIndex = 0;
            // 
            // dataGridView当日平仓
            // 
            this.dataGridView当日平仓.AllowUserToAddRows = false;
            this.dataGridView当日平仓.AllowUserToDeleteRows = false;
            this.dataGridView当日平仓.AllowUserToResizeRows = false;
            this.dataGridView当日平仓.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView当日平仓.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView当日平仓.Location = new System.Drawing.Point(0, 57);
            this.dataGridView当日平仓.Name = "dataGridView当日平仓";
            this.dataGridView当日平仓.RowTemplate.Height = 23;
            this.dataGridView当日平仓.Size = new System.Drawing.Size(1081, 629);
            this.dataGridView当日平仓.TabIndex = 1;
            this.dataGridView当日平仓.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView当日平仓_CellFormatting);
            this.dataGridView当日平仓.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView当日平仓_DataError);
            this.dataGridView当日平仓.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView当日平仓_RowPrePaint);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1081, 57);
            this.panel2.TabIndex = 0;
            // 
            // TodayCloseTrade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1083, 688);
            this.Controls.Add(this.panel1);
            this.Name = "TodayCloseTrade";
            this.Text = "TodayCloseTrade";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView当日平仓)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource当日统计)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView当日平仓;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.BindingSource bindingSource当日统计;
    }
}