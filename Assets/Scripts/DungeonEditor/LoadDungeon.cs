using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadDungeon : MonoBehaviour 
{
    public CMapChunk[] MapChunks = new CMapChunk[16];

    private List<GameObject> m_MapChunks = new List<GameObject>(); 

	// Use this for initialization
	void Start () 
    {
        SerializableDungeon Dungeon = new SerializableDungeon();
        Dungeon.ReadFromXML(DungeonInfo.Singleton.FileName);
        Vector3 vecPos = new Vector3();
        Debug.Log(DungeonInfo.Singleton.FileName);
     
        foreach (SerializableChunk Chunk in Dungeon.Chunks)
        {
            if (Chunk == null)
                continue;
            CMapChunk CurrChunk = MapChunks[Chunk.ID];
            vecPos = Chunk.Position;
            GameObject Obj = Instantiate(CurrChunk, vecPos, Quaternion.identity) as GameObject;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
