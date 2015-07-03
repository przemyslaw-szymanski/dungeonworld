using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ItemCreator
{
    public partial class CItemViewer : UserControl
    {
        TabControl m_TabControl = new TabControl();
        List<TabPage> m_Pages = new List<TabPage>();
        List<CItemGrid> m_Grids = new List<CItemGrid>();

        public CItemViewer()
        {
            InitializeComponent();
            m_TabControl.Parent = this;
            m_TabControl.Dock = DockStyle.Fill;
            SetDatabase();
        }

        public void SetDatabase()
        {
            foreach (Gameplay.ItemType Type in Enum.GetValues(typeof(Gameplay.ItemType)))
            {
                if (Type.ToString().StartsWith("_"))
                    continue;
                TabPage Page = new TabPage(Type.ToString());
                CItemGrid Grid = new CItemGrid(Type);
                Grid.Parent = Page;
                Grid.Dock = DockStyle.Fill;
                m_TabControl.TabPages.Add(Page);
            }
        }
    }
}
