using UnityEngine;

/// <summary>
/// Перечисление, определяющее тип точки
/// </summary>
public enum WaypointCommand {
    /// <summary>
    /// Не реагировать
    /// </summary>
    None = 0,

    /// <summary>
    /// Двигаться к...
    /// </summary>
    Move = 1,

    /// <summary>
    /// Атаковать...
    /// </summary>
    Attack = 2
}

/// <summary>
/// Класс для определения точек движения объектов
/// </summary>
public class MCSWaypoint : MonoBehaviour {
    /// <summary>
    /// Тип точки следования
    /// </summary>
    public WaypointCommand Command = WaypointCommand.None;

    /// <summary>
    /// Пункт назначения
    /// </summary>
    public GameObject Point {
        get { return gameObject; }
    }
}