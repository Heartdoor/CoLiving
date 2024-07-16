using Godot;
using System;
using System.Linq;

public partial class Pathfinding : Node
{

    public AStarGrid2D astarGrid;
    private int gridSize = 500;
    private Vector2 cellSize = new Vector2(16,16);
    private Color _gridColor = new Color(1, 1, 1, 0.5f); // Semi-transparent white for grid lines
    private Color _solidColor = new Color(1, 0, 0, 0.5f); // Semi-transparent red for solid cells

    private TileMap _tileMap;
    private int _gridTileId = 0; // ID of the tile representing grid cells
    private int _solidTileId = 1; // ID of the tile representing solid cells


    void SetupAStar()
    {
        astarGrid = new AStarGrid2D();
        astarGrid.Region = new Rect2I(0, 0, gridSize, gridSize);
        astarGrid.CellSize = new Vector2(cellSize.X, cellSize.Y);
        astarGrid.Region = new Rect2I(Vector2I.Zero, new Vector2I(gridSize, gridSize));
        astarGrid.DiagonalMode = Godot.AStarGrid2D.DiagonalModeEnum.Always;
        astarGrid.Update();

        _tileMap = GetParent().GetNode<TileMap>("GridTileMap"); // Reference the TileMap node
        DrawGrid();
    }


   public void AddObstacle(RigidBody2D obstacle)
    {
        var collisionShape = obstacle.GetNode<CollisionShape2D>("CollisionShape2D");
        var shape = (RectangleShape2D)collisionShape.Shape;
        var shapeSize = shape.Size; // Get the full size of the shape
        var topLeft = WorldToGrid(obstacle.GlobalPosition - shape.Size / 2);
        var bottomRight = WorldToGrid(obstacle.GlobalPosition + shape.Size / 2);

        for (int x = topLeft.X; x <= bottomRight.X; x++)
        {
            for (int y = topLeft.Y; y <= bottomRight.Y; y++)
            {
                astarGrid.SetPointSolid(new Vector2I(x, y), true);
                _tileMap.SetCell(0,new Vector2I(x,y), _solidTileId);
            }
        }
    }

    public void RemoveObstacle(RigidBody2D obstacle)
    {
        var collisionShape = obstacle.GetNode<CollisionShape2D>("CollisionShape2D");
        var shape = (RectangleShape2D)collisionShape.Shape;
        var shapeSize = shape.Size ; // Get the full size of the shape
        var topLeft = WorldToGrid(obstacle.GlobalPosition - shape.Size/2);
        var bottomRight = WorldToGrid(obstacle.GlobalPosition + shape.Size / 2);

        for (int x = topLeft.X; x <= bottomRight.X; x++)
        {
            for (int y = topLeft.Y; y <= bottomRight.Y; y++)
            {
                astarGrid.SetPointSolid(new Vector2I(x, y), false);
                _tileMap.SetCell(0, new Vector2I(x, y), _gridTileId);
            }
        }
    }
    private void DrawGrid()
    {
        for (int x = 0; x < astarGrid.Region.Size.X; x++)
        {
            for (int y = 0; y < astarGrid.Region.Size.Y; y++)
            {
                //_tileMap.SetCell(0, new Vector2I(x, y), _gridTileId); // Initialize the grid with grid tile
                if (astarGrid.IsPointSolid(new Vector2I(x, y)))
                {
                    _tileMap.SetCell(0, new Vector2I(x, y), _solidTileId); // Set solid cells
                }
            }
        }
    }
    public Vector2[] GetPath(Vector2 start, Vector2 end)
    {
        var startGrid = WorldToGrid(start);
        var endGrid = WorldToGrid(end);

        var path = astarGrid.GetPointPath(startGrid, endGrid);
        Vector2[] worldPath = new Vector2[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            worldPath[i] = new Vector2(path[i].X * cellSize.X, path[i].Y * cellSize.Y);
        }

        return worldPath;
    }


    public Vector2I WorldToGrid(Vector2 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.X / cellSize.X);
        int y = Mathf.FloorToInt(worldPosition.Y / cellSize.Y);
        return new Vector2I(x, y);
    }




    public override void _Ready()
    {
        SetupAStar();
    }
}
