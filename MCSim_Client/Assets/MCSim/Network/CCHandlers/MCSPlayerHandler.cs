using System.Linq;
using MilitaryCombatSimulator;
using UnityEngine;

/// <summary>
/// Класс для обработки событий, связанных с игроком
/// </summary>
public class MCSPlayerHandler : MonoBehaviour {
    public MCSPlayerHandler(MCSCommandCenter cc) {
    }

    public void Execute(NetworkPlayer sender, MCSCommand cmd) {
        Weaponry weaponry;

        switch (cmd.Command) {
            case "InitializeTraining":
                if (Network.isClient) {
                    Debug.LogWarning("InitTraining!");
                    weaponry = MCSGlobalSimulation.Weapons.List[(int) cmd.Args[0]];
                    var order =
                        weaponry.GetComponent<MCSTrainingBehaviour>()
                            .GetInfoObject()
                            .GetAllOrders()
                            .First(ord => ord.OrderName.Equals(cmd.Args[1].ToString()));

                    MCSTrainingCenter.InitializeTraining(weaponry, MCSGlobalSimulation.Players.List[Network.player],
                        order);
                }
                break;

            case "InitializeExam":
                if (Network.isClient) {
                    Debug.LogWarning("InitExam!");
                    weaponry = MCSGlobalSimulation.Weapons.List[(int) cmd.Args[0]];
                    var order =
                        weaponry.GetComponent<MCSTrainingBehaviour>()
                            .GetInfoObject()
                            .GetAllOrders()
                            .First(ord => ord.OrderName.Equals(cmd.Args[1].ToString()));

                    MCSTrainingCenter.InitializeExam(weaponry, MCSGlobalSimulation.Players.List[Network.player], order);
                }
                break;

            case "InterruptCheckers": // Прервать все экзамены тренировки для текущего игрока
                if (Network.isClient) {
                    MCSTrainingCenter.InterruptCheckers(MCSPlayer.Me,
                        false); // Останавливаем чекеры, но игроку не отсылаем, т.к. он и прислал нам
                }
                break;
        }
    }
}