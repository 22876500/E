namespace AASClient
{
    partial class PrewarningListForm
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
            this.dataGridViewSettings = new System.Windows.Forms.DataGridView();
            this.contextMenuStripAdd = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.新增ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripDelete = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bindingSourceSettings = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSettings)).BeginInit();
            this.contextMenuStripAdd.SuspendLayout();
            this.contextMenuStripDelete.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSettings)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewSettings
            // 
            this.dataGridViewSettings.AllowUserToAddRows = false;
            this.dataGridViewSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSettings.ContextMenuStrip = this.contextMenuStripAdd;
            this.dataGridViewSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSettings.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewSettings.MultiSelect = false;
            this.dataGridViewSettings.Name = "dataGridViewSettings";
            this.dataGridViewSettings.ReadOnly = true;
            this.dataGridViewSettings.RowTemplate.ContextMenuStrip = this.contextMenuStripDelete;
            this.dataGridViewSettings.RowTemplate.Height = 23;
            this.dataGridViewSettings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewSettings.Size = new System.Drawing.Size(507, 209);
            this.dataGridViewSettings.TabIndex = 0;
            this.dataGridViewSettings.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSettings_CellDoubleClick);
            // 
            // contextMenuStripAdd
            // 
            this.contextMenuStripAdd.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新增ToolStripMenuItem});
            this.contextMenuStripAdd.Name = "contextMenuStripAdd";
            this.contextMenuStripAdd.Size = new System.Drawing.Size(101, 26);
            // 
            // 新增ToolStripMenuItem
            // 
            this.新增ToolStripMenuItem.Name = "新增ToolStripMenuItem";
            this.新增ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.新增ToolStripMenuItem.Text = "新增";
            this.新增ToolStripMenuItem.Click += new System.EventHandler(this.新增ToolStripMenuItem_Click);
            // 
            // contextMenuStripDelete
            // 
            this.contextMenuStripDelete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.编辑ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.contextMenuStripDelete.Name = "contextMenuStripDelete";
            this.contextMenuStripDelete.Size = new System.Drawing.Size(101, 48);
            // 
            // 编辑ToolStripMenuItem
            // 
            this.编辑ToolStripMenuItem.Name = "编辑ToolStripMenuItem";
            this.编辑ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.编辑ToolStripMenuItem.Text = "编辑";
            this.编辑ToolStripMenuItem.Click += new System.EventHandler(this.编辑ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // PrewarningListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 209);
            this.Controls.Add(this.dataGridViewSettings);
            this.Name = "PrewarningListForm";
            this.Text = "预警设置";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSettings)).EndInit();
            this.contextMenuStripAdd.ResumeLayout(false);
            this.contextMenuStripDelete.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSettings)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewSettings;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripAdd;
        private System.Windows.Forms.ToolStripMenuItem 新增ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDelete;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.BindingSource bindingSourceSettings;
        private System.Windows.Forms.ToolStripMenuItem 编辑ToolStripMenuItem;
    }
}