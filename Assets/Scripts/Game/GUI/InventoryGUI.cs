using UnityEngine;
using System.Collections;

public class InventoryGUI : BaseGUI 
{
	public enum Mode
	{
		BACKPACK = 0x00000001,
		EQUIPMENT = 0x00000002,
		STATS = 0x00000004,
		QUEST_ITEMS = 0x00000008,
		PORTRAITS = 0x00000010,
		ALL = Mode.BACKPACK | Mode.STATS | Mode.QUEST_ITEMS | Mode.PORTRAITS
	}
	
	public Mode ShowMode = Mode.ALL;
	protected Unit m_ShowUnit;


    protected override void OnAwake()
    {
        base.m_fullScreen = true;

    }

    protected override void OnDraw()
    {

    }

    protected override void  OnResize()
    {
        

    }


    void DrawMainWnd()
    {
       
    }

    void CreatePartyWnd()
    {
        
    }

    void DrawPartyWnd()
    {
       
    }

    void DrawEquipmentWnd()
    {
    
    }
	
	protected override void OnShow()
	{
		
	}
	
	public void Show(Unit Unit, InventoryGUI.Mode mode)
	{
		m_ShowUnit = Unit;
		ShowMode = mode;
		base.Show(true);
	}
}
