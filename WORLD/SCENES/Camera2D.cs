using Godot;
using System;
using static Asriela.BasicFunctions;

public partial class Camera2D : Godot.Camera2D
{

    
    void Run()
    {
        if (KeyHeld("CamZoom"))
        {
            Zoom = new Vector2 (Zoom.X+0.1f, Zoom.Y+0.1f);
        }
    }

    void Start()
    {

    }


    #region OLD
    public override void _Ready()
    {
        Start();
    }

    public override void _PhysicsProcess(double delta)
    {

        Run();

    }
    #endregion
}
