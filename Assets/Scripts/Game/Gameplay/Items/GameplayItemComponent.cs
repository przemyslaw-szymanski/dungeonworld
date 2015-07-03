using UnityEngine;
using System.Collections;

[System.Serializable()]
public class GameplayItemComponent
{
    protected GameplayItem m_Item;

    public GameplayItemComponent()
    {
    }
	
	public virtual void CopyFrom(GameplayItemComponent Other)
	{
		m_Item = Other.Item;
	}

    public void _Create(GameplayItem Item)
    {
        m_Item = Item;
        OnCreate();
    }

	// Use this for initialization
	public void _Start() 
    {
        OnStart();
	}
	
	// Update is called once per frame
	public void _Update() 
    {
        OnUpdate();
	}

    public void _ProcessAction(Gameplay.UnitAction Action)
    {
        OnAction(Action);
    }

    protected virtual void OnCreate() { }
    protected virtual void OnStart() {}
    protected virtual void OnUpdate() {}
    protected virtual void OnAction(Gameplay.UnitAction Action) { }
    public virtual bool CanEquip(Unit Unit, Gameplay.EquipmentSlotType Slot) { return true; }
    public virtual bool OnEquip(Unit Unit, Gameplay.EquipmentSlotType Slot) { return true; }

    public GameplayItem Item
    {
        get { return m_Item; }
    }
	
	public override string ToString ()
	{
		return string.Format("{0}", GetType().Name);
	}
	

    public virtual string[] GetAdditionalInfoTexts()
    {
        return new string[0];
    }

    public virtual ItemDescLineGUI[] BuildGUIDescription()
    {
        return new ItemDescLineGUI[0];
    }
	
	public virtual string Serialize(System.IO.BinaryWriter Writer)
	{
		return string.Empty;
	}
	
	public virtual string Deserialize(System.IO.BinaryReader Reader)
	{
		return string.Empty;
	}
}

[System.Serializable]
public class GameplayDamageComponent : GameplayItemComponent
{
	public short MinDmg = 1;
    public short MaxDmg = 2;
    public Gameplay.DamageType DamageType = Gameplay.DamageType.NONE;
	
	public GameplayDamageComponent(){}
	
	public override void CopyFrom(GameplayItemComponent Other)
	{
		base.CopyFrom(Other);
		GameplayWeaponComponent Cmp = (GameplayWeaponComponent)Other;
		MinDmg = Cmp.MinDmg;
		MaxDmg = Cmp.MaxDmg;
		DamageType = Cmp.DamageType;
	}
	
	protected Color GetDamageTypeColor()
    {
        switch (DamageType)
        {
            case Gameplay.DamageType.ELECTRIC: return Color.blue;
            case Gameplay.DamageType.FIRE: return Color.yellow;
            case Gameplay.DamageType.ICE: return Color.cyan;
            case Gameplay.DamageType.MAGIC: return Color.magenta;
            case Gameplay.DamageType.NONE: return Color.grey;
            case Gameplay.DamageType.PHYSICAL: return Color.white;
            case Gameplay.DamageType.POISON: return Color.green;
        }

        return Color.grey;
    }
	
	public override string Serialize(System.IO.BinaryWriter Writer)
	{
		Writer.Write(MinDmg);
		Writer.Write(MaxDmg);
		Writer.Write((byte)DamageType);
		return string.Empty;
	}
	
	public override string Deserialize(System.IO.BinaryReader Reader)
	{
		MinDmg = Reader.ReadInt16();
		MaxDmg = Reader.ReadInt16();
		DamageType = (Gameplay.DamageType)Reader.ReadByte();
		return string.Empty;
	}
	
	public override ItemDescLineGUI[] BuildGUIDescription()
    {
        int count = 1;
        ItemDescLineGUI[] Lines = new ItemDescLineGUI[count];
        Lines[0] = new ItemDescLineGUI(2);

        Color Color = GetDamageTypeColor();
        Lines[0].Columns[0].Style.normal.textColor = Lines[0].Columns[1].Style.normal.textColor = Color;
        Lines[0].Columns[0].Style.alignment = TextAnchor.MiddleLeft;
        Lines[0].Columns[0].text = string.Format("Damage: {0} - {1}", MinDmg, MaxDmg);
        Lines[0].Columns[1].Style.alignment = TextAnchor.MiddleRight;
        Lines[0].Columns[1].text = string.Format("{0}", GameText.Singleton.GetDamageType(DamageType));

        return Lines;
    }  
	
}

[System.Serializable()]
public class GameplayWeaponComponent : GameplayDamageComponent
{
    public float Speed = 2;
	public Gameplay.WeaponUseType UseType = Gameplay.WeaponUseType.NONE;

    public GameplayWeaponComponent() { }
	
	public override void CopyFrom(GameplayItemComponent Other)
	{
		base.CopyFrom(Other);
		GameplayWeaponComponent Cmp = (GameplayWeaponComponent)Other;
		MinDmg = Cmp.MinDmg;
		MaxDmg = Cmp.MaxDmg;
		Speed = Cmp.Speed;
		DamageType = Cmp.DamageType;
		UseType = Cmp.UseType;
	}

    public virtual short CalculateDamage()
    {
        if (DamageType == Gameplay.DamageType.NONE)
            return 0;
        return (short)Random.Range(MinDmg, MaxDmg);
    }
	
	public override string ToString ()
	{
		string str = base.ToString();
		str += string.Format("\n Dmg: {0}-{1}\n Speed: {2}\n DmgType: {3}", MinDmg, MaxDmg, Speed, DamageType);
		return str;
	}

    public override ItemDescLineGUI[] BuildGUIDescription()
    {
        int count = 2;
        ItemDescLineGUI[] Lines = new ItemDescLineGUI[count];
        Lines[0] = new ItemDescLineGUI(2);

        Color Color = GetDamageTypeColor();
        Lines[0].Columns[0].Style.normal.textColor = Lines[0].Columns[1].Style.normal.textColor = Color;
        Lines[0].Columns[0].Style.alignment = TextAnchor.MiddleLeft;
        Lines[0].Columns[0].text = string.Format("Damage: {0} - {1}", MinDmg, MaxDmg);
        Lines[0].Columns[1].Style.alignment = TextAnchor.MiddleRight;
        Lines[0].Columns[1].text = string.Format("{0}", GameText.Singleton.GetDamageType(DamageType));

        Lines[1] = new ItemDescLineGUI(1);
        Lines[1].Columns[0].Style.alignment = TextAnchor.MiddleRight;
        Lines[1].Columns[0].text = string.Format("Speed: {0}", Speed);
        Lines[1].Columns[0].Style.normal.textColor = Color;
		
        return Lines;
    }  
	
	public override string Serialize(System.IO.BinaryWriter Writer)
	{
		Writer.Write(MinDmg);
		Writer.Write(MaxDmg);
		Writer.Write(Speed);
		Writer.Write((byte)DamageType);
		Writer.Write((byte)UseType);
		return string.Empty;
	}
	
	public override string Deserialize(System.IO.BinaryReader Reader)
	{
		MinDmg = Reader.ReadInt16();
		MaxDmg = Reader.ReadInt16();
		Speed = Reader.ReadSingle();
		DamageType = (Gameplay.DamageType)Reader.ReadByte();
		UseType = (Gameplay.WeaponUseType)Reader.ReadByte();
		return string.Empty;
	}
	
}

[System.Serializable()]
public class GameplayArmorComponent : GameplayItemComponent
{
    public short ArmorValue = 1;

    public virtual short CalculateArmorValue()
    {
        return ArmorValue;
    }
	
	public override void CopyFrom(GameplayItemComponent Other)
	{
		base.CopyFrom(Other);
		GameplayArmorComponent Cmp = (GameplayArmorComponent)Other;
		ArmorValue = Cmp.ArmorValue;
	}

    public Color GetArmorTypeColor()
    {
        switch (this.Item.SubType)
        {
            case Gameplay.ItemSubType.CLOTH: return Color.white;
            case Gameplay.ItemSubType.LEATHER: return Color.yellow;
            case Gameplay.ItemSubType.MAIL: return Color.blue;
            case Gameplay.ItemSubType.PLATE: return Color.gray;
        }

        return Color.white;
    }

    public override ItemDescLineGUI[] BuildGUIDescription()
    {
        int count = 1;
        ItemDescLineGUI[] Lines = new ItemDescLineGUI[count];
        Lines[0] = new ItemDescLineGUI(1);

        Color Color = GetArmorTypeColor();
        Lines[0].Columns[0].Style.normal.textColor = Lines[0].Columns[1].Style.normal.textColor = Color;
        Lines[0].Columns[0].Style.alignment = TextAnchor.MiddleLeft;
        Lines[0].Columns[0].text = string.Format("Armor: {0}", ArmorValue);
        //Lines[0].Columns[1].Style.alignment = TextAnchor.MiddleRight;
        //Lines[0].Columns[1].text = string.Format("{0}", GameText.Singleton.GetDamageType(DamageType));

        return Lines;
    }

    public override string Serialize(System.IO.BinaryWriter Writer)
    {
        Writer.Write(ArmorValue);
        return string.Empty;
    }

    public override string Deserialize(System.IO.BinaryReader Reader)
    {
        ArmorValue = Reader.ReadInt16();
        return string.Empty;
    }
}