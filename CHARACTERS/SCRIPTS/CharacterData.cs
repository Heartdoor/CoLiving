using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Asriela.BasicFunctions;
using static Main;

public class CharacterData
{
    #region Attributes
    public CharacterType name { get; set; }
    public string image_path { get; set; }
    public Texture2D texture { get; set; }
    public Texture2D shadowTexture { get; set; }
    public Dictionary<Effect, CharacterEffectors.EffectProperties> effectsList { get; set; }

    public Dictionary<CharacterController, CharacterEffectors.Relationship> relationshipsList { get; set; }

    public Dictionary<Effect, float> needs { get; set; }

    public Dictionary<Effect, float> feelings { get; set; }
    public Dictionary<Effect, float> desires { get; set; }

    public Emotion mainEmotion { get; set; }

    public bool debugItem { get; set; }

    public string emoji { get; set; }

    public CharacterEffectors.Goal currentGoal { get; set; }
    #endregion

    // Constructor
    public CharacterData(CharacterType name, string emoji, bool debugItem, Dictionary<Effect, CharacterEffectors.EffectProperties> effectsList)
    {
        this.name = name;
        image_path = $"res://CHARACTERS/SPRITES/{name}.png";
        texture = GetTexture2D(image_path);
        shadowTexture = GetTexture2D($"res://CHARACTERS/SPRITES/{name}_s.png");
        this.effectsList = new Dictionary<Effect, CharacterEffectors.EffectProperties>(effectsList);
        relationshipsList = new Dictionary<CharacterController, CharacterEffectors.Relationship>();
        feelings = new Dictionary<Effect, float>
        {
            { Effect.happiness, 0 },
            { Effect.romance, 0 }
        };
        needs = new Dictionary<Effect, float>();
        desires = new Dictionary<Effect, float>();
        mainEmotion = Emotion.neutral;
        this.emoji = emoji;
        this.debugItem = debugItem;
    }
}
