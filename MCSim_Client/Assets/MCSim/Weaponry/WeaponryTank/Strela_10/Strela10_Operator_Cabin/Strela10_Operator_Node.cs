using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EasyModbus;
using MilitaryCombatSimulator;
using UnityEngine;

public class Strela10_Operator_Node : CrewNode {
    /// <summary>
    /// Хост-Weaponry данной кабины
    /// </summary>
    public WeaponryTank_Strela10 Host;

    /// <summary>
    /// Ядро
    /// </summary>
    private CoreToolkit _core;

    public CoreToolkit Core {
        get {
            if (!_core) {
                _core = Host.Core["Strela-10_Operator"] as CoreToolkit;
            }

            return _core;
        }

        set { _core = value; }
    }

    public Camera Camera;

    public ModbusClient modbusClient;

    private bool isPhysicalJoysticEnabled = true;
    private bool isLaunchPressEmitted;
    private PanelControl controlPosition;

    private void WreateFileControlByName() {
        StringBuilder sb = new StringBuilder();

        foreach (var panel in Core.Panels) {
            sb.AppendLine(panel.GetName());

            sb.AppendLine(ControlType.Tumbler.ToString());
            foreach (var item in panel.GetTumblers()) {
                sb.AppendLine(new string('\t', 2) + item.GetName());
                foreach (var name in item.GetStatesList()) {
                    sb.AppendLine(new string('\t', 3) + name);
                }
            }

            sb.AppendLine("\t" + ControlType.Indicator.ToString());
            foreach (var item in panel.GetIndicators()) {
                sb.AppendLine(new string('\t', 2) + item.GetName());
                foreach (var name in item.GetStatesList()) {
                    sb.AppendLine(new string('\t', 3) + name);
                }
            }

            sb.AppendLine(ControlType.Joystick.ToString());
            foreach (var item in panel.GetJoysticks()) {
                sb.AppendLine(new string('\t', 2) + item.GetName());
            }

            sb.AppendLine(ControlType.Spinner.ToString());
            foreach (var item in panel.GetSpinners()) {
                sb.AppendLine(new string('\t', 2) + item.GetName());
            }
        }

        File.WriteAllText(@"C:\Users\Алексей\Desktop\State\InfoPorts.txt", sb.ToString());
    }

    void Start() {
        //WreateFileControlByName();

        if (Connect()) {
            InitEventControl();
            ///StartCoroutine(RegistersUpdate(ReadRegistersBit, 0.2f));
            StartCoroutine(RegistersUpdate(ReadRegistersBit, 0.02f));
        }

        controlPosition = Host.Core["Strela-10_Operator"].GetPanel("Strela10_SupportPanel").GetControl(
            ControlType.Tumbler, "TUMBLER_POSITION");
    }
    
    private void ReadRegistersFloat() {
        if (isPhysicalJoysticEnabled) {
            var reg = modbusClient.ReadHoldingRegisters(4, 2);
            var horVal = ModbusClient.ConvertRegistersToFloat(reg) * (maxC / 152);
            if (horVal > 2020 && horVal < 2100) {
                horVal = 2000;
            }
            if (ChangedJoystickHorizontal != null) {
                ChangedJoystickHorizontal.Invoke(new ValueEventArgs(horVal));
                //TestJoystickVertical(ModbusClient.ConvertRegistersToFloat(reg));
                Debug.Log("horVal = " + horVal);
            }

            reg = modbusClient.ReadHoldingRegisters(6, 2);
            var vertVal = ModbusClient.ConvertRegistersToFloat(reg) * (maxC / 60);
            if (ChangedJoystickVertical != null) {
                ChangedJoystickVertical.Invoke(new ValueEventArgs(vertVal));
                //TestJoystickVertical(ModbusClient.ConvertRegistersToFloat(reg));
                Debug.Log("vertVal = " + vertVal);
            }
        }
    }

    int centerC = 360;
    int minC = 0;
    int maxC = 4000;
    float k = 56.2f;
    float p = 79.1f;

    private void TestJoystickVertical(float value) {
        Debug.Log("Value in Node = " + value);

        float angle = 0;
        float clamp = 0;
        if ((int) value <= centerC - 60) {
            angle = (value - centerC) / k;
            //Debug.LogError("angle = " + angle);
            //Debug.LogError("value = " + value);
            clamp = Mathf.Clamp(-angle, 0, Host.TowerRotationSpeed.Y * Time.fixedDeltaTime);
            //Debug.LogError("clamp = " + clamp);
        } else if ((int) value > centerC + 120) {
            angle = (value + centerC) / k;
            //Debug.LogError(">angle = " + angle);
            //Debug.LogError(">value = " + value);
            clamp = Mathf.Clamp(0, angle, Host.TowerRotationSpeed.Y * Time.fixedDeltaTime);
            //Debug.LogError(">clamp = " + clamp);
        }
        //Vector3 point = new Vector3(value, 0, 0);

        //var vec = point + Host.TowerHandler.Containers.transform.position;

        //Vector3 localDirection =
        //        Host.TowerHandler.Containers.transform.InverseTransformDirection(vec);
        //localDirection = new Vector3(0, localDirection.y, localDirection.z);

        ////Debug.DrawRay(Containers.transform.position, Containers.transform.TransformDirection(localDirection),
        ////              Color.yellow);

        //float angleContainers = Vector3.Angle(Host.TowerHandler.Containers.transform.forward,
        //                                Host.TowerHandler.Containers.transform.TransformDirection(localDirection));


        //float clamp = Host.TowerHandler.ClampTowerAngle(Host.TowerHandler.Containers.transform.localEulerAngles.x + Mathf.Clamp(angleContainers, 0, Host.TowerRotationSpeed.Y * Time.fixedDeltaTime) *
        //            -Mathf.Sign(localDirection.y));

        ////if (clamp >= Host.TowerLimitAngle.Y.Min && clamp <= Host.TowerLimitAngle.Y.Max)
        ////{
        //    Host.TowerHandler.Containers.transform.localEulerAngles = Vector3.Lerp(Host.TowerHandler.Containers.transform.localEulerAngles,
        //                                                         new Vector3(
        //                                                             Host.TowerHandler.Containers.transform.localEulerAngles.x, angle),// *-Mathf.Sign(localDirection.y)
        //                                                         0.25f);
        ////}

        Host.TowerHandler.Containers.transform.localEulerAngles =
            Vector3.Lerp(Host.TowerHandler.Containers.transform.localEulerAngles, new Vector3(clamp, 0, 0), 0.25f);
    }

    //private void TestJoystickVertical(float value)
    //{

    //}

    IEnumerator RegistersUpdate(Action callback, float time) {
        while (Connect()) {
            try {
                callback();
            } catch {
            }
            yield return new WaitForSeconds(time);
        }
    }

    private void InitEventControl() {
        foreach (var panel in Core.Panels) {
            if (panel.GetName() == "Strela10_OperatorPanel") {
                var info = ConfigPorts.GetInfoPOPorts();

                foreach (var item in panel.SwitcherScripts) {
                    var port = info.FirstOrDefault(c => c.Name == item.GetName());
                    if (port != null) {
                        if (item.GetName().Equals("TUMBLER_FON")) {
                            ChangedFon += item.OnOnChangedValue;
                        } else {
                            item.PortsList = port.Ports.ToList();
                            ChangedSwitcherState += item.OnChangedState;
                        }
                    }
                }

                info = ConfigPorts.GetInfoPOLight();

                foreach (var item in panel.IndicatorScripts) {
                    var port = info.FirstOrDefault(c => c.Name == item.GetName());
                    if (port != null) {
                        item.Port = port.Ports[0];
                        item.ChangedIndicatorState += OnWreateRegisters;
                    }
                }

                info = ConfigPorts.GetInfoPOSpinner();

                foreach (var item in panel.SpinnerScripts) {
                    var port = info.FirstOrDefault(c => c.Name == item.GetName());
                    if (port != null) {
                        item.Port = port.Ports[0];
                        item.ChangedSpinnerState += OnWreateRegisters;
                    }
                }
            }

            if (panel.GetName() == "Strela10_SupportPanel") {
                var info = ConfigPorts.GetInfoPO2Ports();

                foreach (var item in panel.SwitcherScripts) {
                    var port = info.FirstOrDefault(c => c.Name == item.GetName());
                    if (port != null) {
                        item.PortsList = port.Ports.ToList();
                        ChangedSwitcherState += item.OnChangedState;
                    }
                }

                info = ConfigPorts.GetInfoPO2Light();
                foreach (var item in panel.IndicatorScripts) {
                    var port = info.FirstOrDefault(c => c.Name == item.GetName());
                    if (port != null) {
                        item.Port = port.Ports[0];
                        item.ChangedIndicatorState += OnWreateRegisters;
                    }
                }
            }

            if (panel.GetName() == "Strela10_CommonPanel") {
                var info = ConfigPorts.GetInfoCommonPorts();

                foreach (var item in panel.SwitcherScripts) {
                    var port = info.FirstOrDefault(c => c.Name == item.GetName());
                    if (port != null) {
                        item.PortsList = port.Ports.ToList();
                        ChangedSwitcherState += item.OnChangedState;
                        Debug.Log(port.Name);
                    }
                }
            }

            if (panel.GetName() == "Strela10_GuidancePanel") {
                var info = ConfigPorts.GetInfoPNPorts();

                foreach (var item in panel.SwitcherScripts) {
                    var port = info.FirstOrDefault(c => c.Name == item.GetName());
                    if (port != null) {
                        item.PortsList = port.Ports.ToList();
                        ChangedSwitcherState += item.OnChangedState;
                    }
                }
            }
        }
    }

    private void OnWreateRegisters(StateEventArgs arg) {
        if (Connect()) {
            modbusClient.WriteSingleCoil(arg.Port, arg.State);
        }
    }

    public const int CountRegisters = 24;

    private bool Connect() {
        if (modbusClient == null) {
            modbusClient = new ModbusClient("10.0.6.10", 502);
        }

        if (!modbusClient.Connected) {
            try {
                modbusClient.Connect();
            } catch {
                return false;
            }
        }
        return true;
    }

    public event Action<StateEventArgs> ChangedSwitcherState;


    private static bool[] readCoils;

    private const int OLD_REGISTERS = 24;
    private const int VIZIR_BUTTON = 33;

    private bool ShouldRead(int i) {
        return i < OLD_REGISTERS || i == VIZIR_BUTTON;
    }

    private void ReadRegistersBit() {
        //if (Connect())
        //{
        bool[] newReadCoils = modbusClient.ReadDiscreteInputs(0, CountRegisters);

        if (readCoils == null) {
            readCoils = new bool[CountRegisters];
            for (int i = 0; i < newReadCoils.Length; i++) {
                if (ChangedSwitcherState != null && ShouldRead(i))
                    ChangedSwitcherState.Invoke(new StateEventArgs(newReadCoils[i] ? i + 1 : -(i + 1),
                        newReadCoils[i]));
                readCoils[i] = newReadCoils[i];
            }
        } else {
            for (int i = 0; i < newReadCoils.Length; i++) {
                if (readCoils[i] != newReadCoils[i]) {
                    if (ChangedSwitcherState != null && ShouldRead(i))
                        ChangedSwitcherState.Invoke(new StateEventArgs(newReadCoils[i] ? i + 1 : -(i + 1),
                            newReadCoils[i]));
                    readCoils[i] = newReadCoils[i];
                }
            }
        }

        var reg = modbusClient.ReadHoldingRegisters(8, 2);
        if (ChangedFon != null) {
            var fonVal = ModbusClient.ConvertRegistersToFloat(reg);
            ChangedFon.Invoke(new ValueEventArgs(fonVal));
            if (fonVal > 7f) {
                Debug.Log("FON: FON 3");
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
                if (fonVal >= 2f && fonVal <= 5.5f) {
                    Debug.Log("FON: FON 2");
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
                    if (fonVal <= 1f) {
                        Debug.Log("FON: FON 1");
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
            Debug.Log("fonVal = " + fonVal);
        }

        //StringBuilder sb = new StringBuilder();
        //for (int i = 0; i < readCoils.Length; i++)
        //{
        //    sb.Append(string.Format("Value Coil {0} {1}", i + 1, readCoils[i]));
        //}

        //Debug.Log(sb.ToString());

        //var panel = Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POWER_24B").GetComponent<SwitcherToolkit>();
        //var flag = readCoils[5] ? "ON" : "OFF";
        //if (panel.State != flag)
        //{
        //    panel.State = flag;
        //    panel.ControlChanged();
        //}

        //panel = Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POWER_28B").GetComponent<SwitcherToolkit>();
        //flag = readCoils[9] ? 1 : 0;
        //if (panel.TumblerStateID != flag)
        //{
        //    panel.TumblerStateID = flag;
        //    panel.ControlChanged();
        //}


        //panel = Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_WORK_TYPE").GetComponent<SwitcherToolkit>();

        //for (int i = 0; i < 5; i++)
        //{
        //    if(readCoils[i])
        //    {
        //        if (panel.TumblerStateID != i)
        //        {
        //            panel.TumblerStateID = i;
        //            panel.ControlChanged();
        //        }
        //        break;
        //    }
        //}
        //}
    }

    public event Action<ValueEventArgs> ChangedJoystickHorizontal;
    public event Action<ValueEventArgs> ChangedJoystickVertical;

    public event Action<ValueEventArgs> ChangedFon;

    private void MonitorControls() {
        List<PanelControl> controls = new List<PanelControl>();

        try {
            var joystick = Core.GetPanel("Strela10_GuidancePanel")
                .GetControl(ControlType.Joystick, "OperatorJoystickHorizontal");
            ChangedJoystickHorizontal += joystick.GetComponent<JoystickToolkit>().OnChangedState;
            controls.Add(joystick);

            joystick = Core.GetPanel("Strela10_GuidancePanel")
                .GetControl(ControlType.Joystick, "OperatorJoystickVertical");
            ChangedJoystickVertical += joystick.GetComponent<JoystickToolkit>().OnChangedState;
            controls.Add(joystick);
        } catch {
            Debug.LogError("Ошибка при загрузке ядра.");
        }

        Debug.Log("ID Host: " + Host.ID);

        foreach (var panelControl in controls) {
            MCSGlobalSimulation.CommandCenter.ControlMonitor(Host.ID, panelControl);
        }

        if (Connect()) {
            ///StartCoroutine(RegistersUpdate(ReadRegistersFloat, 0.1f));
            StartCoroutine(RegistersUpdate(ReadRegistersFloat, 0.01f));
        }
    }

    private bool flag = true;
    // Update is called once per frame

    private int launcher_active = 0;

    private float GetHorizontalJoystickMultiplierKeyboard() {
        if (controlPosition.State.Equals("BATTLE")) {
            return 1;
        }
        float towerHorizontalAngle = GetTowerHorizontalAngle();
        float towerHorizontalAngleClamp = towerHorizontalAngle > 358 ? 360 - towerHorizontalAngle : towerHorizontalAngle;
        if (towerHorizontalAngleClamp <= 2) {
            return 0;
        } else {
            return 1;
        }
    }

    private float GetTowerHorizontalAngle() {
        return Host.Tower.transform.localEulerAngles.y;
    }

    void Update() {
        //Connect();
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            isPhysicalJoysticEnabled = !isPhysicalJoysticEnabled;
            Debug.Log("PhysicalJoysticEnabled: " + isPhysicalJoysticEnabled);
        }
        
        var joystick = Core.GetPanel("Strela10_GuidancePanel")
            .GetControl(ControlType.Joystick, "OperatorJoystickVertical");
        var joystickVertical = joystick.GetComponent<JoystickToolkit>();

        joystick = Core.GetPanel("Strela10_GuidancePanel")
            .GetControl(ControlType.Joystick, "OperatorJoystickHorizontal");
        var joystickHorizontal = joystick.GetComponent<JoystickToolkit>();

        if (Input.GetKey(KeyCode.UpArrow)) {
            joystickVertical.State = new int[] {4, 0, 0};
        } else if (Input.GetKey(KeyCode.DownArrow)) {
            joystickVertical.State = new int[] {-4, 0, 0};
        } else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)) {
            joystickVertical.State = new int[] {0, 0, 0};
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            joystickHorizontal.State = new[] {(int) joystickHorizontal._state.X.Shift, (int) (-4 * GetHorizontalJoystickMultiplierKeyboard()), 0};
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            joystickHorizontal.State = new int[] { (int)joystickHorizontal._state.X.Shift, (int) (4 * GetHorizontalJoystickMultiplierKeyboard()), 0 };
        } else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) {
            joystickHorizontal.State = new int[] {(int) joystickHorizontal._state.X.Shift, 0, 0};
        }

        if (IsLaunchPressed() && Host.IsTargetInsideLaunchZone) {
            if (CheckNRZTargetBelonging()) {
                //bool isCombat = Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_MODE").State.Equals("COMBAT");
                //Debug.Log("METHOD Update() in Strela10_operatorNode.");
                //if (isCombat) {
                    Debug.LogWarning("SHOTT");
                    if (((WeaponryRocket)Host.Arms.Projectile).Target) Host.Arms.Shoot();
                //}
            } else {
                Debug.LogError("No shot! Beloging a plane is my, its my live target");
            }


            //Random.Range(0, Host.Arms.Launchers.Length);
            // Launchers[launcher_active].Shoot();
            //Host.Arms.Launchers[launcher_active].Reload();
            //launcher_active++;
            //if (launcher_active == 4) launcher_active = 0;
        }
    }

    private bool IsLaunchPressed() {
        bool isLauchPressed = 
            Core.GetPanel("Strela10_GuidancePanel")
                .GetControl(ControlType.Tumbler, "TUMBLER_TRACK_LAUNCH").State.Equals("LAUNCH")
            || Input.GetKeyDown(KeyCode.Space);

        if (isLauchPressed && !isLaunchPressEmitted) {
            isLaunchPressEmitted = true;
            return true;
        }

        if (!isLauchPressed) {
            isLaunchPressEmitted = false;
        }
        return false;
    }

    public override void Initialize() {
        MonitorControls();
        Host.Arms.ChargeProjectile();
        IndicatorsOff();
    }

    /// <summary>
    /// Отключение всех индикаторов панели
    /// </summary>
    public void IndicatorsOff() {
        foreach (ControlPanelToolkit panel in Core.Panels) {
            try {
                foreach (IndicatorToolkit indicatorScript in panel.IndicatorScripts) {
                    indicatorScript.IndicatorStateID = 0;
                }
            } catch (Exception e) {
                Debug.Log("Ошибка при управлении индикатором на панели [" + panel.GetName() + "]: " + e.Message);
            }
        }
    }
	
	    public bool CheckNRZTargetBelonging() {
        //Debug.LogError("Check NRZ");
        if (Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_BL").State.Equals("ON")) {
            if (Strela10_Arms.VeiwIDTargetRussia != null) {
                if (((WeaponryRocket) Host.Arms.Projectile).Target.networkView.viewID.Equals(Strela10_Arms.VeiwIDTargetRussia)) {
                    Debug.LogError("false;");
                    return false;
                }
            }
        }
        return true;
    }

//    public bool CheckNRZTargetBelonging() {
//        if (Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_VizorPanel")
//            .GetControl(ControlType.Indicator, "INDICATOR_U_NRZ_POWER").State.Equals("ON")) {
//            if (Strela10_Arms.VeiwIDTargetRussia != null) {
//                if (((WeaponryRocket) Host.Arms.Projectile).Target.networkView.viewID.Equals(Strela10_Arms
//                    .VeiwIDTargetRussia)) {
//                    Debug.LogError("false;");
//                    return false;
//                }
//            }
//        }
//        return true;
//    }
}

public class ConfigPorts {
    public static InfoPorts GetInfoPOPorts() {
        var info = new InfoPorts();

        info.Add(new Port("TUMBLER_POWER_24B", new int[] {-7, 7}));
        info.Add(new Port("TUMBLER_POWER_28B", new int[] {-6, 6}));
        info.Add(new Port("TUMBLER_POWER_30B", new int[] {-24, 24}));
        info.Add(new Port("TUMBLER_WORK_TYPE", new int[] {1, 2, 3, 4, 5}));
        info.Add(new Port("TUMBLER_AOZ", new int[] {10, -10}));
        info.Add(new Port("TUMBLER_DRIVE_HANDLE_OFF", new int[] {9, 8, -8}));
        info.Add(new Port("TUMBLER_MODE", new int[] {23, -23}));
        info.Add(new Port("TUMBLER_FON", new int[] {8}));
        return info;
    }

    public static InfoPorts GetInfoPOLight() {
        var info = new InfoPorts();

        info.Add(new Port("LAUNCHER_1", new int[] {40}));
        info.Add(new Port("LAUNCHER_2", new int[] {41}));
        info.Add(new Port("LAUNCHER_3", new int[] {42}));
        info.Add(new Port("LAUNCHER_4", new int[] {43}));

        info.Add(new Port("VOLTMETER_BACKLIGHT", new int[] {51}));
        //info.Add(new Port("INDICATOR_COMBAT", new int[] { 51 }));
        //info.Add(new Port("INDICATOR_TRAINING", new int[] { 51 }));
        info.Add(new Port("INDICATOR_BOARD", new int[] {48}));
        info.Add(new Port("PZ_OHL", new int[] {53}));
        info.Add(new Port("CHECK", new int[] {53}));
        info.Add(new Port("INDICATOR_STOWED", new int[] {49}));

        return info;
    }

    public static InfoPorts GetInfoPOSpinner() {
        var info = new InfoPorts();

        info.Add(new Port("SPINNER_VOLTMETER", new int[] {50}));

        return info;
    }

    public static InfoPorts GetInfoPO2Ports() {
        var info = new InfoPorts();

        info.Add(new Port("TUMBLER_POSITION", new int[] {12, 11}));
        info.Add(new Port("TUMBLER_GLASS_HEATING", new int[] {-15, 15}));
        info.Add(new Port("TUMBLER_LIGHT", new int[] {-16, 16}));
        info.Add(new Port("TUMBLER_FAN", new int[] {-14, 14}));
        info.Add(new Port("TUMBLER_TRACKING", new int[] {13, -13}));

        return info;
    }

    public static InfoPorts GetInfoPO2Light() {
        var info = new InfoPorts();

        info.Add(new Port("INDICATOR_GLASS_HEATING", new int[] {52}));

        return info;
    }

    public static InfoPorts GetInfoPNPorts() {
        var info = new InfoPorts();

        info.Add(new Port("TUMBLER_BOARD", new int[] {-17, 17}));
        info.Add(new Port("TUMBLER_TRACK_LAUNCH", new int[] {-18, 18, 19}));
        info.Add(new Port("TUMBLER_TRIGGER_DRIVE", new int[] {-20, 20}));
        info.Add(new Port("TUMBLER_COOL", new int[] {-21, 21}));

        return info;
    }

    public static InfoPorts GetInfoCommonPorts() {
        var info = new InfoPorts();

        info.Add(new Port("PEDAL_AZIM", new int[] {-22, 22}));

        return info;
    }

}

public class InfoPorts : List<Port> {
    public InfoPorts() {
    }
}

public class Port {
    public string Name { get; set; }
    public int[] Ports { get; set; }

    public Port(string name, int[] port) {
        Name = name;
        Ports = port;
    }
}