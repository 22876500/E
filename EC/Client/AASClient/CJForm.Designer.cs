namespace AASClient
{
    partial class CJForm
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
            this.dataGridView成交 = new System.Windows.Forms.DataGridView();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView成交)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridView成交);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(665, 423);
            this.panel1.TabIndex = 2;
            // 
            // dataGridView成交
            // 
            this.dataGridView成交.AllowUserToAddRows = false;
            this.dataGridView成交.AllowUserToDeleteRows = false;
            this.dataGridView成交.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView成交.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView成交.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView成交.Location = new System.Drawing.Point(0, 0);
            this.dataGridView成交.Name = "dataGridView成交";
            this.dataGridView成交.ReadOnly = true;
            this.dataGridView成交.RowHeadersVisible = false;
            this.dataGridView成交.RowTemplate.Height = 23;
            this.dataGridView成交.Size = new System.Drawing.Size(665, 423);
            this.dataGridView成交.TabIndex = 0;
            this.dataGridView成交.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView成交_CellFormatting);
            this.dataGridView成交.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView成交_DataError);
            this.dataGridView成交.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView成交_RowPrePaint);
            // 
            // CJForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 423);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Name = "CJForm";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
            this.Text = "成交";
            this.Load += new System.EventHandler(this.CJForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView成交)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView成交;
        private System.Windows.Forms.BindingSource bindingSource1;
    }
}