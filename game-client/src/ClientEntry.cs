using Godot;
using Game.Shared;

namespace Game.Client;

public partial class ClientEntry : Node
{
    private ENetMultiplayerPeer _peer = new();

    [Export] public string ServerIp { get; set; } = "127.0.0.1";
    [Export] public int ServerPort { get; set; } = 7777;

    public override void _Ready()
    {
        GD.Print("Client starting...");

        Multiplayer.ConnectedToServer += OnConnected;
        Multiplayer.ConnectionFailed += OnConnectionFailed;
        Multiplayer.ServerDisconnected += OnServerDisconnected;

        // Register RPCs
        Multiplayer.PeerConnected += OnPeerConnected;
    }

    public void ConnectToServer(string ip, int port)
    {
        ServerIp = ip;
        ServerPort = port;

        Error err = _peer.CreateClient(ip, port);
        if (err != Error.Ok)
        {
            GD.PushError($"Failed to create client: {err}");
            return;
        }

        Multiplayer.MultiplayerPeer = _peer;
        GD.Print($"Connecting to {ip}:{port}...");
    }

    private void OnConnected()
    {
        GD.Print("Connected to server!");
    }

    private void OnConnectionFailed()
    {
        GD.PushError("Connection failed");
    }

    private void OnServerDisconnected()
    {
        GD.Print("Server disconnected");
    }

    private void OnPeerConnected(long id)
    {
        GD.Print($"Peer connected: {id}");
    }

    [Rpc(CallLocal = true)]
    public void SyncGameState(byte[] stateBytes)
    {
        // Deserialize and apply state
        GD.Print($"Received state: {stateBytes.Length} bytes");
    }

    [Rpc(CallLocal = true)]
    public void SyncPlayCard(int playerId, int cardInstanceId, int targetTileId)
    {
        GD.Print($"Player {playerId} played card {cardInstanceId} on tile {targetTileId}");
    }

    public void RequestPlayCard(int cardInstanceId, int targetTileId)
    {
        RpcId(1, Protocol.RequestPlayCard, cardInstanceId, targetTileId);
    }
}
