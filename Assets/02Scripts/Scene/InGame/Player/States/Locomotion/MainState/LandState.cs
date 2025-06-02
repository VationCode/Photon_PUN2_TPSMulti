using UnityEngine;
using DUS.PlayerCore.Locomotion;

//ÂøÁö »óÅÂ
public class LandState : LocomotionStrategyState
{
    public LandState(PlayerCore playerCore) : base(playerCore){}
    protected override LocomotionMainState DetermineStateType() => LocomotionMainState.Land;
    protected override AniParmType[] SetAniParmType() => new AniParmType[] { AniParmType.SetBool };
    protected override float SetMoveSpeed() => 0;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (m_IsComeInCurrentStateAni)
        {

            return;
        }

        if (m_AnimationTime >= 0)
        {
            
            m_AnimationTime -= Time.deltaTime;
        }
        else
        {
            m_Locomotion.SetNextState(LocomotionMainState.Idle);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }

}
