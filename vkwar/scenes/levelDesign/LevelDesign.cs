using Godot;
using System;

public partial class LevelDesign : Node2D
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
        _allowRestart = true;
    }
    private async void Restart(){
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        GetTree().ChangeSceneToFile($"res://scenes/{Name}/{Name}.tscn");
    }

    public override void _EnterTree()
    {
        EventManager.RestartAvailableLevelEvent += OnRestartAvailableLevelEvent;
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        EventManager.RestartAvailableLevelEvent-= OnRestartAvailableLevelEvent;
        base._ExitTree();
    }
}
