using System;
using System.Text;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public class Strela10_TowerHandler : MonoBehaviour
{
    public Strela10_Operator_CoreHandler Handler;

    private WeaponryTank_Strela10 Strela10;
    private GameObject Tower, Containers;
    private CoreLibrary.Core Core;

    public GameObject[] TowerDetails;
    public GameObject RotationPivot;

    public WorkMode currentMode = WorkMode.Combat;

    private PanelControl controlPosition, indicatorStowed;

    // Определяет можно ли управлять башней в даный момент
    private bool stowed = false;

    public enum WorkMode
    {
        Marching = 0,
        Combat = 1
    }

    void Start()
    {
        if (Handler) {
            Core = Handler.Core;
            Strela10 = Handler.Strela10;

            //************//

            //Strela10 = gameObject.GetComponentInParents<WeaponryTank_Strela10>(false);
            //string roleName = new Strela10_Operator_PanelLibrary().GetName();

            //Strela10.Core[roleName].Virtualize();

            //CoreToolkit ct = Strela10.Core[roleName] as CoreToolkit;
            //ct.Handler.Weaponry = Strela10; // Настраиваем обработчик событий ядра
            //ct.Handler.Core = ct;

            //Core = Handler.Core;

            //************//

            Tower = Handler.Strela10.Tower;
            Containers = Handler.Strela10.Containers;

            Handler.ControlChangeCallEvent += OnControlChanged;

            _operatorJoystickHorizontal = Core.GetPanel("Strela10_GuidancePanel").GetControl(ControlType.Joystick, "OperatorJoystickHorizontal");
            _operatorJoystickVertical = Core.GetPanel("Strela10_GuidancePanel").GetControl(ControlType.Joystick, "OperatorJoystickVertical");
            controlPosition = Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_POSITION");
        }
        else {
            Debug.LogError("Не установлен Strela10_Operator_CoreHandler");
        }

        //SwitchWorkMode(WorkMode.Marching, false);
    }

    void OnControlChanged(PanelControl control)
    {
        //[1]: Контрол [TUMBLER_POSITION] на панели [Strela10_SupportPanel] принял состояние [BATTLE]
        if(controlPosition == null) {
            controlPosition = Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_POSITION");
        }
    }
    
	// Update is called once per frame
	void FixedUpdate () {
        try
        {
            if (Handler.Core.isVirtual)
            {
                if (!stowed) {
                    UpdateTower();
                }
            }
        }
        catch
        {
        }
	}

    private PanelControl _tumblerPosition, _operatorJoystickHorizontal, _operatorJoystickVertical;

    /// <summary>
    /// Изменить положение башни
    /// </summary>
    void UpdateTower()
    {
        float verticalMax;

        int[] joystickHorizontal = (int[])_operatorJoystickHorizontal.State;
        float x = joystickHorizontal[1];

        int[] joystickVertical = (int[])_operatorJoystickVertical.State;
        float y = joystickVertical[0];

        if (!Mathf.Approximately(x, 0)) x = x / 35f;
        if (!Mathf.Approximately(y, 0)) y = y / 35f;

        Tower.transform.localEulerAngles = new Vector3(0f, Tower.transform.localEulerAngles.y + Strela10.TowerRotationSpeed.X * Time.fixedDeltaTime * x, 0f);

        // Настоящий наклон башни в градусах
        float clamp = ClampTowerAngle(Strela10.Containers.transform.localEulerAngles.x - Strela10.TowerRotationSpeed.Y * Time.fixedDeltaTime * y);
        
        // ЕСЛИ ДОВЕЛИ БАШНЮ ДО 83, А ПОТОМ ПЕРЕКЛЮЧИЛИСЬ НА БОЕВОЕ, ТО НУЖНО ЧТОБЫ БАШНЮ НЕ БЛОКИРОВАЛО

        if (controlPosition.State.ToString() == "BATTLE") { // Если тумбелр в боевом положении
        
            verticalMax = Strela10.TowerLimitAngle.Y.Max;
            
        }
        else {
            verticalMax = 83;
        }
        
        // Поворот контейнеров
        if (clamp >= Strela10.TowerLimitAngle.Y.Min && clamp <= verticalMax)
            Strela10.Containers.transform.localEulerAngles =
                new Vector3(
                    Strela10.Containers.transform.localEulerAngles.x -
                    Strela10.TowerRotationSpeed.Y * Time.deltaTime * y, 0f, 0f);
    }

    /// <summary>
    /// Приводит угол поворота контейнеров к виду от -180 до 180
    /// </summary>
    float ClampTowerAngle(float angle) {
        if (angle > 0 && angle < 180)
            return -angle;

        if (angle > 180 && angle < 360)
            return 360 - angle;

        if (angle >= 360)
            return angle - 360;

        return angle;
    }

    
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (currentMode == WorkMode.Marching)
        //        SwitchWorkMode(WorkMode.Combat, false);
        //    else SwitchWorkMode(WorkMode.Marching, false);
        //}
    }

    /// <summary>
    /// Переход башни между режимами.       
    /// </summary>
    /// <param name="mode">Режим</param>
    /// <param name="showAnimation">Отображать ли аниацию</param>
    public void SwitchWorkMode(WorkMode mode, bool showAnimation) {
        if (towerEvolution) return; // Если уже изменяем положение
        if (mode == currentMode) return; // Если текущий режим и есть требуемый

        Model.UCoroutine(this, switchWorkMode(mode, showAnimation), "switchWorkMode");
    }

    private bool towerEvolution = false;
    /// <summary>
    /// Корутина переключения положения башни
    /// </summary>
    /// <param name="mode">Режим Походный/Боевой</param>
    /// <param name="immediately">Без анимации</param>
    /// <returns></returns>
    private IEnumerator switchWorkMode(WorkMode mode, bool showAnimation)
    {
        float rotationSpeed = Time.fixedDeltaTime * 15f;

        switch (mode)
        {
            case WorkMode.Marching:
                towerEvolution = true; // В данный момент происходит изменение состояния башни
                stowed = true; // Делаем невозможным поворот башни

                Tower.transform.localEulerAngles = Vector3.zero;

                // Выравниваем контейнеры
                Containers.transform.localEulerAngles = new Vector3(-83f, 0, 0);

                while (true)
                {
                    foreach (GameObject towerDetail in TowerDetails) {
                        towerDetail.transform.RotateAround(RotationPivot.transform.position,
                                                           RotationPivot.transform.right, -rotationSpeed);
                    }

                    if (Containers.transform.localRotation.eulerAngles.x < rotationSpeed)
                    {
                        currentMode = WorkMode.Marching;
                        towerEvolution = false; // Эволюция башни завершена
                        break;
                    }
                    if (showAnimation) // Если показываем анимацию
                    yield return new WaitForFixedUpdate();
                }
                break;

            case WorkMode.Combat:
                towerEvolution = true; // В данный момент происходит изменение состояния башни
                GameObject pivot = RotationPivot; // Точка вращения
                bool isVertical = false;
                while (true)
                {
                    foreach (GameObject towerDetail in TowerDetails)
                    {
                        if(towerDetail != Containers) // Если не контейнеры, то доводим только до вертикального положения
                        {
                            if(Vector3.Angle(towerDetail.transform.forward, Tower.transform.up) < 1f) // Проверяем насколько вертикально она установлена
                            {
                                isVertical = true;
                                continue;
                            }
                        }

                        if(isVertical)
                        {
                            pivot = Containers; // Если дошли до вертикального поворота, то поворачиваем вокруг оси контейнеров
                        }

                        towerDetail.transform.RotateAround(pivot.transform.position,
                                                           pivot.transform.right, rotationSpeed);
                    }

                    if(Vector3.Angle(Containers.transform.forward, Tower.transform.forward) < 25f)
                    {
                        currentMode = WorkMode.Combat;
                        towerEvolution = false; // Эволюция башни завершена
                        stowed = false; // Делаем возможным поворот башни
                        break;
                    }

                    if (showAnimation) // Если показываем анимацию
                    yield return new WaitForFixedUpdate();
                }
                break;
        }

    }

    public IEnumerator ACUBattle(GameObject target)
    {
        //StopCoroutine("AutoAimTarget"); // TODO: 

        Debug.Log(target.name);
           //определение угла башни 
       

       
        //print(_target.name + ": " + x);
        //CallTargetAngleChanged(x < 0 ? 360 + x : x);

        while (true)
        {
            var vector = Strela10.transform.InverseTransformDirection(target.transform.position - Strela10.transform.position);
            vector.y = 0;
            float sign = Mathf.Sign(vector.x);
            //vector = Tower.transform.TransformDirection(vector);
                                   
            float x = Vector3.Angle(Strela10.transform.forward, vector) * sign;

            Vector3 hz = Vector3.zero;
            //Tower.transform.forward = Vector3.Lerp(Tower.transform.forward, vector, Strela10.TowerRotationSpeed.X * Time.deltaTime);
            Tower.transform.localEulerAngles = Vector3.Lerp(Tower.transform.localEulerAngles, new Vector3(0, x, 0), Strela10.TowerRotationSpeed.X * 0.01F);


            x = x < 0 ? 360 + x : x;
           // Debug.Log(x);

            Debug.DrawLine(Tower.transform.position, Tower.transform.position + Tower.transform.forward * 100f, Color.blue);
            Debug.DrawLine(Tower.transform.position, Tower.transform.position + vector, Color.red);

            //Tower.transform.localEulerAngles = new Vector3(0,x,0);
            yield return new WaitForEndOfFrame();
        }

        //  while (true)
        //{
        //    var vector = Tower.transform.InverseTransformDirection(target.transform.position - Tower.transform.position);
        //    vector.y = 0;
        //    float sign = Mathf.Sign(vector.x);
        //    //vector = Tower.transform.TransformDirection(vector);
            
                       
        //    float x = Vector3.Angle(Tower.transform.forward, vector) * sign;

        //    Vector3 hz = Vector3.zero;
        //    //Tower.transform.forward = Vector3.Lerp(Tower.transform.forward, vector, Strela10.TowerRotationSpeed.X * Time.deltaTime);
        //    Tower.transform.forward = Vector3.Lerp(Tower.transform.forward, vector, Strela10.TowerRotationSpeed.X * Time.deltaTime);


        //    x = x < 0 ? 360 + x : x;
        //    Debug.Log(x);

        //    Debug.DrawLine(Tower.transform.position, Tower.transform.position + Tower.transform.forward * 100f, Color.blue);
        //    Debug.DrawLine(Tower.transform.position, Tower.transform.position + vector, Color.red);

        //    //Tower.transform.localEulerAngles = new Vector3(0,x,0);
        //    yield return new WaitForEndOfFrame();
        //}

/*
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
        startShift = (startDistanceToCrosshair * Containers.transform.forward.normalized + ray.GetPoint(startDistanceToCrosshair)) - target.transform.position;

        //Debug.DrawLine(Containers.transform.position,
        //               startDistanceToCrosshair*Containers.transform.forward.normalized +
        //               Containers.transform.position, Color.white);
        //Debug.DrawLine(target.transform.position, target.transform.position + startShift, Color.green);

        Vector3 localShift = Containers.transform.InverseTransformDirection(startShift);
        //Debug.LogWarning("SHIFT LOC "+localShift);
        // Локальное смещение в системе контейнеров
        // x: 100 y: -100
        while (true)
        {
            // Коэффициент сближения
            distanceK = Vector3.Distance(target.transform.position, Containers.transform.position) / startDistanceToTarget;

            // Позиция и нормаль плоскости
            plane.SetNormalAndPosition(-Containers.transform.forward, target.transform.position);

            point = Containers.transform.InverseTransformPoint(target.transform.position) + localShift * distanceK; // В системе контейнеров делаем смещение относительно цели
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


            clamp = ClampTowerAngle(Containers.transform.localEulerAngles.x + Mathf.Clamp(angleContainers, 0, Strela10.TowerRotationSpeed.Y * Time.fixedDeltaTime) *
                        -Mathf.Sign(localDirection.y));

            if (clamp >= Strela10.TowerLimitAngle.Y.Min && clamp <= Strela10.TowerLimitAngle.Y.Max)
            {
                Containers.transform.localEulerAngles = Vector3.Lerp(Containers.transform.localEulerAngles,
                                                                     new Vector3(
                                                                         Containers.transform.localEulerAngles.x +
                                                                         Mathf.Clamp(angleContainers, 0,
                                                                                     Strela10.TowerRotationSpeed.Y *
                                                                                     Time.fixedDeltaTime) *
                                                                         -Mathf.Sign(localDirection.y), 0, 0),
                                                                     0.25f);
            }

            #endregion
        
            yield return new WaitForFixedUpdate();
        }*/
    }

    /// <summary>
    /// Автоматическое слежение башни за целью
    /// </summary>
    /// <param name="target">Цель</param>
    /// <returns></returns>
    public IEnumerator AutoAimTarget(GameObject target)
    {
        //StopCoroutine("AutoAimTarget"); // TODO: 

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
        startShift = (startDistanceToCrosshair * Containers.transform.forward.normalized + ray.GetPoint(startDistanceToCrosshair)) - target.transform.position;

        //Debug.DrawLine(Containers.transform.position,
        //               startDistanceToCrosshair*Containers.transform.forward.normalized +
        //               Containers.transform.position, Color.white);
        //Debug.DrawLine(target.transform.position, target.transform.position + startShift, Color.green);

        Vector3 localShift = Containers.transform.InverseTransformDirection(startShift);
        //Debug.LogWarning("SHIFT LOC "+localShift);
        // Локальное смещение в системе контейнеров
        // x: 100 y: -100
        while (true)
        {
            // Коэффициент сближения
            distanceK = Vector3.Distance(target.transform.position, Containers.transform.position) / startDistanceToTarget;

            // Позиция и нормаль плоскости
            plane.SetNormalAndPosition(-Containers.transform.forward, target.transform.position);

            point = Containers.transform.InverseTransformPoint(target.transform.position) + localShift * distanceK; // В системе контейнеров делаем смещение относительно цели
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


            clamp = ClampTowerAngle(Containers.transform.localEulerAngles.x + Mathf.Clamp(angleContainers, 0, Strela10.TowerRotationSpeed.Y * Time.fixedDeltaTime) *
                        -Mathf.Sign(localDirection.y));

            if (clamp >= Strela10.TowerLimitAngle.Y.Min && clamp <= Strela10.TowerLimitAngle.Y.Max)
            {
                Containers.transform.localEulerAngles = Vector3.Lerp(Containers.transform.localEulerAngles,
                                                                     new Vector3(
                                                                         Containers.transform.localEulerAngles.x +
                                                                         Mathf.Clamp(angleContainers, 0,
                                                                                     Strela10.TowerRotationSpeed.Y *
                                                                                     Time.fixedDeltaTime) *
                                                                         -Mathf.Sign(localDirection.y), 0, 0),
                                                                     0.25f);
            }

            #endregion

            yield return new WaitForFixedUpdate();
        }
    }

    /*public void ShootWithPrediction(GameObject target)
    {
        Vector3 point; // Точка слежения (с упреждением)
        Vector3 startShift; // Изначальное смещение точки слежения относительно объекта-цели
        Vector3 localDirection;
        float angleTower, angleContainers;
        float clamp;
        float startDistanceToTarget, distanceK = 1f; // Начальная дистанция до цели и коэффициент дистанции
        Plane plane = new Plane();
        startDistanceToTarget = Vector3.Distance(target.transform.position, Containers.transform.position);
       // float startDistanceToTarget, distanceK = 1f; 
        // Высчитываешь поврот башни (метод AutoAimTarget в помощь)
        //Vector3 localShift = new Vector3(-100, 0, 0);
        var ray = new Ray(Containers.transform.position, Containers.transform.forward);
        float startDistanceToCrosshair;
        plane.Raycast(ray, out startDistanceToCrosshair); // Делаем рейкаст до точки прицеливания
        startShift = (startDistanceToCrosshair * Containers.transform.forward.normalized + ray.GetPoint(startDistanceToCrosshair)) - target.transform.position;

        Vector3 localShift = Containers.transform.InverseTransformDirection(startShift);
        localShift.x += 200;
        // Коэффициент сближения
        distanceK = Vector3.Distance(target.transform.position, Containers.transform.position) / startDistanceToTarget;

        // Позиция и нормаль плоскости
        plane.SetNormalAndPosition(-Containers.transform.forward, target.transform.position);

        point = Containers.transform.InverseTransformPoint(target.transform.position) + localShift * distanceK; // В системе контейнеров делаем смещение относительно цели
        point = Containers.transform.TransformPoint(point); // Приводим в глобальные координаты

        //Debug.DrawLine(Containers.transform.position, point, Color.yellow);

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

        localDirection =
            Containers.transform.InverseTransformDirection(point - Containers.transform.position);
        localDirection = new Vector3(0, localDirection.y, localDirection.z);

        //Debug.DrawRay(Containers.transform.position, Containers.transform.TransformDirection(localDirection),
        //              Color.yellow);

        angleContainers = Vector3.Angle(Containers.transform.forward,
                                        Containers.transform.TransformDirection(localDirection));


        clamp = ClampTowerAngle(Containers.transform.localEulerAngles.x + Mathf.Clamp(angleContainers, 0, Strela10.TowerRotationSpeed.Y * Time.fixedDeltaTime) *
                    -Mathf.Sign(localDirection.y));

        if (clamp >= Strela10.TowerLimitAngle.Y.Min && clamp <= Strela10.TowerLimitAngle.Y.Max)
        {
            Containers.transform.localEulerAngles = Vector3.Lerp(Containers.transform.localEulerAngles,
                                                                 new Vector3(
                                                                     Containers.transform.localEulerAngles.x +
                                                                     Mathf.Clamp(angleContainers, 0,
                                                                                 Strela10.TowerRotationSpeed.Y *
                                                                                 Time.fixedDeltaTime) *
                                                                     -Mathf.Sign(localDirection.y), 0, 0),
                                                                 0.25f);
        }

  
    }
    */
    void OnGUI()
    {
       //GUI.Toggle(new Rect(Screen.width / 2 - 1f, Screen.height / 2 - 1f, 2f, 2f), true, "");
    }
}
