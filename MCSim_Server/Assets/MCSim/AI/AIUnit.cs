using System.Collections.Generic;
using UnityEngine;
using System.Collections;

#pragma warning disable 0414, 0108, 0219
#pragma warning disable 0618, 0168

public abstract class AIUnit : MonoBehaviour
{
    /// <summary>
    /// Возвращаться ли к первой токчи при достижении последней
    /// </summary>
    public bool Cyclical;

    /// <summary>
    /// Необходимое расстояние подхода к цели
    /// </summary>
    public float ApproachDistance = 150f;

    /// <summary>
    /// Точки следования
    /// </summary>
    public List<MCSWaypoint> Waypoints = new List<MCSWaypoint>();

    public void Update()
    {
        if (Waypoints.Count == 0) return;

        if (Vector3.Distance(transform.position, Waypoints[CurrentWaypoint].Point) <= ApproachDistance)
        {
            if(!Cyclical)
            {
                MCSWaypoint temp = Waypoints[CurrentWaypoint];

                Waypoints.Remove(temp); // Удаляем из списка
                Destroy(temp.gameObject); // Уничтожаем точку
                Debug.Log("Уничтожили вейпоинт");

                if(Waypoints.Count != 0)
                MoveToWaypoint(0);
            }
            else
            {
                MoveToWaypoint(NextWaypoint);
            }
        }
    }

    /// <summary>
    /// Указать точку следования
    /// </summary>
    /// <param name="index">ID точки следования</param>
    public void MoveToWaypoint(int index)
    {
        if (index < 0 || index > Waypoints.Count - 1)
        {
            Debug.LogWarning("Invalid waypoint index: " + index);
            return;
        }

        CurrentWaypoint = index;
    }

    /// <summary>
    /// Указать точку следования
    /// </summary>
    /// <param name="wpt">Точка следования</param>
    public void MoveToWaypoint(MCSWaypoint wpt)
    {
        int id = -1;
        if ((id = Waypoints.IndexOf(wpt)) != -1) MoveToWaypoint(id);

        // Если нет текущей точки следования, добавляем ее 
        if(CurrentWaypoint == -1) CurrentWaypoint = 0;

        Waypoints.Insert(CurrentWaypoint, wpt);     // Добавляем точку
        MoveToWaypoint(CurrentWaypoint);            // Движемся к ней
    }

    /// <summary>
    /// Добавить дальнейшие точки следования вместо и до текущей
    /// </summary>
    /// <param name="wpt">Точки следования</param>
    public void AddWaypointsAsCurrent(params MCSWaypoint[] wpt)
    {
        if (CurrentWaypoint == -1) CurrentWaypoint = 0;
        Waypoints.InsertRange(CurrentWaypoint, wpt);
    }
    /// <summary>
    /// Добавить дальнейшие точки следования после текущей
    /// </summary>
    /// <param name="wpt">Точки следования</param>
    public void AddWaypointsAfterCurrent(params MCSWaypoint[] wpt)
    {
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
        get
        {
            int tempIndex = -1;

            try
            {
                MCSWaypoint temp = Waypoints[CurrentWaypoint + 1];
                tempIndex = CurrentWaypoint + 1;
            }
            catch
            {
                tempIndex = -1;

                // Если циклично, возвращаемся на первый
                if (Cyclical)
                    tempIndex = 0;
            }

            return tempIndex;
        }
    }

    /// <summary>
    /// Уничтожает все точки следования
    /// </summary>
    public void DestroyWaypoints()
    {
        foreach (var mcsWaypoint in Waypoints) {
            if(mcsWaypoint)
            Destroy(mcsWaypoint.gameObject);
        }

        Waypoints.Clear();
    }

    public void OnDestroy()
    {
        DestroyWaypoints();
    }
}
