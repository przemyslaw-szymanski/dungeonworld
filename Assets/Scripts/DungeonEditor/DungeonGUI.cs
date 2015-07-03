using UnityEngine;
using System.Collections;

public class DungeonGUI : MonoBehaviour 
{
	private int m_selectedChunk = -1;
	private int m_lastSelectedChunk = -1;
	private int m_selectedGridChunk = -1;
	private int m_lastSelectedGridChunk = -1;
	
	public int DungeonWidth = 10;
	public int DungeonHeight = 10;
    public int DungeonLevels = 1;

    public string DungeonName = "test.xml";
	
	private GameObject DungeonObj = null;
	private GameObject DungeonGridObj = null;
	
	public int GUIChunkSize = 25;
    public Vector2 GridChunkSize = new Vector2(12, 11);
	public Texture2D[] ChunkTextures = new Texture2D[16];
	//public CMapChunk[] Chunks = new CMapChunk[16];
    public GameObject[] Chunks = new GameObject[16];
	
	private bool m_dungeonGridCreated = false;
	private Texture2D[] m_DungeonGridTextures = new Texture2D[1];
	
	//private CMapChunk[] m_MapChunks = null;
    private SerializableDungeon m_SerializableDungeon = new SerializableDungeon();

    public GameObject ChunkCube = null;
    private GameObject[,] m_ChunkCubes = null;

    class SelectedObject
    {
        public GameObject Current;
        public GameObject Last;
        public GameObject Clicked;
    }

    private SelectedObject m_SelectedObj = new SelectedObject();
	
	// Use this for initialization
	void Start () 
	{
        ChunkCube.transform.localScale = new Vector3(GridChunkSize.x, GridChunkSize.y, GridChunkSize.x);
        foreach (Texture2D Tex in m_DungeonGridTextures)
        {
            //Tex.Resize(GUIChunkSize, GUIChunkSize);
        }
	}

	// Update is called once per frame
	void Update () 
	{
        CheckSelection(false, false);

		if(Input.GetMouseButtonDown(0))
		{
            int selResult = CheckSelection(true, false);

			if(m_selectedChunk == -1 || selResult != 1)
				return;

            PutMapChunkOnGrid();
		}
	}

    void PutMapChunkOnGrid()
    {
        ChunkCube ChunkCube = m_SelectedObj.Clicked.GetComponent<ChunkCube>();
        //Debug.Log(string.Format("selected chunk: {0}, id: {1}, idx: {2}, idy: {3}", m_selectedChunk, ChunkCube.ID, ChunkCube.IDX, ChunkCube.IDY));
        GameObject Chunk = Chunks[m_selectedChunk];

        //Debug.Log(string.Format("chunk cube: {0}, has geom: {1}", ChunkCube.name, m_SelectedObj.Clicked.gameObject.gameObject.name));

        GameObject.DestroyImmediate(ChunkCube.Geometry);

        Vector3 vecPos = ChunkCube.transform.position;
        vecPos.y = -GridChunkSize.y * 0.5f - 0.5f; //Set it under the chunk cube to select only a chunk cube object
    
        ChunkCube.Geometry = InstantiateChunk(Chunk, vecPos);
		
		m_SerializableDungeon.Chunks[ChunkCube.ID] = CreateSerializableChunk(Chunk, ChunkCube.IDX, ChunkCube.IDY, vecPos);
    }
	
	GameObject InstantiateChunk(GameObject Chunk, Vector3 vecPos)
	{
		GameObject Obj = Instantiate(Chunk, vecPos, Quaternion.identity) as GameObject;
		Obj.transform.parent = DungeonObj.transform;
		
		return Obj;
	}
	
	SerializableChunk CreateSerializableChunk(GameObject BaseChunk, int idx, int idy, Vector3 vecPos)
	{
		SerializableChunk SChunk = new SerializableChunk(BaseChunk.GetComponent<CMapChunk>());
        SChunk.IDX = idx;
        SChunk.IDY = idy;
        SChunk.Position = vecPos;
		return SChunk;
	}
	
	void OnGUI () 
	{	
		// Make a background box
        int chunksWidth = 4 * GUIChunkSize + 10;
        int chunksHeight = chunksWidth + 30;
        int posY = 10;

		GUI.Box(new Rect(10,10, chunksWidth, chunksHeight), "Chunks");
        posY += chunksHeight + 10;
        int optionsHeight = 100;
        GUI.Box(new Rect(10, posY, chunksWidth, optionsHeight), "Dungeon");
		
		m_selectedChunk = GUI.SelectionGrid(new Rect(15, 35, chunksWidth - 10, chunksHeight - 30), m_selectedChunk, ChunkTextures, 4);
		
		//m_selectedGridChunk = GUI.SelectionGrid(new Rect(300, 10, DungeonWidth * (GridChunkSize + 10), DungeonHeight * (GridChunkSize + 10)), m_selectedGridChunk, m_DungeonGridTextures, (int)DungeonWidth);

        posY += 20;
        int btnWidth = chunksWidth - 10;
        int btnHeight = 20;
		if(GUI.Button(new Rect(15, posY, btnWidth, btnHeight), "Create")) 
		{
			CreateDungeonGrid();
		}

        posY += 20;
        if (GUI.Button(new Rect(15, posY, btnWidth, btnHeight), "Save")) 
		{
			SaveToXML();
		}
		
		posY += 20;
        if (GUI.Button(new Rect(15, posY, btnWidth, btnHeight), "Load")) 
		{
            LoadFromXML();
		}

        posY += 20;
        if (GUI.Button(new Rect(15, posY, btnWidth, btnHeight), "Test")) 
		{
            Application.LoadLevel("DungeonTest");
		}
	}
	
	void CreateDungeonGrid()
	{
		if(m_dungeonGridCreated)
			return;
		m_dungeonGridCreated = true;
		
		
		GameObject.Destroy(DungeonObj);
		DungeonObj = new GameObject(DungeonName);
		
		GameObject.Destroy(DungeonGridObj);
		DungeonGridObj = new GameObject(DungeonName + "_grid");
		
		//m_DungeonGridTextures = new Texture2D[DungeonWidth * DungeonHeight];
        int count = DungeonWidth * DungeonHeight;
        m_ChunkCubes = new GameObject[DungeonLevels, count];
        
		m_SerializableDungeon.CreateChunks(DungeonWidth, DungeonHeight, DungeonLevels);
		m_SerializableDungeon.Name = DungeonName;
		
		Debug.Log(m_SerializableDungeon);
		
        Vector3 vecPos = new Vector3(0, 0, 0);
        int currGrid = 0;
		
		//Debug.Log(string.Format("levels: {0}, height: {1}, width: {2}", DungeonLevels, DungeonHeight, DungeonWidth));
		
        for (int i = 0; i < DungeonLevels; ++i)
        {
            for (int z = 0; z < DungeonHeight; ++z)
            {
                for (int x = 0; x < DungeonWidth; ++x)
                {
                    GameObject Cube = Instantiate(ChunkCube, vecPos, Quaternion.identity) as GameObject;
					Cube.transform.parent = DungeonGridObj.transform;
					
                    //Scale to fit an original map chunk
                    Cube.transform.localScale = new Vector3(GridChunkSize.x + 0.01f, GridChunkSize.y + 10, GridChunkSize.x + 0.01f); 
                    Cube.name = "ChunkCube_" + i + "_" + currGrid;
                    ChunkCube CChunk = Cube.GetComponent<ChunkCube>();
                    CChunk.ID = currGrid;
                    CChunk.IDX = x;
                    CChunk.IDY = z;
         
                    m_ChunkCubes[i, currGrid++] = Cube;
                    vecPos.x += GridChunkSize.x;
                }

                vecPos.x = 0;
                vecPos.z += GridChunkSize.x;
            }

            currGrid = 0;
            vecPos.x = vecPos.z = 0;
            vecPos.y -= GridChunkSize.y;
        }
	}

    int CheckSelection(bool leftMouse, bool rightMouse)
    {
        var Hit = new RaycastHit();
        var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(Ray, out Hit, 1000.0f))
        {
            if (!leftMouse && m_SelectedObj.Current == Hit.collider.gameObject)
                return 0;

            m_SelectedObj.Last = m_SelectedObj.Current;
            m_SelectedObj.Current = Hit.collider.gameObject;

            if (Hit.collider.gameObject.tag == "ChunkCube")
            {         
                ChunkCube Cube = m_SelectedObj.Current.GetComponent<ChunkCube>();
                Cube.Highlighted = true;
                Cube.Clicked = leftMouse;

                if (leftMouse)
                {
                    //Unclick last clicked object
                    if (m_SelectedObj.Clicked != null)
                    {
                        Cube = m_SelectedObj.Clicked.GetComponent<ChunkCube>();
                        Cube.Highlighted = false;
                        Cube.Clicked = false;
                        m_SelectedObj.Clicked = null;
                    }
                    m_SelectedObj.Clicked = m_SelectedObj.Current;
                }
               
                if (m_SelectedObj.Last != null)
                {
                    if (m_SelectedObj.Clicked != m_SelectedObj.Last)
                    {
                        Cube = m_SelectedObj.Last.GetComponent<ChunkCube>();
                        if (Cube != null)
                        {
                            Cube.Highlighted = false;
                            Cube.Clicked = leftMouse;
                        }
                    }
                }

                return 1;
            }

            return 2;
        }

        return 0;
    }
	
	void SaveToXML()
	{
		Debug.Log("Saving...");
        DungeonInfo.Singleton.FileName = DungeonName;
		m_SerializableDungeon.Name = DungeonName;
        m_SerializableDungeon.SaveToXML(DungeonName);
		Debug.Log("Saved");
	}
	
	int CalcArrayId(int x, int y, int width)
	{
		return x + (y * width);
	}
	
	void LoadFromXML()
	{
		DungeonInfo.Singleton.FileName = DungeonName;
		m_SerializableDungeon.ReadFromXML(DungeonName);
		
		CreateDungeonGrid();
		
		foreach(SerializableChunk Chunk in m_SerializableDungeon.Chunks)
		{
			//Debug.Log(string.Format("{0},{1},{2}", Chunk.ID, Chunk.IDX, Chunk.Position));
			GameObject Obj = InstantiateChunk(Chunks[Chunk.ID], Chunk.Position);
			//m_ChunkCubes[CalcArrayId(Chunk.IDX, Chunk.IDY, m_SerializableDungeon.Width)].Geometry = Obj;

			int id = CalcArrayId(Chunk.IDX, Chunk.IDY, m_SerializableDungeon.Width);
			//Debug.Log(string.Format("{0}, {1}, {2}", Chunk.IDX, Chunk.IDY, id));
			m_ChunkCubes[0, id].GetComponent<ChunkCube>().Geometry = Obj;
		}
	}
}
