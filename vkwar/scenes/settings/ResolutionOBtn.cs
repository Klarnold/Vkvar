using Godot;
using System;

public partial class ResolutionOBtn : OptionButton
{
    public override void _Ready()
    {
        Selected = GlobalsN.resolutionIndex;
        base._Ready();
    }
    public void OnItemSelected(long id){
        GlobalsN.screenResolution = Text.Substr(0, 2) switch
        {
            "13" => new Vector2I(1366, 768), 
            "12" => new Vector2I(1280, 720),
            _ => new Vector2I(1920, 1080),
        };
        if (Text.Substr(0, 2) == "13")
        {
            GlobalsN.screenResolution = new Vector2I(1366, 768);
            GlobalsN.resolutionIndex = 1;
        }
        else if (Text.Substr(0, 2) == "12")
        {
            GlobalsN.screenResolution = new Vector2I(1280, 720);
            GlobalsN.resolutionIndex = 0;
        }
        else{
            GlobalsN.screenResolution = new Vector2I(1920, 1080);
            GlobalsN.resolutionIndex = 2;
        }

        if (GetWindow().Mode != Window.ModeEnum.Fullscreen)
            GetWindow().Size = GlobalsN.screenResolution;
    }
    public override void _EnterTree()
    {
        ItemSelected += OnItemSelected;
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        ItemSelected -= OnItemSelected;
        base._ExitTree();
    }
}
