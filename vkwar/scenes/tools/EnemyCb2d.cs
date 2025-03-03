using Godot;
using System;
using System.Net.NetworkInformation;

public partial class EnemyCb2d : CharacterBody2D
{
    [Export] private AnimatedSprite2D e_targetAS2D;
    [Export] private Area2D e_chaseA2D;
    [Export] private RayCast2D e_enemyRC2D;
    [Export] private Area2D e_hitBoxA2D;
    [ExportGroup("Animation")]
    [Export] private AnimationPlayer e_enemyAP;
    [Export] private AnimatedSprite2D e_enemyAS2D;
    private GlobalsN.eStateMachine _state;
    private float _antiGrav;
    // private float _pathSpeed;
    // private float _progress;
    private Vector2 _move;
    private bool _target;
    private bool _dead;
    private bool _forward;
    private bool _reality1; 
    private Color _tModulate;
    private Vector2 _movePosition;
    private Random _rnd;
    public GlobalsN.eStateMachine State{
        get{return _state;}
        set{
            _state = value;
        }
    }
    public override void _Ready()
    {
        _dead = false;
        _reality1 = IsInGroup("cReality1");
        if (_reality1){
            if (GlobalsN.playerReality1)
                e_enemyAP.Play("idle_r1");
            else
                e_enemyAP.Play("dissolved_r1");
            SetCollisionLayerValue(4, true);
            e_hitBoxA2D.SetCollisionMaskValue(2, true);
        }
        else{
            if (GlobalsN.playerReality1)
                e_enemyAP.Play("dissolved_r2");
            else
                e_enemyAP.Play("idle_r2");
            SetCollisionLayerValue(5, true);
            e_hitBoxA2D.SetCollisionMaskValue(3, true);
        }
        _move = new Vector2 (2, 0);
        _rnd = new Random();
        _forward = _rnd.Next(0, 2) == 1;
        e_enemyRC2D.Rotate((float)Math.PI * Convert.ToInt16(_forward));
        _target = false;
        State = GlobalsN.eStateMachine.MOVE;
        // GlobalsN.setEnemyCollision(this);
        _tModulate = Modulate;
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        switch (State){
            case GlobalsN.eStateMachine.MOVE:
                MoveState(delta);
                break;
            case GlobalsN.eStateMachine.CHASE:
                ChaseState();
                break;
            case GlobalsN.eStateMachine.ATTACK:
                AttackState();
                break;
            case GlobalsN.eStateMachine.DAMAGED:
                DamagedState();
                break;
            case GlobalsN.eStateMachine.DEATH:
                DeathState();
                break;
        }
        base._PhysicsProcess(delta);
    }

    public void MoveState(double _delta){
        Position += (_forward ? 1 : -1)*_move;
        _movePosition += _move;
        if (e_enemyRC2D.IsColliding() || _movePosition.Length() > 300)
        {
            _movePosition = Vector2.Zero;
            _forward = !_forward;
            e_enemyRC2D.Rotate((float)Math.PI);
        }
        // _progress = _pathSpeed*(float)_delta%1;
        // _enemyPF2D.ProgressRatio += 0.02f;
        // _enemyPF2D.Set("progress_ratio",  _progress); //_pathSpeed*(float)_delta
    }

    public void ChaseState(){
        
    }

    public void AttackState(){
        
    }

    public void DamagedState(){
        
    }

    public async void DeathState(){
        if (!_dead){
            _dead = true;
            e_hitBoxA2D.BodyEntered -= OnBodyEnteredHitBox;
            EventManager.ChangedRealityEvent -= OnChangedRealityEvent;
            EventManager.PlayerAttackEvent -= OnPlayerAttackEvent;
            if (_reality1){
                SetCollisionLayerValue(4, false);
                e_hitBoxA2D.SetCollisionMaskValue(2, false);
            }
            else{
                SetCollisionLayerValue(5, false);
                e_hitBoxA2D.SetCollisionMaskValue(3, false);
            }
            await ToSignal(GetTree().CreateTimer(0.3125), "timeout");
            if (_reality1)
                e_enemyAP.Play("death_r1");
            else
                e_enemyAP.Play("death_r2");

            await ToSignal(GetTree().CreateTimer(1f), "timeout");
            QueueFree();
        }
    }

    public void OnBodyEnteredHitBox(Node2D player){
        EventManager.BroadcastDamagePlayerEvent(GlobalsN.damage, false);
    }

    public void OnChangedRealityEvent(bool real1){
        if (_reality1)
        {
            if (real1)
                e_enemyAP.Play("idle_r1");
            else
                e_enemyAP.Play("dissolved_r1");
        }
        else{
            if (real1)
                e_enemyAP.Play("dissolved_r2");
            else
                e_enemyAP.Play("idle_r2");
        }
    }

    public void OnPlayerAttackEvent(CharacterBody2D enemy){
        if (this == enemy){
            _state = GlobalsN.eStateMachine.DEATH;
        }
    }

    public void Gravitable(float _gravity){
        _antiGrav = _gravity;
    }

    public void SetTarget(bool _nTarget){
        _tModulate.A = 255*Convert.ToInt16(_nTarget);
        _target = _nTarget;
        e_targetAS2D.Modulate = _tModulate;
        if (_nTarget)
            e_targetAS2D.Play("default");
        else   
            e_targetAS2D.Stop();
        // if (_target){
        //     _targetAS2D.Set("modulate:a", 255);
        // }
        // else{
        //     _targetAS2D.Set("modulate:a", 0);
        // }
    }

    public override void _EnterTree()
    {
        e_hitBoxA2D.BodyEntered += OnBodyEnteredHitBox;
        EventManager.ChangedRealityEvent += OnChangedRealityEvent;
        EventManager.PlayerAttackEvent += OnPlayerAttackEvent;
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        if (_state != GlobalsN.eStateMachine.DEATH){
            e_hitBoxA2D.BodyEntered -= OnBodyEnteredHitBox;
            EventManager.ChangedRealityEvent -= OnChangedRealityEvent;
            EventManager.PlayerAttackEvent -= OnPlayerAttackEvent;
        }
        base._ExitTree();
    }
}
