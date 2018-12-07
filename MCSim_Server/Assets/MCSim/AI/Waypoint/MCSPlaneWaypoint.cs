using UnityEngine;
using System.Collections;

/// <summary>
/// ��������� ������� ��������
/// </summary>
public enum PlaneManeuer
{
    None = 0
}

/// <summary>
/// ����� ��� ����������� ����� �������� �������� ��������
/// </summary>
public class MCSPlaneWaypoint : MCSWaypoint
{
    /// <summary>
    /// ������������� ������ �������� ���/��� ���������� ����� ����������
    /// </summary>
    public PlaneManeuer Maneuer = PlaneManeuer.None;
       
    /// <summary>
    /// ������ ����� ��� ������
    /// </summary>
    public float Height = 100f;

    void Start()
    {
      //  SetHeight(Height);
    }

    /// <summary>
    /// ������ ������ ����� ��� ������
    /// </summary>
    /// <param name="height">������</param>
    public void SetHeight(float height)
    {
        Ray ray = new Ray();
        ray.origin = gameObject.transform.position;
        ray.direction = -gameObject.transform.up; // ��� ����

        RaycastHit hit;

        // �������� ��� ����
        Vector3 _contactPoint = Vector3.zero;

        if (Physics.Raycast(ray, out hit, 10000))
        {
            _contactPoint = hit.point;
        }

        // �������� ��� �����
        if (_contactPoint == Vector3.zero)
        {
            ray.direction = gameObject.transform.up; // ��� �����
            if (Physics.Raycast(ray, out hit, 10000))
            {
                _contactPoint = hit.point;
            }
        }

        if (_contactPoint != Vector3.zero)
        {
            gameObject.transform.position = new Vector3(_contactPoint.x, _contactPoint.y + height, _contactPoint.z);
        }
        else
        {
            Debug.Log("����� �������� � ������ �� �������");
        }
    }
}
