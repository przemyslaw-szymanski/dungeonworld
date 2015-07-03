using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ItemCreator
{
    public partial class MainWindow : Form
    {
        CItemViewer m_ItemViewer;

        public MainWindow()
        {
            InitializeComponent();
        }

        void CreateItemViewer()
        {
            m_ItemViewer = new CItemViewer();
            m_ItemViewer.Parent = this.m_MainPanel;
            m_ItemViewer.Dock = DockStyle.Fill;
        }

        private void m_MenuCreate_Click(object sender, EventArgs e)
        {
            if (m_SaveFileDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    GameplayItemManager.Singleton.SaveToBinary(m_SaveFileDlg.FileName);
                    CreateItemViewer();
                }
                catch(Exception E)
                {
                    MessageBox.Show(E.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void m_BtnCreate_Click(object sender, EventArgs e)
        {
            m_MenuCreate_Click(sender, e);
        }

        private void m_MenuOpen_Click(object sender, EventArgs e)
        {
            if (m_OpenFileDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string err = GameplayItemManager.Singleton.LoadFromBinary(m_OpenFileDlg.FileName);
                if(err != string.Empty)
                {
                    MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                CreateItemViewer();
            }
        }

        private void m_BtnOpen_Click(object sender, EventArgs e)
        {
            m_MenuOpen_Click(sender, e);
        }

        private void m_MenuSave_Click(object sender, EventArgs e)
        {
            //If file is opened
            if (m_OpenFileDlg.FileName != string.Empty)
            {
                GameplayItemManager.Singleton.SaveToBinary(m_OpenFileDlg.FileName);
            }
            else
            {
                m_MenuSaveAs_Click(sender, e);
            }
        }

        private void m_BtnSave_Click(object sender, EventArgs e)
        {
            m_MenuSave_Click(sender, e);
        }

        private void m_MenuCreateItem_Click(object sender, EventArgs e)
        {

        }

        private void m_BtnCreateItem_Click(object sender, EventArgs e)
        {

        }

        private void m_MenuEditItem_Click(object sender, EventArgs e)
        {

        }

        private void m_BtnEditItem_Click(object sender, EventArgs e)
        {

        }

        private void m_MenuSaveAs_Click(object sender, EventArgs e)
        {
            if (m_SaveFileDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                bool result = GameplayItemManager.Singleton.SaveToBinary(m_SaveFileDlg.FileName);
                if (!result)
                {
                    MessageBox.Show("Unable to save item database file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
