using Godot;
using System;

public partial class TileMap : Godot.TileMap
{
    private int _tileId = 0; // The ID of the tile you want to place. Change this to the ID of your desired tile.
    Vector2 globalMousePosition;
    Vector2 localMousePosition;
    Vector2I cellCoordinates;
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
    }
    void GetMousePositions()
    {
        // Get the global position of the mouse click.
        globalMousePosition = GetGlobalMousePosition();

        // Convert the global position to a local position relative to the TileMap.
        localMousePosition = ToLocal(globalMousePosition);

        cellCoordinates = LocalToMap(localMousePosition);
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
