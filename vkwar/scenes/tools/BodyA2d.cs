using Godot;
using System;

public partial class BodyA2d : Area2D
{
    public override void _Ready()
    {
        // InputEventMouseButton
        base._Ready();
    }

    public void OnMouseBodyA2dEntered(){
        AddToGroup("chosen");
        // GD.Print("entered");
    }

    public void OnMouseBodyA2dExited(){
        RemoveFromGroup("chosen");
        // GD.Print("exited");
    }

    public override void _EnterTree()
    {
        MouseEntered += OnMouseBodyA2dEntered;
        MouseExited += OnMouseBodyA2dExited;
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        MouseEntered -= OnMouseBodyA2dEntered;
        MouseExited -= OnMouseBodyA2dExited;
        base._ExitTree();
    }
}
