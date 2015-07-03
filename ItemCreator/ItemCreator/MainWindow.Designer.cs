namespace ItemCreator
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_MenuCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.m_MenuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.m_MenuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.m_MenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_MenuCreateItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_MenuEditItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.m_BtnCreate = new System.Windows.Forms.ToolStripButton();
            this.m_BtnOpen = new System.Windows.Forms.ToolStripButton();
            this.m_BtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.m_BtnCreateItem = new System.Windows.Forms.ToolStripButton();
            this.m_BtnEditItem = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.m_MainPanel = new System.Windows.Forms.Panel();
            this.m_SaveFileDlg = new System.Windows.Forms.SaveFileDialog();
            this.m_OpenFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.m_MenuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(847, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_MenuCreate,
            this.toolStripSeparator3,
            this.m_MenuOpen,
            this.toolStripSeparator1,
            this.m_MenuSave,
            this.m_MenuSaveAs,
            this.toolStripSeparator2,
            this.m_MenuExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // m_MenuCreate
            // 
            this.m_MenuCreate.Name = "m_MenuCreate";
            this.m_MenuCreate.Size = new System.Drawing.Size(152, 22);
            this.m_MenuCreate.Text = "Create";
            this.m_MenuCreate.Click += new System.EventHandler(this.m_MenuCreate_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
            // 
            // m_MenuOpen
            // 
            this.m_MenuOpen.Name = "m_MenuOpen";
            this.m_MenuOpen.Size = new System.Drawing.Size(152, 22);
            this.m_MenuOpen.Text = "Open";
            this.m_MenuOpen.Click += new System.EventHandler(this.m_MenuOpen_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // m_MenuSave
            // 
            this.m_MenuSave.Name = "m_MenuSave";
            this.m_MenuSave.Size = new System.Drawing.Size(152, 22);
            this.m_MenuSave.Text = "Save";
            this.m_MenuSave.Click += new System.EventHandler(this.m_MenuSave_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // m_MenuExit
            // 
            this.m_MenuExit.Name = "m_MenuExit";
            this.m_MenuExit.Size = new System.Drawing.Size(152, 22);
            this.m_MenuExit.Text = "Exit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_MenuCreateItem,
            this.m_MenuEditItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // m_MenuCreateItem
            // 
            this.m_MenuCreateItem.Name = "m_MenuCreateItem";
            this.m_MenuCreateItem.Size = new System.Drawing.Size(152, 22);
            this.m_MenuCreateItem.Text = "Create Item";
            this.m_MenuCreateItem.Click += new System.EventHandler(this.m_MenuCreateItem_Click);
            // 
            // m_MenuEditItem
            // 
            this.m_MenuEditItem.Name = "m_MenuEditItem";
            this.m_MenuEditItem.Size = new System.Drawing.Size(152, 22);
            this.m_MenuEditItem.Text = "Edit Item";
            this.m_MenuEditItem.Click += new System.EventHandler(this.m_MenuEditItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_BtnCreate,
            this.m_BtnOpen,
            this.m_BtnSave,
            this.toolStripSeparator4,
            this.m_BtnCreateItem,
            this.m_BtnEditItem});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(847, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // m_BtnCreate
            // 
            this.m_BtnCreate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_BtnCreate.Image = ((System.Drawing.Image)(resources.GetObject("m_BtnCreate.Image")));
            this.m_BtnCreate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_BtnCreate.Name = "m_BtnCreate";
            this.m_BtnCreate.Size = new System.Drawing.Size(23, 22);
            this.m_BtnCreate.Text = "Create";
            this.m_BtnCreate.Click += new System.EventHandler(this.m_BtnCreate_Click);
            // 
            // m_BtnOpen
            // 
            this.m_BtnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_BtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("m_BtnOpen.Image")));
            this.m_BtnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_BtnOpen.Name = "m_BtnOpen";
            this.m_BtnOpen.Size = new System.Drawing.Size(23, 22);
            this.m_BtnOpen.Text = "m_BtnOpen";
            this.m_BtnOpen.Click += new System.EventHandler(this.m_BtnOpen_Click);
            // 
            // m_BtnSave
            // 
            this.m_BtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_BtnSave.Image = ((System.Drawing.Image)(resources.GetObject("m_BtnSave.Image")));
            this.m_BtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_BtnSave.Name = "m_BtnSave";
            this.m_BtnSave.Size = new System.Drawing.Size(23, 22);
            this.m_BtnSave.Text = "m_BtnSave";
            this.m_BtnSave.Click += new System.EventHandler(this.m_BtnSave_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // m_BtnCreateItem
            // 
            this.m_BtnCreateItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_BtnCreateItem.Image = ((System.Drawing.Image)(resources.GetObject("m_BtnCreateItem.Image")));
            this.m_BtnCreateItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_BtnCreateItem.Name = "m_BtnCreateItem";
            this.m_BtnCreateItem.Size = new System.Drawing.Size(23, 22);
            this.m_BtnCreateItem.Text = "m_BtnCreateItem";
            this.m_BtnCreateItem.Click += new System.EventHandler(this.m_BtnCreateItem_Click);
            // 
            // m_BtnEditItem
            // 
            this.m_BtnEditItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_BtnEditItem.Image = ((System.Drawing.Image)(resources.GetObject("m_BtnEditItem.Image")));
            this.m_BtnEditItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_BtnEditItem.Name = "m_BtnEditItem";
            this.m_BtnEditItem.Size = new System.Drawing.Size(23, 22);
            this.m_BtnEditItem.Text = "m_BtnEditItem";
            this.m_BtnEditItem.Click += new System.EventHandler(this.m_BtnEditItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 417);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(847, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // m_MainPanel
            // 
            this.m_MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_MainPanel.Location = new System.Drawing.Point(0, 49);
            this.m_MainPanel.Name = "m_MainPanel";
            this.m_MainPanel.Size = new System.Drawing.Size(847, 368);
            this.m_MainPanel.TabIndex = 3;
            // 
            // m_SaveFileDlg
            // 
            this.m_SaveFileDlg.DefaultExt = "idb";
            this.m_SaveFileDlg.Filter = "Item Database|*.idb";
            // 
            // m_OpenFileDlg
            // 
            this.m_OpenFileDlg.DefaultExt = "idb";
            this.m_OpenFileDlg.Filter = "Item Database|*.idb";
            // 
            // m_MenuSaveAs
            // 
            this.m_MenuSaveAs.Name = "m_MenuSaveAs";
            this.m_MenuSaveAs.Size = new System.Drawing.Size(152, 22);
            this.m_MenuSaveAs.Text = "Save As...";
            this.m_MenuSaveAs.Click += new System.EventHandler(this.m_MenuSaveAs_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 439);
            this.Controls.Add(this.m_MainPanel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_MenuCreate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem m_MenuOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem m_MenuSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem m_MenuExit;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_MenuCreateItem;
        private System.Windows.Forms.ToolStripMenuItem m_MenuEditItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton m_BtnCreate;
        private System.Windows.Forms.ToolStripButton m_BtnOpen;
        private System.Windows.Forms.ToolStripButton m_BtnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton m_BtnCreateItem;
        private System.Windows.Forms.ToolStripButton m_BtnEditItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel m_MainPanel;
        private System.Windows.Forms.SaveFileDialog m_SaveFileDlg;
        private System.Windows.Forms.OpenFileDialog m_OpenFileDlg;
        private System.Windows.Forms.ToolStripMenuItem m_MenuSaveAs;
    }
}

