using UnityEngine;
using System.Collections;

/// <summary>
/// Получатель входящих Marker сообщений
/// </summary>
[AddComponentMenu("MCS/Marker Reciever")]
public class MCSMarkerReciever : MonoBehaviour {

    public enum Trigger
    {
        OnClick,
        OnMouseOver,
        OnMouseOut,
        OnPress,
        OnRelease,
    }

    //public GameObject target;
    //public string functionName;
    
    /// <summary>
    /// Обработчик события маркера
    /// </summary>
    public IMarkerHandler target;

    public Trigger trigger = Trigger.OnClick;
    //public bool includeChildren = false;

    void OnHover(bool isOver)
    {
        if (((isOver && trigger == Trigger.OnMouseOver) ||
            (!isOver && trigger == Trigger.OnMouseOut))) Send();
    }

    void OnPress(bool isPressed)
    {
        if (((isPressed && trigger == Trigger.OnPress) ||
            (!isPressed && trigger == Trigger.OnRelease))) Send();
    }

    void OnClick()
    {
        if (trigger == Trigger.OnClick) Send();
    }

    void Send()
    {
        if (!enabled || !gameObject.active) return;
        if (MCSUICenter.Marker == null) return;
        if (target == null) return;

        target.SendMarker(MCSUICenter.Marker);
    }
}
