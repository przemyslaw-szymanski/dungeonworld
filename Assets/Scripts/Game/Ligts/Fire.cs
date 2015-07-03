using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour 
{
	public float UpdateTime = 0.05f;
	public Vector3 MovementMin = new Vector3(-1, -1, -1);
	public Vector3 MovementMax = new Vector3(1, 1, 1);
	
	private Vector3 m_vecPos = new Vector3();

	private Timer m_Timer;
	
	// Use this for initialization
	void Start () 
	{
		m_Timer = new Timer();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_Timer.TimeLooped(UpdateTime))
		{
			m_vecPos.x = Random.Range(MovementMin.x, MovementMax.x);
			m_vecPos.y = Random.Range(MovementMin.y, MovementMax.y);
			m_vecPos.z = Random.Range(MovementMin.z, MovementMax.z);
	
			transform.localPosition = m_vecPos;
		}
	}
}
