using Godot;
using System;

public partial class WindowCheckBox : CheckBox
{
    public override void _Ready()
    {
        ButtonPressed = GetWindow().Mode == Window.ModeEnum.Fullscreen;
        base._Ready();
    }
    public override void _Pressed()
    {
        if (GetWindow().Mode == Window.ModeEnum.Fullscreen)
        {
            GetWindow().Mode = Window.ModeEnum.Windowed;
            GetWindow().Size = GlobalsN.screenResolution;
        }
        else
            GetWindow().Mode = Window.ModeEnum.Fullscreen;
        base._Pressed();
    }
}
