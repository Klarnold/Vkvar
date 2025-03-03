using Godot;
using System;

public partial class InteractiveA2d : Area2D
{
    [Export] private Node2D _textWindow2D;
    public override void _Ready()
    {
        base._Ready();
    }

    public void OnBodyEntered(Node2D player){
        _textWindow2D.GetType().GetMethod("_Show", new Type[] { }).Invoke(_textWindow2D, null);
    }

    public void OnBodyExited(Node2D player){
        _textWindow2D.GetType().GetMethod("_Hide", new Type[] { }).Invoke(_textWindow2D, null);
    }

    public override void _EnterTree()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        BodyEntered -= OnBodyEntered;
        BodyExited -= OnBodyExited;
        base._ExitTree();
    }
}
