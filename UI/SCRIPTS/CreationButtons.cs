using Godot;
using System;
using System.Data.SqlTypes;
using static Asriela.BasicFunctions;
using static System.Net.Mime.MediaTypeNames;

public partial class CreationButtons : Button
{
    [Export] string buttonType = "";
    Control uiContainer;
    void Run()
    {

    }

    void Start()
    {

    }

    void OnPressed()
    {


        if (Main.SelectionMenuOpen != null)
        {
            Destroy(Main.SelectionMenuOpen);
        }
        
        Log(buttonType, LogType.game);




        uiContainer =SpawnUI(GetScene("res://UI/SCENES/selection_menu.tscn"), this);
        Main.SelectionMenuOpen = uiContainer;
      uiContainer.Position = new Vector2(120,0);
        var scrollContainer = uiContainer.GetNode<ScrollContainer>("ScrollContainer");
        var gridContainer = scrollContainer.GetNode<GridContainer>("GridContainer");
        switch (buttonType)
        {
            case "characters":
                Main.HoldNothing();
                Log("start character fillup", LogType.game);
                foreach (var item in Main.CharactersAvailableToPlayerList)
                {
                    Log("added character", LogType.game);
                    var newButton= (Button) SpawnUI(GetScene("res://UI/SCENES/item_to_select.tscn"), gridContainer);
                    
                    newButton.Text = $"{item.type}";
                    newButton.Icon = item.texture;
                    var buttonClass = (ItemToSelect)newButton;
                    
                    buttonClass.MyCharacter = item;
                }
            break;
            case "objects":
                Main.HoldNothing();
                Log("start objects fillup", LogType.game);
                foreach (var item in Main.FurnitureUnlockedList)
                {
                    Log("added objects", LogType.game);
                    var newButton = (Button)SpawnUI(GetScene("res://UI/SCENES/item_to_select.tscn"), gridContainer);

                    newButton.Text = $"{item.type}  ${item.price}";
                    newButton.Icon = item.texture;
                    if(Main.Money< item.price)
                    ChangeColorUI(newButton, ColorRed);
                    var buttonClass = (ItemToSelect)newButton;
                    buttonClass.MyObject = item;

                }
                break;
            case "rooms":
                Main.HoldNothing();
                Log("start rooms fillup", LogType.game);
                foreach (var item in Main.flatsList)
                {
                    Log("added rooms", LogType.game);
                    var newButton = (Button)SpawnUI(GetScene("res://UI/SCENES/item_to_select.tscn"), gridContainer);

                    newButton.Text = item.number.ToString();
                }
                break;
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
