// Strategy 패턴
using UnityEngine;
using DUS.PlayerCore.Locomotion;
using System.Collections.Generic;

public abstract class LocomotionStrategyState : IBaseState
{
    protected PlayerCore m_PlayerCore;
    protected PlayerLocomotion m_Locomotion;

    public LocomotionStrategyState(PlayerCore playerCore)
    {
        m_PlayerCore = playerCore;
        m_Locomotion = playerCore.m_Locomotion;
    }

    // TODO : 추상 함수들 차후 Interface로 분류
    protected abstract LocomotionMainState DetermineStateType();
    private LocomotionMainState m_ThisState;
    
    protected abstract AniParmType[] SetAniParmType();
    private AniParmType[] m_AniParmType;

    protected abstract float SetMoveSpeed();
    protected float m_moveSpeed = 0;

    // 해당 상태에서 적용할 고정설정 변수들
    protected bool m_IsNotCheckGround = false;
    protected bool isNotApplyGravity = false;
    protected bool m_IsNotBodyRot = false;
    protected bool m_IsNotInputMove = false;
    protected string m_AniName;

    // 해당 상태에서의 일반 변수
    protected bool m_IsComeInCurrentStateAni = false;
    protected float m_AnimationTime = 0;
    protected float m_GoNextStateTime = 0;
    protected float m_GoStateDelayTime = 0;
    #region ======================================== Update, FixedUpdate, Enter, Exit

    public void InitializeExit()
    {
        m_IsComeInCurrentStateAni = false;
        m_AnimationTime = 0;
        m_GoStateDelayTime = 0;
        m_GoNextStateTime = 0;
    }

    public virtual void Enter()
    {
        // TODO : 상태 전한 지연 필요 시 사용
        m_ThisState = DetermineStateType();
        m_AniName = m_ThisState.ToString();

        m_moveSpeed = SetMoveSpeed();

        m_AniParmType = new AniParmType [SetAniParmType().Length];
        m_AniParmType = SetAniParmType();
        m_Locomotion.SetMainStateAnimation(m_ThisState, m_AniParmType, true); //애니메이션 파라미터 값 적용 (시작)
    }

    public virtual void FixedUpdate()
    {
        m_Locomotion.HandleCheckGround(m_IsNotCheckGround);
        m_Locomotion.HandleApplyGravity(isNotApplyGravity);

        m_Locomotion.HandleMove(m_IsNotInputMove, m_moveSpeed);
    }

    public virtual void Update()
    {
        // 2. base.Update로 상태 체크 - 각 상태에서의 변환상태 생성에 의한 보류
        //CheckLocomotion();
        m_Locomotion.HandleRotation(m_IsNotBodyRot);
    }

    public void LateUpdate() { }

    public virtual void Exit()
    {
        //애니메이션 파라미터값 적용 (종료)
        m_Locomotion.SetMainStateAnimation(m_ThisState, m_AniParmType, false); //애니메이션 파라미터 값 적용 (시작)
        
        InitializeExit();
    }

    public bool CheckComeInCurrentAni(string aniName)
    {
        if (!m_IsComeInCurrentStateAni)
        {
            m_AnimationTime = m_PlayerCore.m_AnimationManager.CheckComeInCurrentStateAni(aniName);
            if (m_AnimationTime > 0) m_IsComeInCurrentStateAni = true;
            m_GoNextStateTime = m_AnimationTime + m_GoStateDelayTime;
            return false;
        }
        return true;
    }
    #endregion ======================================== /Update, FixedUpdate, Enter, Exit
}
