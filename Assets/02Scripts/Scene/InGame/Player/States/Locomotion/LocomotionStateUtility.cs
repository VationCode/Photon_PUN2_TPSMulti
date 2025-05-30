// MainState : 단일 상태 유지
// SubFlags : 복수 상태 가능
// Main + Sub

using System;
using System.Collections.Generic;
namespace DUS.Player.Locomotion
{
    public enum LocomotionMainState
    {
        Idle = 0,           // Idle
        Move = 1,           // 기본 이동
        Jump = 2,          // 점프
        InAir = 3,          // 공중 (점프/낙하)
        Land = 4,           // 착지
        Dodge = 5,          // 구르기(회피기)
        Slide = 6,          // 슬라이딩
        Climb = 7,          // 등반
        WallRun = 8,         // 벽 달리기
        Staggered = 9,         // 피격 경직 같은 상태
        Knockback = 10,         // 넉백
    }

    /// <summary>
    /// Flags는 복수의 열거형 선택이 가능한 기능. 즉, 복수 상태가 가능
    /// Flags는 2의 제곱수나 2의 제곱수 조합을 사용하여 선언해야함
    /// CombatIdleState =0, FalgButtonGroupManager = 1, Croucning = 2, 4, 8 이런식 보다는 쉬프트연산)
    /// </summary>
    [Flags]
    public enum LocomotionSubFlags
    {
        None = 0,
        Run = 1 << 1,           // 달리기
        Crouch = 1 << 2,        // 앉기
        CrouchRun = 1 << 3      // 앉아서 달리기 
    }

    public class LocomotionStateUtility
    {
        #region ======================================== MainState 관리
        public Dictionary<LocomotionMainState, LocomotionStrategyState> m_MainStrategyMap { get; private set; } = new();
        //SetBool = 0번 인덱스, SetTrigger = 1번
        public Dictionary<LocomotionMainState, string[]> m_MainStateAniParmMap { get; private set; } = new()
        {
            { LocomotionMainState.Idle, new string[]{"IsIdle" } },
            { LocomotionMainState.Move, new string[]{"IsMove" } },
            { LocomotionMainState.Jump, new string[]{"IsJump","Jump"} },
            { LocomotionMainState.InAir, new string[]{"IsInAir" } },
            { LocomotionMainState.Land, new string[]{"IsLand" } },
            { LocomotionMainState.Slide, new string[]{"IsSlide", "Slide" } },
            { LocomotionMainState.Climb, new string[]{"IsClimb" } },
            { LocomotionMainState.WallRun, new string[]{"IsWallRun" } }
        };
        public void InitializeCreateMainStateMap(PlayerCore player)
        {
            m_MainStrategyMap[LocomotionMainState.Idle] = new IdleState(player);
            m_MainStrategyMap[LocomotionMainState.Move] = new MoveState(player);
            m_MainStrategyMap[LocomotionMainState.Jump] = new JumpState(player);
            m_MainStrategyMap[LocomotionMainState.InAir] = new InAirState(player);
            m_MainStrategyMap[LocomotionMainState.Land] = new LandState(player);
            m_MainStrategyMap[LocomotionMainState.Slide] = new SlideState(player);
            m_MainStrategyMap[LocomotionMainState.Climb] = new ClimbState(player);
            m_MainStrategyMap[LocomotionMainState.WallRun] = new WallRunState(player);
        }
        
        #endregion ======================================== /MainSate 관리

        #region ======================================== SubFlags 관리
        private HashSet<LocomotionSubFlags> m_CurrentFlagsHash = new();
        public Dictionary<LocomotionSubFlags, string> m_FlagAniMap { get; private set; } = new()
        {
            { LocomotionSubFlags.None, "IsNone" },
            { LocomotionSubFlags.Run, "IsRun" },
            { LocomotionSubFlags.Crouch, "IsCrouch" },
            { LocomotionSubFlags.CrouchRun, "IsCrouchRun" },
        };

        //HashSet은 Add 중복 자동 방지
        /// Flag + Ani 모두 변경
        public void SetLocomotionFlag(LocomotionSubFlags flag) => m_CurrentFlagsHash.Add(flag);
        public void RemoveLocomotionFlag(LocomotionSubFlags flag) => m_CurrentFlagsHash.Remove(flag);
        public bool HasLocomotionFlag(LocomotionSubFlags flag) => m_CurrentFlagsHash.Contains(flag);
        public void AllClearFlags() => m_CurrentFlagsHash.Clear();
        
        #endregion ======================================== /SubFlags 관리
    }
}
