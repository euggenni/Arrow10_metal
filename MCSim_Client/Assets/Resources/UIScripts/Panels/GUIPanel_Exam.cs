using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using MilitaryCombatSimulator;


public class GUIPanel_Exam : GUIPanel
{
  //  public ReportExam repot;
    private MCSPlayer _player;
    
    string duraTime;
    
    protected int conmmand = 0;
    protected int remainingAttemptsOpen = 3;
    protected int remainingAttempts = 3;
    public static  int errors;
    private MCSExamChecker _checker;
    private string name;

    void Awake()
    {
        MCSTrainingCenter.OnCheckerDisposed += OnCheckerDisposed;
    }

    public void OnCheckerDisposed(MCSTrainingChecker checker)
    {
        if (checker == _checker)
            Close();
    }

    public void OnCurrentControlChanged(PanelControl control)
    {
        if (control == null) {
            _checker.OnExamCompleted -= OnExamCompleted;        
        }     
    }

	void OnDestroy()
	{
		base.OnDestroy();
		_checker.OnExamCompleted -= OnExamCompleted;
	}

  
    public void OnTaskAttempt(bool success, OrderSubtask required, string discription)
    {
        if (required != null)
        {
            if (success)
            {
                conmmand++;
                SetGlow(GetControl<UISlicedSprite>("Exam"), Color.green);
                GetControl<UILabel>("label").text = conmmand + "/" + name;
            }
            else
            {
                if (remainingAttempts != 0)
                {
                    remainingAttempts--;
                    errors++;
                    SetGlow(GetControl<UISlicedSprite>("Exam"), Color.red);
                    GetControl<UILabel>("labelTwo").text = remainingAttempts + "/" + remainingAttemptsOpen;

                }
                else
                {

                    SetGlow(GetControl<UISlicedSprite>("Exam"), Color.red);
                    foreachs();
                    GetControl<UILabel>("Finish").text = "Экзамен не сдан!";


						  var pan = UICenter.Panels.Instantiate("ExamFinish");
						  pan.SendMessage("Set", "Экзамен не сдан");
						  pan.Show();
                    //st.Stop();
                }
            }
        }
        else
        {

            foreachs();
            //st.Stop();
            //GetControl<UILabel>("label").text = st.Elapsed.Minutes.ToString("00") + ":" + st.Elapsed.Seconds.ToString("00");
            //duraTime = st.Elapsed.Minutes.ToString("00") + ":" + st.Elapsed.Seconds.ToString("00");
           
           // Reporting(remainingAttempts, time, duraTime, "_player.Account.FirstName", "df");


        }
        //demidovaii@sstu.ru
    }

    private void foreachs()
    {
        foreach (GameObject g in Elements)
        {
            // Debug.Log(g.name);
            var label = g.GetComponent<UILabel>();
            if (label != null)
            {
                label.enabled = false;
                //Debug.Log(label.name);
            }



        }
    }
    public void SetCheckerExam(MCSExamChecker checker)
    {
        _checker = checker;
        _checker.Timer.OnTimeOver += new OnTimeOver(Timer_OnTimeOver);

        _checker.OnExamCompleted += OnExamCompleted;

        checker.SubscribeOnTaskAttempt(OnTaskAttempt, OnCurrentControlChanged);
    }

    void OnExamCompleted(ReportExam report)
    {
        UICenter.Panels.CloseGroup("Training");
        var pan = UICenter.Panels.Instantiate("ExamFinish");
        pan.SendMessage("Set", report);
        pan.Show();
        
    }


    void Timer_OnTimeOver(NetworkPlayer player)
    {
        foreachs();
        GetControl<UILabel>("Finish").text = "Экзамен не сдан";
    }

    public void SetPlayerExam(MCSPlayer player)
    {
        _player = player;

    }

    public void SetOrderExam(MCSTrainingOrder order)
    {
        name = order.Commands.Count.ToString();
       
        GetControl<UILabel>("labelTree").text = order.OrderName;
        GetControl<UILabel>("label").text = conmmand + "/" + name;
    }
    void SetGlow(UISlicedSprite sprite, Color clr)
    {
        var tc = sprite.ForceComponent<TweenColor>();
        tc.from = sprite.color;
        tc.to = clr;
        tc.duration = 0.5f;

        tc.onFinished = delegate
        {
            tc.enabled = false;
            Destroy(tc);

            var tc2 = sprite.gameObject.AddComponent<TweenColor>();
            tc2.from = tc2.color;
            tc2.to = Color.white;
            tc2.duration = 1.5f;
            tc2.onFinished = Destroy;
        };
    }

	private UILabel _timer;
    // Use this for initialization
    void Start()
	{
		_timer = GetControl<UILabel>("Timer");
        //st.Start();
        //TimeToExam = new TimeSpan(h,m,s);        
	    errors = 0;
	    remainingAttempts = 3;
	}



    // Update is called once per frame
    void Update()
    {
        if (remainingAttempts != 0)
        {
            if (_checker.Timer.enabled)
            {
                //TimeSpan res = TimeToExam - st.Elapsed;
                //  UnityEngine.Debug.Log(res.Seconds.ToString());
                if (_checker.Timer.TimeLeft.Second >= 0)
                {
                    _timer.text = _checker.Timer.TimeLeft.Minute.ToString("00") + ":" + _checker.Timer.TimeLeft.Second.ToString("00");
                }
                else
                {
                    foreachs();
                    GetControl<UILabel>("Finish").text = "Экзамен не сдан";
                }
            }
            //else { GetControl<UILabel>("Timer").text = ""; }
        }
        else
        {
            foreach (GameObject g in Elements)
            {
                var label = g.GetComponent<UILabel>();
                if (label != null)
                {
                    label.text = "";

                }
            }
				_timer.text = "";
            GetControl<UILabel>("Finish").text = "Экзамен не сдан";
            //st.Stop();
        }

    }
  /*  protected void Reporting(int renAtt, string startTimes, string durTime, string name, string weap)
    {
        repot = new ReportExam();
        repot.ReportGeneration(renAtt, startTimes, durTime, name, weap);
        UnityEngine.Debug.Log(renAtt + " " + startTimes + " " + durTime + " " + name + " " + weap);
    }*/
}
