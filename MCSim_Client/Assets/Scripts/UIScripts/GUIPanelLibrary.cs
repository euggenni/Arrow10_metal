using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class GUIPanelLibrary : MonoBehaviour
{
	public Transform Anchor;

	public Dictionary<string, GUIPanel> List = new Dictionary<string, GUIPanel>();


	public void Add(GUIPanel panel)
	{
		try
		{
			if (!List.ContainsValue(panel))
			{
				List.Add(panel.Name, panel);
			}
		}
		catch
		{
			Debug.LogError("Panel " + panel.name + " is already in list");
		}
	}

	/// <summary>
	/// Закрыть группу с указанными типами панелей
	/// </summary>
	public void CloseAll()
	{

		List<GUIPanel> list = List.Select(pair => pair.Value).ToList();

		foreach (GUIPanel guiPanel in list)
		{
			guiPanel.Close();
		}
	}

	/// <summary>
	/// Закрыть группу с указанными типами панелей
	/// </summary>
	/// <param name="types"></param>
	public void CloseGroup(params FormType[] types)
	{
		List<GUIPanel> list = new List<GUIPanel>();

		foreach (KeyValuePair<string, GUIPanel> pair in List)
		{
			Debug.Log(pair.Value.Form);
			if (pair.Value.Form == FormType.None || pair.Value.Form == FormType.ConfirmDialog) continue;

			if (types.Contains(pair.Value.Form)) list.Add(pair.Value);
		}

		foreach (GUIPanel guiPanel in list)
		{
			guiPanel.Close();
		}
	}

	/// <summary>
	/// Закрыть группу панелей, за исключением панелей типа...
	/// </summary>
	public void CloseGroupExcept(params FormType[] types)
	{
		List<GUIPanel> list = new List<GUIPanel>();

		foreach (KeyValuePair<string, GUIPanel> pair in List)
		{
			//Debug.Log(pair.Value.Form);
			if (pair.Value.Form == FormType.None || pair.Value.Form == FormType.ConfirmDialog) continue;

			if (!types.Contains(pair.Value.Form)) list.Add(pair.Value);
		}

		foreach (GUIPanel guiPanel in list)
		{
			guiPanel.Close();
		}
	}

	/// <summary>
	/// Закрыть группу панелей, за исключением панелей типа...
	/// </summary>
	public void CloseGroupExcept(params string[] types)
	{
		List<GUIPanel> list = new List<GUIPanel>();

		foreach (KeyValuePair<string, GUIPanel> pair in List)
		{
			if (!types.Contains(pair.Value.Group)) list.Add(pair.Value);
		}

		foreach (GUIPanel guiPanel in list)
		{
			guiPanel.Close();
		}
	}

	/// <summary>
	/// GameObject панели с указанным именем. 
	/// </summary>
	/// <param name="panelName"></param>
	/// <returns></returns>
	public GUIPanel this[string panelName]
	{
		get
		{
			try
			{
				if (List.ContainsKey(panelName))
					return List[panelName];
			}
			catch(Exception e)
			{
				Debug.LogError(e.Message);
			}
			//else Debug.LogWarning("no panel with name " + panelName);
			return null;
		}
	}

	/// <summary> Создать панель </summary>
	public GUIPanel Instantiate(string panelName)
    {
        Debug.Log(">>>" + panelName);
		if (this[panelName])
		{
			Debug.LogWarning("Panel " + panelName + " is already exist!");
			return this[panelName];
		}
		try
        {
			GameObject panel = Instantiate(Resources.Load("UIPrefabs/Panels/" + panelName)) as GameObject;
			Vector3 pos = panel.transform.localPosition;
			if (panel)
			{
				panel.transform.parent = Anchor.transform;
				panel.transform.localPosition = new Vector3(pos.x, pos.y, 0);
				//panel.transform.localPosition = Vector3.zero;
				panel.transform.localScale = Vector3.one;
				GUIPanel guip = panel.GetComponent<GUIPanel>();
				guip.Name = panelName;
				List.Add(panelName, panel.GetComponent<GUIPanel>());
				
				return guip;
			}
			else
			{
				Debug.LogError("Не найдена панель с именем [" + panelName + "]");
				return null;
			}
		}
		catch (ArgumentException)
		{
			Debug.LogWarning("Не существует панели с именем " + panelName);
            throw;
		}

		return null;
	}

	public GUIPanel Instantiate(string panelName, string group)
	{
		GUIPanel guip = UICenter.Panels.Instantiate(panelName);
		guip.Group = group;

		return guip;
	}

	private void OnGUI()
	{
		//foreach (var panel in List)
		//{
		//    GUILayout.TextArea(string.Concat(panel.Key, " - ", panel.Value.Group));
		//}

		//if(GUILayout.Button("AddGroup"))
		//{
		//    ChessGUI.Panels.Instantiate("Panel_SetColor");
		//    ChessGUI.Panels.Instantiate("Panel_SetRole");
		//}
		//if (GUILayout.Button("GetGroup"))
		//{
		//    Debug.Log(Group("PreparationForm").Count);
		//}
	}

	/// <summary> Возвращает панели указанной группы </summary>
	/// <param name="groupName">Имя группы</param>
	public List<GUIPanel> Group(string groupName)
	{
		List<GUIPanel> respanels = new List<GUIPanel>();

		var res = (from panel in List
		           where panel.Value.Group.Equals(groupName)
		           select panel.Value);

		foreach (GUIPanel panel in res)
		{
			respanels.Add(panel);
		}

		return respanels;
	}

	/// <summary> Закрыть группу панелей </summary>
	public void CloseGroup(string groupName)
	{
		List<GUIPanel> panelsGroup = Group(groupName);

		if (panelsGroup == null)
		{
			Debug.LogWarning("Отсутствуют панели группы " + groupName);
			return;
		}

		foreach (GUIPanel panel in panelsGroup)
		{
			//Debug.Log("Closing panel [" + panel.Name + "]");
			if (panel)
				panel.Close();
		}
	}

	public void CloseGroupExceptPanel(string groupName, GUIPanel except)
	{
		List<GUIPanel> panelsGroup = Group(groupName);

		if (panelsGroup == null)
		{
			Debug.LogWarning("Отсутствуют панели группы " + groupName);
			return;
		}

		foreach (GUIPanel panel in panelsGroup)
		{
			//Debug.Log("Closing panel [" + panel.Name + "]");
			if (panel && !panel.Equals(except))
				panel.Close();
		}
	}

	/// <summary> Уничтожить панель </summary>
	public void Remove(string panelName)
	{
		if (List.ContainsKey(panelName))
		{
			Destroy(List[panelName].gameObject);
			List.Remove(panelName);
		}
	}

	/// <summary> Уничтожить панель </summary>
	public void Remove(GUIPanel panel)
	{
		if (List.ContainsValue(panel))
		{
			List.Remove(panel.Name);
			//Destroy(panel.gameObject);
		}
	}
}