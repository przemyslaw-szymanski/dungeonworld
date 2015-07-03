using UnityEngine;
using System.Collections;

[System.Serializable]
public class GUIStyles
{
	public GUIStyle ItemSlot; //slot rect in loot window or inventory window
	public GUIStyle CloseButton; //close button
    public GUIStyle ItemTooltipLabel; //label in the tooltip box with item description
}

public class GameGUI : MonoBehaviour 
{
    //private InventoryGUI m_Inventory;
    //private BaseGUI m_CurrGUI;
    public static BaseGUI CurrGUI;
	
    private InventoryGUI m_Inventory;
	private BackpackGUI m_Backpack;
	private EquipmentGUI m_Equipment;
    private LootWindowGUI m_LootWindow;
	private ItemDragGUI m_ItemDrag;
	
	private int framesnum = 0;
	private float time = 0f;
	private float fps = 0f;
	private int m_currWndId = 0;

    private static GameGUI m_Singleton = null;

    public GUIStyle CloseButtonStyle = new GUIStyle();
	
	public GUIStyles Styles;
	
	public float ImageSlotScale = 0.1f;
    public float GUIScale = 1.0f;

    public static GameGUI Singleton
    {
        get
        {
            return m_Singleton;
        }
    }

    void Awake()
    {
        m_Singleton = this;
    }

	// Use this for initialization
	void Start () 
    {
        m_Inventory = this.transform.Find("InventoryGUI").gameObject.GetComponent<InventoryGUI>();
        m_LootWindow = this.transform.Find("LootWindowGUI").gameObject.GetComponent<LootWindowGUI>();
		m_Backpack = this.transform.Find("BackpackGUI").gameObject.GetComponent<BackpackGUI>();
		m_Equipment = this.transform.Find("EquipmentGUI").gameObject.GetComponent<EquipmentGUI>();
		m_ItemDrag = this.transform.Find("ItemDragGUI").gameObject.GetComponent<ItemDragGUI>();
	}
	 
    void Update()
    {
		Resize();
		
        if (Input.GetButtonDown("Inventory"))
        {
            m_Inventory.Show(!m_Inventory.Visible);
        }
		else if(Input.GetButtonDown("Equipment"))
		{
			m_Equipment.Show(!m_Equipment.Visible);
		}
		else if(Input.GetButtonDown("Backpack"))
		{
			m_Backpack.Show(!m_Backpack.Visible);
		}
		
    }
	
	void Resize()
	{
		Styles.ItemSlot.fixedWidth = Styles.ItemSlot.fixedHeight = (float)Screen.height * ImageSlotScale * GUIScale;
	}

    void OnGUI()
    {
        
    }

    void ProcessKeyboard()
    {
    }

    public LootWindowGUI LootWindow
    {
        get { return m_LootWindow; }
    }
	
	public int GenerateWindowId()
	{
		return m_currWndId++;
	}
	
	public ItemDragGUI ItemDrag
	{
		get { return m_ItemDrag; }
	}
	
	public Texture EmptySlotTexture
	{
		get { return Styles.ItemSlot.normal.background; }
	}

    public static ItemTooltipGUI CreateTooltip(BaseGUI Parent)
    {
        GameObject Obj = new GameObject(string.Format("{0}.ItemTooltipGUI", Parent.name));
        ItemTooltipGUI ItemTooltip = Obj.AddComponent<ItemTooltipGUI>();
        Obj.transform.parent = Parent.transform;
        return ItemTooltip;
    }
}
