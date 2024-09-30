using Godot;
using System;
using static Asriela.BasicFunctions;
public partial class EffectLabel : Control
{
    Label myLabel;
    Panel panel;
    Alarms alarm = new Alarms();
    bool started = false;
    float fadeSpeed = 0;

    void Start()
    {
        
        myLabel = GetNode<Label>("EffectLabel");
        //panel = GetNode<Panel>("Panel");
        SetAlpha(myLabel, 0);
        //SetAlpha(panel,0);
    }

    void Run()
    {
        alarm.Run();
        if (alarm.Ended(TimerType.initialAction))
        {
            SetAlpha(myLabel, 1);
            //SetAlpha(panel, 1);
            started =true;
        }
        if (started)
        {
            fadeSpeed -= 0.00005f;
            ChangeAlpha(myLabel, fadeSpeed);
            //ChangeAlpha(panel, fadeSpeed);
            GlobalPosition = new Vector2(GlobalPosition.X , GlobalPosition.Y - 0.3f);


        }

        if (GetAlpha(myLabel) < 0) 
            Destroy(this);

    }

    public void SetLabel(Effect effect, bool bad, int index)
    {

        myLabel.Text = ConvertToEmoji(effect);
        if(bad)
        ChangeColorUI(myLabel,ColorRed);
        alarm.Start(TimerType.initialAction, index, false, 0);
        
    }


    #region OLD
    public override void _Ready()
    {
        Start();
    }

    public override void _PhysicsProcess(double delta)
    {

        Run();

    }
    #endregion
}
