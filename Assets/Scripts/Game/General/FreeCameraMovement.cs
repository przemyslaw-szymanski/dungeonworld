using UnityEngine;
using System.Collections;

public class FreeCameraMovement : MonoBehaviour 
{
	public float MoveSpeed = 1.0f;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 vecDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		
		if(vecDir != Vector3.zero)
		{
		
			Vector3 vecForward 	= this.transform.forward;
			Vector3 vecRight	= this.transform.right;
			Vector3 vecUp		= this.transform.up;
			
			Vector3 vecMove = new Vector3();
			vecMove += vecForward * vecDir.z * MoveSpeed;
			vecMove += vecRight * vecDir.x * MoveSpeed;
			
			this.transform.position += vecMove;
		}
	}
}
