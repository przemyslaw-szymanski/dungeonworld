using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitAnimations : UnitMovement 
{
    public enum BasicAnimationType : int
    {
        IDLE = 0,
        WALK_FORWARD,
        WALK_BACKWARD,
        RUN,
        ROTATE,
        JUMP,
        ATTACK,
        SPECIAL_ATTACK,
        DEATH,
        HIT,
        _COUNT
    }

    private GameObject m_MissleSpawnPoint;
    public AnimationClip[] BasicAnimations = new AnimationClip[(int)BasicAnimationType._COUNT];
    public List<AnimationClip> AdditionalAnimations = new List<AnimationClip>();
    private AnimationClip m_CurrAnim;

	// Use this for initialization
	public void UnitAnimationsStart() 
    {
        //Try to find spawn points
        foreach (Transform T in this.transform)
        {
            if (T.name == "MissleSpawnPoint")
            {
                m_MissleSpawnPoint = T.gameObject;
            }
        }
	}
	
	// Update is called once per frame
	public void UnitAnimationsUpdate() 
    {
	
	}

    public AnimationClip CurrentAnimation
    {
        get { return m_CurrAnim; }
    }

    public AnimationClip StartAnimation(BasicAnimationType AnimType, WrapMode Mode)
    {
        AnimationClip Clip = BasicAnimations[(int)AnimType];
        if (Clip == null)
            return null;

        Clip.wrapMode = Mode;
        this.animation.Play(Clip.name, PlayMode.StopAll);
       
        m_CurrAnim = Clip;

        return Clip;
    }

    public void StartAnimation(BasicAnimationType basicAnimationType, WrapMode wrapMode, bool playAgain)
    {
        if (m_CurrAnim == BasicAnimations[(int)basicAnimationType] && !playAgain)
            return;

        StartAnimation(basicAnimationType, wrapMode);
    }

}
