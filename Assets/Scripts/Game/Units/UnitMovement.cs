using UnityEngine;
using System.Collections;

public class UnitMovement : MonoBehaviour 
{

    public GameObject EndPoint;

    public float WalkSpeed = 3.0f;
    public float RunSpeed = 6.0f;
    public float RotateSpeed = 3.0f;
    public float EndPointRadius = 0.01f;

    private float m_startTime;
    private float m_colliderHeight;

    private CharacterController m_Controller;
    private BoxCollider m_Collider;
    private MeshCollider m_MeshCollider;

    private Vector3 m_vecCurrPos = new Vector3();
    private Vector3 m_vecEnd = new Vector3();
    private Vector3 m_vecDir = new Vector3();
    private Vector3 m_vecGravity = new Vector3();
    private float m_distToEnd = -1;

    public enum MovementState
    {
        STANDING,
        MOVING_FORWARD,
        MOVING_BACKWARD,
        RUNNING,
        ROTATING,
        JUMPING,
        _COUNT
    }

    public enum MovementMessage
    {
        NONE,
        STOP,
        MOVE_FORWARD,
        MOVE_BACKWARD,
        RUN,
        ROTATE,
        JUMP,
        _COUNT
    }

    private MovementState m_movementState = MovementState.STANDING;
    private MovementMessage m_currMsg = MovementMessage.STOP;

    private delegate void MovementDelegate();
    private static MovementDelegate[] m_MovementDelegates = new MovementDelegate[(int)MovementMessage._COUNT];// { new MovementDelegate(UnitMovement.MoveStop), MoveForward(), MoveBackward(), MoveRun(), MoveRotate() };
    private MovementDelegate m_CurrMovementFunction;

    // Use this for initialization
    protected void UnitMovementStart()
    {
        m_MovementDelegates[(int)MovementMessage.MOVE_BACKWARD] = new MovementDelegate(MoveBackward);
        m_MovementDelegates[(int)MovementMessage.MOVE_FORWARD] = new MovementDelegate(MoveForward);
        m_MovementDelegates[(int)MovementMessage.ROTATE] = new MovementDelegate(MoveRotate);
        m_MovementDelegates[(int)MovementMessage.RUN] = new MovementDelegate(MoveRun);
        m_MovementDelegates[(int)MovementMessage.STOP] = new MovementDelegate(MoveStop);
        m_MovementDelegates[(int)MovementMessage.JUMP] = new MovementDelegate(MoveJump);
        m_MovementDelegates[(int)MovementMessage.NONE] = new MovementDelegate(MoveNone);
        
        m_CurrMovementFunction = m_MovementDelegates[(int)MovementMessage.STOP];

        m_startTime = Time.time;
        m_Controller = this.GetComponent<CharacterController>();
        m_Collider = this.GetComponent<BoxCollider>();
   
        m_vecEnd = Position;
        m_vecEnd.y = 0;
        
        if (Controller != null)
        {
            m_colliderHeight = m_Controller.height;
        }

        //MovePut();
    }

    // Update is called once per frame
    protected void UnitMovementUpdate()
    {
        CalcDistanceToEndPoint();
        CalcMoveDir();

        //Debug.Log(m_currMsg);

        if (IsAtPosition())
        {
            SetMovementMessage(MovementMessage.STOP);
        }
        
        m_CurrMovementFunction();
      
        //GameObject Chunk = CheckMapChunk();	

    }

    void CalcMoveDir()
    {
        m_vecCurrPos.x = Position.x;
        m_vecCurrPos.z = Position.z;
        m_vecEnd.x = DestinationPoint.x;
        m_vecEnd.z = DestinationPoint.z;
        m_vecDir = (m_vecEnd - m_vecCurrPos).normalized;
    }

    public void MoveRotate()
    {
        if (m_vecDir == Vector3.zero)
            return;

        SetMovementMessage(MovementMessage.ROTATE);
        m_movementState = MovementState.ROTATING;

        Quaternion quatLook = Quaternion.LookRotation(m_vecDir, Vector3.up);
        
        //Quaternion quatRotation = Quaternion.Slerp(Controller.transform.rotation, quatLook, RotateSpeed * Time.deltaTime);
        //m_Controller.transform.rotation = quatRotation;

        Quaternion quatRotation = Quaternion.Slerp(transform.rotation, quatLook, RotateSpeed * Time.deltaTime);
        transform.rotation = quatRotation;
    }

    public void MoveForward()
    {     
        if (NeedRotate())
        {
            MoveRotate();
        }

        SetMovementMessage(MovementMessage.MOVE_FORWARD);
        m_movementState = MovementState.MOVING_FORWARD;

        Vector3 vecMove = m_vecDir; // CalcGravity(m_vecDir);
        //Controller.Move(vecMove * Time.deltaTime * WalkSpeed);
        transform.position += vecMove * Time.deltaTime * WalkSpeed;
    }

    public void MoveRun()
    {     
        if (NeedRotate())
            MoveRotate();

        SetMovementMessage(MovementMessage.RUN);
        m_movementState = MovementState.RUNNING;

        Vector3 vecMove = CalcGravity(m_vecDir);
        Controller.Move(vecMove * Time.deltaTime * RunSpeed);
    }

    public void MoveBackward()
    {
        SetMovementMessage(MovementMessage.MOVE_BACKWARD);

        m_movementState = MovementState.MOVING_BACKWARD;

        Vector3 vecMove = CalcGravity(m_vecDir);
        Controller.Move(vecMove * Time.deltaTime * WalkSpeed);
    }

    public void MoveJump()
    {
        SetMovementMessage(MovementMessage.JUMP);
        m_movementState = MovementState.JUMPING;
    }

    public void MoveStop()
    {
        SetMovementMessage(MovementMessage.STOP);
        m_movementState = MovementState.STANDING;
        m_vecEnd = m_vecCurrPos;
        m_CurrMovementFunction = m_MovementDelegates[(int)MovementMessage.NONE];
    }

    public void MoveNone()
    {
      
    }

    public void MovePut()
    {
        //m_Controller.transform.Rotate(-90, 0, 0);
        //float tmp = Controller.height;
        //Controller.height = Controller.radius;
        //Controller.radius = tmp;
        m_Collider.size = new Vector3(m_Collider.size.x, 0.2f, m_Collider.size.y);
        m_Collider.center = new Vector3(m_Collider.center.x, 0.1f, m_Collider.center.z);
        Debug.Log(string.Format("pos: {0}, cpos: {1}", transform.position, m_Collider.center));
        SetMovementMessage(MovementMessage.STOP);
    }

    public bool NeedRotate()
    {
        //float angle = Vector3.Dot(Controller.transform.forward, m_vecEnd);
        float angle = Vector3.Dot(transform.forward, m_vecEnd);
        return angle > 0.001f || angle < 0.001f;
    }

    Vector3 CalcGravity(Vector3 vecPos)
    {
        Vector3 vecRet = vecPos;
        vecRet.y -= 10 * Time.deltaTime;
        return vecRet;
    }

    public MovementState MoveState
    {
        get { return m_movementState; }
    }

    public Vector3 DestinationPoint
    {
        get { return m_vecEnd; }
        set { m_vecEnd.x = value.x; m_vecEnd.y = 0; m_vecEnd.z = value.z; }
    }

    public bool IsAtPosition()
    {
        //return m_distToEnd <= Controller.radius;
        return m_distToEnd <= m_Collider.size.z;
    }

    public void CalcDistanceToEndPoint()
    {
        m_distToEnd = Vector3.Distance(m_vecCurrPos, m_vecEnd);
    }

    public Vector3 VecMul(Vector3 Left, Vector3 Right)
    {
        return new Vector3(Left.x * Right.x, Left.y * Right.y, Left.z * Right.z);
    }

    public Vector3 Position
    {
        //get { return Controller.transform.position; }
        get { return m_Collider.transform.position; }
        set { Controller.transform.position = value; }
    }

    public Vector3 ForwardDirection
    {
        get { return Controller.transform.forward; }
        //set { m_Controller.transform.position = value; }
    }

    public bool EnablePhysics
    {
        set { m_Controller.enabled = value; }
        get { return m_Controller.enabled; }
    }

    public Vector3 MoveDirection
    {
        get { return m_vecDir; }
    }

    public GameObject CheckMapChunk()
    {
        var Hit = new RaycastHit();
        var Ray = new Ray(Position, Vector3.down);

        if (Physics.Raycast(Ray, out Hit, 1000.0f))
        {
            Transform Parent = Hit.collider.transform.parent;
            if (Parent == null)
                return Hit.collider.gameObject;

            return Parent.gameObject;
        }

        return null;
    }

    public void SetMovementMessage(MovementMessage msg)
    {
        if (m_currMsg == msg)
            return;
        m_currMsg = msg;
        m_CurrMovementFunction = m_MovementDelegates[(int)m_currMsg];
        //Debug.Log(msg);
    }

    public MovementMessage MoveMessage
    {
        get { return m_currMsg; }
    }

    public CharacterController Controller
    {
        get { return m_Controller; }
    }

}
