using System.Reflection;
using UnityEngine;
using System.Collections;
using MilitaryCombatSimulator;

/// <summary>
/// ��������� ����������� ������ MilitartCombatSimulator
/// </summary>
public interface MCSFactory
{
    /// <summary>
    /// ���������� GameObject ������� Weaponry, ���������� �� ���������� ���
    /// </summary>
    /// <param name="weaponryType">��� Weaponry</param>
    GameObject CreateWeaponry<WeaponryT>(Weaponry weaponry);
}

/// <summary>
/// ��������� �����. ������ ������ �������, ����������� �������, ��������� �� ��� � �� ���������.
/// </summary>
public static class MCSGlobalFactory {
    
    /// <summary>
    /// ���������� GameObject ������� Weaponry, ���������� �� ���������� ���
    /// </summary>
    public static GameObject InstantiateWeaponry<WeaponryT>()
    {
        GameObject weaponryObject = null;
        Weaponry weaponry = null;

        try {
            weaponry = (Weaponry) Assembly.GetExecutingAssembly().CreateInstance(typeof (WeaponryT).FullName);

            weaponryObject = (GameObject) GameObject.Instantiate(Resources.Load(weaponry.PrefabPath), Vector3.zero, Quaternion.identity);
            return weaponryObject;
        }
        catch
        {
            Debug.LogWarning("������ ��� �������� ���������� Weaponry.");
            return null;
        }
    }

    public static GameObject InstantiateWeaponry(string weaponryType)
    {
        GameObject weaponryObject = null;
        Weaponry weaponry = null;

        try {
            weaponry = (Weaponry)Assembly.GetExecutingAssembly().CreateInstance(weaponryType);

            weaponryObject = (GameObject)GameObject.Instantiate(Resources.Load(weaponry.PrefabPath), Vector3.zero, Quaternion.identity);
            return weaponryObject;
        }
        catch
        {
            Debug.LogWarning("������ ��� �������� ���������� Weaponry.");
            return null;
        }
    }
}
