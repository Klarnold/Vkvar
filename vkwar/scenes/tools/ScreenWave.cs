using Godot;
using System;

public partial class ScreenWave : Node2D
{
    [Export] private AnimatedSprite2D[] e_listAS2D;
    public override void _Ready()
    {
        OnChangedRealityEvent(GlobalsN.playerReality1);
        base._Ready();
    }

    public void OnChangedRealityEvent(bool real1){
        if (real1){
            foreach (AnimatedSprite2D anim in e_listAS2D)
                anim.Play("real1");
        }
        else{
            foreach (AnimatedSprite2D anim in e_listAS2D)
                anim.Play("real2");
        }
    }
    public override void _EnterTree()
    {
        EventManager.ChangedRealityEvent += OnChangedRealityEvent;
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        EventManager.ChangedRealityEvent -= OnChangedRealityEvent;
        base._ExitTree();
    }
}
