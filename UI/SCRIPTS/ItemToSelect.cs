using Godot;
using System;
using static Asriela.BasicFunctions;

public partial class ItemToSelect : Button
{
    public Main.Object MyObject=null;
    public Main.Character MyCharacter=null;



    void OnPressed()
    {
        Log("HAS COUCH", LogType.game);
        if (MyObject != null)
        {

            

            Main.HeldObject = MyObject;

        }
        else
        if (MyCharacter != null)
        {
            Main.HeldCharacter = MyCharacter;

        }
    }

    #region OLD


    #endregion
}
