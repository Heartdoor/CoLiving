using Godot;
using System.Collections.Generic;
using static Asriela.BasicFunctions;
public partial class DebugHotkeys : Node
{
    Effect chosenEffect = Effect.social;
    List<Effect> listOfEffects = new List<Effect>();
    int listIndex = 0;
    float rate = 10;
    public Main myMain;

    bool action1 = false;
    bool action2 = false;

    public void Start()
    {
        listOfEffects.Add(Effect.social);
        listOfEffects.Add(Effect.happiness);
        listOfEffects.Add(Effect.anger);

        if (Settings.autoSetupStart)
        {
            action1 = true;
            action2 = true;

        }
    }
    public void Run()
    {
        if (Settings.debugHotkeys_RoomQuickSetup)
            QuickLevelSetupHotKeys();
        if (Settings.debugHotkeys_DesireControls)
            DesireManipulationHotKeys();
        Main.DebugEffectOnMouse = $"{chosenEffect}";
    }

    void QuickLevelSetupHotKeys()
    {

        FurnitureItem furniture;
        Character character;
        CharacterController newCharacter;
        if (KeyPressed("debug_3"))
        {
            action1 = false;
            furniture = Main.GetObjectFromType(FurnitureName.couch);
            Main.RoomNumberMouseIsIn = 1;
            myMain.PlaceObjects(ref furniture, true, new Vector2(265, 255));
            furniture = Main.GetObjectFromType(FurnitureName.fridge);
            Main.RoomNumberMouseIsIn = 2;
            myMain.PlaceObjects(ref furniture, true, new Vector2(45, 335));

        }

        if (KeyPressed("debug_1") || action1)
        {
            action1 = false;

            Main.RoomNumberMouseIsIn = 1;
            furniture = Main.GetObjectFromType(FurnitureName.couch);
            myMain.PlaceObjects(ref furniture, true, new Vector2(265, 255));

            furniture = Main.GetObjectFromType(FurnitureName.recordPlayer);
            myMain.PlaceObjects(ref furniture, true, new Vector2(205, 265));

            furniture = Main.GetObjectFromType(FurnitureName.yarnBasket);
            myMain.PlaceObjects(ref furniture, true, new Vector2(315, 265));
        }

        if (KeyPressed("debug_2") || action2)
        {
            action2 = false;
            character = Main.GetCharacterFromType(CharacterType.punkRocker);
            newCharacter = myMain.PlaceCharacter(ref character, true, new Vector2(115, 205));
            newCharacter.characterData.desires[Effect.social] = 90;
            var tempOtherCharacter = newCharacter;

            character = Main.GetCharacterFromType(CharacterType.granny);
            newCharacter = myMain.PlaceCharacter(ref character, true, new Vector2(135, 355));
            newCharacter.characterData.relationshipsList[tempOtherCharacter].strength[RelationshipType.friendship] = 40;


        }

    }

    void DesireManipulationHotKeys()
    {
        if (KeyPressed("debug_increase"))
        {


            if (Main.SelectedCharacter.characterData.desires.ContainsKey(chosenEffect))
                Main.SelectedCharacter.characterData.desires[chosenEffect] += rate;
            else
                Main.SelectedCharacter.characterData.desires.Add(chosenEffect, 0);
        }

        if (KeyPressed("debug_decrease"))
        {
            if (Main.SelectedCharacter.characterData.desires.ContainsKey(chosenEffect))
                Main.SelectedCharacter.characterData.desires[chosenEffect] -= rate;
            else
                Main.SelectedCharacter.characterData.desires.Add(chosenEffect, 0);
        }

        if (KeyPressed("debug_scrollUp"))
        {

            if (listIndex > listOfEffects.Count - 1) listIndex = 0; else listIndex++;
            chosenEffect = listOfEffects[listIndex];

        }

        if (KeyPressed("debug_scrollDown"))
        {
            if (listIndex < 0) listIndex = listOfEffects.Count - 1; else listIndex--;
            chosenEffect = listOfEffects[listIndex];
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
