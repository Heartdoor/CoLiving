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

    public static Object HeldObject=null;
    public static Character HeldCharacter= null;
    public static Dictionary<int,List<Node2D>> objectsInFlat = new Dictionary<int, List<Node2D>>(); 
    public static int flatNumberMouseIsIn = 1;
    Sprite2D placeItemImage;
    #endregion

    #region CLASSES SETUP
    public class Object
        {
            public FurnitureType type;
            public string image_path;
            public Texture2D texture;
            public Dictionary<Effect, int> usedEffects;
            public Vector2 usePosition;
            public float useLength;
            public Dictionary<Effect, int> flatEffects;
            public Object(FurnitureType type,  Dictionary<Effect, int> usedEffects, Vector2 usePosition, float useLength,Dictionary<Effect, int> flatEffects)
            {
                this.type = type;
                image_path = $"res://OBJECTS/SPRITES/{type}.png";
                texture = (Texture2D)GetResource(image_path);


                
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
            public Character(CharacterType type,  Dictionary<Effect, int> usedEffects, Dictionary<Effect, int> flatEffects)
            {
                this.type = type;
                image_path = $"res://CHARACTERS/SPRITES/{type}.png";
                texture = (Texture2D)GetResource(image_path);
                this.usedEffects = new Dictionary<Effect, int>(usedEffects);
                this.flatEffects = new Dictionary<Effect, int>(flatEffects); 
            }
        }
        public static List<Character> charactersList = new List<Character>();
        public class Flat
        {
            public int number;
            public string image_path;
            public Color color;
            public double happiness;
            public Dictionary<Effect, int> flatWideEffects = new Dictionary<Effect, int>();

            public Flat(int number, Color color)
            {
                this.number = number;
                image_path = $"res://FLATS/SPRITES/{number}.png";
                this.color = color;
            }
        }
        public static List<Flat> flatsList = new List<Flat>();

    #endregion




    #region SETUPS
    void SetupObjects()
    {
        #region setup
        FurnitureType type;
        Dictionary<Effect, int> seenEffects = new Dictionary<Effect, int>();
        Dictionary<Effect, int> usedEffects = new Dictionary<Effect, int>();
        Vector2 usePosition = new Vector2(0, 0);
        float useLength = 1;
        Dictionary<Effect, int> flatEffects = new Dictionary<Effect, int>();
        #endregion



        type = FurnitureType.couch;
        useLength = 10;
        usedEffects.Add(Effect.comfort, 1);
        #region apply
        objectsList.Add(new Object(type, usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref flatEffects);
        #endregion

        type = FurnitureType.electricGuitar;
        useLength = 10;
        usedEffects.Add(Effect.music, 3);
        usedEffects.Add(Effect.noise, 3);
        usedEffects.Add(Effect.grunge, 2);
        #region apply
        objectsList.Add(new Object(type, usedEffects, usePosition, useLength, flatEffects));
        ClearObjectListData(ref usedEffects, ref usePosition, ref flatEffects);
        #endregion

        type = FurnitureType.recordPlayer;
        useLength = 10;
        usedEffects.Add(Effect.music, 3);
        usedEffects.Add(Effect.vintage, 3);
        #region apply
        objectsList.Add(new Object(type, usedEffects, usePosition, useLength, flatEffects));
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
        Dictionary<Effect, int> seenEffects = new Dictionary<Effect, int>();
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
        usedEffects.Add(Effect.food, 2);
        usedEffects.Add(Effect.noise, -3);
        usedEffects.Add(Effect.entertainment, 1);
        usedEffects.Add(Effect.hygiene, 3);
        usedEffects.Add(Effect.academic, 1);
        #region apply
        var charInfo = new Character(type, usedEffects, flatEffects);
        charactersList.Add(charInfo);
        ClearCharacterListData(ref usedEffects, ref flatEffects);
        #endregion

        type = CharacterType.punkRocker;
        usedEffects.Add(Effect.comfort, 1);
        usedEffects.Add(Effect.grunge, 3);
        usedEffects.Add(Effect.cozy, -5);
        usedEffects.Add(Effect.music, 3);
        usedEffects.Add(Effect.vintage, 3);
        usedEffects.Add(Effect.food, 1);
        usedEffects.Add(Effect.noise, 3);
        usedEffects.Add(Effect.entertainment, 3);
        usedEffects.Add(Effect.hygiene, 0);
        usedEffects.Add(Effect.academic, -1);
        #region apply
        charactersList.Add(new Character(type, usedEffects, flatEffects));
        ClearCharacterListData(ref usedEffects, ref flatEffects);
        #endregion

    }

    void SetupFlats()
    {
        AddNewFlat(0, ColorGrey); 
        AddNewFlat(1, ColorBlue); 
    }

    void SetupUI()
    {
        placeItemImage = GetNode<Sprite2D>("PlaceItemImage");
    }

    void SetupFirstFlat()
    {
        var theObject = GetObjectFromType(FurnitureType.couch);

        PlaceObjects(ref theObject, true, new Vector2(576, 237));

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
        RunFlatWideEffectsOnCharacters();

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
    void PlaceObjects(ref Object heldObject, bool placeManually, Vector2 position)
    {

        if (heldObject == null) return;
   



            placeItemImage.GlobalPosition = GetGlobalMousePosition();
            placeItemImage.Texture = heldObject.texture;
            if (ButtonPressed("RightClick") || placeManually)
            {
                var newObject=Add2DNode("res://OBJECTS/SCENES/object.tscn",this);
            if (placeManually)
                newObject.GlobalPosition = position;
            else
                newObject.GlobalPosition = GetGlobalMousePosition();
                var newObjectClass = (Furniture)newObject;
                newObjectClass.objectData = heldObject;
                newObjectClass.objectData.type = heldObject.type;
                newObjectClass.mySprite.Texture = placeItemImage.Texture;
                objectsInFlat[flatNumberMouseIsIn].Add(newObject);



                if (placeManually == false)
                if (HeldCharacter != null || heldObject != null)
                {
                    heldObject = null;
                    HeldCharacter = null;
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

            if (ButtonPressed("RightClick") || placeManually)
            {
                var newObject = Add2DNode("res://CHARACTERS/SCENES/character.tscn", this);
            if (placeManually)
                newObject.GlobalPosition = position;
            else
                newObject.GlobalPosition = GetGlobalMousePosition();
                var newObjectClass = (Characters)newObject;
                newObjectClass.characterData = heldCharacter;
                newObjectClass.mySprite.Texture = placeItemImage.Texture;
                newObjectClass.myFlatNumber = flatNumberMouseIsIn;

                if(placeManually==false)
                if (heldCharacter != null || heldObject != null)
                {
                    heldObject = null;
                    heldCharacter = null;
                    Destroy(Main.SelectionMenuOpen);
                    Main.SelectionMenuOpen = null;

                }
                }
        
    }   


    void RunFlatWideEffectsOnCharacters()
    {
        //foreach(){

       // }
    }

    void ClearObjectListData( ref Dictionary<Effect, int> usedEffects, ref Vector2 usePosition, ref Dictionary<Effect, int> flatEffects)
    {
     
        usedEffects.Clear();
        flatEffects.Clear();
        usePosition.X = 0;
        usePosition.Y = 0; 
    }

    void ClearCharacterListData( ref Dictionary<Effect, int> usedEffects, ref Dictionary<Effect, int> flatEffects)
    {
       
        usedEffects.Clear();
        flatEffects.Clear();
    }

    void AddNewFlat(int flatNumber, Color color)
    {
        flatsList.Add(new Flat(flatNumber, color));
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
