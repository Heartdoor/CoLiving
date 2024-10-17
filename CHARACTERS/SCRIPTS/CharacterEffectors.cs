using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Asriela.BasicFunctions;

public class CharacterEffectors
{
    public class EffectProperties
    {
        public float strength { get; set; }
        public float needBleedRate { get; set; }
        public float desireGrowRate { get; set; }
        public SocialAction action { get; set; }
        public float actionLength { get; set; }
        public float actionSatisfaction { get; set; }
        public bool syncDesireWithNeed { get; set; }



        public EffectProperties(float strength, float needBleedRate, float desireGrowRate, SocialAction action, float actionLength, float actionSatisfaction, bool syncDesireWithNeed)
        {

            this.strength = strength;
            this.needBleedRate = needBleedRate;
            this.desireGrowRate = desireGrowRate;
            this.action = action;
            this.syncDesireWithNeed = syncDesireWithNeed;
            this.actionLength = actionLength;
            this.actionSatisfaction = actionSatisfaction;
        }
    }

    public abstract class Goal
    {
        public GoalType Type { get; protected set; }
        public TargetWrapper Target { get; protected set; }

        public int RepeatsNeededToAchieveGoal { get; protected set; }


        protected Goal(GoalType type, TargetWrapper target, int repeatsNeededToAchieveGoal)
        {
            Type = type;
            Target = target;
            RepeatsNeededToAchieveGoal = repeatsNeededToAchieveGoal;
        }
    }

    // Specific Goal for Socializing
    public class SocializeGoal : Goal
    {
        public SocialAction SocialActionType { get; set; } // The additional property

        public SocializeGoal(CharacterController target, SocialAction action, int repeatsNeeded)
            : base(GoalType.socialize, new TargetWrapper(target), repeatsNeeded)
        {
            SocialActionType = action;
        }
    }

    // Specific Goal for Using an Item
    public class UseItemGoal : Goal
    {
        public UseItemGoal(FurnitureName target, int repeatsNeeded)
            : base(GoalType.useObject, new TargetWrapper(target), repeatsNeeded)
        {
        }
    }


    public class Relationship
    {
        public Dictionary<RelationshipType, float> strength { get; set; }

        public Relationship()
        {
            strength = new Dictionary<RelationshipType, float>();
            strength[RelationshipType.friendship] = 0;
            strength[RelationshipType.romantic] = 0;
        }
    }
}
