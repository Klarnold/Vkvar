using Godot;
using System;
using System.Collections.Generic;

public partial class MouseA2d : Area2D
{
    private Node2D _target;
    private Node2D _tempTarget;
    private bool _targeting;
    private float _min;
    private float _tempMin;
    private List<Node2D> _enemyList;
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Hidden;
        _min = (float)Math.Pow(10, 5);
        _targeting = false;
        _target = null;
        _tempTarget = null;
        _enemyList = new List<Node2D>();
        SetCollisionMaskValue(4, GlobalsN.playerReality1);
        SetCollisionMaskValue(5, !GlobalsN.playerReality1);
        base._Ready();
    }
    public override void _Process(double delta)
    {
        Position = GetGlobalMousePosition();
        base._Process(delta);
    }
    public override void _PhysicsProcess(double delta)
    {
        // if (_targeting)
        //     if (!GlobalsN.mouseRC2D.IsColliding())
        //         checkAttackable();
        //     else{

        //     }
        base._PhysicsProcess(delta);
    }

    public void checkAttackable(){
        // _min = (GetGlobalMousePosition() - _target.Position).Length();
        // foreach (Node2D enemy in GetTree().GetNodesInGroup("attackable")){
        //     _tempMin = (GetGlobalMousePosition() - enemy.Position).Length();
        //     if (_min > _tempMin)
        //     {
        //         _min = _tempMin;
        //         _tempTarget = enemy;
        //     }
        // }
        // if (_tempTarget != null)
        //     ChangeTarget(_tempTarget);
    }

    public void ChangeTarget(Node2D _tTarget, bool tNull=false){
        // if (_target != _tTarget)
        // {
        //     if (!tNull){
        //         _target.GetType().GetMethod("SetTarget", [typeof(bool)]).Invoke(_target, [false]);
        //     }
        //     if (_tTarget != null)
        //         _tTarget.GetType().GetMethod("SetTarget", [typeof(bool)]).Invoke(_tTarget, [true]);
        //     _target = _tTarget;
        // }
        _target?.GetType().GetMethod("SetTarget", [typeof(bool)]).Invoke(_target, [false]);
        _tTarget?.GetType().GetMethod("SetTarget", [typeof(bool)]).Invoke(_tTarget, [true]);
        _target = _tTarget;
    }

    public void OnBodyEntered(Node2D body){
        body.AddToGroup("attackable");
        ChangeTarget(body);
        // if (!GlobalsN.mouseRC2D.IsColliding()){
        //     if (_target == null)
        //         ChangeTarget(body, true);
        //     _targeting = true;
        //     body.AddToGroup("attackable");
        // }
    }

    public void OnBodyExited(Node2D body){
        body.RemoveFromGroup("attackable");
        ChangeTarget(null);
        // if (_target == body){
        //     _min = (float)Math.Pow(10, 5);
        //     ChangeTarget(null);
        // }
        // body.RemoveFromGroup("attackable");
        // _targeting = GetTree().GetNodeCountInGroup("attackable") != 0; 
    }

    public void OnRealityChanged(bool reality1){
        SetCollisionMaskValue(4, reality1);
        SetCollisionMaskValue(5, !reality1);
    }

    public void OnRestartAvailableLevelEvent(){
        // await ToSignal(GetTree().CreateTimer(0.1), "timeout");
        // QueueFree();
    }

    public override void _EnterTree()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
        EventManager.RestartAvailableLevelEvent += OnRestartAvailableLevelEvent;
        EventManager.ChangedRealityEvent += OnRealityChanged;
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        BodyEntered -= OnBodyEntered;
        BodyExited -= OnBodyExited;
        EventManager.RestartAvailableLevelEvent -= OnRestartAvailableLevelEvent;
        EventManager.ChangedRealityEvent -= OnRealityChanged;
        base._ExitTree();
    }
}
