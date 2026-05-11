using System.Collections.Generic;

namespace Game.Shared;

public record GameState
{
    public int TurnNumber { get; set; }
    public int CurrentPlayerId { get; set; }
    public Phase CurrentPhase { get; set; }
    public List<PlayerState> Players { get; set; } = new();
    public List<TileState> Tiles { get; set; } = new();
    public List<UnitState> Units { get; set; } = new();
}

public record PlayerState
{
    public int PlayerId { get; set; }
    public int Mana { get; set; }
    public int Money { get; set; }
    public bool IsPuppetMaster { get; set; }
    public bool IsAlive { get; set; } = true;
    public List<int> HandCardIds { get; set; } = new();
    public List<int> DeckCardIds { get; set; } = new();
}

public record TileState
{
    public int TileId { get; set; }
    public TileType Type { get; set; }
    public int OwnerId { get; set; } = -1;
    public int? OccupantUnitId { get; set; }
    public List<int> NeighborTileIds { get; set; } = new();
    public int OccupationIndex { get; set; }
}

public record UnitState
{
    public int UnitId { get; set; }
    public int PlayerId { get; set; }
    public int CardDataId { get; set; }
    public int CurrentHealth { get; set; }
    public int CurrentAttack { get; set; }
    public int CurrentTileId { get; set; }
    public bool HasMoved { get; set; }
    public bool HasAttacked { get; set; }
    public bool IsJailed { get; set; }
    public bool IsSleeping { get; set; }
    public bool IsMuted { get; set; }
}

public enum Phase
{
    Draw,
    Resource,
    Main,
    Movement,
    Combat,
    End
}

public enum TileType
{
    Neutral,
    MainBase,
    ManaBank,
    MoneyBank,
    Barracks,
    SuperBoss,
    Objective
}
