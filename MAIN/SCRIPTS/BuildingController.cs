using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static Asriela.BasicFunctions;

public partial class BuildingController : Node
{
    public class RoomItem
    {
        public RoomType type { get; set; }
        public int number { get; set; }

        public List<Node2D> furnitureObjects { get; set; }

        public Dictionary<Effect, int> decorationEffects { get; set; }

        public RoomItem(int number, RoomType type)
        {
            this.number = number;
            this.type = type;
            furnitureObjects = new List<Node2D>();
            decorationEffects = new Dictionary<Effect, int>();
        }


    }
    public class BuildingItem
    {
        public int number { get; set; }
        private List<RoomItem> _rooms;

        public List<Node2D> furnitureObjects { get; set; }

        public string image_path { get; set; }
        public Color color { get; set; }
        public double happiness { get; set; }
        public List<CharacterController> charactersInBuilding { get; set; }


        public BuildingItem(int number, Color color, params RoomItem[] rooms)
        {

            this.number = number;
            image_path = $"res://FLATS/SPRITES/{number}.png";
            this.color = color;
            this.charactersInBuilding = new List<CharacterController>();
            this._rooms = new List<RoomItem>(rooms);

            this.furnitureObjects = new List<Node2D>();
        }
        public List<RoomItem> rooms
        {
            get
            {
                return new List<RoomItem>(_rooms.Select((room, index) => index > 0 ? _rooms[index - 1] : room));
            }
            set { _rooms = value; }
        }
    }
    public static List<BuildingItem> buildingsList = new List<BuildingItem>();
    public void Start()
    {
       

    }

    public void Run()
    {

    }

    public static BuildingItem AddNewBuilding(int flatNumber, Color color, params RoomItem[] rooms)
    {
        var newFlat = new BuildingItem(flatNumber, color, rooms);

        buildingsList.Add(newFlat);
        return newFlat;
    }




    public static void AddFurnitureToRoom(int buildingNumber, RoomItem room, Node2D furnitureObject)
    {
        
        buildingsList[buildingNumber].furnitureObjects.Add(furnitureObject);
        room.furnitureObjects.Add(furnitureObject);
        AddDecorFurnitureEffectValuesToRoom(room, furnitureObject);
        
    }

    static void AddDecorFurnitureEffectValuesToRoom(RoomItem room, Node2D furnitureObject)
    {
        var furnitureData = ((FurnitureController)furnitureObject).furnitureData;

        if (furnitureData.type != FurnitureType._decor) return;

        foreach (KeyValuePair<Effect, int> effects in furnitureData.basicEffects)
        {
            if (!room.decorationEffects.ContainsKey(effects.Key))
            {

                Log($"Added {effects.Key} with {effects.Value} as decoration", LogType.game);

                room.decorationEffects.Add(effects.Key, effects.Value);
            }

            else
            {
                Log($"Added {effects.Key} with {effects.Value} as decoration", LogType.game);
                room.decorationEffects[effects.Key] += effects.Value;
            }
               
        }
    }
    public static float GetRoomDecorEffect(CharacterData characterData , RoomItem room )
    {
        Dictionary<Effect, float> effectsBreakdown = new Dictionary<Effect, float>();

        var objE = room.decorationEffects;

        var charE = characterData.effectsList;

        // Log($"Base Pref Of: {objectItem.objectData.name} is [objE]", LogType.step);

        //basePrefOfObjects.Add(objectItem, objE.Keys.Intersect(charE.Keys).Select(key => objE[key] * charE[key]).Sum());
        // Assuming objE and charE are dictionaries
        var intersectedKeys = objE.Keys.Intersect(charE.Keys).ToList();

        Log("Intersected Keys: " + string.Join(", ", intersectedKeys), LogType.step);


        var products = intersectedKeys.Select(key =>
        {
           
      
            var product = objE[key] * charE[key].strength;
            effectsBreakdown.Add(key, product);
            //Log(, LogType.step);
            Log($"Key: {key}, objE[{key}] = {objE[key]}, charE[{key}] = {charE[key]}, Product: {product}", LogType.step);
            return product;
        }).ToList();


        return products.Sum();
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
