using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
	public GameObject Prefab;

    private CharacterController m_Controller;
    private GameObject m_MissleSpawnPoint;

    public float DistanceToUse = 5;
    public const int PartySize = 5;

    private Unit[] m_PartyUnits = new Unit[PartySize];
    private Unit m_CurrPartyUnit;

	// Use this for initialization
	void Start () 
	{
        m_Controller = this.GetComponent<CharacterController>();

        m_MissleSpawnPoint = Camera.mainCamera.transform.FindChild("MissleSpawnPoint").gameObject;
        //m_MissleSpawnPoint = Gameplay.FindChild(this.gameObject, "MissleSpawnPoint");
        //Debug.Log(m_MissleSpawnPoint.name);

        CreatePartyUnits();
	}
	
	// Update is called once per frame
	void Update () 
	{
        DetectMouseClick();
        DetectKeyDown();
        
	}

    void DetectKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject Missle = Instantiate(Prefab) as GameObject;
            Missle.rigidbody.transform.rotation = m_MissleSpawnPoint.transform.rotation;
            Missle.rigidbody.transform.position = m_MissleSpawnPoint.transform.position;
            //Missle.rigidbody.transform.rotation = m_Controller.transform.rotation;
            //Missle.rigidbody.transform.position = m_Controller.transform.position + m_Controller.transform.forward * (m_Controller.radius + 1);
        }
    }

    GameObject DetectMouseClick()
    {
        GameObject HitObj = null;
        
        if ( Input.GetMouseButtonDown(0))
        {
            RaycastHit Hit;
            Ray Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //var select = GameObject.FindWithTag("select").transform;
            if (Physics.Raycast(Ray, out Hit, DistanceToUse))
            {
                //Debug.Log(string.Format("clicked: {0}", Hit.collider.name));
                if (Hit.collider == null)
                    return null;
                return ProcessClickedObject(Hit.collider.gameObject);
            }
        }

        return HitObj;
    }

    public GameObject ProcessClickedObject(GameObject HitObj)
    {
        switch (HitObj.tag)
        {
            case Gameplay.ObjectTag.Interact:
            {
                InteractObject Obj = HitObj.GetComponent<InteractObject>();
                m_CurrPartyUnit.Interact(Obj);
            }
            break;
        }

        return HitObj;
    }

    public void CreatePartyUnits()
    {
        for (int i = 0; i < m_PartyUnits.Length; ++i)
        {
            //m_PartyUnits[i] = Gameplay.CreateRandomUnit(true);
            m_PartyUnits[i] = InstantiateManager.Singleton.CreateUnit();
            m_PartyUnits[i].transform.parent = this.transform;
            m_PartyUnits[i].UseController = false;
			m_PartyUnits[i].Portrait = ResourceManager.GetRandomPortrait();

            if (m_PartyUnits[i].AttachPoint == null)
            {
                m_PartyUnits[i].AttachPoint = m_MissleSpawnPoint;
            }
        }

        m_CurrPartyUnit = m_PartyUnits[0];
    }
	
	public Unit[] PartyUnits
	{
		get { return m_PartyUnits; }
	}
	
	public Unit CurrentPartyUnit
	{
		get { return m_CurrPartyUnit; }
	}
}
