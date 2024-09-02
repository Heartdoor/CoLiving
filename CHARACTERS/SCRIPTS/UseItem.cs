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

    public void SocialInteractWithTarget()
    {
        ChangeColor(myCharacter.stateSquare, Settings.stateColorSocializing);
        if (myCharacter.stopCurrentAction)
        {
            StopSocializing();
            myCharacter.stopCurrentAction = false;
            myCharacter.canPerformAction = false;
            return;
        }
        myCharacter.isInInteraction = true;

        //setup access 
        //Furniture accessObject =null;
        //if(myCharacter.accessTarget!=null)
        //accessObject  = (Furniture)myCharacter.accessTarget;
        var socialTarget = (Characters)myCharacter.useTarget;

        //start social interaction
        if (myCharacter.interactingWith == null)
        {

            myCharacter.interactingWith=socialTarget;
            //add flat wide effects later 

            //set each other as in a social engagement
            myCharacter.interactingWithCharacter= socialTarget;
            socialTarget.interactingWithCharacter= myCharacter;

            //play social interaction animation
            PlayAnimation(myCharacter.myAnimator, $"{myCharacter.chosenInteractionWithCharacter}_{myCharacter.characterData.biggestEmotion}");

        }

        //end social interaction
        if (myCharacter.alarm.Ended(TimerType.actionLength))
        {
            GetEffectedBySocial();
            StopSocializing();
        }

    }
    public void UseFunitureTarget()
    {
        ChangeColor(myCharacter.stateSquare,Settings.stateColorUsingFurniture);
        if (myCharacter.stopCurrentAction)
        {
            StopUsingObject();
            myCharacter.stopCurrentAction = false;
            myCharacter.canPerformAction = false;
            return;
        }

        myCharacter.isInInteraction = true;

        var accessObject = (Furniture)myCharacter.accessTarget;
        var useObject = (Furniture)myCharacter.useTarget;

        if (accessObject == null)
            accessObject = useObject;
            //START USING
        if (myCharacter.interactingWith == null)
        {
            if(accessObject.occupants.Count < accessObject.objectData.size) 
            { 
                myCharacter.interactingWith = useObject;
                //MAKE A FLAT WIDE EFFECT HAPPEN
                if(useObject.objectData.flatWideEffect)
                EffectEntireFlat(useObject.objectData);

                useObject.occupants.Add(myCharacter);
                if(accessObject!= useObject)
                accessObject.occupants.Add(myCharacter);

                PlayAnimation(myCharacter.myAnimator, $"{useObject.objectData.useAnimation}_{myCharacter.characterData.biggestEmotion}");//myCharacter.ChangeSprite($"{ useObject.objectData.useAnimation}");

                if (!accessObject.objectData.ontopUsePosition)
                {
                    myCharacter.GlobalPosition = myCharacter.accessNode.GlobalPosition;
                }
                else
                    myCharacter.GlobalPosition = accessObject.occupants.IndexOf(myCharacter) == 0 ? accessObject.myUseLocation1.GlobalPosition : accessObject.myUseLocation2.GlobalPosition;

            }
            else
            {
                myCharacter.interactingWith = null;
                myCharacter.accessTarget = null;
                myCharacter.useTarget = null;
            }


        }
        //Log($"TIME LEFT: {alarm.Total(TimerType.actionLength)} / {alarm.Left(TimerType.actionLength)} | {alarm.Global()}", LogType.step );
        if (myCharacter.alarm.Ended(TimerType.actionLength))
        {


            var item = useObject.objectData;
            // Update heat values after task completion 
            foreach (var key in myCharacter.heatOfObjects.Keys.ToList())
            {
                var basePreferenceForObject = myCharacter.basePrefOfObjects[key];
                if (basePreferenceForObject > 0)
                    myCharacter.heatOfObjects[key] += basePreferenceForObject;
            }
            myCharacter.heatOfObjects[item] = 0;



            GetEffectedByFurniture(myCharacter.basePrefOfObjects[item]);
            var effectsBreakdown = CalculateBasePreference(myCharacter.characterData, item, false, out float sum);
            GetMoneyEffected(sum, effectsBreakdown);
            StopUsingObject();
            

        }

   


    }

    void StopUsingObject()
    {
        var accessObject = (Furniture)myCharacter.accessTarget;
        var useObject = (Furniture)myCharacter.useTarget;
        myCharacter.interactingWith = null;
        myCharacter.accessTarget = null;
        myCharacter.interactingWithTarget = null;


        useObject.occupants.Remove(myCharacter);
        if (accessObject != useObject)
            accessObject.occupants.Remove(myCharacter);

        Log("DONE USING ITEM", LogType.step);
        
        myCharacter.GlobalPosition = myCharacter.accessNode.GlobalPosition;
    }
    void StopSocializing()
    {
       // var accessObject = (Furniture)myCharacter.accessTarget;
        var socialTarget = (Characters)myCharacter.useTarget;
        myCharacter.interactingWith = null;
        myCharacter.accessTarget = null;
        myCharacter.interactingWithTarget = null;


        myCharacter.interactingWithCharacter = null;
        socialTarget.interactingWithCharacter = null;

        Log("✔DONE SOCIAL INTERACTION", LogType.step);

        
    }
    public void EffectEntireFlat(Main.Object furnitureItem)
    {
        foreach(Characters tenant in Main.flatsList[myCharacter.myFlatNumber].charactersInFlat)
        {
            //calculate base preference for this object
             
            var effectsBreakdown = CalculateBasePreference(tenant.characterData,furnitureItem,true, out float sum );
            
            tenant.useItemArm.GetMoneyEffected(sum, effectsBreakdown);


                
        }
    }
    public Dictionary<Effect, float> CalculateBasePreference(Main.Character tenant, Main.Object furniture, bool isRadiantEffect, out float sum)
    {

            Dictionary<Effect, float> effectsBreakdown = new Dictionary<Effect, float>();
            
            var objE = furniture.usedEffects;
            if (isRadiantEffect) objE = furniture.usedRadiantEffects;
            var charE = tenant.effectsList;
            Log($"CALCULATING {furniture.type}", LogType.step);
            // Log($"Base Pref Of: {objectItem.objectData.name} is [objE]", LogType.step);

            //basePrefOfObjects.Add(objectItem, objE.Keys.Intersect(charE.Keys).Select(key => objE[key] * charE[key]).Sum());
            // Assuming objE and charE are dictionaries
            var intersectedKeys = objE.Keys.Intersect(charE.Keys).ToList();

            Log("Intersected Keys: " + string.Join(", ", intersectedKeys), LogType.step);


            var products = intersectedKeys.Select(key =>
            {
                var product = objE[key] * charE[key].strength;
                effectsBreakdown.Add(key, product);
                //Log(, LogType.step);
                Log($"Key: {key}, objE[{key}] = {objE[key]}, charE[{key}] = {charE[key]}, Product: {product}", LogType.step);
                return product;
            }).ToList();

        sum= products.Sum();
        return effectsBreakdown;
    }
    public void GetEffectedByFurniture(float effectValue)
    {
        if (Main.TestGameMode != Main.testGameMode.complex) return;
        myCharacter.happiness += effectValue;
    }

    public void GetEffectedBySocial( )
    {
        var effect = myCharacter.chosenDesireToSocializeOn;
        myCharacter.characterData.desires[effect] = ClampMin(0,myCharacter.characterData.desires[effect]-myCharacter.characterData.effectsList[effect].actionSatisfaction);
        
    }

    public void CalculateStaticHappiness()
    {
        if (Main.TestGameMode != Main.testGameMode.zooTycoon) return;
        myCharacter.happiness = myCharacter.basePrefOfObjects.Values.Sum();
    }
    public void GetMoneyEffected(float effectValue, Dictionary<Effect, float> effectBreakdown)
    {
        if (Main.TestGameMode != Main.testGameMode.flowingMoney) return;
        Main.Money += effectValue;
        myCharacter.lastMoneyEffect = (int)effectValue;

        if (effectValue < 0)
        {
            myCharacter.grumpyIcon.Visible = true;
            myCharacter.stopCurrentAction = true;
            myCharacter.isUpset = true;
            PlayAnimation(myCharacter.myAnimator, $"upset");
            myCharacter.alarm.Start(TimerType.grumpy, 5, false, 0);
            myCharacter.characterData.feelings[Effect.happiness] += effectValue*Settings.tweak_negativeFlatEffectsBoost;

        }
        else
        myCharacter.characterData.feelings[Effect.happiness]+= effectValue;


        int index=0;
        foreach (KeyValuePair<Effect, float> effect in effectBreakdown)
        {
            var effectLabel = (EffectLabel)AddUINode("res://UI/SCENES/effects_label.tscn", myCharacter);
            effectLabel.GlobalPosition = ChangePosition(effectLabel.GlobalPosition, 0, -50);

            if(myCharacter.characterData.needs.ContainsKey(effect.Key)) 
            myCharacter.characterData.needs[effect.Key] += effect.Value;


            effectLabel.SetLabel( effect.Key , effect.Value<0 ? true : false,  index);
            index++;
        }

    }
}
