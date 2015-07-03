using UnityEngine;
using System.Collections.Generic;

public class ItemSlotGUI
{
    public Rect ImgRect = new Rect();
    //public GUIContent Content = new GUIContent();
    //public GUIStyle Style = new GUIStyle();
    protected Texture m_Tex;
	protected Texture m_DefaultTex;
    GameplayItem m_Item = null;
    bool m_mouseOver = false;
    bool m_mouseDown = false;
	bool m_locked = false;
	
	public ItemSlotGUI()
	{
		m_Tex = m_DefaultTex = GameGUI.Singleton.EmptySlotTexture;
	}
	
    public ItemSlotGUI(Texture Default) 
    {
        m_Tex = m_DefaultTex = Default;
    }
	
	public Texture Tex
	{
		get { return m_Tex; }
	}
	
	public bool Locked
	{
		get { return m_locked; }
	}
	
	public void Lock(bool locked)
	{
		m_locked = locked;
		
		if(locked)
		{
			m_Tex = new Texture2D(GameGUI.Singleton.EmptySlotTexture.width, GameGUI.Singleton.EmptySlotTexture.height);
		}
		else
		{
			m_Tex = m_DefaultTex;
		}
	}

    public void Draw()
    {
        m_mouseOver = false;
        m_mouseDown = false;
        GUI.DrawTexture(ImgRect, m_Tex, ScaleMode.StretchToFill);
        if (ImgRect.Contains(Event.current.mousePosition))
        {
            m_mouseOver = true;
            if (Event.current.type == EventType.mouseDown || Event.current.type == EventType.used)
            {
                m_mouseDown = true;
            }
        }
    }

    public bool MouseOver
    {
        get { return m_mouseOver; }
    }

    public bool MouseDown
    {
        get { return m_mouseDown; }
    }

    public GameplayItem Item
    {
        get { return m_Item; }
        set
        {
			if(Locked)
				return;
			
            if (value == null)
            {
                m_Item = value;
                m_Tex = m_DefaultTex;
            }
            else
            {
                m_Item = value;
                m_Tex = m_Item.GetIcon();
            }
        }
    }
}

public class ItemDescLineGUI
{
    public class Column
    {
        public GUIStyle Style;
        public string text = "";
        public int width = 0;
        public Texture Icon;

        public Column() 
        {
            Style = new GUIStyle(GameGUI.Singleton.Styles.ItemTooltipLabel);
        }
    }

    public Column[] Columns;

    public ItemDescLineGUI(int columns) 
    {
        Columns = new Column[columns];
        for (int i = 0; i < columns; ++i)
        {
            Columns[i] = new Column();
        }
    }

}

public class MovableTextureGUI
{
	Rect m_Rect = new Rect();
	Texture m_Tex;
	bool m_canDraw = false;
	
	public MovableTextureGUI(Rect StartRect, Texture Tex)
	{
		m_Rect = StartRect;
		m_Tex = Tex;
	}
	
	public void Move(Vector2 vecPos)
	{
		m_Rect.x = vecPos.x;
		m_Rect.y = vecPos.y;
	}
	
	public void Draw()
	{
		if(m_canDraw)
			GUI.DrawTexture(m_Rect, m_Tex);
	}
	
	public Texture Tex
	{
		get { return m_Tex; }
		set { m_Tex = value; }
	}
	
	public bool CanDraw
	{
		get { return m_canDraw; }
		set { m_canDraw = value; }
	}
	
	public bool IsClicked
	{
		get
		{
			if(m_Rect.Contains(Event.current.mousePosition) && Event.current.button == 0)
			{
				return true;
			}
			
			return false;
		}
	}
}

public class BaseItemWindowGUI : BaseGUI 
{
    protected int m_wndId = 0;
    protected int m_rowWidth = 4; //items per row - vertical
    protected int m_borderWidth = 3;
    protected int m_colHeight = 4; //items per column - horizontal
    protected float m_titleBarHeight = 20;

    protected Rect m_TooltipWndRect = new Rect();
    protected int m_tooltipWndId = 1;
	protected int m_tooltipItemId = -1;
	protected float m_imgSize = 0;
	
	protected Rect m_CloseBtnRect = new Rect();

    protected List<ItemSlotGUI> m_ItemSlots = new List<ItemSlotGUI>();
    protected GameplayItem m_SelectedItem;
    protected ItemTooltipGUI m_ItemTooltip;

    public Texture EmptySlotTexture;
    public Texture Temp;
	
	public string WindowTitle = "";
	
	protected MovableTextureGUI m_MovableTex;
	protected bool m_drawItemInfoWnd = true;
	//public GUIStyle SlotStyle;// = GUI.skin.label;

    public BaseItemWindowGUI()
    {
        
    }
	
	public void SetMaxSize(int width, int height)
	{
		m_colHeight = height;
		m_rowWidth = width;
	}

    protected override void OnAwake()
    {
        m_ItemTooltip = GameGUI.CreateTooltip(this);
    }
	
	protected override void OnStart()
	{
		m_tooltipWndId = GameGUI.Singleton.GenerateWindowId();
		
		m_imgSize = GameGUI.Singleton.Styles.ItemSlot.fixedWidth;
		MainWndRect.width = m_imgSize * m_rowWidth + m_borderWidth * 2; //left-right border
        MainWndRect.height = m_imgSize * m_colHeight + m_borderWidth * 2 + m_titleBarHeight;
        MainWndRect.x = Screen.width * 0.5f - MainWndRect.width * 0.5f;
        MainWndRect.y = Screen.height * 0.25f - MainWndRect.height * 0.5f;

		m_MovableTex = new MovableTextureGUI(new Rect(0, 0, m_imgSize, m_imgSize), GameGUI.Singleton.Styles.ItemSlot.normal.background);
		
        CreateItemSlots();
	}

    protected override void OnResize()
    {	
		m_imgSize = GameGUI.Singleton.Styles.ItemSlot.fixedHeight;
		MainWndRect.width = m_imgSize * m_rowWidth + m_borderWidth * 2; //left-right border
        MainWndRect.height = m_imgSize * m_colHeight + m_borderWidth * 2 + m_titleBarHeight;
		//m_CloseBtnRect.width = m_CloseBtnRect.height = Screen.currentResolution.height * 0.02f;
		m_CloseBtnRect.width = m_CloseBtnRect.height = GameGUI.Singleton.Styles.CloseButton.fixedHeight;
		m_CloseBtnRect.x = MainWndRect.width - m_CloseBtnRect.width - m_borderWidth;
		m_CloseBtnRect.y = 0;
		m_WndDragRect.x = m_WndDragRect.y = 0;
		m_WndDragRect.width = MainWndRect.width - m_CloseBtnRect.width;
		m_WndDragRect.height = m_titleBarHeight;

        int currRect = 0;
        Vector2 vecPos = new Vector2(m_borderWidth * 2, m_borderWidth + m_titleBarHeight);
        for (int y = 0; y < m_rowWidth; ++y)
        {
            vecPos.x = m_borderWidth;

            for (int x = 0; x < m_colHeight; ++x)
            {
                m_ItemSlots[currRect].ImgRect.width = m_ItemSlots[currRect].ImgRect.height = GameGUI.Singleton.Styles.ItemSlot.fixedWidth;
                m_ItemSlots[currRect].ImgRect.x = vecPos.x;
                m_ItemSlots[currRect].ImgRect.y = vecPos.y;
                //Debug.Log(m_ImgLabelRects[currRect]);
                vecPos.x += m_imgSize;
                currRect++;
            }

            vecPos.y += m_imgSize;
        }
    }

    protected override void OnDraw()
    {
		GUI.SetNextControlName(this.m_controlName);
        MainWndRect = GUI.Window(this.MainWndID, MainWndRect, WndFunc, WindowTitle);
		if(m_SelectedItem != null)
		{
			m_ItemTooltip.Show(m_SelectedItem, Event.current.mousePosition.x + 1, Event.current.mousePosition.y + 1);	
			m_SelectedItem = null; //show only one time per frame
		}
		else
		{
            m_ItemTooltip.Show(false);
		}
		
    }
	
	public void MoveWindow(int posX, int posY)
	{
	}

    void WndFunc(int wndId)
    {
        int currRect = 0;
		ItemSlotGUI Slot;
		
        for (int y = 0; y < m_rowWidth; ++y)
        {
            for (int x = 0; x < m_colHeight; ++x)
            {
				Slot = m_ItemSlots[currRect];
				
				GUI.SetNextControlName(m_controlName);

                Slot.Draw();
                if (Slot.MouseOver)
                {
                    if (Slot.Item != null)
                    {
                        m_SelectedItem = Slot.Item;
                        m_tooltipItemId = currRect;
                    }

                    OnMouseOverItemSlot(Slot);

                    if (Slot.MouseDown)
                    {
                        LootItemClicked(x, y, currRect, Event.current.button);
                    }
                }
              
                currRect++;
            }
        }
        
        if(GUI.Button(m_CloseBtnRect, "X", GameGUI.Singleton.CloseButtonStyle))
		{
			this.Show(false);
		}
		
		GUI.DragWindow(m_WndDragRect);
    }
	
	
    protected override void OnHide()
    {
        //m_Items.Clear();
    }

    protected override void OnShow()
    {
        CreateItemSlots();
    }

    void CreateItemSlots()
    {
        int count = m_rowWidth * m_colHeight;
        for (int i = 0; i < count; ++i)
        {
            ItemSlotGUI ItemSlot = new ItemSlotGUI();
            ItemSlot.ImgRect = new Rect(0, 0, m_imgSize, m_imgSize);
            //ItemSlot.Content.image = EmptySlotTexture;
            m_ItemSlots.Add(ItemSlot);
        }
    }

    /*public List<GameplayItem> Items
    {
        get { return m_Items; }
    }*/

    protected void LootItemClicked(int x, int y, int id, int button)
    {
        GameplayItem Item = m_ItemSlots[id].Item;
        if (Item == null)
		{
			if(button == 0)
			{
				if(GameGUI.Singleton.ItemDrag.Active)
				{
					Item = GameGUI.Singleton.ItemDrag.Drop(this);
	                if (Item == null)
	                    return;
	
	                if (!OnDropItem(GameGUI.Singleton.ItemDrag.Source, Item))
	                    return;
	
	                if (!HasItem(Item))
	                {
	                    AddItem(Item);
	                }
				}
			}
			return;
		}
		
        
        //Debug.Log(string.Format("click: {0}, {1}, {2}", Input.GetMouseButtonDown(0), Input.GetMouseButtonDown(1), Event.current.button));
        if (button == 0)
        {
            if (GameGUI.Singleton.ItemDrag.Active)
            {
                Item = GameGUI.Singleton.ItemDrag.Drop(this);
                if (Item == null)
                    return;

                if (!OnDropItem(GameGUI.Singleton.ItemDrag.Source, Item))
                    return;

                if (!HasItem(Item))
                {
                    AddItem(Item);
                }
            }
            else
            {
                if(!OnGetItem(Item))
					return;
				
                Item = RemoveItem(id);
                GameGUI.Singleton.ItemDrag.Drag(this, Item);
                GameGUI.Singleton.ItemDrag.Show(true);
            }
        }
        else if (button == 1)
        {
            //Get this item to the inventory
            if (!OnGetItem(Item))
                return;

            RemoveItem(id);
        }
		
    }

    public void SetItems(List<string> List)
    {
        if (Visible)
            return;
      
		//m_Items.Clear();
		
        for(int i = 0; i < List.Count && i < m_ItemSlots.Count; ++i)
        {
            GameplayItem Item = GameplayItemManager.Singleton.Instantiate(List[i]);
            if (Item == null)
            {
                Debug.LogError(string.Format("Item with name: {0} does not exists in item database", List[i]));
                continue;
            }
            //m_Items.Add(Item);
            m_ItemSlots[i].Item = Item;
            //m_ItemSlots[i].Content.image = Item.GetIcon();
            //m_ItemSlots[i].Content.tooltip = i.ToString();
            //m_ItemSlots[i].Content.text = Item.Name;
        }

    }

    public void SetItems(List<GameplayItem> List)
    {
        if (Visible)
            return;

        for (int i = 0; i < List.Count && i < m_ItemSlots.Count; ++i)
        {
            GameplayItem Item = List[i];
			if(Item == null)
			{
				Debug.LogError("Item is null. No such item in database");
				continue;
			}
            //Make sure this item is not a prefab, use only instances
            if (!Item.Instantiated)
            {
                if (Item.IconPath == string.Empty)
                {
                    Item = GameplayItemManager.Singleton.Instantiate(Item.Name);
                }
                else
                {
                    Item = GameplayItemManager.Singleton.Instantiate(Item);
                }
            }

            if (Item == null)
                continue;
            //m_Items.Add(Item);
            m_ItemSlots[i].Item = Item;
            //m_ItemSlots[i].Content.image = Item.GetIcon();
            //m_ItemSlots[i].Content.tooltip = i.ToString();
            //m_ItemSlots[i].Content.text = Item.Name;
        }
    }

    public void AddItem(GameplayItem Item)
    {
        if (Item == null)
            return;
        //m_Items.Add(Item);
		foreach(ItemSlotGUI Slot in m_ItemSlots)
		{
			if(Slot.Item == null)
			{
				Slot.Item = Item;
				//Slot.Content.image = Item.GetIcon();
				//Slot.Content.text = Item.Name;
				break;
			}
		}
    }
	
	public GameplayItem RemoveItem(int slotId)
	{
		if(slotId >= m_ItemSlots.Count || slotId < 0)
			return null;
		
		GameplayItem Item = m_ItemSlots[slotId].Item;//m_Items[slotId];
		m_ItemSlots[slotId].Item = null;
		return Item;
	}
	
	public bool HasItem(GameplayItem Item)
	{
		foreach(ItemSlotGUI Slot in m_ItemSlots)
		{
			if(Slot.Item == Item)
				return true;
		}
		
		return false;
	}
	
	void MouseDown()
	{
		if(!OnMouseDown())
			return;
		
		
	}
	
	protected virtual void OnItemClick(GameplayItem Item){}
	protected virtual void OnItemHover(GameplayItem Item){}
	//protected virtual void OnMouseOverMainWindow(){}
	protected virtual void OnMouseOverItemSlot(ItemSlotGUI Slot){}
	protected virtual bool OnMouseDown(){ return true; }
	protected virtual bool OnDropItem(BaseGUI Src, GameplayItem Item){ return true;}
    protected virtual bool OnGetItem(GameplayItem Item) { return true; }
}
