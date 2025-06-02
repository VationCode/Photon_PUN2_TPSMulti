using System.Threading.Tasks;
using UnityEngine;
using DUS.PlayerCore.Locomotion;
using DUS.PlayerCore;

public class SlideState : LocomotionStrategyState
{
    public SlideState(PlayerCore playerCore) : base(playerCore) { }
    protected override LocomotionMainState DetermineStateType() => LocomotionMainState.Slide;
    protected override AniParmType[] SetAniParmType() => new AniParmType[] { AniParmType.SetBool, AniParmType.SetTrigger}; //슬라이딩 상태와 시작의 Trigger가 필요
    protected override float SetMoveSpeed() => m_PlayerCore.m_SlideSpeed;

    public  override void Enter()
    {
        base.Enter();
        
        m_GoStateDelayTime = 3;
        m_IsNotInputMove = true;
        m_IsNotBodyRot = true;

        m_AniName = "Slide";
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        m_Locomotion.HandleMaintainForwardForceMove(m_PlayerCore.transform.forward * m_PlayerCore.m_SlideSpeed);
    }


    public override void Update()
    {
        base.Update();

        if (!CheckComeInCurrentAni(m_AniName)) return;

        if (m_GoNextStateTime >= 0)
        {
            m_GoNextStateTime -= Time.fixedDeltaTime;
        }
        else
        {
            m_Locomotion.SetNextState(LocomotionMainState.Idle);
        }

    }

    public override void Exit()
    {
        base.Exit();
        m_PlayerCore.OnChangeColider(true);
    }
}
