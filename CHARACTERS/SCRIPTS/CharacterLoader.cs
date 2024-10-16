using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Asriela.BasicFunctions;

public class CharactersConfig
{
    public List<CharacterJson> Characters { get; set; }
}

public class CharacterJson
{
    public string Name { get; set; }
    public string Emoji { get; set; }
    public bool DebugItem { get; set; }
    public List<EffectJson> Effects { get; set; }
}

public class EffectJson
{
    public string Type { get; set; }
    public int Strength { get; set; }
    public string NeedBleedRate { get; set; }
    public string DesireGrowRate { get; set; }
    public string Action { get; set; }
    public float ActionLength { get; set; }
    public float ActionSatisfaction { get; set; }
    public bool SyncDesireWithNeed { get; set; }
}

public static class Constants
{
    public const float sleepyR = 0.001f;
    public const float hungryR = 0.01f;
    public const float hygieneR = 0.001f;
    public const float socialR = 0.0001f;
    public const float desireR = 0.0001f;
}

public partial class CharacterLoader : Node
{
    Main mainController = new Main();
    public void LoadCharacters()
    {
        string jsonFilePath = "res://CONFIGS/CharactersConfig.json";
        string absolutePath = ProjectSettings.GlobalizePath(jsonFilePath);
        if (File.Exists(absolutePath))
        {
            string jsonContent = File.ReadAllText(absolutePath);
            GD.Print("File content loaded successfully.");
            CharactersConfig charactersConfig = JsonConvert.DeserializeObject<CharactersConfig>(jsonContent);

            foreach (var character in charactersConfig.Characters)
            {
                var myEffects = new Dictionary<Effect, CharacterEffectors.EffectProperties>();

                foreach (var effect in character.Effects)
                {
                    Effect effectType = (Effect)Enum.Parse(typeof(Effect), effect.Type);
                    DesireAction actionType = DesireAction.none;
                    if (!string.IsNullOrEmpty(effect.Action))
                    {
                        actionType = (DesireAction)Enum.Parse(typeof(DesireAction), effect.Action);
                    }

                    // Check for placeholders and replace them with constants
                    float needBleedRate = GetRateValue(effect.NeedBleedRate);
                    float desireGrowRate = GetRateValue(effect.DesireGrowRate);

                    myEffects.Add(effectType, new CharacterEffectors.EffectProperties(
                        effect.Strength,
                        needBleedRate,
                        desireGrowRate,
                        actionType,
                        effect.ActionLength,
                        effect.ActionSatisfaction,
                        effect.SyncDesireWithNeed
                    ));
                }

                var charInfo = new CharacterData(
                    (CharacterType)Enum.Parse(typeof(CharacterType), character.Name),
                    character.Emoji,
                    character.DebugItem,
                    myEffects
                );

                Main.charactersList.Add(charInfo);
                mainController.ClearCharacterListData(ref myEffects);
            }

            var startingAmountOfUnlocked = 1;
            if (Settings.charactersUnlocked)
                startingAmountOfUnlocked = Main.charactersList.Count();

            for (int i = 0; i < startingAmountOfUnlocked; i++)
            {
                mainController.ChooseNewCharacterToUnlock();
            }
        }
        else
        {
            GD.PrintErr($"File does not exist: {absolutePath}");
        }
    }

    float GetRateValue(object rate)
    {
        if (rate is string rateString)
        {
            // Check if the rateString contains a multiplication sign
            if (rateString.Contains("*"))
            {
                // Split the string to extract the components (e.g., "sleepyR * 10")
                string[] parts = rateString.Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 2)
                {
                    // Trim parts and extract constant and multiplier
                    string constantPart = parts[0].Trim();
                    if (float.TryParse(parts[1].Trim(), out float multiplier))
                    {
                        // Lookup the constant value
                        float constantValue = GetConstantValue(constantPart);

                        // Perform the multiplication
                        return constantValue * multiplier;
                    }
                }
            }
            else
            {
                // Return the constant value if it's a single constant (no multiplication)
                return GetConstantValue(rateString);
            }
        }
        else if (rate is float)
        {
            // Return the value directly if it's already a float
            return (float)rate;
        }

        return 0; // Default value if nothing matches
    }

    // Helper method to retrieve constant values based on string keys
    float GetConstantValue(string key)
    {
        switch (key)
        {
            case "sleepyR": return Constants.sleepyR;
            case "hungryR": return Constants.hungryR;
            case "hygieneR": return Constants.hygieneR;
            case "socialR": return Constants.socialR;
            case "desireR": return Constants.desireR;
            default: return 0; // Default if constant not found
        }
    }
}
