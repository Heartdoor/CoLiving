using Godot;
using System;

public partial class GameTileGrid : Godot.TileMap
{
    private int _tileId = 0; // The ID of the tile you want to place. Change this to the ID of your desired tile.
    public static Vector2 globalMousePosition;
    public static Vector2 localMousePosition;
    public static Vector2I cellCoordinates;
  
    
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
        Main.MyTileMap = this;
    }

    void Run()
    {
        GetMousePositions();
        GetTileAtlasPositionWeAreOver();
  
    }

    void GetTileAtlasPositionWeAreOver()
    {
        Main.FlatNumberMouseIsIn = GetCellSourceId(0,cellCoordinates);
        Main.OverPlaceableTile = false;

        for (var i = 1; i < 9; i++) 
        {
            if(!Main.OverPlaceableTile)
            Main.OverPlaceableTile = GetCellSourceId(i, cellCoordinates) == 5 ? true : false;
            if (GetCellSourceId(i, cellCoordinates) == 5 ? true : false)
            Main.OverLeaderFurnitureLayer = i;  
        }

    }
    void GetMousePositions()
    {
        // Get the global position of the mouse click.
        globalMousePosition = GetGlobalMousePosition();

        // Convert the global position to a local position relative to the TileMap.
        localMousePosition = ToLocal(globalMousePosition);

        cellCoordinates = LocalToMap(localMousePosition);
    }



    public void FillFlatWithPlaceableArea()
    {
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
