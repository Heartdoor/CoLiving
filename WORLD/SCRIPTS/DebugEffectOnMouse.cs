using Godot;
using System;
using static Asriela.BasicFunctions;

public partial class DebugEffectOnMouse : Label
{
    

    void Start()
    {

    }

    void Run()
    {
        GlobalPosition = GetGlobalMousePosition();
        Text= Main.DebugEffectOnMouse;
    }
    #region OLD
    public override void _Ready()
    {
        Start();
    }
    public override void _Process(double delta)
    {
        Run();
    }
    #endregion
}
