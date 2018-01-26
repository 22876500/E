namespace AASClient
{
    partial class TradeLimitForm
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
            this.dataGridView额度分配 = new System.Windows.Forms.DataGridView();
            this.dataGridView平台用户 = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.bindingSource平台用户 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource额度分配 = new System.Windows.Forms.BindingSource(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView额度分配)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView平台用户)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource平台用户)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource额度分配)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView额度分配
            // 
            this.dataGridView额度分配.AllowUserToAddRows = false;
            this.dataGridView额度分配.AllowUserToDeleteRows = false;
            this.dataGridView额度分配.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView额度分配.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView额度分配.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView额度分配.Location = new System.Drawing.Point(0, 0);
            this.dataGridView额度分配.Name = "dataGridView额度分配";
            this.dataGridView额度分配.ReadOnly = true;
            this.dataGridView额度分配.RowHeadersVisible = false;
            this.dataGridView额度分配.RowTemplate.Height = 23;
            this.dataGridView额度分配.Size = new System.Drawing.Size(250, 212);
            this.dataGridView额度分配.TabIndex = 0;
            // 
            // dataGridView平台用户
            // 
            this.dataGridView平台用户.AllowUserToAddRows = false;
            this.dataGridView平台用户.AllowUserToDeleteRows = false;
            this.dataGridView平台用户.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView平台用户.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView平台用户.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridView平台用户.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView平台用户.Location = new System.Drawing.Point(0, 0);
            this.dataGridView平台用户.Name = "dataGridView平台用户";
            this.dataGridView平台用户.ReadOnly = true;
            this.dataGridView平台用户.RowHeadersVisible = false;
            this.dataGridView平台用户.RowTemplate.Height = 23;
            this.dataGridView平台用户.Size = new System.Drawing.Size(250, 66);
            this.dataGridView平台用户.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView平台用户);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView额度分配);
            this.splitContainer1.Size = new System.Drawing.Size(250, 282);
            this.splitContainer1.SplitterDistance = 66;
            this.splitContainer1.TabIndex = 2;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "仓位限制";
            this.dataGridViewTextBoxColumn1.HeaderText = "仓位限制";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 78;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "亏损限制";
            this.dataGridViewTextBoxColumn2.HeaderText = "亏损限制";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 78;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "仓位限制";
            this.Column1.HeaderText = "仓位限制";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 78;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "亏损限制";
            this.Column2.HeaderText = "亏损限制";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 78;
            // 
            // TradeLimitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 282);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.Name = "TradeLimitForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "交易额度";
            this.Load += new System.EventHandler(this.TradeLimitForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView额度分配)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView平台用户)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource平台用户)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource额度分配)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView额度分配;
        private System.Windows.Forms.DataGridView dataGridView平台用户;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.BindingSource bindingSource平台用户;
        private System.Windows.Forms.BindingSource bindingSource额度分配;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}