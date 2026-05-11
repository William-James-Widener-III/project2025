namespace Game.Shared;

public abstract record PlayerAction
{
    public int PlayerId { get; init; }
    public long Timestamp { get; init; }
}

public record MoveUnitAction : PlayerAction
{
    public int UnitId { get; init; }
    public int TargetTileId { get; init; }
}

public record PlayCardAction : PlayerAction
{
    public int CardInstanceId { get; init; }
    public int? TargetTileId { get; init; }
}

public record AttackAction : PlayerAction
{
    public int AttackerUnitId { get; init; }
    public int TargetTileId { get; init; }
    public bool AttackLocation { get; init; }
}

public record EndTurnAction : PlayerAction
{
}

public record DeployTrapAction : PlayerAction
{
    public int CardInstanceId { get; init; }
    public int TargetTileId { get; init; }
}
