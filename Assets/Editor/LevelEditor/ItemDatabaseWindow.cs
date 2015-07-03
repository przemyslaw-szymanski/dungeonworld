using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
 
public class ItemDatabaseWindow : EditorWindow 
{
 	Gameplay.ItemType m_itemType = Gameplay.ItemType.NONE;
	Gameplay.ItemSubType m_itemSubType = Gameplay.ItemSubType.NONE;
	string m_name = "";
	bool m_searchName = true;
	bool m_searchType = false;
	bool m_searchSubType = false;
	Vector2 m_vecScroll = new Vector2();
    static GUILayoutOption[] m_IconOptions = new GUILayoutOption[2];
	
	static Dictionary<string, GameplayItem> m_Items = new Dictionary<string, GameplayItem>();
	
	[MenuItem ("Window/My EditorWindow")]
	static void Init ()
	{
		// Get existing open window or if none, create it
 
		ItemDatabaseWindow window = (ItemDatabaseWindow)EditorWindow.GetWindow(typeof(ItemDatabaseWindow));
		
		if(GameplayItemManager.Singleton.DatabaseItems.Count == 0)
		{
			GameplayItemManager.Singleton.LoadDatabase();
		}
		
		m_Items = GameplayItemManager.Singleton.CloneDatabase();
        m_IconOptions = new GUILayoutOption[] { GUILayout.Width(30), GUILayout.Height(30) };
	}
 
	void OnGUI ()
	{
		EditorGUILayout.BeginHorizontal();
		m_searchName = EditorGUILayout.Toggle(m_searchName);
		m_name = EditorGUILayout.TextField(m_name);
		m_searchType = EditorGUILayout.Toggle(m_searchType);
		m_itemType = (Gameplay.ItemType)EditorGUILayout.EnumPopup(m_itemType);
		m_searchSubType = EditorGUILayout.Toggle(m_searchSubType);
		m_itemSubType = (Gameplay.ItemSubType)EditorGUILayout.EnumPopup(m_itemSubType);
		if(GUILayout.Button("Search"))
		{
			Search();
		}
		EditorGUILayout.EndHorizontal();
		
		m_vecScroll = EditorGUILayout.BeginScrollView(m_vecScroll);
		EditorGUILayout.BeginVertical();
		
		EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("#", GUILayout.Width(20));
        EditorGUILayout.LabelField("Name", GUILayout.Width(200));
        EditorGUILayout.LabelField("Type", GUILayout.Width(100));
        EditorGUILayout.LabelField("Sub Type", GUILayout.Width(100));
		EditorGUILayout.EndHorizontal();
		
		int id = 1;
		foreach(GameplayItem Item in m_Items.Values)
		{
			if(Item.Icon == null && Item.IconPath != string.Empty)
			{
				Item.Icon = ResourceManager.Singleton.LoadIcon(Item.IconPath);
			}
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField((id++).ToString(), GUILayout.Width(20));
			EditorGUILayout.LabelField(Item.Name, GUILayout.Width(200));
			EditorGUILayout.LabelField(Item.Type.ToString(), GUILayout.Width(100));
            EditorGUILayout.LabelField(Item.SubType.ToString(), GUILayout.Width(100));
            GUILayout.Label(Item.Icon, m_IconOptions);
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndScrollView();
	}
	
	void Search()
	{
        m_Items.Clear();
        foreach (KeyValuePair<string, GameplayItem> Pair in GameplayItemManager.Singleton.DatabaseItems)
        {
            Debug.Log(Pair.Key);
            if (m_searchName)
            {
                if (Pair.Key.Contains(m_name))
                {
                    m_Items.Add(Pair.Key, Pair.Value);
                    continue;
                }
            }

            if (m_searchType)
            {
                if (Pair.Value.Type == m_itemType)
                {
                    m_Items.Add(Pair.Key, Pair.Value);
                    continue;
                }
            }

            if (m_searchSubType)
            {
                if (Pair.Value.SubType == m_itemSubType)
                {
                    m_Items.Add(Pair.Key, Pair.Value);
                    continue;
                }
            }
        }
	}
}
