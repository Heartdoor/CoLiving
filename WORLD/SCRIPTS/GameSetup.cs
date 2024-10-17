using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using static Asriela.BasicFunctions;

internal class GameSetup
{
	FurnitureData furnitureItem;
	Main mainController = new Main();
	CharacterLoader cl = new CharacterLoader();
	FurnitureLoader fl = new FurnitureLoader();

	public void SetupStart()
	{
		fl.LoadFurniture();
		cl.LoadCharacters();
	}
}
