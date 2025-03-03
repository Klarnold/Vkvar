using Godot;
using System;

public partial class ExitBtn : Button
{
    public override void _Ready()
    {
        base._Ready();
    }
    public async void OnPressed(){
        await ToSignal(GetTree().CreateTimer(0.1), "timeout");
        GetTree().Quit();
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
