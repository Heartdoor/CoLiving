using Godot;

public partial class ItemToSelect : Button
{

	public FurnitureItem myFurnitureItem = null;
	public Character MyCharacter = null;
	public int listIndex;



	void OnPressed()
	{

		if (myFurnitureItem != null)
		{
			if (Main.Money >= myFurnitureItem.price)
			{
				Main.HeldFurnitureItem = myFurnitureItem;
				// if(Main.HeldFurnitureItem.isGroupLeader != FurnitureGroup.none)
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
