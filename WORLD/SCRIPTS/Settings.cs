using Godot;
using System;
using System.Collections.Generic;
using static Asriela.BasicFunctions;

public partial class Settings : Node
{
    public static bool autoSetupStart = Toggle(0);
    public static bool enableDebugItems = Toggle(1);
    public static bool furnitureUnlocked =      Toggle(1);
    public static bool charactersUnlocked =     Toggle(1);
    public static bool infinateMoney      =     Toggle(1);
    public static bool debugHotkeys_RoomQuickSetup = Toggle(1);
    public static bool debugHotkeys_DesireControls = Toggle(1);

    public static bool objectsRoomTypeLabel=    Toggle(0);
    public static bool characterLabelHasTargetName = Toggle(0);
    public static bool characterLabelHasHappinessValue = Toggle(1);
    public static bool characterLabelHasDesiresValues = Toggle(1);
    public static bool characterLabelHasRelationshipValues = Toggle(1);
    public static bool objectsLabelHasCoOrdinates = Toggle(0);

    public static bool showCharacterStateSquare = Toggle(1);
    public static bool exitButton = Toggle(1);


    public static bool playerMode = Toggle(0);

    public static float tweak_desireVSobjectValue = 1;//percentage to devide desire value to put it in the 0-10 range instead of 0-100 range
    public static float tweak_negativeFlatEffectsBoost = 3;
    public static float tweak_socializingDistance = 10*7;
    public static float tweak_interactionDistance = 10;
    public static float tweak_minimumRelationshipToWantToInteract =-10;
    public static float tweak_socialInteractionDampner = 5;
    public static float tweak_socialRejectionActionLength=3;
    public static float tweak_rejectionNegativeImpact=20;

    public static Color stateColorBeingSocializedWithAndNotUsingFurniture = ColorYellow;
    public static Color stateColorInactive = ColorGrey;
    public static Color stateColorMovingToFurniture = ColorBlue;
    public static Color stateColorMovingToSocialTarget = ColorPurple;
    public static Color stateColorSocializing = ColorPink;
    public static Color stateColorUsingFurniture = ColorLime;
    public static Color stateColorUpset = ColorMaroon;
    public static Color stateColorFind = ColorRed;
    public static Color stateColorArrive = ColorGreen;
    public static Color stateColorNothing = ColorBlack;
    void Start()
    {
        if (infinateMoney)
        {
            Main.Money = 99999;
        }

        if(playerMode)
        {
            enableDebugItems = Toggle(0);
            autoSetupStart = Toggle(0);
            exitButton = Toggle(0);
            characterLabelHasTargetName = Toggle(0);
            characterLabelHasHappinessValue = Toggle(0);
            characterLabelHasRelationshipValues = Toggle(0);
            objectsRoomTypeLabel = Toggle(0);
            infinateMoney = Toggle(0);
            charactersUnlocked = Toggle(0);
            furnitureUnlocked = Toggle(0);
            showCharacterStateSquare= Toggle(0);
            characterLabelHasDesiresValues= Toggle(0);
            objectsLabelHasCoOrdinates = Toggle(0);
            debugHotkeys_RoomQuickSetup= Toggle(0);
            debugHotkeys_DesireControls = Toggle(0);
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
