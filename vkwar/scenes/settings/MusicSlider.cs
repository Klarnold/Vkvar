using Godot;
using System;

public partial class MusicSlider : HSlider
{
    [Export] private String e_busName;
    //[Export] private int e_busIndex;
    private int e_busIndex;
    public override void _Ready()
    {
        e_busIndex = AudioServer.GetBusIndex(e_busName);
        Value = GlobalsN.decibelArray[e_busIndex];
        base._Ready();
    }

    public void OnValueChanged(double value){
        GlobalsN.decibelArray[e_busIndex] = (float)value;
        AudioServer.SetBusVolumeDb(e_busIndex, (float)linearToDecibel(value));
    }

    public float linearToDecibel(double linear){
        if (linear > 0){
            return  (float)Math.Clamp(20 * Math.Log10(linear), -80, 0);
        }
        return -80;
    }

    public double decibelToLinear(float decibel){
        return (float)Math.Pow(10, decibel/10);
    }
    public override void _EnterTree()
    {
        ValueChanged += OnValueChanged;
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        ValueChanged -= OnValueChanged;
        base._ExitTree();
    }
}
