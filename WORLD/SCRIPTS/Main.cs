using Godot;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Xml.Linq;
using static Asriela.BasicFunctions;

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
    public static Dictionary<int,List<Node2D>> objectsInFlat = new Dictionary<int, List<Node2D>>(); 
    public static int FlatNumberMouseIsIn = 1;
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
            public FurnitureType type { get; set; }
            public bool flatWideEffect { get; set; }
            public string image_path { get; set; }
            public Texture2D texture { get; set; }
            public Dictionary<Effect, int> usedEffects { get; set; }
            public Vector2 usePosition { get; set; }
            public float useLength { get; set; }
            public int size { get; set; }
            public int price { get; set; }
            public Dictionary<Effect, int> flatEffects { get; set; }
            public FurnitureGroup isGroupLeader { get; set; }
            public List<FurnitureGroup> furnitureGroups { get; set; }

        public Object(FurnitureType type, FurnitureGroup isGroupLeader, List<FurnitureGroup> furnitureGroups, bool flatWideEffect,  int size,int price, Dictionary<Effect, int> usedEffects, Vector2 usePosition, float useLength,Dictionary<Effect, int> flatEffects)
            {
                this.type = type;
                this.isGroupLeader = isGroupLeader;
                this.furnitureGroups = new List<FurnitureGroup>(furnitureGroups);
                this.flatWideEffect = flatWideEffect;
                image_path = $"res://OBJECTS/SPRITES/{type}.png";
                texture = (Texture2D)GetResource(image_path);
                
                
                this.size = size;
                this.price = price;
                this.usePosition = usePosition;
                this.useLength = useLength;
                this.usedEffects = new Dictionary<Effect, int>(usedEffects);
                this.flatEffects = new Dictionary<Effect, int>(flatEffects);
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
            public Dictionary<Effect,EffectProperties> effectsList { get; set; }


        public Character(CharacterType type, Dictionary<Effect, EffectProperties> effectsList)
            {
                this.type = type;
                image_path = $"res://CHARACTERS/SPRITES/{type}.png";
                texture = (Texture2D)GetResource(image_path);
                this.effectsList = new Dictionary<Effect, EffectProperties>(effectsList);

            }
        }
        public static List<Character> charactersList = new List<Character>();
        public class Flat
        {
            public int number { get; set; }
            public string image_path { get; set; }
            public Color color { get; set; }
            public double happiness { get; set; }
            public List<Characters> charactersInFlat { get; set; }


            public Flat(int number, Color color, List<Characters> charactersInFlat)
            {
                this.number = number;
                image_path = $"res://FLATS/SPRITES/{number}.png";
                this.color = color;
                this.charactersInFlat = charactersInFlat;
            }
        }
        public static List<Flat> flatsList = new List<Flat>();

    #endregion

    #region COLOR SETUPS
    public static readonly Color ColorCantBePlaced = new Color(0xff00008d);
    public static readonly Color ColorPlace = new Color(0x00ffff64);
    #endregion

    #region SETUPS

    void SetupObjects()
    {
        #region setup
        FurnitureType type;
        FurnitureGroup isGroupLeader;
        List<FurnitureGroup> furnitureGroups = new List<FurnitureGroup>();
        bool flatWideEffect;
        Dictionary<Effect, int> seenEffects = new Dictionary<Effect, int>();
        Dictionary<Effect, int> usedEffects = new Dictionary<Effect, int>();
        Vector2 usePosition = new Vector2(0, 0);
        float useLength = 1;
        int size = 0;
        int price = 0;
        Dictionary<Effect, int> flatEffects = new Dictionary<Effect, int>();
        #endregion



        type = FurnitureType.couch;
        isGroupLeader = FurnitureGroup.couch;     
        flatWideEffect = false;
        size = 2;
        price = 100;
        useLength = 10;
        usedEffects.Add(Effect.comfort, 1);
        #region apply
        objectsList.Add(new Object(type, isGroupLeader, furnitureGroups, flatWideEffect,size, price,usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref furnitureGroups, ref isGroupLeader);
        #endregion

        type = FurnitureType.electricGuitar;
        isGroupLeader = FurnitureGroup.guitar;
        flatWideEffect = true;
        size = 1;
        price = 200;
        useLength = 10;
        usedEffects.Add(Effect.music, 3);
        usedEffects.Add(Effect.noise, 3);
        usedEffects.Add(Effect.grunge, 2);
        #region apply
        objectsList.Add(new Object(type, isGroupLeader, furnitureGroups, flatWideEffect,size, price, usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref furnitureGroups, ref isGroupLeader);
        #endregion

        type = FurnitureType.recordPlayer;
        flatWideEffect = true;
        size = 1;
        price = 100;
        useLength = 10;
        furnitureGroups.Add(FurnitureGroup.couch);
        furnitureGroups.Add(FurnitureGroup.chair);

        usedEffects.Add(Effect.music, 3);
        usedEffects.Add(Effect.vintage, 3);
        #region apply
        objectsList.Add(new Object(type, isGroupLeader, furnitureGroups,flatWideEffect, size, price, usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref furnitureGroups, ref isGroupLeader);
        #endregion

        type = FurnitureType.stove;
        isGroupLeader = FurnitureGroup.stove;
        flatWideEffect = false;
        size = 1;
        price = 200;
        useLength = 10;

        usedEffects.Add(Effect.food, 4);
        usedEffects.Add(Effect.cozy,2);
        #region apply
        objectsList.Add(new Object(type, isGroupLeader, furnitureGroups, flatWideEffect , size, price, usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref furnitureGroups, ref isGroupLeader);
        #endregion

        type = FurnitureType.fridge;
        isGroupLeader = FurnitureGroup.none;
        flatWideEffect = false;
        size = 1;
        price = 200;
        useLength = 10;
        furnitureGroups.Add(FurnitureGroup.stove);
        usedEffects.Add(Effect.food, 4);
        usedEffects.Add(Effect.cozy, 2);
        #region apply
        objectsList.Add(new Object(type, isGroupLeader, furnitureGroups, flatWideEffect, size, price, usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref furnitureGroups, ref isGroupLeader);
        #endregion

        type = FurnitureType.counterTop;
        isGroupLeader = FurnitureGroup.none;
        flatWideEffect = false;
        size = 1;
        price = 100;
        useLength = 10;
        furnitureGroups.Add(FurnitureGroup.stove);

        usedEffects.Add(Effect.food, 4);
        usedEffects.Add(Effect.cozy, 2);
        #region apply
        objectsList.Add(new Object(type, isGroupLeader, furnitureGroups, flatWideEffect, size, price, usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref furnitureGroups, ref isGroupLeader);
        #endregion
        type = FurnitureType.sideCountertop;
        isGroupLeader = FurnitureGroup.none;
        flatWideEffect = false;
        size = 1;
        price = 100;
        useLength = 10;
        furnitureGroups.Add(FurnitureGroup.stove);
        usedEffects.Add(Effect.food, 4);
        usedEffects.Add(Effect.cozy, 2);
        #region apply
        objectsList.Add(new Object(type, isGroupLeader, furnitureGroups, flatWideEffect, size, price, usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref furnitureGroups, ref isGroupLeader);
        #endregion
        type = FurnitureType.rockingChair;
        isGroupLeader = FurnitureGroup.chair;
        flatWideEffect = false;
        size = 1;
        price = 100;
        useLength = 10;

        usedEffects.Add(Effect.comfort, 4);
        usedEffects.Add(Effect.cozy, 2);
        #region apply
        objectsList.Add(new Object(type, isGroupLeader, furnitureGroups, flatWideEffect, size, price, usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref furnitureGroups, ref isGroupLeader);
        #endregion
        type = FurnitureType.rug;
        isGroupLeader = FurnitureGroup.none;
        flatWideEffect = false;
        size = 1;
        price = 100;
        useLength = 10;
        furnitureGroups.Add(FurnitureGroup.chair);
        furnitureGroups.Add(FurnitureGroup.couch);
        furnitureGroups.Add(FurnitureGroup.bed);
        usedEffects.Add(Effect.comfort, 4);
        usedEffects.Add(Effect.cozy, 2);
        #region apply
        objectsList.Add(new Object(type, isGroupLeader, furnitureGroups, flatWideEffect, size, price, usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref furnitureGroups, ref isGroupLeader);
        #endregion
        type = FurnitureType.tv;
        isGroupLeader = FurnitureGroup.none;
        flatWideEffect = false;
        size = 1;
        price = 100;
        useLength = 10;
        furnitureGroups.Add(FurnitureGroup.couch);
        usedEffects.Add(Effect.comfort, 4);
        usedEffects.Add(Effect.cozy, 2);
        #region apply
        objectsList.Add(new Object(type, isGroupLeader, furnitureGroups, flatWideEffect, size, price, usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref furnitureGroups, ref isGroupLeader);
        #endregion
        type = FurnitureType.yarnBasket;
        isGroupLeader = FurnitureGroup.none;
        flatWideEffect = false;
        size = 1;
        price = 100;
        useLength = 10;
        furnitureGroups.Add(FurnitureGroup.chair);
        usedEffects.Add(Effect.comfort, 4);
        usedEffects.Add(Effect.cozy, 2);
        #region apply
        objectsList.Add(new Object(type, isGroupLeader, furnitureGroups, flatWideEffect, size, price, usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref furnitureGroups, ref isGroupLeader);
        #endregion
        type = FurnitureType.stonePainting;
        isGroupLeader = FurnitureGroup.painting;
        flatWideEffect = false;
        size = 1;
        price = 100;
        useLength = 10;

        usedEffects.Add(Effect.comfort, 4);
        usedEffects.Add(Effect.cozy, 2);
        #region apply
        objectsList.Add(new Object(type, isGroupLeader, furnitureGroups, flatWideEffect, size, price, usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref furnitureGroups, ref isGroupLeader);
        #endregion
        type = FurnitureType.roarRock;
        isGroupLeader = FurnitureGroup.guitar;
        flatWideEffect = false;
        size = 1;
        price = 100;
        useLength = 10;

        usedEffects.Add(Effect.comfort, 4);
        usedEffects.Add(Effect.cozy, 2);
        #region apply
        objectsList.Add(new Object(type, isGroupLeader, furnitureGroups, flatWideEffect, size, price, usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref furnitureGroups, ref isGroupLeader);
        #endregion
        /* name = "paintEasel";
         usedEffects.Add(Effect.messy, 3);
         #region apply
         objectsList.Add(new Object(name, seenEffects, usedEffects, usePosition, flatEffects));
         ClearObjectListData(ref seenEffects, ref usedEffects, ref usePosition, ref flatEffects);
         #endregion
        */
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
        NewEffect(ref myEffects,Effect.entertainment,    1, 0, 0);
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

        ChooseNewCharacterToUnlock();
        ChooseNewCharacterToUnlock();


    }

    void SetupFlats()
    {
        AddNewFlat(0, ColorGrey, new List<Characters>()); 
        AddNewFlat(1, ColorBlue, new List<Characters>());
        AddNewFlat(2, ColorBlue, new List<Characters>());
        AddNewFlat(3, ColorBlue, new List<Characters>());
        AddNewFlat(4, ColorBlue, new List<Characters>());
    }

    void SetupUI()
    {
        placeItemImage = GetNode<Node2D>("PlaceItemImage").GetNode<Sprite2D>("Sprite2D");
        MyUnlocksLabel = GetNode<Godot.CanvasLayer>("CanvasLayer").GetNode<Label>("UnlocksLabel");
    }

    void SetupFirstFlat()
    {
        if (SpawnInitialObjects == false) return;
        var theObject = GetObjectFromType(FurnitureType.couch);

        PlaceObjects(ref theObject, true, new Vector2(576, 237));
        theObject = GetObjectFromType(FurnitureType.electricGuitar);

        PlaceObjects(ref theObject, true, new Vector2(376, 437));
        theObject = GetObjectFromType(FurnitureType.recordPlayer);

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

        if(lastFlatWeWereIn != FlatNumberMouseIsIn && HeldObject.isGroupLeader!=FurnitureGroup.none)
        {
            var tilemap = (GameTileGrid)MyTileMap;
            
            tilemap.FillFlatWithPlaceableArea();
            lastFlatWeWereIn = FlatNumberMouseIsIn;
        }


        placeItemImage.GlobalPosition = GameTileGrid.cellCoordinates* MyTileMap.TileSet.TileSize + MyTileMap.TileSet.TileSize / 2;
        placeItemImage.Texture = HeldObject.texture;
        if(OverPlaceableTile)
            ChangeColor(placeItemImage, ColorPlace);
        else
            ChangeColor(placeItemImage, ColorCantBePlaced);

            var cost = HeldObject.price;
        if ((KeyPressed("RightClick") && Money >= cost && OverPlaceableTile) || placeManually)
            {
                Main.MyTileMap.ClearLayer(1);
                var newObject=Add2DNode("res://OBJECTS/SCENES/object.tscn",this);
                var newObjectClass = (Furniture)newObject;
                newObjectClass.objectData = HeldObject;
                newObjectClass.objectData.type = HeldObject.type;
                newObjectClass.mySprite.Texture = placeItemImage.Texture;
                newObjectClass.flatIAmIn = FlatNumberMouseIsIn;
                if(HeldObject.isGroupLeader != FurnitureGroup.none)
                {
                    leaderFurnitureList.Add(newObjectClass);
                    newObjectClass.myLayer = leaderFurnitureList.Count;
                    newObjectClass.isALeader = true;
                }
                else
                {
                    newObjectClass.myLeader = leaderFurnitureList[OverLeaderFurnitureLayer-1];
                }
                
                objectsInFlat[FlatNumberMouseIsIn].Add(newObject);
                newObjectClass.ChangeDimensions(newObjectClass.objectData.size);
                

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

            var newObjectClass = (Characters)newObject;
                newObjectClass.characterData = heldCharacter;
                newObjectClass.SetupBleedList();
                newObjectClass.mySprite.Texture = placeItemImage.Texture;
                newObjectClass.myFlatNumber = FlatNumberMouseIsIn;
                flatsList[FlatNumberMouseIsIn].charactersInFlat.Add(newObjectClass);
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

    void AddNewFlat(int flatNumber, Color color, List<Characters> charactersInFlat )
    {
        flatsList.Add(new Flat(flatNumber, color, charactersInFlat));
        objectsInFlat[flatNumber] = new List<Node2D>();
    }

    Object GetObjectFromType(FurnitureType type)
    {
        var correctObject = objectsList.FirstOrDefault(obj => obj.type == type);
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
