using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class UIPanel_TrainingCenter : GUIPanel
{
	public Transform Table;
	public GameObject ElementPrefab;

	Dictionary<MCSPlayer, GameObject> _elements = new Dictionary<MCSPlayer, GameObject>();

	void Start()
	{
		MCSNetworkCenter.OnParticipantConnected += AddPlayer;
		MCSNetworkCenter.OnParticipantDisconnected += RemovePlayer;

		RefreshPlayers();

		Show();
	}
	
	private void AddPlayer(MCSPlayer player)
	{
		var element = Instantiate(ElementPrefab) as GameObject;

		if (element)
		{
			element.transform.parent = Table.transform;
			element.transform.localScale = Vector3.one;
			element.transform.localPosition = Vector3.zero;
			element.name = string.Concat("element (", player.Account.FirstName, ")");

			element.SendMessage("Set", player);
			_elements.Add(player, element);

			Table.SendMessage("Reposition");
		}
		else
		{
			Debug.LogError("Не удалось разместить игрока в списке");
		}
	}

	private void RemovePlayer(MCSPlayer player)
	{
		try
		{
			DestroyImmediate(_elements[player]);

			Table.SendMessage("Reposition");
		}
		catch
		{
			Debug.LogWarning("В коллекции элементов отсутствует элемент, связанный с удаляемым игроком");
		}
	}

	private void ClearList()
	{
		foreach (var element in _elements)
		{
			Destroy(element.Value.gameObject);
		}
			
		_elements.Clear();
	}

	/// <summary>
	/// Запустить все экзамены
	/// </summary>
	void StartAll()
	{
		
	}

	/// <summary>
	/// Отменить все экзамены
	/// </summary>
	void CancelAll()
	{
		
	}

	/// <summary>
	/// Обновить список игроков
	/// </summary>
	void RefreshPlayers()
	{
		ClearList();

		foreach (var player in MCSGlobalSimulation.Players.List.Values)
		{
			AddPlayer(player);
		}

		Table.SendMessage("Reposition");
	}

	new void OnDestroy()
	{
		base.OnDestroy();

		MCSNetworkCenter.OnParticipantConnected -= AddPlayer;
		MCSNetworkCenter.OnParticipantDisconnected -= RemovePlayer;
	}
}
