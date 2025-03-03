using Godot;
using System;

public partial class TextWindow2d : Node2D
{
    [Export] private RichTextLabel _richTL;
    [Export] private Timer _letterTimer;
    private Vector2 _scale;
    public override void _Ready()
    {
        _scale = GlobalsN.scale;
        Scale = Vector2.Zero;
        _richTL.VisibleCharacters = 0;
        base._Ready();
    }

    public void OnTimerTimeout(){
        _richTL.VisibleCharacters += 1;
    }

    public async void _Show(){
        GD.Print("show");
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(this, "scale", _scale, 0.5f);
        _letterTimer.Start();
        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
        Scale = _scale;
    }

    public void _Hide(){
        GD.Print("hide");
        _richTL.VisibleCharacters = 0;
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(this, "scale", Vector2.Zero, 0.5f);
        _letterTimer.Stop();
    }

    public override void _EnterTree()
    {
        _letterTimer.Timeout += OnTimerTimeout;
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        _letterTimer.Timeout -= OnTimerTimeout;
        base._ExitTree();
    }
}
