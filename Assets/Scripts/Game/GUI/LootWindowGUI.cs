using UnityEngine;
using System.Collections.Generic;

public class LootWindowGUI : BaseItemWindowGUI
{
    InteractObject m_Obj;
	Unit m_Unit;
	
	protected override void OnStart ()
	{
		base.WindowTitle = "Loot";
		base.OnStart();
	}

    public void Show(object Owner)
    {
        if (Owner.GetType() == typeof(InteractObject))
        {
			m_Obj = (InteractObject)Owner;
            ShowForInteract(m_Obj);
        }
        else if (Owner.GetType() == typeof(Unit))
        {
            m_Unit = (Unit)Owner;
            ShowForUnit(m_Unit);
        }
           
		base.Show(true);
    }

    void ShowForInteract(InteractObject Obj)
    {
        if (!Obj.IsContainer)
            return;
        base.SetItems(Obj.ContainedItems);
    }

    void ShowForUnit(Unit Unit)
    {
        base.SetItems(Unit.Inventory.Items);
    }

    protected override bool OnDropItem(BaseGUI Src, GameplayItem Item)
    {
		if(m_Obj != null)
		{
			m_Obj.AddItem(Item);
			return true;
		}
		
		if(m_Unit != null)
		{
		}
		
        return false;
    }

    protected override bool OnGetItem(GameplayItem Item)
    {
		if(m_Obj != null)
		{
            m_Obj.RemoveItem(Item);
            return true;
		}
		
		if(m_Unit != null)
		{
		}
		
        return false;
    }
}
