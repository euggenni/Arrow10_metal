using System;
using System.Collections.Generic;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;
#pragma warning disable 0414, 0168, 0618

public class WeaponryPlane_Cloud : WeaponryPlane, AIControllable //MonoBehaviour
{
    #region Ресурсы

    Hashtable _resources;

    public override Hashtable Resources
    {
        get { return _resources; }
    }

    private void LoadResources()
    {
        _resources = new Hashtable();

        _resources.Add("Icon_WeaponryList", "Icon_WL_AH64"); // Иконка для списка вооружений
    }

    #endregion
    public override void OnWeaponryInstantiate()
    {
        Debug.Log("Размещен " + Name + " [" + ID + "]");
    }

    private void Awake()
    {
        LoadResources();
    }


    public override string Name
    {
        get { return "Cloud"; }
    }

    public override WeaponryCategory Category
    {
        get { return WeaponryCategory.Air; }
    }

    public override string PrefabPath
    {
        get { return "WeaponryModel/WeaponryPlane/Cloud/Prefab_Cloud_1"; }
    }

    public override void Execute(MCSCommand cmd)
    {
        throw new System.NotImplementedException();
    }

    #region AIControllable Members

    public void InitializeAI()
    {
        //AirCraftAI ai = gameObject.GetComponent<AirCraftAI>();

        //ai.WheelSpeed = 1;
        //ai.AngleFault = 12;
        //ai.PitchAreaAngle = 30;

        //ai.enabled = true;
        //GetComponent<AirCraftToolkit>().enabled = true;
    }

    public AIUnit AIUnit
    {
        get
        {
            //AirCraftAI ai = gameObject.GetComponent<AirCraftAI>();

            //if (ai == null)
            //Debug.LogWarning("Weaponry: ID[" + ID + "] Name[" + Name + "] не содержит компонента AirCraftAI.");

            return new AirCraftAI();
        }
    }

    private AirCraftAI ai;

    #endregion

    [RPC]
    public override void Destroy()
    {
        //Debug.Log("AH64 Destroy");

        if (Network.isServer)
        {
            networkView.RPC("Destroy", RPCMode.Others);
            Debug.Log("Отослали остальным");
        }

        // GameObject.Destroy(GetComponent<AIUnit>());
        //GameObject.Destroy(GetComponent<AirCraftToolkit>());
        // Удаляем NetworkView
        foreach (NetworkView view in GetComponentsInChildren<NetworkView>())
        {
            Destroy(view);
        }

        //gameObject.FallAPart(transform.position);

        Destroy(this);
    }
}

