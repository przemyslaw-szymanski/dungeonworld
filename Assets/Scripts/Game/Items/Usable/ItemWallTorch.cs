using UnityEngine;
using System.Collections;

public class ItemWallTorch : Item 
{
    private FXTorch m_FXTorch;
    private GameObject m_Torch;

	// Use this for initialization
	protected override void OnItemStart () 
    {
        GameObject FX = this.FindFXGameObject();
        m_FXTorch = FX.GetComponent<FXTorch>();

        m_Torch = this.transform.FindChild("Torch").gameObject;
	}
	
	// Update is called once per frame
	protected override void OnItemUpdate() 
    {
      
	}

    protected override bool OnUse(Unit Unit)
    {
        //If this item is disabled - a unit took a torch from it, it can be put back if a unit has a torch

        EnableItem(!ItemEnabled);
        return true;
    }

    protected override bool OnItemEnable(bool enable)
    {
        Utils.EnableGameObjectRender(m_Torch, enable);
        m_FXTorch.Enabled = enable;
		return true;
    }
    
}
