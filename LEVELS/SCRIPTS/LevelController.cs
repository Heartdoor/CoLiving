using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using static Asriela.BasicFunctions;
using static CharacterEffectors;
public partial class LevelController : Node
{
    public static LevelName Level {get; set; } 
    public static Dictionary<CharacterType, CharacterEffectors.Goal> CharactersInitialGoals = new Dictionary<CharacterType, CharacterEffectors.Goal>();
    public void Start()
    {

        SetupCurrentLevel();

    }

    public void Run()
    {

    }


    void SetupCurrentLevel()
    {
        switch (Level)
        {
            case LevelName.cozyPunk:

                CharactersInitialGoals.Add(CharacterType.granny, new UseItemGoal(FurnitureName.fridge,3));
              
                BuildingController.AddNewBuilding(0, ColorGrey);
                var firstflat = BuildingController.AddNewBuilding(1, ColorBlue,
                    new BuildingController.RoomItem(1, RoomType.none),
                    new BuildingController.RoomItem(2, RoomType.none),
                    new BuildingController.RoomItem(3, RoomType.none),
                    new BuildingController.RoomItem(4, RoomType.none),
                    new BuildingController.RoomItem(4, RoomType.none));
                break;
        }
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
