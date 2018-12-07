using System.Reflection;
using MilitaryCombatSimulator;
using UnityEngine;

/// <summary>
/// Интерфейс абстрактных фабрик MilitartCombatSimulator
/// </summary>
public interface MCSFactory {
    /// <summary>
    /// Возвращает GameObject объекта Weaponry, созданного по указанному тпу
    /// </summary>
    /// <param name="weaponryType">Тип Weaponry</param>
    GameObject CreateWeaponry<WeaponryT>(Weaponry weaponry);
}

/// <summary>
/// Командный центр. Хранит списки игроков, обрабаывает события, пришедшие от них и их контролов.
/// </summary>
public static class MCSGlobalFactory {
    /// <summary>
    /// Возвращает GameObject объекта Weaponry, созданного по указанному тпу
    /// </summary>
    public static GameObject InstantiateWeaponry<WeaponryT>() {
        GameObject weaponryObject = null;
        Weaponry weaponry = null;

        try {
            weaponry = (Weaponry) Assembly.GetExecutingAssembly().CreateInstance(typeof(WeaponryT).FullName);
            Debug.Log(typeof(WeaponryT).FullName + " <");
            weaponryObject = (GameObject) GameObject.Instantiate(Resources.Load(weaponry.PrefabPath), Vector3.zero,
                Quaternion.identity);
            return weaponryObject;
        } catch {
            Debug.LogWarning("Ошибка при создании экземпляра Weaponry.");
            return null;
        }
    }

    public static GameObject InstantiateWeaponry(string weaponryType) {
        GameObject weaponryObject = null;
        Weaponry weaponry = null;

        try {
            weaponry = (Weaponry) Assembly.GetExecutingAssembly().CreateInstance(weaponryType);

            weaponryObject = (GameObject) GameObject.Instantiate(Resources.Load(weaponry.PrefabPath), Vector3.zero,
                Quaternion.identity);
            return weaponryObject;
        } catch {
            Debug.LogWarning("Ошибка при создании экземпляра Weaponry.");
            return null;
        }
    }
}