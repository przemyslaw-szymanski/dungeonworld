using UnityEngine;
using System.Collections;

public class Missle : MonoBehaviour 
{
    public GameObject HitPrefab;

    public int DamageType = (int)Gameplay.DamageType.NONE;
    public float Speed = 10;
    public float ExplosionRange = 1;
    public float ExplosionForce = 1;

    private GameObject m_HitObj;

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        Transform.position += Transform.forward * Speed * Time.deltaTime;
	}

    public Transform Transform
    {
        get { return this.rigidbody.transform; }
    }

    void OnTriggerEnter(Collider Col)
    {
        Debug.Log(string.Format("Collision: {0} / {1}", Col.name, Col.tag));

        Explode();

        if (Col.rigidbody != null)
        {
            Col.rigidbody.AddExplosionForce(ExplosionForce, Transform.position, ExplosionRange);
        }

        if (Col.tag == "Unit")
        {
            Unit Unit = Col.GetComponent<Unit>();

            if (Unit.IsDead)
                Unit.Alive();
            else
                Unit.Die();
        }

        Destroy();
    }

    public void Explode()
    {
        //m_HitObj = Instantiate(HitPrefab, transform.position, Quaternion.identity) as GameObject;
        //m_Timer.Start();
        m_HitObj = InstantiateManager.Singleton.GetNextParticleSystem(HitPrefab.name);
        m_HitObj.transform.position = transform.position;
        m_HitObj.GetComponent<ParticleSystem>().Play();
    }

    public void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }
}
