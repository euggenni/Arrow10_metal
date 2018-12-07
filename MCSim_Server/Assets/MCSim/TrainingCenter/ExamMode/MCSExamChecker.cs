using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MilitaryCombatSimulator;
using System;

public delegate void onExamComplete(ReportExam report);

public class MCSExamChecker: MCSTrainingChecker
{
    public event onExamComplete OnExamCompleted;

    public StopwatchTimer Timer;   

    private int sec = 30;

    /// <summary>
    /// Результаты экзамена
    /// </summary>
    private ReportExam Results;
    TimeSpan ts;
    public  static int remainingAttempts = 3;

    public DateTime Time;
    public DateTime Gap;
    public int error;

    public OrderSubtask CurrentCommand
    {
        get
        {
            try
            {
                return taskCommands[0];
            }
            catch { return null; }
        }
    }
 
    public override void CallOnTaskAttempt(bool succes, OrderSubtask required, string discription) {
        base.CallOnTaskAttempt(succes, required, discription);
        if (!succes)
        {
            if (remainingAttempts > 0)
            {
                remainingAttempts--;
                Debug.Log("ERROR" + remainingAttempts);
            }
            else
            {
                Weaponry.Core[Order.PerformerName].UnsubscribeFromOnControlChanged(OnControlChanged);
            }
        }
        else 
        {
            // действие если правильно выполнена промежуточная команда
          
        }
        Debug.LogError("____DO");
        // после выполнения экзамена 
        
    }
	// Use this for initialization
	void Awake () {
        Timer = gameObject.AddComponent<StopwatchTimer>();
        Timer.OnTimeOver += new OnTimeOver(Timer_OnTimeOver);
        Timer.SetTimer(sec);
        
        Timer.Activate();
        Time = DateTime.Now;
	}

    void Timer_OnTimeOver(NetworkPlayer player)
    {
        Weaponry.Core[Order.PerformerName].UnsubscribeFromOnControlChanged(OnControlChanged);
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
