using UnityEngine;
using System.Collections.Generic;

public class InstantiateManager : MonoBehaviour 
{
    private static InstantiateManager m_Singleton;

    public int MaxParticleSystems = 10;
    public List<GameObject> ParticleSystemTypes = new List<GameObject>();

    private Dictionary<string, ParticleSystemPool> m_ParticlePools = new Dictionary<string, ParticleSystemPool>();
	

    public GameObject UnitEmpty;

    public static InstantiateManager Singleton
    {
        get { return m_Singleton; }
    }

    void Awake()
    {
        m_Singleton = this;
        GameplayItemManager.Singleton.LoadDatabase();
    }

	// Use this for initialization
	void Start () 
    {
        foreach (GameObject GO in ParticleSystemTypes)
        {
            ParticleSystemPool Pool = new ParticleSystemPool();
            for (int i = 0; i < MaxParticleSystems; ++i)
            {
                Pool.AddParticleSystem(Instantiate(GO) as GameObject);
            }

            m_ParticlePools.Add(GO.name, Pool);
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        foreach (ParticleSystemPool Pool in m_ParticlePools.Values)
        {
            Pool.Update();
        }
	}

    public GameObject GetNextParticleSystem(string name)
    {
        return m_ParticlePools[name].GetNext();
    }

    public Unit CreateUnit()
    {
        GameObject Obj = Instantiate(UnitEmpty) as GameObject;
        Obj.name = string.Format("Unit_{0}", Random.Range(1, 10000));
        Unit Unit = Obj.GetComponent<Unit>();
        Unit.Name = Obj.name;
        return Unit;
    }
}

public class ParticleSystemPool
{
    public List<GameObject> m_ParticleSystems = new List<GameObject>();
    public Stack<int> m_FreeParticleSystems = new Stack<int>();

    public void AddParticleSystem(GameObject PS)
    {
        int id = 0;
        m_ParticleSystems.Add(PS);
        m_FreeParticleSystems.Push(id++);
    }

    public void Update()
    {
        //for (int i = m_ParticleSystems.Count; i-- > 0; )
        //{
        //    if (!m_ParticleSystems[i].GetComponent<ParticleSystem>().isPlaying)
        //    {
        //        //m_FreeParticleSystems.Push(i);
        //    }
        //}
    }

    GameObject FindNotPlaying()
    {
        for (int i = m_ParticleSystems.Count; i-- > 0; )
        {
            if (!m_ParticleSystems[i].GetComponent<ParticleSystem>().isPlaying)
            {
                return m_ParticleSystems[i];
            }
        }

        return null;
    }

    public GameObject GetNext()
    {
        //Debug.Log(m_FreeParticleSystems.Count);
        //if (m_FreeParticleSystems.Count == 0)
        //    return null;
        //return m_ParticleSystems[m_FreeParticleSystems.Pop()];
        return FindNotPlaying();
    }
}
