using Godot;
using System;
using System.Collections.Generic;
using static Asriela.BasicFunctions;
using System.Linq;

public partial class Interact : Node
{
    Characters myCharacter;


    public Interact(Characters myCharacter)
    {
        this.myCharacter = myCharacter;
    }

    public void SocialInteractWithTarget()
    {
        myCharacter.ChangeStateSquare(myCharacter.stateSquare, Settings.stateColorSocializing);
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
            // START SOCIAL INTERACTION
            myCharacter.interactingWith=socialTarget;
            //add flat wide effects later 

            //set each other as in a social engagement
            myCharacter.interactingWithCharacter= socialTarget;
            socialTarget.interactingWithCharacter= myCharacter;
            var lastPosition = myCharacter.GlobalPosition;
            FlipToFaceEachOther(myCharacter, socialTarget, myCharacter.myAnimator);

            //play social interaction animation
            if (DidSocialTargetRejectUs(socialTarget))
            {
                socialTarget.rejectingSomeone = myCharacter;
                PlayAnimationOnce(socialTarget.myAnimator, $"reject");
                PlayAnimationOnce(myCharacter.myAnimator, $"{myCharacter.chosenInteractionWithCharacter}_{myCharacter.characterData.mainEmotion}");
                FlipToFaceEachOther(socialTarget, myCharacter, socialTarget.myAnimator);
                socialTarget.alarm.Start(TimerType.sideActionLength, Settings.tweak_socialRejectionActionLength, false, 0);
                socialTarget.alarm.Pause(TimerType.actionLength);
                myCharacter.alarm.Start(TimerType.actionLength, Settings.tweak_socialRejectionActionLength, false, 0);
            }
            else
            {
                PlayAnimation(myCharacter.myAnimator, $"{myCharacter.chosenInteractionWithCharacter}_{myCharacter.characterData.mainEmotion}");


            }


        }

        //end social interaction
        if (myCharacter.alarm.Ended(TimerType.actionLength))
        {
            GetEffectedBySocial();
            StopSocializing();
        }

    }

    bool DidSocialTargetRejectUs(Characters socialTarget)
    {
        var myCharacterData = myCharacter.characterData;
        var targetCharacterData = socialTarget.characterData;
        var desireOfSocial = myCharacter.chosenDesireToSocializeOn;
        myCharacter.targetObjectedToSocialInteraction= false;

        //PHASE 1 WILL TARGET ENGAGE WITH TYPE OF SOCIAL INTERACTION  
        var targetsPreferenceToDesire = socialTarget.characterData.effectsList[desireOfSocial].strength;
        if (targetsPreferenceToDesire < 0)
            myCharacter.targetObjectedToSocialInteraction = true;



        //PHASE 2 DO THEY EVEN LIKE ME ENOUGH TO WANT MY INTERACTION
        if (!myCharacter.targetObjectedToSocialInteraction)
        {
            var targetsRelationshipWithMe = socialTarget.characterData.relationshipsList[myCharacter];
            if (targetsRelationshipWithMe.strength[RelationshipType.friendship] < Settings.tweak_minimumRelationshipToWantToInteract)
            {
                myCharacter.targetObjectedToSocialInteraction = true;
            }
        }

        

        return myCharacter.targetObjectedToSocialInteraction;
    }
    public void GetEffectedBySocial()
    {
        //SATISFY DESIRE
        

        HaveConversation();
        //CALCULATE RELATIONSHIP EFFECT



    }

   void HaveConversation()
    {
        /*4 ponged effect
         * 1- the type of social interaction
         * 2- the object target is using
         * 3- emotion they are both in
         * 4- their previous activity
         * */
        var socialTarget = (Characters)myCharacter.useTarget;
        var myCharacterData = myCharacter.characterData;
        var targetCharacterData = socialTarget.characterData;

        //1
        var typeOfSocial = myCharacter.chosenInteractionWithCharacter;

        //2
        var objectTargetIsUsing = (FurnitureController)socialTarget.interactingWith;
        //3
        var myEmotion = myCharacter.characterData.mainEmotion;
        var targetsEmotion = socialTarget.characterData.mainEmotion;
        //4
        //to be added




        //PHASE 3 BOND OVER TARGETS CURRENT ACTIVITY
        var myTotalPreference = 0f;
        var targetsTotalPreference = 0f;

        if (!myCharacter.targetObjectedToSocialInteraction && objectTargetIsUsing!=null)
        {
            //try to find common ground


            foreach(KeyValuePair<Effect,int> effect in objectTargetIsUsing.furnitureData.basicEffects)
            {
                var objectEffectValue =(float)effect.Value;
                myTotalPreference += objectEffectValue  * myCharacterData.effectsList[effect.Key].strength;
                targetsTotalPreference+= objectEffectValue* targetCharacterData.effectsList[effect.Key].strength;
            }
        }

        //PHASE 4 VIBE CHECK EMOTION COMBINATION
        //contextual anger - maybe in the future we can bond over being angry at something we both dislike
        //for now lets just see what the general vibe of both emotions are and if romance is involved
        var basicVibe = 0f;
        var romanticVibe = 0f;
        if (!myCharacter.targetObjectedToSocialInteraction)
        {
            basicVibe=myCharacterData.feelings[Effect.happiness]+ targetCharacterData.feelings[Effect.happiness];
            romanticVibe= myCharacterData.feelings[Effect.romance] + targetCharacterData.feelings[Effect.romance];
        }


        //PHASE 5 MIX EMOTION VIBES AND CONVERSATION BOND
        var basicImpactOnTarget = 0f;
        var basicImpactOnUs = 0f;

        if (!myCharacter.targetObjectedToSocialInteraction)
        {
            basicImpactOnUs= (basicVibe+ myTotalPreference)/Settings.tweak_socialInteractionDampner;
            basicImpactOnTarget = (basicVibe+ targetsTotalPreference) / Settings.tweak_socialInteractionDampner;

        }
        else
        {
            basicImpactOnTarget=-Settings.tweak_rejectionNegativeImpact;
            basicImpactOnUs =-Settings.tweak_rejectionNegativeImpact;
        }

        ImpactNeedsAndDesires(myCharacter, myCharacter.chosenDesireToSocializeOn, myCharacter.characterData.effectsList[myCharacter.chosenDesireToSocializeOn].actionSatisfaction);
        ImpactNeedsAndDesires(socialTarget, myCharacter.chosenDesireToSocializeOn, myCharacter.characterData.effectsList[myCharacter.chosenDesireToSocializeOn].actionSatisfaction/2);
        ImpactFeelings(myCharacter, basicImpactOnUs);
        ImpactFeelings(socialTarget, basicImpactOnTarget);
        ImpactRelationships(myCharacter, socialTarget, basicImpactOnUs);
        ImpactRelationships(socialTarget, myCharacter, basicImpactOnTarget);
        //if both happy and both like the activity then romantic can happen!?
    }
    void ImpactNeedsAndDesires(Characters character, Effect effect, float impact)
    {
      
        character.characterData.desires[effect] = ClampMin(0, character.characterData.desires[effect] - impact);
        character.characterData.needs[effect] = ClampMin(0, character.characterData.needs[effect] - impact);
   }
    void ImpactFeelings(Characters character, float impact )
    {
        //effect feelings 
        character.characterData.feelings[Effect.happiness] += impact;
    }
    void ImpactRelationships(Characters subjectCharacter,Characters objectCharacter, float impact)
    {
        subjectCharacter.characterData.relationshipsList[objectCharacter].strength[RelationshipType.friendship] += impact;
    }

    public void UseFunitureTarget()
    {
        myCharacter.ChangeStateSquare(myCharacter.stateSquare, Settings.stateColorUsingFurniture);
        if (myCharacter.stopCurrentAction)
        {
            StopUsingObject();
            myCharacter.stopCurrentAction = false;
            myCharacter.canPerformAction = false;
            return;
        }

        myCharacter.isInInteraction = true;

        var accessObject = (FurnitureController)myCharacter.accessTarget;
        var useObject = (FurnitureController)myCharacter.useTarget;

        if (accessObject == null)
            accessObject = useObject;
            //START USING
        if (myCharacter.interactingWith == null)
        {
            if(accessObject.occupants.Count < accessObject.furnitureData.size) 
            { 
                myCharacter.interactingWith = useObject;
                //MAKE A FLAT WIDE EFFECT HAPPEN
                if(useObject.furnitureData.flatWideEffect)
                EffectEntireFlat(useObject.furnitureData);

                myCharacter.mySeatingIndex = accessObject.occupants.Count; 

                
                if (accessObject.occupants.Count==1)
                {
                    var otherSeater = (Characters)accessObject.occupants[0];
                    if(otherSeater.mySeatingIndex==1)
                        myCharacter.mySeatingIndex = 0;
                }
               
              
                
                useObject.occupants.Add(myCharacter);
                
                if (accessObject!= useObject)
                accessObject.occupants.Add(myCharacter);

                PlayAnimation(myCharacter.myAnimator, $"{useObject.furnitureData.useAnimation}_{myCharacter.characterData.mainEmotion}");//myCharacter.ChangeSprite($"{ useObject.objectData.useAnimation}");

                if (!accessObject.furnitureData.ontopUsePosition)
                {
                    myCharacter.GlobalPosition = myCharacter.accessNode.GlobalPosition;
                }
                else
                    myCharacter.GlobalPosition = myCharacter.mySeatingIndex == 0 ? accessObject.myUseLocation1.GlobalPosition : accessObject.myUseLocation2.GlobalPosition;

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


            var item = useObject.furnitureData;
            // Update heat values after task completion 
            foreach (var key in myCharacter.heatOfObjects.Keys.ToList())
            {
                var basePreferenceForObject = myCharacter.basePrefOfObjects[key];
                if (basePreferenceForObject > 0)
                    myCharacter.heatOfObjects[key] += basePreferenceForObject;
            }
            myCharacter.heatOfObjects[item] = 0;


            GetEffectedByFurniture(BuildingController.GetRoomDecorEffect(myCharacter.characterData, myCharacter.roomIAmIn ));
            GetEffectedByFurniture(myCharacter.basePrefOfObjects[item]);
            var effectsBreakdown = CalculateBasePreference(myCharacter.characterData, item, false, out float sum);
            
            GetMoneyEffected(sum, null,effectsBreakdown);
            StopUsingObject();
            

        }

   


    }

    void StopUsingObject()
    {
        var accessObject = (FurnitureController)myCharacter.accessTarget;
        var useObject = (FurnitureController)myCharacter.useTarget;
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
        myCharacter.targetObjectedToSocialInteraction = false;

        myCharacter.interactingWithCharacter = null;
        socialTarget.interactingWithCharacter = null;

        Log("✔DONE SOCIAL INTERACTION", LogType.step);

        
    }
    public void EffectEntireFlat(Main.FurnitureItem furnitureItem)
    {
        foreach(Characters tenant in BuildingController.buildingsList[myCharacter.myBuildingNumber].charactersInBuilding)
        {
            //calculate base preference for this object
             
            var effectsBreakdown = CalculateBasePreference(tenant.characterData,furnitureItem,true, out float sum );
            
            tenant.useItemArm.GetMoneyEffected(sum, myCharacter, effectsBreakdown);


                
        }
    }
    public Dictionary<Effect, float> CalculateBasePreference(Main.CharacterItem tenant, Main.FurnitureItem furniture, bool isRadiantEffect, out float sum)
    {

            Dictionary<Effect, float> effectsBreakdown = new Dictionary<Effect, float>();
            
            var objE = furniture.basicEffects;
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

   

    public void CalculateStaticHappiness()
    {
        if (Main.TestGameMode != Main.testGameMode.zooTycoon) return;
        myCharacter.happiness = myCharacter.basePrefOfObjects.Values.Sum();
    }
    public void GetMoneyEffected(float effectValue, Characters characterSource, Dictionary<Effect, float> effectBreakdown)
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
            if(characterSource!=null)
            myCharacter.ImpactRelationship(characterSource, RelationshipType.friendship,effectValue * Settings.tweak_negativeFlatEffectsBoost);
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
