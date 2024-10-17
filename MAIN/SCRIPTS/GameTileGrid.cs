using Godot;
using System;
using System.Collections.Generic;
using static Asriela.BasicFunctions;
public partial class GameTileGrid : Godot.TileMap
{
    private int _tileId = 0; // The ID of the tile you want to place. Change this to the ID of your desired tile.
    public static Vector2 globalMousePosition;
    public static Vector2 localMousePosition;
    public static Vector2I cellCoordinates;
    public static ShaderMaterial shaderMaterial;
    public static Dictionary<FloorTexture, Texture2D> floorTextures = new Dictionary<FloorTexture, Texture2D>();

    public static List<string> roomNames = new List<string>();



    public override void _Input(InputEvent @event)
    {
        // Check if the input event is a mouse button press and if it's the left mouse button.
        if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed && eventMouseButton.ButtonIndex == MouseButton.Left)
        {


            // Get the cell coordinates from the local mouse position.


            // Set the tile at the cell coordinates.
            //SetCell(0, cellCoordinates, 2);
        }
    }

    void Start()
    {
        floorTextures[FloorTexture.wood] = GetTexture2D("res://WORLD/FLOORS/woodFloor.png");
        floorTextures[FloorTexture.tiledKitchen] = GetTexture2D("res://WORLD/FLOORS/tiledKitchenFloor.png");
        floorTextures[FloorTexture.tiledBathroom] = GetTexture2D("res://WORLD/FLOORS/tiledBathroomFloor.png");
        floorTextures[FloorTexture.carpet] = GetTexture2D("res://WORLD/FLOORS/carpetFloor.png");

        roomNames.Add("none");
        roomNames.Add("floor_texture_blue");
        roomNames.Add("floor_texture_red");
        roomNames.Add("floor_texture_green");
        roomNames.Add("floor_texture_yellow");
        Main.MyTileMap = this;
        shaderMaterial = (ShaderMaterial)Material;

        //  SetLayerModulate(1, ColorTransparent);
    }

    public static void SetFloorMaterial(int roomNumber, RoomType roomType)
    {
        FloorTexture floorTexture = roomType switch
        {
            RoomType.livingroom => FloorTexture.wood,
            RoomType.kitchen => FloorTexture.tiledKitchen,
            RoomType.bathroom => FloorTexture.tiledBathroom,
            RoomType.bedroom => FloorTexture.carpet,
            _ => FloorTexture.cement
        };
        shaderMaterial.SetShaderParameter(roomNames[roomNumber], floorTextures[floorTexture]);
    }
    void Run()
    {
        GetMousePositions();
        GetTileAtlasPositionWeAreOver();

    }

    void GetTileAtlasPositionWeAreOver()
    {
        Main.RoomNumberMouseIsIn = GetCellSourceId(1, cellCoordinates);
        Main.OverPlaceableTile = false;



    }
    void GetMousePositions()
    {
        // Get the global position of the mouse click.
        globalMousePosition = GetGlobalMousePosition();

        // Convert the global position to a local position relative to the TileMap.
        localMousePosition = ToLocal(globalMousePosition);

        cellCoordinates = LocalToMap(localMousePosition);
    }

    public static int GetRoomNumberWeAreIn(Vector2 position, TileMap tileMap)
    {
        // Get the global position of the mouse click.
        

        // Convert the global position to a local position relative to the TileMap.
        var localPosition = tileMap.ToLocal(position);

        return tileMap.GetCellSourceId(1, tileMap.LocalToMap(localPosition));
    }


    public void FillFlatWithPlaceableArea()
    {
        /*
        Main.MyTileMap.ClearLayer(1);
        var topLeftTile = new Vector2I(0, 0);
        var size = GetUsedRect().Size;
        var bottomRightTile = new Vector2I(size.X, size.Y);
        // Loop through the tile coordinates and set the tile.
        for (int x = topLeftTile.X; x <= bottomRightTile.X; x++)
        {
            for (int y = topLeftTile.Y; y <= bottomRightTile.Y; y++)
            {
                if (Main.MyTileMap.GetCellSourceId(0, new Vector2I(x, y)) == Main.FlatNumberMouseIsIn)
                    Main.MyTileMap.SetCell(1, new Vector2I(x, y), 5, new Vector2I(0, 0));
            }
        }
        */
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
