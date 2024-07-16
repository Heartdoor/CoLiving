using Godot;
using System;
using System.Collections.Generic;
using static Asriela.BasicFunctions;

public partial class Furniture : StaticBody2D
{
    [Export] public string name = "unassigned";
    bool firstRun = true;
    public Main.Object objectData;
    public CollisionShape2D myArea;
    public CollisionShape2D myCollision;
    public Node2D myUseLocation1;
    public Node2D myUseLocation2;
    public List<CharacterBody2D> occupants = new List<CharacterBody2D>();
    public int flatIAmIn = 0;

    [Export]
    public NodePath TileMapPath; // The path to the TileMap node in the scene.

    private int _tileId = 0; // The ID of the tile you want to place. Change this to the ID of your desired tile.
    private TileMap _tileMap;

    #region BASIC OBJECT
    public Sprite2D mySprite;

    #endregion

    private void PlaceTilesToCoverShape(Rect2 shapeRect)
    {
        // Get the top-left and bottom-right corners of the rectangle in local coordinates.
        Vector2 topLeft = shapeRect.Position + GlobalPosition;
        Vector2 bottomRight = shapeRect.Position + shapeRect.Size + GlobalPosition;

        // Convert these coordinates to tile coordinates.
        Vector2I topLeftTile = Main.MyTileMap.LocalToMap(Main.MyTileMap.ToLocal(topLeft));
        Vector2I bottomRightTile = Main.MyTileMap.LocalToMap(Main.MyTileMap.ToLocal(bottomRight));

        // Loop through the tile coordinates and set the tile.
        for (int x = topLeftTile.X; x <= bottomRightTile.X; x++)
        {
            for (int y = topLeftTile.Y; y <= bottomRightTile.Y; y++)
            {
                Main.MyTileMap.SetCell(0, new Vector2I(x, y), flatIAmIn, new Vector2I(1, 0) );
            }
        }
    }


    void PlaceOnTileMap()
    {


        // Get the CollisionShape2D node.
        CollisionShape2D collisionShape = myCollision;

        // Get the collision shape's rectangle area.
        Rect2 shapeRect = collisionShape.Shape.GetRect();

        // Place tiles to cover the collision shape.
        PlaceTilesToCoverShape(shapeRect);
    }
    void SetupObject()
    {
        mySprite = GetNode<Sprite2D>("Sprite2D");
        myUseLocation1= GetNode<Node2D>("UseLocation1");
        myUseLocation2 = GetNode<Node2D>("UseLocation2");
        Area2D areaParent= GetNode<Area2D>("Area2D");
        myArea = areaParent.GetNode<CollisionShape2D>("AreaShape");
        myCollision= GetNode<CollisionShape2D>("CollisionShape");


    }
    void Start()
    {
       
        SetupObject();
        
    }

    void Run() {

        ZIndex = (int)GlobalPosition.Y;
        if (firstRun)
        {
            PlaceOnTileMap();
            firstRun = false;
        }
        if (KeyPressed("RightClick"))
        {

        }



    }
    public void ChangeDimensions(int size)
    {
        switch (size)
        {
            case 1:
                
                break;
            case 2:
                myArea.Scale = new Vector2(2,1) ;
                myCollision.Scale = new Vector2(6, 1);
                myUseLocation1.Position= new Vector2(-39, 14);
                myUseLocation2.Position = new Vector2(39, 14);
                break;
        }
    }

    private void OnBodyEntered(Node body)
    {
        if (body is CharacterBody2D)
        {
            var character = (Characters)body;
            var target = (Furniture)character.currentTarget;
            var myType = target.objectData.type;
            var yourType = objectData.type;
            if (myType == yourType)
            {
                character.ReachTarget();

                Log("CHAR ENTERED", LogType.game);

            }
              
           
        }
        Log("BODY ENTERED", LogType.game);
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
