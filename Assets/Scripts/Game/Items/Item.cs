using UnityEngine;
using System.Collections.Generic;


//Item can be got by a unit (eg. sword)
//Item can has one or more sub-items (child_items) which can be got by a unit (eg. chest)
public class Item : MonoBehaviour 
{
	protected GameplayItem m_GameplayItem;
    
	public int InventorySize = 0; //how many items it can contains
    public bool ItemEnabled = true;
	public bool ItemPhysicsEnabled = true;
    public string FailedUseReason = ""; //If item use method returns false this member should be set

    protected bool m_inUse = false; //if this item is already in use
    protected bool m_isGetable = false; //if this item can be get to the unit inventory (eg. sword)
	//protected bool m_isGetable = false; //can it be put in the unit inventory
	protected bool m_isContainer = false; //can it holds another items (eg. it is a chest)
	
	protected int m_currInventorySize = 0; //current number of items in this item
	
	protected bool m_itemStarted = false; //Sets only once after Start() method

    //public GameObject ChildItemPrefab; //If this item contains another item which unit can get to the inventory
    public GameObject[] ChildItemPrefabs; //items produced by this item
	
    //Deferred actions. If item was instantiated and some methods was called (eg. EnableItem) it goes to deferred and runs on Update
	public delegate void DeferredAction();
	public List<DeferredAction> m_DeferredActions = new List<DeferredAction>();
	
    protected Unit m_Owner;
	
	void Awake()
	{
		ItemAwake();
        OnItemAwake();
	}
	
	void Start()
	{
		ItemStart();
        OnItemStart();
	}
	
	void Update()
	{
		ItemUpdate();
        OnItemUpdate();
	}

    void OnDestroy()
    {
        OnItemDestroy();
        ItemDestroy();
    }
	
	void ItemAwake()
	{
	}

    void ItemDestroy()
    {
    }
	
	public void SetGameplayItem(GameplayItem Item)
	{
		m_GameplayItem = Item;
		UpdateForGameplay();
	}
	
	public void UpdateForGameplay()
	{
		//If item visible
	}
	
	public GameplayItem GameplayItem
	{
		get { return m_GameplayItem; }
		set { SetGameplayItem(value); }
	}
    
	// Use this for initialization
	void ItemStart () 
    {
        //if (this.rigidbody != null)
        //    GameplayItem.Weight = this.rigidbody.mass;
		
		if(ChildItemPrefabs == null)
			ChildItemPrefabs = new GameObject[0];
		
		//If item has child items (eg. chest) it can't be get, but its children can be get
        if (ChildItemPrefabs.Length > 0)
        {
            m_isContainer = true;
			InventorySize = ChildItemPrefabs.Length;
			m_currInventorySize = InventorySize;
        }
		
		EnableItem(ItemEnabled);
		
		m_itemStarted = true;
	}

    protected virtual void OnItemAwake() { }
    protected virtual void OnItemStart() { }
    protected virtual void OnItemUpdate() { }
    protected virtual void OnItemDestroy() { }
	
	// Update is called once per frame
	protected void ItemUpdate() 
    {
		foreach(DeferredAction Action in m_DeferredActions)
		{
			Action();
		}
		
		m_DeferredActions.Clear();
	}

    public bool Use(Unit Unit)
    {
        //Debug.Log(string.Format("Item.Use: {0}", Unit.name));
        m_inUse = OnUse(Unit);
        return m_inUse;
    }

    protected virtual bool OnUse(Unit Unit)
    {
        return true;
    }

    public bool InUse
    {
        get { return m_inUse; }
    }

    public Item GetContainedItem(int itemId)
    {
        if ((!ContainsItems && !IsGetable) || itemId >= ChildItemPrefabs.Length)
        {
            Debug.LogError("Item has nothing to get");
            return null;
        }
        
		m_currInventorySize--;
		GameObject Obj = CreateItem(ChildItemPrefabs[itemId]);
		return Obj.GetComponent<Item>();
    }
	
	public Item GetContainedItem(string itemName, bool remove)
	{
		for(int i = ChildItemPrefabs.Length; i --> 0;)
		{
			if(ChildItemPrefabs[i].name == itemName)
			{
				return GetContainedItem(i);
			}
		}
		
		//Debug.LogError(string.Format("Item: {0} has no item with name: {1}", this.Name, itemName));
		return null;
	}
	
	//Get this item
	public Item Get()
	{
		if(IsGetable)
		{
			return this;
		}
		
		if(ContainsItems)
		{
			//Get first item
			return GetContainedItem(0);
		}
		
		//Debug.LogError(string.Format("Item.Get(): {0}: no item to get", this.Name));
		return null;
	}
	
	public bool IsGetable
	{
		get { return m_isGetable; }
	}
	
	public bool ItemGameObjectStarted
	{
		get { return m_itemStarted; }
	}
	
    public void PutItem(Item Item)
    {
        if (ContainsItems)
            return;
        //ChildItem = Item.gameObject;
        m_isContainer = true;
    }

    public bool ContainsItems
    {
        get { return m_isContainer && m_currInventorySize > 0; }
    }

    public bool Drop(Unit Owner)
    {
        return false;
    }

    public bool ChangeOwner(Unit NewOwner)
    {
        if (m_Owner == NewOwner)
            return true;
        m_Owner = NewOwner;
        return OnChangeOwner(NewOwner);
    }
	
	public Item[] GetContainedItems()
	{
		Item[] Items = new Item[ChildItemPrefabs.Length];
		m_currInventorySize = 0;
		
		for(int i = Items.Length; i-->0;)
		{
			Items[i] = CreateItem(ChildItemPrefabs[i]).GetComponent<Item>();	
		}
		
		return Items;
	}
	
	protected GameObject CreateItem(Object Prefab)
	{
		GameObject Obj = Instantiate(Prefab) as GameObject;
		return Obj;
	}

    protected virtual bool OnChangeOwner(Unit NewOwner)
    {
        return true;
    }

    public GameObject FindFXGameObject()
    {
        foreach (Transform T in this.transform)
        {
            if (T.tag == "FX")
                return T.gameObject;
        }

        return null;
    }

    public void EnableItem(bool enable)
    {
		if(!ItemGameObjectStarted)
		{
			if(enable)
			{
				m_DeferredActions.Add(new DeferredAction(EnableItem));
			}
			else
			{
				m_DeferredActions.Add(new DeferredAction(DisableItem));
			}
			
			return;
		}
		
        if(OnItemEnable(enable))
			ItemEnabled = enable;
    }
	
	public void EnableItem()
	{
		EnableItem(true);
	}
	
	public void DisableItem()
	{
		EnableItem(false);
	}

    protected virtual bool OnItemEnable(bool enable)
    {
        return true;
    }
	
	public bool PhysicsEnabled
	{
		get { return ItemPhysicsEnabled; }
		set { EnablePhysics(value); }
	}
	
	public void EnablePhysics(bool enable)
	{
		ItemPhysicsEnabled = enable;
		if(this.gameObject.rigidbody != null)
		{
			this.gameObject.rigidbody.isKinematic = !enable;
		}
		
		if(this.gameObject.collider != null)
		{
			this.gameObject.collider.enabled = enable;
		}
	}
}
