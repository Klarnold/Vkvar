using Godot;
using System;
using System.Linq.Expressions;

public partial class GravityA2d : Area2D
{
    [Export] AnimatedSprite2D e_animatedS2D;
    private float _reality;
    public override void _Ready()
    {
        if (IsInGroup("wReality1")){
            _reality = 1.5f;
            e_animatedS2D.Play("real1");
            e_animatedS2D.FlipV = true;
        }
        else {
            _reality = -1;
            e_animatedS2D.Play("real2"); 
            e_animatedS2D.FlipV = false;
        }
        
        base._Ready();
    }

    public void OnBodyEntered(Node2D body){
        
        if (body.IsInGroup("gravitable"))
           body.GetType().GetMethod("Gravitable", [typeof(float)]).Invoke(body, [_reality]);
    }

    public void OnBodyExited(Node2D body){
        if (body.IsInGroup("gravitable"))
            body.GetType().GetMethod("Gravitable", [typeof(float)]).Invoke(body, [1]);
    }

    public void OnChangedRealityEvent(bool reality1){
        _reality = (float)(_reality - (float)Convert.ToInt16(_reality==1.5)/2 - (float)Convert.ToInt16(_reality==-1)/2)*-1;
        if (_reality==-1){
            e_animatedS2D.Play("real2");
            // e_animatedS2D.FlipV = false;
        }
        else{
            // e_animatedS2D.FlipV = true;
            e_animatedS2D.Play("real1");
        }
        e_animatedS2D.FlipV = !e_animatedS2D.FlipV;
        foreach (Node2D body in GetOverlappingBodies())
            body.GetType().GetMethod("Gravitable", [typeof(float)]).Invoke(body, [_reality]);
    }

    public override void _EnterTree()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
        EventManager.ChangedRealityEvent += OnChangedRealityEvent;
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        BodyEntered -= OnBodyEntered;
        BodyExited -= OnBodyExited;
        EventManager.ChangedRealityEvent -= OnChangedRealityEvent;
        base._ExitTree();
    }
}
