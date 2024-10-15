using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Asriela.BasicFunctions;

public class FurnitureData
{
    public FurnitureName name { get; set; }
    public bool flatWideEffect { get; set; }
    public string image_path { get; set; }
    public Texture2D texture { get; set; }
    public Texture2D shadowTexture { get; set; }
    public Dictionary<Effect, int> basicEffects { get; set; }
    public Dictionary<Effect, int> usedRadiantEffects { get; set; }
    public Vector2 usePosition { get; set; }
    public float useLength { get; set; }
    public int size { get; set; }
    public int price { get; set; }

    public FurnitureType type { get; set; }
    public List<RoomType> roomTypes { get; set; }

    public bool floorObject { get; set; }
    public FurnitureGroup group { get; set; }

    public FurnitureGroup useFromGroup { get; set; }
    public Direction rotation { get; set; }

    public bool ontopUsePosition { get; set; }
    public bool debugItem { get; set; }
    public List<AccessPosition> accessPositions { get; set; }

    public UseAnimation useAnimation { get; set; }

    // Constructor
    public FurnitureData(FurnitureName name, bool flatWideEffect = false, int size = 1, int price = 0, float useLength = 10f)
    {
        this.name = name;
        this.type = FurnitureType._decor;
        this.roomTypes = new List<RoomType>();
        this.flatWideEffect = flatWideEffect;
        image_path = $"res://FURNITURE/SPRITES/{name}.png";
        texture = GetTexture2D(image_path);
        var temp_path = $"res://FURNITURE/SPRITES/{name}_s.png";

        shadowTexture = GetTexture2D(temp_path);
        useFromGroup = FurnitureGroup.none;
        group = FurnitureGroup.none;
        rotation = Direction.down;
        accessPositions = new List<AccessPosition>();
        ontopUsePosition = false;
        useAnimation = UseAnimation.sit;
        this.size = size;
        this.price = price;
        this.usePosition = new Vector2(0, 0);
        this.useLength = useLength;
        this.basicEffects = new Dictionary<Effect, int>();
        this.usedRadiantEffects = new Dictionary<Effect, int>();
        this.floorObject = false;
        this.debugItem = false;
    }
}
