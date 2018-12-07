using UnityEngine;

/// <summary>
/// Возможные маневры самолета
/// </summary>
public enum PlaneManeuer {
    None = 0
}

/// <summary>
/// Класс для определения точек движения летающих объектов
/// </summary>
public class MCSPlaneWaypoint : MCSWaypoint {
    /// <summary>
    /// Характеризует маневр самолета при/для достижения точки следования
    /// </summary>
    public PlaneManeuer Maneuer = PlaneManeuer.None;

    /// <summary>
    /// Высота точки над землей
    /// </summary>
    public float Height = 100f;

    Ray ray;

    void Start() {
        ray.origin = gameObject.transform.position;
        ray.direction = -gameObject.transform.up; // Луч вниз

        RaycastHit hit;

        // Посылаем луч вниз
        Vector3 _contactPoint = Vector3.zero;

        if (Physics.Raycast(ray, out hit, 10000)) {
            _contactPoint = hit.point;
        }

        // Посылаем луч вверх
        if (_contactPoint == Vector3.zero) {
            ray.direction = gameObject.transform.up; // Луч вверх
            if (Physics.Raycast(ray, out hit, 10000)) {
                _contactPoint = hit.point;
            }
        }

        if (_contactPoint != Vector3.zero) {
            gameObject.transform.position = new Vector3(_contactPoint.x, _contactPoint.y + Height, _contactPoint.z);
        } else {
            Debug.Log("Точка контакта с землей не найдена");
        }
    }
}