using UnityEngine;

/// <summary>
/// Класс для управления кабинами экипажа
/// </summary>
public abstract class CrewNode : MonoBehaviour {
    /// <summary>
    /// Инициализирует кабину, загружает контролы и синхроинизирует их
    /// </summary>
    public abstract void Initialize();
}