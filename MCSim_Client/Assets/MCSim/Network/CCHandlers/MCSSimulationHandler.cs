using System;
using MilitaryCombatSimulator;
using UnityEngine;

public class MCSSimulationHandler : MonoBehaviour {
    NetworkPlayer players;
    public static UniSkyAPI UniSky;

    void OnPlayerConnected(NetworkPlayer player) {
        if (Network.isServer) {
            players = player;
            // Синхронизируем время симуляции
            MCSCommand command = new MCSCommand(MCSCommandType.Simulation, "SynhronizeEnvironmentTime", false,
                MCSGlobalEnvironment.UniSky.TIME);
            MCSGlobalSimulation.CommandCenter.Execute(player, command);
        }
    }

    public void Execute(NetworkPlayer sender, MCSCommand cmd) {
        Weaponry weaponry;

        switch (cmd.Command) {
            case "WeatherPosted":
                Debug.Log("WeatherPosted 1");
                try {
                    UniSky = GameObject.Find("UniSkyAPI").GetComponent("UniSkyAPI") as UniSkyAPI;

                    // Set some initial states 
                    //UniSky.SetTime(float.Parse(cmd.Args[0].ToString()));
                    Debug.Log("New time: " + float.Parse(cmd.Args[0].ToString()));
                    UniSky.SetTime(float.Parse(cmd.Args[0].ToString()));
                    //UniSky.SetAmbientLighting(new Color(0.1f, 0.1f, 0.1f, 0.1f));
                    //UniSky.SetStormCenter(new Vector3(0, 0, 0));
                    //UniSky.SetSunShadows(LightShadows.Hard);
                } catch {
                    Debug.Log("Ошибка при инициализации окружения.");
                }
                break;
            case "Instantiate":
                Debug.Log("Instantiated 1");
                string type = cmd.Args[0].ToString(); // Тип объекта

                // Если клиент - запрашиваем ViewID
                if (Network.isClient) {
                    MCSGlobalSimulation.ClientInstantiate((int)cmd.Args[1], type);
                } else // Если пришлом на сервер, создаем объект и сервер - отсылаем ViewID
                {
                    MCSGlobalSimulation.Instantiate(type);
                }
                break;

            // Клиент сообщает, что Weaponry размещен
            case "WeaponryPosted":
                if (Network.isServer) {
                    Debug.Log("Weaponry [" + cmd.Args[0] + "] Posted by " + sender);

                    // Обращаемся к Weaponry по его ID
                    if (MCSGlobalSimulation.Weapons.List.ContainsKey((int)cmd.Args[0])) {
                        try {
                            // Получаем список NetworkView на нем
                            NetworkView[] nwlist =
                                MCSGlobalSimulation.Weapons.List[(int)cmd.Args[0]].gameObject.GetComponentsInChildren
                                    <NetworkView>();

                            // Для каждого networkView - отсылаем на клиент его viewID и включаем его
                            foreach (NetworkView networkView in nwlist) {
                                MCSGlobalSimulation.CommandCenter.networkView.RPC("RPC_AssignViewID", sender,
                                    (int)cmd.Args[0], networkView.viewID);
                                //Debug.Log("Шлем клиенту " + sender + " viewID " + networkView.viewID);
                                networkView.enabled = true;
                                networkView.stateSynchronization = NetworkStateSynchronization.Unreliable;
                            }

                            //MCSGlobalSimulation.CommandCenter.Execute(new MCSCommand(MCSCommandType.Weaponry, "SetRole", false, new Strela10_Operator_PanelLibrary().GetName()));
                        } catch (Exception e) {
                            Debug.LogError("Ошибка при обработке команды [WeaponryPosted]:" + e.Message);
                        }
                    }
                }
                break;

            case "DestroyWeaponry":
                weaponry = MCSGlobalSimulation.Weapons.List[(int)cmd.Args[0]];

                if (weaponry != null) {
                    foreach (MCSPlayer player in weaponry.GetOwners()) {
                        player.Weaponry = null;
                        player.Role = null;
                    }

                    // Сервер уже уничтожил
                    //Destroy(weaponry.gameObject);
                }
                break;

            case "WeaponrySynchronized":
                int id = (int)cmd.Args[0];

                if (Network.isServer) {
                    try {
                        if (MCSGlobalSimulation.Weapons.Synchronizations.ContainsKey(id))
                            MCSGlobalSimulation.Weapons.Synchronizations[id]++;
                        else MCSGlobalSimulation.Weapons.Synchronizations[id] = 1;

                        weaponry = MCSGlobalSimulation.Weapons.List[id];

                        // Если пришло от всех кроме нас
                        if (MCSGlobalSimulation.Weapons.Synchronizations[id] == Network.connections.Length)
                            weaponry.OnWeaponryInstantiate(); // Отсылаем событие синхронизации
                    } catch (Exception) {
                        Debug.Log("Не удалось распознать Weaponry c ID [" + (int)cmd.Args[0] + "]");
                    }
                } else {
                    try {
                        weaponry = MCSGlobalSimulation.Weapons.List[id];
                        weaponry.OnWeaponryInstantiate();
                    } catch (Exception) {
                        Debug.Log("Не удалось распознать Weaponry c ID [" + (int)cmd.Args[0] + "]");
                    }
                }
                break;

            case "SynhronizeEnvironmentTime":
                //MCSGlobalEnvironment.UniSky.TIME = (float)cmd.Args[0];
                break;

            case "DestroyWeaponryByExplosion":
                try {
                    Debug.Log("Пришла команда на подрыв объекта");
                    //Vector3 explosion = (Vector3) cmd.Args[0];
                    Weaponry explosion = MCSGlobalSimulation.Weapons.List[(int)cmd.Args[0]];
                    weaponry = MCSGlobalSimulation.Weapons.List[(int)cmd.Args[1]];
                    weaponry.gameObject.FallAPart(explosion.transform.position);

                    weaponry.Destroy();
                } catch {
                }
                break;

            case "ClientInstantiateRequest":
                if (Network.isServer) {
                    if ((int)cmd.Args[1] == -1) {
                        weaponry = MCSGlobalSimulation.Instantiate(cmd.Args[0].ToString());
                    } else weaponry = MCSGlobalSimulation.Weapons.List[(int)cmd.Args[1]];

                    if (weaponry != null) {
                        var ti = weaponry.GetComponent<MCSTrainingBehaviour>();

                        weaponry.OnWeaponryInstantiated += delegate() {
                            // Привязать камеру к стреле
                            foreach (var info in ti.GetInfoObject().GetAllOrders()) {
                                if (info.OrderName.Equals(cmd.Args[2].ToString()))
                                    MCSTrainingCenter.InitializeTraining(weaponry, null, info);
                            }
                        };
                    } else Debug.LogError("There is no weaponry with id [" + (int)cmd.Args[1] + "]");
                }
                break;

            default:
                Debug.Log("Команда [" + cmd.Command + "], поступившая в [" + this.GetType() + "] не определена");
                break;
        }
    }
}