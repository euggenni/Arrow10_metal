using System.Collections.Generic;
using UnityEngine;
using System.Collections;

#pragma warning disable 0414, 0108, 0219
#pragma warning disable 0618, 0168

public abstract class AIUnit : MonoBehaviour
{
    /// <summary>
    /// ������������ �� � ������ ����� ��� ���������� ���������
    /// </summary>
    public bool Cyclical;

    /// <summary>
    /// ����������� ���������� ������� � ����
    /// </summary>
    public float ApproachDistance = 150f;

    /// <summary>
    /// ����� ����������
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

                Waypoints.Remove(temp); // ������� �� ������
                Destroy(temp.gameObject); // ���������� �����
                Debug.Log("���������� ��������");

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
    /// ������� ����� ����������
    /// </summary>
    /// <param name="index">ID ����� ����������</param>
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
    /// ������� ����� ����������
    /// </summary>
    /// <param name="wpt">����� ����������</param>
    public void MoveToWaypoint(MCSWaypoint wpt)
    {
        int id = -1;
        if ((id = Waypoints.IndexOf(wpt)) != -1) MoveToWaypoint(id);

        // ���� ��� ������� ����� ����������, ��������� �� 
        if(CurrentWaypoint == -1) CurrentWaypoint = 0;

        Waypoints.Insert(CurrentWaypoint, wpt);     // ��������� �����
        MoveToWaypoint(CurrentWaypoint);            // �������� � ���
    }

    /// <summary>
    /// �������� ���������� ����� ���������� ������ � �� �������
    /// </summary>
    /// <param name="wpt">����� ����������</param>
    public void AddWaypointsAsCurrent(params MCSWaypoint[] wpt)
    {
        if (CurrentWaypoint == -1) CurrentWaypoint = 0;
        Waypoints.InsertRange(CurrentWaypoint, wpt);
    }
    /// <summary>
    /// �������� ���������� ����� ���������� ����� �������
    /// </summary>
    /// <param name="wpt">����� ����������</param>
    public void AddWaypointsAfterCurrent(params MCSWaypoint[] wpt)
    {
        if (CurrentWaypoint == -1) CurrentWaypoint = 0;
        Waypoints.InsertRange(CurrentWaypoint + 1, wpt);
    }

    /// <summary>
    /// ���������� ���������� � ������� �����
    /// </summary>
    public void Resume() {
        MoveToWaypoint(CurrentWaypoint);
    }

    private int _currentWaypoint = -1;

    /// <summary>
    /// ID ������� ����� ����������
    /// </summary>
    public int CurrentWaypoint {
        get { return _currentWaypoint; }
        private set { _currentWaypoint = value; }
    }
    
    /// <summary>
    /// ���������� ��� ������ ����� ��� ����������
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

                // ���� ��������, ������������ �� ������
                if (Cyclical)
                    tempIndex = 0;
            }

            return tempIndex;
        }
    }

    /// <summary>
    /// ���������� ��� ����� ����������
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
