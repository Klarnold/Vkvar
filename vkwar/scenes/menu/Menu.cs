using Godot;
using System;

public partial class Menu : Node2D
{
    [Export] private AudioStreamPlayer2D e_audioPlayer2D;
    [Export] private ParallaxBackground e_parallaxBG;

    private int _speed;
    private Vector2 _offset;
    public override void _Ready()
    {
        if (!e_audioPlayer2D.Playing)
            e_audioPlayer2D.Play();
        _offset = e_parallaxBG.ScrollOffset;
        _speed = 200;
        base._Ready();
    }
    public override void _PhysicsProcess(double delta)
    {
        _offset.X -= _speed*(float)delta;
        e_parallaxBG.Set("scroll_offset", _offset); 
        base._PhysicsProcess(delta);
    }
}
