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
    public int roomIAmIn = 0;
    
    public List<Furniture> myConnectedFurniture = new List<Furniture>();
    public int myLayer;
    public Main.Room myRoom;

    public bool inUse = false;
    [Export]
    public NodePath TileMapPath; // The path to the TileMap node in the scene.

    private int _tileId = 0; // The ID of the tile you want to place. Change this to the ID of your desired tile.
    private TileMap _tileMap;

    Vector2I myTopLeftTile;
    Vector2I myBottomRightTile;
    Vector2I myCenterTile;

    #region BASIC OBJECT
    public Sprite2D mySprite;
    public Sprite2D myShadow;
    public Label myLabel;
    #endregion

    private void PlaceTilesToCoverShape(Rect2 shapeRect)
    {
        // Get the top-left and bottom-right corners of the rectangle in local coordinates.
        Vector2 topLeft = shapeRect.Position + GlobalPosition;
        Vector2 bottomRight = shapeRect.Position + shapeRect.Size + GlobalPosition;

        // Convert these coordinates to tile coordinates.
        myCenterTile= Main.MyTileMap.LocalToMap(Main.MyTileMap.ToLocal(GlobalPosition));
        myTopLeftTile = Main.MyTileMap.LocalToMap(Main.MyTileMap.ToLocal(topLeft));
        myBottomRightTile = Main.MyTileMap.LocalToMap(Main.MyTileMap.ToLocal(bottomRight));

        var count = 0;
        // Loop through t =he tile coordinates and set the tile.
        for (int x = myTopLeftTile.X; x <= myBottomRightTile.X; x++)
        {
            for (int y = myTopLeftTile.Y; y <= myBottomRightTile.Y; y++)
            {
                count++;
               Main.MyTileMap.SetCell(0, new Vector2I(x, y), 0, new Vector2I(0, 2) );
            }
        }

        Log($"{count}", LogType.game);

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
        myShadow = GetNode<Sprite2D>("Shadow");
        
        myLabel = GetNode<Label>("Label");
        myUseLocation1 = GetNode<Node2D>("UseLocation1");
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

        ShowFurnitureGroupArea();


            myLabel.Text = $"{ myRoom.type}";
        if (occupants.Count > 0 && inUse==false)
        {
            inUse = true;
            var temp_texture = GetTexture2D($"res://OBJECTS/SPRITES/{objectData.name}_used.png");
            if (temp_texture != null)
            mySprite.Texture = temp_texture;
        }
        else
        if(inUse && occupants.Count==0)
        {
            inUse = false;
            mySprite.Texture = GetTexture2D($"res://OBJECTS/SPRITES/{objectData.name}.png");
        }


    }

    public void ShowFurnitureGroupArea()
    {
        /*
        //if we are not holding a core object 
        if (Main.HeldObject == null || objectData.type != FurnitureType._core) return;
           if( !Main.HeldObject.furnitureGroups.Contains(objectData.isGroupLeader) ) return;
        var halfWidth = 8;
        var halfHeight = 8;
        var topLeftTile = new Vector2I(myCenterTile.X-halfWidth, myCenterTile.Y-halfHeight);
        var bottomRightTile = new Vector2I(myCenterTile.X + halfWidth, myCenterTile.Y + halfHeight);
        // Loop through the tile coordinates and set the tile.
        for (int x = topLeftTile.X; x <= bottomRightTile.X; x++)
        {
            for (int y = topLeftTile.Y; y <= bottomRightTile.Y; y++)
            {
                if(Main.MyTileMap.GetCellSourceId(0, new Vector2I(x,y))== flatIAmIn)
                Main.MyTileMap.SetCell(myLayer, new Vector2I(x, y), 5, new Vector2I(0, 0) );
            }
        }
        */
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
                myUseLocation1.Position= new Vector2(-19, 14);
                myUseLocation2.Position = new Vector2(19, 14);
                break;
        }
    }

    private void OnBodyEntered(Node body)
    {
        if (body is CharacterBody2D)
        {
            Log("entered area", LogType.weird);

            var character = (Characters)body;
            var target = (Furniture)character.accessTarget;
            var myType = target.objectData.type;
            var yourType = objectData.type;
            if (character.accessTarget == this)
            {
                character.ReachTarget();

                Log("CHAR ENTERED", LogType.weird);

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
