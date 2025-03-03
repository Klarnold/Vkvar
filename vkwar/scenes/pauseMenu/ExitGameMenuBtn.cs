using Godot;
using System;

public partial class ExitGameMenuBtn : Button
{
    public override async void _Pressed()
    {
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://scenes/menu/menu.tscn");
        base._Pressed();
    }
}
