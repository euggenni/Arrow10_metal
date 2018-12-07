using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// ������� ������� ��������� ������� �������
/// </summary>
/// <param name="player">�����, � �������� ������� �����</param>
public delegate void OnTimeOver(NetworkPlayer player);

public class StopwatchTimer : MonoBehaviour
{
    private bool _active;

    public DateTime StartTime;
    // ����� ��� �������
    private float _time;

    /// <summary>
    /// ����� �����������
    /// </summary>
    public DateTime TimeLeft;

    /// <summary>
    /// �����, ��������� � ���� ��������
    /// </summary>
    public NetworkPlayer Player;

    /// <summary>  ������� ����� ������� �������  </summary>
    public event OnTimeOver OnTimeOver;

    private void CallTimeOver()
    {
        OnTimeOver handler = OnTimeOver;
        if (handler != null) handler(Player);
    }

    void Awake()
    {
        enabled = false;
    }

    void Update()
    {
        if (TimeLeft.Minute == 0 && TimeLeft.Second == 0)
        {
            enabled = false;

            // ������� ������� ����� �������
            CallTimeOver();
        }

        TimeLeft = TimeLeft.AddSeconds(-Time.deltaTime);
    }

    public void SetTimer(float seconds)
    {
        _time = seconds;
        StartTime = StartTime.AddSeconds(seconds);
        TimeLeft = new DateTime(0, 0);
        TimeLeft = TimeLeft.AddSeconds(seconds + 1);
    }
    

    /// <summary>
    /// ��������� ������
    /// </summary>
    public void Activate()
    {
        enabled = true;
    }

    /// <summary>
    /// ���������� ������
    /// </summary>
    public void Deactivate()
    {
        enabled = false;
    }
}
