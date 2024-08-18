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
    public float interactionDistance = 10;
    public bool performedAction = false;
    int count = 0;
    float restAtPoint = 0;
    Vector2 destination;
    public float delta;

    public float accel = 1800f;
    public float speed = 40;
    public float friction = 1300f;

    public Node2D usingTarget=null;
    #endregion
    #region A STAR
    [Export] private NodePath _pathfindingNodePath;
    private Pathfinding _pathfinding;
    private Vector2[] _path;
    private int _pathIndex;
    public float Speed = 200.0f; 
    #endregion
    #region BASIC OBJECT
    public Sprite2D mySprite;
    public Sprite2D myShadow;
    public Sprite2D grumpyIcon;
    public Main.Character characterData;
    public UseItem useItemArm;
    Label mainLabel;
    Label bleedLabel;
    #endregion

    #region EFFECTS
    public float happiness=0; 
    public Dictionary<Main.Object, float> basePrefOfObjects = new Dictionary<Main.Object, float>();       // agent's item preferences 
    public Dictionary<Main.Object, float> heatOfObjects = new Dictionary<Main.Object, float>();           // current "heat" of items 
    Queue<Main.Object> objectQueue = new Queue<Main.Object>();
    int maxQueueLength=20;
    public int lastMoneyEffect =0;
    public Dictionary<Effect, float> bleedList = new Dictionary<Effect, float>();
    /// <summary>
    /// 
    /// </summary>
    public string bleedText="";
    #endregion

    #region ACTIONS
    public Furniture busyUsing =null;
    public bool wander = false;

    #endregion

    #region DATA
    public int myFlatNumber = 1;
    #endregion
    void SetupNavigation()
    {
        navRegion = null;// GetParent().GetParent().GetParent().GetParent().GetNode<NavigationRegion2D>("MainNavRegion"); 
        SetupNavAgent(this, ref NavAgent, interactionDistance);
        NavAgent.TargetReached += ReachTarget;
        NavAgent.VelocityComputed += VelocityComputedSignal;
    }





   

    void SetupObject()
    {
        mySprite = GetNode<Sprite2D>("Sprite2D");
        myShadow = GetNode<Sprite2D>("Shadow");
        mainLabel = GetNode<Label>("HappinessLabel");

        bleedLabel= GetNode<Label>("BleedLabel");
        grumpyIcon = GetNode<Sprite2D>("Grumpy");
        grumpyIcon.Visible = false;
        useItemArm = new UseItem(this);
    }

    void SetupAStar()
    {
        
        _pathfinding = GetParent().GetNode<Pathfinding>("Pathfinding");

        Log($"PATH {_pathfinding}", LogType.error);

        // Set your target position here


    }

    public void SetupBleedList()
    {
        foreach (KeyValuePair<Effect, Main.EffectProperties> effect in characterData.effectsList)
        {
            if(effect.Value.needBleedRate>0)
                bleedList.Add(effect.Key, 0);
        }
            
    }
    void Start()
    {
        // SetupAStar();
       
        SetupNavigation();
        SetupObject();
  
    }
    //🏃‍
    void Run()
    {
        ZIndex = (int)GlobalPosition.Y;
        FlipSprite(mySprite,Velocity);
        alarm.Run();
        if (wander)
            Wander(); 
        else
        {
            if (accessTarget == null)
            {
                FindTarget();
            }
            else
            {
                if (usingTarget == null)
                    MoveToTarget(accessTarget);
                else
                    useItemArm.UseTarget();
            }
        }

        ManageIconsCharacterUI();
        useItemArm.CalculateStaticHappiness();

        Bleed();
    }

    public void Bleed()
    {
        bleedText = "";
        foreach (KeyValuePair<Effect, float> effect in bleedList)
        {
            bleedList[effect.Key] -= characterData.effectsList[effect.Key].needBleedRate/1000;
            if (bleedList[effect.Key] < 0)
            {
                if (Main.TestGameMode == Main.testGameMode.flowingMoney)
                Main.Money += bleedList[effect.Key];

                bleedText += ConvertToEmoji(effect.Key);
            }

        }
        if(bleedText=="") 
            bleedLabel.Visible=false;
        else
            bleedLabel.Visible = true;
        bleedLabel.Text = bleedText;
    }
    public void ManageIconsCharacterUI()
    {

        // happinessLabel.Text =Main.TestGameMode == Main.testGameMode.flowingMoney ? $"{lastMoneyEffect}" : $"{happiness}";\
        if (accessTarget != null && Settings.characterTargetNameLabel)
            mainLabel.Text = $"{((Furniture)useTarget).objectData.name}";
        if (alarm.Ended(TimerType.grumpy))
            grumpyIcon.Visible = false;
    }
    public void Wander()
    {
        if (alarm.Ended(TimerType.wander))
        {
            wander = false;
        }
    }

   

    Main.Object ChooseObjectFromList()
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
            if (heatOfObjects[item] > 0 && !objectQueue.Contains(item))
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
        if (noObjectsFound == false)
        {


            
            objectQueue.TryPeek(out mostValuedObject);


            //var mostValuedObject = myListOfObjects.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            Log($"CHOSE {mostValuedObject.name}", LogType.game);
       
        }

        return mostValuedObject;
    }

    void FindTarget()
    {

        var highestValuedObject = ChooseObjectFromList();

        if (highestValuedObject== null)
        {
            alarm.Start(TimerType.wander, 5, false, 0);
            wander = true;
            accessTarget = null;
        }
        else
        {
           
            if (highestValuedObject.useFromGroup== FurnitureGroup.chair)
            {
                useTarget = FindNearest(this, this.GetTree(), null, "Object", highestValuedObject.name, myFlatNumber);
                accessTarget = FindNearestOfGroupType(useTarget, this.GetTree(), null, "Object", highestValuedObject.useFromGroup, myFlatNumber);
                accessNode = FindNearestAccessNode(this, accessTarget);
            }
            else
            {
                useTarget = FindNearest(this, this.GetTree(), null, "Object", highestValuedObject.name, myFlatNumber);
                accessTarget = useTarget;
                accessNode = FindNearestAccessNode(this, accessTarget);
            }
                
        }
            


        // UpdatePath(currentTarget.GlobalPosition);

    }

    void MoveToTarget(Node2D target)
    {
        Log("move to target", LogType.weird);
        ChangeApproachDistance(NavAgent, interactionDistance);
        // Log($"{name}  GOTO TARGET", LogType.game);
        var destination = accessNode.GlobalPosition;
        // GotoPointAndAvoid(this, NavAgent, speed, friction, accel, delta, destination, completedJourney);
        GotoPoint(this, NavAgent, speed, accel, delta, destination);
    }

    public void ReachTarget()
    {
        if(useTarget!=null )
        usingTarget = useTarget;
        else
        usingTarget = accessTarget;

        Log("reach target", LogType.weird);
        var furnitureObject = (Furniture)usingTarget;
        alarm.Start(TimerType.actionLength, furnitureObject.objectData.useLength, false, 0);
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
        return GetTexture2D($"res://CHARACTERS/SPRITES/{characterData.type}{addedText}.png");
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
