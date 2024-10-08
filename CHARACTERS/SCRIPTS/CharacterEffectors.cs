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
        public DesireAction action { get; set; }
        public float actionLength { get; set; }
        public float actionSatisfaction { get; set; }
        public bool syncDesireWithNeed { get; set; }

        public EffectProperties(float strength, float needBleedRate, float desireGrowRate, DesireAction action, float actionLength, float actionSatisfaction, bool syncDesireWithNeed)
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
