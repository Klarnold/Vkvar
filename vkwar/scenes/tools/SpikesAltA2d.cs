using Godot;
using System;

public partial class SpikesAltA2d : Area2D
{
    [Export] Sprite2D e_spikesS2D;
    public override void _Ready(){
        if (GlobalsN.playerReality1)
            e_spikesS2D.Texture = GD.Load<Texture2D>("res://assets/aseprite/SpikesAltDissolved.png");
        base._Ready();
    }

    public void OnBodyEntered(Node2D player){
        // await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        // GetTree().ChangeSceneToFile("res://scenes/menu/menu.tscn");
        EventManager.DamagePlayerEvent(GlobalsN.damage, false);
    }

    public void OnChangedRealityEvent(bool reality1){
        e_spikesS2D.Texture = reality1 ? GD.Load<Texture2D>("res://assets/aseprite/SpikesAltDissolved.png") : GD.Load<Texture2D>("res://assets/aseprite/SpikesAlt.png");
    }

    public override void _EnterTree()
    {
        BodyEntered += OnBodyEntered;
        EventManager.ChangedRealityEvent += OnChangedRealityEvent;
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        BodyEntered -= OnBodyEntered;
        EventManager.ChangedRealityEvent-= OnChangedRealityEvent;
        base._ExitTree();
    }
}
