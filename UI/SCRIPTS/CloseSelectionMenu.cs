using Godot;
using System;
using static Asriela.BasicFunctions;
public partial class CloseSelectionMenu : Button
{

    void OnPressed() 
    {
     
            Destroy(Main.SelectionMenuOpen);
        Main.SelectionMenuOpen = null;
    }

}
