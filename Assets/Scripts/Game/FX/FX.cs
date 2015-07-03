using UnityEngine;
using System.Collections.Generic;

public class FX : MonoBehaviour 
{
    private bool m_enabled = true;
    private bool m_fxEnabled = true;
    private bool m_lightEnabled = true;

    protected ParticleSystem[] m_ParticleSystems;
    protected List<Light> m_Lights = new List<Light>();
	
	void Awake()
	{
        FXAwake();
		OnFXAwake();
	}
	
	// Use this for initialization
	void Start () 
    {
        FXStart();
        OnFXStart();
	}
	
	// Update is called once per frame
	void Update () 
    {
        FXUpdate();
        OnFXUpdate();
	}

    void OnDestroy()
    {
        OnFXDestroy();
        FXDestroy();
    }

    protected virtual void OnFXAwake()
    {
    }

    protected virtual void OnFXUpdate()
    {
    }

    protected virtual void OnFXStart()
    {
    }

    protected virtual void OnFXDestroy()
    {
    }

    void FXDestroy()
    {
        m_Lights.Clear();
        m_ParticleSystems = null;
    }
	
	void FXStart()
	{
		//FXStart();
	}

    void FXAwake()
    {
        m_Lights.Clear();
        m_ParticleSystems = this.GetComponents<ParticleSystem>();
        //Debug.Log("particle len: " + m_ParticleSystems.Length);
        //Get light children
        foreach (Transform T in this.transform)
        {
            if (T.light != null)
                m_Lights.Add(T.light);
        }
    }

    protected void FXUpdate()
    {
    }
	
	void OnEnable()
	{
		Enable(true);
	}
	
	void OnDisable()
	{
		Enable(false);
	}

    public void Enable(bool enable)
    {
        if (m_enabled == enable)
            return;

        EnableFX(enable);
		EnableLight(enable);
		
		m_enabled = enable;
    }

    public bool Enabled
    {
        get { return m_enabled; }
        set { Enable(value); }
    }

    public bool FXEnabled
    {
        get { return m_fxEnabled; }
        set { EnableFX(value); }
    }

    public bool LightEnabled
    {
        get { return m_lightEnabled; }
        set { EnableLight(value); }
    }

    public void EnableFX(bool enable)
    {
        if (m_fxEnabled == enable)
            return;

        m_fxEnabled = enable;

        foreach (ParticleSystem PS in ParticleSystems)
        {
            PS.enableEmission = enable;
        }
    }

    public void EnableLight(bool enable)
    {
        if (m_lightEnabled == enable)
            return;

        m_lightEnabled = enable;
        foreach (Light L in Lights)
        {
            if(L != null)
                L.enabled = enable;
        }
    }

    public ParticleSystem[] ParticleSystems
    {
        get { return m_ParticleSystems; }
    }

    public List<Light> Lights
    {
        get { return m_Lights; }
    }
}
