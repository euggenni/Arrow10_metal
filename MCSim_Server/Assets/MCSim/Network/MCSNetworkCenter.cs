using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public delegate void onParticipantConnectionStatusChanged(MCSPlayer player);

public class MCSNetworkCenter : MonoBehaviour {

	public static event onParticipantConnectionStatusChanged OnParticipantConnected;
	public static event onParticipantConnectionStatusChanged OnParticipantDisconnected;


	/*       *       */
	/* ВЫЗОВ СОБЫТИЙ */
	/*       *       */

	protected virtual void CallOnParticipantConnected(MCSPlayer player)
	{
		onParticipantConnectionStatusChanged handler = OnParticipantConnected;
		if (handler != null) handler(player);
	}

	protected virtual void CallOnParticipantDisconnected(MCSPlayer player)
	{
		onParticipantConnectionStatusChanged handler = OnParticipantDisconnected;
		if (handler != null) handler(player);
	}

	/*    *    */
	/* СОБЫТИЯ */
	/*    *    */

	public void OnPlayerConnected(NetworkPlayer player)
	{
		MCSAccountInfo accountInfo = new MCSAccountInfo("Курсант", "Новый", "KYPCAHT");

		var mcsplayer = new MCSPlayer(player, accountInfo);

		try
		{
			MCSGlobalSimulation.Players.List.Add(player, mcsplayer);
		}
		catch
		{
			Debug.LogWarning("Игрок уже присутствует в списке участников, передобавляем");
			MCSGlobalSimulation.Players.List.Remove(player);
			MCSGlobalSimulation.Players.List.Add(player, mcsplayer);
		}

		CallOnParticipantConnected(mcsplayer);
	}

	private void OnPlayerDisconnected(NetworkPlayer player)
	{
		Debug.Log("Отключен игрок " + MCSGlobalSimulation.Players.List[player].Account.FirstName);


		//// ПЕРЕНЕСТИ ////
		try
		{
			var mcsplayer = MCSGlobalSimulation.Players.List[player];
			if (mcsplayer.Weaponry.GetOwners().Count - 1 <= 0)
				Network.Destroy(mcsplayer.Weaponry.gameObject);
		}
		catch
		{
			Debug.LogError("Не удалось уничтожить Weaponry игрока при отключении");
		}

		CallOnParticipantDisconnected(MCSGlobalSimulation.Players.List[player]);
		MCSGlobalSimulation.Players.List.Remove(player);

	}

	//private void OnDisconnectedFromServer(NetworkDisconnection info)
	//{
	//   Network.RemoveRPCs(Network.player);

	//   if (Network.isClient)
	//      Network.DestroyPlayerObjects(Network.player);

	//   Application.LoadLevel(Application.loadedLevel);
	//}

}
