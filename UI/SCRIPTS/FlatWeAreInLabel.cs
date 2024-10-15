using Godot;

public partial class FlatWeAreInLabel : Label
{
    void Run()
    {

        Text = $"Room: {Main.RoomNumberMouseIsIn}";
    }


    public override void _Process(double delta)
    {
        Run();
    }
}
