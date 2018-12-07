using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public enum WindowResponse
{
    Cancel,
    Ok
}

public delegate void SeekForTooltip();

/// <summary>
/// Событие, возникающее при закрытии диалогового окна
/// </summary>
public delegate void OnWindowResponse(WindowResponse response);

public class UICenter : MonoBehaviour
{
	/// <summary>
	/// Контроллер для создания новых панелей и хранения информации о них
	/// </summary>
	public static GUIPanelLibrary Panels;


	public static Camera UICamera;
	public static Camera ARCamera;
	private static Light _topLight;

	private static LayerMask _layer2D, _layer3D;

	public static GameObject Popup, Root;

	public static readonly Vector2 nativeResolution = new Vector2(1440, 900);

	private static Dictionary<string, Vector2> _borders = new Dictionary<string, Vector2>();

	private Ray ray;
	private RaycastHit hit;

	// Use this for initialization
	private void Awake()
	{
		UICamera = GameObject.Find("GUI Camera").GetComponent<Camera>();
		Panels = GetComponent<GUIPanelLibrary>();

		Root = GameObject.Find("UI Root (2D)");
	}

	void Update()
	{
		//cursorPosition = Input.mousePosition;
	}
	
	public static Vector2 GetBorder(string borderName)
	{
		try
		{
			return _borders[borderName];
		}
		catch
		{
			//Debug.LogWarning("There is no border in collection with name [" + borderName + "]");
			return Vector2.zero;
		}
	}

	public static void SetBorder(string borderName, Vector2 pos)
	{
		try
		{
			_borders[borderName] = pos;
		}
		catch
		{
			_borders.Add(borderName, pos);
		}
	}


	public static string Tooltip
	{
		get { return Panels.Tooltip.text; }
		set { Panels.Tooltip.text = value.ToUpper(); }
	}

	/// <summary>
	/// Коэффицент разрешения экрана по отношению к исходному по ширине
	/// </summary>
	public static float resolutionCoefficientX
	{
		get { return Resolution.x / nativeResolution.x; }
	}

	/// <summary>
	/// Коэффицент разрешения экрана по отношению к исходному по высоте
	/// </summary>
	public static float resolutionCoefficientY
	{
		get { return Resolution.y / nativeResolution.y; }
	}

	public static float CursorCoefficient = 1.3f;



	public static Vector2 Resolution
	{
		get { return UIBehaviour.Resolution; }
	}



	/// <summary>
	/// Создание диалогового окна
	/// </summary>
	/// <param name="title">Заголовок диалогового окна</param>
	/// <param name="text">Текст диалогового окна</param>
	/// <param name="callback">Метод, который будет вызван при закрытии окна</param>
	public static void ShowConfirmationDialog(string title, string text, OnWindowResponse callback)
	{
		GUIPanel go = Panels.Instantiate("Panel_Confirm", "PreparationForm");

		//GUI_Panel_Confirm win = go.GetComponent<GUI_Panel_Confirm>();
		//win.Set(title, text, callback);
	}

	/// <summary>
	/// Создание информационного окна
	/// </summary>
	/// <param name="title">Заголовок информационного окна</param>
	/// <param name="text">Текст информационного окна</param>
	public static GUIPanel ShowInfoWindow(string title, string text)
	{
		GUIPanel go = Panels.Instantiate("Panel_ShowInfo", "InfoForm" + UnityEngine.Random.Range(1, 100));

		//GUI_Panel_Confirm win = go.GetComponent<GUI_Panel_Confirm>();
		//win.Set(title, text, null);

		return go;
	}

	public void OpenTrainingCenter()
	{
		Panels.Instantiate("UIPanel_TrainingCenter").Show();
	}
}
