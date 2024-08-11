using Godot;
using System;
using System.Collections.Generic;
using static Asriela.BasicFunctions;

public partial class Settings : Node
{

    public static bool furnitureUnlocked =  Toggle(1);
    public static bool charactersUnlocked = Toggle(1);
    public static bool infinateMoney      = Toggle(1);


    void Start()
    {
        if (infinateMoney)
        {
            Main.Money = 99999;
        }
    }
    #region OLD
    public override void _Ready()
    {
        Start();
    }


    #endregion
}
