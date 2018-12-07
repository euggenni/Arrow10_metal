using System;
using UnityEngine;

/// <summary>
/// Делегат события истечения времени таймера
/// </summary>
/// <param name="player">Игрок, у которого истекло время</param>
public delegate void OnTimeOver(NetworkPlayer player);

public class StopwatchTimer : MonoBehaviour {
    private bool _active;

    public DateTime StartTime;

    // Время для таймера
    private float _time;

    /// <summary>
    /// Время секундомера
    /// </summary>
    public DateTime TimeLeft;

    /// <summary>
    /// Игрок, связанный с этим таймером
    /// </summary>
    public NetworkPlayer Player;

    /// <summary>  Событие конца времени таймера  </summary>
    public event OnTimeOver OnTimeOver;

    private void CallTimeOver() {
        OnTimeOver handler = OnTimeOver;
        if (handler != null) handler(Player);
    }

    void Awake() {
        enabled = false;
    }

    void Update() {
        if (TimeLeft.Minute == 0 && TimeLeft.Second == 0) {
            enabled = false;

            // Вызвать событие конца таймера
            CallTimeOver();
        }

        TimeLeft = TimeLeft.AddSeconds(-Time.deltaTime);
    }

    public void SetTimer(float seconds) {
        _time = seconds;
        StartTime = StartTime.AddSeconds(seconds);
        TimeLeft = new DateTime(0, 0);
        TimeLeft = TimeLeft.AddSeconds(seconds + 1);
    }


    /// <summary>
    /// Запустить таймер
    /// </summary>
    public void Activate() {
        enabled = true;
    }

    /// <summary>
    /// Остановить таймер
    /// </summary>
    public void Deactivate() {
        enabled = false;
    }
}