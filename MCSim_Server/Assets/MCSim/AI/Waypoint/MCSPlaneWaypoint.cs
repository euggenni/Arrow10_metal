using UnityEngine;
using System.Collections;

/// <summary>
/// Возможные маневры самолета
/// </summary>
public enum PlaneManeuer
{
    None = 0
}

/// <summary>
/// Класс для определения точек движения летающих объектов
/// </summary>
public class MCSPlaneWaypoint : MCSWaypoint
{
    /// <summary>
    /// Характеризует маневр самолета при/для достижения точки следования
    /// </summary>
    public PlaneManeuer Maneuer = PlaneManeuer.None;
       
    /// <summary>
    /// Высота точки над землей
    /// </summary>
    public float Height = 100f;

    void Start()
    {
      //  SetHeight(Height);
    }

    /// <summary>
    /// Задать высоту точки над землей
    /// </summary>
    /// <param name="height">Высота</param>
    public void SetHeight(float height)
    {
        Ray ray = new Ray();
        ray.origin = gameObject.transform.position;
        ray.direction = -gameObject.transform.up; // Луч вниз

        RaycastHit hit;

        // Посылаем луч вниз
        Vector3 _contactPoint = Vector3.zero;

        if (Physics.Raycast(ray, out hit, 10000))
        {
            _contactPoint = hit.point;
        }

        // Посылаем луч вверх
        if (_contactPoint == Vector3.zero)
        {
            ray.direction = gameObject.transform.up; // Луч вверх
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
            Debug.Log("Точка контакта с землей не найдена");
        }
    }
}
