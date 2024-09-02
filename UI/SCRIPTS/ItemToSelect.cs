using Godot;
using System;
using System.Linq;
using static Asriela.BasicFunctions;

public partial class ItemToSelect : Button
{
    public Main.Object MyObject=null;
    public Main.Character MyCharacter=null;
    public int listIndex;


    void OnPressed()
    {

        if (MyObject != null)
        {

            
            if(Main.Money>= MyObject.price)
            {
                Main.HeldObject = MyObject;
               // if(Main.HeldObject.isGroupLeader != FurnitureGroup.none)
                //{
                //    var mapClass=(GameTileGrid)Main.MyTileMap;
                 //   mapClass.FillFlatWithPlaceableArea();
               // }
                
            }
        
         
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
