using Godot;
using System;

public partial class Control : Godot.Control
{
    public override void _Ready()
    {
        // MouseEntered += OnMouseEntered;
        // MouseExited += OnMouseExited;
        base._Ready();
    }

    public void OnMouseEntered(){
        GD.Print("in_control");
    }

    public void OnMouseExited(){
        GD.Print("out_of_control");
    }
}
