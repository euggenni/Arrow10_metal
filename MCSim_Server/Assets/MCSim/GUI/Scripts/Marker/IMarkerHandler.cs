using UnityEngine;
using System.Collections;

/// <summary>
/// ��������� ��� ��������� ������� ������ �������
/// </summary>
public interface IMarkerHandler {

    /// <summary>
    /// �������� ������� �� ���������
    /// </summary>
    /// <param name="marker">������</param>
    void SendMarker(MCSMarker marker);

    /// <summary>
    /// �������� ���������� �� ������� �� ���������
    /// </summary>
    /// <param name="data">����������</param>
    void SendMarkerData(object data);
}
