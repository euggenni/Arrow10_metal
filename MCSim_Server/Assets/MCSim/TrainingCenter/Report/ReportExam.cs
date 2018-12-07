using UnityEngine;
using System.Collections;

public class ReportExam : MonoBehaviour {

    /// <summary>
    /// Количество неправильных попыток
    /// </summary>
    public int remainingAtt;

    /// <summary>
    /// Время начала  
    /// </summary>
    public string startTime;

    /// <summary>
    /// Продолжительность экзамена
    /// </summary>
    public string durationTime;
    /// <summary>
    /// Имя сдающего 
    /// </summary>
    public string nameShuffler;

    /// <summary>
    /// Название техники 
    /// </summary>
    public string weanpory;

    /// <summary>
    /// Приказ на выполнение 
    /// </summary>
    public string orders;


    public void ReportGeneration(int remA, string  strT, string  durT, string nameS, string weaR,string ord) 
    {   remainingAtt = remA;
        startTime = strT;
        durationTime = durT;
        nameShuffler = nameS;
        weanpory = weaR;
        orders = ord;
    }
}
