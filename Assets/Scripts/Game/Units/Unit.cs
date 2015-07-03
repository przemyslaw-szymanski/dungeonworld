using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : UnitController 
{
	public Texture Portrait;
	public string Name;
	public int Strength;
	public int Dexterity;
	public int Vitality;
	public int Health;
    public bool IsDead = false;

    public float DistanceToUse = 5;
    public bool UseController = true;

    private Timer m_Timer;

    private GameplayInventory m_Inventory;
    private GameplayEquipment m_Equipment;

    public GameObject AttachPoint;

    void Awake()
    {
        m_Timer = new Timer();
		m_Inventory = new GameplayInventory(this);
        m_Equipment = new GameplayEquipment(this);
    }

	// Use this for initialization
	void Start () 
	{
        if (UseController)
        {
            this.UnitControllerStart();
            if (IsDead)
                Die();

            this.DestinationPoint = this.EndPoint.transform.position;
            this.SetMovementMessage(MovementMessage.MOVE_FORWARD);
            //this.Die();
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
        if(!IsDead && UseController)
            this.UnitControllerUpdate();
	}

    public void Die()
    {
        IsDead = true;
        //this.MoveStop();
        this.SetMovementMessage(MovementMessage.STOP);
        this.StartAnimation(BasicAnimationType.DEATH, WrapMode.Once);
        //this.EnablePhysics = false;
    }

    public void Alive()
    {
        IsDead = false;
        this.EnablePhysics = true;
        this.MoveForward();
    }

    public void GetHit(float damgae)
    {
        this.StartAnimation(BasicAnimationType.HIT, WrapMode.Once);
    }

    public GameplayInventory Inventory
    {
        get { return m_Inventory; }
    }

    public GameplayEquipment Equipment
    {
        get { return m_Equipment; }
    }

    public bool Use(Item Item)
    {
        //Debug.Log(string.Format("Using item: {0} by {1}", Item.Name, Name));
        return Item.Use(this);
    }

    public Item Get(Item Item)
    {
        if (!Inventory.IsFreeSpace)
            return null;

        //Debug.Log(string.Format("Get item: {0}", Item.Name));
        Item I = Item.Get();
		I.EnableItem(false);
        //Inventory.PutItem(I);
        return I;
    }

    public void Interact(InteractObject Obj)
    {
        Gameplay.ProcessInteraction(this, Obj);
    }
}
