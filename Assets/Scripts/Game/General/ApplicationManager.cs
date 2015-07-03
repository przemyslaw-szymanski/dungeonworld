using System;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ApplicationManager : TSingleton<ApplicationManager>
{
	protected Player m_Player;
	
	public Player GetPlayer()
	{
		if(m_Player != null)
			return m_Player;
		m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		return m_Player;
	}
	
	public static string ItemToString(GameplayItem Obj)
	{
		FieldInfo[] Fields = Obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
		string str = string.Format("Type: {0}", Obj.GetType().Name);
		foreach(FieldInfo Info in Fields)
		{
			object val = Info.GetValue(Obj);
			str += string.Format("\n {0}: {1}", Info.Name, val);
		}
		
		str += "\nComponents: " + Obj.Components.Count;
		foreach(GameplayItemComponent Cmp in Obj.Components)
		{
			Fields = Cmp.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
			str += string.Format("\n {0}", Cmp.GetType().Name);
			foreach(FieldInfo Info in Fields)
			{
				object val = Info.GetValue(Cmp);
				str += string.Format("\n  {0}: {1}", Info.Name, val);
			}
		}
		return str;
	}

}

public class ResourceManager : TSingleton<ResourceManager>
{
	public static Texture[] PortraitTextures = null;
	
	//protected Dictionary<string, GameObject> m_PrefabMap = new Dictionary<string, GameObject>();
	protected Hashtable m_PrefabMap = new Hashtable();
	protected Hashtable m_IconMap = new Hashtable();
	protected Dictionary<System.Type, Hashtable> m_ResourceMap = new Dictionary<System.Type, Hashtable>();
	
	public ResourceManager()
	{
		AddResourceType<Texture>();
		AddResourceType<GameObject>();
	}
	
	public Hashtable AddResourceType<T>()
	{
		Hashtable Table = new Hashtable();
		m_ResourceMap.Add(typeof(T), Table);
		return Table;
	}
	
	public static bool LoadPortraits()
	{
		if(PortraitTextures != null)
			return true;
		
		PortraitTextures = UnityEngine.Resources.LoadAll(AssetPaths.Portraits, typeof(Texture)) as Texture[];
		if(PortraitTextures == null)
			return false;
		return true;
	}
	
	public static Texture GetRandomPortrait()
	{
		if(!LoadPortraits())
			return null;
		return PortraitTextures[UnityEngine.Random.Range(0, PortraitTextures.Length -1)];
	}
	
	public GameObject LoadPrefab(string path)
	{
		GameObject Obj = m_PrefabMap[path] as GameObject;
		if(Obj != null)
			return Obj;
		
		Obj = Resources.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
		m_PrefabMap.Add(path, Obj);
		return Obj;
	}
	
	public Texture2D LoadIcon(string path)
	{
		Texture2D Tex = m_IconMap[path] as Texture2D;
		if(Tex != null)
		{
			return Tex;
		}
		
		Tex = Resources.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
		m_IconMap.Add(path, Tex);
		return Tex;
	}
	
	public T LoadResource<T>(string path) where T: class
	{
		Hashtable Table = m_ResourceMap[typeof(T)];
		T Obj = Table[path] as T;
		if(Obj != null)
			return Obj;
		
		Obj = Resources.LoadAssetAtPath(path, typeof(T)) as T;
		return Obj;
	}
	
}

public static class AssetPaths
{
	public const string Portraits = "Assets/Textures/Portraits/";
	public const string ItemPrefabs = "Assets/Prefabs/Game/Items/";
	public const string Resources = "Assets/Resources/";
}

public class GameText : TSingleton<GameText>
{
    string[] m_ItemTypes = new string[(int)Gameplay.ItemType._COUNT];
    string[] m_ItemSubTypes = new string[(int)Gameplay.ItemSubType._COUNT];
    string[] m_DamageTypes = new string[(int)Gameplay.DamageType._COUNT];

    public GameText()
    {
        string[] names = System.Enum.GetNames(typeof(Gameplay.ItemType));
        for (int i = 0; i < m_ItemTypes.Length; ++i)
        {
            m_ItemTypes[i] = GameUtils.ParseEnumName(names[i]);
        }

        names = System.Enum.GetNames(typeof(Gameplay.ItemSubType));
        for (int i = 0; i < m_ItemSubTypes.Length; ++i)
        {
            m_ItemSubTypes[i] = GameUtils.ParseEnumName(names[i]);
        }

        names = System.Enum.GetNames(typeof(Gameplay.DamageType));
        for (int i = 0; i < m_DamageTypes.Length; ++i)
        {
            m_DamageTypes[i] = GameUtils.ParseEnumName(names[i]);
        }
    }

    public string GetItemType(Gameplay.ItemType Type)
    {
        return m_ItemTypes[(int)Type];
    }

    public string GetItemSubType(Gameplay.ItemSubType Type)
    {
        return m_ItemSubTypes[(int)Type];
    }

    public string GetDamageType(Gameplay.DamageType Type)
    {
        //Debug.Log(string.Format("{0}, {1}, {2}", m_DamageTypes.Length, (int)Type, Type));
        return m_DamageTypes[(int)Type];
    }
}

public static class GameUtils
{
    public static string ParseEnumName(string name)
    {
        string str = "";
        string[] astr = name.Split('_');

        for (int i = 0; i < astr.Length; ++i)
        {
            astr[i] = astr[i].ToLower();
            System.Text.StringBuilder SB = new System.Text.StringBuilder(astr[i]);
            SB[0] = Char.ToUpper(SB[0]);
            astr[i] = SB.ToString();
            str += astr[i] + ((i + 1 == astr.Length) ? "" : " ");
        }

        return str;
    }

    public static string EnumToString(System.Enum Enum)
    {
        return ParseEnumName(Enum.ToString());
    }
}