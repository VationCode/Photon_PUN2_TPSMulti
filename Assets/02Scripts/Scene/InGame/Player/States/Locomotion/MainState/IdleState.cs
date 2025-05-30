using UnityEngine;
using DUS.Player.Locomotion;

public class IdleState : LocomotionStrategyState
{
    public IdleState(PlayerCore playerCore) : base(playerCore) { }
    protected override LocomotionMainState DetermineStateType() => LocomotionMainState.Idle;
    protected override AniParmType[] SetAniParmType() => new AniParmType[]{ AniParmType.SetBool};
    protected override float SetMoveSpeed() => 0;
    public override void Enter()
    {
        base.Enter();
    }

    public override void FixedUpdate()
    {
        // 1. UpdateMovement µø¿€
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        bool isCrouch = m_PlayerCore.m_InputManager.m_IsCrouch_LocoF;
        bool isJump = m_PlayerCore.m_InputManager.m_IsJump_LocoM;
        bool isMove = m_PlayerCore.m_InputManager.m_IsMove_LocoM;

        m_Locomotion.HandleCheckFlags(LocomotionSubFlags.Crouch, isCrouch);

        // Main
        if (isJump)
        {
            m_Locomotion.SetNextState(LocomotionMainState.Jump);
        }
        else if (isMove)
        {
            m_Locomotion.SetNextState(LocomotionMainState.Move);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}