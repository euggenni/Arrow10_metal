using UnityEngine;

public interface MCSIUIObject {
    /// <summary>
    /// Скрыть элемент
    /// </summary>
    void Hide();

    /// <summary>
    /// Отобразить элемент
    /// </summary>
    void Show();

    /// <summary>
    /// Закрыть элемент
    /// </summary>
    void Close();

    /// <summary>
    /// Видимость объекта
    /// </summary>
    bool Visible { get; set; }
}

public abstract class MCSUIObject : MonoBehaviour {
    protected bool _visible;

    /// <summary>
    /// Скрыть элемент
    /// </summary>
    public abstract void Hide();

    /// <summary>
    /// Отобразить элемент
    /// </summary>
    public abstract void Show();

    /// <summary>
    /// Закрыть элемент
    /// </summary>
    public abstract void Close();

    /// <summary>
    /// Видимость объекта
    /// </summary>
    bool Visible { get; set; }
}