using UnityEngine;
using System.Collections;

public class ItemTorch : Weapon 
{
    public float Fuel = 100;

    protected FXTorch m_FXObj = null;

    protected override void  OnWeaponAwake()
    {
       
    }

	// Use this for initialization
	protected override void OnWeaponStart() 
    {	
		//Debug.Log("item torch start");		
		//Debug.Log("item torch awake");	
		GameObject FXObj = this.FindFXGameObject();
		//Debug.Log(string.Format("fxobj: {0}", FXObj.name));
		m_FXObj = FXObj.GetComponent<FXTorch>();
		//Debug.Log(string.Format("fxtorch: {0}, {1}", m_FXObj.name, m_FXObj.ParticleSystems.Length));
	}
	
	// Update is called once per frame
	protected override void OnWeaponUpdate() 
    {
        if (this.InUse)
        {
        }
	}

    public void CheckFuel()
    {
    }

    protected override bool OnUse(Unit Unit)
    {
        if (Fuel <= 0.0f)
        {
            this.FailedUseReason = "No fuel in a torhch";
            return false;
        }
        //Debug.Log(string.Format("OnUse Torch owner: {0}", Unit.name));
        this.OnChangeOwner(Unit);

        return true;
    }

    protected override bool OnChangeOwner(Unit NewOwner)
    {
        //Debug.Log(string.Format("new owner: {0}", NewOwner.name));

        this.gameObject.transform.parent = NewOwner.AttachPoint.transform;
        this.gameObject.transform.localPosition = new Vector3(0, 0, -1);
        
        //Debug.Log(NewOwner.AttachPoint.name);
        //Debug.Log(this.gameObject.transform.position);
        if (NewOwner == null)
        {
            //m_FXObj.Enabled = true;
        }
        else
        {
            //Debug.Log(string.Format("ItemTorch.Start: {0}", m_FXObj.name));
            //m_FXObj.Enabled = false;
            //m_FXObj.LightEnabled = true;
        }
        return true;
    }
	
	protected override bool OnItemEnable (bool enable)
	{	
		m_FXObj.FXEnabled = enable;
		m_FXObj.LightEnabled = enable;
		return true;
	}
}
