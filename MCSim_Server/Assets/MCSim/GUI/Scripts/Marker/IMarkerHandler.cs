using UnityEngine;
using System.Collections;

/// <summary>
/// Интерфейс для обработки событий приема маркера
/// </summary>
public interface IMarkerHandler {

    /// <summary>
    /// Передача маркера на обработку
    /// </summary>
    /// <param name="marker">Маркер</param>
    void SendMarker(MCSMarker marker);

    /// <summary>
    /// Передача информации из маркера на обработку
    /// </summary>
    /// <param name="data">Информация</param>
    void SendMarkerData(object data);
}
