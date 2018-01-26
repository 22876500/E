namespace AASClient
{
    partial class ClosedTradeForm
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
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView已平仓订单 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView已平仓订单)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridView已平仓订单);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(781, 441);
            this.panel1.TabIndex = 1;
            // 
            // dataGridView已平仓订单
            // 
            this.dataGridView已平仓订单.AllowUserToAddRows = false;
            this.dataGridView已平仓订单.AllowUserToDeleteRows = false;
            this.dataGridView已平仓订单.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView已平仓订单.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView已平仓订单.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView已平仓订单.Location = new System.Drawing.Point(0, 0);
            this.dataGridView已平仓订单.Name = "dataGridView已平仓订单";
            this.dataGridView已平仓订单.ReadOnly = true;
            this.dataGridView已平仓订单.RowHeadersVisible = false;
            this.dataGridView已平仓订单.RowTemplate.Height = 23;
            this.dataGridView已平仓订单.Size = new System.Drawing.Size(781, 441);
            this.dataGridView已平仓订单.TabIndex = 1;
            this.dataGridView已平仓订单.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView已平仓订单_CellFormatting);
            this.dataGridView已平仓订单.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView已平仓订单_DataError);
            this.dataGridView已平仓订单.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView已平仓订单_RowPrePaint);
            // 
            // ClosedTradeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 441);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Name = "ClosedTradeForm";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
            this.Text = "已平仓交易";
            this.Load += new System.EventHandler(this.ClosedTradeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView已平仓订单)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView已平仓订单;
    }
}