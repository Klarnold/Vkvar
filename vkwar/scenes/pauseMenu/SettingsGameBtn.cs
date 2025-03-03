using Godot;
using System;

public partial class SettingsGameBtn : Button
{
    public void OnPressed(){
        GetParent().GetParent().GetParent().GetParent().GetNode<Godot.Control>("Settings").Visible = true;
        GetParent().GetParent().GetParent<Godot.Control>().Visible = false;
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
