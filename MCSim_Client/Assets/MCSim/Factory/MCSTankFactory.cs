using MilitaryCombatSimulator;
using UnityEngine;

public class MCSTankFactory : MCSFactory {
    public GameObject CreateWeaponry<WeaponryT>(Weaponry weaponry) {
        GameObject weaponryObject = null;

        if (weaponry != null) {
            weaponryObject = (GameObject) GameObject.Instantiate(Resources.Load(weaponry.PrefabPath), Vector3.zero,
                Quaternion.identity);
        } else {
            Debug.LogWarning("Ошибка при создании экземпляра Weaponry.");
        }

        return weaponryObject;
    }
}