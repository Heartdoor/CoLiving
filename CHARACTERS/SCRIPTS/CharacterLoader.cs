//using Godot;
//using System.Collections.Generic;

//public class CharacterData
//{
//    public string name { get; set; }
//    public string emoji { get; set; }
//    public bool debugItem { get; set; }
//    public Dictionary<string, Dictionary<string, float>> effects { get; set; }
//}

//public partial class CharacterLoader : Node
//{
//    // Save character data to a file
//    public void SaveCharacterData(List<CharacterData> characters, string filePath)
//    {
//        File file = new File();
//        file.Open(filePath, File.ModeFlags.Write);

//        var json = JSON.Print(characters); // Convert the list to JSON
//        file.StoreString(json); // Save it to the file

//        file.Close();
//    }

//    // Load character data from a file
//    public List<CharacterData> LoadCharacterData(string filePath)
//    {
//        File file = new File();
//        if (!file.FileExists(filePath))
//        {
//            GD.PrintErr($"File not found: {filePath}");
//            return null;
//        }

//        file.Open(filePath, File.ModeFlags.Read);
//        var jsonData = file.GetAsText();
//        file.Close();

//        var parsedData = JSON.Parse(jsonData);
//        if (parsedData.Error != Error.Ok)
//        {
//            GD.PrintErr("Failed to parse JSON");
//            return null;
//        }

//        var characterList = new List<CharacterData>();
//        var jsonArray = parsedData.Result as Godot.Collections.Array;

//        foreach (var characterObj in jsonArray)
//        {
//            var dict = characterObj as Godot.Collections.Dictionary;
//            var characterData = new CharacterData
//            {
//                name = dict["name"].ToString(),
//                emoji = dict["emoji"].ToString(),
//                debugItem = (bool)dict["debugItem"],
//                effects = new Dictionary<string, Dictionary<string, float>>()
//            };

//            var effectsDict = dict["effects"] as Godot.Collections.Dictionary;
//            foreach (string effect in effectsDict.Keys)
//            {
//                var effectProps = effectsDict[effect] as Godot.Collections.Dictionary;
//                var propsDict = new Dictionary<string, float>();
//                foreach (string propKey in effectProps.Keys)
//                {
//                    propsDict[propKey] = (float)effectProps[propKey];
//                }
//                characterData.effects[effect] = propsDict;
//            }

//            characterList.Add(characterData);
//        }

//        return characterList;
//    }
//}
