using System;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public class UIElement_Training_Participant : GUIObject {

	// �������� � ������, ����� ���� ������������� �������, ��� ��������� ���� ��� ���

	// MCSUICenter - OnExamInitialized - ������������� �����, ��� ������ ������������������ - ������ ������, ���� ����������� ������

	private MCSPlayer _player;
	private UIPopupList _listOrders;

	private MCSTrainingInfo _trainingInfo = null;

	private MCSTrainingChecker _checker;

	void Awake()
	{
		MCSTrainingCenter.OnCheckerInitialized += OnCheckerInitialized;
		MCSTrainingCenter.OnCheckerDisposed += OnCheckerDisposed;

		_listOrders = GetControl<UIPopupList>("Orders");

		GetControl<UIButtonMessage>("Button_Activate").functionName = "Activate";
	}

	private void OnCheckerDisposed(MCSTrainingChecker checker)
	{
		if (checker.Player.Equals(_player)) // ���� ��������������� ����� ��� ������ ������
		{
			_checker = null;
			UpdateUI();
		}
	}

	private void OnCheckerInitialized(MCSTrainingChecker checker)
	{
		if (checker.Player.Equals(_player)) // ���� ��������������� ����� ��� ������ ������
		{
			_checker = checker;
			UpdateUI();
		}
	}

	void Set(MCSPlayer player)
	{
		_player = player;
		
		if(MCSTrainingCenter.CheckersList.ContainsKey(player))
			_checker = MCSTrainingCenter.CheckersList[player];

		UpdateUI();
	}

	void Activate() // ��������� ����� ��������
	{
		if (_player.Weaponry)
		{
			int orderID = _listOrders.items.IndexOf(_listOrders.selection);

			if (orderID != -1 && _trainingInfo != null)
			{
				MCSTrainingCenter.InitializeExam(_player.Weaponry, _player, _trainingInfo.GetAllOrders()[orderID]);
			} else Debug.LogWarning("������ ����������� �������");
		}
	}

	void Deactivate()
	{
		MCSTrainingCenter.InterruptCheckers(_player, true);

		UpdateUI();
	}

	void UpdateUI()
	{
		_listOrders.items.Clear();

		if (_player == null)
		{
			GetControl<UILabel>("Player").text = "������";
			this["Button_Activate"].gameObject.SetActive(false);
			return;
		}


		try
		{
			GetControl<UILabel>("Player").text = _player.Account.FirstName;
			GetControl<UILabel>("Weaponry").text = _player.Weaponry.Name + " [" + _player.Weaponry.ID + "]";

			_trainingInfo = _player.Weaponry.GetComponent<MCSTrainingBehaviour>().GetInfoObject();

			foreach (var order in _trainingInfo.GetAllOrders())
				_listOrders.items.Add(order.OrderName);

			if (_listOrders.items.Count != 0)
				_listOrders.selection = _listOrders.items[0];


			if (_checker)
			{
				GetControl<UIButtonMessage>("Button_Activate").functionName = "Deactivate";
				GetControl<UILabel>("Button_Label").text = "����������";


				GetControl<UILabel>("Status").text = (_checker is MCSExamChecker ? "���� �������" : "���� ����������");

				_listOrders.collider.enabled = false; // ��������� ���������� ������
			}
			else
			{
				GetControl<UIButtonMessage>("Button_Activate").functionName = "Activate";
				GetControl<UILabel>("Button_Label").text = "���������";

				GetControl<UILabel>("Status").text = "��������...";

				_listOrders.collider.enabled = true;
			}

			// ��������� ������ ������
		}
		catch
		{
			GetControl<UILabel>("Weaponry").text = "�����������";

			_listOrders.selection = "-";
			_listOrders.enabled = false;

			this["Button_Activate"].gameObject.SetActive(false);
		}
	}

	void OnDestroy()
	{
		MCSTrainingCenter.OnCheckerInitialized -= OnCheckerInitialized;
		MCSTrainingCenter.OnCheckerDisposed -= OnCheckerDisposed;
	}
}
