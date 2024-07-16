using Godot;
using System;
using System.Collections.Generic;
using static Asriela.BasicFunctions;
using System.Linq;

public partial class UseItem : Node
{
    Characters myCharacter;

    public UseItem(Characters myCharacter)
    {
        this.myCharacter = myCharacter;
    }
    public void UseTarget()
    {
        var furnitureItem = (Furniture)myCharacter.currentTarget;

        //START USING
        if (myCharacter.busyUsing == null)
        {
            if(furnitureItem.occupants.Count < furnitureItem.objectData.size) { 
            myCharacter.busyUsing = furnitureItem;
            //MAKE A FLAT WIDE EFFECT HAPPEN
            if(furnitureItem.objectData.flatWideEffect)
            EffectEntireFlat(furnitureItem.objectData);

            furnitureItem.occupants.Add(myCharacter);
            }
            else
            {
                myCharacter.busyUsing = null;
                myCharacter.currentTarget = null;
                myCharacter.usingTarget = null;
            }
     
        if(furnitureItem.occupants.Count==2)
            myCharacter.GlobalPosition = furnitureItem.myUseLocation2.GlobalPosition;
        else
            myCharacter.GlobalPosition = furnitureItem.myUseLocation1.GlobalPosition;
            myCharacter.mySprite.Texture = myCharacter.ChangeSprite("Sit");
        }
        //Log($"TIME LEFT: {alarm.Total(TimerType.actionLength)} / {alarm.Left(TimerType.actionLength)} | {alarm.Global()}", LogType.step );
        if (myCharacter.alarm.Ended(TimerType.actionLength))
        {

            Log("DONE USING ITEM", LogType.step);
            myCharacter.mySprite.Texture = myCharacter.ChangeSprite("");
            var item = ((Furniture)myCharacter.currentTarget).objectData;
            // Update heat values after task completion 
            foreach (var key in myCharacter.heatOfObjects.Keys.ToList())
            {
                var basePreferenceForObject = myCharacter.basePrefOfObjects[key];
                if (basePreferenceForObject > 0)
                    myCharacter.heatOfObjects[key] += basePreferenceForObject;
            }
            myCharacter.heatOfObjects[item] = 0;
            myCharacter.busyUsing = null;
            myCharacter.currentTarget = null;
            myCharacter.usingTarget = null;


            GetEffected(myCharacter.basePrefOfObjects[item]);
            var effectsBreakdown = CalculateBasePreference(myCharacter.characterData, item, out float sum);
            GetMoneyEffected(sum, effectsBreakdown);

            furnitureItem.occupants.Remove(myCharacter);


        }


    }

    public void EffectEntireFlat(Main.Object furnitureItem)
    {
        foreach(Characters tenant in Main.flatsList[myCharacter.myFlatNumber].charactersInFlat)
        {
            //calculate base preference for this object
             
            var effectsBreakdown = CalculateBasePreference(tenant.characterData,furnitureItem, out float sum );
            
            tenant.useItemArm.GetMoneyEffected(sum, effectsBreakdown);


                
        }
    }
    public Dictionary<Effect, int> CalculateBasePreference(Main.Character tenant, Main.Object furniture, out float sum)
    {

            Dictionary<Effect, int> effectsBreakdown = new Dictionary<Effect, int>();
            var objE = furniture.usedEffects;
            var charE = tenant.usedEffects;
            Log($"CALCULATING {furniture.type}", LogType.step);
            // Log($"Base Pref Of: {objectItem.objectData.name} is [objE]", LogType.step);

            //basePrefOfObjects.Add(objectItem, objE.Keys.Intersect(charE.Keys).Select(key => objE[key] * charE[key]).Sum());
            // Assuming objE and charE are dictionaries
            var intersectedKeys = objE.Keys.Intersect(charE.Keys).ToList();

            Log("Intersected Keys: " + string.Join(", ", intersectedKeys), LogType.step);


            var products = intersectedKeys.Select(key =>
            {
                var product = objE[key] * charE[key];
                effectsBreakdown.Add(key, product);
                //Log(, LogType.step);
                Log($"Key: {key}, objE[{key}] = {objE[key]}, charE[{key}] = {charE[key]}, Product: {product}", LogType.step);
                return product;
            }).ToList();

        sum= products.Sum();
        return effectsBreakdown;
    }
    public void GetEffected(float effectValue)
    {
        if (Main.TestGameMode != Main.testGameMode.complex) return;
        myCharacter.happiness += effectValue;
    }
    public void CalculateStaticHappiness()
    {
        if (Main.TestGameMode != Main.testGameMode.zooTycoon) return;
        myCharacter.happiness = myCharacter.basePrefOfObjects.Values.Sum();
    }
    public void GetMoneyEffected(float effectValue, Dictionary<Effect,int> effectBreakdown)
    {
        if (Main.TestGameMode != Main.testGameMode.flowingMoney) return;
        Main.Money += effectValue;
        myCharacter.lastMoneyEffect = (int)effectValue;

        if (effectValue < 0)
        {
            myCharacter.grumpyIcon.Visible = true;
            myCharacter.mySprite.Texture = myCharacter.ChangeSprite("Angry");
            myCharacter.alarm.Start(TimerType.grumpy, 5, false, 0);
        }

        


        int index=0;
        foreach (KeyValuePair<Effect,int> effect in effectBreakdown)
        {
            var effectLabel = (EffectLabel)AddUINode("res://UI/SCENES/effects_label.tscn", myCharacter);
            effectLabel.GlobalPosition = ChangePosition(effectLabel.GlobalPosition, 0, -100);

            if(myCharacter.bleedList.ContainsKey(effect.Key)) 
            myCharacter.bleedList[effect.Key] += effect.Value;


            effectLabel.SetLabel( effect.Key , effect.Value<0 ? true : false,  index);
            index++;
        }

    }
}
