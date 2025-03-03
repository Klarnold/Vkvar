using Godot;
using System;
using System.Transactions;

public partial class PlatformSb2d : StaticBody2D
{
    [Export] private Area2D e_checkA2D;
    [Export] private Sprite2D e_S2D;
    [Export] private AnimatedSprite2D e_animatedSprite2D;
    [Export] private AnimationPlayer e_animationPlayer;
    [Export] private Texture2D[] e_platformTextures;
    private bool _playerIn;
    private bool _currentReality1; // реальность платформы - wReality1?
    private bool _finalReality1; // финальная реальность игрока - cReality2?
    public override void _Ready()
    {
        _finalReality1 = GlobalsN.playerReality1;
        _playerIn = false;
        e_S2D.Scale = GlobalsN.scale;
        e_animatedSprite2D.Visible = GlobalsN.playerReality1 != IsInGroup("wReality1") && (IsInGroup("wReality1") != IsInGroup("wReality2"));
        CheckModulate(_finalReality1);
        if (!IsInGroup("wReality1") && !IsInGroup("wReality2"))
            e_animatedSprite2D.Visible = false;
        base._Ready();
    }

    public void OnCheckAreaBodyEntered(Node2D player){
        SetCollisionLayerValue(7-Convert.ToInt16(_currentReality1), false);
        _playerIn = true;
    }
    public void OnChangedRealityEvent(bool reality1){
        _finalReality1 = reality1;
        CheckModulate(_finalReality1);
    }
    public void CheckModulate(bool reality){
        e_animatedSprite2D.Visible = !e_animatedSprite2D.Visible;
        if (!_playerIn){
            e_S2D.Texture = e_platformTextures[Convert.ToInt16(!_currentReality1)*2 + 1 + Convert.ToInt16(reality != _currentReality1)];
            if (IsInGroup("wReality1"))
            {
                if (reality)
                    e_animationPlayer.Stop();
                else
                    e_animationPlayer.Play("dissolved1");
            }
            else if (IsInGroup("wReality2"))
                if (reality)
                    e_animationPlayer.Play("dissolved2");
                else
                    e_animationPlayer.Stop();
            else
                e_S2D.Texture = e_platformTextures[0];
            // if (reality == _currentReality1)
            //     Modulate = _modulate;
            // else{
            //     Modulate = _finalModulate;
            // }
        }
    }

    public void OnCheckAreaBodyExited(Node2D player){
        _playerIn = false;
        SetCollisionLayerValue(7-Convert.ToInt16(_currentReality1), true);
        CheckModulate(_finalReality1);
    }

    public override void _EnterTree()
    {
        if (IsInGroup("wReality1"))
        {
            SetCollisionLayerValue(1, false);
            SetCollisionLayerValue(6, true);
            e_S2D.Texture = e_platformTextures[1];
            e_checkA2D.BodyEntered += OnCheckAreaBodyEntered;
            e_checkA2D.BodyExited += OnCheckAreaBodyExited;
            _currentReality1 = true;
            EventManager.ChangedRealityEvent += OnChangedRealityEvent;
        }
        else if (IsInGroup("wReality2")){
            SetCollisionLayerValue(1, false);
            SetCollisionLayerValue(7, true);
            e_S2D.Texture = e_platformTextures[2];
            e_checkA2D.BodyEntered += OnCheckAreaBodyEntered;
            e_checkA2D.BodyExited += OnCheckAreaBodyExited;
            _currentReality1 = false;
            EventManager.ChangedRealityEvent += OnChangedRealityEvent;
        }
        else{
            // GD.Print(e_platformTextures[0].ResourceName);
            e_S2D.Texture = GD.Load<Texture2D>("res://assets/aseprite/PLSolidSolidt.png");
        }
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        if (IsInGroup("wReality1"))
        {
            e_checkA2D.BodyEntered -= OnCheckAreaBodyEntered;
            e_checkA2D.BodyExited -= OnCheckAreaBodyExited;
            EventManager.ChangedRealityEvent -= OnChangedRealityEvent;
        }
        else if (IsInGroup("wReality2")){
            e_checkA2D.BodyEntered -= OnCheckAreaBodyEntered;
            e_checkA2D.BodyExited -= OnCheckAreaBodyExited;
            EventManager.ChangedRealityEvent -= OnChangedRealityEvent;
        }
        base._ExitTree();
    }
}
