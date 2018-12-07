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

/// <summary>
/// Класс для управления интерфейсом приложения, позволяет создавать новые панели
/// </summary>
public class UICenter : MonoBehaviour
{
	/// <summary>
	/// Контроллер для создания новых панелей и хранения информации о них
	/// </summary>
	public static GUIPanelLibrary Panels;
    
	public static Camera MainCamera, UICamera;

	private static LayerMask _layer2D, _layer3D;

	public static GameObject Popup, Root;

	public static readonly Vector2 nativeResolution = new Vector2(1440, 900);

	private static Dictionary<string, Vector2> _borders = new Dictionary<string, Vector2>();

	private Ray ray;
	private RaycastHit hit;
	// Use this for initialization

	private void Awake()
    {
        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        Panels = GetComponent<GUIPanelLibrary>();
        //Panels.Anchor = transform.Find("Anchor").transform;

		Root = GameObject.Find("UI Root (2D)");
	}

    void OnConnectedToServer()
    {       
        // Панель с запросом на тренировку
    }
	
	public static Vector2 GetBorder(string borderName)
	{
		try
		{
			return _borders[borderName];
		}
		catch
		{
			Debug.LogWarning("There is no border in collection with name [" + borderName + "]");
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

	private static Vector2 _cursorPosition;
	public static Vector2 cursorPosition
	{
		set { 
			_cursorPosition = value;
		}
		get
        {
            return Input.mousePosition;
		}
		//get { return Input.mousePosition; }
	}

	public static Vector2 Resolution
	{
		get { return UIBehaviour.Resolution; }
	}

	public static bool useMouse = false;

	void OnGUI()
	{
		useMouse = GUI.Toggle(new Rect(10, 50, 100, 25), useMouse, "Use Mouse");
	}

	void Update()
	{
		//cursorPosition = Input.mousePosition;
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
}
