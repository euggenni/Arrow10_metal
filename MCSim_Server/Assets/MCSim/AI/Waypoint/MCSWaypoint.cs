using UnityEngine;
using System.Collections;

/// <summary>
/// ������������, ������������ ��� �����
/// </summary>
public enum WaypointCommand
{
    /// <summary>
    /// �� �����������
    /// </summary>
    None = 0,

    /// <summary>
    /// ��������� �...
    /// </summary>
    Move = 1,

    /// <summary>
    /// ���������...
    /// </summary>
    Attack = 2
}

/// <summary>
/// ����� ��� ����������� ����� �������� ��������
/// </summary>
public class MCSWaypoint : MonoBehaviour {

    /// <summary>
    /// ��� ����� ����������
    /// </summary>
    public WaypointCommand Command = WaypointCommand.None;

    /// <summary>
    /// ����� ����
    /// </summary>
    public Vector3 Point
    {
        get { return gameObject.transform.position; }
    }

    /// <summary>
    /// ��������� ����� ����������
    /// </summary>
    public MCSWaypoint NextPoint { get; set; }
}
