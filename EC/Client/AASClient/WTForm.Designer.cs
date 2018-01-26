namespace AASClient
{
    partial class WTForm
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
            this.dataGridView委托 = new System.Windows.Forms.DataGridView();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView委托)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridView委托);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(625, 396);
            this.panel1.TabIndex = 2;
            // 
            // dataGridView委托
            // 
            this.dataGridView委托.AllowUserToAddRows = false;
            this.dataGridView委托.AllowUserToDeleteRows = false;
            this.dataGridView委托.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView委托.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView委托.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView委托.Location = new System.Drawing.Point(0, 0);
            this.dataGridView委托.Name = "dataGridView委托";
            this.dataGridView委托.ReadOnly = true;
            this.dataGridView委托.RowHeadersVisible = false;
            this.dataGridView委托.RowTemplate.Height = 23;
            this.dataGridView委托.Size = new System.Drawing.Size(625, 396);
            this.dataGridView委托.TabIndex = 0;
            this.dataGridView委托.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView委托_CellDoubleClick);
            this.dataGridView委托.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView委托_CellFormatting);
            this.dataGridView委托.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView委托_DataError);
            this.dataGridView委托.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView委托_RowPrePaint);
            // 
            // WTForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 396);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Name = "WTForm";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
            this.Text = "委托";
            this.Load += new System.EventHandler(this.WTForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView委托)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView委托;
        private System.Windows.Forms.BindingSource bindingSource1;
    }
}