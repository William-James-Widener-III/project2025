namespace Game.Shared;

public static class Protocol
{
    // RPC method names for Godot MultiplayerAPI
    public const string RequestPlayCard = nameof(RequestPlayCard);
    public const string SyncPlayCard = nameof(SyncPlayCard);
    public const string RequestMoveUnit = nameof(RequestMoveUnit);
    public const string SyncMoveUnit = nameof(SyncMoveUnit);
    public const string RequestAttack = nameof(RequestAttack);
    public const string SyncAttack = nameof(SyncAttack);
    public const string SyncGameState = nameof(SyncGameState);
    public const string PlayerConnected = nameof(PlayerConnected);
    public const string PlayerDisconnected = nameof(PlayerDisconnected);
    public const string StartGame = nameof(StartGame);
}
