using UnityEngine;
using System.Collections.Generic;


public class GameplayInventory
{
    private Unit m_Owner;

    public int MaxSize = 10;
    protected List<GameplayItem> m_Items = new List<GameplayItem>();

    public GameplayInventory(Unit Owner)
    {
        m_Owner = Owner;
    }

    public Unit Owner
    {
        get { return m_Owner; }
    }

    public bool AddItem(GameplayItem Item)
    {
        if (m_Items.Count + 1 >= MaxSize)
            return false; //no free space
		
        m_Items.Add(Item);
        return true;
    }
	
	public GameplayItem GetItem(int itemId)
	{
		if(m_Items.Count == 0)
			return null;
		GameplayItem I = m_Items[itemId];
		return I;
	}
	
	public GameplayItem RemoveItem(int itemId)
	{
		GameplayItem I = GetItem(itemId);
		m_Items.RemoveAt(itemId);
		return I;
	}
	
	public GameplayItem GetItem(string itemName)
	{
		for(int i = m_Items.Count; i --> 0;)
		{
			if(m_Items[i].Name == itemName)
				return GetItem(i);
		}
		
		return null;
	}
	
	public GameplayItem RemoveItem(string itemName)
	{
		for(int i = m_Items.Count; i --> 0;)
		{
			if(m_Items[i].Name == itemName)
				return RemoveItem(i);
		}
		
		return null;
	}
	
	public GameplayItem[] GetAllItems()
	{
		GameplayItem[] Items = m_Items.ToArray();
		return Items;
	}
	
	public GameplayItem[] RemoveAllItems()
	{
		GameplayItem[] Items = GetAllItems();
		foreach(GameplayItem I in Items)
		{
			//I.EnableItem(true);
		}
		
		m_Items.Clear();
		return Items;
	}
	
	public void DestroyItems()
	{
		for(int i = 0; i < m_Items.Count; ++i)
		{
			//GameObject.Destroy(m_Items[i]);
		}
	}

    public bool IsFreeSpace
    {
        get { return m_Items.Count + 1 < MaxSize; }
    }
	
	public int ItemCount
	{
		get { return m_Items.Count; }
	}
	
	public int FreeSlotCount
	{
		get { return MaxSize - m_Items.Count; }
	}

    public List<GameplayItem> Items
    {
        get { return m_Items; }
    }

    public override string ToString()
    {
        string str = string.Format("Inventory of: {0}{1}", m_Owner.name, System.Environment.NewLine);
        for (int i = 0; i < m_Items.Count; ++i)
        {
            str += string.Format("{0}. {1}", i + 1, m_Items[i]);
        }
        return str;
    }
}
