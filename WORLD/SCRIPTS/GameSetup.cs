using Godot;
using System.Collections.Generic;
using System.Linq;
using static Asriela.BasicFunctions;

internal class GameSetup
{
    FurnitureItem furnitureItem;
    Main mainController = new Main();

    public void SetupStart()
    {
        SetupFurnitureItems();
        SetupCharacters();
    }

    void AddNewFurnitureItem(FurnitureName name)
    {

        furnitureItem = new FurnitureItem(name);
        // Referenced using class instead of instance because the lists are labelled as static
        Main.objectsList.Add(furnitureItem);

    }
    void SetupFurnitureItems()
    {

        AddNewFurnitureItem(FurnitureName.debug1);
        furnitureItem.debugItem = true;
        furnitureItem.type = FurnitureType._core;
        furnitureItem.roomTypes.Add(RoomType.livingroom);
        furnitureItem.flatWideEffect = false;
        furnitureItem.size = 1;
        furnitureItem.price = 0;
        furnitureItem.useLength = 10;
        furnitureItem.rotation = Direction.down;
        furnitureItem.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down, AccessPosition.up, AccessPosition.left, AccessPosition.right });
        furnitureItem.useAnimation = UseAnimation.idle;
        furnitureItem.usedEffects.Add(Effect.debug1, 5);


        AddNewFurnitureItem(FurnitureName.couch);
        furnitureItem.type = FurnitureType._core;
        furnitureItem.group = FurnitureGroup.chair;
        furnitureItem.roomTypes.Add(RoomType.livingroom);
        furnitureItem.flatWideEffect = false;
        furnitureItem.size = 2;
        furnitureItem.price = 100;
        furnitureItem.useLength = 10;
        furnitureItem.ontopUsePosition = true;
        furnitureItem.rotation = Direction.down;
        furnitureItem.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down });
        furnitureItem.useAnimation = UseAnimation.sit;
        furnitureItem.usedEffects.Add(Effect.comfort, 1);



        AddNewFurnitureItem(FurnitureName.electricGuitar);
        furnitureItem.type = FurnitureType._object;
        furnitureItem.roomTypes.Add(RoomType.livingroom);
        furnitureItem.flatWideEffect = true;
        furnitureItem.size = 1;
        furnitureItem.price = 200;
        furnitureItem.useLength = 10;
        furnitureItem.rotation = Direction.down;
        furnitureItem.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down, AccessPosition.up, AccessPosition.left, AccessPosition.right });
        furnitureItem.useAnimation = UseAnimation.strumGuitar;
        furnitureItem.usedEffects.Add(Effect.music, 3);
        furnitureItem.usedEffects.Add(Effect.noise, 3);
        furnitureItem.usedEffects.Add(Effect.grunge, 2);

        furnitureItem.usedRadiantEffects.Add(Effect.noise, 2);
        furnitureItem.usedRadiantEffects.Add(Effect.grunge, 2);

        AddNewFurnitureItem(FurnitureName.recordPlayer);
        furnitureItem.type = FurnitureType._object;
        furnitureItem.useFromGroup = FurnitureGroup.chair;
        furnitureItem.roomTypes.Add(RoomType.livingroom);
        furnitureItem.flatWideEffect = true;
        furnitureItem.size = 1;
        furnitureItem.price = 100;
        furnitureItem.useLength = 10;
        furnitureItem.rotation = Direction.down;
        furnitureItem.useAnimation = UseAnimation.listenToMusic;
        furnitureItem.usedEffects.Add(Effect.music, 4);
        furnitureItem.usedEffects.Add(Effect.vintage, 3);
        furnitureItem.usedRadiantEffects.Add(Effect.music, 2);
        furnitureItem.usedRadiantEffects.Add(Effect.vintage, 1);

        AddNewFurnitureItem(FurnitureName.stove);
        furnitureItem.type = FurnitureType._core;
        furnitureItem.roomTypes.Add(RoomType.kitchen);
        furnitureItem.flatWideEffect = true;
        furnitureItem.size = 1;
        furnitureItem.price = 200;
        furnitureItem.useLength = 10;
        furnitureItem.rotation = Direction.up;
        furnitureItem.useAnimation = UseAnimation.cook;
        furnitureItem.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down });
        furnitureItem.usedEffects.Add(Effect.food, 4);
        furnitureItem.usedEffects.Add(Effect.cozy, 2);
        furnitureItem.usedRadiantEffects.Add(Effect.cozy, 2);

        AddNewFurnitureItem(FurnitureName.fridge);
        furnitureItem.type = FurnitureType._core;
        furnitureItem.roomTypes.Add(RoomType.kitchen);
        furnitureItem.flatWideEffect = false;
        furnitureItem.size = 1;
        furnitureItem.price = 200;
        furnitureItem.useLength = 10;
        furnitureItem.rotation = Direction.right;
        furnitureItem.useAnimation = UseAnimation.idle;
        furnitureItem.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down });
        furnitureItem.usedEffects.Add(Effect.food, 4);
        furnitureItem.usedEffects.Add(Effect.cozy, 2);

        AddNewFurnitureItem(FurnitureName.counterTop);
        furnitureItem.type = FurnitureType._core;
        furnitureItem.roomTypes.Add(RoomType.kitchen);
        furnitureItem.flatWideEffect = false;
        furnitureItem.size = 1;
        furnitureItem.price = 100;
        furnitureItem.useLength = 10;
        furnitureItem.rotation = Direction.down;
        furnitureItem.useAnimation = UseAnimation.idle;
        furnitureItem.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down });
        furnitureItem.usedEffects.Add(Effect.food, 4);
        furnitureItem.usedEffects.Add(Effect.cozy, 2);



        AddNewFurnitureItem(FurnitureName.rockingChair);
        furnitureItem.debugItem = true;
        furnitureItem.type = FurnitureType._object;
        furnitureItem.group = FurnitureGroup.chair;
        furnitureItem.roomTypes.Add(RoomType.livingroom);
        furnitureItem.flatWideEffect = false;
        furnitureItem.size = 1;
        furnitureItem.price = 100;
        furnitureItem.useLength = 10;
        furnitureItem.ontopUsePosition = true;
        furnitureItem.rotation = Direction.left;
        furnitureItem.useAnimation = UseAnimation.sit;
        furnitureItem.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down });
        furnitureItem.usedEffects.Add(Effect.comfort, 4);
        furnitureItem.usedEffects.Add(Effect.cozy, 2);

        AddNewFurnitureItem(FurnitureName.rug);
        furnitureItem.type = FurnitureType._decor;
        furnitureItem.roomTypes.Add(RoomType.livingroom);
        furnitureItem.roomTypes.Add(RoomType.bedroom);
        furnitureItem.roomTypes.Add(RoomType.kitchen);
        furnitureItem.roomTypes.Add(RoomType.bathroom);
        furnitureItem.floorObject = true;
        furnitureItem.flatWideEffect = false;
        furnitureItem.size = 1;
        furnitureItem.price = 100;
        furnitureItem.useLength = 10;
        furnitureItem.rotation = Direction.down;
        furnitureItem.usedEffects.Add(Effect.comfort, 4);
        furnitureItem.usedEffects.Add(Effect.cozy, 2);

        AddNewFurnitureItem(FurnitureName.smallSideTable);
        furnitureItem.type = FurnitureType._decor;
        furnitureItem.roomTypes.Add(RoomType.livingroom);
        furnitureItem.roomTypes.Add(RoomType.bedroom);
        furnitureItem.roomTypes.Add(RoomType.bathroom);
        furnitureItem.size = 1;
        furnitureItem.price = 50;
        furnitureItem.useLength = 10;
        furnitureItem.rotation = Direction.down;
        furnitureItem.usedEffects.Add(Effect.comfort, 4);
        furnitureItem.usedEffects.Add(Effect.cozy, 2);

        AddNewFurnitureItem(FurnitureName.tv);
        furnitureItem.type = FurnitureType._object;
        furnitureItem.useFromGroup = FurnitureGroup.chair;
        furnitureItem.roomTypes.Add(RoomType.livingroom);
        furnitureItem.roomTypes.Add(RoomType.bedroom);
        furnitureItem.flatWideEffect = false;
        furnitureItem.size = 1;
        furnitureItem.price = 100;
        furnitureItem.useLength = 10;
        furnitureItem.rotation = Direction.up;
        furnitureItem.useAnimation = UseAnimation.sit;
        furnitureItem.usedEffects.Add(Effect.entertainment, 5);
        furnitureItem.usedRadiantEffects.Add(Effect.entertainment, 2);

        AddNewFurnitureItem(FurnitureName.yarnBasket);
        furnitureItem.type = FurnitureType._object;
        furnitureItem.useFromGroup = FurnitureGroup.chair;
        furnitureItem.roomTypes.Add(RoomType.livingroom);
        furnitureItem.roomTypes.Add(RoomType.bedroom);
        furnitureItem.flatWideEffect = false;
        furnitureItem.size = 1;
        furnitureItem.price = 100;
        furnitureItem.useLength = 10;
        furnitureItem.rotation = Direction.down;
        furnitureItem.useAnimation = UseAnimation.sitAndKnitt;
        furnitureItem.usedEffects.Add(Effect.comfort, 4);
        furnitureItem.usedEffects.Add(Effect.cozy, 2);

        AddNewFurnitureItem(FurnitureName.stonePainting);
        furnitureItem.debugItem = true;
        furnitureItem.type = FurnitureType._object;
        furnitureItem.roomTypes.Add(RoomType.livingroom);
        furnitureItem.roomTypes.Add(RoomType.bedroom);
        furnitureItem.flatWideEffect = false;
        furnitureItem.size = 1;
        furnitureItem.price = 100;
        furnitureItem.useLength = 10;
        furnitureItem.rotation = Direction.down;
        furnitureItem.useAnimation = UseAnimation.idle;
        furnitureItem.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down });
        furnitureItem.usedEffects.Add(Effect.comfort, 4);
        furnitureItem.usedEffects.Add(Effect.cozy, 2);

        AddNewFurnitureItem(FurnitureName.roarRock);
        furnitureItem.debugItem = true;
        furnitureItem.type = FurnitureType._object;
        furnitureItem.roomTypes.Add(RoomType.livingroom);
        furnitureItem.roomTypes.Add(RoomType.bedroom);
        furnitureItem.flatWideEffect = false;
        furnitureItem.size = 1;
        furnitureItem.price = 100;
        furnitureItem.useLength = 10;
        furnitureItem.rotation = Direction.down;
        furnitureItem.ontopUsePosition = true;
        furnitureItem.useAnimation = UseAnimation.roar;
        furnitureItem.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down, AccessPosition.up, AccessPosition.left, AccessPosition.right });
        furnitureItem.usedEffects.Add(Effect.comfort, 4);
        furnitureItem.usedEffects.Add(Effect.cozy, 2);

        if (Settings.furnitureUnlocked)
            mainController.startingAmountOfUnlockedFurniture = Main.objectsList.Count();//2;
        else
            mainController.startingAmountOfUnlockedFurniture = 2;

        for (int i = 0; i < mainController.startingAmountOfUnlockedFurniture; i++)
        {
            mainController.ChooseNewFurnitureToUnlock();
        }

    }

    void SetupCharacters()
    {
        #region setup
        CharacterType name;
        string emoji;
        bool debugItem;

        Dictionary<Effect, CharacterEffectors.EffectProperties> myEffects = new Dictionary<Effect, CharacterEffectors.EffectProperties>();

        void NewEffect(ref Dictionary<Effect, CharacterEffectors.EffectProperties> myEffects, Effect type, float strength, float needBleedRate, float desireGrowRate, DesireAction action = DesireAction.none, float actionLength = 10, float actionSatisfaction = 80, bool syncDesireWithNeed = false)
        {

            myEffects.Add(type, new CharacterEffectors.EffectProperties(strength, needBleedRate, desireGrowRate, action, actionLength, actionSatisfaction, syncDesireWithNeed));
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
        var charInfo = new Character(name, emoji, debugItem, myEffects);
        Main.charactersList.Add(charInfo);
        mainController.ClearCharacterListData(ref myEffects);
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
        Main.charactersList.Add(new Character(name, emoji, debugItem, myEffects));
        mainController.ClearCharacterListData(ref myEffects);
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
        Main.charactersList.Add(new Character(name, emoji, debugItem, myEffects));
        mainController.ClearCharacterListData(ref myEffects);
        #endregion



        var startingAmountOfUnlocked = 1;
        if (Settings.charactersUnlocked)
            startingAmountOfUnlocked = Main.charactersList.Count();//2;



        for (int i = 0; i < startingAmountOfUnlocked; i++)
        {
            mainController.ChooseNewCharacterToUnlock();
        }

    }

    //void SetupFlats()
    //{
    //    AddNewFlat(0, ColorGrey);
    //    var firstflat = AddNewFlat(1, ColorBlue,
    //        new Room(1, RoomType.none),
    //        new Room(2, RoomType.none),
    //        new Room(3, RoomType.none),
    //        new Room(4, RoomType.none),
    //        new Room(4, RoomType.none));
    //}

    //void SetupUI()
    //{
    //    placeItemImage = GetNode<Node2D>("PlaceItemImage").GetNode<Sprite2D>("Sprite2D");
    //    MyUnlocksLabel = GetNode<Godot.CanvasLayer>("CanvasLayer").GetNode<Label>("UnlocksLabel");
    //}

    //void SetupFirstFlat()
    //{
    //    if (SpawnInitialObjects == false) return;
    //    var theObject = GetObjectFromType(FurnitureName.couch);

    //    PlaceObjects(ref theObject, true, new Vector2(576, 237));
    //    theObject = GetObjectFromType(FurnitureName.electricGuitar);

    //    PlaceObjects(ref theObject, true, new Vector2(376, 437));
    //    theObject = GetObjectFromType(FurnitureName.recordPlayer);

    //    PlaceObjects(ref theObject, true, new Vector2(656, 437));
    //    var theCharacter = GetCharacterFromType(CharacterType.granny);
    //    PlaceCharacter(ref theCharacter, true, new Vector2(276, 237));
    //}
}
