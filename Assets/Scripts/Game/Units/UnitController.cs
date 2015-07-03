using UnityEngine;
using System.Collections;

public class UnitController : UnitAnimations 
{
    protected static BasicAnimationType[] MoveStateAnimTypeMap = new BasicAnimationType[(int)MovementState._COUNT] { BasicAnimationType.IDLE, BasicAnimationType.WALK_FORWARD, BasicAnimationType.WALK_BACKWARD, BasicAnimationType.RUN, BasicAnimationType.ROTATE, BasicAnimationType.JUMP };

    public void UnitControllerStart()
    {
        this.UnitMovementStart();
        this.UnitAnimationsStart();
    }

    public void UnitControllerUpdate()
    {
        this.UnitMovementUpdate();
        this.UnitAnimationsUpdate();
        
        BasicAnimationType animType = MoveStateAnimTypeMap[(int)this.MoveState];
        StartAnimation(animType, WrapMode.Loop, false);
    }
}
