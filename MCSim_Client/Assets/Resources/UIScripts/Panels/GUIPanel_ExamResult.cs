using UnityEngine;
using System.Collections;

public class GUIPanel_ExamResult : GUIPanel {

	void Start()
	{
		Show();
	}

	public void Set(string str)
	{
		GetControl<UILabel>("Label").text = str;
	}

    public void Set(ReportExam report)
    {

        GetControl<UILabel>("Label").text = "Ошибки: " + report.remainingAtt;
        GetControl<UILabel>("Label1").text = "Время начала: " + report.startTime;
        GetControl<UILabel>("Label2").text = "Продолжительность:  " + report.durationTime;
        GetControl<UILabel>("Label3").text = "Имя: " + report.nameShuffler;
        GetControl<UILabel>("Label4").text = "Оружие: " + report.weanpory;
        GetControl<UILabel>("Label5").text = "Приказ: " + report.orders;
    }
}
