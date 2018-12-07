using System;
using System.Collections;
using MilitaryCombatSimulator;
using UnityEngine;

public class Strela10_TowerHandler : MonoBehaviour {
    public delegate void TargetAngleChanged(float angle);

    public event TargetAngleChanged OnTargetAngleChanged;

    public void CallTargetAngleChanged(float angle) {
        var handler = OnTargetAngleChanged;

        if (handler != null)
            OnTargetAngleChanged(angle);
    }

    public Strela10_Operator_CoreHandler Handler;

    public WeaponryTank_Strela10 Strela10;
    public GameObject Tower, Containers;
    private CoreLibrary.Core Core;

    public GameObject[] TowerDetails;
    public GameObject RotationPivot;

    public float TowerAngle { get; protected set; }

    private PanelControl _controlPosition,
        _indicatorStowed,
        _control24B,
        _control28B,
        _controlDrive,
        _controlStowedAzim,
        _controlPedalAzim;

    public enum WorkMode {
        Marching = 0,
        Combat = 1
    }

    private Transform _target;

    void Update() {
        TowerAngle = ClampTowerAngle(Strela10.Containers.transform.localEulerAngles.x);
        if (_target == null) {
            foreach (var weaponry in MCSGlobalSimulation.Weapons.List.Values) {
                if (weaponry is WeaponryPlane) {
                    if (MCSTrainingCenter.initTrain) {
                        _target = weaponry.transform;
                    } else {
                        if (!weaponry.name.Contains("Cloud") && !weaponry.name.Contains("War")) {
                            _target = weaponry.transform;
                        }
                    }
                }
            }
        }

        if (_target) {
            var vector =
                Tower.transform.InverseTransformDirection(_target.transform.position - Tower.transform.position);
            vector.y = 0;
            float sign = Mathf.Sign(vector.x);
            vector = Tower.transform.TransformDirection(vector);

            //Debug.DrawLine(Tower.transform.position, Tower.transform.position + Tower.transform.forward * 100f, Color.blue);
            //Debug.DrawLine(Tower.transform.position, Tower.transform.position + vector, Color.red);
            float x = Vector3.Angle(Tower.transform.forward, vector) * sign;
            //print(_target.name + ": " + x);
            CallTargetAngleChanged(x < 0 ? 360 + x : x);
        }
    }

    /// <summary>
    /// Текущее положение башни
    /// </summary>
    public WorkMode currentMode = WorkMode.Combat;

    // Определяет можно ли управлять башней в даный момент
    private bool stowed = false;

    // 
    private bool towerEvolution = false;

    public void OnSystemReady() {
        Core = Handler.Core;
        Strela10 = Handler.Strela10;

        Handler.ControlChangeCallEvent += OnControlChanged;

        _controlPosition = Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_SupportPanel").GetControl(
            ControlType.Tumbler, "TUMBLER_POSITION");
        _control24B = Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
            ControlType.Tumbler, "TUMBLER_POWER_24B");
        _control28B = Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
            ControlType.Tumbler, "TUMBLER_POWER_28B");
        _controlDrive = Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
            ControlType.Tumbler, "TUMBLER_DRIVE_HANDLE_OFF");

        foreach (var t in Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_CommonPanel").PanelGameObjects.tumblers) {
            Debug.Log(t.name);
        }

        _controlStowedAzim = Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_CommonPanel").GetControl(
            ControlType.Tumbler, "TUMBLER_STOP");
        _controlPedalAzim = Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_CommonPanel").GetControl(
            ControlType.Tumbler, "PEDAL_AZIM");
    }

    void OnControlChanged(PanelControl control) {
        // Переход в боевое
        if (_control24B.State.Equals("ON") && _control28B.State.Equals("ON") && _controlDrive.State.Equals("DRIVE") &&
            _controlPosition.State.Equals("BATTLE") && _controlStowedAzim.State.Equals("OFF") &&
            _controlPedalAzim.State.Equals("ON")) {
            MCSGlobalSimulation.CommandCenter.Execute(new MCSCommand(MCSCommandType.Weaponry, "Execute", true,
                Strela10.ID, "ChangeTowerState",
                (int) WorkMode.Combat, true));

            // Если еще нажата педаль стопора
            SwitchWorkMode(WorkMode.Combat, true);
        }

        // Если сработал тумблер переключения ПОХОДНЫЙ-БОЕВОЙ
        if (control.Equals(_controlPosition)) {
            switch (control.State.ToString()) {
                case "BATTLE":
                    // Отключаем режим слежения за переходом в походное положение
                    Model.StopUCoroutine("SeekForStowing");
                    break;

                case "STOWED":
                    // Включаем режим слежения за поворотом башни, чтобы при необходимости перевести в походный
                    Model.UCoroutine(this, SeekForStowing(), "SeekForStowing");
                    break;
            }
        }
    }


    /// <summary>
    /// Слежение за положением контейнеров для перевода их в походное положение
    /// </summary>
    /// <returns></returns>
    public IEnumerator SeekForStowing() {
        while (true) {
            if (!towerEvolution) {
                // Если башня изменяет свое состояние, не следим за поворотом

                //Debug.Log("Container " + ClampTowerAngle(Strela10.Containers.transform.localEulerAngles.x));
                // Если контейнера установлены на 83 по вертикали
                if (ClampTowerAngle(Strela10.Containers.transform.localEulerAngles.x) >= 82.2f) {
                    // Проверяем направление башни, если она сонаправлена кузову
                    //Debug.Log("Tower " + Vector3.Angle(Strela10.transform.forward, Tower.transform.forward));
                    if (Vector3.Angle(Strela10.transform.forward, Tower.transform.forward) <= 1f) {
                        MCSGlobalSimulation.CommandCenter.Execute(new MCSCommand(MCSCommandType.Weaponry, "Execute",
                            true,
                            Strela10.ID, "ChangeTowerState",
                            (int) WorkMode.Marching, true));
                        SwitchWorkMode(WorkMode.Marching, true);
                        break;
                    }
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Переход башни между режимами.       
    /// </summary>
    /// <param name="mode">Режим</param>
    /// <param name="showAnimation">Отображать ли аниацию</param>
    public void SwitchWorkMode(WorkMode mode, bool showAnimation) {
        if (towerEvolution) return; // Если уже изменяем положение
        if (mode == currentMode) return;

        StartCoroutine(EnumeratorSwitchWorkMode(mode, showAnimation));
    }

    private IEnumerator EnumeratorSwitchWorkMode(WorkMode mode, bool showAnimation) {
        // Скорость поворота
        float rotationSpeed = Time.fixedDeltaTime * 15f;

        switch (mode) {
            case WorkMode.Marching:
                towerEvolution = true;

                Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
                    ControlType.Indicator, "VOLTMETER_BACKLIGHT").State = "OFF";
                Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
                    ControlType.Indicator, "PZ_OHL").State = "OFF";
                Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
                    ControlType.Indicator, "CHECK").State = "OFF";
                Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "LAUNCHER_1").State = "OFF";
                Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "LAUNCHER_2").State = "OFF";
                Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "LAUNCHER_3").State = "OFF";
                Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "LAUNCHER_4").State = "OFF";
				Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_BL").State = "OFF";
				Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_COMBAT").State = "OFF";
	       	 	Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_TRAINING").State = "OFF";
	        	Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_BOARD").State = "OFF";
				Core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_I").State = "OFF";
				Strela10.Arms.Host.TowerHandler.Handler.vizirController.bort = false;
				Strela10.Arms.Host.TowerHandler.Handler.vizirController.bortFlag = false;
				Strela10.Arms.Host.TowerHandler.Handler.vizirController.StopNRZSound();
				Strela10.Arms.Host.TowerHandler.Handler.vizirController.StopBortSound();
			
                // Отключаем синхронизацию поворота, чтобы башня не крутилась на месте, т.к. мы отслеживаем только ее поворот
                Containers.GetComponent<NetworkInterpolatedRotation>().enabled = false;

                // Центрируем
                Tower.transform.localEulerAngles = Vector3.zero;
                Tower.GetComponent<NetworkInterpolatedRotation>().enabled = false;

                // Выравниваем контейнеры
                Containers.transform.localEulerAngles = new Vector3(-83f, 0, 0);

                // Отключаем слежение за ракетами
                foreach (WeaponryArms_Strela10_Launcher launcher in Strela10.Arms.Launchers) {
                    try {
                        launcher.Projectile.GetComponent<NetworkInterpolatedTransform>().enabled = false;
                    } catch {
                        // Нет ракеты
                    }
                }

                while (true) {
                    foreach (GameObject towerDetail in TowerDetails) {
                        towerDetail.transform.RotateAround(RotationPivot.transform.position,
                            RotationPivot.transform.right, -rotationSpeed);
                    }

                    if (Containers.transform.localRotation.eulerAngles.x < rotationSpeed) {
                        currentMode = WorkMode.Marching;
                        break;
                    }

                    if (showAnimation) // Если показываем анимацию
                        yield return new WaitForFixedUpdate();
                }

                // Включаем лампу ПОХОД и отключаем подсветку ВОЛЬТМЕТРА
                if (Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POWER_24B").State.Equals("OFF") &&
                    Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POWER_28B").State.Equals("OFF")) {
                    Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_STOWED").State = "OFF";
                } else if (Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POWER_24B").State.Equals("ON") &&
                    Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_POWER_28B").State.Equals("ON") && 
					Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_DRIVE_HANDLE_OFF").State.Equals("DRIVE")) {
                        Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_STOWED").State = "ON";
                }
                towerEvolution = false;
                break;

            case WorkMode.Combat:
                towerEvolution = true; // В данный момент происходит изменение состояния башни
                GameObject pivot = RotationPivot; // Точка вращения
                bool isVertical = false;

                // Отключается лампа ПОХОД
                /*Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
                    ControlType.Indicator, "INDICATOR_STOWED").State = "OFF";*/

                NetworkInterpolatedRotation containersNIR = Containers.GetComponent<NetworkInterpolatedRotation>();

                while (true) {
                    foreach (GameObject towerDetail in TowerDetails) {
                        // Если включена синхронизация поворота контейнеров, то не нужно их поворачивать
                        if (containersNIR.enabled) break;

                        if (towerDetail != Containers
                        ) // Если не контейнеры, то доводим только до вертикального положения
                        {
                            if (Vector3.Angle(towerDetail.transform.forward, Tower.transform.up) < 1f
                            ) // Проверяем насколько вертикально она установлена
                            {
                                isVertical = true;

                                // C этого момента включаем Включаем синхронизацию поворота контейнеров, т.к. они вращаются на одном месте
                                containersNIR.enabled = true;
                                Tower.GetComponent<NetworkInterpolatedRotation>().enabled = true;

                                // Включаем слежение за ракетами
                                foreach (WeaponryArms_Strela10_Launcher launcher in Strela10.Arms.Launchers) {
                                    try {
                                        launcher.Projectile.GetComponent<NetworkInterpolatedTransform>().enabled = true;
                                    } catch {
                                        // Нет ракеты
                                    }
                                }

                                // Начальное положение контейнеров
                                Containers.transform.localPosition = new Vector3(0, 0.326427f, -0.06953819f);
                                break;
                            }
                        }

                        towerDetail.transform.RotateAround(pivot.transform.position,
                            pivot.transform.right, rotationSpeed);
                    }

                    //if (containersNIR.enabled)
                    //{
                    //}

                    if (Vector3.Angle(Containers.transform.forward, Tower.transform.forward) < 27f) {
                        currentMode = WorkMode.Combat;
                        // Включается подсветка вольтметра
                        Strela10.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
                            ControlType.Indicator, "VOLTMETER_BACKLIGHT").State = "ON";

                        towerEvolution = false;
                        break;
                    }

                    if (showAnimation) // Если показываем анимацию
                        yield return new WaitForFixedUpdate();
                }
                break;
        }
    }

    /// <summary>
    /// Приводит угол поворота контейнеров к виду от -180 до 180
    /// </summary>
    public float ClampTowerAngle(float angle) {
        if (angle > 0 && angle < 180)
            return -angle;

        if (angle > 180 && angle < 360)
            return 360 - angle;

        if (angle >= 360)
            return angle - 360;

        return angle;
    }

    /// <summary>
    /// Автоматическое слежение башни за целью
    /// </summary>
    /// <param name="target">Цель</param>
    /// <returns></returns>
    public IEnumerator AutoAimTarget(GameObject target) {
        Vector3 point; // Точка слежения (с упреждением)
        Vector3 startShift; // Изначальное смещение точки слежения относительно объекта-цели

        float angleTower, angleContainers;
        float clamp;

        Plane plane = new Plane(); // Плоскость для проекции

        Ray ray;

        // Определяем положение самолета в координатах башни
        Vector3 localDirection;

        float startDistanceToTarget, distanceK = 1f; // Начальная дистанция до цели и коэффициент дистанции

        //// ИЗНАЧАЛЬНАЯ НАСТРОЙКА И ОПРЕДЕЛЕНИЕ ПАРАМЕТРОВ ////

        // Задаем нормаль и положение плоскости
        plane.SetNormalAndPosition(-Containers.transform.forward, target.transform.position);
        // Начальная дистанация до цели, для рассчета коэффициента сближения
        startDistanceToTarget = Vector3.Distance(target.transform.position, Containers.transform.position);

        //Debug.DrawLine(Containers.transform.position, target.transform.position, Color.red);

        // Изначальное смещение точки прицела от цели
        ray = new Ray(Containers.transform.position, Containers.transform.forward);
        float startDistanceToCrosshair;
        plane.Raycast(ray, out startDistanceToCrosshair); // Делаем рейкаст до точки прицеливания
        startShift =
        (startDistanceToCrosshair * Containers.transform.forward.normalized +
         ray.GetPoint(startDistanceToCrosshair)) - target.transform.position;

        //Debug.DrawLine(Containers.transform.position,
        //               startDistanceToCrosshair*Containers.transform.forward.normalized +
        //               Containers.transform.position, Color.white);
        //Debug.DrawLine(target.transform.position, target.transform.position + startShift, Color.green);

        Vector3 localShift = Containers.transform.InverseTransformDirection(startShift);
        // Локальное смещение в системе контейнеров

        while (true) {
            // Коэффициент сближения
            distanceK = Vector3.Distance(target.transform.position, Containers.transform.position) /
                        startDistanceToTarget;

            // Позиция и нормаль плоскости
            plane.SetNormalAndPosition(-Containers.transform.forward, target.transform.position);

            point = Containers.transform.InverseTransformPoint(target.transform.position) +
                    localShift * distanceK; // В системе контейнеров делаем смещение относительно цели
            point = Containers.transform.TransformPoint(point); // Приводим в глобальные координаты

            //Debug.DrawLine(Containers.transform.position, point, Color.yellow);

            #region Вращаем башню

            // Вектор до самолета в локальных координатах
            localDirection = Tower.transform.InverseTransformDirection(point - Tower.transform.position);
            localDirection.y = 0;

            // Угол между вектором ВПЕРЕД башни и целью
            angleTower = Vector3.Angle(Tower.transform.forward,
                Tower.transform.TransformDirection(localDirection));

            // Вращаем башню - определяем слева цель или справа (Mathf.Sign), и зажимаем скорость поворота 
            // между 0 и максимальной скоростью башни за кадр
            Tower.transform.localEulerAngles = new Vector3(0,
                Tower.transform.localEulerAngles.y +
                Mathf.Clamp(angleTower, 0,
                    Strela10.TowerRotationSpeed.X * Time.fixedDeltaTime) *
                Math.Sign(localDirection.x));

            #endregion

            #region Вращаем контейнеры

            localDirection =
                Containers.transform.InverseTransformDirection(point - Containers.transform.position);
            localDirection = new Vector3(0, localDirection.y, localDirection.z);

            //Debug.DrawRay(Containers.transform.position, Containers.transform.TransformDirection(localDirection),
            //              Color.yellow);

            angleContainers = Vector3.Angle(Containers.transform.forward,
                Containers.transform.TransformDirection(localDirection));


            clamp = ClampTowerAngle(Containers.transform.localEulerAngles.x +
                                    Mathf.Clamp(angleContainers, 0,
                                        Strela10.TowerRotationSpeed.Y * Time.fixedDeltaTime) *
                                    -Mathf.Sign(localDirection.y));

            if (clamp >= Strela10.TowerLimitAngle.Y.Min && clamp <= Strela10.TowerLimitAngle.Y.Max) {
                Containers.transform.localEulerAngles = Vector3.Lerp(Containers.transform.localEulerAngles,
                    new Vector3(
                        Containers.transform.localEulerAngles.x +
                        Mathf.Clamp(angleContainers, 0, Strela10.TowerRotationSpeed.Y * Time.fixedDeltaTime) *
                        -Mathf.Sign(localDirection.y), 0, 0), 0.25f);
            }

            #endregion

            yield return new WaitForFixedUpdate();
        }
    }
}