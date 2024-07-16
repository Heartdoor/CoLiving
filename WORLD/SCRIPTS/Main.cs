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
    public static List<Character> CharactersAvailableToPlayerList = new List<Character> ();
    Sprite2D placeItemImage;
    public static TileMap MyTileMap;
    #endregion

    #region SETTINGS
    public static bool SpawnInitialObjects = false;
    public static bool MoneyCheat = true;
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
    public class Object
        {
            public FurnitureType type;
            public bool flatWideEffect;
            public string image_path;
            public Texture2D texture;
            public Dictionary<Effect, int> usedEffects;
            public Vector2 usePosition;
            public float useLength;
            public int size;
            public int price;
            public Dictionary<Effect, int> flatEffects;
            public Object(FurnitureType type, bool flatWideEffect, int size,int price, Dictionary<Effect, int> usedEffects, Vector2 usePosition, float useLength,Dictionary<Effect, int> flatEffects)
            {
                this.type = type;
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
        public class Character
        {
            public CharacterType type;
            public string image_path;
            public Texture2D texture;
            public Dictionary<Effect, int> usedEffects;
            public Dictionary<Effect, int> flatEffects;
            public Dictionary<Effect, float> bleedEffects;
            public Character(CharacterType type,  Dictionary<Effect, int> usedEffects, Dictionary<Effect, float> bleedEffects)
            {
                this.type = type;
                image_path = $"res://CHARACTERS/SPRITES/{type}.png";
                texture = (Texture2D)GetResource(image_path);
                this.usedEffects = new Dictionary<Effect, int>(usedEffects);
                this.bleedEffects = new Dictionary<Effect, float>(bleedEffects); 
            }
        }
        public static List<Character> charactersList = new List<Character>();
        public class Flat
        {
            public int number;
            public string image_path;
            public Color color;
            public double happiness;
            public List<Characters> charactersInFlat;
            public Dictionary<Effect, int> flatWideEffects = new Dictionary<Effect, int>();

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



    #region SETUPS

    void SetupObjects()
    {
        #region setup
        FurnitureType type;
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
        flatWideEffect = false;
        size = 2;
        price = 100;
        useLength = 10;
        usedEffects.Add(Effect.comfort, 1);
        #region apply
        objectsList.Add(new Object(type, flatWideEffect,size, price,usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref flatEffects);
        #endregion

        type = FurnitureType.electricGuitar;
        flatWideEffect = true;
        size = 1;
        price = 200;
        useLength = 10;
        usedEffects.Add(Effect.music, 3);
        usedEffects.Add(Effect.noise, 3);
        usedEffects.Add(Effect.grunge, 2);
        #region apply
        objectsList.Add(new Object(type, flatWideEffect,size, price, usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref flatEffects);
        #endregion

        type = FurnitureType.recordPlayer;
        flatWideEffect = true;
        size = 1;
        price = 100;
        useLength = 10;
        usedEffects.Add(Effect.music, 3);
        usedEffects.Add(Effect.vintage, 3);
        #region apply
        objectsList.Add(new Object(type, flatWideEffect, size, price, usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref flatEffects);
        #endregion

        type = FurnitureType.stove;  
        flatWideEffect = false;
        size = 1;
        price = 200;
        useLength = 10;
        usedEffects.Add(Effect.food, 4);
        usedEffects.Add(Effect.cozy,2);
        #region apply
        objectsList.Add(new Object(type, flatWideEffect, size, price, usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref flatEffects);
        #endregion

        /* name = "paintEasel";
         usedEffects.Add(Effect.messy, 3);
         #region apply
         objectsList.Add(new Object(name, seenEffects, usedEffects, usePosition, flatEffects));
         ClearObjectListData(ref seenEffects, ref usedEffects, ref usePosition, ref flatEffects);
         #endregion
        */

    }

    void SetupCharacters()
    {
        #region setup
        CharacterType type;
        Dictionary<Effect, float> bleedEffects = new Dictionary<Effect, float>();
        Dictionary<Effect, int> usedEffects = new Dictionary<Effect, int>();
        Dictionary<Effect, int> flatEffects = new Dictionary<Effect, int>();
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

        type = CharacterType.granny;
        usedEffects.Add(Effect.comfort, 3); 
        usedEffects.Add(Effect.grunge, -3);
        usedEffects.Add(Effect.cozy, 5);
        usedEffects.Add(Effect.music, 2);
        usedEffects.Add(Effect.vintage, 3);
        usedEffects.Add(Effect.food, 2); bleedEffects.Add(Effect.food, 0.01f);
        usedEffects.Add(Effect.noise, -3);
        usedEffects.Add(Effect.entertainment, 1);
        usedEffects.Add(Effect.hygiene, 3);
        usedEffects.Add(Effect.academic, 1);
        #region apply
        var charInfo = new Character(type, usedEffects, bleedEffects);
        charactersList.Add(charInfo);
        ClearCharacterListData(ref usedEffects, ref bleedEffects);
        #endregion

        type = CharacterType.punkRocker;
        usedEffects.Add(Effect.comfort, 1);
        usedEffects.Add(Effect.grunge, 3);
        usedEffects.Add(Effect.cozy, -5);
        usedEffects.Add(Effect.music, 3); bleedEffects.Add(Effect.music, 0.01f);
        usedEffects.Add(Effect.vintage, 3);
        usedEffects.Add(Effect.food, 1);
        usedEffects.Add(Effect.noise, 3);
        usedEffects.Add(Effect.entertainment, 3);
        usedEffects.Add(Effect.hygiene, 0);
        usedEffects.Add(Effect.academic, -1);
        #region apply
        charactersList.Add(new Character(type, usedEffects, bleedEffects));
        ClearCharacterListData(ref usedEffects, ref bleedEffects);
        #endregion

        CharactersAvailableToPlayerList.Add(GetCharacterFromType(CharacterType.granny));
        CharactersAvailableToPlayerList.Add(GetCharacterFromType(CharacterType.punkRocker));

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
        placeItemImage = GetNode<Sprite2D>("PlaceItemImage");
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
    }
    void PlaceObjects(ref Object HeldObject, bool placeManually, Vector2 position)
    {

        if (HeldObject == null) return;
   



            placeItemImage.GlobalPosition = GetGlobalMousePosition();
            placeItemImage.Texture = HeldObject.texture;
            var cost = HeldObject.price;
        if ((KeyPressed("RightClick") && Money >= cost) || placeManually)
            {
                var newObject=Add2DNode("res://OBJECTS/SCENES/object.tscn",this);
                var newObjectClass = (Furniture)newObject;
                newObjectClass.objectData = HeldObject;
                newObjectClass.objectData.type = HeldObject.type;
                newObjectClass.mySprite.Texture = placeItemImage.Texture;
                newObjectClass.flatIAmIn = FlatNumberMouseIsIn;
                objectsInFlat[FlatNumberMouseIsIn].Add(newObject);
                newObjectClass.ChangeDimensions(newObjectClass.objectData.size);
                

            if (placeManually)
            {
                newObject.GlobalPosition = position;


            }

            else
            {
                newObject.GlobalPosition = GetGlobalMousePosition();
              
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
    }

    void ClearObjectListData( ref Dictionary<Effect, int> usedEffects, ref Vector2 usePosition, ref Dictionary<Effect, int> flatEffects)
    {
     
        usedEffects.Clear();
        flatEffects.Clear();
        usePosition.X = 0;
        usePosition.Y = 0; 
    }

    void ClearCharacterListData( ref Dictionary<Effect, int> usedEffects, ref Dictionary<Effect, float> bleedEffects)
    {
       
        usedEffects.Clear();
        bleedEffects.Clear();
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
