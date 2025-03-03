using Godot;
using System;

public partial class ExitMenuBtn : Button
{
    public override void _Pressed()
    {
        GD.Print(GetParent().Name);
        GetParent().GetParent<Godot.Control>().Visible = !GetParent().GetParent<Godot.Control>().Visible;
        GetParent().GetParent().GetParent().GetNode<Godot.Control>("PauseMenu").Visible = true;
        GetTree().Paused = GetParent().GetParent().GetParent().GetNode<Godot.Control>("PauseMenu").ProcessMode == ProcessModeEnum.WhenPaused;
        // EventManager.BroadcastReturnMouse();
        base._Pressed();
    }
}
