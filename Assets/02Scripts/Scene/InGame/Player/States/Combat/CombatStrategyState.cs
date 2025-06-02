using DUS.PlayerCore.Combat;

public abstract class CombatStrategyState : IBaseState
{
    protected PlayerCore m_PlayerCore;
    protected PlayerCombat m_Combat;

    public CombatStrategyState(PlayerCore playerCore)
    {
        m_PlayerCore = playerCore;
        m_Combat = m_PlayerCore.m_Combat;
    }

    public abstract void Enter();
    public void FixedUpdate(){}
    public abstract void Update();
    public void LateUpdate(){}
    public abstract void Exit();


}
