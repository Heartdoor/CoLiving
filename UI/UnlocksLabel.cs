using Godot;
using System;
using static Asriela.BasicFunctions;

public partial class UnlocksLabel : Label
{
    Alarms alarm = new Alarms();
    bool started = false;
    float fadeSpeed = 0;
    void Start() {

        
        SetAlpha(this, 0);
    }
    void Run()
    {
        alarm.Run();
        if (alarm.Ended(TimerType.initialAction))
        {
            SetAlpha(this, 1);
       
            started = true;
        }
        if (started)
        {
            fadeSpeed -= 0.00005f;
            ChangeAlpha(this, fadeSpeed);
     
           


        }



    }

    public void NewUnlock(string lastUnlock)
    {
        Text = $"UNLOCKED : {lastUnlock}";
        SetAlpha(this, 1);
        started = false;
        alarm.Start(TimerType.initialAction, 3, false, 0);
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
