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
        
    




        uiContainer =SpawnUI(GetScene("res://UI/SCENES/selection_menu.tscn"), this);
        Main.SelectionMenuOpen = uiContainer;
      uiContainer.Position = new Vector2(120,0);
        var scrollContainer = uiContainer.GetNode<ScrollContainer>("ScrollContainer");
        var gridContainer = scrollContainer.GetNode<GridContainer>("GridContainer");
        switch (buttonType)
        {
            case "characters":
                Main.HoldNothing();
             
                foreach (var item in Main.CharactersAvailableToPlayerList)
                {
                    if(item.debugItem && !Settings.enableDebugItems) continue;
                    var newButton= (Button) SpawnUI(GetScene("res://UI/SCENES/item_to_select.tscn"), gridContainer);
                    
                    newButton.Text = $"{item.name}";
                    newButton.Icon = item.texture;
                    var buttonClass = (ItemToSelect)newButton;
                    
                    buttonClass.MyCharacter = item;
                }
            break;
            case "objects":
                Main.HoldNothing();
             
                foreach (var item in Main.FurnitureUnlockedList)
                {
                    if (item.debugItem && !Settings.enableDebugItems) continue;
                    var newButton = (Button)SpawnUI(GetScene("res://UI/SCENES/item_to_select.tscn"), gridContainer);

                    newButton.Text = $"{item.name}  ${item.price}";
                    newButton.Icon = item.texture;
                    if(Main.Money< item.price)
                    ChangeColorUI(newButton, ColorRed);
                    var buttonClass = (ItemToSelect)newButton;
                    buttonClass.MyObject = item;

                }
                break;
            case "rooms":
                Main.HoldNothing();
              
                foreach (var item in Main.flatsList)
                {
                   
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
