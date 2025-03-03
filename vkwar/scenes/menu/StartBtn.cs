using Godot;
using System;

public partial class StartBtn : Button
{
    public override void _Ready()
    {
        base._Ready();
    }

    public async void OnPressed(){
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        GetTree().ChangeSceneToFile("res://scenes/levelDesign/level_design.tscn");
    }

    public override void _EnterTree()
    {
        Pressed += OnPressed;
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        Pressed -= OnPressed;
        base._ExitTree();
    }
}
