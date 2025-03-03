using Godot;
using System;

public partial class SettingsBtn : Button
{
    [Export] Godot.Control e_settings;
    [Export] Godot.Control e_mainMenu;
    public override void _Pressed()
    {
        GetTree().Paused = true;
        e_mainMenu.Visible = false;
        e_settings.Visible = true;
        base._Pressed();
    }
}
