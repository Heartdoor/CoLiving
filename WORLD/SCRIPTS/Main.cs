using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static Asriela.BasicFunctions;

public partial class Main : Node2D
{
	#region STATES
	public static Control SelectionMenuOpen = null;
	public static bool canPlace = true;
	#endregion

	#region MAIN CONTROL SETUP
	public static float Money = 200f;
	public static FurnitureData HeldFurnitureItem = null;
	public static CharacterData HeldCharacter = null;


	public static int BuildingNumberMouseIsIn = 1;
	public static int RoomNumberMouseIsIn = 1;
	int lastFlatWeWereIn = 1;
	public static List<CharacterData> CharactersAvailableToPlayerList = new List<CharacterData>();
	public static List<CharacterData> PlacedCharactersList = new List<CharacterData>();
	public static List<FurnitureController> leaderFurnitureList = new List<FurnitureController>();
	public static int OverLeaderFurnitureLayer = 0;

	public Sprite2D placeItemImage;
	public static TileMap MyTileMap;
	public static Label MyUnlocksLabel;
	float highestUnlock = 0;

	public static bool OverPlaceableTile;
	#endregion

	#region SETTINGS
	public static bool SpawnInitialObjects = false;
	public static bool MoneyCheat = true;
	public static bool UnlockCheat = true;

	public static bool QuickRestart = true;
	public enum testGameMode : short
	{
		complex,
		zooTycoon,
		flowingMoney

	}
	public static testGameMode TestGameMode = testGameMode.flowingMoney;
	#endregion

	#region STATICS
	public static CharacterController SelectedCharacter = null;
	public static string DebugEffectOnMouse = "";
	#endregion

	#region CLASSES SETUP
	public static List<FurnitureData> FurnitureUnlockedList = new List<FurnitureData>();
	public int startingAmountOfUnlockedFurniture;
	public static List<FurnitureData> objectsList = new List<FurnitureData>();
	public static List<CharacterData> charactersList = new List<CharacterData>();


	#endregion

	#region COLOR SETUPS
	public static readonly Color ColorCantBePlaced = new Color(0xff00008d);
	public static readonly Color ColorPlace = new Color(0x00ffff64);
	#endregion

	#region LOCAL SETUPS
	FurnitureData o;
	DebugHotkeys debugHotkeys = new DebugHotkeys();
	#endregion

	#region SETUPS


	void SetupUI()
	{
		placeItemImage = GetNode<Node2D>("PlaceItemImage").GetNode<Sprite2D>("Sprite2D");
		MyUnlocksLabel = GetNode<CanvasLayer>("CanvasLayer").GetNode<Label>("UnlocksLabel");
	}


	#endregion


	void SetupDebugHotkeys()
	{
		debugHotkeys.myMain = this;
		debugHotkeys.Start();
	}


	void Start()
	{
		//So far this sets up Characters and Furniture, can expand with UI and Flats eventually
		GameSetup gameSetup = new GameSetup();
		gameSetup.SetupStart();

		SetupUI();
		SetupDebugHotkeys();
	}

	void Run()
	{

		RunDebugHotkeys();

		if (canPlace)
		{
			PlaceObjects(ref HeldFurnitureItem, false, new Vector2(0, 0));
			PlaceCharacter(ref HeldCharacter, false, new Vector2(0, 0));

			if (HeldFurnitureItem == null && HeldCharacter == null)
				placeItemImage.Texture = null;
		}
		else
			placeItemImage.Texture = null;

		UnlockNewCharacters();


	}
	void UnlockNewCharacters()
	{
		if (Money > 400 && highestUnlock < 400)
		{
			highestUnlock = 400;
			UnlockNewCharacter();
		}

	}
	public FurnitureData ChooseNewFurnitureToUnlock()
	{
		Random random = new Random();
		FurnitureData chosenItem;
		var tries = 50;
		var i = 0;
		do
		{
			chosenItem = objectsList.OrderBy(x => random.Next()).FirstOrDefault();
			i++;
		}
		while (FurnitureUnlockedList.Contains(chosenItem) && i < tries);

		if (i < tries)
			FurnitureUnlockedList.Add(chosenItem);
		else
			chosenItem = null;
		return chosenItem;
	}

	public CharacterData ChooseNewCharacterToUnlock()
	{
		Random random = new Random();
		CharacterData chosenItem;
		var tries = 50;
		var i = 0;
		do
		{


			chosenItem = charactersList.OrderBy(x => random.Next()).FirstOrDefault();
			i++;
		}
		while ((CharactersAvailableToPlayerList.Contains(chosenItem) || PlacedCharactersList.Contains(chosenItem)) && i < tries);

		if (i < tries)
			CharactersAvailableToPlayerList.Add(chosenItem);
		else
			chosenItem = null;
		return chosenItem;
	}
	void UnlockNewFurniture()
	{
		var unlocksLabelClass = (UnlocksLabel)MyUnlocksLabel;

		var furnitureItem = ChooseNewFurnitureToUnlock();
		if (furnitureItem != null)
			unlocksLabelClass.NewUnlock($"{furnitureItem.type}");
	}

	void UnlockNewCharacter()
	{
		var unlocksLabelClass = (UnlocksLabel)MyUnlocksLabel;

		var furnitureItem = ChooseNewCharacterToUnlock();
		if (furnitureItem != null)
			unlocksLabelClass.NewUnlock($"{furnitureItem.name}");
	}
	public void PlaceObjects(ref FurnitureData HeldFurnitureItem, bool placeManually, Vector2 position)
	{

		if (HeldFurnitureItem == null) return;

		BuildingController.RoomItem roomWeAreIn = null;
		if (BuildingNumberMouseIsIn > 0 && RoomNumberMouseIsIn > 0)
			roomWeAreIn = BuildingController.buildingsList[BuildingNumberMouseIsIn].rooms[RoomNumberMouseIsIn];

		OverPlaceableTile = false;

		if (roomWeAreIn != null && BuildingNumberMouseIsIn > 0 && RoomNumberMouseIsIn > -1)
			if ((HeldFurnitureItem.type == FurnitureType._core && (roomWeAreIn.type == RoomType.none || roomWeAreIn.type == HeldFurnitureItem.roomTypes[0])) ||
				  (HeldFurnitureItem.type != FurnitureType._core && HeldFurnitureItem.roomTypes.Contains(roomWeAreIn.type)))
				OverPlaceableTile = true;



		placeItemImage.GlobalPosition = GameTileGrid.cellCoordinates * MyTileMap.TileSet.TileSize + MyTileMap.TileSet.TileSize / 2;
		placeItemImage.Texture = HeldFurnitureItem.texture;
		if (OverPlaceableTile)
			ChangeColor(placeItemImage, ColorPlace);
		else
			ChangeColor(placeItemImage, ColorCantBePlaced);

		var cost = HeldFurnitureItem.price;
		if ((KeyPressed("RightClick") && Money >= cost && OverPlaceableTile) || placeManually)
		{


			var newObject = Add2DNode("res://FURNITURE/SCENES/object.tscn", this);
			var newObjectClass = (FurnitureController)newObject;
			var newObjectData = newObjectClass.furnitureData;
			newObjectData = HeldFurnitureItem;
			newObjectClass.furnitureData = newObjectData;
			newObjectClass.myShadow.Texture = HeldFurnitureItem.shadowTexture;
			newObjectClass.mySprite.Texture = placeItemImage.Texture;
			newObjectClass.myFlatNumber = BuildingNumberMouseIsIn;
			newObjectClass.roomIAmIn = RoomNumberMouseIsIn;
			newObjectClass.myRoom = roomWeAreIn;

			if (newObjectData.floorObject)
				newObjectClass.mySprite.ZIndex = -2000;

			if (HeldFurnitureItem.type == FurnitureType._core)
			{
				roomWeAreIn.type = newObjectData.roomTypes[0];
				GameTileGrid.SetFloorMaterial(RoomNumberMouseIsIn, roomWeAreIn.type);
			}


			BuildingController.AddFurnitureToRoom(BuildingNumberMouseIsIn, roomWeAreIn, newObject);

			newObjectClass.ChangeDimensions(newObjectData.size);



			if (placeManually)
			{
				newObject.GlobalPosition = position;


			}

			else
			{
				UnlockNewFurniture();
				newObject.GlobalPosition = placeItemImage.GlobalPosition;

				Money -= cost;
			}




			if (placeManually == false)
				if (HeldCharacter != null || HeldFurnitureItem != null)
				{
					HoldNothing();

					Destroy(SelectionMenuOpen);
					SelectionMenuOpen = null;

				}
		}
	}


	public CharacterController PlaceCharacter(ref CharacterData heldCharacter, bool placeManually, Vector2 position)
	{
		if (heldCharacter == null) return null;

		CharacterController newCharacterClass = null;
		placeItemImage.GlobalPosition = GetGlobalMousePosition();
		placeItemImage.Texture = heldCharacter.texture;

		if (KeyPressed("RightClick") || placeManually)
		{
			PlacedCharactersList.Add(heldCharacter);



			var newObject = Add2DNode("res://CHARACTERS/SCENES/character.tscn", this);
			if (placeManually)
				newObject.GlobalPosition = position;
			else
				newObject.GlobalPosition = GetGlobalMousePosition();
			var character = heldCharacter;
			Main.CharactersAvailableToPlayerList = Main.CharactersAvailableToPlayerList.Where(x => x != character).ToList();

			newCharacterClass = (CharacterController)newObject;
			newCharacterClass.characterData = heldCharacter;
			newCharacterClass.SetupBleedList();
			newCharacterClass.myAnimator.SpriteFrames = (SpriteFrames)GetResource($"res://CHARACTERS/ANIMATIONS/{newCharacterClass.characterData.name}Animations.tres");
			newCharacterClass.myAnimator.Animation = "idle_neutral";
			newCharacterClass.myShadow.Texture = heldCharacter.shadowTexture;
			newCharacterClass.myBuildingNumber = BuildingNumberMouseIsIn;
			newCharacterClass.AddMyselfToEveryonesRelationshipsList();
			BuildingController.buildingsList[BuildingNumberMouseIsIn].charactersInBuilding.Add(newCharacterClass);
			if (placeManually == false)
				if (heldCharacter != null)
				{
					HoldNothing();
					Destroy(Main.SelectionMenuOpen);
					Main.SelectionMenuOpen = null;

				}
		}
		return newCharacterClass;


	}



	public static void HoldNothing()
	{
		HeldFurnitureItem = null;
		HeldCharacter = null;
	}

	void RunDebugHotkeys()
	{
		if (KeyPressed("Restart") && QuickRestart)
		{
			RestartScene(this);
		}
		if (KeyPressed("Money") && MoneyCheat)
		{
			Money += 100;
		}
		if (KeyPressed("UnlockObject") && UnlockCheat)
		{
			UnlockNewFurniture();
		}

		debugHotkeys.Run();

	}

	void ClearObjectListData(ref Dictionary<Effect, int> usedEffects, ref Vector2 usePosition, ref List<FurnitureGroup> furnitureGroups, ref FurnitureGroup isGroupLeader)
	{

		usedEffects.Clear();
		furnitureGroups.Clear();
		usePosition.X = 0;
		usePosition.Y = 0;
		isGroupLeader = FurnitureGroup.none;
	}

	public void ClearCharacterListData(ref Dictionary<Effect, CharacterEffectors.EffectProperties> usedEffects)
	{

		usedEffects.Clear();

	}



	public static FurnitureData GetObjectFromType(FurnitureName name)
	{
		var correctObject = objectsList.FirstOrDefault(obj => obj.name == name);
		return correctObject;
	}

	public static CharacterData GetCharacterFromType(CharacterType name)
	{
		var correctObject = charactersList.FirstOrDefault(obj => obj.name == name);
		return correctObject;
	}



	#region OLD
	public override void _Ready()
	{
		Start();
	}
	public override void _Process(double delta)
	{
		Run();
	}
	#endregion
}
