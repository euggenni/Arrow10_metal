using System;
using System.Collections;
using MilitaryCombatSimulator;
using UnityEngine;

public class WeaponryPlane_WarElement : WeaponryPlane, AIControllable {
    #region Ресурсы

    Hashtable _resources;

    public override Hashtable Resources {
        get { return _resources; }
    }

    private void LoadResources() {
        _resources = new Hashtable();

        _resources.Add("Icon_WeaponryList", "Icon_WL_A10"); // Иконка для списка вооружений
    }

    #endregion

    public override string Name {
        get { return "WarElement"; }
    }

    public override WeaponryCategory Category {
        get { return WeaponryCategory.Air; }
    }

    public override string PrefabPath {
        get { return "WeaponryModel/WeaponryPlane/WarElement/WarElement"; }
    }

    public override void Execute(MCSCommand cmd) {
        throw new NotImplementedException();
    }


    #region AIControllable Members

    void Awake() {
        LoadResources();
    }

    public void InitializeAI() {
        AirCraftAI ai = gameObject.AddComponent<AirCraftAI>();

        ai.WheelSpeed = 1;
        ai.AngleFault = 4;
        ai.PitchAreaAngle = 15;
    }

    public AIUnit AIUnit {
        get {
            AirCraftAI ai = gameObject.GetComponent<AirCraftAI>();

            if (ai == null)
                Debug.LogWarning("Weaponry: ID[" + ID + "] Name[" + Name + "] не содержит компонента AIUnit.");

            return ai;
        }
    }

    #endregion

    public override void OnWeaponryInstantiate() {
        Debug.Log("Размещен " + Name + " [" + ID + "]");
    }

    [RPC]
    public override void Destroy() {
        // Удаляем NetworkView
        foreach (NetworkView view in GetComponentsInChildren<NetworkView>()) {
            Destroy(view);
        }

        Destroy(GetComponent<NetworkInterpolatedTransform>());

        //gameObject.FallAPart(gameObject.transform.position);
    }
}