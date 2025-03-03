using Godot;
using System;

public partial class FinishA2d : Area2D
{
    public async void OnBodyEntered(Node2D player){
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        GetTree().Paused = false;
        EventManager.BroadcastReturnMouse();
        GetTree().ChangeSceneToFile("res://scenes/menu/menu.tscn");
    }

    public override void _EnterTree()
    {
        BodyEntered += OnBodyEntered;
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        BodyEntered -= OnBodyEntered;
        base._ExitTree();
    }
}
