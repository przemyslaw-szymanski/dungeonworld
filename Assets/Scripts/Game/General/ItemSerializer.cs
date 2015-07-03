using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public class ItemSerializer 
{
    int m_currItem = 0;
    int m_itemCount = 0;
    int m_dbChecksum = 0;
	string m_err = "";

    public ItemSerializer()
    {

    }


    public string Serialize(BinaryWriter Writer, Dictionary<string, GameplayItem>.ValueCollection Values)
    {
        string err = WriteHeader(Writer, Values.Count);
        if (err != string.Empty)
            return err;
        return WriteItems(Writer, Values);
        return string.Empty;
    }

    public string Deserialize(BinaryReader Reader, ref Dictionary<string, GameplayItem> Map)
    {
        string err = ReadHeader(Reader);
        if(err != string.Empty)
            return err;

        return ReadItems(Reader, ref Map);
    }

    string ReadHeader(BinaryReader Reader)
    {
        //Read header name
        string headerName = Reader.ReadString();
        if (headerName != "idb")
        {
            return "Wrong header name";
        }

        //Read version
        short ver = Reader.ReadInt16();
        if (ver != 100)
        {
            return "Wrong version";
        }

        uint checksum = Reader.ReadUInt32();

        m_itemCount = Reader.ReadInt32();

        return string.Empty;
    }

    string WriteHeader(BinaryWriter Writer, int itemCount)
    {
        Writer.Write("idb"); //header name string
        Writer.Write((short)100); //version short
        Writer.Write(m_dbChecksum);
        Writer.Write((int)itemCount); //item count int
        return string.Empty;
    }

    string WriteItems(BinaryWriter Writer, Dictionary<string, GameplayItem>.ValueCollection Values)
    {
        try
        {
            foreach (GameplayItem Item in Values)
            {
                //Base
                Writer.Write((int)Item.Type); //type int
                Writer.Write((int)Item.SubType); //sub type int
                Writer.Write((int)Item.Slot); //int
                Writer.Write(Item.Dragable); //dragable bool
                Writer.Write(Item.IconPath); //icon name string
                Writer.Write(Item.Inventory); //inventory bool
                Writer.Write(Item.PrefabPath); //prefab name string
                Writer.Write(Item.Usable); //bool
                Writer.Write(Item.ScriptPath); //string
                //Attributes
                Writer.Write(Item.Name); //name string
                Writer.Write(Item.Weight); //weight float

                //Components
                Writer.Write((short)Item.ComponentCount); //number of components
                foreach (GameplayItemComponent Comp in Item.Components)
                {
                    WriteComponent(Writer, Comp);
                }
            }
        }
        catch (System.Exception Ex)
        {
            return Ex.Message;
        }
        return string.Empty;
    }

    string WriteComponent(BinaryWriter Writer, GameplayItemComponent Comp)
    {
        System.Type CmpType = Comp.GetType();

        Writer.Write(CmpType.Name); //string
		
		m_err = Comp.Serialize(Writer);

        return m_err;
    }

    GameplayItemComponent ReadComponent(BinaryReader Reader)
    {
        string typeName = Reader.ReadString();
        GameplayItemComponent Cmp = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(typeName) as GameplayItemComponent;
		
		m_err = Cmp.Deserialize(Reader);
		if(m_err != string.Empty)
		{
			return null;
		}
        return Cmp;
    }

    void WriteField(BinaryWriter Writer, object Obj, FieldInfo Info)
    {
        //Writer.Write(Info.Name); //string
        object value = Info.GetValue(Obj);

        if (Info.FieldType == typeof(System.String))
        {
            Writer.Write((string)value); //string
        }
        else if (Info.FieldType == typeof(System.Int16))
        {
            Writer.Write((short)value); //short
        }
        else if (Info.FieldType == typeof(System.Single))
        {
            Writer.Write((float)value); //float
        }
        else if (Info.FieldType == typeof(System.Int32))
        {
            Writer.Write((int)value);
        }
		else if(Info.FieldType == typeof(System.Enum) || Info.FieldType.BaseType == typeof(System.Enum))
		{
			Writer.Write((int)value);
		}
		
    }

    object ReadField(BinaryReader Reader, object Obj, FieldInfo Info)
    {
        //string name = Reader.ReadString();
        object value = null;

        if (Info.FieldType == typeof(System.String))
        {
            value = Reader.ReadString();
        }
        else if (Info.FieldType == typeof(System.Int16))
        {
            value = Reader.ReadInt16();
        }
        else if (Info.FieldType == typeof(System.Single))
        {
            value = Reader.ReadSingle();
        }
        else if (Info.FieldType == typeof(System.Int32))
        {
            value = Reader.ReadInt32();
        }
		else if(Info.FieldType == typeof(System.Enum) || Info.FieldType.BaseType == typeof(System.Enum))
		{
			value = Reader.ReadInt32();
		}

        return value;
    }

    string ReadItems(BinaryReader Reader, ref Dictionary<string, GameplayItem> Map)
    {
        try
        {
            for (int i = 0; i < m_itemCount; ++i)
            {
                GameplayItem Item = new GameplayItem();
                Item.Type = (Gameplay.ItemType)Reader.ReadInt32();
                Item.SubType = (Gameplay.ItemSubType)Reader.ReadInt32();
                Item.Slot = (Gameplay.EquipmentSlotType)Reader.ReadInt32();
                Item.Dragable = Reader.ReadBoolean();
                Item.IconPath = Reader.ReadString();
                Item.Inventory = Reader.ReadBoolean();
                Item.PrefabPath = Reader.ReadString();
                Item.Usable = Reader.ReadBoolean();
                Item.ScriptPath = Reader.ReadString();

                //Attributes
                Item.Name = Reader.ReadString();
                Item.Weight = Reader.ReadSingle();

                //Components
				short componentCount = Reader.ReadInt16();
				for(short c = 0; c < componentCount; ++c)
				{
					GameplayItemComponent Cmp = ReadComponent(Reader);
					if(Cmp != null)
					{
						Item.AddComponent(Cmp);
					}
				}

                Debug.Log("Read item from db: " + Item);
                Map.Add(Item.Name, Item);
            }
        }
        catch (System.Exception Ex)
        {
			Debug.LogError(Ex.Message);
            return Ex.Message;
        }
        return string.Empty;
    }
	
}
