using Godot;
using System;
using System.Collections.Generic;
using static Asriela.BasicFunctions;

public partial class Settings : Node
{

    public static bool furnitureUnlocked =      Toggle(1);
    public static bool charactersUnlocked =     Toggle(1);
    public static bool infinateMoney      =     Toggle(1);
    public static bool objectsRoomTypeLabel=    Toggle(0);
    public static bool characterLabelHasTargetName = Toggle(0);
    public static bool characterLabelHasHappinessValue = Toggle(1);
    public static bool characterLabelHasDesiresValues = Toggle(1);
    public static bool showCharacterStateSquare = Toggle(1);
    public static bool exitButton = Toggle(1);


    public static bool playerMode = Toggle(0);

    public static float tweak_desireVSobjectValue = 120;//percentage to devide desire value to put it in the 0-10 range instead of 0-100 range

    void Start()
    {
        if (infinateMoney)
        {
            Main.Money = 99999;
        }

        if(playerMode)
        {
            exitButton = Toggle(0);
            characterLabelHasTargetName = Toggle(0);
            characterLabelHasHappinessValue = Toggle(0);
            objectsRoomTypeLabel = Toggle(0);
            infinateMoney = Toggle(0);
            charactersUnlocked = Toggle(0);
            furnitureUnlocked = Toggle(0);
            showCharacterStateSquare= Toggle(0);
            characterLabelHasDesiresValues= Toggle(0);
        }
    }

    void Run()
    {
        if (exitButton && KeyPressed("exitGame"))
            EndGame(this);


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
