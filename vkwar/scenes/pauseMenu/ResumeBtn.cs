using Godot;
using System;

public partial class ResumeBtn : Button
{
    public override void _Pressed()
    {
        EventManager.BroadcastResumeButtonPressedEvent();
        EventManager.BroadcastReturnMouse();
        base._Pressed();
    }
}
