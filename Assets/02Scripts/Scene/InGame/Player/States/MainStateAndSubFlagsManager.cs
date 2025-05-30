using System;
using UnityEngine;


public enum AbilityMainState
{
    None = 0,
    UsingAbility = 1,
    UsingUltimate = 2
}


#region ======================================== Interaction
/// <summary>
/// 상호작용 상태
/// </summary>
[Flags]
public enum InteractionFlags
{
    None = 0,
    LootingItem = 1 << 0,    // 아이템 획득
    OpeningChest = 1 << 1,   // 상자 열기
    Trading = 1 << 2,        // 거래
    Crafting = 1 << 3,       // 제작
    Upgrading = 1 << 4       // 장비 강화
}
#endregion ======================================== /Interaction

public class MainStateAndSubFlagsManager : MonoBehaviour
{
    
    public AbilityMainState m_AbilityMain
    {
        get => m_abilityMain;
        set => m_abilityMain = value;
    }

    public InteractionFlags m_InteractionFlags
    {
        get => m_interactionFlags;
        set => m_interactionFlags = value;
    }

    #region ======================================== Combat
    /*private CombatMainState m_combatMain = CombatMainState.CombatIdleState;
    private CombatSubFlags m_CombatFlags = CombatSubFlags.CombatIdleState;
    public void SetCombatFlag(CombatSubFlags flag) => m_CombatFlags |= flag;
    public bool HasCombatFlag(CombatSubFlags flag) => (m_CombatFlags & flag) != 0;
    public void ClearCombatFlag(CombatSubFlags flag) => m_CombatFlags &= ~flag;*/
    #endregion ======================================== /Combat

    #region ======================================== Ability
    private AbilityMainState m_abilityMain { get; set; } = AbilityMainState.None;
    #endregion ======================================== /Ability

    #region ======================================== Interaction
    private InteractionFlags m_interactionFlags = InteractionFlags.None;
    public void SetInteractionFlag(InteractionFlags flag) => m_interactionFlags |= flag;
    public bool HasInteractionFlag(InteractionFlags flag) => (m_interactionFlags & flag) != 0;
    public void ClearInteractionFlag(InteractionFlags flag) => m_interactionFlags &= ~flag;
    #endregion ======================================== /Interaction

}