using Godot;
using System;
using static Asriela.BasicFunctions;

public partial class AccessNode : Area2D
{
    public FurnitureController myObject;
    /*
    private void OnBodyEntered(Node body)
    {
        Log("triggered area", LogType.weird);
        if (body is CharacterBody2D)
        {
            Log("entered area", LogType.weird);

            var character = (Characters)body;

            if (character.accessNode == this)
            {
                Log("CHAR ENTERED", LogType.weird);
                character.ReachTarget();

               

            }


        }
        Log("BODY ENTERED", LogType.game);
    }*/
}
