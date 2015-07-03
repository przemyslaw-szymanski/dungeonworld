using UnityEngine;
using System.Collections;

public class ItemTooltipGUI : BaseGUI 
{	
	Rect m_ParentRect;
	GameplayItem m_Item;
	
	public void Show(GameplayItem Item, float posX, float posY)
	{
		//Add offset to enable click content below this window (eg. item slot)
		MainWndRect.x = posX + 1;
		MainWndRect.y = posY + 1;
		m_Item = Item;
		
		base.Show(true);
	}
	
	protected override void OnStart ()
	{
		
	}
	
	
	protected override void OnShow ()
	{
		
	}
	
	protected override void OnResize ()
	{
		MainWndRect.width = 200;
		MainWndRect.height = 200;
		if(MainWndRect.x + MainWndRect.width > Screen.width)
		{
			MainWndRect.x -= MainWndRect.width;
		}
		
		if(MainWndRect.y + MainWndRect.height > Screen.height)
		{
			MainWndRect.y -= MainWndRect.height;
		}
	}
	
	protected override void OnDraw ()
	{
		if(m_Item == null)
			return;
		
		MainWndRect = GUI.Window(MainWndID, MainWndRect, WndFunc, m_Item.Name);
	}
	
	void WndFunc(int wndId)
    {
        if (m_Item == null || m_Item.Icon == null)
            return;

		GUI.BringWindowToFront(wndId);
		GUIStyle Style = GameGUI.Singleton.Styles.ItemTooltipLabel;
        int fontSize = Style.fontSize;
        int currHeight = 20;
        int currWidth = (int)MainWndRect.width - 5;
		//Icon
		GUI.Label(new Rect(5, currHeight, 40, 40),  m_Item.Icon);
        currHeight += 5;
		//Item type
		GUI.Label(new Rect(45, currHeight, currWidth, Style.fontSize), GameText.Singleton.GetItemType(m_Item.Type), Style);
        Style.fontSize = (int)(fontSize * 0.8f);
        currHeight += Style.fontSize + 5;
		//Item sub type
        GUI.Label(new Rect(45, currHeight, currWidth, Style.fontSize), GameText.Singleton.GetItemSubType(m_Item.SubType), Style);
        Style.fontSize = fontSize;
		
        foreach (GameplayItemComponent Cmp in m_Item.Components)
        {
            ItemDescLineGUI[] Lines = Cmp.BuildGUIDescription();
			currHeight += 5; //make more space betwheen components
			
            foreach (ItemDescLineGUI Line in Lines)
            {
                currHeight += fontSize;
                foreach (ItemDescLineGUI.Column Col in Line.Columns)
                {
                    fontSize = Col.Style.fontSize;
                    currWidth = ((Col.width == 0)? (int)MainWndRect.width - 20 : Col.width);
                    GUI.Label(new Rect(10, currHeight, currWidth, Col.Style.fontSize), Col.text, Col.Style);
                }
            }
        }
    }
}
