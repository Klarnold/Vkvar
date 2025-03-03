using Godot;
using Godot.NativeInterop;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;

public partial class Player : CharacterBody2D
{
    [Export] private TextureProgressBar e_healthTPB; 
    [ExportGroup("Audio")]
    [Export] AudioStreamPlayer2D e_audioStreamPlayer1;
    [Export] AudioStreamPlayer2D e_audioStreamPlayer2;
    [Export] AudioStreamPlayer2D e_attackASP2D;
    // запись и объявление констант в классе, определение экспортируемых узлов
    [ExportGroup("Animation")]
    [Export] private AnimatedSprite2D e_waveAP2D;
    [Export] private AnimationPlayer e_animationPlayer;
    [Export] private AnimatedSprite2D e_animatedSprite2D;
    [ExportGroup("Timers")]
    [Export] private Timer e_coyotJumpTimer;
    private bool _end; // костыль
    private bool _jumpHold;
    private float _gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
    private bool _invincible;
    private  int _health;
    public float antiGrav;
    // определение базовых методов с "_..."
    public override void _Ready()
    {
        _health = GlobalsN.MAX_HP;
        e_healthTPB.MaxValue = _health;
        e_healthTPB.Value = _health;
        _invincible = false;
        _end = false;
        GlobalsN.State = GlobalsN.StateMachine.MOVE;
        // GlobalsN.player = this;
        GlobalsN.playerReality1 = IsInGroup("cReality1");
        GlobalsN.direction = 0;
        GlobalsN.velocity = Vector2.Zero;
        GlobalsN.allowChangeState = true;
        _jumpHold = false;
        antiGrav = 1;
        SetCollision(); // перед SetCollision нужно менять реальность игрока
        _end = true;
        base._Ready();
    }
    public override void _Input(InputEvent @event)
    {
        if (GlobalsN.State != GlobalsN.StateMachine.DEATH){
            if (@event.IsPressed()){
                if (@event.IsAction("lmb")){
                    ChangeReality();
                }
                else if (GlobalsN.allowChangeState && @event.IsAction("rmb") && GetTree().GetFirstNodeInGroup("attackable")!=null){
                    GlobalsN.State = GlobalsN.StateMachine.ATTACK;
                }
                else if (@event.IsAction("jump") && GlobalsN.allowJump)
                {
                    _jumpHold = true;
                    // GlobalsN.velocity = Velocity;
                    // GlobalsN.velocity.Y -= 200;
                    // Velocity = GlobalsN.velocity;
                    // GD.Print(Input.GetActionStrength("jump"));
                    GlobalsN.State = GlobalsN.StateMachine.JUMP;
                }
            }
            else if (@event.IsReleased()){
                if (@event.IsAction("jump")){
                    _jumpHold = false;
                }
            }
        }
        base._Input(@event);
    }
    public override void _PhysicsProcess(double delta)
    {
    //     e_mouseRC2D.LookAt(GetGlobalMousePosition());
    //     e_mouseRC2D.Rotation -= (float)Math.PI/2;
        if (GlobalsN.allowChangeState){
            // e_mouseRC2D.TargetPosition = GetGlobalMousePosition() - Position;
            switch (GlobalsN.State){
                case GlobalsN.StateMachine.MOVE:
                    MoveState();
                    break;
                case GlobalsN.StateMachine.JUMP:
                    JumpState();
                    break;
                case GlobalsN.StateMachine.ATTACK:
                    AttackState();
                    break;
                case GlobalsN.StateMachine.DAMAGED:
                    DamagedState();
                    break;
                case GlobalsN.StateMachine.DEATH:
                    DeathState();
                    break;
            }
        }
        base._PhysicsProcess(delta);
    }
    public override void _Process(double delta)
   {
        //if (!IsOnFloor() || antiGrav==-1){
        if (!IsOnFloor() && GlobalsN.State != GlobalsN.StateMachine.DEATH){
            GlobalsN.velocity = Velocity;
            GlobalsN.velocity.Y += GlobalsN.Clamp<float>(_gravity*(float)delta*antiGrav, -1000, 1000);//GlobalsN.Clamp<float>(_gravity*(float)delta, -1000, 1000); // 
            Velocity = GlobalsN.velocity;
        }
        MoveAndSlide();
        base._Process(delta);
    }
    // определение методов-состояний
    public void MoveState(){
        GlobalsN.direction = (int)Input.GetAxis("left", "right");
        SetDirection();
        if (GlobalsN.direction != 0)
        {
            if (IsOnFloor()){
                // GlobalsN.allowJump = true;
                e_animationPlayer.Play("move");
            }
            else{
                if (e_coyotJumpTimer.TimeLeft == 0)
                    e_coyotJumpTimer.Start();
                e_animationPlayer.Play("jump");
            }
        }
        else
        {
            if (IsOnFloor()){
                e_animationPlayer.Play("idle");
            }
            else{
                e_animationPlayer.Play("jump");
            }
        }
        GlobalsN.allowJump = IsOnFloor() || GlobalsN.allowJump;
        GlobalsN.velocity = Velocity;
        GlobalsN.velocity.X = GlobalsN.speed*GlobalsN.direction;
        Velocity = GlobalsN.velocity;
    }

    public async void AttackState(){
        if (GlobalsN.allowChangeState){
            GlobalsN.allowChangeState = false;
            CharacterBody2D tempEnemy = GetTree().GetFirstNodeInGroup("attackable") as CharacterBody2D;
            float tempGravity = _gravity;
            if (tempEnemy != null){
                Position = tempEnemy.Position;
                EventManager.PlayerAttackEvent(tempEnemy);
            }
            _gravity = 0;
            Velocity = Vector2.Zero;
            e_animationPlayer.Play("attack");
            await ToSignal(GetTree().CreateTimer(0.16), "timeout");
            e_attackASP2D.Play();
            await ToSignal(GetTree().CreateTimer(0.16), "timeout");
            GlobalsN.allowChangeState = true;
            _gravity = tempGravity;
            GlobalsN.velocity = Velocity;
            GlobalsN.velocity.Y = -GlobalsN.MAX_FALL*3/4;
            Velocity = GlobalsN.velocity;
            GlobalsN.State = GlobalsN.StateMachine.MOVE;
        }
    }

    public void JumpState(){
        GlobalsN.allowJump = false;
        GlobalsN.velocity = Velocity; //
        GlobalsN.velocity.Y -= GlobalsN.MAX_FALL;//(float)(30 + Math.Pow(Velocity.Y/GlobalsN.MAX_FALL*8.5, 2));//GlobalsN.Clamp((float)(30 *Math.Exp(-GlobalsN.velocity.Y/200)), GlobalsN.MAX_FALL*2, 0);
        Velocity = GlobalsN.velocity;
        //if (Input.IsActionJustReleased("jump") || Velocity.Y <=GlobalsN.MAX_FALL)
        GlobalsN.State = GlobalsN.StateMachine.MOVE;
    }

    public void RestartState(){
        return;
    }
    public async void DamagedState(){
        if (!_invincible){
            _invincible = true;
            Velocity = new Vector2(0, -500);
            e_animationPlayer.Play("damaged");
            for (int i=0; i<8; i++)
            {
                await ToSignal(GetTree().CreateTimer(0.25f), "timeout");
                if (i == 1)
                    GlobalsN.State = GlobalsN.StateMachine.MOVE;
                Visible = !Visible;
            }
            _invincible = false;
            SetCollisionLayerValue(2 + Convert.ToInt16(IsInGroup("cReality2")), false);
            SetCollisionLayerValue(2 + Convert.ToInt16(IsInGroup("cReality2")), true);
        }
    }

    public void DeathState(){
        Visible = false;
        Velocity = Vector2.Zero;
        GlobalsN.allowChangeState = false;
        //GlobalsN.player = null;
        // RemoveFromGroup("gravitable");
        // RemoveFromGroup("cReality1");
        // RemoveFromGroup("cReality2");
        //EventManager.RestartAvailableLevelEvent();
        Input.MouseMode = Input.MouseModeEnum.Visible;
        QueueFree();
    }
    // определение методов-событий

    public void OnCoyotJumpTimerTimeout(){
        GlobalsN.allowJump = false;
    }

    // public void OnRestartTimerTimeout(){
    //     e_RestartLbl.Visible = !e_RestartLbl.Visible;
    // }

    public async void OnDamagePlayerEvent(int damage, bool directionL){
        // GlobalsN.
        if (!_invincible){
            ChangeHealthBarAnim(damage);
            _health -= damage;
            if (_health <= 0)
            {
                await ToSignal(GetTree().CreateTimer(0.1), "timeout");
                _health = 0;
                EventManager.ReturnMouse();
                GetTree().ChangeSceneToFile("res://scenes/menu/menu.tscn");
                GlobalsN.State = GlobalsN.StateMachine.DEATH;
            }
            GlobalsN.State = GlobalsN.StateMachine.DAMAGED;
        }
        // GlobalsN.State = GlobalsN.StateMachine.DEATH;
    }
    // определение внутренних методов
    public void SetDirection(){
        if (GlobalsN.direction == -1){
            e_animatedSprite2D.FlipH = true;
        }
        else if (GlobalsN.direction == 1){
            e_animatedSprite2D.FlipH = false;
        }
    }

    public async void ChangeHealthBarAnim(int damage){
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(e_healthTPB, "value", e_healthTPB.Value - damage, 0.6f);
        await ToSignal(GetTree().CreateTimer(0.6f), "timeout");
    }

    public void ChangeReality(){
        GlobalsN.playerReality1 = !GlobalsN.playerReality1;
        SetCollision();
        if (GlobalsN.playerReality1){
            e_waveAP2D.Play("real1");
            e_audioStreamPlayer2.Stop();
            e_audioStreamPlayer1.Play();
        }
        else{
            e_waveAP2D.Play("real2");
            e_audioStreamPlayer1.Stop();
            e_audioStreamPlayer2.Play();
        }

    }
    public void SetCollision(){
        try{
            if (GlobalsN.playerReality1){
                RemoveFromGroup("cReality2");
                AddToGroup("cReality1");
                EventManager.BroadcastChangedRealityEvent(true);
                SetCollisionLayerValue(2, true);
                SetCollisionLayerValue(3, false);
                SetCollisionMaskValue(6, true);
                SetCollisionMaskValue(7, false);
            }
            else{
                RemoveFromGroup("cReality1");
                AddToGroup("cReality2");
                EventManager.BroadcastChangedRealityEvent(false);
                SetCollisionLayerValue(2, false);
                SetCollisionLayerValue(3, true);
                SetCollisionMaskValue(6, false);
                SetCollisionMaskValue(7, true);
            }
        }
        catch (System.ObjectDisposedException ex){
            GD.Print(GetNode<ColorRect>("CanvasLayer/RealityCR") == null);
            GD.Print(ex.Message);
        }
    }

    public void Gravitable(float _antiGrav){
        antiGrav = _antiGrav;
    }

    public void Restart(){
        // await ToSignal(GetTree().CreateTimer(0.1), "timeout");
        GetTree().ChangeSceneToFile($"res://scenes/{GlobalsN.currentScene}/{GlobalsN.currentScene}.tscn");
    }
    // определение оставшихся методов с "_..."
    public override void _EnterTree()
    {
        e_coyotJumpTimer.Timeout += OnCoyotJumpTimerTimeout;
        EventManager.DamagePlayerEvent += OnDamagePlayerEvent;
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        e_coyotJumpTimer.Timeout -= OnCoyotJumpTimerTimeout;
        EventManager.DamagePlayerEvent -= OnDamagePlayerEvent;
        base._ExitTree();
    }
}
