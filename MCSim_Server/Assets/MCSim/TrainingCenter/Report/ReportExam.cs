using UnityEngine;
using System.Collections;

public class ReportExam : MonoBehaviour {

    /// <summary>
    /// ���������� ������������ �������
    /// </summary>
    public int remainingAtt;

    /// <summary>
    /// ����� ������  
    /// </summary>
    public string startTime;

    /// <summary>
    /// ����������������� ��������
    /// </summary>
    public string durationTime;
    /// <summary>
    /// ��� �������� 
    /// </summary>
    public string nameShuffler;

    /// <summary>
    /// �������� ������� 
    /// </summary>
    public string weanpory;

    /// <summary>
    /// ������ �� ���������� 
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
