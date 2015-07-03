using UnityEngine;
using System.Collections;

public class ItemDragGUI : BaseGUI
{
	Rect m_Rect = new Rect();
	GameplayItem m_Item;
	Texture m_Tex;
	bool m_active = false;
	BaseGUI m_DstGUI = null; //gui which got dropped item
	BaseGUI m_SrcGUI = null; //gui which dragged item
	
	public void Move(Vector2 vecPos)
	{
		m_Rect.x = vecPos.x;
		m_Rect.y = vecPos.y;
	}
	
	protected override void OnStart ()
	{
		
	}
	
	protected override void OnUpdate ()
	{
		
	}
	
	protected override void OnHide ()
	{
		m_Tex = null;
		m_Item = null;
	}
	
	protected override void OnResize ()
	{
		
	}
	
	protected override void OnDraw ()
	{
		if(!CanDraw)
			return;
		GUI.depth = 0;
		Move(Event.current.mousePosition);
		GUI.SetNextControlName("Drag");
		GUI.DrawTexture(m_Rect, m_Tex);
		//GUI.FocusControl("Drag");
	}
	
	public void Drag(BaseGUI DragGUI, GameplayItem Item)
	{
		if(Item == null)
			return;
		
		m_Item = Item;
		m_Tex = Item.GetIcon();
		
		m_Rect.width = m_Tex.width;
		m_Rect.height = m_Tex.height;
		m_active = true;
		m_SrcGUI = DragGUI;
	}
	
	public GameplayItem Drop(BaseGUI DstGUI)
	{
		m_Tex = null;
		GameplayItem Item = m_Item;
		m_Item = null;
		m_active = false;
		m_DstGUI = DstGUI;
		return Item;
	}
	
	public BaseGUI Receiver
	{
		get { return m_DstGUI; }
	}
	
	public BaseGUI Source
	{
		get { return m_SrcGUI; }
	}
	
	public bool CanDraw
	{
		get { return m_Tex != null && m_Item != null; }
	}
	
	public bool Active
	{
		get { return m_active; }
	}
	
	public GameplayItem Item
	{
		get { return m_Item; }
	}
	
	public Rect DrawRect
	{
		get { return m_Rect; }
	}
}
