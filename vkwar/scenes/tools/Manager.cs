using Godot;
using System;

public partial class Manager : Node
{
    [Export] Godot.Control e_pauseMenu;
    private bool _gamePaused;

    public override void _Ready()
    {
        _gamePaused = false;
        base._Ready();
    }
    public override void _Input(InputEvent @event)
    {
        // if (@event.IsActionPressed("escape"))
        if (@event.IsActionPressed("escape") && !e_pauseMenu.GetParent().GetNode<Godot.Control>("Settings").Visible)
        {
            EventManager.BroadcastReturnMouse();
            ChangePauseMenu();
        }
        base._Input(@event);
    }

    public void OnResumeBtnPressed(){
        ChangePauseMenu();
    }

    public void OnReturnMouse(){
        if (Input.MouseMode == Input.MouseModeEnum.Hidden)
            Input.MouseMode = Input.MouseModeEnum.Visible;
        else
            Input.MouseMode = Input.MouseModeEnum.Hidden;
    }

    public void ChangePauseMenu(){
        _gamePaused = !_gamePaused;
        if (_gamePaused)
        {
            GetTree().Paused = true;
            e_pauseMenu.Show();
        }
        else{
            GetTree().Paused = false;
            e_pauseMenu.Hide();
        }
    }

    public override void _EnterTree()
    {
        EventManager.ReturnMouse += OnReturnMouse;
        EventManager.ResumeButtonPressedEvent += OnResumeBtnPressed;
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        EventManager.ReturnMouse -= OnReturnMouse;
        EventManager.ResumeButtonPressedEvent -= OnResumeBtnPressed;
        base._ExitTree();
    }
}
