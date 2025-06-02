using DUS.PlayerCore.Locomotion;

public class WallRunState : LocomotionStrategyState
{
    public WallRunState(PlayerCore playerCore) : base(playerCore){}

    protected override LocomotionMainState DetermineStateType() => LocomotionMainState.WallRun;

    protected override AniParmType[] SetAniParmType() => new AniParmType[] { AniParmType.SetBool };
    protected override float SetMoveSpeed() => m_PlayerCore.m_WallRunSpeed;
}
