using Godot;
using System;
using System.IO;

public partial class Game : Node2D
{
    private bool _allowRestart;
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Hidden;
        _allowRestart = false;
        base._Ready();
    }
    public override void _Input(InputEvent @event)
    {
        if (_allowRestart && Input.IsActionJustPressed("restart"))
            Restart();
        base._Input(@event);
    }
    public void OnRestartAvailableLevelEvent(){
        GD.Print("restart");
        _allowRestart = true;
    }
    private async void Restart(){
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        // GetTree().ChangeSceneToFile($"res://scenes/{Name}/{Name}.tscn");
        GetTree().ChangeSceneToFile($"res://scenes/game/game.tscn");
    }

    public override void _EnterTree()
    {
        EventManager.RestartAvailableLevelEvent += OnRestartAvailableLevelEvent;
        base._EnterTree();
    }

        public override void _ExitTree()
    {
        EventManager.RestartAvailableLevelEvent -= OnRestartAvailableLevelEvent;
        base._EnterTree();
    }
}
