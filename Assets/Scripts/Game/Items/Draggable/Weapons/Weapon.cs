using UnityEngine;
using System.Collections;

public class Weapon : Item 
{

    protected override void OnItemAwake()
    {
        OnWeaponAwake();
    }

	// Use this for initialization
	protected override void OnItemStart() 
    {
        OnWeaponStart();
	}
	
	// Update is called once per frame
	protected override void OnItemUpdate() 
    {
        OnWeaponUpdate();
	}

    protected override void OnItemDestroy()
    {
        OnWeaponDestroy();
    }

    protected virtual void OnWeaponAwake() { }
    protected virtual void OnWeaponStart() { }
    protected virtual void OnWeaponUpdate() { }
    protected virtual void OnWeaponDestroy() { }
}
