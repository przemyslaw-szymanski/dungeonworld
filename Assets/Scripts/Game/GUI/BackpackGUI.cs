using UnityEngine;
using System.Collections.Generic;
 
public class BackpackGUI : BaseItemWindowGUI 
{
	Unit m_Unit;
	
	protected override void OnStart ()
	{
		base.WindowTitle = "Backpack";
		base.OnStart();
	}
	
	public void Show(Unit Unit)
	{
		if(Unit == null)
			return;
		m_Unit = Unit;
		base.Show(true);
	}
	
	protected override void OnShow ()
	{
		if(m_Unit == null)
		{
			m_Unit = base.Player.PartyUnits[0];
		}
		base.SetItems(m_Unit.Inventory.Items);
	}
}
