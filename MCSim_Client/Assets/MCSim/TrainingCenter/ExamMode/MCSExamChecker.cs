using System;
using MilitaryCombatSimulator;
using UnityEngine;

public delegate void onExamComplete(ReportExam report);

public class MCSExamChecker : MCSTrainingChecker {
    public event onExamComplete OnExamCompleted;

    public StopwatchTimer Timer;

    private int sec = 300;

    /// <summary>
    /// Результаты экзамена
    /// </summary>
    private ReportExam Results;

    TimeSpan ts;
    public static int remainingAttempts = 3;

    public DateTime Time;
    public DateTime Gap;
    public int error;

    public OrderSubtask CurrentCommand {
        get {
            try {
                return taskCommands[0];
            } catch {
                return null;
            }
        }
    }

    public override void CallOnTaskAttempt(bool succes, OrderSubtask required, string discription) {
        base.CallOnTaskAttempt(succes, required, discription);
        if (!succes) {
            if (remainingAttempts > 0) {
                remainingAttempts--;
            }

            if (remainingAttempts == 0) {
                Debug.Log("ПОЗОООООООООООР");

                Weaponry.Core[Order.PerformerName].UnsubscribeFromOnControlChanged(OnControlChanged);
                MCSTrainingCenter.InterruptCheckers(MCSPlayer.Me, true);
            }
        } else {
            // действие если правильно выполнена промежуточная команда
        }

        // после выполнения экзамена 
        if (taskCommands.Count == 0) {
            Timer.Deactivate();

            error = GUIPanel_Exam.errors;
            ts = DateTime.Now - Time;
            Results = new ReportExam();

            Results.ReportGeneration(error, Time.ToShortTimeString(),
                ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00"),
                Player.Account.FirstName.ToString(), Weaponry.Name.ToString(), Order.OrderName);

            if (OnExamCompleted != null)
                OnExamCompleted(Results);

            MCSTrainingCenter.InterruptCheckers(MCSPlayer.Me, true);

            //CallOnTaskAttemptFinish("Ошибок " + error, " Время " + Time.ToShortTimeString() + " " + ts.Hours.ToString() + ":" + ts.Minutes.ToString() + ":" + ts.Seconds.ToString(), " Орудие " + Weaponry.Name.ToString(), " Приказ " + trOrder.OrderName.ToString());
            //CallOnTaskAttempt(true, null, "Ошибок " + error + " Время " + Time.ToShortTimeString() + " " + ts.Hours.ToString() + ":" + ts.Minutes.ToString() + ":" + ts.Seconds.ToString() + " " + " Орудие " + Weaponry.Name.ToString() + " Приказ " + trOrder.OrderName.ToString());
            // Debug.Log(error + " " + Time.ToShortTimeString()+" "+ ts.Hours.ToString() + ":" + ts.Minutes.ToString() + ":" + ts.Seconds.ToString() + " "  + " " + Weaponry.Name.ToString() + " " + trOrder.OrderName.ToString());
        }
    }

    // Use this for initialization
    void Awake() {
        Timer = gameObject.AddComponent<StopwatchTimer>();
        Timer.OnTimeOver += new OnTimeOver(Timer_OnTimeOver);
        Timer.SetTimer(sec);

        Timer.Activate();
        Time = DateTime.Now;

        remainingAttempts = 3;
    }

    void Timer_OnTimeOver(NetworkPlayer player) {
        Weaponry.Core[Order.PerformerName].UnsubscribeFromOnControlChanged(OnControlChanged);
    }

    // Update is called once per frame
    void OnDestroy() {
        Weaponry.Core[Order.PerformerName].UnsubscribeFromOnControlChanged(OnControlChanged);
    }
}