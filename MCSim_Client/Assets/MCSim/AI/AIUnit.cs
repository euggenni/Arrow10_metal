using System.Collections.Generic;
using UnityEngine;

public abstract class AIUnit : MonoBehaviour {
    /// <summary>
    /// Точки следования
    /// </summary>
    public List<MCSWaypoint> Waypoints = new List<MCSWaypoint>();

    /// <summary>
    /// Указать точку следования
    /// </summary>
    /// <param name="index">ID точки следования</param>
    public void MoveToWaypoint(int index) {
        if (index < 0 || index > Waypoints.Count - 1) {
            Debug.Log("Invalid waypoint index: " + index);
            return;
        }

        CurrentWaypoint = index;
    }

    /// <summary>
    /// Указать точку следования
    /// </summary>
    /// <param name="wpt">Точка следования</param>
    public void MoveToWaypoint(MCSWaypoint wpt) {
        int id = -1;
        if ((id = Waypoints.IndexOf(wpt)) != -1) MoveToWaypoint(id);

        // Если нет текущей точки следования, добавляем ее 
        if (CurrentWaypoint == -1) CurrentWaypoint = 0;

        Waypoints.Insert(CurrentWaypoint, wpt); // Добавляем точку
        MoveToWaypoint(CurrentWaypoint); // Движемся к ней
    }

    /// <summary>
    /// Добавить дальнейшие точки следования после текущей
    /// </summary>
    /// <param name="wpt">Точки следования</param>
    public void AddWaypointsBeforeCurrent(params MCSWaypoint[] wpt) {
        if (CurrentWaypoint == -1) CurrentWaypoint = 0;
        Waypoints.InsertRange(CurrentWaypoint, wpt);
    }

    /// <summary>
    /// Добавить дальнейшие точки следования после текущей
    /// </summary>
    /// <param name="wpt">Точки следования</param>
    public void AddWaypointsAfterCurrent(params MCSWaypoint[] wpt) {
        if (CurrentWaypoint == -1) CurrentWaypoint = 0;
        Waypoints.InsertRange(CurrentWaypoint + 1, wpt);
    }

    /// <summary>
    /// Продолжить следование к текущей точке
    /// </summary>
    public void Resume() {
        MoveToWaypoint(CurrentWaypoint);
    }

    private int _currentWaypoint = -1;

    /// <summary>
    /// ID текущей точки следования
    /// </summary>
    public int CurrentWaypoint {
        get { return _currentWaypoint; }
        private set { _currentWaypoint = value; }
    }

    /// <summary>
    /// Возвращает или задает точку для следования
    /// </summary>
    private int NextWaypoint {
        get {
            if (CurrentWaypoint + 1 < Waypoints.Count)
                return CurrentWaypoint + 1;
            else return -1;
        }
    }
}