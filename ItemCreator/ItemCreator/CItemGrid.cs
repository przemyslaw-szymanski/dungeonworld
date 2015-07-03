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
    public partial class CItemGrid : UserControl
    {
        DataGridView m_Grid;
        DataTable m_Table;

        public CItemGrid(Gameplay.ItemType Type)
        {
            InitializeComponent();
            CreateGrid(Type);
        }

        void CreateGrid(Gameplay.ItemType Type)
        {
            CreateEmptyGrid();
            switch(Type)
            {
                case Gameplay.ItemType.WEAPON: CreateWeaponGrid(); break;
            }
        }

        void CreateEmptyGrid()
        {
            m_Grid = new DataGridView();
            m_Grid.ReadOnly = true;
            m_Grid.Parent = this;
            m_Grid.Dock = DockStyle.Fill;
            m_Table = new DataTable();
            m_Grid.DataSource = m_Table;

            //Add base item attributes
            Type ItemType = typeof(GameplayItem);
            Type ItemAttrType = typeof(ItemAttributes);

            System.Reflection.FieldInfo[] ItemFields = ItemType.GetFields();
            System.Reflection.FieldInfo[] ItemAttrFields = ItemAttrType.GetFields();

            List<string> FieldNames = new List<string>();
            foreach (System.Reflection.FieldInfo Info in ItemFields)
            {
                FieldNames.Add(Info.Name);
           
                DataGridViewColumn Col = new DataGridViewColumn(CreateCellTemplate(Info));
                Col.Name = Info.Name;
                //m_Grid.Columns.Add(Col);
                m_Table.Columns.Add(Info.Name, Info.FieldType);
            }
            foreach (System.Reflection.FieldInfo Info in ItemAttrFields)
            {
                FieldNames.Add(Info.Name);
                DataGridViewColumn Col = new DataGridViewColumn(CreateCellTemplate(Info));
                Col.Name = Info.Name;
                //m_Grid.Columns.Add(Col);
                m_Table.Columns.Add(Info.Name, Info.FieldType);
            }

           
            
        }

        DataGridViewCell CreateCellTemplate(System.Reflection.FieldInfo Info)
        {
            DataGridViewCell Cell = new DataGridViewTextBoxCell();
            Type FieldType = Info.FieldType;
            if (FieldType == typeof(bool))
            {
                Cell = new DataGridViewCheckBoxCell();
            }
            //else if (FieldType == typeof(Enum))
            //{
            //    Cell = new DataGridViewTextBoxCell();
            //}
            //else if (   FieldType == typeof(int) || FieldType == typeof(float) || FieldType == typeof(uint) || FieldType == typeof(short) ||
            //            FieldType == typeof(ushort) || FieldType == typeof(byte) || FieldType == typeof(float))
            //{
            //    Cell = new Datagridview
            //}

            return Cell;
        }

        void CreateWeaponGrid()
        {
       
        }
    }
}
