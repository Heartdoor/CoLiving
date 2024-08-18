using Godot;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Xml.Linq;
using static Asriela.BasicFunctions;
using static Main;

public partial class Main : Node2D
{
    #region STATES
    public static Control SelectionMenuOpen = null;
    public static bool canPlace = true;
    #endregion

    #region MAIN CONTROL SETUP
    public static float Money=200f;
    public static Object HeldObject=null;
    public static Character HeldCharacter= null;


    public static int FlatNumberMouseIsIn = 1;
    public static int RoomNumberMouseIsIn = 1;
    int lastFlatWeWereIn = 1;
    public static List<Character> CharactersAvailableToPlayerList = new List<Character> ();
    public static List<Character> PlacedCharactersList = new List<Character>();
    public static List<Furniture> leaderFurnitureList = new List<Furniture>();
    public static int OverLeaderFurnitureLayer=0;

    Sprite2D placeItemImage;
    public static TileMap MyTileMap;
    public static Label MyUnlocksLabel;
    float highestUnlock = 0;

    public static bool OverPlaceableTile;
    #endregion

    #region SETTINGS
    public static bool SpawnInitialObjects = false;
    public static bool MoneyCheat = true;
    public static bool UnlockCheat = true;
    
    public static bool QuickRestart = true;
    public enum testGameMode : short
    {
        complex,
        zooTycoon,
        flowingMoney

    }
    public static testGameMode TestGameMode = testGameMode.flowingMoney;
    #endregion

    #region CLASSES SETUP
    public static List<Object>  FurnitureUnlockedList = new List<Object>();
    public int startingAmountOfUnlockedFurniture;
    public class Object
        {
            public FurnitureName name { get; set; }
            public bool flatWideEffect { get; set; }
            public string image_path { get; set; }
            public Texture2D texture { get; set; }
            public Texture2D shadowTexture { get; set; }
            public Dictionary<Effect, int> usedEffects { get; set; }
            public Vector2 usePosition { get; set; }
            public float useLength { get; set; }
            public int size { get; set; }
            public int price { get; set; }

            public FurnitureType type { get; set; }
            public List<RoomType> roomTypes { get; set; }
        
            public bool floorObject { get; set; }
            public FurnitureGroup group { get; set; }

            public FurnitureGroup useFromGroup { get; set; }

        public Object(FurnitureName name,  bool flatWideEffect = false,  int size=1,int price=0,  float useLength = 10f) 
            {
                this.name = name;
                this.type = FurnitureType._decor;
                this.roomTypes = new List<RoomType>();
                this.flatWideEffect = flatWideEffect;
                image_path = $"res://OBJECTS/SPRITES/{name}.png";
                texture = GetTexture2D(image_path);
                var temp_path = $"res://OBJECTS/SPRITES/{name}_s.png";

                shadowTexture = GetTexture2D(temp_path);
                useFromGroup = FurnitureGroup.none;
                group = FurnitureGroup.none;
                this.size = size;
                this.price = price;
                this.usePosition =   new Vector2(0, 0);
                this.useLength = useLength;
                this.usedEffects = new Dictionary<Effect, int>();
                this.floorObject = false;

        }
        }
        public static List<Object> objectsList = new List<Object>();

        public class EffectProperties
        {
       
            public float strength { get; set; }
            public float needBleedRate { get; set; }
            public float desireGrowRate { get; set; }
            public DesireAction action { get; set; }

            public bool syncDesireWithNeed { get; set; }

        public EffectProperties( float strength, float needBleedRate, float desireGrowRate, DesireAction action = DesireAction.none, bool syncDesireWithNeed = false)
            {
   
                this.strength = strength;
                this.needBleedRate = needBleedRate;
                this.desireGrowRate = desireGrowRate;
                this.action = action;
                this.syncDesireWithNeed = syncDesireWithNeed;
            }
        }
        public class Character
        {
            public CharacterType type { get; set; }
            public string image_path { get; set; }
            public Texture2D texture { get; set; }
            public Texture2D shadowTexture { get; set; }
            public Dictionary<Effect,EffectProperties> effectsList { get; set; }


        public Character(CharacterType type, Dictionary<Effect, EffectProperties> effectsList)
            {
                this.type = type;
                image_path = $"res://CHARACTERS/SPRITES/{type}.png";
                texture = GetTexture2D(image_path);
                shadowTexture = GetTexture2D($"res://CHARACTERS/SPRITES/{type}_s.png");
                this.effectsList = new Dictionary<Effect, EffectProperties>(effectsList);

            }
        }
        public static List<Character> charactersList = new List<Character>();

        public class Room
        {
            public RoomType type { get; set; }
            public int number { get; set; }
         
            public List<Node2D> objects { get; set; }

        public Room(int number, RoomType type)
        {
            this.number = number;
            this.type = type;
            objects = new List<Node2D>();

        }


    }
        public class Flat
        {
            public int number { get; set; }
        private List<Room> _rooms;

        public List<Node2D> objects { get; set; }

            public string image_path { get; set; }
            public Color color { get; set; }
            public double happiness { get; set; }
            public List<Characters> charactersInFlat { get; set; }


            public Flat(int number, Color color, params Room[] rooms)
            {
           
                this.number = number;
                image_path = $"res://FLATS/SPRITES/{number}.png";
                this.color = color;
                this.charactersInFlat = new List<Characters>();
            this._rooms = new List<Room>(rooms);

            this.objects = new List<Node2D>();
           }
          public List<Room> rooms
        {
            get
            {
                return new List<Room>(_rooms.Select((room, index)=>index>0 ? _rooms[index - 1] :room));
            }
            set { _rooms = value; }
        } 
        }
        public static List<Flat> flatsList = new List<Flat>();

    #endregion

    #region COLOR SETUPS
    public static readonly Color ColorCantBePlaced = new Color(0xff00008d);
    public static readonly Color ColorPlace = new Color(0x00ffff64);
    #endregion

    #region LOCAL SETUPS
    Object o;
    #endregion

    #region SETUPS


    void AddNewObject(FurnitureName name)
    {
        o = new Object(name);  
        objectsList.Add(o);
    }
    void SetupObjects()
    {



        AddNewObject(FurnitureName.couch);
        o.type = FurnitureType._core;
        o.group = FurnitureGroup.chair;
        o.roomTypes.Add(RoomType.livingroom); 
        o.flatWideEffect = false;
        o.size = 2;
        o.price = 100;
        o.useLength = 10;
        o.usedEffects.Add(Effect.comfort, 1);



        AddNewObject(FurnitureName.electricGuitar);
        o.type = FurnitureType._object;
        o.roomTypes.Add(RoomType.livingroom);
        o.flatWideEffect = true;
        o.size = 1;
        o.price = 200;
        o.useLength = 10;
        o.usedEffects.Add(Effect.music, 3);
        o.usedEffects.Add(Effect.noise, 3);
        o.usedEffects.Add(Effect.grunge, 2);

        AddNewObject(FurnitureName.recordPlayer);
        o.type = FurnitureType._object;
        o.roomTypes.Add(RoomType.livingroom);
        o.flatWideEffect = true;
        o.size = 1;
        o.price = 100;
        o.useLength = 10;
        o.usedEffects.Add(Effect.music, 3);
        o.usedEffects.Add(Effect.vintage, 3);
 

        AddNewObject(FurnitureName.stove);
        o.type = FurnitureType._core;
        o.roomTypes.Add(RoomType.kitchen);
        o.flatWideEffect = false;
        o.size = 1;
        o.price = 200;
        o.useLength = 10;
        o.usedEffects.Add(Effect.food, 4);
        o.usedEffects.Add(Effect.cozy,2);

        AddNewObject(FurnitureName.fridge);
        o.type = FurnitureType._core;
        o.roomTypes.Add(RoomType.kitchen);
        o.flatWideEffect = false;
        o.size = 1;
        o.price = 200;
        o.useLength = 10; 
        o.usedEffects.Add(Effect.food, 4);
        o.usedEffects.Add(Effect.cozy, 2);

        AddNewObject(FurnitureName.counterTop);
        o.type = FurnitureType._core;
        o.roomTypes.Add(RoomType.kitchen);
        o.flatWideEffect = false;
        o.size = 1;
        o.price = 100;
        o.useLength = 10;
        o.usedEffects.Add(Effect.food, 4);
        o.usedEffects.Add(Effect.cozy, 2);

        AddNewObject(FurnitureName.sideCountertop);
        o.type = FurnitureType._core;
        o.roomTypes.Add(RoomType.kitchen);
        o.flatWideEffect = false;
        o.size = 1;
        o.price = 100;
        o.useLength = 10;
        o.usedEffects.Add(Effect.food, 4);
        o.usedEffects.Add(Effect.cozy, 2);

        AddNewObject(FurnitureName.rockingChair);
        o.type = FurnitureType._object;
        o.group = FurnitureGroup.chair;
        o.roomTypes.Add(RoomType.livingroom);
        o.flatWideEffect = false;
        o.size = 1;
        o.price = 100;
        o.useLength = 10;
        o.usedEffects.Add(Effect.comfort, 4);
        o.usedEffects.Add(Effect.cozy, 2);

        AddNewObject(FurnitureName.rug);
        o.type = FurnitureType._decor;
        o.roomTypes.Add(RoomType.livingroom);
        o.roomTypes.Add(RoomType.bedroom);
        o.roomTypes.Add(RoomType.kitchen);
        o.roomTypes.Add(RoomType.bathroom);
        o.floorObject = true;
        o.flatWideEffect = false;
        o.size = 1;
        o.price = 100;
        o.useLength = 10;
        o.usedEffects.Add(Effect.comfort, 4);
        o.usedEffects.Add(Effect.cozy, 2);

        AddNewObject(FurnitureName.tv);
        o.type = FurnitureType._object;
        o.useFromGroup = FurnitureGroup.chair;
        o.roomTypes.Add(RoomType.livingroom);
        o.roomTypes.Add(RoomType.bedroom);
        o.flatWideEffect = false;
        o.size = 1;
        o.price = 100;
        o.useLength = 10;
        o.usedEffects.Add(Effect.entertainment, 5);
        o.usedEffects.Add(Effect.cozy, 2);

        AddNewObject(FurnitureName.yarnBasket);
        o.type = FurnitureType._object;
        o.useFromGroup = FurnitureGroup.chair;
        o.roomTypes.Add(RoomType.livingroom);
        o.roomTypes.Add(RoomType.bedroom);
        o.flatWideEffect = false;
        o.size = 1;
        o.price = 100;
        o.useLength = 10;
        o.usedEffects.Add(Effect.comfort, 4);
        o.usedEffects.Add(Effect.cozy, 2);

        AddNewObject(FurnitureName.stonePainting);
        o.type = FurnitureType._object;
        o.roomTypes.Add(RoomType.livingroom);
        o.roomTypes.Add(RoomType.bedroom);
        o.flatWideEffect = false;
        o.size = 1;
        o.price = 100;
        o.useLength = 10;
        o.usedEffects.Add(Effect.comfort, 4);
        o.usedEffects.Add(Effect.cozy, 2);

        AddNewObject(FurnitureName.roarRock);
        o.type = FurnitureType._object;
        o.roomTypes.Add(RoomType.livingroom);
        o.roomTypes.Add(RoomType.bedroom);
        o.flatWideEffect = false;
        o.size = 1;
        o.price = 100;
        o.useLength = 10;
        o.usedEffects.Add(Effect.comfort, 4);
        o.usedEffects.Add(Effect.cozy, 2);

        if (Settings.furnitureUnlocked)
            startingAmountOfUnlockedFurniture = objectsList.Count();//2;
        else
            startingAmountOfUnlockedFurniture = 2;

        for (int i = 0; i < startingAmountOfUnlockedFurniture; i++)
        {
            ChooseNewFurnitureToUnlock();
        }

    }

    void SetupCharacters()
    {
        #region setup
        CharacterType type;

        Dictionary<Effect, EffectProperties> myEffects =new Dictionary<Effect, EffectProperties>();

        void NewEffect(ref Dictionary<Effect, EffectProperties>  myEffects, Effect type, float strength, float needBleedRate, float desireGrowRate, DesireAction action = DesireAction.none, bool syncDesireWithNeed = false)
        {

            myEffects.Add(type, new EffectProperties(strength, needBleedRate, desireGrowRate, action, syncDesireWithNeed));  
        }
        #endregion


        /*
                name = "emo";
                usedEffects.Add(Effect.comfort, -1);
                usedEffects.Add(Effect.grim, 2);
                #region apply
                charactersList.Add(new Character(name, seenEffects, usedEffects, flatEffects));
                ClearCharacterListData(ref seenEffects, ref usedEffects,  ref flatEffects);
                #endregion

                name = "artist";
                usedEffects.Add(Effect.comfort, -1);
                usedEffects.Add(Effect.grim, 2);
                #region apply
                charactersList.Add(new Character(name, seenEffects, usedEffects, flatEffects));
                ClearCharacterListData(ref seenEffects, ref usedEffects, ref flatEffects);
                #endregion
        */
        var sleepyR = 0.001f;
        var hungryR = 0.01f;
        var hygieneR = 0.001f;
        var socialR = 0.0001f;
        var desireR = 0.0001f;

        type = CharacterType.granny;
        NewEffect(ref myEffects, Effect.food,           2, hungryR, 0);
        NewEffect(ref myEffects,Effect.sleep,           3, sleepyR, 0);
        NewEffect(ref myEffects,Effect.hygiene,         3, hygieneR, 0);
        NewEffect(ref myEffects,Effect.social,           1, socialR, 0);
        NewEffect(ref myEffects,Effect.safety,           3, 0, 0);
        NewEffect(ref myEffects,Effect.comfort,         3, 0, 0);
        NewEffect(ref myEffects,Effect.romance,        0, 0, 0);
        NewEffect(ref myEffects,Effect.noise,          -3, 0, 0);
        NewEffect(ref myEffects,Effect.music,           2, 0, 0);
        NewEffect(ref myEffects,Effect.painting,         0, 0, 0);
        NewEffect(ref myEffects,Effect.entertainment,    5, 0, 0);
        NewEffect(ref myEffects,Effect.grunge,         -3, 0, 0);
        NewEffect(ref myEffects,Effect.cozy,            5, 0, 0);
        NewEffect(ref myEffects,Effect.vintage,          3, 0, 0);
        NewEffect(ref myEffects,Effect.academic,        1, 0, 0);
        NewEffect(ref myEffects,Effect.hunting,          1, 0, 0);
        #region apply
        var charInfo = new Character(type, myEffects);
        charactersList.Add(charInfo);
        ClearCharacterListData(ref myEffects);
        #endregion

        type = CharacterType.punkRocker;
        NewEffect(ref myEffects,Effect.food,         0, 0, 0);
        NewEffect(ref myEffects,Effect.sleep,        1, sleepyR, 0);
        NewEffect(ref myEffects,Effect.hygiene,      -1, 0, 0);
        NewEffect(ref myEffects,Effect.social,        1, socialR, 0);
        NewEffect(ref myEffects,Effect.safety,        0, 0, 0);
        NewEffect(ref myEffects,Effect.comfort,      1, 0, 0);
        NewEffect(ref myEffects,Effect.romance,     0, 0, 0);
        NewEffect(ref myEffects,Effect.noise,        3, 0, 0);
        NewEffect(ref myEffects,Effect.music,        4, 0, 0);
        NewEffect(ref myEffects,Effect.painting,      0, 0, 0);
        NewEffect(ref myEffects,Effect.entertainment, 1, 0, 0);
        NewEffect(ref myEffects,Effect.grunge,       5, 0, 0);
        NewEffect(ref myEffects,Effect.cozy,        -4, 0, 0);
        NewEffect(ref myEffects,Effect.vintage,       3, 0, 0);
        NewEffect(ref myEffects,Effect.academic,    -1, 0, 0);
        NewEffect(ref myEffects,Effect.hunting,       0, 0, 0);

        #region apply
        charactersList.Add(new Character(type, myEffects));
        ClearCharacterListData(ref myEffects);
        #endregion

        type = CharacterType.yeti;
        NewEffect(ref myEffects,Effect.food,         3, hungryR*2, 0);
        NewEffect(ref myEffects,Effect.sleep,        1, sleepyR, 0);
        NewEffect(ref myEffects,Effect.hygiene,      -1, 0, 0);
        NewEffect(ref myEffects,Effect.social,        1, socialR, 0);
        NewEffect(ref myEffects,Effect.safety,        3, 0, 0);
        NewEffect(ref myEffects,Effect.comfort,      1, 0, 0);
        NewEffect(ref myEffects,Effect.romance,     0, 0, 0);
        NewEffect(ref myEffects,Effect.noise,        4, 0, 0);
        NewEffect(ref myEffects,Effect.music,        1, 0, 0);
        NewEffect(ref myEffects,Effect.painting,      2, 0, 0);
        NewEffect(ref myEffects,Effect.entertainment, 1, 0, 0);
        NewEffect(ref myEffects,Effect.grunge,       0, 0, 0);
        NewEffect(ref myEffects,Effect.cozy,         3, 0, desireR, DesireAction.hug, false);
        NewEffect(ref myEffects,Effect.vintage,       0, 0, 0);
        NewEffect(ref myEffects,Effect.academic,    -1, 0, 0);
        NewEffect(ref myEffects,Effect.hunting,       2, 0, 0);
        #region apply
        charactersList.Add(new Character(type, myEffects));
        ClearCharacterListData(ref myEffects);
        #endregion


 
        var startingAmountOfUnlocked = 1;
        if (Settings.charactersUnlocked)
            startingAmountOfUnlocked = charactersList.Count();//2;

            

        for (int i = 0; i < startingAmountOfUnlocked; i++)
        {
            ChooseNewCharacterToUnlock();
        }

    }

    void SetupFlats()
    {
        AddNewFlat(0, ColorGrey); 
        var firstflat=AddNewFlat(1, ColorBlue, 
            new Room(1,RoomType.none),
            new Room(2, RoomType.none),
            new Room(3, RoomType.none),
            new Room(4, RoomType.none),
            new Room(4, RoomType.none));

 
    }

    void SetupUI()
    {
        placeItemImage = GetNode<Node2D>("PlaceItemImage").GetNode<Sprite2D>("Sprite2D");
        MyUnlocksLabel = GetNode<Godot.CanvasLayer>("CanvasLayer").GetNode<Label>("UnlocksLabel");
    }

    void SetupFirstFlat()
    {
        if (SpawnInitialObjects == false) return;
        var theObject = GetObjectFromType(FurnitureName.couch);

        PlaceObjects(ref theObject, true, new Vector2(576, 237));
        theObject = GetObjectFromType(FurnitureName.electricGuitar);

        PlaceObjects(ref theObject, true, new Vector2(376, 437));
        theObject = GetObjectFromType(FurnitureName.recordPlayer);

        PlaceObjects(ref theObject, true, new Vector2(656, 437));
        var theCharacter = GetCharacterFromType(CharacterType.granny);
        PlaceCharacter(ref theCharacter , ref theObject, true, new Vector2(276, 237));
    }
    #endregion



 

    void Start()
    {
        
        SetupUI();
        SetupObjects();
        SetupCharacters();
        SetupFlats();
        SetupFirstFlat();

        




    }

    void Run()
    {

        HotKeys();

        if (canPlace) 
        { 
            PlaceObjects(ref HeldObject, false, new Vector2(0,0));
            PlaceCharacter(ref HeldCharacter, ref HeldObject, false, new Vector2(0, 0));
            
            if(HeldObject==null && HeldCharacter==null) 
                placeItemImage.Texture = null;
        }
        else
            placeItemImage.Texture = null;

        UnlockNewCharacters();


    }
    void UnlockNewCharacters()
    {
        if (Money > 400 && highestUnlock<400)
        {
            highestUnlock = 400;
            UnlockNewCharacter();
        }

    }
    Object ChooseNewFurnitureToUnlock()
    {
        Random random = new Random();
        Object chosenItem;
        var tries = 50;
        var i = 0;
            do
            {
                

                chosenItem = objectsList.OrderBy(x => random.Next()).FirstOrDefault();
                i++;
            }
            while (FurnitureUnlockedList.Contains(chosenItem) && i< tries);

        if (i < tries)
            FurnitureUnlockedList.Add(chosenItem);
        else
            chosenItem = null;
        return chosenItem;
    }

    Character ChooseNewCharacterToUnlock()
    {
        Random random = new Random();
        Character chosenItem;
        var tries = 50;
        var i = 0;
        do
        {


            chosenItem = charactersList.OrderBy(x => random.Next()).FirstOrDefault();
            i++;
        }
        while ((CharactersAvailableToPlayerList.Contains(chosenItem) || PlacedCharactersList.Contains(chosenItem)) && i < tries);

        if(i< tries)
        CharactersAvailableToPlayerList.Add(chosenItem);
        else
            chosenItem = null;
        return chosenItem;
    }
    void UnlockNewFurniture( )
    {
        var unlocksLabelClass = (UnlocksLabel)MyUnlocksLabel;

        var furnitureItem = ChooseNewFurnitureToUnlock();
        if(furnitureItem!=null)
        unlocksLabelClass.NewUnlock($"{furnitureItem.type}");
    }

    void UnlockNewCharacter()
    {
        var unlocksLabelClass = (UnlocksLabel)MyUnlocksLabel;

        var furnitureItem = ChooseNewCharacterToUnlock();
        if (furnitureItem != null)
            unlocksLabelClass.NewUnlock($"{furnitureItem.type}");
    }
    void PlaceObjects(ref Object HeldObject, bool placeManually, Vector2 position)
    {

        if (HeldObject == null) return;


        Room roomWeAreIn = null;
        if(FlatNumberMouseIsIn > 0 && RoomNumberMouseIsIn>0)
            roomWeAreIn= flatsList[FlatNumberMouseIsIn].rooms[RoomNumberMouseIsIn] ;
       
        

        if ((RoomNumberMouseIsIn > -1 && roomWeAreIn!=null) &&
            (HeldObject.type == FurnitureType._core && (roomWeAreIn.type == RoomType.none || roomWeAreIn.type == HeldObject.roomTypes[0])) ||
                (HeldObject.type != FurnitureType._core && HeldObject.roomTypes.Contains(roomWeAreIn.type)))
            OverPlaceableTile = true;
        else
            OverPlaceableTile = false;


        placeItemImage.GlobalPosition = GameTileGrid.cellCoordinates * MyTileMap.TileSet.TileSize + MyTileMap.TileSet.TileSize / 2;
        placeItemImage.Texture = HeldObject.texture;
        if (OverPlaceableTile)
            ChangeColor(placeItemImage, ColorPlace);
        else
            ChangeColor(placeItemImage, ColorCantBePlaced);

        var cost = HeldObject.price;
        if ((KeyPressed("RightClick") && Money >= cost && OverPlaceableTile) || placeManually)
        {


            var newObject = Add2DNode("res://OBJECTS/SCENES/object.tscn", this);
            var newObjectClass = (Furniture)newObject;
            var newObjectData = newObjectClass.objectData;
            newObjectData = HeldObject;
            newObjectClass.objectData = newObjectData;
            newObjectClass.myShadow.Texture = HeldObject.shadowTexture;
            newObjectClass.mySprite.Texture = placeItemImage.Texture;
            newObjectClass.flatIAmIn = FlatNumberMouseIsIn;
            newObjectClass.roomIAmIn = RoomNumberMouseIsIn;
            newObjectClass.myRoom = roomWeAreIn;
            if (newObjectData.floorObject)
                newObjectClass.mySprite.ZIndex = -2000;

            if (HeldObject.type == FurnitureType._core)
            {
                roomWeAreIn.type = newObjectData.roomTypes[0];
                GameTileGrid.SetFloorMaterial(RoomNumberMouseIsIn, roomWeAreIn.type);
            }


            roomWeAreIn.objects.Add(newObject);
            flatsList[FlatNumberMouseIsIn].objects.Add(newObject);

            newObjectClass.ChangeDimensions(newObjectData.size);



            if (placeManually)
            {
                newObject.GlobalPosition = position;


            }

            else
            {
                UnlockNewFurniture();
                newObject.GlobalPosition = placeItemImage.GlobalPosition;

                Money -= cost;
            }




            if (placeManually == false)
                if (HeldCharacter != null || HeldObject != null)
                {
                    HoldNothing();

                    Destroy(Main.SelectionMenuOpen);
                    Main.SelectionMenuOpen = null;

                }
        }
    }


    void PlaceCharacter(ref Character heldCharacter, ref Object heldObject, bool placeManually, Vector2 position)
    {
        if (heldCharacter == null) return;


            placeItemImage.GlobalPosition = GetGlobalMousePosition();
        placeItemImage.Texture = heldCharacter.texture;

            if (KeyPressed("RightClick") || placeManually)
            {
            PlacedCharactersList.Add(heldCharacter);

            Log("CHARACTER PLACED", LogType.game);

            var newObject = Add2DNode("res://CHARACTERS/SCENES/character.tscn", this);
            if (placeManually)
                newObject.GlobalPosition = position;
            else
                newObject.GlobalPosition = GetGlobalMousePosition();
            var character = heldCharacter;
            Main.CharactersAvailableToPlayerList = Main.CharactersAvailableToPlayerList.Where(x => x != character).ToList();

            var newCharacterClass = (Characters)newObject;
                newCharacterClass.characterData = heldCharacter;
                newCharacterClass.SetupBleedList();
                newCharacterClass.mySprite.Texture = placeItemImage.Texture;
                newCharacterClass.myShadow.Texture = heldCharacter.shadowTexture;
                newCharacterClass.myFlatNumber = FlatNumberMouseIsIn;
                flatsList[FlatNumberMouseIsIn].charactersInFlat.Add(newCharacterClass);
                if(placeManually==false)
                if (heldCharacter != null || heldObject != null)
                {
                    HoldNothing();
                    Destroy(Main.SelectionMenuOpen);
                    Main.SelectionMenuOpen = null;

                }
                }
        
    }   



    public static void HoldNothing()
    {
        HeldObject = null;
        HeldCharacter = null;
    }
    void HotKeys()
    {
        if(KeyPressed("Restart") && QuickRestart)
        {
            RestartScene(this); 
        }
        if (KeyPressed("Money") && MoneyCheat)
        {
            Money += 100;
        }
        if (KeyPressed("UnlockObject") && UnlockCheat)
        {
            UnlockNewFurniture();
        }
        
    }

    void ClearObjectListData( ref Dictionary<Effect, int> usedEffects, ref Vector2 usePosition, ref List<FurnitureGroup> furnitureGroups,  ref FurnitureGroup isGroupLeader)
    {
     
        usedEffects.Clear();
        furnitureGroups.Clear();
        usePosition.X = 0;
        usePosition.Y = 0;
        isGroupLeader = FurnitureGroup.none;
    }

    void ClearCharacterListData( ref Dictionary<Effect, EffectProperties> usedEffects)
    {
       
        usedEffects.Clear();

    }

    Flat AddNewFlat(int flatNumber, Color color, params Room[] rooms  )
    {
        var newFlat = new Flat(flatNumber, color, rooms);
        
        flatsList.Add(newFlat);
        return newFlat;
    }

    Object GetObjectFromType(FurnitureName name)
    {
        var correctObject = objectsList.FirstOrDefault(obj => obj.name == name);
        return correctObject;
    }

    Character GetCharacterFromType(CharacterType type)
    {
        var correctObject = charactersList.FirstOrDefault(obj => obj.type == type);
        return correctObject;
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
