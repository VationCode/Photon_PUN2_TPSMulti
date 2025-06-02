using DUS.PlayerCore.Locomotion;

public class ClimbState : LocomotionStrategyState
{
    public ClimbState(PlayerCore playerCore) : base(playerCore) { }

    protected override LocomotionMainState DetermineStateType() => LocomotionMainState.Climb;

    protected override AniParmType[] SetAniParmType() => new AniParmType[] { AniParmType.SetBool };

    protected override float SetMoveSpeed() => m_PlayerCore.m_ClimbSpeed;
}
