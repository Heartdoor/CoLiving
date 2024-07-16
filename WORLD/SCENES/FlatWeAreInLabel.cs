using Godot;
using System;
using System.Linq;

public partial class FlatWeAreInLabel : Label
{
    void Run()
    {

            Text = $"Flat: {Main.FlatNumberMouseIsIn}";
    }


    public override void _Process(double delta)
    {
        Run();
    }
}
