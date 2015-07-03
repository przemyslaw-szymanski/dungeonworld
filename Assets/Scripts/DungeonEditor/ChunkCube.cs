using UnityEngine;
using System.Collections;

public class ChunkCube : MonoBehaviour
{
    public Color ColorEmpty = new Color(0.1f, 0.7f, 0.9f, 0.5f);
    public Color ColorFilled = new Color(1.0f, 0.03f, 0.03f, 0.1f);
    public Color ColorHighlight = new Color(0, 0.9f, 0.03f, 0.5f);
    public bool Clicked = false;
    public int ID = -1;
    public int IDX = -1;
    public int IDY = -1;

    private bool m_highlighted = false;
    private bool m_empty = true;

    public CMapChunk MapChunk = null;
    public GameObject Geometry = null;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public bool Highlighted
    {
        get { return m_highlighted; }
        set
        {
            m_highlighted = value;
            if (m_highlighted)
            {
                this.renderer.material.color = ColorHighlight;
            }
            else
            {
                if (m_empty)
                {
                    this.renderer.material.color = ColorEmpty;
                }
                else
                {
                    this.renderer.material.color = ColorFilled;
                }
            }
        }
    }
}
