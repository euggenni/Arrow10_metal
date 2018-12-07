using System;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

/// <summary>
/// Интерфейс для приема и обработки команд, поступающих в Core
/// </summary>
public class Strela10_Operator_CoreHandler : CoreLibrary.CoreHandler
{
    [SerializeField]
    public CoreLibrary.Core _core;

    public WeaponryTank_Strela10 Strela10;

    [SerializeField]
    private MilitaryCombatSimulator.Weaponry _weaponry;

    private CoreToolkit _coreToolkit;
    
    void Awake()
    {
        enabled = false;
        Debug.Log("Awake of [" + Weaponry.ID + "]");
    }


    void Start()
    {
        // Добавление контроллера башни
        //Strela10_TowerHandler towerHandler = gameObject.AddComponent<Strela10_TowerHandler>();
        //towerHandler.Handler = this;

        //
    }

    public override CoreLibrary.Core Core
    {
        get { return _core; }
        set {
            _core = value;
            _coreToolkit = _core as CoreToolkit;
            enabled = true;
        }
    }

    public override void ControlChanged(PanelControl control)
    {
        base.ControlChanged(control);
        //Debug.Log(String.Format("Control [{0}] on [{1}] changed state to [{2}]", control.GetName(), control.GetPanelName(), control.State));

        switch (control.GetPanelName())
        {
            case "Strela10_OperatorPanel":
                break;

            case "Strela10_OperationalPanel":
                break;
            
            case "Strela10_ControlBlock":
                break;

            case "Strela10_ARC":
                break;

            case "Strela10_AzimuthIndicator":
                break;

            case "Strela10_SupportPanel":
                break;

            case "Strela10_GuidancePanel":
                GuidancePanelChanged(control);
                break;

            case "Strela10_VizorPanel":
                break;
        }
    }


    

    public override MilitaryCombatSimulator.Weaponry Weaponry
    {
        get { return _weaponry; }
        set {
            if (value is WeaponryTank_Strela10)
            {
                _weaponry = value;
                Strela10 = value as WeaponryTank_Strela10;
            }
        }
    }

    #region Панели
    void GuidancePanelChanged(PanelControl control)
    {
        switch (control.GetName())
        {
            case "OperatorJoystickHorizontal":

                break;
        }
    }
    #endregion


    void FixedUpdate()
    {
    }
}
