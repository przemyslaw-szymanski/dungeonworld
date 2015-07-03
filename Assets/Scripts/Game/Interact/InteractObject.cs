using UnityEngine;
using System.Collections.Generic;

public class InteractObject : MonoBehaviour
{
	public bool IsContainer = false;
	//public string[] ContainedItems = new string[0];
	//public GameplayItem[] ContainedItems = new GameplayItem[0];
    //public List<string> ContainedItems = new List<string>();
    public List<GameplayItem> ContainedItems = new List<GameplayItem>();
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    public GameplayItem GetItem(int id)
    {
        GameplayItem Item = ContainedItems[id];
        if (!Item.Instantiated)
        {
            Item = GameplayItemManager.Singleton.Instantiate(Item.Name);
        }
        return Item;
    }

    public void RemoveItem(GameplayItem Item)
    {
        ContainedItems.Remove(Item);
    }
	
	public void AddItem(GameplayItem Item)
	{
		if(ContainedItems.Contains(Item))
			return;
		ContainedItems.Add(Item);
	}
	
	public void Interact(Unit Unit)
	{
		if(!OnInteract(Unit))
			return;
		
		//Instantiate items on interact
        for (int i = 0; i < ContainedItems.Count; ++i)
        {
            if (!ContainedItems[i].Instantiated)
            {
                ContainedItems[i] = GameplayItemManager.Singleton.Instantiate(ContainedItems[i].Name);
            }
        }
	}

    protected virtual void OnAwake() { }
	protected virtual void OnStart(){}
	protected virtual void OnUpdate(){}
	protected virtual void OnEnable(){}
	protected virtual void OnDisable(){}
	protected virtual bool OnInteract(Unit Unit){ return true;}
}
