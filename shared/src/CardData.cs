namespace Game.Shared;

public record CardData
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public int ManaCost { get; init; }
    public int MoneyCost { get; init; }
    public CardType Type { get; init; }

    // Minion stats
    public int Attack { get; init; }
    public int Health { get; init; }
    public int Speed { get; init; } = 1;
    public int Range { get; init; } = 1;
    public int SpawnpointThreshold { get; init; }

    // Keywords
    public bool HasRanged { get; init; }
    public bool HasSpawnpoint { get; init; }
    public bool IsUgly { get; init; }
    public bool IsMartyrs { get; init; }
    public bool IsJailable { get; init; }
    public bool IsSleepable { get; init; }
    public bool IsMutable { get; init; }
}

public enum CardType
{
    Minion,
    Spell,
    Trap
}
