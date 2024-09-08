using Godot;
using System;
using System.Collections.Generic;
using static Asriela.BasicFunctions;
using System.Linq;

public partial class Characters : CharacterBody2D
{

    #region BASIC FUNCTIONS CONNECTIONS
    public Alarms alarm = new Alarms();
    #endregion
    #region MOVEMENT VARIABLES
    public int canReachTarget = 0;
    NavigationRegion2D navRegion;
    public NavigationAgent2D NavAgent = null;
    public Node2D accessTarget;
    public Node2D accessNode;
    public Node2D useTarget;
    public bool completedJourney;
    public bool hasTarget;
    bool reachedPoint = false;
    Vector2 wanderDestination;
    Node2D lastTarget = null;
    Vector2 direction;
    public bool foundRandomPlace = false;
    Vector2 myVelocity;

    public bool performedAction = false;
    int count = 0;
    float restAtPoint = 0;
    Vector2 destination;
    public float delta;

    public float accel = 1800f;
    public float speed = 40;
    public float friction = 1300f;

    public Node2D interactingWithTarget = null;
    #endregion
    #region A STAR
    [Export] private NodePath _pathfindingNodePath;
    private Pathfinding _pathfinding;
    private Vector2[] _path;
    private int _pathIndex;
    public float Speed = 200.0f;
    #endregion
    #region BASIC OBJECT
    public Sprite2D mySelectionBox;
    public Sprite2D myShadow;
    public Sprite2D stateSquare;
    public Sprite2D actionSquare;
    public Sprite2D grumpyIcon;
    public Main.Character characterData;
    public AnimatedSprite2D myAnimator;
    public Interact useItemArm;
    Label mainLabel;
    Label bleedLabel;
    #endregion

    #region EFFECTS
    public float happiness = 0;
    public Dictionary<Main.Object, float> basePrefOfObjects = new Dictionary<Main.Object, float>();       // agent's item preferences 
    public Dictionary<Main.Object, float> heatOfObjects = new Dictionary<Main.Object, float>();           // current "heat" of items 
    Queue<Main.Object> objectQueue = new Queue<Main.Object>();

    int maxQueueLength = 20;
    public int lastMoneyEffect = 0;

    /// <summary>
    /// 
    /// </summary>
    public string bleedText = "";
    #endregion

    #region ACTIONS
    public Node2D interactingWith = null;
    public Characters interactingWithCharacter = null;
    public bool wander = false;
    public bool stopCurrentAction = false;
    public bool canPerformAction = true;
    public bool isUpset = false;
    public bool isInInteraction = false;
    public DesireAction chosenInteractionWithCharacter = DesireAction.none;
    public Effect chosenDesireToSocializeOn = Effect.none;
    public bool interpersonalInteraction = false;
    public int mySeatingIndex =0;
    #endregion

    #region DATA
    public int myFlatNumber = 1;
    #endregion
    void SetupNavigation()
    {
        navRegion = null;// GetParent().GetParent().GetParent().GetParent().GetNode<NavigationRegion2D>("MainNavRegion"); 
        SetupNavAgent(this, ref NavAgent, Settings.tweak_interactionDistance); 
        NavAgent.TargetReached += ReachTarget;
        NavAgent.VelocityComputed += VelocityComputedSignal;
    }







    void SetupObject()
    {

        myAnimator = GetNode<AnimatedSprite2D>("Animation2D");
        myShadow = GetNode<Sprite2D>("Shadow");
        mySelectionBox = GetNode<Sprite2D>("Select");
        mySelectionBox.Visible=false;
        stateSquare = GetNode<Sprite2D>("StateUI");
        actionSquare = GetNode<Sprite2D>("ActionUI");
        mainLabel = GetNode<Label>("HappinessLabel");

        bleedLabel = GetNode<Label>("BleedLabel");
        grumpyIcon = GetNode<Sprite2D>("Grumpy");
        grumpyIcon.Visible = false;
        useItemArm = new Interact(this);
    }



    public void SetupBleedList()
    {
        foreach (KeyValuePair<Effect, Main.EffectProperties> effect in characterData.effectsList)
        {
            if (effect.Value.needBleedRate > 0)
                characterData.needs.Add(effect.Key, 0);
        }

    }
    void Start()
    {
        // SetupAStar();

        SetupNavigation();
        SetupObject();

    }
    public void ChangeStateSquare(Sprite2D square, Color color)
    {
        ChangeColor(square, color);

        if(color == Settings.stateColorInactive)
            square.Texture = GetTexture2D("res://UI/SPRITES/StateSquares/stateSquare.png");
        if (color == Settings.stateColorBeingSocializedWithAndNotUsingFurniture)
            square.Texture = GetTexture2D("res://UI/SPRITES/StateSquares/stateSquare_socialize.png");
        if (color == Settings.stateColorSocializing)
            square.Texture = GetTexture2D("res://UI/SPRITES/StateSquares/stateSquare_socialize.png");
        if (color == Settings.stateColorUpset)
            square.Texture = GetTexture2D("res://UI/SPRITES/StateSquares/stateSquare_upset.png");
        if (color == Settings.stateColorUsingFurniture)
            square.Texture = GetTexture2D("res://UI/SPRITES/StateSquares/stateSquare_use.png");
        if (color == Settings.stateColorMovingToFurniture)
            square.Texture = GetTexture2D("res://UI/SPRITES/StateSquares/stateSquare_goto.png");
        if (color == Settings.stateColorMovingToSocialTarget )
            square.Texture = GetTexture2D("res://UI/SPRITES/StateSquares/stateSquare_goto.png");
        if (color == Settings.stateColorFind)
            square.Texture = GetTexture2D("res://UI/SPRITES/StateSquares/stateSquare_find.png");
        if (color == Settings.stateColorArrive)
            square.Texture = GetTexture2D("res://UI/SPRITES/StateSquares/stateSquare_arrive.png");
    }
    //🏃‍
    void Run()
    {
    
        stateSquare.Visible = Settings.showCharacterStateSquare;
        ChangeStateSquare(stateSquare, Settings.stateColorInactive);
        
        isInInteraction =false;
        ZIndex = (int)GlobalPosition.Y;
        FlipAnimatedSprite(myAnimator, Velocity);
        alarm.Run();
        if (wander)
            Wander();
        else
        {
            if (accessTarget == null && interactingWithCharacter ==null)
            {
                FindTarget();
            }
            else
            {
                if (interactingWithCharacter != null)
                    ChangeStateSquare(stateSquare, Settings.stateColorBeingSocializedWithAndNotUsingFurniture);

                if (interactingWithTarget == null )
                    MoveToTarget(accessTarget);
                else
                    if (canPerformAction)
                    {
                        if(interpersonalInteraction)
                        useItemArm.SocialInteractWithTarget();
                        else
                        useItemArm.UseFunitureTarget();
                    }
                    


            }
        }

        ManageIconsCharacterUI();
        useItemArm.CalculateStaticHappiness();

        RunNeeds();
        RunDesires();
        DetermineBiggestEmotion();
        StopBeingMad();
        MoveAndIdleAnimations();
        FadeActionSquare();
        CheckIfSelected();
    }
    void CheckIfSelected()
    {
        mySelectionBox.Visible = false;
        if (Main.SelectedCharacter == this)
            mySelectionBox.Visible = true;
    }
    void FadeActionSquare()
    {
        if(GetSpriteAlpha(actionSquare)>0)
        ChangeSpriteAlpha(actionSquare,-0.01f);
    }
    void MoveAndIdleAnimations()
    {
        if(isUpset || isInInteraction)
        return;
        

        var idleAnimation = $"idle_{ characterData.mainEmotion}";

        

        PlayAnimation(myAnimator, idleAnimation);
    }
    void RunNeeds()
    {
        bleedText = "";
        foreach (KeyValuePair<Effect, float> effect in characterData.needs)
        {
            characterData.needs[effect.Key] -= characterData.effectsList[effect.Key].needBleedRate/1000;
            if (characterData.needs[effect.Key] < 0)
            {
                if (Main.TestGameMode == Main.testGameMode.flowingMoney)
                Main.Money += characterData.needs[effect.Key];

                bleedText += ConvertToEmoji(effect.Key);
            }

        }
        if(bleedText=="") 
            bleedLabel.Visible=false;
        else
            bleedLabel.Visible = true;
        bleedLabel.Text = bleedText;
    }

    void RunDesires()
    {
        foreach (KeyValuePair<Effect, Main.EffectProperties> effect in characterData.effectsList)
        {
            var desireGrowRate = effect.Value.desireGrowRate;
            if (desireGrowRate != 0)
            {
                if (!characterData.desires.ContainsKey(effect.Key))
                {
                    characterData.desires.Add(effect.Key, desireGrowRate);
                }
                else
                    characterData.desires[effect.Key]+= desireGrowRate;

            }
        }
    }

    void DetermineBiggestEmotion()
    {
        //add that needs drain happiness
        characterData.mainEmotion= Emotion.neutral;
        var happiness= characterData.feelings[Effect.happiness];
        var romance = characterData.feelings[Effect.romance];
        var changeLimmit =30;
       if(happiness  > changeLimmit)
            characterData.mainEmotion = Emotion.happy;
       else if(happiness < -changeLimmit)
            characterData.mainEmotion = Emotion.angry;

       if((happiness > 0 && romance>changeLimmit*2) || (happiness<0 && romance+ happiness> changeLimmit * 2))
        {
            characterData.mainEmotion = Emotion.flirty;
        }
    }
    public void ManageIconsCharacterUI()
    {
        mainLabel.Text = ""; 
        // happinessLabel.Text =Main.TestGameMode == Main.testGameMode.flowingMoney ? $"{lastMoneyEffect}" : $"{happiness}";\
        if (accessTarget != null && Settings.characterLabelHasTargetName) 
            mainLabel.Text += $"{((Furniture)useTarget).objectData.name}\n";

        if ( Settings.characterLabelHasHappinessValue)
            mainLabel.Text += $"{characterData.feelings[Effect.happiness]}\n";

        if (Settings.characterLabelHasDesiresValues)
        {
            foreach (KeyValuePair< Effect, float> desire in characterData.desires)
            {
                mainLabel.Text += $"{desire.Key} : {desire.Value.ToString("F2")}\n";
            }

            
        }
    }

    void StopBeingMad()
    {
        if (alarm.Ended(TimerType.grumpy))
        {
            grumpyIcon.Visible = false;
            PlayAnimation(myAnimator,"idle");
            canPerformAction = true;
            isUpset=false;
        }
        else
        if(isUpset)
        {
            ChangeStateSquare(stateSquare, Settings.stateColorUpset);
        }
    }
    public void Wander()
    {
        ChangeStateSquare(stateSquare, Settings.stateColorInactive);
        if (alarm.Ended(TimerType.wander))
        {
            wander = false;
        }
    }

    public void AddMyselfToEveryonesRelationshipsList()
    {
        var allCharacters = Main.flatsList[Main.FlatNumberMouseIsIn].charactersInFlat;
        foreach (Characters character in allCharacters)
        {
            if (character != this)
            {
                //add characters to me
                characterData.relationshipsList.Add(character, new Main.Relationship());
                //add me to characters
                character.characterData.relationshipsList.Add(this, new Main.Relationship());
            }
        }
    }

    (Main.Object furniture,float value) ChooseObjectFromList()
    {
        var noObjectsFound = false;

        foreach (var item in Main.flatsList[myFlatNumber].objects)
        {
            var furnitureItem = (Furniture)item;
            var objectItem = furnitureItem.objectData;
            //not already in list add new base preference 
            if (basePrefOfObjects.ContainsKey(objectItem) == false && (objectItem.type == FurnitureType._core || objectItem.type == FurnitureType._object))
            {
                var objE = objectItem.usedEffects;
                var charE = characterData.effectsList;
                Log($"CALCULATING {objectItem.name}", LogType.step);
                // Log($"Base Pref Of: {objectItem.objectData.name} is [objE]", LogType.step);

                //basePrefOfObjects.Add(objectItem, objE.Keys.Intersect(charE.Keys).Select(key => objE[key] * charE[key]).Sum());
                // Assuming objE and charE are dictionaries
                var intersectedEffects = charE.Keys.Intersect(objE.Keys).ToList(); 

                Log("Intersected Keys: " + string.Join(", ", intersectedEffects), LogType.step);


                var products = intersectedEffects.Select(key =>
                {
                    var product = objE[key] * charE[key].strength;
                    //Log(, LogType.step);
                    Log($"Key: {key}, objE[{key}] = {objE[key]}, charE[{key}] = {charE[key]}, Product: {product}", LogType.step);
                    return product;
                }).ToList();

                var sum = products.Sum();


                Log("Sum of Products: " + sum, LogType.step);


                basePrefOfObjects.Add(objectItem, sum);

                heatOfObjects.Add(objectItem, basePrefOfObjects[objectItem]);

            }
        }


        // Create a list of items sorted by descending heat 
        var sortedItems = heatOfObjects.OrderByDescending(item => item.Value).Select(item => item.Key).ToList();
        objectQueue.Clear();
    
        // Add critical items to the queue in order 
        foreach (var item in sortedItems)
        {
            if (heatOfObjects[item] >= 0 && !objectQueue.Contains(item))
            {
                objectQueue.Enqueue(item);        // add item to queue if it's not already in the queue 
                                                     // Assume queue length is limited to n 
                if (objectQueue.Count >= maxQueueLength)
                {
                    break;      // break if queue is full
                }
            }
        }
        //ok
        // If no items were above 10, add the highest heat item only 
        if (objectQueue.Count == 0)
        {
            //var maxHeatItem = heatOfObjects.OrderByDescending(item => item.Value).First().Key;
            //objectQueue.Enqueue(maxHeatItem);     // add the item with max heat to the queue
            noObjectsFound = true;
        }



        //  Log($"{basePrefOfObjects[item]}", LogType.game);  
        Main.Object mostValuedObject=null;
        float highestHeat = 0f;
        if (noObjectsFound == false)
        {


            
            objectQueue.TryPeek(out mostValuedObject);
            highestHeat= heatOfObjects[mostValuedObject];

            //var mostValuedObject = myListOfObjects.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            if(mostValuedObject!=null)
            Log($"{characterData.emoji} WANTS TO USE [{mostValuedObject.name}] AT VALUE [{highestHeat}]", LogType.game);
       
        }

        return (mostValuedObject,highestHeat);
    }
     (Characters character,float value) ChooseFromCharacterList()
    {
        chosenInteractionWithCharacter = DesireAction.none;
        Characters characterObject = null;
        var highestValue = 0f;
        var charactersInFlat=Main.flatsList[Main.FlatNumberMouseIsIn].charactersInFlat;
        if (charactersInFlat.Count < 2) return (null, 0f);

        
        foreach (Characters character in charactersInFlat)
        {
            if(character != this) 
            {
            //look at all possible interactions with this character, 
            //by looking at the desires we have 
            chosenDesireToSocializeOn=Effect.none;
            

            foreach (KeyValuePair<Effect,float> desire in characterData.desires)
            {
                if(highestValue< desire.Value)
                {
                    highestValue= desire.Value;
                    chosenDesireToSocializeOn = desire.Key;
                    chosenInteractionWithCharacter = characterData.effectsList[desire.Key].action;
                    characterObject= character;
                }
            }
            }
        }
        highestValue /= Settings.tweak_desireVSobjectValue;
        if(characterObject!=null)
        Log($"{characterData.emoji} WANTS TO [{chosenInteractionWithCharacter}] WITH [{characterObject.characterData.emoji}] AT VALUE [{highestValue}]", LogType.game);
        return (characterObject,highestValue); 
    }


    void FindTarget()
    {

        PulseActionDebugSquare(Settings.stateColorFind);
        var mostValuedObject = ChooseObjectFromList();
        var valuedFurniture= mostValuedObject.furniture;
        var furnitureValue = mostValuedObject.value;

        var mostValuedInterpersonal = ChooseFromCharacterList();
        Main.Character valuedCharacter=null;
        var characterValue =0f;
        if (mostValuedInterpersonal.character!= null)
        {
            valuedCharacter = mostValuedInterpersonal.character.characterData;
            characterValue = mostValuedInterpersonal.value;
        }


     
        if (valuedFurniture == null && valuedCharacter == null)
        {
            alarm.Start(TimerType.wander, 5, false, 0);
            wander = true;
            accessTarget = null;
        }
        else
        if(furnitureValue>= characterValue)
        {
            interpersonalInteraction= false;
            if (valuedFurniture.useFromGroup== FurnitureGroup.chair)
            {
                useTarget = FindNearestFurniture(this, this.GetTree(), null, "Object", valuedFurniture.name, myFlatNumber);
                accessTarget = FindNearestOfGroupType(useTarget, this.GetTree(), null, "Object", valuedFurniture.useFromGroup, myFlatNumber);
                accessNode = FindNearestAccessNode(this, accessTarget);
            }
            else
            {
                useTarget = FindNearestFurniture(this, this.GetTree(), null, "Object", valuedFurniture.name, myFlatNumber);
                accessTarget = useTarget;
                accessNode = FindNearestAccessNode(this, accessTarget);
            }
                
        }
        else
        {
            interpersonalInteraction=true;
            useTarget = FindNearestCharacter(this, this.GetTree(), null, "Character", valuedCharacter.name, myFlatNumber);  
            accessTarget = useTarget;
            accessNode= accessTarget;

        }
            


        // UpdatePath(currentTarget.GlobalPosition);

    }

    void MoveToTarget(Node2D target)
    {
        if(interactingWithCharacter != null)return;
        if (interpersonalInteraction)
        {
            ChangeApproachDistance(NavAgent, Settings.tweak_socializingDistance);
            ChangeStateSquare(stateSquare, Settings.stateColorMovingToSocialTarget);
        }
            
        else
        {
            ChangeStateSquare(stateSquare, Settings.stateColorMovingToFurniture);
            ChangeApproachDistance(NavAgent, Settings.tweak_interactionDistance);
        }
            


        
        // Log($"{name}  GOTO TARGET", LogType.game);
        var destination = accessNode.GlobalPosition;
        // GotoPointAndAvoid(this, NavAgent, speed, friction, accel, delta, destination, completedJourney);
        GotoPoint(this, NavAgent, speed, accel, delta, destination);
    }

    public void ReachTarget()
    {
        //MAKE SURE WE ARNT TOO CLOSE FOR A SOCIAL INTERACTION
        if(interpersonalInteraction && DistanceBetweenObjects(this, useTarget) < Settings.tweak_socializingDistance-10) {
            var destination = ChangePositionByAngle(useTarget.GlobalPosition, PointDirection((CharacterBody2D)useTarget,this), Settings.tweak_socializingDistance);
            GotoPoint(this, NavAgent, speed, accel, delta, destination);
            return;
        }

        PulseActionDebugSquare(Settings.stateColorArrive);
        if (useTarget!=null )
        interactingWithTarget = useTarget;
        else
        interactingWithTarget = accessTarget;



        if (interpersonalInteraction) 
        {
            var characterObject = (Characters)interactingWithTarget;
            alarm.Start(TimerType.actionLength, characterObject.characterData.effectsList[chosenDesireToSocializeOn].actionLength, false, 0);
        }
        else
        {
            var furnitureObject = (Furniture)interactingWithTarget;
            alarm.Start(TimerType.actionLength, furnitureObject.objectData.useLength, false, 0);
        }
        Log("Reached target", LogType.step);
    }

    public void VelocityComputedSignal(Vector2 safeVelocity)
    {
        Velocity = safeVelocity;
        MoveAndSlide();

    }

    public Texture2D ChangeSprite(string addedText)
    {
        if (addedText == "stand") addedText = "";
        addedText = Capitalize(addedText);
        return GetTexture2D($"res://CHARACTERS/SPRITES/{characterData.name}{addedText}.png");
    }

    public void PulseActionDebugSquare(Color color)
    {
        ChangeStateSquare(actionSquare, color);
        SetSpriteAlpha(actionSquare, 1);
    }


    public void SelectedCharacter(Node viewport, InputEvent inputEvent, int shapeIdx)
     {
        if (LeftMousePressed(inputEvent)) {
            
            Main.SelectedCharacter = this;

            }

       }
    #region OLD
    public override void _Ready()
    {
        Start();
    }

    public override void _PhysicsProcess(double delta)
    {

        Run();
        this.delta = (float)delta;
    }
    #endregion

}
