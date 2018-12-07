using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public delegate void onParticipantConnectionStatusChanged(MCSPlayer player);

public class MCSNetworkCenter : MonoBehaviour {

	public static event onParticipantConnectionStatusChanged OnParticipantConnected;
	public static event onParticipantConnectionStatusChanged OnParticipantDisconnected;


	/*       *       */
	/* ����� ������� */
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
	/* ������� */
	/*    *    */

	public void OnPlayerConnected(NetworkPlayer player)
	{
		MCSAccountInfo accountInfo = new MCSAccountInfo("�������", "�����", "KYPCAHT");

		var mcsplayer = new MCSPlayer(player, accountInfo);

		try
		{
			MCSGlobalSimulation.Players.List.Add(player, mcsplayer);
		}
		catch
		{
			Debug.LogWarning("����� ��� ������������ � ������ ����������, �������������");
			MCSGlobalSimulation.Players.List.Remove(player);
			MCSGlobalSimulation.Players.List.Add(player, mcsplayer);
		}

		CallOnParticipantConnected(mcsplayer);
	}

	private void OnPlayerDisconnected(NetworkPlayer player)
	{
		Debug.Log("�������� ����� " + MCSGlobalSimulation.Players.List[player].Account.FirstName);


		//// ��������� ////
		try
		{
			var mcsplayer = MCSGlobalSimulation.Players.List[player];
			if (mcsplayer.Weaponry.GetOwners().Count - 1 <= 0)
				Network.Destroy(mcsplayer.Weaponry.gameObject);
		}
		catch
		{
			Debug.LogError("�� ������� ���������� Weaponry ������ ��� ����������");
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
