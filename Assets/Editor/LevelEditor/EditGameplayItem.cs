using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class EditGameplayItem : ScriptableWizard 
{
	string m_searchText = "";
	
	Gameplay.ItemType Type = Gameplay.ItemType.NONE;
	Gameplay.ItemSubType SubType = Gameplay.ItemSubType.NONE;
	bool Usable = false; //can be used from inventory (potion), from hand (magic wand), from environment (door button), etc
	bool Dragable = false; //can be get from the ground (sword)
	bool Inventory = true; //if this item can be put in the inventory
	
	//public WeaponAttributes WeaponAttributes = new WeaponAttributes();
	//public ArmorAttributes ArmorAttributes = new ArmorAttributes();

    private Gameplay.ItemType m_SelectedType = Gameplay.ItemType.NONE;
    private Gameplay.ItemSubType m_SelectedSubType = Gameplay.ItemSubType.NONE;
    private static List<System.Type> m_ItemComponents = new List<System.Type>();
    private static string[] m_ItemComponentNames;
    private int m_selectedItemComponent = 0;
    private bool m_showItemComponents = false;
    private bool m_showAddedComponents = false;
    //private List<GameplayItemComponent> m_AddedItemComponents = new List<GameplayItemComponent>();

    private Vector2 m_vecScrollView = new Vector2();
    private GameplayItem m_Item = new GameplayItem();

    class ItemComponentGUI
    {
        public bool Edit = false;

        public ItemComponentGUI() { }
    }

    private List<ItemComponentGUI> m_ItemComponentGUIs = new List<ItemComponentGUI>();
	
	[MenuItem("GameObject/Game/Edit Item")]
	static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Edit Item", typeof(EditGameplayItem));

        m_ItemComponents = GetItemComponents();
        m_ItemComponentNames = ItemComponentsToNames();
    }

	void OnWizardUpdate()
    {
        

    }

    void OnGUI()
    {
		EditorGUILayout.BeginHorizontal();
		m_searchText = EditorGUILayout.TextField(m_searchText);
		if(GUILayout.Button("Search"))
		{
			SetItem(SearchItem());
		}
		EditorGUILayout.EndHorizontal();
        m_vecScrollView = EditorGUILayout.BeginScrollView(m_vecScrollView);
        //EditorGUILayout.TextField("Item Name:", "");
        //m_SelectedType = (Gameplay.ItemType)EditorGUILayout.EnumPopup("Item Type:", m_SelectedType);
        //if (m_SelectedType == Gameplay.ItemType._COUNT)
        //    m_SelectedType = Gameplay.ItemType.NONE;
        //m_SelectedSubType = (Gameplay.ItemSubType)EditorGUILayout.EnumPopup("Item SubType:", m_SelectedSubType);
        //if (m_SelectedSubType == Gameplay.ItemSubType._COUNT)
        //    m_SelectedSubType = Gameplay.ItemSubType.NONE;

        //m_SelectedType = Gameplay.CheckItemType(m_SelectedSubType);
        if (m_Item == null)
            m_Item = System.Activator.CreateInstance(typeof(GameplayItem)) as GameplayItem;
        if (m_Item == null)
            Debug.LogError("null");
        object obj = (object)m_Item;
        //Debug.Log(obj.GetType());
        GUIDrawObject(m_Item);

        GUIItemComponents();

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Edit"))
        {
            EditItem();
        }
        if (GUILayout.Button("Create"))
        {
            CreateNewItem();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }
	
	void OnWizardCreate()
	{
		if(!ValidatePrefab())
			return;
		
		GameplayItem Item = CreateNewItem();
	
		if(!GameplayItemManager.Singleton.AddOrUpdateDatabaseItem(Item))
			return;
	}

    public static List<System.Type> GetItemComponents()
    {
        System.Type[] Types = typeof(GameplayItemComponent).Assembly.GetTypes();
        List<System.Type> TypeList = new List<System.Type>();

        foreach (System.Type T in Types)
        {
            if (T == typeof(GameplayItemComponent) || T.IsSubclassOf(typeof(GameplayItemComponent)))
            {             
                TypeList.Add(T);
            }
        }

        return TypeList;
    }

    public static string[] ItemComponentsToNames()
    {
        string[] Names = new string[m_ItemComponents.Count];
        for (int i = 0; i < Names.Length; ++i)
        {
            Names[i] = m_ItemComponents[i].Name;
        }

        return Names;
    }

    void GUIItemComponents()
    {
        m_showItemComponents = EditorGUILayout.Foldout(m_showItemComponents, "Components");
        if (m_showItemComponents)
        {
            m_selectedItemComponent = EditorGUILayout.Popup("Item Components:", m_selectedItemComponent, m_ItemComponentNames);
            if (GUILayout.Button("Add Selected Component"))
            {
                //m_AddedItemComponents.Add(m_ItemComponents[m_selectedItemComponent]);
                System.Type CompType = m_ItemComponents[m_selectedItemComponent];
                GameplayItemComponent Comp = System.Activator.CreateInstance(CompType) as GameplayItemComponent;
                //m_AddedItemComponents.Add(Comp);
                m_Item.AddComponent(Comp);
                m_ItemComponentGUIs.Add(new ItemComponentGUI());
            }

            m_showAddedComponents = EditorGUILayout.Foldout(m_showAddedComponents, "Added Components");
            if (m_showAddedComponents)
            {
                for(int i = 0; i < m_Item.Components.Count; ++i)
                {
                    //if (GUILayout.Button(T.Name))
                    {
                        GUIItemComponent(m_Item.Components[i], i);
                    }
                }
            }
        }
    }

    void GUIItemComponent(GameplayItemComponent Comp, int componentId)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.SelectableLabel(Comp.GetType().Name);
        m_ItemComponentGUIs[componentId].Edit = GUILayout.Toggle(m_ItemComponentGUIs[componentId].Edit, "Edit");
        if (GUILayout.Button("Remove"))
        {
            //m_AddedItemComponents.RemoveAt(componentId);
            m_Item.Components.RemoveAt(componentId);
        }
        EditorGUILayout.EndHorizontal();

        if (m_ItemComponentGUIs[componentId].Edit)
        {
            //GUIEditItemComponent(Comp);
            GUIDrawObject(Comp);
        }
    }

    void GUIDrawObject(object Obj)
    {
        //Debug.Log(Obj);
        System.Type CompType = Obj.GetType();
        FieldInfo[] Fields = CompType.GetFields(BindingFlags.Public | BindingFlags.Instance);
        
        GUILayout.BeginVertical();
        foreach (FieldInfo Field in Fields)
        {
            object value = Field.GetValue(Obj);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(Field.Name);

            if (Field.FieldType == typeof(System.Int16))
            {
                value = (short)EditorGUILayout.IntField((short)value);
            }
            if (Field.FieldType == typeof(System.Int32))
            {
                value = (int)EditorGUILayout.IntField((int)value);
            }
            else if (Field.FieldType == typeof(System.Single))
            {
                value = (float)EditorGUILayout.FloatField((float)value);
            }
            else if (Field.FieldType == typeof(System.Boolean))
            {
                value = (bool)EditorGUILayout.Toggle((bool)value);
            }
            else if (Field.FieldType == typeof(Object) || Field.FieldType.IsSubclassOf(typeof(Object)))
            {
                value = (Object)EditorGUILayout.ObjectField((Object)value, Field.FieldType, false);
            }
            else if (Field.FieldType.BaseType == typeof(System.Enum))
            {
                value = (System.Enum)EditorGUILayout.EnumPopup((System.Enum)value);
            }
            else if (Field.FieldType == typeof(System.String))
            {
                value = (System.String)EditorGUILayout.TextField((string)value);
            }
            else if (Field.FieldType.IsClass)
            {
                EditorGUILayout.Space();
                GUIDrawObject(value);
            }

            Field.SetValue(Obj, value);
            //Debug.Log(string.Format("{0} = {1} : {2}", Field.Name, value, value.GetType()));
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    void SetItem(GameplayItem Item)
    {
        if (Item == null)
		{
			m_Item = new GameplayItem();
			return;
		}

        m_Item = Item;
        m_ItemComponentGUIs.Clear();
        foreach (GameplayItemComponent Cmp in m_Item.Components)
        {
            m_ItemComponentGUIs.Add(new ItemComponentGUI());
        }

        if (m_Item.Prefab == null && m_Item.PrefabPath != string.Empty)
        {
            m_Item.Prefab = ResourceManager.Singleton.LoadPrefab(m_Item.PrefabPath);
        }

        if (m_Item.Icon == null && m_Item.IconPath != string.Empty)
        {
            m_Item.Icon = ResourceManager.Singleton.LoadIcon(m_Item.IconPath);
        }
    }

    bool IsIntegerType(System.Type Type)
    {
        return Type == typeof(int) || Type == typeof(uint) || Type == typeof(short) || Type == typeof(ushort) || Type == typeof(long) || Type == typeof(ulong);
    }

    string CheckScriptName()
	{
		string ret = "Item";
		
		switch(Type)
		{
			case Gameplay.ItemType.ARMOR: ret = "Armor"; break;
			case Gameplay.ItemType.POTION: ret = "Potion"; break;
			case Gameplay.ItemType.WEAPON: ret = "Weapon"; break;
		}
		
		return ret;
	}
	
	GameplayItem SearchItem()
	{
		if(m_searchText == string.Empty)
			return null;
		
		GameplayItem Item = GameplayItemManager.Singleton.SearchItem(m_searchText);

		Debug.Log(Item);
		if(Item == null)
			return Item;
		
		return Item;
	}
	
	
	GameplayItem CreateNewItem()
	{
		if(!ValidateItem())
			return null;
		
        //foreach (GameplayItemComponent Comp in m_AddedItemComponents)
        //{
        //    m_Item.AddComponent(Comp);
        //}
		
		//Debug.Log("save item: " + ApplicationManager.ItemToString(m_Item));
		
		
		GameplayItemManager.Singleton.AddOrUpdateDatabaseItem(m_Item);
		
		
		return m_Item;
	}

    GameplayItem EditItem()
    {
		GameplayItemManager.Singleton.DatabaseItems.Clear();
		string err = GameplayItemManager.Singleton.LoadFromFile(AssetPaths.Resources + GameplayItemManager.BIN_FILE_NAME + GameplayItemManager.BIN_EXT);
		if(err != string.Empty)
			Debug.LogError(err);
		GameplayItemManager.Singleton.LogDatabase();
        return null;
    }
	
	bool ValidatePrefab()
	{  
		if(m_Item.Prefab != null)
		{
			PrefabType PrefType = PrefabUtility.GetPrefabType(m_Item.Prefab);
			if(PrefType != PrefabType.Prefab)
			{
				Debug.LogError(string.Format("Wrong type of prefab: {0}! Choose user created prefab", PrefType));
				return false;
			}
			
			if(m_Item.Prefab.tag != Gameplay.ObjectTag.Item)
			{
				Debug.LogError(string.Format("Wrong type of prefab. This is not an item. Prefab should has Item tag set"));
				return false;
			}
			
			if(m_Item.Prefab.GetComponent<Item>() == null)
			{
				Debug.LogError(string.Format("Wrong type of prefab. This is not an item. Prefab should has Item script set"));
				return false;
			}
			
			return true;
		}
		Debug.LogError("No prefab set");
		return false;
	}
	
	bool ValidateItem()
	{
		if(!ValidatePrefab())
			return false;
		
		if(m_Item.Name == string.Empty)
		{
			Debug.LogError("No item name");
			return false;
		}
		
		m_Item.PrefabPath = AssetDatabase.GetAssetPath(m_Item.Prefab);
		m_Item.IconPath = AssetDatabase.GetAssetPath(m_Item.Icon);
		
		return true;
	}
}

