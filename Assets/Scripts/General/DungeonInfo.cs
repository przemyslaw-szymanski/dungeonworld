using UnityEngine;
using System.Collections;

public class DungeonInfo  : TSingleton<DungeonInfo>
{
    public string FileName = "test.xml";
	
	public GameObject[,] Chunks = null;
	private int m_width = 0;
	private int m_height = 0;

    public class ChunkPosition
    {
        public const int LEFT_RIGHT = 0;
        public const int UP_DOWN = 1;
        public const int LEFT_DOWN = 2;
        public const int LEFT_UP = 3;
        public const int RIGHT_DOWN = 4;
        public const int RIGHT_UP = 5;
        public const int RIGHT = 6;
        public const int LEFT = 7;
        public const int UP = 8;
        public const int DOWN = 9;
        public const int LEFT_RIGHT_DOWN = 10;
        public const int LEFT_RIGHT_UP = 11;
        public const int UP_DOWN_LEFT = 12;
        public const int UP_DOWN_RIGHT = 13;
        public const int LEFT_RIGHT_UP_DOWN = 14;
        public const int NONE = 15;
    }

    public DungeonInfo()
    {
    }
	
	public void CreateChunks(int width, int height)
	{
		m_width = width;
		m_height = height;
		
		Chunks = new GameObject[width, height];
	}
}
