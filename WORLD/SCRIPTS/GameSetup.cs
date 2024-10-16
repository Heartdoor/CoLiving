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

	public void SetupStart()
	{
		SetupFurnitureItems();
		cl.LoadCharacters();
	}

	void AddNewFurnitureItem(FurnitureName name)
	{

		furnitureItem = new FurnitureData(name);
		// Referenced using class instead of instance because the lists are labelled as static
		Main.objectsList.Add(furnitureItem);

	}

	void SetupFurnitureItems()
	{

		AddNewFurnitureItem(FurnitureName.debug1);
		furnitureItem.debugItem = true;
		furnitureItem.type = FurnitureType._core;
		furnitureItem.roomTypes.Add(RoomType.livingroom);
		furnitureItem.flatWideEffect = false;
		furnitureItem.size = 1;
		furnitureItem.price = 0;
		furnitureItem.useLength = 10;
		furnitureItem.rotation = Direction.down;
		furnitureItem.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down, AccessPosition.up, AccessPosition.left, AccessPosition.right });
		furnitureItem.useAnimation = UseAnimation.idle;
		furnitureItem.basicEffects.Add(Effect.debug1, 5);


		AddNewFurnitureItem(FurnitureName.couch);
		furnitureItem.type = FurnitureType._core;
		furnitureItem.group = FurnitureGroup.chair;
		furnitureItem.roomTypes.Add(RoomType.livingroom);
		furnitureItem.flatWideEffect = false;
		furnitureItem.size = 2;
		furnitureItem.price = 100;
		furnitureItem.useLength = 10;
		furnitureItem.ontopUsePosition = true;
		furnitureItem.rotation = Direction.down;
		furnitureItem.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down });
		furnitureItem.useAnimation = UseAnimation.sit;
		furnitureItem.basicEffects.Add(Effect.comfort, 1);



		AddNewFurnitureItem(FurnitureName.electricGuitar);
		furnitureItem.type = FurnitureType._object;
		furnitureItem.roomTypes.Add(RoomType.livingroom);
		furnitureItem.flatWideEffect = true;
		furnitureItem.size = 1;
		furnitureItem.price = 200;
		furnitureItem.useLength = 10;
		furnitureItem.rotation = Direction.down;
		furnitureItem.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down, AccessPosition.up, AccessPosition.left, AccessPosition.right });
		furnitureItem.useAnimation = UseAnimation.strumGuitar;
		furnitureItem.basicEffects.Add(Effect.music, 3);
		furnitureItem.basicEffects.Add(Effect.noise, 3);
		furnitureItem.basicEffects.Add(Effect.grunge, 2);

		furnitureItem.usedRadiantEffects.Add(Effect.noise, 2);
		furnitureItem.usedRadiantEffects.Add(Effect.grunge, 2);

		AddNewFurnitureItem(FurnitureName.recordPlayer);
		furnitureItem.type = FurnitureType._object;
		furnitureItem.useFromGroup = FurnitureGroup.chair;
		furnitureItem.roomTypes.Add(RoomType.livingroom);
		furnitureItem.flatWideEffect = true;
		furnitureItem.size = 1;
		furnitureItem.price = 100;
		furnitureItem.useLength = 10;
		furnitureItem.rotation = Direction.down;
		furnitureItem.useAnimation = UseAnimation.listenToMusic;
		furnitureItem.basicEffects.Add(Effect.music, 4);
		furnitureItem.basicEffects.Add(Effect.vintage, 3);
		furnitureItem.usedRadiantEffects.Add(Effect.music, 2);
		furnitureItem.usedRadiantEffects.Add(Effect.vintage, 1);

		AddNewFurnitureItem(FurnitureName.stove);
		furnitureItem.type = FurnitureType._core;
		furnitureItem.roomTypes.Add(RoomType.kitchen);
		furnitureItem.flatWideEffect = true;
		furnitureItem.size = 1;
		furnitureItem.price = 200;
		furnitureItem.useLength = 10;
		furnitureItem.rotation = Direction.up;
		furnitureItem.useAnimation = UseAnimation.cook;
		furnitureItem.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down });
		furnitureItem.basicEffects.Add(Effect.food, 4);
		furnitureItem.basicEffects.Add(Effect.cozy, 2);
		furnitureItem.usedRadiantEffects.Add(Effect.cozy, 2);

		AddNewFurnitureItem(FurnitureName.fridge);
		furnitureItem.type = FurnitureType._core;
		furnitureItem.roomTypes.Add(RoomType.kitchen);
		furnitureItem.flatWideEffect = false;
		furnitureItem.size = 1;
		furnitureItem.price = 200;
		furnitureItem.useLength = 10;
		furnitureItem.rotation = Direction.right;
		furnitureItem.useAnimation = UseAnimation.idle;
		furnitureItem.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down });
		furnitureItem.basicEffects.Add(Effect.food, 4);
		furnitureItem.basicEffects.Add(Effect.cozy, 2);

		AddNewFurnitureItem(FurnitureName.counterTop);
		furnitureItem.type = FurnitureType._core;
		furnitureItem.roomTypes.Add(RoomType.kitchen);
		furnitureItem.flatWideEffect = false;
		furnitureItem.size = 1;
		furnitureItem.price = 100;
		furnitureItem.useLength = 10;
		furnitureItem.rotation = Direction.down;
		furnitureItem.useAnimation = UseAnimation.idle;
		furnitureItem.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down });
		furnitureItem.basicEffects.Add(Effect.food, 4);
		furnitureItem.basicEffects.Add(Effect.cozy, 2);



		AddNewFurnitureItem(FurnitureName.rockingChair);
		furnitureItem.debugItem = true;
		furnitureItem.type = FurnitureType._object;
		furnitureItem.group = FurnitureGroup.chair;
		furnitureItem.roomTypes.Add(RoomType.livingroom);
		furnitureItem.flatWideEffect = false;
		furnitureItem.size = 1;
		furnitureItem.price = 100;
		furnitureItem.useLength = 10;
		furnitureItem.ontopUsePosition = true;
		furnitureItem.rotation = Direction.left;
		furnitureItem.useAnimation = UseAnimation.sit;
		furnitureItem.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down });
		furnitureItem.basicEffects.Add(Effect.comfort, 4);
		furnitureItem.basicEffects.Add(Effect.cozy, 2);

		AddNewFurnitureItem(FurnitureName.rug);
		furnitureItem.type = FurnitureType._decor;
		furnitureItem.roomTypes.Add(RoomType.livingroom);
		furnitureItem.roomTypes.Add(RoomType.bedroom);
		furnitureItem.roomTypes.Add(RoomType.kitchen);
		furnitureItem.roomTypes.Add(RoomType.bathroom);
		furnitureItem.floorObject = true;
		furnitureItem.flatWideEffect = false;
		furnitureItem.size = 1;
		furnitureItem.price = 100;
		furnitureItem.useLength = 10;
		furnitureItem.rotation = Direction.down;
		furnitureItem.basicEffects.Add(Effect.comfort, 4);
		furnitureItem.basicEffects.Add(Effect.cozy, 2);

		AddNewFurnitureItem(FurnitureName.smallSideTable);
		furnitureItem.type = FurnitureType._decor;
		furnitureItem.roomTypes.Add(RoomType.livingroom);
		furnitureItem.roomTypes.Add(RoomType.bedroom);
		furnitureItem.roomTypes.Add(RoomType.bathroom);
		furnitureItem.size = 1;
		furnitureItem.price = 50;
		furnitureItem.useLength = 10;
		furnitureItem.rotation = Direction.down;
		furnitureItem.basicEffects.Add(Effect.comfort, 4);
		furnitureItem.basicEffects.Add(Effect.cozy, 2);

		AddNewFurnitureItem(FurnitureName.tv);
		furnitureItem.type = FurnitureType._object;
		furnitureItem.useFromGroup = FurnitureGroup.chair;
		furnitureItem.roomTypes.Add(RoomType.livingroom);
		furnitureItem.roomTypes.Add(RoomType.bedroom);
		furnitureItem.flatWideEffect = false;
		furnitureItem.size = 1;
		furnitureItem.price = 100;
		furnitureItem.useLength = 10;
		furnitureItem.rotation = Direction.up;
		furnitureItem.useAnimation = UseAnimation.sit;
		furnitureItem.basicEffects.Add(Effect.entertainment, 5);
		furnitureItem.usedRadiantEffects.Add(Effect.entertainment, 2);

		AddNewFurnitureItem(FurnitureName.yarnBasket);
		furnitureItem.type = FurnitureType._object;
		furnitureItem.useFromGroup = FurnitureGroup.chair;
		furnitureItem.roomTypes.Add(RoomType.livingroom);
		furnitureItem.roomTypes.Add(RoomType.bedroom);
		furnitureItem.flatWideEffect = false;
		furnitureItem.size = 1;
		furnitureItem.price = 100;
		furnitureItem.useLength = 10;
		furnitureItem.rotation = Direction.down;
		furnitureItem.useAnimation = UseAnimation.sitAndKnitt;
		furnitureItem.basicEffects.Add(Effect.comfort, 4);
		furnitureItem.basicEffects.Add(Effect.cozy, 2);

		AddNewFurnitureItem(FurnitureName.stonePainting);
		furnitureItem.debugItem = true;
		furnitureItem.type = FurnitureType._object;
		furnitureItem.roomTypes.Add(RoomType.livingroom);
		furnitureItem.roomTypes.Add(RoomType.bedroom);
		furnitureItem.flatWideEffect = false;
		furnitureItem.size = 1;
		furnitureItem.price = 100;
		furnitureItem.useLength = 10;
		furnitureItem.rotation = Direction.down;
		furnitureItem.useAnimation = UseAnimation.idle;
		furnitureItem.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down });
		furnitureItem.basicEffects.Add(Effect.comfort, 4);
		furnitureItem.basicEffects.Add(Effect.cozy, 2);

		AddNewFurnitureItem(FurnitureName.roarRock);
		furnitureItem.debugItem = true;
		furnitureItem.type = FurnitureType._object;
		furnitureItem.roomTypes.Add(RoomType.livingroom);
		furnitureItem.roomTypes.Add(RoomType.bedroom);
		furnitureItem.flatWideEffect = false;
		furnitureItem.size = 1;
		furnitureItem.price = 100;
		furnitureItem.useLength = 10;
		furnitureItem.rotation = Direction.down;
		furnitureItem.ontopUsePosition = true;
		furnitureItem.useAnimation = UseAnimation.roar;
		furnitureItem.accessPositions.AddRange(new AccessPosition[] { AccessPosition.down, AccessPosition.up, AccessPosition.left, AccessPosition.right });
		furnitureItem.basicEffects.Add(Effect.comfort, 4);
		furnitureItem.basicEffects.Add(Effect.cozy, 2);

		if (Settings.furnitureUnlocked)
			mainController.startingAmountOfUnlockedFurniture = Main.objectsList.Count();//2;
		else
			mainController.startingAmountOfUnlockedFurniture = 2;

		for (int i = 0; i < mainController.startingAmountOfUnlockedFurniture; i++)
		{
			mainController.ChooseNewFurnitureToUnlock();
		}

	}
}
