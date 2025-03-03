using Godot;
using System;

public partial class RealityCr : ColorRect
{
    public override void _Ready()
    {
        if (GetParent().GetParent().IsInGroup("cReality1"))
            Color = new Color(255, 0, 0, 255);
        else if (GetParent().GetParent().IsInGroup("cReality2"))
            Color = new Color(0, 0, 255, 255);
        else
            Color = new Color(0, 0, 0, 255);
        base._Ready();
    }

    public void OnRealityChanged(bool reality1){
        if (reality1)
            Color = new Color(255, 0, 0, 255);
        else
            Color = new Color(0, 0, 255, 255);
    }

    public override void _EnterTree()
    {
        EventManager.ChangedRealityEvent += OnRealityChanged;
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        EventManager.ChangedRealityEvent -= OnRealityChanged;
        base._ExitTree();
    }
}
