using System;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public class WeaponryPlane_A10 : WeaponryPlane, AIControllable {
    
    #region Ресурсы

    Hashtable _resources;

    public override Hashtable Resources
    {
        get { return _resources; }
    }

    private void LoadResources()
    {
        _resources = new Hashtable();

        _resources.Add("Icon_WeaponryList", "Icon_WL_A10"); // Иконка для списка вооружений
    }

    #endregion

    public override string Name
    {
        get { return "A-10 Thunderbolt"; }
    }

    public override WeaponryCategory Category
    {
        get { return WeaponryCategory.Air; }
    }

    public override string PrefabPath
    {
        get { return "WeaponryModel/WeaponryPlane/A-10/Prefab_A10"; }
    }

    public override void Execute(MCSCommand cmd)
    {
        throw new System.NotImplementedException();
    }

    public override void OnWeaponryInstantiate()
    {
        Debug.Log("Клиентов: " + MCSGlobalSimulation.Weapons.Synchronizations[ID]);
        Debug.Log("Размещен " + Name + " [" + ID + "]");
    }

    #region AIControllable Members

    void Awake()
    {
        LoadResources();
    }

    public void InitializeAI()
    {
        AirCraftAI ai = gameObject.GetComponent<AirCraftAI>();

        ai.WheelSpeed = 1;
        ai.AngleFault = 4;
        ai.PitchAreaAngle = 15;

        ai.enabled = true;
        GetComponent<AirCraftToolkit>().enabled = true;
    }

    public AIUnit AIUnit
    {
        get
        {
            try
            {
                AirCraftAI ai = gameObject.GetComponent<AirCraftAI>();

                if (ai == null)
                    Debug.LogWarning("Weaponry: ID[" + ID + "] Name[" + Name + "] не содержит компонента AIUnit.");

                return ai;
            }
            catch
            {
                return null;
            }
        }
    }

    [RPC]
    public override void Destroy()
    {
        Debug.Log("Su-34 Destroy");

        if (Network.isServer)
        {
            networkView.RPC("Destroy", RPCMode.Others);
            Debug.Log("Отослали остальным");
        }

        GameObject.Destroy(GetComponent<AIUnit>());
        GameObject.Destroy(GetComponent<AirCraftToolkit>());
        // Удаляем NetworkView
        foreach (NetworkView view in GetComponentsInChildren<NetworkView>())
        {
            Destroy(view);
        }

        gameObject.FallAPart(transform.position);
        try
        {
            Destroy(gameObject.GetComponent<LieTargetOut>());
        }
        catch (Exception) { }
        Destroy(this);
    }

    #endregion

}
