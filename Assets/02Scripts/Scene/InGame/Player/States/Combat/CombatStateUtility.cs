using System;
using System.Collections.Generic;
using DUS.Player.Locomotion;
using UnityEngine;

namespace DUS.Player.Combat
{
    // 전투상태일 때의 메인 상태
    public enum CombatMainState
    {
        CombatIdle = 0,         
        Aim = 1,                // 조준
        Melee = 2,              // 근접 상태
        WeaponSwap = 3,         // 무기 교체
        Reload = 4              // 장전
    }

    // 메인 전투상태 + Flags 복수개
    [Flags]
    public enum CombatSubFlags
    {
        None = 0,
        Attack = 1 << 0,            // 공격
        Charging = 1 << 1,     // 차지 공격 준비
        Executing = 1 << 2     // 처형 (피니시)
    }

    public class CombatStateUtility
    {
        public Dictionary<CombatMainState, CombatStrategyState> m_MainStrategyMap { get; private set; } = new();
        public Dictionary<CombatMainState, string[]> m_MainStateAniParmMap { get; private set; } = new()
        {
            { CombatMainState.CombatIdle, new string[]{"IsCombatIdle"} },
            { CombatMainState.Aim, new string[]{"IsAim" } },
            { CombatMainState.WeaponSwap, new string[]{"IsWeaponSwap" } },
            { CombatMainState.Reload, new string[]{"IsReload" } }
        };

        private HashSet<CombatSubFlags> m_CurrentFlagsHash = new();
        public Dictionary<CombatSubFlags, string> m_FlagAniMap { get; private set; } = new()
        {
            {CombatSubFlags.Attack, "Attack" },
            {CombatSubFlags.Charging, "IsCharging" },
            {CombatSubFlags.Executing, "IsExecuting" },
        };

        public void InitializeCreateCombat(PlayerCore player)
        {

            m_MainStrategyMap[CombatMainState.CombatIdle] = new CombatIdleState(player);
            m_MainStrategyMap[CombatMainState.Aim] = new AimState(player);
            m_MainStrategyMap[CombatMainState.WeaponSwap] = new AimState(player);
            m_MainStrategyMap[CombatMainState.Reload] = new AimState(player);
        }

        public void SetFlag(CombatSubFlags flag) => m_CurrentFlagsHash.Add(flag);
        public void Remove(CombatSubFlags flag) => m_CurrentFlagsHash.Remove(flag);
        public void HasFlag(CombatSubFlags flag) => m_CurrentFlagsHash.Contains(flag);
        public void AllClear() => m_CurrentFlagsHash.Clear();

    }
}
