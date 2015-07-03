using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class Gameplay
{
    public class Party
    {
        public const int MaxSize = 5; //max member size
    }

    public enum DamageType
    {
        NONE,
        PHYSICAL,
        FIRE,
        ICE,
        ELECTRIC,
        POISON,
        MAGIC,
        _COUNT
    }

    public class Damage
    {
        
    }

    public class Armor
    {
    }

    public class ObjectTag
    {
        public const string Item = "Item";
        public const string Unit = "Unit";
        public const string Interact = "Interact";
    };

    public enum GameObjectLayer
    {
        USABLE = 8,
        DRAGGABLE,
        MOVABLE
    }

    public enum ItemType
    {
        NONE,
        WEAPON,
        ARMOR,
        POTION,
        _COUNT
    }

    public enum EquipmentSlotType
    {
		[Description("Left Hand")]
        LEFT_HAND, //weapon
		[Description("Right Hand")]
        RIGHT_HAND, //weapon
		[Description("Left Finger")]
        LEFT_FINGER, //ring
		[Description("Right Finger")]
        RIGHT_FINGER, //ring
		[Description("Hands")]
        HANDS,
		[Description("Head")]
        HEAD,
		[Description("Chest")]
        CHEST,
		[Description("Legs")]
        LEGS,
		[Description("Feet")]
        FEET,
		[Description("Necklace")]
        NECK,
        _COUNT,
		NONE, //none, goes only to inventory
        HAND, //generic, left or right hand
        LEFT_RIGHT_HAND, //generic both hands
        FINGER, //left or right finger
        LEFT_RIGHT_FINGER, //both left and right fingers
    }

    public enum ItemSubType
    {
        NONE,
        //Weapons
		[Description("")]
        KNIFE,
		[Description("")]
        DAGGER,
		[Description("")]
        SWORD,
		[Description("")]
        AXE,
		[Description("")]
        MACE,
        [Description("")]
        BOW,
        [Description("")]
        CROSSBOW,
        //Armors
        [Description("")]
        CLOTH,
        [Description("")]
        LEATHER,
        [Description("")]
        MAIL,
        [Description("")]
        PLATE,
        //
        //Potions

        _COUNT
    }
	
	public enum WeaponUseType
	{
		NONE,
		ONE_HANDED,
		TWO_HANDED,
		RANGED,
		THROW,
		_COUNT
	}

    //Map item_type-inventory_slot_type, eg. boots = feet
    public static EquipmentSlotType[] ItemSubTypeSlotTypeMap = new EquipmentSlotType[(int)ItemSubType._COUNT];
	public static ItemType[] ItemSubTypeTypeMap = new ItemType[(int)ItemSubType._COUNT];
    public static EquipmentSlotType[] GenericSlotTypeMap = new EquipmentSlotType[(int)EquipmentSlotType._COUNT];
	
	public static string ItemToString(GameplayItem Item)
	{
		string str = string.Format("Name: {0}", Item.Name);
        str += string.Format("\nType: {0}\t{1}", GameText.Singleton.GetItemType(Item.Type), GameText.Singleton.GetItemSubType(Item.SubType));
        return str;
	}

    public enum UnitActionType
    {
        NONE, //when unit do nothing
        MOVE, //when unit move
        RUN, //when unit run
        JUMP, //when unit jump
        ATTACK, //when unit try to attack
        DEAL_HIT, //when unit hit another hunit
        RECEIVE_HIT, //when unit got hit from another unit
        CAST_SPELL, //when unit cast spell
        EAT, //when unit eat food
        USE, //when unit use item
        INTERACT, //when unit interact with environment
        _COUNT //total number of all unit actions
    }

    public static void ProcessInteraction(Unit Unit, InteractObject Obj)
    {
        Debug.Log("Interact");
        if (Obj.IsContainer)
        { 
            GameGUI.Singleton.LootWindow.SetItems(Obj.ContainedItems);
			Obj.Interact(Unit);
            GameGUI.Singleton.LootWindow.Show(Obj);
        }
    }

    public class UnitAction
    {
        public UnitActionType Type = UnitActionType.NONE;
        public object Source = null; //action invoker
        public object Target = null; //target of the action

        public UnitAction() { }
    }
	
	public class GameManager : TSingleton<GameManager>
	{
        public GameManager()
        {
            
            //ItemSubTypeSlotTypeMap[(int)ItemSubType.CROSSBOW] = EquipmentSlotType.HANDS;
            //ItemSubTypeSlotTypeMap[(int)ItemSubType.DAGGER] = EquipmentSlotType.HAND;
            //ItemSubTypeSlotTypeMap[(int)ItemSubType.KNIFE] = EquipmentSlotType.HAND;
            //ItemSubTypeSlotTypeMap[(int)ItemSubType.LIGHT_CROSSBOW] = EquipmentSlotType.HANDS;
            //ItemSubTypeSlotTypeMap[(int)ItemSubType.LONG_BOW] = EquipmentSlotType.HANDS;
            //ItemSubTypeSlotTypeMap[(int)ItemSubType.NONE] = EquipmentSlotType.NONE;
            //ItemSubTypeSlotTypeMap[(int)ItemSubType.ONE_HANDED_AXE] = EquipmentSlotType.HAND;
            //ItemSubTypeSlotTypeMap[(int)ItemSubType.ONE_HANDED_MACE] = EquipmentSlotType.HAND;
            //ItemSubTypeSlotTypeMap[(int)ItemSubType.ONE_HANDED_SWORD] = EquipmentSlotType.HAND;
            //ItemSubTypeSlotTypeMap[(int)ItemSubType.SHORT_BOW] = EquipmentSlotType.HANDS;
            //ItemSubTypeSlotTypeMap[(int)ItemSubType.TWO_HANDED_AXE] = EquipmentSlotType.HANDS;
            //ItemSubTypeSlotTypeMap[(int)ItemSubType.TWO_HANDED_MACE] = EquipmentSlotType.HANDS;
            //ItemSubTypeSlotTypeMap[(int)ItemSubType.TWO_HANDED_SWORD] = EquipmentSlotType.HANDS;
			
            //ItemSubTypeTypeMap[(int)ItemSubType.CROSSBOW] = ItemType.WEAPON;
            //ItemSubTypeTypeMap[(int)ItemSubType.DAGGER] = ItemType.WEAPON;
            //ItemSubTypeTypeMap[(int)ItemSubType.KNIFE] = ItemType.WEAPON;
            //ItemSubTypeTypeMap[(int)ItemSubType.LIGHT_CROSSBOW] = ItemType.WEAPON;
            //ItemSubTypeTypeMap[(int)ItemSubType.LONG_BOW] = ItemType.WEAPON;
            //ItemSubTypeTypeMap[(int)ItemSubType.NONE] = ItemType.NONE;
            //ItemSubTypeTypeMap[(int)ItemSubType.ONE_HANDED_AXE] = ItemType.WEAPON;
            //ItemSubTypeTypeMap[(int)ItemSubType.ONE_HANDED_MACE] = ItemType.WEAPON;
            //ItemSubTypeTypeMap[(int)ItemSubType.ONE_HANDED_SWORD] = ItemType.WEAPON;
            //ItemSubTypeTypeMap[(int)ItemSubType.SHORT_BOW] = ItemType.WEAPON;
            //ItemSubTypeTypeMap[(int)ItemSubType.TWO_HANDED_AXE] = ItemType.WEAPON;
            //ItemSubTypeTypeMap[(int)ItemSubType.TWO_HANDED_MACE] = ItemType.WEAPON;
            //ItemSubTypeTypeMap[(int)ItemSubType.TWO_HANDED_SWORD] = ItemType.WEAPON;
        }
		
		public Gameplay.ItemType CheckItemType(Gameplay.ItemSubType SubType)
		{
			return ItemSubTypeTypeMap[(int)SubType];
		}

        public static void ProcessFight(Unit Attacker, Unit Defender)
        {

        }

	}

}