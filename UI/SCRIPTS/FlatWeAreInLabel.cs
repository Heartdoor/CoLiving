using Godot;
using System;
using System.Linq;

public partial class FlatWeAreInLabel : Label
{
    void Run()
    {

            Text = $"Room: {Main.RoomNumberMouseIsIn}";
    }


    public override void _Process(double delta)
    {
        Run();
    }
}
