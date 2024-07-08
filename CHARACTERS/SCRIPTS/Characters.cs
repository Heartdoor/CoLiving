using Godot;
using System;
using System.Collections.Generic;
using static Asriela.BasicFunctions;
using System.Linq;

public partial class Characters : CharacterBody2D
{

    #region BASIC FUNCTIONS CONNECTIONS
    Alarms alarm = new Alarms();
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
    public float interactionDistance = 80;
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

    #region BASIC OBJECT
    public Sprite2D mySprite;
    public Main.Character characterData;

    #endregion

    #region EFFECTS
    private Dictionary<Main.Object, int> basePrefOfObjects = new Dictionary<Main.Object, int>();       // agent's item preferences 
    private Dictionary<Main.Object, float> heatOfObjects = new Dictionary<Main.Object, float>();           // current "heat" of items 
    Queue<Main.Object> objectQueue = new Queue<Main.Object>();
    int maxQueueLength=20;
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



    string ChooseObjectFromList()
    {
        foreach (var item in Main.objectsInFlat[myFlatNumber])
        {
            var objectItem = ((Furniture)item).objectData;
            if (basePrefOfObjects.ContainsKey(objectItem) == false)
            {
                var objE = objectItem.usedEffects;
                var charE = characterData.usedEffects;
                Log($"CALCULATING {objectItem.name}", LogType.step);
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

                // Add critical items to the queue in order 
                foreach (var item in sortedItems)
                {
                    if (heatOfObjects[item] > 10 && !objectQueue.Contains(item))
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
                    var maxHeatItem = heatOfObjects.OrderByDescending(item => item.Value).First().Key;
                    objectQueue.Enqueue(maxHeatItem);     // add the item with max heat to the queue
                }



        //  Log($"{basePrefOfObjects[item]}", LogType.game);  


        Main.Object mostValuedObject;
        objectQueue.TryPeek(out mostValuedObject);


                //var mostValuedObject = myListOfObjects.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
         
        Log($"CHOSE {mostValuedObject.name}", LogType.game);
        string mostWantedObjectName= mostValuedObject.name;
        return mostWantedObjectName;
    }

    void FindTarget()
    {

        var objectName= ChooseObjectFromList();
        currentTarget = FindNearest(this, this.GetTree(),null,"Object" , objectName);

    }

    void MoveToTarget(Node2D target)
    {
        ChangeApproachDistance(NavAgent, 200);
        // Log($"{name}  GOTO TARGET", LogType.game);
        var destination = target.GlobalPosition;
        GotoPointAndAvoid(this, NavAgent, speed, friction, accel, delta, destination, completedJourney);
    }

    public void ReachTarget()
    {
        usingTarget = currentTarget;
        var furnitureObject = (Furniture)currentTarget;
        alarm.Start(TimerType.actionLength, furnitureObject.objectData.useLength, false, 0);
        Log("Reached target", LogType.step);
    }

    void UseTarget()
    {
        GlobalPosition = currentTarget.GlobalPosition;
        mySprite.Texture = GetTexture2D($"res://CHARACTERS/SPRITES/{characterData.name}Sit.png");
        //Log($"TIME LEFT: {alarm.Total(TimerType.actionLength)} / {alarm.Left(TimerType.actionLength)} | {alarm.Global()}", LogType.step );
        if (alarm.Ended(TimerType.actionLength))
        {
        
            Log("DONE USING ITEM", LogType.step);
            mySprite.Texture = characterData.texture;
            var item = ((Furniture)currentTarget).objectData;
            // Update heat values after task completion 
            foreach (var key in heatOfObjects.Keys.ToList())
            {
                heatOfObjects[key] += basePrefOfObjects[key];
            }
            heatOfObjects[item] = basePrefOfObjects[item];

            currentTarget = null;
            usingTarget = null;
        }
            
        
    }
    void SetupObject()
    {
        mySprite = GetNode<Sprite2D>("Sprite2D");
    }


    void Start()
    {
        SetupNavigation();
        SetupObject();
    }

    void Run()
    {
        alarm.Run();
        if (currentTarget==null)
        {
            FindTarget();
        }
        else
        {
            if (usingTarget == null)
                MoveToTarget(currentTarget);
            else
                UseTarget();
        }

    }

    public void VelocityComputedSignal(Vector2 safeVelocity)
    {
        Velocity = safeVelocity;
        MoveAndSlide();

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
