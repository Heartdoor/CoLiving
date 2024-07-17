using Godot;
using System;
using System.Linq;

public partial class FlatHappinessLabel : Label
{
    void Run()
    {
        if(Main.TestGameMode != Main.testGameMode.flowingMoney)
        Text = $"Flat Happiness: {Main.flatsList[Main.FlatNumberMouseIsIn].charactersInFlat.Sum(node=>node.happiness)}   Money:{Main.Money}";
        else
        Text = $"Money: {(int)Main.Money}";
    }


    public override void _Process(double delta)
    {
        Run();
    }
}
