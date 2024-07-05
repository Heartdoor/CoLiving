using Godot;
using System;
using static Asriela.BasicFunctions;

public partial class MainScene : Node3D
{
    void Start()
    {


        for (var i=0; i<40;i++)
        {

            Log($"TEST {i}", LogType.game);
            Node3D objectOrigin  = Add3DNode("res://MAIN/object_origin.tscn", this);
            
            
            Object spriteNode = (Object)GetSprite3D(objectOrigin, "Sprite3D");
            Sprite3D objectSprite = (Sprite3D)spriteNode;
            var number = (int)RandomRange(1, 16);

            objectSprite.Texture = GetTexture2D($"res://OBJECTS/OBJECTS/SPRITES/CORAL/{number}.png");
            
            
            Change3DZ(objectSprite, Choose(-5,-11));
            var rotation = (float)RandomRange(0, 360);
            objectOrigin.RotateY(rotation);
            // Change3DY(objectSprite, 1);

        }


    }

    
    void Run()
    {

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
