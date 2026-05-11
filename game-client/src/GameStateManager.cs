using Godot;

namespace Game.Client;

public partial class GameStateManager : Node
{
    public static GameStateManager Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        ProcessMode = ProcessModeEnum.Always;
    }
}
