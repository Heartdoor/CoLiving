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
    public Node2D currentTarget;
    public bool completedJourney;
    public bool hasTarget;
    bool reachedPoint = false;
    Vector2 wanderDestination;
    Node2D lastTarget = null;
    Vector2 direction;
    public bool foundRandomPlace = false;
    Vector2 myVelocity;
    public float interactionDistance = 40;
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
    public Sprite2D grumpyIcon;
    public Main.Character characterData;
    public UseItem useItemArm;
    Label happinessLabel;
    Label bleedLabel;
    #endregion

    #region EFFECTS
    public float happiness=0; 
    public Dictionary<Main.Object, int> basePrefOfObjects = new Dictionary<Main.Object, int>();       // agent's item preferences 
    public Dictionary<Main.Object, float> heatOfObjects = new Dictionary<Main.Object, float>();           // current "heat" of items 
    Queue<Main.Object> objectQueue = new Queue<Main.Object>();
    int maxQueueLength=20;
    public int lastMoneyEffect =0;
    public Dictionary<Effect, float> bleedList = new Dictionary<Effect, float>();
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
        //NavAgent.TargetReached += ReachTarget;
        NavAgent.VelocityComputed += VelocityComputedSignal;
    }





   

    void SetupObject()
    {
        mySprite = GetNode<Sprite2D>("Sprite2D");
        happinessLabel= GetNode<Label>("HappinessLabel");

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
        foreach (KeyValuePair<Effect, float> effect in characterData.bleedEffects)
            bleedList.Add(effect.Key, 0);
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
        alarm.Run();
        if (wander)
            Wander(); 
        else
        {
            if (currentTarget == null)
            {
                FindTarget();
            }
            else
            {
                if (usingTarget == null)
                    MoveToTarget(currentTarget);
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
            bleedList[effect.Key] -= characterData.bleedEffects[effect.Key]/1000;
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
        
        happinessLabel.Text =Main.TestGameMode == Main.testGameMode.flowingMoney ? $"{lastMoneyEffect}" : $"{happiness}";
        if(alarm.Ended(TimerType.grumpy))
            grumpyIcon.Visible = false;
    }
    public void Wander()
    {
        if (alarm.Ended(TimerType.wander))
        {
            wander = false;
        }
    }
    public void UpdatePath(Vector2 targetPosition)
    {
        _path = _pathfinding.GetPath(GlobalPosition, targetPosition);
        _pathIndex = 0;
    }

    void MoveToAStar()
    {
        if (_path != null && _pathIndex < _path.Length)
        {
            Vector2 direction = (_path[_pathIndex] - GlobalPosition).Normalized();
            Velocity = direction * Speed;

            if (GlobalPosition.DistanceTo(_path[_pathIndex]) < 5.0f)
            {
                _pathIndex++;
            }
        }
        else
        {
            Velocity = Vector2.Zero;
        }

        MoveAndSlide();
    }


    FurnitureType ChooseObjectFromList()
    {
        var noObjectsFound = false;

        foreach (var item in Main.objectsInFlat[myFlatNumber])
        {
            var objectItem = ((Furniture)item).objectData;
            if (basePrefOfObjects.ContainsKey(objectItem) == false)
            {
                var objE = objectItem.usedEffects;
                var charE = characterData.usedEffects;
                Log($"CALCULATING {objectItem.type}", LogType.step);
                // Log($"Base Pref Of: {objectItem.objectData.name} is [objE]", LogType.step);

                //basePrefOfObjects.Add(objectItem, objE.Keys.Intersect(charE.Keys).Select(key => objE[key] * charE[key]).Sum());
                // Assuming objE and charE are dictionaries
                var intersectedKeys = objE.Keys.Intersect(charE.Keys).ToList();

                Log("Intersected Keys: " + string.Join(", ", intersectedKeys), LogType.step);


                var products = intersectedKeys.Select(key =>
                {
                    var product = objE[key] * charE[key];
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
        FurnitureType mostWantedObjectName = FurnitureType.none;
        if (noObjectsFound == false)
        {


            Main.Object mostValuedObject;
            objectQueue.TryPeek(out mostValuedObject);


            //var mostValuedObject = myListOfObjects.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            Log($"CHOSE {mostValuedObject.type}", LogType.game);
            mostWantedObjectName = mostValuedObject.type;
        }

        return mostWantedObjectName;
    }

    void FindTarget()
    {

        var objectType = ChooseObjectFromList();
        if (objectType == FurnitureType.none)
            currentTarget = null;
        else
            currentTarget = FindNearest(this, this.GetTree(), null, "Object", objectType, myFlatNumber);

        if (currentTarget == null)
        {
            alarm.Start(TimerType.wander, 5, false, 0);
            wander = true;
        }

        // UpdatePath(currentTarget.GlobalPosition);

    }

    void MoveToTarget(Node2D target)
    {

        ChangeApproachDistance(NavAgent, interactionDistance);
        // Log($"{name}  GOTO TARGET", LogType.game);
        var destination = target.GlobalPosition;
        // GotoPointAndAvoid(this, NavAgent, speed, friction, accel, delta, destination, completedJourney);
        GotoPoint(this, NavAgent, speed, accel, delta, destination);
    }

    public void ReachTarget()
    {
        usingTarget = currentTarget;
        var furnitureObject = (Furniture)currentTarget;
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
