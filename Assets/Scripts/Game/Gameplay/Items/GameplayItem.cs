using UnityEngine;
using System.Collections.Generic;


[System.Serializable()]
public class GameplayItem
{
    public string Name = "Unknown";
    public float Weight = 0;
	
    public Gameplay.ItemType Type = Gameplay.ItemType.NONE; //type of the item
    public Gameplay.ItemSubType SubType = Gameplay.ItemSubType.NONE; //sub-type of the item
    public Gameplay.EquipmentSlotType Slot = Gameplay.EquipmentSlotType.NONE; //type of the slot in the equipment panel

	public bool Usable = false; //can be used from inventory (potion), from hand (magic wand), from environment (door button), etc
	public bool Dragable = false; //can be get from the ground (sword)
	public bool Inventory = true; //if this item can be put in the inventory
	
	public string PrefabPath = "";
	public string IconPath = "";
	public string ScriptPath = "";
	
	protected bool m_inUse; //is in use already (torch)
	protected Item m_UnityItem;
    protected Gameplay.EquipmentSlotType m_itemSlot = Gameplay.EquipmentSlotType.NONE;
	protected int m_id = 0;
	protected bool m_instantiated = false;

    protected List<GameplayItemComponent> m_Components = new List<GameplayItemComponent>();
	
	public GameObject Prefab;
	public Texture2D Icon;
	
	public GameplayItem()
	{
	}
	
	public GameplayItem(GameplayItem Item)
	{	
		CopyFrom(Item);
	}
	
	public void CopyFrom(GameplayItem Other)
	{
		//BaseAttributes = new ItemAttributes(Other.BaseAttributes);
		//WeaponAttributes = new WeaponAttributes(Other.WeaponAttributes);
		//ArmorAttributes = new ArmorAttributes(Other.ArmorAttributes);
        if (this.Equals(Other))
            return;

        Name = Other.Name;
        Weight = Other.Weight;
		Type = Other.Type;
		SubType = Other.SubType;
        Slot = Other.Slot;
		Usable = Other.Usable;
		Dragable = Other.Dragable;
		Inventory = Other.Inventory;
		PrefabPath = Other.PrefabPath;
		IconPath = Other.IconPath;
		ScriptPath = Other.ScriptPath;
		m_UnityItem = Other.m_UnityItem;
		m_itemSlot = Other.m_itemSlot;
		Prefab = Other.Prefab;
		Icon = Other.Icon;
		
		foreach(GameplayItemComponent Cmp in Other.Components)
		{
			AddComponent(Cmp);
		}
	}
	
	public GameplayItem Instantiate()
	{
		GameplayItem Item = new GameplayItem(this);
		Item.m_instantiated = true;
		return Item;
	}

    public bool Instantiated
    {
        get { return m_instantiated; }
    }

    public Texture2D GetIcon()
    {
        if (Icon != null)
            return Icon;
        Icon = ResourceManager.Singleton.LoadIcon(IconPath);
        return Icon;
    }

    public GameObject GetPrefab()
    {
        if (Prefab != null)
            return Prefab;
        Prefab = ResourceManager.Singleton.LoadPrefab(PrefabPath);
        return Prefab;
    }

	public Item UnityItem
	{
		get { return m_UnityItem; }
		set { SetUnityItem(value); }
	}
	
	public void SetUnityItem(Item UnityItem)
	{
		if(m_UnityItem == UnityItem)
			return;
		m_UnityItem = UnityItem;
		if(m_UnityItem == null)
			return;
		
		m_UnityItem.GameplayItem = this;
	}

    public int ComponentCount
    {
        get { return m_Components.Count; }
    }
	
	public int ID
	{
		get { return m_id; }
	}
	
	public void SetID(int id)
	{
		if(m_id > 0)
			return;
		m_id = id;
	}

    public void _Update()
    {
        foreach (GameplayItemComponent Comp in m_Components)
        {
            Comp._Update();
        }
    }

    public bool Equip(Unit Unit, Gameplay.EquipmentSlotType Slot)
    {
        if (!CanEquip(Unit, Slot))
            return false;

        foreach (GameplayItemComponent Cmp in Components)
        {
            if (!Cmp.OnEquip(Unit, Slot))
                return false;
        }
        return true;
    }

    public bool CanEquip(Unit Unit, Gameplay.EquipmentSlotType Slot)
    {
        foreach (GameplayItemComponent Cmp in Components)
        {
            if (!Cmp.CanEquip(Unit, Slot))
                return false;
        }

        return true;
    }

    public T AddComponent<T>() where T : GameplayItemComponent, new()
    {
        //T Comp = System.Activator.CreateInstance(typeof(T), this) as T;
        T Comp = new T();
        Comp._Create(this);
        return Comp;
    }

    public void AddComponent(GameplayItemComponent Component)
    {
        m_Components.Add(Component);
    }

    public void RemoveComponent<T>() where T : GameplayItemComponent
    {
        T Comp = GetComponent<T>();
        m_Components.Remove(Comp);
        Comp = null;
    }

    public T GetComponent<T>() where T : GameplayItemComponent
    {
        //Find component with type T
        foreach (GameplayItemComponent C in m_Components)
        {
            if (C.GetType() == typeof(T))
            {
                return C as T;
            }
        }

        return null;
    }

    public List<GameplayItemComponent> Components
    {
        get { return m_Components; }
    }
	
	public override string ToString ()
	{
        return ApplicationManager.ItemToString(this);
	}
}
