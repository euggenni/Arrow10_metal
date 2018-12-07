using System;
using UnityEngine;
using System.Collections;
using MilitaryCombatSimulator;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public delegate void onCheckerChanged(MCSTrainingChecker checker);

/// <summary>
/// ����� ��� �������������� ��������
/// </summary>
public class MCSTrainingCenter : MonoBehaviour
{

    public GameObject Cloud;
	/// <summary>
	/// ������� �� ��������� ������ ���������� ������������� ����������
	/// </summary>
	public static event onCheckerChanged OnCheckerInitialized;

	private static void CallOnCheckerInitialized(MCSTrainingChecker checker)
	{
		onCheckerChanged handler = OnCheckerInitialized;
		if (handler != null) handler(checker);
	}

	/// <summary>
	/// ������� �� ���������� ������ ���������� ������������� ����������
	/// </summary>
	public static event onCheckerChanged OnCheckerDisposed;

	private static void CallOnCheckerDisposed(MCSTrainingChecker checker)
	{
		onCheckerChanged handler = OnCheckerDisposed;
		if (handler != null) handler(checker);
	}

	/// <summary>
    /// ������ �������� �� �����������
    /// </summary>
	public static Dictionary<MCSPlayer, MCSTrainingChecker> CheckersList = new Dictionary<MCSPlayer, MCSTrainingChecker>();

    private static GameObject _gameObject;

    void Awake()
    {
        _gameObject = gameObject;
    }

    public static void InitializeTrainingMode(Weaponry weaponry, MCSPlayer player, MCSTrainingOrder order)
    {
        var checker = _gameObject.AddComponent<MCSTrainingChecker>();
        checker.MonitorOrderExecution(weaponry, player, order);
    }

	public static void InitializeTraining(Weaponry weaponry, MCSPlayer player, MCSTrainingOrder order) {

		if (Network.isServer)
		{
			var checker = _gameObject.AddComponent<MCSTrainingChecker>();

			checker.Player = player;
			checker.Weaponry = weaponry;
			checker.Order = order;

			MCSGlobalSimulation.CommandCenter.Execute(player.NetworkPlayer,
													  new MCSCommand(MCSCommandType.Player, "InitializeTraining", false, weaponry.ID,
																	 order.OrderName));
			
			CheckersList.Add(player, checker);

			CallOnCheckerInitialized(checker);
		}
    }
    
    public static MCSTrainingChecker InitializeExam(Weaponry weaponry, MCSPlayer player, MCSTrainingOrder order)
    {
	    MCSTrainingChecker checkerExam = null;
	    if (Network.isServer)
		{
			 checkerExam = _gameObject.AddComponent<MCSExamChecker>();

		    try
		    {
			    checkerExam.Player = player;
			    checkerExam.Weaponry = weaponry;
			    checkerExam.Order = order;

			    MCSGlobalSimulation.CommandCenter.Execute(player.NetworkPlayer,
			                                              new MCSCommand(MCSCommandType.Player, "InitializeExam", false, weaponry.ID,
																		 order.OrderName));

				CheckersList.Add(player, checkerExam);

			    CallOnCheckerInitialized(checkerExam);
		    }
		    catch
		    {
			    Debug.LogError("������ ��� ������������� ��������.");
			    throw;
		    }
	    }

	    return checkerExam;
    }


	/// <summary>
	/// �������� �������� ��� �������
	/// </summary>
	public static void InterruptCheckers(MCSPlayer player, bool sendToPlayer)
	{
		Debug.LogError("������ ���������� �����");
		try
		{
			if (CheckersList.ContainsKey(player))
			{
				var checker = CheckersList[player];
				CallOnCheckerDisposed(checker);

				// ���� ����� - �������������, �� ������ ���������� Weaponry
				if (checker.GetType() == typeof(MCSTrainingChecker))
				{
					Debug.Log("������������� ����� - ���������� ������");
					MCSGlobalSimulation.Destroy(checker.Weaponry);
				}

				CheckersList.Remove(player);

				if (sendToPlayer)	// �������� ������ ���������� �������� ������� ����������
					MCSGlobalSimulation.CommandCenter.Execute(player.NetworkPlayer, new MCSCommand(MCSCommandType.Player, "InterruptCheckers", false, checker.Weaponry.ID, checker.Order.PerformerName));

				Destroy(checker);
			} 
			else Debug.LogWarning("�� ������ Checker ��� ������� ������ " + player.NetworkPlayer);
		}
		catch
		{
			Debug.LogError("�� ��������������� ������� ��� ������, ������ �������");
		}
	}
}
