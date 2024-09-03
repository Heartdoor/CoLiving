using Godot;
using System;
using System.Collections.Generic;
using static Asriela.BasicFunctions;
public partial class DebugHotkeys : Node
{
    Effect chosenEffect = Effect.social;
    List<Effect> listOfEffects  = new List<Effect>(); 
    int listIndex =0;
    float rate =10;
    public Main myMain;

    public void Start()
    {
        listOfEffects.Add(Effect.social);
        listOfEffects.Add(Effect.happiness);
        listOfEffects.Add(Effect.anger);
    }
    public void Run()
    {
        if(Settings.debugHotkeys_RoomQuickSetup)
        QuickLevelSetupHotKeys();
        if (Settings.debugHotkeys_DesireControls)
            DesireManipulationHotKeys();
        Main.DebugEffectOnMouse= $"{ chosenEffect}";
    }

    void QuickLevelSetupHotKeys()
    {
        Main.Object furniture;
        Main.Character character;
        if (KeyPressed("debug_1"))
        {
     
            furniture = Main.GetObjectFromType(FurnitureName.couch);
            Main.RoomNumberMouseIsIn = 1;
            myMain.PlaceObjects(ref furniture, true, new Vector2(265, 255));
            furniture = Main.GetObjectFromType(FurnitureName.fridge);
            Main.RoomNumberMouseIsIn = 2;
            myMain.PlaceObjects(ref furniture, true, new Vector2(45, 335));

        }

        if (KeyPressed("debug_2"))
        {
            character = Main.GetCharacterFromType(CharacterType.punkRocker);
            myMain.PlaceCharacter(ref character, true, new Vector2(115, 205));
            character = Main.GetCharacterFromType(CharacterType.granny);
            myMain.PlaceCharacter(ref character, true, new Vector2(135, 355));


        }
        if (KeyPressed("debug_3"))
        {

        }
    }

    void DesireManipulationHotKeys()
    {
        if (KeyPressed("debug_increase"))
        {


            if(Main.SelectedCharacter.characterData.desires.ContainsKey(chosenEffect))
            Main.SelectedCharacter.characterData.desires[chosenEffect]+=rate; 
            else
                Main.SelectedCharacter.characterData.desires.Add(chosenEffect,0);
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

            if(listIndex> listOfEffects.Count-1) listIndex=0; else listIndex++;
            chosenEffect = listOfEffects[listIndex];

        }

        if (KeyPressed("debug_scrollDown"))
        {
            if (listIndex <0) listIndex = listOfEffects.Count - 1; else listIndex--;
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
