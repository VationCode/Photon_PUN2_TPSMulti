using UnityEngine;
using DUS.Player.Locomotion;
using DUS.Player;
using System.Threading.Tasks;

public class JumpState : LocomotionStrategyState
{
    public JumpState(PlayerCore playerCore) : base(playerCore) { }
    protected override LocomotionMainState DetermineStateType() => LocomotionMainState.Jump;
    protected override AniParmType[] SetAniParmType() => new AniParmType[] { AniParmType.SetTrigger };

    protected override float SetMoveSpeed() => m_Locomotion.m_CurrentSpeed / 2;

    bool m_isJumping = false;

    public override async void Enter()
    {
        base.Enter();
        m_IsNotInputMove = true;
        
        await WaitUntillJumpStart();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        m_Locomotion.HandleMaintainForwardForceMove(m_Locomotion.m_CurrentVelocityXZ * m_PlayerCore.m_JumpForwardSpeedPercent);
    }

    public override void Update()
    {
        base.Update();
        
        if (m_isJumping)
        {
            m_GoNextStateTime -= Time.deltaTime;
            if (m_GoNextStateTime <= 0) m_Locomotion.SetNextState(LocomotionMainState.InAir);
        }
    }

    public override void Exit()
    {
        base.Exit();
        m_PlayerCore.m_AnimationManager.m_IsJumpStart = false;
        m_isJumping = false;
    }

    private async Task WaitUntillJumpStart()
    {
        while (!m_PlayerCore.m_AnimationManager.m_IsJumpStart) await Task.Yield(); // 매 프레임 기다림
        m_isJumping = true;
        m_GoNextStateTime = 1; 
        m_Locomotion.HandleJumpForce();
    }



    /*public void UpdateJumpForce()
    {
        // 자연스럽게 점프포스를 주는 계산
        m_JumpTimer += Time.fixedDeltaTime;
        float t = m_JumpTimer / m_PlayerCore.m_JumpDuration;

        if (t < 1f)
        {
            float force = m_PlayerCore.m_JumpForce * m_PlayerCore.m_JumpCurve.Evaluate(t);
            m_PlayerCore.SetRigidVelocityY(force);
            m_Locomotion.m_CurrentVelocityY = force;
        }
        else
        {
            m_Locomotion.SetNextState(LocomotionMainState.InAir);
        }
    }*/
}
