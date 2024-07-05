using Godot;
using System;

public partial class CantClick : ColorRect
{

    void MouseHasEntered()
    {
        Main.canPlace = false;
    }

    void MouseHasExit()
    {
        Main.canPlace = true;
    }
}
