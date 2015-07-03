using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveTest : MonoBehaviour 
{
	public GameObject EndPoint;
	
	public float MoveSpeed = 3.0f;
	public float RotateSpeed = 3.0f;
	public float EndPointRadius = 0.01f;
	
	private float m_startTime;
	
	private CharacterController m_Controller;
	
	private Vector3 m_vecCurrPos = new Vector3();
	private Vector3 m_vecEnd = new Vector3();
	private Vector3 m_vecDir = new Vector3();
	private float m_distToEnd = -1;
	
	// Use this for initialization
	void Start () 
	{
		m_startTime = Time.time;
		m_Controller = this.GetComponent<CharacterController>();
		
		if(animation != null)
		{
		animation["idle"].layer = 0; animation["idle"].wrapMode = WrapMode.Loop;
		animation["walk01"].layer = 1; animation["walk01"].wrapMode = WrapMode.Loop;
		animation["run"].layer = 1; animation["run"].wrapMode = WrapMode.Loop;
		animation["attack"].layer = 3; animation["attack"].wrapMode = WrapMode.Once;
		animation["headbutt"].layer = 3; animation["headbutt"].wrapMode = WrapMode.Once;
		animation["scratchidle"].layer = 3; animation["scratchidle"].wrapMode = WrapMode.Once;
		animation["walk02"].layer = 3; animation["walk02"].wrapMode = WrapMode.Once;
		animation["standup"].layer = 3; animation["standup"].wrapMode = WrapMode.Once;
		}		
		Animation Anim = this.GetComponent<Animation>();
		GetAnimationNames(Anim);
	}
	
	// Update is called once per frame
	void Update () 
	{
		CalcDistanceToEndPoint();
		CalcMoveDir();
		Rotate(EndPoint.transform.position);
		Move(EndPoint.transform.position);
		
		//GameObject Chunk = CheckMapChunk();	
	
	}
	
	System.Collections.Generic.List<string> GetAnimationNames(Animation Anim)
	{
		Debug.Log(string.Format("Animations for: {0}", this.name));
		System.Collections.Generic.List<string> Names = new System.Collections.Generic.List<string>();
		if(Anim == null)
			return Names;
		
		Debug.Log(Anim.name);
		
		foreach(AnimationState State in Anim)
		{
			Names.Add(State.name);
			Debug.Log(State.name);
		}
		
		return Names;
	}
	
	void CalcMoveDir()
	{
		m_vecCurrPos.x = Position.x;
		m_vecCurrPos.z = Position.z;
		m_vecEnd.x = EndPoint.transform.position.x;
		m_vecEnd.z = EndPoint.transform.position.z;
		m_vecDir = (m_vecEnd - m_vecCurrPos).normalized;
	}
	
	void Rotate(Vector3 vecDstPoint)
	{
		if(IsAtPosition())
			return;
		
		Quaternion quatLook = Quaternion.LookRotation(m_vecDir, Vector3.up);
		Quaternion quatRotation = Quaternion.Slerp(m_Controller.transform.rotation, quatLook, RotateSpeed * Time.deltaTime);
		m_Controller.transform.rotation = quatRotation;
	}
	
	void Move(Vector3 vecDstPoint)
	{	
		if(IsAtPosition())
			return;
		
		Vector3 vecMove = Vector3.MoveTowards(Position, vecDstPoint, MoveSpeed * Time.deltaTime);
		//Debug.Log(vecMove);
		//vecMove *= m_Controller.transform.forward;
		//m_Controller.SimpleMove(VecMul(m_Controller.transform.forward, vecMove));
		//m_Controller.SimpleMove(new Vector3(MoveSpeed, MoveSpeed, MoveSpeed));
	
		//vecDir = m_Controller.transform.TransformDirection(vecDir);
		//m_vecDir.y -= 10 * Time.deltaTime;
		if(this.animation != null)
			this.animation.CrossFade("Walk");
		
		vecMove = m_vecDir;
		vecMove.y -= 10 * Time.deltaTime;
		m_Controller.Move(vecMove * Time.deltaTime * MoveSpeed);
	}
	
	bool IsAtPosition()
	{	
		return m_distToEnd <= m_Controller.radius;
	}
	
	void CalcDistanceToEndPoint()
	{
		m_distToEnd = Vector3.Distance(m_vecCurrPos, m_vecEnd);
	}
	
	Vector3 VecMul(Vector3 Left, Vector3 Right)
	{
		return new Vector3(Left.x * Right.x, Left.y * Right.y, Left.z * Right.z);
	}
	
	public Vector3 Position
	{
		get { return m_Controller.transform.position; }
		set { m_Controller.transform.position = value; }
	}
	
	GameObject CheckMapChunk()
	{
		var Hit = new RaycastHit();
        var Ray = new Ray(Position, Vector3.down);
		
        if (Physics.Raycast(Ray, out Hit, 1000.0f))
		{
			Transform Parent = Hit.collider.transform.parent;
			if(Parent == null)
				return Hit.collider.gameObject;

			return Parent.gameObject;
		}
		
		return null;
	}
}
