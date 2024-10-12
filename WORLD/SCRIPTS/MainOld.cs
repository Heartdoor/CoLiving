﻿using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static Asriela.BasicFunctions;

public partial class MainOld : Node2D
{
    /*
    #region STATES
    public static Control SelectionMenuOpen = null;
    public static bool canPlace = true;
    #endregion

    #region MAIN CONTROL SETUP
    public static float Money = 200f;
    public static FurnitureItem HeldObject = null;
    public static CharacterItem HeldCharacter = null;


    public static int BuildingNumberMouseIsIn = 1;
    public static int RoomNumberMouseIsIn = 1;
    int lastFlatWeWereIn = 1;
    public static List<CharacterItem> CharactersAvailableToPlayerList = new List<CharacterItem>();
    public static List<CharacterItem> PlacedCharactersList = new List<CharacterItem>();
    public static List<FurnitureController> leaderFurnitureList = new List<FurnitureController>();
    public static int OverLeaderFurnitureLayer = 0;

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

    #region STATICS
    public static Characters SelectedCharacter = null;
    public static string DebugEffectOnMouse = "";

    #endregion

    #region CLASSES SETUP
    public static List<FurnitureItem> FurnitureUnlockedList = new List<FurnitureItem>();
    public int startingAmountOfUnlockedFurniture;
    public class FurnitureItem
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

        public FurnitureItem(FurnitureName name, bool flatWideEffect = false, int size = 1, int price = 0, float useLength = 10f)
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
    public static List<FurnitureItem> objectsList = new List<FurnitureItem>();

    public class EffectProperties
    {

        public float strength { get; set; }
        public float needBleedRate { get; set; }
        public float desireGrowRate { get; set; }
        public DesireAction action { get; set; }

        public float actionLength { get; set; }

        public float actionSatisfaction { get; set; }



        public bool syncDesireWithNeed { get; set; }

        public EffectProperties(float strength, float needBleedRate, float desireGrowRate, DesireAction action, float actionLength, float actionSatisfaction, bool syncDesireWithNeed)
        {

            this.strength = strength;
            this.needBleedRate = needBleedRate;
            this.desireGrowRate = desireGrowRate;
            this.action = action;
            this.syncDesireWithNeed = syncDesireWithNeed;
            this.actionLength = actionLength;
            this.actionSatisfaction = actionSatisfaction;
        }
    }

    public class Relationship
    {
        public Dictionary<RelationshipType, float> strength { get; set; }

        public Relationship()
        {
            strength = new Dictionary<RelationshipType, float>();
            strength[RelationshipType.friendship] = 0;
            strength[RelationshipType.romantic] = 0;
        }

    }
    public class CharacterItem
    {
        public CharacterType name { get; set; }
        public string image_path { get; set; }
        public Texture2D texture { get; set; }
        public Texture2D shadowTexture { get; set; }
        public Dictionary<Effect, EffectProperties> effectsList { get; set; }

        public Dictionary<Characters, Relationship> relationshipsList { get; set; }

        public Dictionary<Effect, float> needs { get; set; }

        public Dictionary<Effect, float> feelings { get; set; }
        public Dictionary<Effect, float> desires { get; set; }

        public Emotion mainEmotion { get; set; }

        public bool debugItem { get; set; }

        public string emoji { get; set; }

        public CharacterItem(CharacterType name, string emoji, bool debugItem, Dictionary<Effect, EffectProperties> effectsList)
        {
            this.name = name;
            image_path = $"res://CHARACTERS/SPRITES/{name}.png";
            texture = GetTexture2D(image_path);
            shadowTexture = GetTexture2D($"res://CHARACTERS/SPRITES/{name}_s.png");
            this.effectsList = new Dictionary<Effect, EffectProperties>(effectsList);
            relationshipsList = new Dictionary<Characters, Relationship>();
            feelings = new Dictionary<Effect, float>();
            feelings.Add(Effect.happiness, 0);
            feelings.Add(Effect.romance, 0);
            needs = new Dictionary<Effect, float>();
            desires = new Dictionary<Effect, float>();
            mainEmotion = Emotion.neutral;
            this.emoji = emoji;
            this.debugItem = debugItem;
        }
    }
    public static List<CharacterItem> charactersList = new List<CharacterItem>();



    #endregion

    #region COLOR SETUPS
    public static readonly Color ColorCantBePlaced = new Color(0xff00008d);
    public static readonly Color ColorPlace = new Color(0x00ffff64);
    #endregion

    #region LOCAL SETUPS
    FurnitureItem o;
    DebugHotkeys debugHotkeys = new DebugHotkeys();
    #endregion

    #region SETUPS


    void AddNewObject(FurnitureName name)
    {

        o = new FurnitureItem(name);
        objectsList.Add(o);

    }
    void SetupObjects()
    {

        AddNewObject(FurnitureName.debug1);
        o.debugItem = true;
        o.type = FurnitureType._core;
        o.roomTypes.Add(RoomType.livingroom);
        o.flatWideEffect = false;
        o.size = 1;
        o.price = 0;
        o.useLength = 10;
        o.rotation = Direction.down;
        o.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down, AccessPosition.up, AccessPosition.left, AccessPosition.right });
        o.useAnimation = UseAnimation.idle;
        o.basicEffects.Add(Effect.debug1, 5);


        AddNewObject(FurnitureName.couch);
        o.type = FurnitureType._core;
        o.group = FurnitureGroup.chair;
        o.roomTypes.Add(RoomType.livingroom);
        o.flatWideEffect = false;
        o.size = 2;
        o.price = 100;
        o.useLength = 10;
        o.ontopUsePosition = true;
        o.rotation = Direction.down;
        o.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down });
        o.useAnimation = UseAnimation.sit;
        o.basicEffects.Add(Effect.comfort, 1);



        AddNewObject(FurnitureName.electricGuitar);
        o.type = FurnitureType._object;
        o.roomTypes.Add(RoomType.livingroom);
        o.flatWideEffect = true;
        o.size = 1;
        o.price = 200;
        o.useLength = 10;
        o.rotation = Direction.down;
        o.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down, AccessPosition.up, AccessPosition.left, AccessPosition.right });
        o.useAnimation = UseAnimation.strumGuitar;
        o.basicEffects.Add(Effect.music, 3);
        o.basicEffects.Add(Effect.noise, 3);
        o.basicEffects.Add(Effect.grunge, 2);

        o.usedRadiantEffects.Add(Effect.noise, 2);
        o.usedRadiantEffects.Add(Effect.grunge, 2);

        AddNewObject(FurnitureName.recordPlayer);
        o.type = FurnitureType._object;
        o.useFromGroup = FurnitureGroup.chair;
        o.roomTypes.Add(RoomType.livingroom);
        o.flatWideEffect = true;
        o.size = 1;
        o.price = 100;
        o.useLength = 10;
        o.rotation = Direction.down;
        o.useAnimation = UseAnimation.listenToMusic;
        o.basicEffects.Add(Effect.music, 4);
        o.basicEffects.Add(Effect.vintage, 3);
        o.usedRadiantEffects.Add(Effect.music, 2);
        o.usedRadiantEffects.Add(Effect.vintage, 1);

        AddNewObject(FurnitureName.stove);
        o.type = FurnitureType._core;
        o.roomTypes.Add(RoomType.kitchen);
        o.flatWideEffect = true;
        o.size = 1;
        o.price = 200;
        o.useLength = 10;
        o.rotation = Direction.up;
        o.useAnimation = UseAnimation.cook;
        o.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down });
        o.basicEffects.Add(Effect.food, 4);
        o.basicEffects.Add(Effect.cozy, 2);
        o.usedRadiantEffects.Add(Effect.cozy, 2);

        AddNewObject(FurnitureName.fridge);
        o.type = FurnitureType._core;
        o.roomTypes.Add(RoomType.kitchen);
        o.flatWideEffect = false;
        o.size = 1;
        o.price = 200;
        o.useLength = 10;
        o.rotation = Direction.right;
        o.useAnimation = UseAnimation.idle;
        o.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down });
        o.basicEffects.Add(Effect.food, 4);
        o.basicEffects.Add(Effect.cozy, 2);

        AddNewObject(FurnitureName.counterTop);
        o.type = FurnitureType._core;
        o.roomTypes.Add(RoomType.kitchen);
        o.flatWideEffect = false;
        o.size = 1;
        o.price = 100;
        o.useLength = 10;
        o.rotation = Direction.down;
        o.useAnimation = UseAnimation.idle;
        o.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down });
        o.basicEffects.Add(Effect.food, 4);
        o.basicEffects.Add(Effect.cozy, 2);



        AddNewObject(FurnitureName.rockingChair);
        o.debugItem = true;
        o.type = FurnitureType._object;
        o.group = FurnitureGroup.chair;
        o.roomTypes.Add(RoomType.livingroom);
        o.flatWideEffect = false;
        o.size = 1;
        o.price = 100;
        o.useLength = 10;
        o.ontopUsePosition = true;
        o.rotation = Direction.left;
        o.useAnimation = UseAnimation.sit;
        o.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down });
        o.basicEffects.Add(Effect.comfort, 4);
        o.basicEffects.Add(Effect.cozy, 2);

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
        o.rotation = Direction.down;
        o.basicEffects.Add(Effect.comfort, 4);
        o.basicEffects.Add(Effect.cozy, 2);

        AddNewObject(FurnitureName.smallSideTable);
        o.type = FurnitureType._decor;
        o.roomTypes.Add(RoomType.livingroom);
        o.roomTypes.Add(RoomType.bedroom);
        o.roomTypes.Add(RoomType.bathroom);
        o.size = 1;
        o.price = 50;
        o.useLength = 10;
        o.rotation = Direction.down;
        o.basicEffects.Add(Effect.comfort, 4);
        o.basicEffects.Add(Effect.cozy, 2);

        AddNewObject(FurnitureName.tv);
        o.type = FurnitureType._object;
        o.useFromGroup = FurnitureGroup.chair;
        o.roomTypes.Add(RoomType.livingroom);
        o.roomTypes.Add(RoomType.bedroom);
        o.flatWideEffect = false;
        o.size = 1;
        o.price = 100;
        o.useLength = 10;
        o.rotation = Direction.up;
        o.useAnimation = UseAnimation.sit;
        o.basicEffects.Add(Effect.entertainment, 5);
        o.usedRadiantEffects.Add(Effect.entertainment, 2);

        AddNewObject(FurnitureName.yarnBasket);
        o.type = FurnitureType._object;
        o.useFromGroup = FurnitureGroup.chair;
        o.roomTypes.Add(RoomType.livingroom);
        o.roomTypes.Add(RoomType.bedroom);
        o.flatWideEffect = false;
        o.size = 1;
        o.price = 100;
        o.useLength = 10;
        o.rotation = Direction.down;
        o.useAnimation = UseAnimation.sitAndKnitt;
        o.basicEffects.Add(Effect.comfort, 4);
        o.basicEffects.Add(Effect.cozy, 2);

        AddNewObject(FurnitureName.stonePainting);
        o.debugItem = true;
        o.type = FurnitureType._object;
        o.roomTypes.Add(RoomType.livingroom);
        o.roomTypes.Add(RoomType.bedroom);
        o.flatWideEffect = false;
        o.size = 1;
        o.price = 100;
        o.useLength = 10;
        o.rotation = Direction.down;
        o.useAnimation = UseAnimation.idle;
        o.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down });
        o.basicEffects.Add(Effect.comfort, 4);
        o.basicEffects.Add(Effect.cozy, 2);

        AddNewObject(FurnitureName.roarRock);
        o.debugItem = true;
        o.type = FurnitureType._object;
        o.roomTypes.Add(RoomType.livingroom);
        o.roomTypes.Add(RoomType.bedroom);
        o.flatWideEffect = false;
        o.size = 1;
        o.price = 100;
        o.useLength = 10;
        o.rotation = Direction.down;
        o.ontopUsePosition = true;
        o.useAnimation = UseAnimation.roar;
        o.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down, AccessPosition.up, AccessPosition.left, AccessPosition.right });
        o.basicEffects.Add(Effect.comfort, 4);
        o.basicEffects.Add(Effect.cozy, 2);

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
        CharacterType name;
        string emoji;
        bool debugItem;

        Dictionary<Effect, EffectProperties> myEffects = new Dictionary<Effect, EffectProperties>();

        void NewEffect(ref Dictionary<Effect, EffectProperties> myEffects, Effect type, float strength, float needBleedRate, float desireGrowRate, DesireAction action = DesireAction.none, float actionLength = 10, float actionSatisfaction = 80, bool syncDesireWithNeed = false)
        {

            myEffects.Add(type, new EffectProperties(strength, needBleedRate, desireGrowRate, action, actionLength, actionSatisfaction, syncDesireWithNeed));
        }
        #endregion


        var sleepyR = 0.001f;
        var hungryR = 0.01f;
        var hygieneR = 0.001f;
        var socialR = 0.0001f;
        var desireR = 0.0001f;

        name = CharacterType.granny;
        emoji = "👵";
        debugItem = false;
        //add a new effect of social, likes socializing by 1, bleeds as a need by socialR, grows a desire to act on social by social r, will do action talk when desire is high enough, do social action for length of?, then reduce the desire by amount after action
        NewEffect(ref myEffects, Effect.social, 1, socialR, socialR, DesireAction.talk);
        //NewEffect(ref myEffects, Effect.social, -3, 0, 0, DesireAction.talk);
        NewEffect(ref myEffects, Effect.food, 2, hungryR, 0);
        NewEffect(ref myEffects, Effect.sleep, 3, 0, 0);
        NewEffect(ref myEffects, Effect.hygiene, 3, 0, 0);
        NewEffect(ref myEffects, Effect.safety, 3, 0, 0);
        NewEffect(ref myEffects, Effect.comfort, 3, 0, 0);
        NewEffect(ref myEffects, Effect.romance, 0, 0, 0);
        NewEffect(ref myEffects, Effect.noise, -3, 0, 0);
        NewEffect(ref myEffects, Effect.music, 2, 0, 0);
        NewEffect(ref myEffects, Effect.painting, 0, 0, 0);
        NewEffect(ref myEffects, Effect.entertainment, 2, 0, 0);
        NewEffect(ref myEffects, Effect.grunge, -3, 0, 0);
        NewEffect(ref myEffects, Effect.cozy, 5, 0, 0);
        NewEffect(ref myEffects, Effect.vintage, 3, 0, 0);
        NewEffect(ref myEffects, Effect.academic, 1, 0, 0);
        NewEffect(ref myEffects, Effect.hunting, 1, 0, 0);

        #region apply
        var charInfo = new CharacterItem(name, emoji, debugItem, myEffects);
        charactersList.Add(charInfo);
        ClearCharacterListData(ref myEffects);
        #endregion

        name = CharacterType.punkRocker;
        emoji = "👩‍🎤";
        debugItem = false;
        NewEffect(ref myEffects, Effect.food, 0, 0, 0);
        NewEffect(ref myEffects, Effect.sleep, 1, sleepyR, 0);
        NewEffect(ref myEffects, Effect.hygiene, -1, 0, 0);
        NewEffect(ref myEffects, Effect.social, 1, socialR, socialR * 10, DesireAction.talk);
        NewEffect(ref myEffects, Effect.safety, 0, 0, 0);
        NewEffect(ref myEffects, Effect.comfort, 1, 0, 0);
        NewEffect(ref myEffects, Effect.romance, 0, 0, 0);
        NewEffect(ref myEffects, Effect.noise, 3, 0, 0);
        NewEffect(ref myEffects, Effect.music, 4, 0, 0);
        NewEffect(ref myEffects, Effect.painting, 0, 0, 0);
        NewEffect(ref myEffects, Effect.entertainment, 3, 0, 0);
        NewEffect(ref myEffects, Effect.grunge, 5, 0, 0);
        NewEffect(ref myEffects, Effect.cozy, -4, 0, 0);
        NewEffect(ref myEffects, Effect.vintage, 3, 0, 0);
        NewEffect(ref myEffects, Effect.academic, -1, 0, 0);
        NewEffect(ref myEffects, Effect.hunting, 0, 0, 0);
        NewEffect(ref myEffects, Effect.debug1, 5, 0, 0);

        #region apply
        charactersList.Add(new CharacterItem(name, emoji, debugItem, myEffects));
        ClearCharacterListData(ref myEffects);
        #endregion

        name = CharacterType.yeti;
        emoji = "❄🐵";
        debugItem = true;
        NewEffect(ref myEffects, Effect.food, 3, hungryR * 2, 0);
        NewEffect(ref myEffects, Effect.sleep, 1, sleepyR, 0);
        NewEffect(ref myEffects, Effect.hygiene, -1, 0, 0);
        NewEffect(ref myEffects, Effect.social, 1, socialR, socialR, DesireAction.talk);
        NewEffect(ref myEffects, Effect.safety, 3, 0, 0);
        NewEffect(ref myEffects, Effect.comfort, 1, 0, 0);
        NewEffect(ref myEffects, Effect.romance, 0, 0, 0);
        NewEffect(ref myEffects, Effect.noise, 4, 0, 0);
        NewEffect(ref myEffects, Effect.music, 1, 0, 0);
        NewEffect(ref myEffects, Effect.painting, 2, 0, 0);
        NewEffect(ref myEffects, Effect.entertainment, 1, 0, 0);
        NewEffect(ref myEffects, Effect.grunge, 0, 0, 0);
        NewEffect(ref myEffects, Effect.cozy, 3, 0, desireR, DesireAction.hug);
        NewEffect(ref myEffects, Effect.vintage, 0, 0, 0);
        NewEffect(ref myEffects, Effect.academic, -1, 0, 0);
        NewEffect(ref myEffects, Effect.hunting, 2, 0, 0);
        #region apply
        charactersList.Add(new CharacterItem(name, emoji, debugItem, myEffects));
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


    void SetupUI()
    {
        placeItemImage = GetNode<Node2D>("PlaceItemImage").GetNode<Sprite2D>("Sprite2D");
        MyUnlocksLabel = GetNode<Godot.CanvasLayer>("CanvasLayer").GetNode<Label>("UnlocksLabel");
    }




    #endregion


    void SetupDebugHotkeys()
    {
        debugHotkeys.myMain = this;
        debugHotkeys.Start();
    }


    void Start()
    {

        SetupUI();
        SetupObjects();
        SetupCharacters();
        SetupDebugHotkeys();





    }

    void Run()
    {

        RunDebugHotkeys();


        if (canPlace)
        {
            PlaceObjects(ref HeldObject, false, new Vector2(0, 0));
            PlaceCharacter(ref HeldCharacter, false, new Vector2(0, 0));

            if (HeldObject == null && HeldCharacter == null)
                placeItemImage.Texture = null;
        }
        else
            placeItemImage.Texture = null;

        UnlockNewCharacters();


    }
    void UnlockNewCharacters()
    {
        if (Money > 400 && highestUnlock < 400)
        {
            highestUnlock = 400;
            UnlockNewCharacter();
        }

    }
    FurnitureItem ChooseNewFurnitureToUnlock()
    {
        Random random = new Random();
        FurnitureItem chosenItem;
        var tries = 50;
        var i = 0;
        do
        {


            chosenItem = objectsList.OrderBy(x => random.Next()).FirstOrDefault();
            i++;
        }
        while (FurnitureUnlockedList.Contains(chosenItem) && i < tries);

        if (i < tries)
            FurnitureUnlockedList.Add(chosenItem);
        else
            chosenItem = null;
        return chosenItem;
    }

    CharacterItem ChooseNewCharacterToUnlock()
    {
        Random random = new Random();
        CharacterItem chosenItem;
        var tries = 50;
        var i = 0;
        do
        {


            chosenItem = charactersList.OrderBy(x => random.Next()).FirstOrDefault();
            i++;
        }
        while ((CharactersAvailableToPlayerList.Contains(chosenItem) || PlacedCharactersList.Contains(chosenItem)) && i < tries);

        if (i < tries)
            CharactersAvailableToPlayerList.Add(chosenItem);
        else
            chosenItem = null;
        return chosenItem;
    }
    void UnlockNewFurniture()
    {
        var unlocksLabelClass = (UnlocksLabel)MyUnlocksLabel;

        var furnitureItem = ChooseNewFurnitureToUnlock();
        if (furnitureItem != null)
            unlocksLabelClass.NewUnlock($"{furnitureItem.type}");
    }

    void UnlockNewCharacter()
    {
        var unlocksLabelClass = (UnlocksLabel)MyUnlocksLabel;

        var furnitureItem = ChooseNewCharacterToUnlock();
        if (furnitureItem != null)
            unlocksLabelClass.NewUnlock($"{furnitureItem.name}");
    }
    public void PlaceObjects(ref FurnitureItem HeldObject, bool placeManually, Vector2 position)
    {

        if (HeldObject == null) return;


        BuildingController.RoomItem roomWeAreIn = null;
        if (BuildingNumberMouseIsIn > 0 && RoomNumberMouseIsIn > 0)
            roomWeAreIn = BuildingController.buildingsList[BuildingNumberMouseIsIn].rooms[RoomNumberMouseIsIn];

        OverPlaceableTile = false;

        if (roomWeAreIn != null && BuildingNumberMouseIsIn > 0 && RoomNumberMouseIsIn > -1)
            if ((HeldObject.type == FurnitureType._core && (roomWeAreIn.type == RoomType.none || roomWeAreIn.type == HeldObject.roomTypes[0])) ||
                  (HeldObject.type != FurnitureType._core && HeldObject.roomTypes.Contains(roomWeAreIn.type)))
                OverPlaceableTile = true;




        placeItemImage.GlobalPosition = GameTileGrid.cellCoordinates * MyTileMap.TileSet.TileSize + MyTileMap.TileSet.TileSize / 2;
        placeItemImage.Texture = HeldObject.texture;
        if (OverPlaceableTile)
            ChangeColor(placeItemImage, ColorPlace);
        else
            ChangeColor(placeItemImage, ColorCantBePlaced);

        var cost = HeldObject.price;
        if ((KeyPressed("RightClick") && Money >= cost && OverPlaceableTile) || placeManually)
        {


            var newObject = Add2DNode("res://FURNITURE/SCENES/object.tscn", this);
            var newObjectClass = (FurnitureController)newObject;
            var newObjectData = newObjectClass.furnitureData;
            newObjectData = HeldObject;
            newObjectClass.furnitureData = newObjectData;
            newObjectClass.myShadow.Texture = HeldObject.shadowTexture;
            newObjectClass.mySprite.Texture = placeItemImage.Texture;
            newObjectClass.myFlatNumber = BuildingNumberMouseIsIn;
            newObjectClass.roomIAmIn = RoomNumberMouseIsIn;
            newObjectClass.myRoom = roomWeAreIn;

            if (newObjectData.floorObject)
                newObjectClass.mySprite.ZIndex = -2000;

            if (HeldObject.type == FurnitureType._core)
            {
                roomWeAreIn.type = newObjectData.roomTypes[0];
                GameTileGrid.SetFloorMaterial(RoomNumberMouseIsIn, roomWeAreIn.type);
            }

            BuildingController.AddFurnitureToRoom(BuildingNumberMouseIsIn, roomWeAreIn, newObject);


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


    public Characters PlaceCharacter(ref CharacterItem heldCharacter, bool placeManually, Vector2 position)
    {
        if (heldCharacter == null) return null;

        Characters newCharacterClass = null;
        placeItemImage.GlobalPosition = GetGlobalMousePosition();
        placeItemImage.Texture = heldCharacter.texture;

        if (KeyPressed("RightClick") || placeManually)
        {
            PlacedCharactersList.Add(heldCharacter);



            var newObject = Add2DNode("res://CHARACTERS/SCENES/character.tscn", this);
            if (placeManually)
                newObject.GlobalPosition = position;
            else
                newObject.GlobalPosition = GetGlobalMousePosition();
            var character = heldCharacter;
            Main.CharactersAvailableToPlayerList = Main.CharactersAvailableToPlayerList.Where(x => x != character).ToList();

            newCharacterClass = (Characters)newObject;
            newCharacterClass.characterData = heldCharacter;
            newCharacterClass.SetupBleedList();
            newCharacterClass.myAnimator.SpriteFrames = (SpriteFrames)GetResource($"res://CHARACTERS/ANIMATIONS/{newCharacterClass.characterData.name}Animations.tres");
            newCharacterClass.myAnimator.Animation = "idle_neutral";
            newCharacterClass.myShadow.Texture = heldCharacter.shadowTexture;
            newCharacterClass.myBuildingNumber = BuildingNumberMouseIsIn;
            newCharacterClass.AddMyselfToEveryonesRelationshipsList();
            BuildingController.buildingsList[BuildingNumberMouseIsIn].charactersInBuilding.Add(newCharacterClass);
            if (placeManually == false)
                if (heldCharacter != null)
                {
                    HoldNothing();
                    Destroy(Main.SelectionMenuOpen);
                    Main.SelectionMenuOpen = null;

                }
        }
        return newCharacterClass;


    }



    public static void HoldNothing()
    {
        HeldObject = null;
        HeldCharacter = null;
    }
    void RunDebugHotkeys()
    {
        if (KeyPressed("Restart") && QuickRestart)
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

        debugHotkeys.Run();

    }

    void ClearObjectListData(ref Dictionary<Effect, int> usedEffects, ref Vector2 usePosition, ref List<FurnitureGroup> furnitureGroups, ref FurnitureGroup isGroupLeader)
    {

        usedEffects.Clear();
        furnitureGroups.Clear();
        usePosition.X = 0;
        usePosition.Y = 0;
        isGroupLeader = FurnitureGroup.none;
    }

    void ClearCharacterListData(ref Dictionary<Effect, EffectProperties> usedEffects)
    {

        usedEffects.Clear();

    }



    public static FurnitureItem GetObjectFromType(FurnitureName name)
    {
        var correctObject = objectsList.FirstOrDefault(obj => obj.name == name);
        return correctObject;
    }

    public static CharacterItem GetCharacterFromType(CharacterType name)
    {
        var correctObject = charactersList.FirstOrDefault(obj => obj.name == name);
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
*/
}
