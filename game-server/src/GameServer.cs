using Godot;
using Game.Shared;
using System.Collections.Generic;

namespace Game.Server;

public partial class GameServer : Node
{
    private ENetMultiplayerPeer _peer = new();
    private Dictionary<long, int> _peerToPlayerId = new();
    private GameState _gameState = new();

    [Export] public int Port { get; set; } = 7777;
    [Export] public int MaxPlayers { get; set; } = 6;

    public override void _Ready()
    {
        if (!OS.HasFeature("dedicated_server"))
        {
            GD.Print("Not running as dedicated server. Use --headless flag.");
            return;
        }

        StartServer();
    }

    private void StartServer()
    {
        Error err = _peer.CreateServer(Port, MaxPlayers + 1); // +1 for Puppet Master
        if (err != Error.Ok)
        {
            GD.PushError($"Failed to start server: {err}");
            GetTree().Quit();
            return;
        }

        Multiplayer.MultiplayerPeer = _peer;
        Multiplayer.PeerConnected += OnPeerConnected;
        Multiplayer.PeerDisconnected += OnPeerDisconnected;

        GD.Print($"Dedicated server listening on port {Port}");
        GD.Print($"Max players: {MaxPlayers}");
    }

    private void OnPeerConnected(long peerId)
    {
        GD.Print($"Peer connected: {peerId}");

        // Assign player ID
        int playerId = _peerToPlayerId.Count;
        _peerToPlayerId[peerId] = playerId;

        // Notify all clients
        Rpc(Protocol.PlayerConnected, peerId, playerId);

        // If full, start game
        if (_peerToPlayerId.Count == MaxPlayers)
        {
            StartGame();
        }
    }

    private void OnPeerDisconnected(long peerId)
    {
        GD.Print($"Peer disconnected: {peerId}");
        if (_peerToPlayerId.TryGetValue(peerId, out int playerId))
        {
            Rpc(Protocol.PlayerDisconnected, playerId);
            _peerToPlayerId.Remove(peerId);
        }
    }

    private void StartGame()
    {
        GD.Print("Starting game!");
        Rpc(Protocol.StartGame);

        // Initialize game state
        _gameState.TurnNumber = 1;
        _gameState.CurrentPlayerId = 0;
        _gameState.CurrentPhase = Phase.Draw;
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false)]
    public void RequestPlayCard(int cardInstanceId, int targetTileId)
    {
        long senderPeer = Multiplayer.GetRemoteSenderId();
        if (!_peerToPlayerId.TryGetValue(senderPeer, out int playerId))
        {
            GD.PushError($"Unknown peer: {senderPeer}");
            return;
        }

        // Validate: is it this player's turn?
        if (_gameState.CurrentPlayerId != playerId)
        {
            GD.PushError($"Not player {playerId}'s turn");
            return;
        }

        // TODO: Validate mana, card in hand, target valid

        GD.Print($"Player {playerId} playing card {cardInstanceId} -> tile {targetTileId}");

        // Broadcast to all clients
        Rpc(Protocol.SyncPlayCard, playerId, cardInstanceId, targetTileId);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false)]
    public void RequestMoveUnit(int unitId, int targetTileId)
    {
        long senderPeer = Multiplayer.GetRemoteSenderId();
        if (!_peerToPlayerId.TryGetValue(senderPeer, out int playerId))
            return;

        // TODO: Validate unit ownership, movement range, path clear

        Rpc(Protocol.SyncMoveUnit, playerId, unitId, targetTileId);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false)]
    public void RequestAttack(int attackerUnitId, int targetTileId, bool attackLocation)
    {
        long senderPeer = Multiplayer.GetRemoteSenderId();
        if (!_peerToPlayerId.TryGetValue(senderPeer, out int playerId))
            return;

        // TODO: Validate range, attack availability, apply Constitution combat rules

        Rpc(Protocol.SyncAttack, playerId, attackerUnitId, targetTileId, attackLocation);
    }
}
