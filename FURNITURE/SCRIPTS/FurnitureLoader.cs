using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using static Asriela.BasicFunctions;

public class FurnitureConfig
{
	public List<FurnitureJson> FurnitureItems { get; set; }
}

public class FurnitureJson
{
	public string Name { get; set; }
	public bool FlatWideEffect { get; set; }
	public string Type { get; set; }
	public List<string> RoomTypes { get; set; }
	public int Size { get; set; }
	public int Price { get; set; }
	public float UseLength { get; set; }
	public string Rotation { get; set; }
	public List<string> AccessPositions { get; set; }
	public string UseAnimation { get; set; }
	public Dictionary<string, int> BasicEffects { get; set; }
	public Dictionary<string, int> UsedRadiantEffects { get; set; }
	public bool OntopUsePosition { get; set; }
	public bool DebugItem { get; set; }
}

public partial class FurnitureLoader : Node
{
	Main mainController = new Main();

	public void LoadFurniture()
	{
		string jsonFilePath = "res://CONFIGS/FurnitureConfig.json";
		string absolutePath = ProjectSettings.GlobalizePath(jsonFilePath);

		if (File.Exists(absolutePath))
		{
			string jsonContent = File.ReadAllText(absolutePath);
			GD.Print("Furniture file content loaded successfully.");
			FurnitureConfig furnitureConfig = JsonConvert.DeserializeObject<FurnitureConfig>(jsonContent);

			foreach (var furniture in furnitureConfig.FurnitureItems)
			{
				var roomTypes = new List<RoomType>();
				foreach (var roomType in furniture.RoomTypes)
				{
					// Debug print for room types
					GD.Print($"Parsing RoomType: {roomType}");
					roomTypes.Add((RoomType)Enum.Parse(typeof(RoomType), roomType));
				}

				var accessPositions = new List<AccessPosition>();
				foreach (var position in furniture.AccessPositions)
				{
					GD.Print($"Parsing AccessPosition: {position}");
					accessPositions.Add((AccessPosition)Enum.Parse(typeof(AccessPosition), position));
				}

				var basicEffects = new Dictionary<Effect, int>();
				foreach (var effect in furniture.BasicEffects)
				{
					Effect effectType = (Effect)Enum.Parse(typeof(Effect), effect.Key);
					basicEffects.Add(effectType, effect.Value);
				}

				var usedRadiantEffects = new Dictionary<Effect, int>();
				if (furniture.UsedRadiantEffects != null)
				{
					foreach (var effect in furniture.UsedRadiantEffects)
					{
						Effect effectType = (Effect)Enum.Parse(typeof(Effect), effect.Key);
						usedRadiantEffects.Add(effectType, effect.Value);
					}
				}

				var furnitureData = new FurnitureData(
					(FurnitureName)Enum.Parse(typeof(FurnitureName), furniture.Name),
					furniture.FlatWideEffect,
					furniture.Size,
					furniture.Price,
					furniture.UseLength
				)
				{
					type = (FurnitureType)Enum.Parse(typeof(FurnitureType), furniture.Type),
					roomTypes = roomTypes,
					rotation = (Direction)Enum.Parse(typeof(Direction), furniture.Rotation),
					accessPositions = accessPositions,
					useAnimation = (UseAnimation)Enum.Parse(typeof(UseAnimation), furniture.UseAnimation),
					basicEffects = basicEffects,
					usedRadiantEffects = usedRadiantEffects,
					ontopUsePosition = furniture.OntopUsePosition,
					debugItem = furniture.DebugItem
				};
				GD.Print($"Loaded Furniture: {furnitureData.name}, RoomTypes: {string.Join(", ", roomTypes)}");

				Main.furnitureList.Add(furnitureData);
			}

			if (Settings.furnitureUnlocked)
				mainController.startingAmountOfUnlockedFurniture = Main.furnitureList.Count;
			else
				mainController.startingAmountOfUnlockedFurniture = 2;

			for (int i = 0; i < mainController.startingAmountOfUnlockedFurniture; i++)
			{
				mainController.ChooseNewFurnitureToUnlock();
			}
		}
		else
		{
			GD.PrintErr($"Furniture file does not exist: {absolutePath}");
		}
	}


}
