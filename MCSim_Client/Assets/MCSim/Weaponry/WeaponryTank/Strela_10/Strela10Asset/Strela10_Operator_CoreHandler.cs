using System;
using System.Collections;
using MilitaryCombatSimulator;
using UnityEngine;

//////Artyom's comment
/// <summary>
/// Интерфейс для приема и обработки команд, поступающих в Core
/// </summary>
public class Strela10_Operator_CoreHandler : CoreLibrary.CoreHandler {
    [SerializeField]
    private CoreLibrary.Core _core;

    [SerializeField]
    public WeaponryTank_Strela10 Strela10;

    public Strela10_TowerHandler TowerHandler;

    public static CoreLibrary.Core CoreStatic;

    public Strela10_Operator_VizirController vizirController;
    public float timer = 10f;
    public static float _currentAngle;
    private PanelControl _towerAngle;
    private PanelControl _towerArrowRigth;
    private PanelControl _towerArrowLeft;
    private PanelControl _towerArrowCenter;
    private bool psp = false;
    private bool pspState;
    private bool cool;


    void Start() {
        _core = gameObject.GetComponent<CoreToolkit>();
        Weaponry = gameObject.GetComponentInParents<WeaponryTank_Strela10>(false);
        Debug.Log(gameObject.name);
        TowerHandler = Strela10.TowerHandler;
        TowerHandler.enabled = true;
        TowerHandler.Strela10 = this.Strela10;
        CoreStatic = Core;

        // Корутина управления белой стрелкой поворота башни на панели азимута
        Model.UCoroutine(this, TowerAzimutSeeker(), "TowerAzimutSeeker");

        _towerAngle = Core.GetPanel("Strela10_AzimuthIndicator").GetControl(ControlType.Spinner, "SPINNER_ORIENTATION");
        _towerArrowRigth = Core.GetPanel("Strela10_AzimuthIndicator")
            .GetControl(ControlType.Indicator, "INDICATOR_RIGHT");
        _towerArrowLeft = Core.GetPanel("Strela10_AzimuthIndicator")
            .GetControl(ControlType.Indicator, "INDICATOR_LEFT");
        _towerArrowCenter = Core.GetPanel("Strela10_AzimuthIndicator")
            .GetControl(ControlType.Indicator, "INDICATOR_FORWARD");
        _currentAngle = (int) _towerAngle.State;
        if (MCSTrainingCenter.initTrain) {
            Debug.LogError("Иннициализируем тренировку при функциональном контроле");
            initTraining();
        } else {
            Debug.LogError("Подписываемся на событие слежение за целью");
            TowerHandler.OnTargetAngleChanged += TowerHandler_OnTargetAngleChanged;
        }

        setDefaulValuesToIndications24V();
    }

    void setDefaulValuesToIndications28V() {
        Debug.LogError("setDefaulValuesToIndications28V()");
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "VOLTMETER_BACKLIGHT").State = "OFF";
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_COMBAT").State = "OFF";
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_TRAINING").State = "OFF";
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_BOARD").State = "OFF";
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "LAUNCHER_1").State = "OFF";
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "LAUNCHER_2").State = "OFF";
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "LAUNCHER_3").State = "OFF";
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "LAUNCHER_4").State = "OFF";
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "CHECK").State = "OFF";
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "PZ_OHL").State = "OFF";
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_STOWED").State = "OFF";
		Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_AOZ").State = "ON";
		vizirController.aoz = true;
        Core.GetPanel("Strela10_CommonPanel").GetControl(ControlType.Tumbler, "PEDAL_AZIM").State = "OFF";
		Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_BL").State = "OFF";
        //Core.GetPanel("Strela10_CommonPanel").GetControl(ControlType.Tumbler, "PEDAL_AZIM").ControlChanged();
    }

    void setDefaulValuesToIndications24V() {
        Debug.LogError("setDefaulValuesToIndications24V() new");
        setDefaulValuesToIndications28V();
        Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Indicator, "INDICATOR_GLASS_HEATING").State = "OFF";
    }

    void initTraining() {
        Core.GetPanel("Strela10_OperatorPanel").GetControl(
            ControlType.Tumbler, "TUMBLER_POWER_24B").GetComponent<SwitcherToolkit>().TumblerStateID = 1;
        Core.GetPanel("Strela10_OperatorPanel").GetControl(
            ControlType.Tumbler, "TUMBLER_POWER_24B").GetComponent<SwitcherToolkit>().ControlChanged();
        Core.GetPanel("Strela10_OperatorPanel").GetControl(
            ControlType.Tumbler, "TUMBLER_POWER_28B").GetComponent<SwitcherToolkit>().TumblerStateID = 1;
        Core.GetPanel("Strela10_OperatorPanel").GetControl(
            ControlType.Tumbler, "TUMBLER_POWER_28B").GetComponent<SwitcherToolkit>().ControlChanged();

        Core.GetPanel("Strela10_OperatorPanel").GetControl(
            ControlType.Tumbler, "TUMBLER_DRIVE_HANDLE_OFF").GetComponent<SwitcherToolkit>().TumblerStateID = 1;
        Core.GetPanel("Strela10_OperatorPanel").GetControl(
            ControlType.Tumbler, "TUMBLER_DRIVE_HANDLE_OFF").GetComponent<SwitcherToolkit>().ControlChanged();

        Core.GetPanel("Strela10_SupportPanel").GetControl(
            ControlType.Tumbler, "TUMBLER_POSITION").GetComponent<SwitcherToolkit>().TumblerStateID = 0;
        Core.GetPanel("Strela10_SupportPanel").GetControl(
            ControlType.Tumbler, "TUMBLER_POSITION").GetComponent<SwitcherToolkit>().ControlChanged();

        Core.GetPanel("Strela10_CommonPanel").GetControl(
            ControlType.Tumbler, "PEDAL_AZIM").GetComponent<SwitcherToolkit>().TumblerStateID = 1;
        Core.GetPanel("Strela10_CommonPanel").GetControl(
            ControlType.Tumbler, "PEDAL_AZIM").GetComponent<SwitcherToolkit>().ControlChanged();

        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_STOWED").State = "OFF";
    }

    void TowerHandler_OnTargetAngleChanged(float angle) {
        if (!MCSTrainingCenter.initTrain) {
            _currentAngle = angle;
            //_towerAngle.State = (int)_currentAngle;
            if (_currentAngle >= 20 && _currentAngle < 180) {
                _towerArrowRigth.State = "ON";
                _towerArrowLeft.State = "OFF";
                _towerArrowCenter.State = "OFF";
            } else {
                if (_currentAngle >= 180 && _currentAngle < 340) {
                    _towerArrowRigth.State = "OFF";
                    _towerArrowLeft.State = "ON";
                    _towerArrowCenter.State = "OFF";
                } else {
                    _towerArrowRigth.State = "OFF";
                    _towerArrowLeft.State = "OFF";
                    _towerArrowCenter.State = "ON";
                }
            }
        } else {
            Debug.LogError("ВЫКЛЮЧАЕМ");
        }
    }

    public override CoreLibrary.Core Core {
        get { return _core; }
        set { _core = value; }
    }

    public override Weaponry Weaponry {
        get { return Strela10; }
        set {
            if (value is WeaponryTank_Strela10) {
                Strela10 = value as WeaponryTank_Strela10;
                Debug.Log("Установлен родитель [" + Strela10.name + "] для обработчика Core");
            }
        }
    }

    public override void ControlChanged(PanelControl control) {
        base.ControlChanged(control);

        Debug.Log(String.Format("Control [{0}] on [{1}] changed state to [{2}]", control.GetName(),
            control.GetPanelName(), control.State));

        Debug.LogError("MyErr: INDICATOR_GLASS_HEATING is " + Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Indicator, "INDICATOR_GLASS_HEATING").State);
        if (Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POWER_24B").State.Equals("ON") &&
            Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Indicator, "INDICATOR_GLASS_HEATING").State.Equals("ON"))
            Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Indicator, "INDICATOR_GLASS_HEATING").State = "ON";

        if (Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POWER_24B").State.Equals("OFF")) {
            setDefaulValuesToIndications24V();
            return;
        } else if (Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POWER_28B").State.Equals("OFF")) {
            //////setDefaulValuesToIndications28V();
        }


        //if(TowerHandler.currentMode == Strela10_TowerHandler.WorkMode.Marching)
        //try
        //{
        switch (control.GetPanelName()) {
            case "Strela10_OperatorPanel":
                PanelOperatorChanged(control);
                break;

            case "Strela10_OperationalPanel":
                PanelOperationalChanged(control);
                break;

            case "Strela10_ControlBlock":
                ControlBlockChanged(control);
                break;

            case "Strela10_SupportPanel":
                SupportPanelChanged(control);
                break;
            case "Strela10_GuidancePanel":
                GuidancePanelChaged(control);
                break;
            case "Strela10_CommonPanel":
                CommonPanelPanelChanged(control);
                break;
            case "Strela10_VizorPanel":
                VizorPanelChange(control);
                break;
            case "Strela10_ARC":
                ARCPanelChange(control);
                break;

            default:
                break;
        }
        //}
        //catch(Exception e)
        //{
        //    Debug.LogWarning("Ошибка при обработке переключения тумблера (НЕ В БОЕВОМ)");
        //}
    }

    public void ARCPanelChange(PanelControl control) {
        switch (control.GetName()) {
            case "TUMBLER_A3":
                if (control.State.Equals("ON")) {
                    Core.GetPanel("Strela10_ARC").GetControl(ControlType.Indicator, "INDICATOR_STATUS").State = "ON";
                } else {
                    Core.GetPanel("Strela10_ARC").GetControl(ControlType.Indicator, "INDICATOR_STATUS").State = "OFF";
                }
                break;
        }
    }

    public void VizorPanelChange(PanelControl control) {
        switch (control.GetName()) {
            case "TUMBLER_POSITION_WORK_TYPE":
                if (control.State.Equals("POS_6")) {
                    Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Indicator, "INDICATOR_U_AMPER").State =
                        "ON";
                    Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Indicator, "INDICATOR_U_VENT").State =
                        "OFF";
                }
                if (control.State.Equals("POS_7")) {
                    Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Indicator, "INDICATOR_U_AMPER").State =
                        "ON";
                }
                if (control.State.Equals("POS_8")) {
                    Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Indicator, "INDICATOR_U_NC").State =
                        "ON";
                }
                break;
            case "TUMBLER_POWER_NRZ":
                if (control.State.Equals("ON")) {
                    Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Indicator, "INDICATOR_U_NRZ_POWER")
                        .State = "ON";
                    vizirController.nrz = true;
                } else {
                    Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Indicator, "INDICATOR_U_NRZ_POWER")
                        .State = "OFF";
                }
                break;
            case "TUMBLER_VZ":
                if (control.State.Equals("ON")) {
                    Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Indicator, "INDICATOR_U_CENTER").State =
                        "ON";
                    Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Indicator, "INDICATOR_U_BACK").State =
                        "ON";
                } else {
                    Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Indicator, "INDICATOR_U_CENTER").State =
                        "OFF";
                    Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Indicator, "INDICATOR_U_BACK").State =
                        "OFF";
                }
                break;
            case "TUMBLER_SD":
                Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Indicator, "INDICATOR_U_CENTER").State =
                    "OFF";
                Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Indicator, "INDICATOR_U_BACK").State =
                    "OFF";
                break;
        }
    }

    public void CommonPanelPanelChanged(PanelControl control) {
        switch (control.GetName()) {
            case "PEDAL_AZIM":
                //if (control.State.Equals("OFF")) {
                //    Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_STOWED")
                //        .State = "ON";
                //    Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "CHECK").State = "OFF";
                //    Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "PZ_OHL").State = "OFF";
                //}
                if (control.State.Equals("ON")) {
                    Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_STOWED")
                        .State = "OFF";
                    StartCoroutine(LateStart(10.3f));
                }
                break;
        }
    }

    IEnumerator LateStart(float timer) {
        yield return new WaitForSeconds(timer);
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_STOWED")
            .State = "OFF";
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "CHECK").State =
            "ON";
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "PZ_OHL").State =
            "ON";
		Debug.Log("Попытка установить боевой-учебный + НРЗ индикатор");
		if (Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_MODE").State.Equals("COMBAT")){
			Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_COMBAT").State = "ON";
			Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_TRAINING").State = "OFF";
		} else if (Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_MODE").State.Equals("TRAINING")){
			Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_COMBAT").State = "OFF";
			Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_TRAINING").State = "ON";
		}
		if (Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_BL").State.Equals("ON")){
			Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_BL").State = "OFF";
		} else{
			Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_BL").State = "ON";	
		}
		
        Strela10.Arms.ChargeProjectile();
    }

    public void GuidancePanelChaged(PanelControl control) {
        /*
         _controlPosition = Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_SupportPanel").GetControl(
                                    ControlType.Tumbler, "TUMBLER_POSITION");
        _control24B = Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
                            ControlType.Tumbler, "TUMBLER_POWER_24B");
        _control28B = Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
                            ControlType.Tumbler, "TUMBLER_POWER_28B");
        _controlDrive = Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
                            ControlType.Tumbler, "TUMBLER_DRIVE_HANDLE_OFF");
        _controlStowedAzim = Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_CommonPanel").GetControl(
                            ControlType.Tumbler, "TUMBLER_STOP");
        _controlPedalAzim = Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_CommonPanel").GetControl(
                            ControlType.Tumbler, "PEDAL_AZIM");
         */

        ///test
        PanelControl _controlPosition = Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_SupportPanel")
            .GetControl(
                ControlType.Tumbler, "TUMBLER_POSITION");
        PanelControl _control24B = Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
            ControlType.Tumbler, "TUMBLER_POWER_24B");
        PanelControl _control28B = Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
            ControlType.Tumbler, "TUMBLER_POWER_28B");
        PanelControl _controlDrive = Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
            ControlType.Tumbler, "TUMBLER_DRIVE_HANDLE_OFF");
        PanelControl _controlStowedAzim = Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_CommonPanel")
            .GetControl(
                ControlType.Tumbler, "TUMBLER_STOP");
        PanelControl _controlPedalAzim = Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_CommonPanel")
            .GetControl(
                ControlType.Tumbler, "PEDAL_AZIM");


        switch (control.GetName()) {
            case "TUMBLER_COOL":
                if (control.State.Equals("ON")) {
                    cool = true;
                    if (psp && pspState) {
                        WeaponryRocket_9M37.pspState = true;
                    } else {
                        WeaponryRocket_9M37.pspState = false;
                    }
                    WeaponryRocket_9M37 rocket = Strela10.Arms.Projectile as WeaponryRocket_9M37;
                    if (rocket) {
                        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "PZ_OHL").State = "OFF";
                        if (!rocket.FirstCooler.IsUsed) {
                            rocket.FirstCooler.Activate();
                        } else {
                            Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "CHECK").State = "OFF";
                            if (!rocket.SecondCooler.IsUsed) {
                                rocket.SecondCooler.Activate();
                            }
                        }
                        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_I").State = rocket.IsCoolingActive ? "ON" : "OFF";
                    } else {
                        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_I").State = "OFF";
                    }
                    foreach (GameObject cloud in MCSGlobalSimulation.CloudPointsWhite) {
                        cloud.SetActive(false);
                    }
                    foreach (var cloud in MCSGlobalSimulation.Cloud) {
                        cloud.enabled = false;
                    }
                    foreach (GameObject cloud in MCSGlobalSimulation.CloudPointsGrey) {
                        cloud.SetActive(false);
                    }
                } else {
                    if (control.State.Equals("OFF")) {
                        cool = false;
                        if (psp && pspState) {
                            WeaponryRocket_9M37.pspState = true;
                        } else {
                            WeaponryRocket_9M37.pspState = false;
                        }
						if (Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_FON").State
                            .Equals("STATE_3")) {
                            foreach (GameObject cloud in MCSGlobalSimulation.CloudPointsWhite) {
                                cloud.SetActive(false);
                            }
                            foreach (var cloud in MCSGlobalSimulation.Cloud) {
                                cloud.enabled = false;
                            }
                            foreach (GameObject cloud in MCSGlobalSimulation.CloudPointsGrey) {
                                cloud.SetActive(false);
                            }
                        } else{
	                        if (Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_FON").State
	                            .Equals("STATE_2")) {
	                            foreach (GameObject cloud in MCSGlobalSimulation.CloudPointsWhite) {
	                                cloud.SetActive(false);
	                            }
	                            foreach (var cloud in MCSGlobalSimulation.Cloud) {
	                                cloud.enabled = false;
	                            }
	                            foreach (GameObject cloud in MCSGlobalSimulation.CloudPointsGrey) {
	                                cloud.SetActive(true);
	                            }
	                        } else {
	                            if (Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_FON")
	                                .State.Equals("STATE_1")) {
	                                foreach (GameObject cloud in MCSGlobalSimulation.CloudPointsWhite) {
	                                    cloud.SetActive(true);
	                                }
	                                foreach (var cloud in MCSGlobalSimulation.Cloud) {
	                                    cloud.enabled = true;
	                                }
	                                foreach (GameObject cloud in MCSGlobalSimulation.CloudPointsGrey) {
	                                    cloud.SetActive(true);
	                                }
	                            }
	                        }
						}
                    }
                }

                break;
            case "TUMBLER_WORK_TYPE":
                Debug.LogError("TUMBLER_WORK_TYPE!!!!");
                break;
            case "TUMBLER_BOARD":
                ///Log privod
                //Debug.Log(String.Format("Check result of BOARD:"));
                //Debug.Log(String.Format("_controlPosition: " + _controlPosition.State));
                //Debug.Log(String.Format("_control24B: " + _control24B.State));
                //Debug.Log(String.Format("control.State: " + control.State));
                //Debug.Log(String.Format("_control28B: " + _control28B.State));
                //Debug.Log(String.Format("_controlDrive: " + _controlDrive.State));
                //Debug.Log(String.Format("_controlStowedAzim: " + _controlStowedAzim.State));
                //Debug.Log(String.Format("_controlPedalAzim: " + _controlPedalAzim.State));

                Debug.Log(String.Format("control.State: " + control.State.Equals("ON")));
                Debug.Log(String.Format("Check result of BOARD:"));
                Debug.Log(String.Format("_controlPosition: " + _controlPosition.State.Equals("BATTLE")));
                Debug.Log(String.Format("_control24B: " + _control24B.State.Equals("ON")));
                Debug.Log(String.Format("_control28B: " + _control28B.State.Equals("ON")));
                Debug.Log(String.Format("_controlDrive: " + !_controlDrive.State.Equals("OFF")));
                Debug.Log(String.Format("_controlStowedAzim: " + _controlStowedAzim.State.Equals("OFF")));
                Debug.Log(String.Format("_controlPedalAzim: " + _controlPedalAzim.State.Equals("ON")));

                if ((Strela10.Arms.Projectile as WeaponryRocket) != null) 
                {
                    if (control.State.Equals("ON") && _controlPosition.State.Equals("BATTLE") &&
                        _control24B.State.Equals("ON") && _control28B.State.Equals("ON") &&
                        !_controlDrive.State.Equals("OFF") && _controlStowedAzim.State.Equals("OFF") &&
                        _controlPedalAzim.State.Equals("ON")) {
                        Debug.Log(String.Format("BOARD IS ON"));
                        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_BOARD").State =
                            control.State;
                        vizirController.bort = true;
						vizirController.PlayBortSound();
                    }
                }

                Debug.Log("Status: Strela10.Arms.countLancher" + Strela10.Arms.countLancher);
                Debug.Log("Status: tmp " + (Strela10.Arms.Projectile as WeaponryRocket)); 
                break;
            case "TUMBLER_TRACK_LAUNCH":
                vizirController.nrz = true;
                break;
        }
    }

    public void PanelOperatorChanged(PanelControl control) {
        bool v24 = Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "VOLTMETER_BACKLIGHT")
            .State.Equals("ON");

        switch (control.GetName()) {
			case "TUMBLER_BL":
				if (Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_BL").State.Equals("ON"))
				{
					Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_BL").State = "OFF";
				} else
				{
					if (control.State.ToString().Equals("COMBAT"))
						Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_BL").State = "ON";
				}
			break;
            case "TUMBLER_DRIVE_HANDLE_OFF":
                if (Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_DRIVE_HANDLE_OFF").State.Equals("DRIVE")) {
                    Debug.LogError("TUMBLER_DRIVE_HANDLE_OFF -> DRIVE!!!!");
                    if (Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POWER_24B").State.Equals("ON") &&
                        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POWER_28B").State.Equals("ON")) {
                        Debug.LogError("TUMBLER_DRIVE_HANDLE_OFF -> DRIVE -> 24 & 28 ON!!!!");
                        //////initialise in methon when matching finished
                        //////Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Spinner, "SPINNER_VOLTMETER").State =
                        //////    (int) 27;
                        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_STOWED").State = "ON";
                        //////Strela10.Arms.ChargeProjectile();
                    } else {
                        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_STOWED").State = "OFF";
                    }
                } else if (Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_DRIVE_HANDLE_OFF").State.Equals("OFF")) {
                    Debug.LogError("TUMBLER_DRIVE_HANDLE_OFF -> OFF -> Default!!!!");
                    setDefaulValuesToIndications28V();
                }
                break;
            case "TUMBLER_POWER_24B":
                //////Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "VOLTMETER_BACKLIGHT").State =
                //////    control.State;
                //////if (control.State.Equals("ON") && Core.GetPanel("Strela10_OperatorPanel")
                //////        .GetControl(ControlType.Tumbler, "TUMBLER_POWER_28B").State.Equals("ON")) {
                //////    Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Spinner, "SPINNER_VOLTMETER").State =
                //////        (int) 28;
                //////} else {
                //////    Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Spinner, "SPINNER_VOLTMETER").State =
                //////        (int) 0;
                //////}

                //////initialise in methon when matching finished
                //////if (((string) control.State).Equals("OFF")) {
                //////    Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_TRAINING")
                //////        .State = control.State;
                //////    Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_COMBAT")
                //////        .State = control.State;

                //////    Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_BOARD").State =
                //////        "OFF";
                //////    vizirController.bort = false;
                //////    Debug.Log("BORT OFF");
                //////} else {
                //////    Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_MODE")
                //////        .ControlChanged();
                //////}

                Debug.LogError("MyErr: inside 24V INDICATOR_GLASS_HEATING is " + Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Indicator, "INDICATOR_GLASS_HEATING").State);
                if (Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POWER_24B").State.Equals("ON") &&
                    Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_GLASS_HEATING").State.Equals("ON")) {
                    Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Indicator, "INDICATOR_GLASS_HEATING").State = "OFF";
                    Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Indicator, "INDICATOR_GLASS_HEATING").State = "ON";

                    Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_GLASS_HEATING").State = "OFF";
                    Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_GLASS_HEATING").State = "ON";
                }
                break;
            case "TUMBLER_POWER_28B":
                //////Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_STOWED").State =
                //////    control.State;

                //////if (control.State.Equals("ON") && Core.GetPanel("Strela10_OperatorPanel")
                //////        .GetControl(ControlType.Tumbler, "TUMBLER_POWER_24B").State.Equals("ON")) {
                //////    //////initialise in methon when matching finished
                //////    //////Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Spinner, "SPINNER_VOLTMETER").State =
                //////    //////    (int) 27;
                //////    Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_STOWED").State = "ON";
                //////    //////Strela10.Arms.ChargeProjectile();
                //////} else {
                //////    Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_STOWED").State = "OFF";
                //////    //////setDefaulValuesToIndications24V();
                //////    //////Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Spinner, "SPINNER_VOLTMETER").State =
                //////    //////    (int) 0;
                //////    //Debug.Log(String.Format("HIDE BOARD22222222222222222222222222222222222222"));
                //////}

                if (((string)control.State).Equals("OFF")) {
                    //////Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "LAUNCHER_1").State =
                    //////    control.State;
                    //////Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "LAUNCHER_2").State =
                    //////    control.State;
                    //////Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "LAUNCHER_3").State =
                    //////    control.State;
                    //////Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "LAUNCHER_4").State =
                    //////    control.State;
                    //////setDefaulValuesToIndications24V();
                    Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_BOARD").State =
                        "OFF";
                    vizirController.bort = false;
					vizirController.StopBortSound();
                    //vizirController.switchIndicatorOff();
                    //vizirController.stateOne = false;
                    //vizirController.bortFlag = false;
                    //vizirController.stateZeroBort();
                } else {
                    if (v24) {
                        //Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "CHECK").State = "ON";
                        //Debug.Log(Weaponry.name);
                        if (!Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_WORK_TYPE").State.Equals("AUTO")) {
                            vizirController.bort = false;
                            vizirController.bortFlag = false;
                            vizirController.StopBortSound();
                            Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_BOARD").State = "OFF";
                            Debug.Log("TMP -> Board off CoreHandler.....cs");
                        }
                        Strela10.Arms.ChargeProjectile();
                    }
                    //Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_WORK_TYPE")
                    //    .ControlChanged();
                }
                break;
            case "TUMBLER_POWER_30B":
                Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "CHECK").State =
                    control.State;
                if (Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POWER_30B").State
                        .Equals("OFF") && Core.GetPanel("Strela10_OperatorPanel")
                        .GetControl(ControlType.Tumbler, "TUMBLER_POWER_28B").State.Equals("OFF")) {
                    Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Spinner, "SPINNER_VOLTMETER").State =
                        (int)0;
                } else {
                    Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Spinner, "SPINNER_VOLTMETER").State =
                        (int)29;
                }

                break;
            case "TUMBLER_TEST":

                Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "NVU_MODE").State =
                    control.State;
                break;
            case "TUMBLER_AOZ":

                vizirController.aoz = control.State.Equals("ON");
                Core.GetPanel("Strela10_OperationalPanel").GetControl(ControlType.Indicator, "INDICATOR_AOZ").State = control.State;
                break;
            case "TUMBLER_ACU":

                //Debug.LogWarning("ACY press");
                //var target = (Strela10.Arms.Projectile as WeaponryRocket).Target;
                // Берем ID у текущей цели ракеты
                //int targetID = target.GetComponent<Weaponry>().ID;
                if (v24) {
                    MCSGlobalSimulation.CommandCenter.Execute(new MCSCommand(MCSCommandType.Weaponry, "Execute", false,
                        Strela10.ID,
                        "ACUBattle",
                        true, // Запустить слежение
                        -1)); // Айди текущей цели

                    //Core.GetPanel("Strela10_GuidancePanel").GetControl(ControlType.Tumbler, "TUMBLER_TRIGGER_DRIVE").State = "ON";

                    //TowerHandler.StartCoroutine("ACUBattle", target);
                    EnableNetworkSeeking(true);
                }
                break;
            case "TUMBLER_PSP":
                if (control.State.Equals("ON")) {
                    psp = true;
                    if (psp && pspState) {
                        WeaponryRocket_9M37.pspState = true;
                    } else {
                        WeaponryRocket_9M37.pspState = false;
                    }
                } else {
                    psp = false;
                    if (psp && pspState) {
                        WeaponryRocket_9M37.pspState = true;
                    } else {
                        WeaponryRocket_9M37.pspState = false;
                    }
                }
                break;
            case "TUMBLER_PSP_MODE":
                if (control.State.Equals("DP")) {
                    pspState = true;
                    if (psp && pspState) {
                        Debug.LogError("PSP true");
                        WeaponryRocket_9M37.pspState = true;
                    } else {
                        WeaponryRocket_9M37.pspState = false;
                    }
                } else {
                    pspState = false;
                    if (psp && pspState) {
                        WeaponryRocket_9M37.pspState = true;
                    } else {
                        WeaponryRocket_9M37.pspState = false;
                    }
                }

                break;
        }


        switch (control.GetName()) {
            case "TUMBLER_WORK_TYPE":
                if (v24) {
                    //Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "CHECK").State = "ON";
                    //Debug.Log(Weaponry.name);
                    if (!Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_WORK_TYPE").State.Equals("AUTO")) {
                        vizirController.bort = false;
                        vizirController.bortFlag = false;
						vizirController.StopBortSound();
                        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_BOARD").State = "OFF";
                        Debug.Log("TMP -> Board off CoreHandler.....cs");
                    }
                    Strela10.Arms.ChargeProjectile();
                }

                break;

            case "TUMBLER_FON":
          		if (control.State.Equals("STATE_3")){
					foreach (GameObject cloud in MCSGlobalSimulation.CloudPointsWhite) {
                        cloud.SetActive(false);
                    }
                    foreach (var cloud in MCSGlobalSimulation.Cloud) {
                        cloud.enabled = false;
                    }
                    foreach (GameObject cloud in MCSGlobalSimulation.CloudPointsGrey) {
                        cloud.SetActive(false);
                    }
				} else{
	                if (control.State.Equals("STATE_2")) {
	                    foreach (GameObject cloud in MCSGlobalSimulation.CloudPointsWhite) {
	                        cloud.SetActive(false);
	                    }
	                    foreach (var cloud in MCSGlobalSimulation.Cloud) {
	                        cloud.enabled = false;
	                    }
	                    foreach (GameObject cloud in MCSGlobalSimulation.CloudPointsGrey) {
	                        cloud.SetActive(true);
	                    }
	                } else {
	                    if (control.State.Equals("STATE_1")) {
	                        foreach (GameObject cloud in MCSGlobalSimulation.CloudPointsWhite) {
	                            cloud.SetActive(true);
	                        }
	                        foreach (var cloud in MCSGlobalSimulation.Cloud) {
	                            cloud.enabled = true;
	                        }
	                        foreach (GameObject cloud in MCSGlobalSimulation.CloudPointsGrey) {
	                            cloud.SetActive(true);
	                        }
	                    }
	                }
				}
                break;
            case "TUMBLER_MODE":
                switch (control.State.ToString()) {
                    case "COMBAT":
                        if (v24) {
                            Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_STOWED").State = "OFF";
                            Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_COMBAT").State = "ON";
                            Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_TRAINING").State = "OFF";
                            Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Indicator, "INDICATOR_U_AMPER")
                                .State = "OFF";
                            Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Indicator, "INDICATOR_U_115")
                                .State = "ON";
                            //Strela10.Arms.ChargeProjectile();
                        }
                        break;

                    case "TRAINING":
                        if (v24) {
                            Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_STOWED").State = "OFF";
                            Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_COMBAT").State = "OFF";
                            Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_TRAINING").State = "ON";
                        }
                        break;
                }
                return;
        }
    }

    public void PanelOperationalChanged(PanelControl control) {
        bool v24 = Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "VOLTMETER_BACKLIGHT")
            .State.Equals("ON");


        switch (control.GetName()) {
            case "TUMBLER_MODE":
                switch (control.State.ToString()) {
                    case "COMBAT":
                        if (v24) {
                            Core.GetPanel("Strela10_OperationalPanel").GetControl(ControlType.Indicator,
                                "INDICATOR_BATTLE").State = "ON";
                            Core.GetPanel("Strela10_OperationalPanel").GetControl(ControlType.Indicator,
                                "INDICATOR_TRAINING").State = "OFF";
                            Core.GetPanel("Strela10_OperationalPanel")
                                .GetControl(ControlType.Indicator, "INDICATOR_RADIATION").State = "ON";
                        } else {
                            Core.GetPanel("Strela10_OperationalPanel").GetControl(ControlType.Indicator,
                                "INDICATOR_BATTLE").State = "OFF";
                            Core.GetPanel("Strela10_OperationalPanel").GetControl(ControlType.Indicator,
                                "INDICATOR_TRAINING").State = "OFF";
                            Core.GetPanel("Strela10_OperationalPanel")
                                .GetControl(ControlType.Indicator, "INDICATOR_RADIATION").State = "OFF";
                        }
                        return;

                    case "TRAINING":
                        if (v24) {
                            Core.GetPanel("Strela10_OperationalPanel").GetControl(ControlType.Indicator,
                                "INDICATOR_BATTLE").State = "OFF";
                            Core.GetPanel("Strela10_OperationalPanel").GetControl(ControlType.Indicator,
                                "INDICATOR_TRAINING").State = "ON";
                            Core.GetPanel("Strela10_OperationalPanel")
                                .GetControl(ControlType.Indicator, "INDICATOR_RADIATION").State = "OFF";
                        } else {
                            Core.GetPanel("Strela10_OperationalPanel").GetControl(ControlType.Indicator,
                                "INDICATOR_BATTLE").State = "OFF";
                            Core.GetPanel("Strela10_OperationalPanel").GetControl(ControlType.Indicator,
                                "INDICATOR_TRAINING").State = "OFF";
                        }
                        return;
                }
                return;

            case "TUMBLER_AOZ":
                if (v24) {
                    Core.GetPanel("Strela10_OperationalPanel").GetControl(ControlType.Indicator, "INDICATOR_AOZ").State
                        = control.State;
                    Core.GetPanel("Strela10_OperationalPanel").GetControl(ControlType.Indicator,
                        "INDICATOR_TRAINING").State = control.State;
                    Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Indicator,
                        "INDICATOR_U_VENT").State = control.State;
                }
                return;
        }
    }

    public void ControlBlockChanged(PanelControl control) {
        switch (control.GetName()) {
            case "TUMBLER_STATUS":
                Core.GetPanel("Strela10_ControlBlock").GetControl(ControlType.Indicator, "INDICATOR_STATUS").State =
                    control.State;

                break;
            case "TUMBLER_MODE":
                if (control.State.Equals("CHECK_I")) {
                    Core.GetPanel("Strela10_AzimuthIndicator").GetControl(ControlType.Indicator, "INDICATOR_LEFT").State
                        =
                        "ON";
                    Core.GetPanel("Strela10_AzimuthIndicator").GetControl(ControlType.Indicator, "INDICATOR_RIGHT")
                            .State =
                        "ON";
                    Core.GetPanel("Strela10_AzimuthIndicator").GetControl(ControlType.Indicator, "INDICATOR_FORWARD")
                            .State =
                        "ON";
                } else if (control.State.Equals("WORK")) {
                    Core.GetPanel("Strela10_AzimuthIndicator").GetControl(ControlType.Indicator, "INDICATOR_LEFT").State
                        =
                        "OFF";
                    Core.GetPanel("Strela10_AzimuthIndicator").GetControl(ControlType.Indicator, "INDICATOR_RIGHT")
                            .State =
                        "OFF";
                    Core.GetPanel("Strela10_AzimuthIndicator").GetControl(ControlType.Indicator, "INDICATOR_FORWARD")
                            .State =
                        "OFF";
                } else {
                    if (control.State.Equals("CHECK_II")) {
                        Core.GetPanel("Strela10_AzimuthIndicator").GetControl(ControlType.Indicator, "INDICATOR_LEFT")
                                .State
                            =
                            "OFF";
                        Core.GetPanel("Strela10_AzimuthIndicator").GetControl(ControlType.Indicator, "INDICATOR_RIGHT")
                                .State =
                            "OFF";
                        Core.GetPanel("Strela10_AzimuthIndicator")
                                .GetControl(ControlType.Indicator, "INDICATOR_FORWARD").State =
                            "OFF";
                    }
                }
                break;
        }
    }

    public void SupportPanelChanged(PanelControl control) {
        ///ART
        Debug.Log(String.Format("BOARD off, because go to MARCHING!"));
        //Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_BOARD").State = "OFF";
        //vizirController.bort = false;
        //vizirController.StopBortSound();
       
        switch (control.GetName()) {
            case "TUMBLER_FAN":
                Debug.Log("Click to TUMBLER_FAN");
                if (Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_FAN").State.Equals("ON") || Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_FAN").State.Equals("OFF")) {
                    if (!vizirController.VizirActivated) {
                        vizirController.activateOfVizir();
                        Debug.Log("Click to TUMBLER_FAN -> activate");
                    } else {
                        vizirController.destroyOfVizir();
                        Debug.Log("Click to TUMBLER_FAN -> destroy");
                    }
                }         
                break;
            case "TUMBLER_GLASS_HEATING":
                if (Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POWER_24B").State.Equals("ON"))
                    Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Indicator, "INDICATOR_GLASS_HEATING").State = control.State;
                break;
			case "TUMBLER_TRACKING":
				if (Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_TRACKING").State.Equals("MANUAL")){
				
				} else if (Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_TRACKING").State.Equals("AUTO")){
					
				}
				break;
        }
    }

    #region Корутины для слежения за приборами

    IEnumerator TowerAzimutSeeker() {
        PanelControl azimuthWhite = Core.GetPanel("Strela10_AzimuthIndicator")
            .GetControl(ControlType.Spinner, "SPINNER_TOWERANGLE");
        (azimuthWhite as SpinnerToolkit).isCyclical = true;

        while (Strela10) {
            //Debug.LogWarning(Strela10.Tower.transform.localEulerAngles.y);
            azimuthWhite.State = (int) Strela10.Tower.transform.localEulerAngles.y;
            yield return new WaitForFixedUpdate();
        }
    }

    #endregion

    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) // Захват цели
        {
            try {
                var target = (Strela10.Arms.Projectile as WeaponryRocket).Target;
                // Берем ID у текущей цели ракеты
                int targetID = target.GetComponent<Weaponry>().ID;
                MCSGlobalSimulation.CommandCenter.Execute(new MCSCommand(MCSCommandType.Weaponry, "Execute", false,
                    Strela10.ID,
                    "AutoAimTarget",
                    true, // Запустить слежение
                    targetID)); // Айди текущей цели

                Core.GetPanel("Strela10_GuidancePanel").GetControl(ControlType.Tumbler, "TUMBLER_TRIGGER_DRIVE").State =
                    "ON";

                TowerHandler.StartCoroutine("AutoAimTarget", target);
                EnableNetworkSeeking(false);
            } catch {
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            //Strela10.Tower.GetComponent<NetworkInterpolatedRotation>().StreamStatus = BitStreamStatus.Read;
            //Strela10.Containers.GetComponent<NetworkInterpolatedRotation>().StreamStatus = BitStreamStatus.Read;

            MCSGlobalSimulation.CommandCenter.Execute(new MCSCommand(MCSCommandType.Weaponry, "Execute", false,
                Strela10.ID,
                "AutoAimTarget",
                false)); // Остановить слежение

            //Model.StopUCoroutine("autoAim");

            Core.GetPanel("Strela10_GuidancePanel").GetControl(ControlType.Tumbler, "TUMBLER_TRIGGER_DRIVE").State =
                "OFF";

            TowerHandler.StopCoroutine("AutoAimTarget");
            EnableNetworkSeeking(true);
        }
    }

    void EnableNetworkSeeking(bool value) {
        TowerHandler.Tower.GetComponent<NetworkInterpolatedRotation>().enabled = value;

        // Выравниваем контейнеры
        //Containers.transform.localEulerAngles = new Vector3(-83f, 0, 0);

        // Отключаем слежение за ракетами
        //foreach (WeaponryArms_Strela10_Launcher launcher in Strela10.Arms.Launchers)
        //{
        //    try
        //    {

        //        launcher.Projectile.GetComponent<NetworkInterpolatedTransform>().enabled = value;
        //    }
        //    catch
        //    {
        //        // Нет ракеты
        //    }
        //}
    }

    public static void ConvertorButton(String state) {
        CoreStatic.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_BOARD").State =
            state;
    }

    void ChangeAllElements() {
        Debug.LogError("ChangeAllElements()");
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_AOZ").ControlChanged();
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_FON").ControlChanged();
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_DRIVE_HANDLE_OFF").ControlChanged();
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_WORK_TYPE").ControlChanged();
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POWER_24B").ControlChanged();
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POWER_28B").ControlChanged();
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POWER_30B").ControlChanged();
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_MODE").ControlChanged();
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_PSP").ControlChanged();
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_PSP_MODE").ControlChanged();
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_DROP").ControlChanged();
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_ACU").ControlChanged();
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_BL").ControlChanged();
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_SS").ControlChanged();
        Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_TEST").ControlChanged();

        Core.GetPanel("Strela10_OperationalPanel").GetControl(ControlType.Tumbler, "TUMBLER_AOZ").ControlChanged();
        Core.GetPanel("Strela10_OperationalPanel").GetControl(ControlType.Tumbler, "TUMBLER_MODE").ControlChanged();

        Core.GetPanel("Strela10_ControlBlock").GetControl(ControlType.Tumbler, "TUMBLER_STATUS").ControlChanged();
        Core.GetPanel("Strela10_ControlBlock").GetControl(ControlType.Tumbler, "TUMBLER_MODE").ControlChanged();
        Core.GetPanel("Strela10_ControlBlock").GetControl(ControlType.Tumbler, "TUMBLER_STAGE_1").ControlChanged();
        Core.GetPanel("Strela10_ControlBlock").GetControl(ControlType.Tumbler, "TUMBLER_STAGE_2").ControlChanged();
        Core.GetPanel("Strela10_ControlBlock").GetControl(ControlType.Tumbler, "TUMBLER_STAGE_3").ControlChanged();
        Core.GetPanel("Strela10_ControlBlock").GetControl(ControlType.Tumbler, "TUMBLER_STAGE_4").ControlChanged();

        Core.GetPanel("Strela10_AzimuthIndicator").GetControl(ControlType.Tumbler, "TUMBLER_BACKLIGHT").ControlChanged();
        Core.GetPanel("Strela10_AzimuthIndicator").GetControl(ControlType.Tumbler, "TUMBLER_C").ControlChanged();

        Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_GLASS_HEATING").ControlChanged();
        Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_LIGHT").ControlChanged();
        Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_FAN").ControlChanged();
        Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_POSITION").ControlChanged();
        Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_CLEANER").ControlChanged();
        Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_TRACKING").ControlChanged();

        Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_COOL").ControlChanged();
        Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_BOARD").ControlChanged();
        Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_TRACK_LAUNCH").ControlChanged();
        Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_TRIGGER_DRIVE").ControlChanged();

        Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Tumbler, "TUMBLER_ILLUM").ControlChanged();
        Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POSITION_WORK_TYPE").ControlChanged();
        Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POWER_NRZ").ControlChanged();
        Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Tumbler, "TUMBLER_WORK_NRZ").ControlChanged();
        Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Tumbler, "TUMBLER_VV").ControlChanged();
        Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Tumbler, "TUMBLER_СС").ControlChanged();
        Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Tumbler, "TUMBLER_СС2").ControlChanged();
        Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Tumbler, "TUMBLER_LOSE").ControlChanged();
        Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Tumbler, "TUMBLER_CODE_POSITION").ControlChanged();
        Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Tumbler, "TUMBLER_FILTER_UP").ControlChanged();
        Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Tumbler, "TUMBLER_FILTER_DOWN").ControlChanged();
        Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Tumbler, "TUMBLER_VZ").ControlChanged();
        Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Tumbler, "TUMBLER_SD").ControlChanged();
        Core.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Tumbler, "TUMBLER_YPCH").ControlChanged();

        Core.GetPanel("Strela10_ARC").GetControl(ControlType.Tumbler, "TUMBLER_A3").ControlChanged();

        Core.GetPanel("Strela10_SoundPanel").GetControl(ControlType.Tumbler, "Indicator").ControlChanged();

        Core.GetPanel("Strela10_CommonPanel").GetControl(ControlType.Tumbler, "TUMBLER_PEDAL").ControlChanged();
        Core.GetPanel("Strela10_CommonPanel").GetControl(ControlType.Tumbler, "HANDL").ControlChanged();
        Core.GetPanel("Strela10_CommonPanel").GetControl(ControlType.Tumbler, "PEDAL_AZIM").ControlChanged();
        Core.GetPanel("Strela10_CommonPanel").GetControl(ControlType.Tumbler, "TUMBLER_STOP").ControlChanged();
    }
}