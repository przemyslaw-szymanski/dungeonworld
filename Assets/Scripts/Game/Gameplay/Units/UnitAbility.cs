using UnityEngine;
using System.Collections;

public class UnitAbility 
{
    public string Name = "Unknown";
    public bool Passive = false;
    public Gameplay.UnitActionType ActionTrigger = Gameplay.UnitActionType.NONE;

    public Unit Unit; //it has to be set after object created

    public UnitAbility()
    {
    }
    
    public void Use()
    {
        
    }
}
