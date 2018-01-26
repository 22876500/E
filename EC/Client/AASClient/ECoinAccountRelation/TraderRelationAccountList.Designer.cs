namespace AASClient.ECoinAccountRelation
{
    partial class TraderRelationAccountList
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
            this.dataGridView交易员关联帐户 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip交易员帐户关联 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新增ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bindingSource交易员关联帐户 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView交易员关联帐户)).BeginInit();
            this.contextMenuStrip交易员帐户关联.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource交易员关联帐户)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView交易员关联帐户
            // 
            this.dataGridView交易员关联帐户.AllowUserToAddRows = false;
            this.dataGridView交易员关联帐户.AllowUserToDeleteRows = false;
            this.dataGridView交易员关联帐户.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView交易员关联帐户.ContextMenuStrip = this.contextMenuStrip交易员帐户关联;
            this.dataGridView交易员关联帐户.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView交易员关联帐户.Location = new System.Drawing.Point(0, 0);
            this.dataGridView交易员关联帐户.Name = "dataGridView交易员关联帐户";
            this.dataGridView交易员关联帐户.ReadOnly = true;
            this.dataGridView交易员关联帐户.RowTemplate.Height = 23;
            this.dataGridView交易员关联帐户.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView交易员关联帐户.Size = new System.Drawing.Size(552, 408);
            this.dataGridView交易员关联帐户.TabIndex = 0;
            // 
            // contextMenuStrip交易员帐户关联
            // 
            this.contextMenuStrip交易员帐户关联.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem,
            this.新增ToolStripMenuItem});
            this.contextMenuStrip交易员帐户关联.Name = "contextMenuStrip交易员帐户关联";
            this.contextMenuStrip交易员帐户关联.Size = new System.Drawing.Size(101, 48);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // 新增ToolStripMenuItem
            // 
            this.新增ToolStripMenuItem.Name = "新增ToolStripMenuItem";
            this.新增ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.新增ToolStripMenuItem.Text = "新增";
            this.新增ToolStripMenuItem.Click += new System.EventHandler(this.新增ToolStripMenuItem_Click);
            // 
            // TraderRelationAccountList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 408);
            this.Controls.Add(this.dataGridView交易员关联帐户);
            this.Name = "TraderRelationAccountList";
            this.Text = "TraderRelationAccountForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView交易员关联帐户)).EndInit();
            this.contextMenuStrip交易员帐户关联.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource交易员关联帐户)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView交易员关联帐户;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip交易员帐户关联;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新增ToolStripMenuItem;
        private System.Windows.Forms.BindingSource bindingSource交易员关联帐户;
    }
}