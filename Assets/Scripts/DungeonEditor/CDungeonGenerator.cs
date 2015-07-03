using UnityEngine;
using System.Collections;

public class CDungeonGenerator : MonoBehaviour 
{
    public int MapWidth = 33;
    public int MapHeight = 33;
    public float ChunkHeight = 10;
    public float ChunkWidth = 10;
    public CMapChunk[] MapChunks = new CMapChunk[16];

	// Use this for initialization
	void Start () 
	{
		Debug.Log("Start");

        CreateMapChunks();

        renderer.material.mainTexture = CreateDungeonMap();
        renderer.material.mainTexture.wrapMode = TextureWrapMode.Clamp;
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public Texture2D CreateDungeonMap()
    {
        var Texture = new Texture2D(MapWidth, MapHeight, TextureFormat.RGB24, false);
        Color Color = new Color();
        CMapChunk Chunk;
        Vector3 pos = new Vector3();

        //for (int y = 0; y < MapHeight; y += 3)
        //{
        //    for (int x = 0; x < MapWidth; x += 3)
        //    {
        //        for (int pixY = 0; pixY < 3; ++pixY)
        //        {
        //            for (int pixX = 0; pixX < 3; ++pixX)
        //            {
        //                Color.r = Color.g = Color.b = Chunk.Points[pixY, pixX];
        //                Texture.SetPixel(x + pixX, y + pixY, Color);
        //            }
        //        }
        //    }
        //}

        for (int y = 0; y < MapHeight; y += 1)
        {
            for (int x = 0; x < MapWidth; x += 1)
            {
                int id = Random.Range(0, 15);
                Chunk = MapChunks[id];
                pos.x = x  * Chunk.Size.x;
                pos.z = y  * Chunk.Size.y;
                GameObject Obj = Instantiate(Chunk, pos, Quaternion.identity) as GameObject;
                Debug.Log("id: " + id + ": " + Chunk.name);
            }
        }

        Texture.Apply();

        SaveTextureToFile(Texture, "texture.png");

        return Texture;
    }

    void SaveTextureToFile(Texture2D Tex, string fileName)
    {
        var bytes= Tex.EncodeToPNG();
        var file = System.IO.File.Open(fileName, System.IO.FileMode.Create);
        var binary= new System.IO.BinaryWriter(file);
        binary.Write(bytes);
        file.Close();
    }

    private void CreateMapChunks()
    {
        
    }
}
