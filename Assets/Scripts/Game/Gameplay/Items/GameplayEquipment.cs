using UnityEngine;
using System.Collections.Generic;

public class GameplayEquipmentSlot
{
    public GameplayItem Item;
    public bool LockedByOtherSlot = false;
    public Gameplay.EquipmentSlotType Locker;
}

public class GameplayEquipment 
{
    Unit m_Unit;

    //public GameplayItem[] Items = new GameplayItem[(int)Gameplay.EquipmentSlotType._COUNT];
    public GameplayEquipmentSlot[] Slots = new GameplayEquipmentSlot[(int)Gameplay.EquipmentSlotType._COUNT];

    public GameplayEquipment(Unit Owner)
    {
        m_Unit = Owner;

        for (int i = 0; i < Slots.Length; ++i)
        {
            Slots[i] = new GameplayEquipmentSlot();
        }
    }

    public bool CanSetItem(Gameplay.EquipmentSlotType Slot, GameplayItem Item)
    {
        //Check if this slot is locked by other item
        if (Slot < Gameplay.EquipmentSlotType._COUNT)
        {
            if (Slots[(int)Slot].LockedByOtherSlot)
            {
                return false;
            }
        }

        //Check generics
        if (Item.Slot == Gameplay.EquipmentSlotType.HAND || Item.Slot == Gameplay.EquipmentSlotType.LEFT_RIGHT_HAND)
        {
            if (Slot == Gameplay.EquipmentSlotType.LEFT_HAND)
                return true;
            if (Slot == Gameplay.EquipmentSlotType.RIGHT_HAND)
                return true;
        }
        else if (Item.Slot == Gameplay.EquipmentSlotType.FINGER || Item.Slot == Gameplay.EquipmentSlotType.LEFT_RIGHT_FINGER)
        {
            if (Slot == Gameplay.EquipmentSlotType.LEFT_FINGER)
                return true;
            if (Slot == Gameplay.EquipmentSlotType.RIGHT_FINGER)
                return true;
        }

        return Slot == Item.Slot;
    }
	
	public List<Gameplay.EquipmentSlotType> CheckItemSlots(Gameplay.EquipmentSlotType DstSlot, GameplayItem Item)
	{
		List<Gameplay.EquipmentSlotType> List = new List<Gameplay.EquipmentSlotType>();
		//Check generics
        if (Item.Slot == Gameplay.EquipmentSlotType.LEFT_RIGHT_HAND)
        {
            List.Add(Gameplay.EquipmentSlotType.RIGHT_HAND);
			List.Add(Gameplay.EquipmentSlotType.LEFT_HAND);
        }
        else if (Item.Slot == Gameplay.EquipmentSlotType.LEFT_RIGHT_FINGER)
        {
            List.Add(Gameplay.EquipmentSlotType.LEFT_FINGER);
			List.Add(Gameplay.EquipmentSlotType.RIGHT_FINGER);
        }
		/*else if(Item.Slot == Gameplay.EquipmentSlotType.HAND)
		{
			if((DstSlot == Gameplay.EquipmentSlotType.LEFT_HAND || DstSlot == Gameplay.EquipmentSlotType.RIGHT_HAND) &&
				!Slots[(int)DstSlot].LockedByOtherSlot)
			{
				List.Add(DstSlot);
			}
		}
		else if(Item.Slot == Gameplay.EquipmentSlotType.FINGER)
		{
			if((DstSlot == Gameplay.EquipmentSlotType.LEFT_FINGER || DstSlot == Gameplay.EquipmentSlotType.RIGHT_FINGER) &&
				!Slots[(int)DstSlot].LockedByOtherSlot)
			{
				List.Add(DstSlot);
			}
		}
		else if(!Slots[(int)DstSlot].LockedByOtherSlot)
		{
			List.Add(DstSlot);
		}*/
		
		return List;
	}
	
	public bool IsSlotLocked(Gameplay.EquipmentSlotType Slot)
	{
		return Slots[(int)Slot].LockedByOtherSlot;
	}

    public List<Gameplay.EquipmentSlotType> SetItem(Gameplay.EquipmentSlotType Slot, GameplayItem Item)
    {
		List<Gameplay.EquipmentSlotType> List = CheckItemSlots(Slot, Item);
      	return SetItem (Slot, Item, List);
    }
	
	//Returns list of slots to remove from equipment
	public List<Gameplay.EquipmentSlotType> SetItem(Gameplay.EquipmentSlotType Slot, GameplayItem Item, List<Gameplay.EquipmentSlotType> SlotsToLock)
    {
      	if(SlotsToLock == null)
			return null; //no slots
		if(!CanSetItem(Slot, Item))
			return null;
		
		//If this item needs other items to remove
		List<Gameplay.EquipmentSlotType> RemSlots = new List<Gameplay.EquipmentSlotType>();
		foreach(Gameplay.EquipmentSlotType CurrSlot in SlotsToLock)
		{
			if(Slots[(int)CurrSlot].Item != null)
			{
				RemSlots.Add(CurrSlot);
			}
		}
		
		//Check if unit's inventory has room for removed items
		if(m_Unit.Inventory.FreeSlotCount < RemSlots.Count)
		{
			//No room for items from equipment
			RemSlots.Clear();
			return null;
		}
		
        Slots[(int)Slot].Item = Item;
        Slots[(int)Slot].LockedByOtherSlot = false;

        return RemSlots;
    }
	
	public List<Gameplay.EquipmentSlotType> RemoveItem(Gameplay.EquipmentSlotType Slot)
	{
		if(Slots[(int)Slot].Item == null || Slots[(int)Slot].LockedByOtherSlot)
			return null;
		
		List<Gameplay.EquipmentSlotType> List = new List<Gameplay.EquipmentSlotType>();
		
		for(int i = 0; i < Slots.Length; ++i)
		{
			if(Slots[i].LockedByOtherSlot && Slots[i].Locker == Slot)
			{
				List.Add((Gameplay.EquipmentSlotType)i);
			}
		}
		
		Slots[(int)Slot].Item = null;
		
		return List;
	}

    public void LockSlot(Gameplay.EquipmentSlotType SrcSlot, Gameplay.EquipmentSlotType Locker)
    {
        if (Slots[(int)SrcSlot].LockedByOtherSlot)
            return;

        Slots[(int)SrcSlot].LockedByOtherSlot = true;
        Slots[(int)SrcSlot].Locker = Locker;
    }

    public GameplayItem GetItem(Gameplay.EquipmentSlotType Slot)
    {
        return Slots[(int)Slot].Item;
    }
}
