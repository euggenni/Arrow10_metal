using UnityEngine;
using System.Collections;
using MilitaryCombatSimulator;

public class WeaponryPlane_TargetsCloud : WeaponryPlane, AIControllable
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
        get { return "TargetsCloud"; }
    }

    public override WeaponryCategory Category
    {
        get { return WeaponryCategory.Air; }
    }

    public override string PrefabPath
    {
        get { return "WeaponryModel/WeaponryPlane/TargetCloud/TargetsCloud"; }
    }

    public override void Execute(MCSCommand cmd)
    {
        throw new System.NotImplementedException();
    }

    #region AIControllable Members

    public void InitializeAI()
    {
        AirCraftAI ai = gameObject.GetComponent<AirCraftAI>();

        //ai.WheelSpeed = 1;
        //ai.AngleFault = 12;
        //ai.PitchAreaAngle = 30;

        ai.enabled = true;
        GetComponent<AirCraftToolkit>().enabled = true;
    }

    public AIUnit AIUnit
    {
        get
        {
            AirCraftAI ai = gameObject.GetComponent<AirCraftAI>();

            if (ai == null)
                Debug.LogWarning("Weaponry: ID[" + ID + "] Name[" + Name + "] не содержит компонента AirCraftAI.");

            return ai;
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

        GameObject.Destroy(GetComponent<AIUnit>());
        GameObject.Destroy(GetComponent<AirCraftToolkit>());
        //Удаляем NetworkView
        foreach (NetworkView view in GetComponentsInChildren<NetworkView>())
        {
            Destroy(view);
        }

        //gameObject.FallAPart(transform.position);

        Destroy(this);
    }
}
