using UnityEngine;
using System.Collections.Generic;

public class EquipmentGUI : BaseGUI 
{
	public Texture[] EmptySlotTextures = new Texture[(int)Gameplay.EquipmentSlotType._COUNT];

    ItemTooltipGUI m_ItemTooltip;
	
	Rect m_CenterBoxRect = new Rect();
	
	ItemSlotGUI[] m_Slots = new ItemSlotGUI[(int)Gameplay.EquipmentSlotType._COUNT];
	ItemSlotGUI m_CurrSlot;
	ItemSlotGUI m_ClickedSlot;
	Unit m_Unit;
	
	protected override void OnAwake ()
	{
        m_ItemTooltip = GameGUI.CreateTooltip(this);
	}
	
	protected override void OnStart ()
	{	
		for(int i = 0; i < m_Slots.Length; ++i)
		{
			m_Slots[i] = new ItemSlotGUI(EmptySlotTextures[i]);
			m_Slots[i].ImgRect.width = m_Slots[i].ImgRect.height = GameGUI.Singleton.Styles.ItemSlot.fixedHeight;
		}
		
		MainWndRect.x = Screen.currentResolution.width * 0.3f;
		MainWndRect.y = Screen.currentResolution.height * 0.1f;
		
		//OnResize();
	}
	
	protected override void OnResize ()
	{
		MainWndRect.width = 5 * GameGUI.Singleton.Styles.ItemSlot.fixedWidth;
		MainWndRect.height = 5f * GameGUI.Singleton.Styles.ItemSlot.fixedHeight;
		
		for(int i = 0; i < m_Slots.Length; ++i)
		{
			m_Slots[i].ImgRect.width = m_Slots[i].ImgRect.height = GameGUI.Singleton.Styles.ItemSlot.fixedHeight;
		}
		
		Prepare();
	}
	
	protected override void OnDraw ()
	{
		MainWndRect = GUI.Window(this.MainWndID, MainWndRect, WndFunc, m_Unit.Name);

		if(m_CurrSlot != null)
		{
			m_ItemTooltip.Show(m_CurrSlot.Item, Event.current.mousePosition.x, Event.current.mousePosition.y);
			m_CurrSlot = null;
		}
		else
		{
			m_ItemTooltip.Show(false);
		}
	}
	
	
	protected override void OnShow ()
	{
		if(m_Unit == null)
			m_Unit = base.Player.CurrentPartyUnit;
		
		Prepare();
	}
	
	protected override void OnHide ()
	{
		m_Unit = null;
	}
	
	void WndFunc(int wndId)
	{
        ItemSlotGUI Slot;

		for(int i = 0; i < m_Slots.Length; ++i)
		{
            Slot = m_Slots[i];

            Slot.Draw();
			
            if(Slot.MouseOver)
			{
				m_CurrSlot = Slot;
				OnMouseOverSlot(m_CurrSlot);

                if (Slot.MouseDown)
                {
                    OnSlotClick(i, Slot);
                }
			}
		}
		
		GUI.Box(m_CenterBoxRect, base.Player.CurrentPartyUnit.Portrait);

        GUI.DragWindow(m_WndDragRect);
	}
	
	public void Show(Unit Unit)
	{
		m_Unit = Unit;
		base.Show(true);
	}
	
	void OnMouseOverSlot(ItemSlotGUI Slot)
	{
        
	}

    void OnSlotClick(int slotId, ItemSlotGUI Slot)
    {
        if (Slot == null)
            return;
		
		Gameplay.EquipmentSlotType CurrSlot = (Gameplay.EquipmentSlotType)slotId;
		
        if (Slot.Item == null)
        {
            if (Event.current.button == 0)
            {
                if (GameGUI.Singleton.ItemDrag.Active)
                {
                    /*if (!m_Unit.Equipment.CanSetItem((Gameplay.EquipmentSlotType)slotId, GameGUI.Singleton.ItemDrag.Item))
                    {
                        //Wrong slot
                        return;
                    }*/
					
					GameplayItem Item = GameGUI.Singleton.ItemDrag.Item;
					if(Item == null)
						return;
					
					List<Gameplay.EquipmentSlotType> SlotsToLock = m_Unit.Equipment.CheckItemSlots(CurrSlot, Item);
					List<Gameplay.EquipmentSlotType> SlotsToRemove = m_Unit.Equipment.SetItem(CurrSlot, Item, SlotsToLock);
					
					if(SlotsToRemove == null) //error no space in the backpack
					{
						return;
					}
					
					foreach(Gameplay.EquipmentSlotType TmpSlot in SlotsToRemove)
					{
						//Add item to backpack
						m_Unit.Inventory.AddItem(m_Slots[(int)TmpSlot].Item);
						m_Slots[(int)TmpSlot].Item = null;
					}
					
					foreach(Gameplay.EquipmentSlotType TmpSlot in SlotsToLock)
					{
						m_Slots[(int)TmpSlot].Lock(true);
					}
					
                    Item = GameGUI.Singleton.ItemDrag.Drop(this);
                    GameGUI.Singleton.ItemDrag.Show(false);
          
                    Slot.Item = Item;
                    return;
                }
            }
        }
        else
        {
            if (Event.current.button == 0)
            {
                GameGUI.Singleton.ItemDrag.Drag(this, Slot.Item);
                GameGUI.Singleton.ItemDrag.Show(true);
                Slot.Item = null;
				//Unlock all locked slot by this slot
				List<Gameplay.EquipmentSlotType> SlotsToUnlock = m_Unit.Equipment.RemoveItem(CurrSlot);
				foreach(Gameplay.EquipmentSlotType TmpSlot in SlotsToUnlock)
				{
					m_Slots[(int)TmpSlot].Lock(false);
				}
				
                return;
            }
        }
    }

	
	void Prepare()
	{
        //for(int i = m_Slots.Length; i-->0;)
        //{
        //    m_Slots[i].Content.image = EmptySlotTextures[i];
        //}
		
		float slotSpace = 0;
		float slotHeight = m_Slots[0].ImgRect.height;
		Vector2 vecLeftCol = new Vector2(5, 20/*slotHeight * 0.5f*/);
		Vector2 vecRightCol = new Vector2(MainWndRect.width - 5 - m_Slots[0].ImgRect.width, vecLeftCol.y);
		Vector2 vecMiddle = new Vector2(MainWndRect.width * 0.5f, vecLeftCol.y);
		
		//Top
		
		//Left column
		m_Slots[(int)Gameplay.EquipmentSlotType.HEAD].ImgRect.x = vecLeftCol.x;
		m_Slots[(int)Gameplay.EquipmentSlotType.HEAD].ImgRect.y = vecLeftCol.y;
		
		vecLeftCol.y += slotHeight + slotSpace;
		m_Slots[(int)Gameplay.EquipmentSlotType.NECK].ImgRect.x = vecLeftCol.x;
		m_Slots[(int)Gameplay.EquipmentSlotType.NECK].ImgRect.y = vecLeftCol.y;
		
		vecLeftCol.y += slotHeight + slotSpace;
		m_Slots[(int)Gameplay.EquipmentSlotType.CHEST].ImgRect.x = vecLeftCol.x;
		m_Slots[(int)Gameplay.EquipmentSlotType.CHEST].ImgRect.y = vecLeftCol.y;
		
		vecLeftCol.y += slotHeight + slotSpace;
		m_Slots[(int)Gameplay.EquipmentSlotType.LEFT_FINGER].ImgRect.x = vecLeftCol.x;
		m_Slots[(int)Gameplay.EquipmentSlotType.LEFT_FINGER].ImgRect.y = vecLeftCol.y;
		
		//Right column

		m_Slots[(int)Gameplay.EquipmentSlotType.HANDS].ImgRect.x = vecRightCol.x;
		m_Slots[(int)Gameplay.EquipmentSlotType.HANDS].ImgRect.y = vecRightCol.y;
		
		vecRightCol.y += slotHeight + slotSpace;
		m_Slots[(int)Gameplay.EquipmentSlotType.LEGS].ImgRect.x = vecRightCol.x;
		m_Slots[(int)Gameplay.EquipmentSlotType.LEGS].ImgRect.y = vecRightCol.y;
		
		vecRightCol.y += slotHeight + slotSpace;
		m_Slots[(int)Gameplay.EquipmentSlotType.RIGHT_FINGER].ImgRect.x = vecRightCol.x;
		m_Slots[(int)Gameplay.EquipmentSlotType.RIGHT_FINGER].ImgRect.y = vecRightCol.y;
		
		vecRightCol.y += slotHeight + slotSpace;
		m_Slots[(int)Gameplay.EquipmentSlotType.FEET].ImgRect.x = vecRightCol.x;
		m_Slots[(int)Gameplay.EquipmentSlotType.FEET].ImgRect.y = vecRightCol.y;
		
		//Bottom
		
		vecMiddle.x -= slotHeight;
		vecMiddle.y = MainWndRect.height - slotHeight - 5;
		m_Slots[(int)Gameplay.EquipmentSlotType.LEFT_HAND].ImgRect.x = vecMiddle.x;
		m_Slots[(int)Gameplay.EquipmentSlotType.LEFT_HAND].ImgRect.y = vecMiddle.y;
		
		vecMiddle.x += slotHeight;
		m_Slots[(int)Gameplay.EquipmentSlotType.RIGHT_HAND].ImgRect.x = vecMiddle.x;
		m_Slots[(int)Gameplay.EquipmentSlotType.RIGHT_HAND].ImgRect.y = vecMiddle.y;
		
		m_CenterBoxRect.x = vecLeftCol.x + slotHeight + 5;
		m_CenterBoxRect.y = 20;
		m_CenterBoxRect.width = MainWndRect.width - 2 * m_CenterBoxRect.x;
		m_CenterBoxRect.height = MainWndRect.height - m_CenterBoxRect.y - slotHeight - 10;
	}
}
