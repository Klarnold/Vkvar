using Godot;
using System;

public partial class DamageA2d : Area2D
{
    public void OnBodyEntered(Node2D player){
        // await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        // GetTree().ChangeSceneToFile("res://scenes/menu/menu.tscn");
        EventManager.DamagePlayerEvent(GlobalsN.damage, false);
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
