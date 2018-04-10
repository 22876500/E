namespace AASClient.RiskControlDetail
{
    partial class OrderLogDetail
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
            this.dataGridView委托列表 = new System.Windows.Forms.DataGridView();
            this.bindingSource委托记录 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView委托列表)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource委托记录)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1041, 82);
            this.panel1.TabIndex = 0;
            // 
            // dataGridView委托列表
            // 
            this.dataGridView委托列表.AllowUserToAddRows = false;
            this.dataGridView委托列表.AllowUserToDeleteRows = false;
            this.dataGridView委托列表.AllowUserToResizeRows = false;
            this.dataGridView委托列表.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView委托列表.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView委托列表.Location = new System.Drawing.Point(0, 82);
            this.dataGridView委托列表.Name = "dataGridView委托列表";
            this.dataGridView委托列表.RowTemplate.Height = 23;
            this.dataGridView委托列表.Size = new System.Drawing.Size(1041, 574);
            this.dataGridView委托列表.TabIndex = 1;
            this.dataGridView委托列表.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView委托记录_DataError);
            // 
            // OrderLogDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 656);
            this.Controls.Add(this.dataGridView委托列表);
            this.Controls.Add(this.panel1);
            this.Name = "OrderLogDetail";
            this.Text = "委托记录";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView委托列表)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource委托记录)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView委托列表;
        private System.Windows.Forms.BindingSource bindingSource委托记录;
    }
}