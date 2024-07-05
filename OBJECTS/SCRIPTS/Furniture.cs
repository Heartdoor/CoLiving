using Godot;
using System;
using static Asriela.BasicFunctions;

public partial class Furniture : StaticBody2D
{
        [Export] public string name="unassigned";

    public Main.Object objectData;

    #region BASIC OBJECT
    public Sprite2D mySprite;

    #endregion
    void SetupObject()
    {
        mySprite = GetNode<Sprite2D>("Sprite2D");
    }
    void Start()
    {
      
        SetupObject();
    }

    void Run() {
        if (ButtonPressed("RightClick"))
        {

        } 
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
