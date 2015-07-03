using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class CMapChunk : MonoBehaviour 
{
    public bool UpWall = true; 
    public bool DownWall = true; 
    public bool LeftWall = true;
    public bool RightWall = true;
    public int ID = 15;
    public int IDX = -1;
    public int IDY = -1;
	public int Level = 0;
    public bool StartPoint = false;

    public Vector2 Size = new Vector2(12, 11);
    public Vector3 ObjPosition = new Vector3();
    public GameObject ChunkObj;

    public CMapChunk(CMapChunk MapChunk)
    {
        UpWall = MapChunk.UpWall;
        DownWall = MapChunk.DownWall;
        LeftWall = MapChunk.LeftWall;
        RightWall = MapChunk.RightWall;
        ID = MapChunk.ID;
        IDX = MapChunk.IDX;
        IDY = MapChunk.IDY;
        Size = MapChunk.Size;
        ObjPosition = MapChunk.ObjPosition;
    }

	// Use this for initialization
	void Start () 
    {
       
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}


public class SerializableChunk
{
    public bool UpWall = true;
    public bool DownWall = true;
    public bool LeftWall = true;
    public bool RightWall = true;
    public int ID = -1; //id in CDungeonGenerator.ChunkPosition
    public int IDX = -1; //x position in grid
    public int IDY = -1; //y position in grid
	public int Level = 0;
    public bool StartPoint = false;

    public Vector2 Size = new Vector2(12, 11);
    public Vector3 Position = new Vector3();

    public SerializableChunk()
    {
    }

    public SerializableChunk(CMapChunk Chunk)
    {
        DownWall = Chunk.DownWall;
        UpWall = Chunk.UpWall;
        LeftWall = Chunk.LeftWall;
        RightWall = Chunk.RightWall;
        ID = Chunk.ID;
        Size = Chunk.Size;
        IDX = Chunk.IDX;
        IDY = Chunk.IDY;
        Position = Chunk.ObjPosition;
        StartPoint = Chunk.StartPoint;
		Level = Chunk.Level;
    }
}

public class SerializableDungeon
{
	public string Name = "";
	public int Width = 0;
	public int Height = 0;
	public int Levels = 0;
    public SerializableChunk[] Chunks = new SerializableChunk[0];
	
	public override string ToString ()
	{
		string ret = string.Format("Name: {0}\nSize: {1}x{2}\nLevels: {3}\nChunks: {4}", Name, Width, Height, Levels, Chunks.Length);
		
		return ret;
	}

    public void CreateChunks(int width, int height, int levels)
    {
		if(Chunks != null)
			return;
		
		Width = width;
		Height = height;
		Levels = levels;
		
		int count = width * height;
        Chunks = new SerializableChunk[count];
		for(int i = 0; i < Chunks.Length; ++i)
		{
			Chunks[i] = new SerializableChunk();
		}
    }
	
	public void DestroyChunks()
	{
		Width = Height = Levels = 0;
		Chunks = null;
	}

    public void SaveToXML(string fileName)
    {
        System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(typeof(SerializableDungeon));
        System.IO.TextWriter Writer = new System.IO.StreamWriter(fileName);

        Serializer.Serialize(Writer, this);
        Writer.Close();
    }

    public void ReadFromXML(string fileName)
    {
        System.Xml.Serialization.XmlSerializer Serializer = new System.Xml.Serialization.XmlSerializer(typeof(SerializableDungeon));
        System.IO.TextReader Reader = new System.IO.StreamReader(fileName);

        SerializableDungeon Dungeon = Serializer.Deserialize(Reader) as SerializableDungeon;
        
		Chunks = Dungeon.Chunks;
		Width = Dungeon.Width;
		Height = Dungeon.Height;
		Levels = Dungeon.Levels;
		
		//Debug.Log(fileName);
        Reader.Close();
    }
}