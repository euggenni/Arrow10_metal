using System.Linq;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

/// <summary>
/// Класс для обработки событий, связанных с игроком
/// </summary>
public class MCSPlayerHandler : MonoBehaviour
{

    public MCSPlayerHandler(MCSCommandCenter cc)
    {
    }

    public void Execute(NetworkPlayer sender, MCSCommand cmd)
    {
		Weaponry weaponry;

	    switch (cmd.Command)
	    {
		    case "InitializeTraining":
			    if (Network.isClient)
			    {
				    Debug.LogWarning("InitTraining!");
					weaponry = MCSGlobalSimulation.Weapons.List[(int)cmd.Args[0]];
				    var order =
						weaponry.GetComponent<MCSTrainingBehaviour>()
					            .GetInfoObject()
					            .GetAllOrders()
					            .First(ord => ord.OrderName.Equals(cmd.Args[1].ToString()));

					MCSTrainingCenter.InitializeTraining(weaponry, MCSGlobalSimulation.Players.List[Network.player], order);
			    }
			    break;

		    case "InitializeExam":
			    if (Network.isClient)
			    {
				    Debug.LogWarning("InitExam!");
					weaponry = MCSGlobalSimulation.Weapons.List[(int)cmd.Args[0]];
				    var order =
						weaponry.GetComponent<MCSTrainingBehaviour>()
					            .GetInfoObject()
					            .GetAllOrders()
					            .First(ord => ord.OrderName.Equals(cmd.Args[1].ToString()));

					MCSTrainingCenter.InitializeExam(weaponry, MCSGlobalSimulation.Players.List[Network.player], order);
			    }
			    break;

			case "InterruptCheckers": // Прервать все экзамены тренировки для игрока
				if (Network.isServer)
				{
					//Debug.LogError("Нас просят прервать чекер " + cmd.Args[0] + " " + cmd.Args[1]);
					// Оповещаем всех что прерван экзамен
				
					// Получаем айди Weaponry, для которого должны завершить проверку
					weaponry = MCSGlobalSimulation.Weapons.List[(int) cmd.Args[0]];

					//Debug.Log("Должны прервать чекеры для Weaponry " + weaponry.Name);

					// Получаем игрока-владельца данной машины, роль которого совпадает с пришедшей ролью

					MCSPlayer player = null;

					foreach (var owner in weaponry.GetOwners())
					{
						Debug.LogError(owner.Role + " - " + cmd.Args[1]);

						if(owner.Role.Equals(cmd.Args[1].ToString()))
							player = owner;
					}

					//var player = weaponry.GetOwners().First(owner => owner.Role.Equals(cmd.Args[1].ToString()));
					//Debug.LogError("Для игрока " + player.NetworkPlayer);

					MCSTrainingCenter.InterruptCheckers(player, false); // Останавливаем чекеры, но игроку не отсылаем, т.к. он и прислал нам
				}
				break;
	    }
    }
}