using System;
using System.Collections;
using MilitaryCombatSimulator;
using UnityEngine;

public class Strela10_Arms : WeaponryArms {
    /// <summary>
    /// Контейнеры
    /// </summary>
    [SerializeField]
    public WeaponryArms_Strela10_Launcher[] Launchers;

    /// <summary>
    /// Объект Weaponry Strela-10
    /// </summary>
    public WeaponryTank_Strela10 Host;

    public static NetworkViewID VeiwIDTargetRussia;
    public int countLancher = 0;
    private bool shot = false;

    public WeaponryArms GetLauncher(int id) {
        return Launchers[id - 1];
    }

    // Use this for initialization
    void Awake() {
        int i = 0;
        foreach (WeaponryArms_Strela10_Launcher launcher in Launchers) {
            launcher.Index = i;
            launcher.Arms = this;
            i++;
        }
    }

    // Update is called once per frame
    void Update() {
    }

    [RPC]
    public void CheckLaunch() {
    }

    [RPC]
    public void BelongingTargetCall(NetworkViewID id) {
        Debug.LogError("приняли свою цель ");
        VeiwIDTargetRussia = id;
    }

    public void SetDistanceTargetFind(WeaponryArms_Strela10_Launcher launcher) {
        Debug.LogError("Отправляем границы пускка");
        var mass = (launcher.Projectile as WeaponryRocket).GetParameterForShootTarget();
        (launcher.Projectile as WeaponryRocket).GetDistanceTargetWar((float) mass[1], (float) mass[0]);
    }

    [RPC]
    public void Shoot(int launcherIndex) {
        if (Host.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_MODE").State.Equals("TRAINING")) {
            return;
        }
//		if ((Strela10_Arms.VeiwIDTargetRussia != null) && (Host.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_BL").State.Equals("ON"))) {
//			Debug.Log("VIEW ID TARGET RUSSIA!!!");
//			return;	
//		}
        WeaponryArms_Strela10_Launcher launcher = Launchers[launcherIndex];

        if (Network.isClient) {
            //Если включен псп  то лтц не захватывает
            if (Host.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
                    ControlType.Tumbler, "TUMBLER_PSP").State.Equals("ON") &&
                Host.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
                    ControlType.Tumbler, "TUMBLER_PSP_MODE").State.Equals("DP")) {
                networkView.RPC("CheckLaunch", RPCMode.Server);
            }

            //Указываем границы зоны поражения
            Debug.Log("Указываем границы зоны поражения");
            SetDistanceTargetFind(launcher);

            Debug.Log("Отправляем RPC Shoot");
            networkView.RPC("Shoot", RPCMode.Server, launcherIndex);

            Debug.Log("Отсоединяем ракету");
            launcher.Projectile = null; // От контейнера
            _activeRocket = null; // Она больше не установлена как активная
            Host.TowerHandler.Handler.vizirController.IsRocketShot = true;

            if (!Host.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_WORK_TYPE").State.Equals("AUTO")) 
            {
                Host.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_BOARD").State = "OFF";
                Host.TowerHandler.Handler.vizirController.bort = false;
                Host.TowerHandler.Handler.vizirController.bortFlag = false;
                Debug.Log("TMP -> Board off Strela10_Arms.cs");
            }

            //if (launcher.Projectile) // Отсоединяем ракету, и, если выбран режим AUTO запуска, выбираем следующую
            //{
            //    Debug.Log("Пытаемся отсоединить ракету");
            //    if(launcher.Projectile == _activeRocket)
            //    {
            //        Debug.Log("Текущая ракета - установлена как активная");
            //        _activeRocket = null;


            //    }
            //}
        } else {
            if (launcher.Projectile) {
                (launcher.Projectile as WeaponryRocket).Launch();
                launcher.Projectile = null;
            }
        }
    }

    [RPC]
    public void Reload(int launcherIndex) {
        if (Network.isServer) {
            Launchers[launcherIndex].Reload();
        }

        if (Network.isClient) {
            networkView.RPC("Reload", RPCMode.Server, launcherIndex);
        }
    }

    public void LoadAll<T>() where T : WeaponryProjectile {
        foreach (var launcher in Launchers) {
            launcher.ChargeProjectile<T>();
        }
    }

    public override ArmsType ArmsClass {
        get { return ArmsType.ArmsContainer; }
    }

    /// <summary>
    /// Текущая активная ракета
    /// </summary>
    private WeaponryRocket_9M37 _activeRocket;

    public override WeaponryProjectile Projectile {
        get { return _activeRocket; }
        set {
            _activeRocket = value as WeaponryRocket_9M37;

            //foreach (var launcher in Launchers) // Находится ли эта ракета в одном из контейнеров
            //{
            //    if(launcher.Projectile.Equals(value))
            //    {
            //        _activeRocket = value as WeaponryRocket_9M37;
            //        return;
            //    }
            //}

            //throw new Exception("Снаряд [" + value.name + "] не находится ни в одном из контейнеров [" + name + "]");
        }
    }

    public override void Shoot() {
        // Если устаовлена активная ракета
        if (Host.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_MODE").State.Equals("TRAINING")) {
            return;
        }
        if (Projectile) {
            int launcherIndex = 0;
            foreach (WeaponryArms_Strela10_Launcher launcher in Launchers) // Бежим по контейнерам, ищем активную ракету
            {
                // Нашли контейнер с активной ракетой
                if (launcher.Projectile == _activeRocket) {
                    Shoot(launcherIndex); // Стреляем активной ракетой 
                    countLancher++;
                    shot = true;
                    ChargeProjectile();
                    return;
                }

                launcherIndex++;
            }
        } else // Если нет активной ракеты
        {
            if (_activeRocket)
                Debug.Log("Задана активная ракета [" + _activeRocket.name + "] для контейнера");
            else
                Debug.Log("Не осталось ракет, или текущий контейнер пуст");
        }
    }

    /// <summary>
    /// Автоматически заряжает следующую ракету, исходя из режима TUMBLER_WORK_TYPE. Возвращает индекс активного контейнера
    /// </summary>
    public int ChargeProjectile() {
        // Если уже выбрана ракета (переключаемс с 1 на 2), то ставим ее в режим ожидания
        if (_activeRocket)
            _activeRocket.rocketState = WeaponryRocket_9M37.RocketState.Wait;


        int launcherIndex = -1;
        WeaponryArms_Strela10_Launcher launcher = null;
        String statusGlobal = "";
        try {
            string status =
                Host.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
                    ControlType.Tumbler, "TUMBLER_WORK_TYPE").State.ToString();
            statusGlobal = status;
            // Проверка режима Auto, I, II ....
            if (status.Equals("AUTO")) {
                foreach (var nextLauncher in Launchers) // Бежим по контейнерам, ищем следующую ракету
                {
                    launcherIndex++;
                    //if (nextLauncher == launcher) continue; // Пропускаем текущий контейнер

                    if (nextLauncher.Projectile) // Берем ракету из следующего 
                    {
                        Projectile = nextLauncher.Projectile as WeaponryRocket_9M37;
                        launcher = nextLauncher;

                        Debug.Log("Установлен режим AUTO, выбрали другую ракету [" + _activeRocket.name +
                                  "] с индексом " + (launcherIndex + 1));
                        launcherIndex = launcherIndex + 1;
                        break;
                    }
                }
            } else // Если выбрана конкретная ракета
            {
                status = status.Replace("MODE_", "");

                if (int.TryParse(status, out launcherIndex)) {
                    launcher = Launchers[launcherIndex - 1];

                    if (launcher.Projectile) // Если в указанном контейнере есть ракета 
                    {
                        Debug.Log("В лаунчере " + launcher.name + " обнаружена ракета " + launcher.Projectile.name);
                        Projectile = launcher.Projectile as WeaponryRocket_9M37;
                        Debug.Log("Установлен режим MODE_" + status + ", зарядили другую ракету [" +
                                  _activeRocket.name +
                                  "] с индексом " + launcherIndex);
                    } else {
                        Projectile = null;
                    }
                    //return launcherIndex;
                }
            }

            // Если ракета найдена, ставим ее в режим поиска
            if (_activeRocket)
                _activeRocket.rocketState = WeaponryRocket_9M37.RocketState.Search;
        } catch (Exception e) {
            Debug.Log("Не удалось задать следующую ракету: " + e.Message);
            Projectile = null;
        }

        // Отключаем индикаторы остальных контейнеров
        for (int i = 1; i <= Launchers.Length; i++) {
            Host.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
                ControlType.Indicator, "LAUNCHER_" + i).State = "OFF";
        }

        // После определения активной ракеты, включаем нужную
        if (Projectile) // Если была задана ракета
        {
            /*  if (statusGlobal.Equals("AUTO"))
            {
                for (int i = 1; i <= Launchers.Length; i++)
                {
                    if (countLancher <= i)
                    {
                        Host.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
                            ControlType.Indicator, "LAUNCHER_" + i).State = "ON";
                    }
                }
                if (shot)
                {
                  
                        Host.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
                            ControlType.Indicator, "LAUNCHER_" + (countLancher)).State = "OFF";
                    
                }
                return launcherIndex;
            }*/
            // Включаем индикатор контейнера с активной ракетой
            Host.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(
                ControlType.Indicator, "LAUNCHER_" + (launcher.Index + 1)).State = "ON";
            /**
             * выключение колайдеров для 4 ракеты и включения псп ик режим
             */

            WeaponryRocket_9M37 rocket = Projectile as WeaponryRocket_9M37;
            if (rocket) {
                var core = Host.Core["Strela-10_Operator"];
                core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "PZ_OHL").State = rocket.FirstCooler.IsFull ? "ON" : "OFF";
                core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "CHECK").State = rocket.SecondCooler.IsFull ? "ON" : "OFF";
                core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_I").State = rocket.IsCoolingActive ? "ON" : "OFF";
            } else {
                var core = Host.Core["Strela-10_Operator"];
                core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "PZ_OHL").State = "OFF";
                core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "CHECK").State = "OFF";
                core.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_I").State = "OFF";
            }
        }
        // }

        return launcherIndex; // Не удалось зарядить ракету
    }

    public override void Reload() {
        throw new NotImplementedException();
    }

    public override void ChargeProjectile<T>() {
        throw new NotImplementedException();
    }

    public override void ChargeProjectile(string projectileType) {
        throw new NotImplementedException();
    }

    public override Hashtable Resources {
        get { throw new NotImplementedException(); }
    }

    public override string Name {
        get { return "Установка-контейнер ЗРК Стрела-10"; }
    }

    public override WeaponryCategory Category {
        get { return WeaponryCategory.None; }
    }

    public override string PrefabPath {
        get { throw new NotImplementedException(); }
    }

    public override void Execute(MCSCommand cmd) {
        throw new NotImplementedException();
    }

    public override void OnWeaponryInstantiate() {
        throw new NotImplementedException();
    }

    public override void Destroy() {
        throw new NotImplementedException();
    }
}