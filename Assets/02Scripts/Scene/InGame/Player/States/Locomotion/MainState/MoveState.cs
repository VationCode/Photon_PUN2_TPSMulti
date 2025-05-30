using DUS.Player.Locomotion;

public class MoveState : LocomotionStrategyState
{
    public MoveState(PlayerCore playerCore) : base(playerCore) { }

    protected override LocomotionMainState DetermineStateType() => LocomotionMainState.Move;
    protected override AniParmType[] SetAniParmType() => new AniParmType[] { AniParmType.SetBool };
    protected override float SetMoveSpeed() => m_PlayerCore.m_WalkSpeed;
    public override void Enter()
    {
        base.Enter();
    }
    public override void FixedUpdate()
    {
        // 1. UpdateMovement 동작
        base.FixedUpdate();
    }
    public override void Update()
    {
        // 애니메이션 및 변환 상태 생성
        base.Update();

        //Flags && Speed 관리
        bool isRun = m_PlayerCore.m_InputManager.m_IsRun_LocoF;
        bool isCrouch = m_PlayerCore.m_InputManager.m_IsCrouch_LocoF;
        bool isMove = m_PlayerCore.m_InputManager.m_IsMove_LocoM; // 이동 입력 여부
        bool isJump = m_PlayerCore.m_InputManager.m_IsJump_LocoM;

        // 3. 5가 되기전의 이전 SubFlag 상태 체크
        bool hasRun = m_Locomotion.m_StateUtility.HasLocomotionFlag(LocomotionSubFlags.Run);
        bool hasCrouch = m_Locomotion.m_StateUtility.HasLocomotionFlag(LocomotionSubFlags.Crouch);
        bool hasCrouchRun = m_Locomotion.m_StateUtility.HasLocomotionFlag(LocomotionSubFlags.CrouchRun);

        // 4. 각 진입 조건 
        if (!hasCrouch && hasRun && isCrouch)             // 달리기 중 앉기 = 슬라이드
        {
            // 슬라이드 시작
            m_Locomotion.SetNextState(LocomotionMainState.Slide);
            m_Locomotion.HandleCheckFlags(LocomotionSubFlags.Crouch, isCrouch);
            return;
        }

        if (isRun && isCrouch)       // 앉기 중 달리기 = 앉으며 달리기
        {
            m_moveSpeed = m_PlayerCore.m_CrouchRunSpeed;
            hasCrouchRun = true;
        }
        else if (isRun)          // 일반 달리기
        {
            m_moveSpeed = m_PlayerCore.m_RunSpeed;
            hasCrouchRun = false;
        }
        else if (isCrouch)       // 일반 앉기
        {
            m_moveSpeed = m_PlayerCore.m_CrouchSpeed;
            hasCrouchRun = false;
        }
        else
        {
            m_moveSpeed = m_PlayerCore.m_WalkSpeed;
            hasCrouchRun = false;
        }

        // 5. SubFlag 업데이트
        m_Locomotion.HandleCheckFlags(LocomotionSubFlags.CrouchRun, hasCrouchRun);
        m_Locomotion.HandleCheckFlags(LocomotionSubFlags.Run, isRun);
        m_Locomotion.HandleCheckFlags(LocomotionSubFlags.Crouch, isCrouch);


        // ChangeMain
        if (isJump)
        {
            m_Locomotion.SetNextState(LocomotionMainState.Jump);
        }
        else if (!isMove)
        {
            m_Locomotion.SetNextState(LocomotionMainState.Idle);
        }
    }
    public override void Exit()
    {
        base.Exit();
        m_Locomotion.AllClearFlag();
    }
}