namespace AASClient.ECoinAccount
{
    partial class formECoinAccountList
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
            this.dataGridView电子币帐户 = new System.Windows.Forms.DataGridView();
            this.contextMenuStripDGV电子币帐户列表 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bindingSource电子币帐户 = new System.Windows.Forms.BindingSource(this.components);
            this.新增ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView电子币帐户)).BeginInit();
            this.contextMenuStripDGV电子币帐户列表.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource电子币帐户)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView电子币帐户
            // 
            this.dataGridView电子币帐户.AllowUserToAddRows = false;
            this.dataGridView电子币帐户.AllowUserToDeleteRows = false;
            this.dataGridView电子币帐户.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView电子币帐户.ContextMenuStrip = this.contextMenuStripDGV电子币帐户列表;
            this.dataGridView电子币帐户.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView电子币帐户.Location = new System.Drawing.Point(0, 0);
            this.dataGridView电子币帐户.Name = "dataGridView电子币帐户";
            this.dataGridView电子币帐户.ReadOnly = true;
            this.dataGridView电子币帐户.RowTemplate.Height = 23;
            this.dataGridView电子币帐户.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView电子币帐户.Size = new System.Drawing.Size(695, 340);
            this.dataGridView电子币帐户.TabIndex = 0;
            // 
            // contextMenuStripDGV电子币帐户列表
            // 
            this.contextMenuStripDGV电子币帐户列表.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem,
            this.新增ToolStripMenuItem,
            this.编辑ToolStripMenuItem});
            this.contextMenuStripDGV电子币帐户列表.Name = "contextMenuStripDGV电子币帐户列表";
            this.contextMenuStripDGV电子币帐户列表.Size = new System.Drawing.Size(153, 92);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // 新增ToolStripMenuItem
            // 
            this.新增ToolStripMenuItem.Name = "新增ToolStripMenuItem";
            this.新增ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.新增ToolStripMenuItem.Text = "新增";
            this.新增ToolStripMenuItem.Click += new System.EventHandler(this.新增ToolStripMenuItem_Click);
            // 
            // 编辑ToolStripMenuItem
            // 
            this.编辑ToolStripMenuItem.Name = "编辑ToolStripMenuItem";
            this.编辑ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.编辑ToolStripMenuItem.Text = "编辑";
            this.编辑ToolStripMenuItem.Click += new System.EventHandler(this.编辑ToolStripMenuItem_Click);
            // 
            // formECoinAccountList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 340);
            this.Controls.Add(this.dataGridView电子币帐户);
            this.Name = "formECoinAccountList";
            this.Text = "电子币帐户列表";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView电子币帐户)).EndInit();
            this.contextMenuStripDGV电子币帐户列表.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource电子币帐户)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView电子币帐户;
        private System.Windows.Forms.BindingSource bindingSource电子币帐户;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDGV电子币帐户列表;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新增ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 编辑ToolStripMenuItem;
    }
}