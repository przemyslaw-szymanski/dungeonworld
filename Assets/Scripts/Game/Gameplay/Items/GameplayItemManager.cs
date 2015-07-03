using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class GameplayItemManager : TSingleton<GameplayItemManager>
{
	public List<GameplayItem> SerializableItemList = new List<GameplayItem>();
	protected Dictionary<string, GameplayItem> m_ItemMap = new Dictionary<string, GameplayItem>();
	//protected Dictionary<string, GameplayItem> m_UserItemMap = new Dictionary<string, GameplayItem>();
	protected List<GameplayItem> m_GeneratedItems = new List<GameplayItem>();
	
	protected List<GameplayItem> m_InstantiatedItems = new List<GameplayItem>();
	
	protected int m_currItemId = 1;
	protected uint m_dbChecksum = 0;
	
	public const string XML_FILE_NAME = "item_xml_db";
	public const string BIN_FILE_NAME = "item_db";
	public const string BIN_EXT = ".idb";

    public bool SaveToBinary(string path)
    {
        System.IO.FileStream Stream = new System.IO.FileStream(path, System.IO.FileMode.Create);
        System.IO.BinaryWriter Writer = new System.IO.BinaryWriter(Stream);

        ItemSerializer Serializer = new ItemSerializer();
        string err = Serializer.Serialize(Writer, m_ItemMap.Values);

        Writer.Close();

        return err == string.Empty;
    }

	public bool SaveToBinary()
	{
		string path = AssetPaths.Resources + BIN_FILE_NAME + BIN_EXT;
        return SaveToBinary(path);
	}
	
	/*
	public string LoadFromBinary(string path)
	{
		return LoadFromMemory(System.IO.File.ReadAllBytes(path));
	}	
	*/
	public string LoadFromMemory(byte[] Data)
	{
		if(Data == null || Data.Length == 0)
		{
			return "File data is empty";
		}
		
        System.IO.MemoryStream Stream;
		System.IO.BinaryReader Reader;
        Stream = new System.IO.MemoryStream(Data);
        Reader = new System.IO.BinaryReader(Stream);

        ItemSerializer Serializer = new ItemSerializer();
        string err = Serializer.Deserialize(Reader, ref m_ItemMap);

		Reader.Close();
		return "";
	}
	
	public string LoadFromFile(string path)
	{
		try
		{
			if(!System.IO.File.Exists(path))
			{
				new System.IO.FileStream(path, System.IO.FileMode.Create);
			}
			
			byte[] Data = System.IO.File.ReadAllBytes(path);
			return LoadFromMemory(Data);
		}
		catch(System.Exception Ex)
		{
			return Ex.Message;
		}
		
		return string.Empty;
	}
	
	public string LoadDatabase()
	{
		if(Application.isEditor)
		{
			string path = AssetPaths.Resources + BIN_FILE_NAME + BIN_EXT;
			Debug.Log("Editor mode: Loading item db from file: " + path);
			return LoadFromFile(path);
		}
		
		UnityEngine.TextAsset Asset = Resources.Load(BIN_FILE_NAME) as TextAsset;
		if(Asset == null)
		{
			string err = string.Format("Asset: {0} in Resources is null", BIN_FILE_NAME);
			Debug.LogError(err);
			return err;
		}
		return LoadFromMemory(Asset.bytes);
	}
	
	void AddItemToDatabase(GameplayItem Item)
	{
		if(Item.Prefab == null)
		{
			Item.Prefab = ResourceManager.Singleton.LoadPrefab(Item.PrefabPath);
		}
		
		m_ItemMap.Add(Item.Name, Item);
	}
	
	public Dictionary<string, GameplayItem> DatabaseItems
	{
		get { return m_ItemMap; }
	}

    public Dictionary<string, GameplayItem> CloneDatabase()
    {
        return new Dictionary<string, GameplayItem>(m_ItemMap);
    }
	
	public GameplayItem GetItem(string name)
	{
		GameplayItem Item;
		Item = GetDatabaseItem(name);
		if(Item != null)
			return Item;
		
		Item = GetGeneratedItem(name);
		if(Item != null)
			return Item;
		
		return null;
	}
	
	public GameplayItem GetDatabaseItem(string name)
	{
		GameplayItem Item;
		m_ItemMap.TryGetValue(name, out Item);
		return Item;
	}
	
	public void LogDatabase()
	{
		Debug.Log(string.Format("Item count: {0}", m_ItemMap.Count));
		foreach(var Pair in m_ItemMap)
		{
			Debug.Log(string.Format("Item: {0}\n{1}", Pair.Key, Pair.Value));
		}
	}
	
	public GameplayItem GetGeneratedItem(string name)
	{
		foreach(GameplayItem Item in m_GeneratedItems)
		{
			if(Item.Name == name)
				return Item;
		}
		
		return null;
	}
	
	public int GenerateID()
	{
		return m_currItemId++;
	}
	
	public GameplayItem Instantiate(string name)
	{
		GameplayItem Original = GetItem(name);
        return Instantiate(Original);
	}
	
	public GameplayItem Instantiate(GameplayItem Original)
	{
        if (Original == null)
            return null;
        if (Original.Instantiated)
            return Original;

        GameplayItem Item = Original.Instantiate();
        int id = GenerateID();
        Item.SetID(id);

        m_InstantiatedItems.Add(Item);
        return Item;
	}
	
	public GameplayItem CreateEmpty()
	{
		GameplayItem Item = new GameplayItem();
		int id = GenerateID();
		Item.SetID(id);
		m_InstantiatedItems.Add(Item);
		return Item;
	}
	
	public bool RemoveInstantiatedItem(GameplayItem Item)
	{
		int itemPos = -1;
		for(int i = m_InstantiatedItems.Count; i-->0;)
		{
			if(m_InstantiatedItems[i] == Item)
			{
				itemPos = i;
				break;
			}
		}
		
		if(itemPos < 0)
			return false;
		
		m_InstantiatedItems.RemoveAt(itemPos);
		return true;
	}
	
	public bool AddOrUpdateDatabaseItem(GameplayItem Other)
	{
		GameplayItem Item;
		
		if(!m_ItemMap.ContainsKey(Other.Name))
		{
			Item = new GameplayItem();
			int id = GenerateID();
			Item.SetID(id);
			m_ItemMap.Add(Other.Name, Item);
		}
		else
		{
			Item = GetDatabaseItem(Other.Name);
		}
		
		Item.CopyFrom(Other);
		
		return SaveToBinary();
	}

	public GameplayItem SearchItem(string name)
	{
		if(m_ItemMap.Count == 0)
		{
			Debug.Log("ItemDB is empty. Trying to load database");
			LoadDatabase();
		}
		
		foreach(KeyValuePair<string, GameplayItem> Pair in m_ItemMap)
		{
			if(Pair.Key.Contains(name))
				return Pair.Value;
		}
		
		return null;
	}
   
}
