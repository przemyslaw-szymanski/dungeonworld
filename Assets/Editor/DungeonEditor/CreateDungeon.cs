using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CreateDungeon : ScriptableWizard
{
    public string Name = "Test";
    public string FileName = "test.xml";
    //public static GameObject[] BaseChunks = new GameObject[16];
	public static List<Object> BaseChunks = new List<Object>();

    [MenuItem("GameObject/Game/Create Dungeon")]
    static void CreateWizard()
    {
		if(BaseChunks.Count != 16)
			BaseChunks = LoadPrefabs("Assets/Prefabs/DungeonEditor/MapChunks/");
		
		if(BaseChunks.Count != 16)
		{
			Debug.LogError(string.Format("Wrong number of prefab chunks at Assets/Prefabs/DungeonEditor/MapChunks/ {0}", BaseChunks.Count));
			return;
		}
		
        ScriptableWizard.DisplayWizard("Create Dungeon", typeof(CreateDungeon));
    }

    void OnWizardUpdate()
    {
    }
	
	static List<Object> LoadPrefabs(string path)
	{
		string[] aFiles = Directory.GetFiles(path, "*.prefab");
		List<Object> Prefabs = new List<Object>();
		//Debug.Log(string.Format("file count: {0}", aFiles.Length));
		foreach(string file in aFiles)
		{
			//Debug.Log(string.Format("Loading prefab: {0}...", file));
			Object Obj = AssetDatabase.LoadMainAssetAtPath(file);
			Prefabs.Add(Obj);
			//Debug.Log(string.Format("Prefab loaded: {0}", Obj.name));
		}
		
		return Prefabs;
	}

    void OnWizardCreate()
    {
        if (Name == string.Empty)
            return;
        if (FileName == string.Empty)
            return;
   
		//Load from xml
		SerializableDungeon Dungeon = new SerializableDungeon();
		Dungeon.ReadFromXML(FileName);
	
		//At first delete dungeon obj if one is created already
		GameObject DungeonObj = GameObject.Find(Name);
		if(DungeonObj != null)
		{
			GameObject.DestroyImmediate(DungeonObj);
		}
		
        DungeonObj = new GameObject(Name);
		
		DungeonInfo.Singleton.CreateChunks(Dungeon.Width, Dungeon.Height);
		
		int currId = 0;
		
		Debug.Log(Dungeon);
		
		for(int x = 0; x < Dungeon.Width; ++x)
		{
			for(int y = 0; y < Dungeon.Height; ++y)
			{
				SerializableChunk Chunk = Dungeon.Chunks[currId];
				if(Chunk == null)
					continue;
				
				Object Obj = BaseChunks[Chunk.ID];
				GameObject MapChunk = Instantiate(Obj, Chunk.Position, Quaternion.identity) as GameObject;
			
				MapChunk.transform.parent = DungeonObj.transform;
				
				DungeonInfo.Singleton.Chunks[x, y] = MapChunk;
				
				++currId;		
			}
		}
		
		/*
		foreach(SerializableChunk Chunk in Dungeon.Chunks)
		{
			if(Chunk == null)
				continue;
			
			Object Obj = BaseChunks[Chunk.ID];
			GameObject MapChunk = Instantiate(Obj, Chunk.Position, Quaternion.identity) as GameObject;
			MapChunk.transform.parent = DungeonObj.transform;
			
			for(int i = 0; i < MapChunk.transform.childCount; ++i)
			{
				//Transform Child = MapChunk.transform.GetChild(i);
				//Child.name = string.Format("{0}_{1}_{2}", MapChunk.name, Child.name, i.ToString());
			}
		}
		*/
    }
}
