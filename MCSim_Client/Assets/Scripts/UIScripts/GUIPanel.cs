using System;
using UnityEngine;
using System.Collections;

public enum FormType
{
    None,
    Form,
    MainForm,
    SubForm,
    InfoForm,
    ConfirmDialog
}

public enum VectorDirection
{
    None,
    Up,
    Down,
    Right,
    Left
}

public class GUIPanel : GUIObject
{
	public string PageLabel = "";

	public GUIPanel[] SubPanels;

	// Метод, вызывающийся каждый кадр UICenter'ом и изменяющий подсказку наверху
	public virtual void TooltipSeeker() {}

	public FormType Form = FormType.Form;

	/// <summary>
	/// Время появления/исчезания окна
	/// </summary>
	protected float Duration = 0.5f;

	/// <summary>
	/// Лейбл-консоль данного окна
	/// </summary>
	protected UILabel console;

	/// <summary>
	/// В какую сторону окно выплывает при открытии
	/// </summary>
	protected Vector3 MoveOnShow = Vector3.up;

	/// <summary>
	/// Направление движения при закрытии
	/// </summary>
	protected Vector3 MoveOnClose = Vector3.right;

	/// <summary>
	/// Направление движения при закрытии
	/// </summary>
	protected Vector3 MoveOnBack = Vector3.left;

	/// <summary>
	/// Направление движения при закрытии
	/// </summary>
	protected Vector3 MoveOnApply = Vector3.left;
	
	public VectorDirection OnShowFrom;

	public VectorDirection OnCloseTo;

	public VectorDirection OnApplyTo;

	public VectorDirection OnBackTo;

	public bool SingleInGroup;

	/// <summary>
	/// Если установлено значение true - MainMenu будет скрываться при появлении меню с тегом SubMenu
	/// </summary>
	public bool SubMenu = true;

	/// <summary>
	/// Статическое меню - отключена анимация при появлении и исчезании
	/// </summary>
	public bool Static = false;

	/// <summary>
	/// Группа, к которой принадлежит окно
	/// </summary>
	public string Group = "none";

	/// <summary>
	/// Начальная позиция окна при инициализации
	/// </summary>
	private Vector3 _startPos;

	public static void ShowConfirmationDialog(string title, string text, OnWindowResponse callback)
	{
		UICenter.ShowConfirmationDialog(title, text, callback);
	}

	/// <summary>
	/// Размещение панели
	/// </summary>
	/// <param name="panelName">Имя панели</param>
	public static GUIPanel Instantiate(string panelName)
	{
		return UICenter.Panels.Instantiate(panelName);
	}

	public static GUIPanel Instantiate(string panelName, string group)
	{
		return UICenter.Panels.Instantiate(panelName, group);
	}

	public string Console
	{
		set
		{
			if (console)
				console.text = value;
		}
	}

	public virtual void Awake()
	{
		SubscribeEvents();

		console = GetControl<UILabel>("Console");

		// Подписываемся на события

		MoveOnShow = DirectionToVector3(OnShowFrom);
		MoveOnClose = DirectionToVector3(OnCloseTo);
		MoveOnApply = DirectionToVector3(OnApplyTo);
		MoveOnBack = DirectionToVector3(OnBackTo);

		if (SingleInGroup) UICenter.Panels.CloseGroupExceptPanel(Group, this);

		SetColliders(false);

        //var anchor = GetComponent<UIAnchor>();
        //if (anchor) Destroy(anchor);

		_startPos = transform.localPosition;

		var panel = GetComponent<UIPanel>();
		if (panel) panel.enabled = false;

		InitializeButtons();
	}

	/// <summary>
	/// Метод, который будет вызван при загрузке страницы для определения ее состояния
	/// </summary>
	protected virtual void InitializePageInfo()
	{
	}

	/// <summary>
	/// Определить кнопки
	/// </summary>
	protected virtual void InitializeButtons()
	{
	}

	/// <summary>
	/// Открыть окно с эффектами анимации
	/// </summary>
	public void Show()
	{
		var panel = GetComponent<UIPanel>();
		if (panel) panel.enabled = true;

		var sp = gameObject.ForceComponent<TweenPosition>();
		sp.method = UITweener.Method.EaseIn;
		sp.duration = 0.5f;
		sp.from = sp.position;
		sp.to = _startPos;
		sp.onFinished = OnShowCompleted;
	}


	public void Hide()
	{
		SetColliders(false);

		UIWidget[] widgets = GetComponentsInChildren<UIWidget>();
		
		var sp = gameObject.ForceComponent<TweenPosition>();
		sp.duration = 0.5f;
		sp.from = sp.position;
		sp.to = _startPos + MoveOnShow * 300f;
		sp.onFinished = Destroy;
	}

	private void OnFinishedClose(SpringPosition sp)
	{
		Destroy(gameObject);
	}

	protected virtual void OnShowCompleted(UITweener tween)
	{
		Destroy(tween);
		SetColliders(true);

		foreach (GUIPanel panel in SubPanels)
		{
			if (panel == null) continue;
		 	panel.Show();
		}
	}

	/// <summary>
	/// Закрытие окна/панели
	/// </summary>
	public virtual void Back()
	{
		//StartCoroutine(FreezeColliders(0));
		SetColliders(false);

		//Debug.Log("Back");
		UICenter.Panels.Remove(this);

		CloseSubpanels();
		MoveAndDestroy(MoveOnBack, 0.3f);
	}

	public bool isClosing;

	/// <summary>
	/// Закрытие окна/панели
	/// </summary>
	public virtual void Close()
	{
		UICenter.Panels.Remove(this);
		//if(isClosing) return;
		//Debug.Log("Прошли до закрытия" + Name + "/ " + name);
		isClosing = true;

		SetColliders(false);

		CloseSubpanels();
		MoveAndDestroy(MoveOnClose, 0.6f);

		//Debug.Log("Close");
	}

	public virtual void Apply(GameObject go)
	{
		isClosing = true;
		//Debug.Log("Apply");

		CloseSubpanels();
		UICenter.Panels.Remove(this);

		MoveAndDestroy(MoveOnApply, 0.3f);
	}

	private void MoveAndDestroy(Vector3 direction, float t)
	{
		// Если на объекте анхор - надо уничтожить

		foreach (var anchor in GetComponentsInChildren<UIAnchor>())
		{
			Destroy(anchor);
		}

		var sp = gameObject.ForceComponent<SpringPosition>();

		sp.onFinished = OnFinishedClose;

		sp.target = transform.localPosition + direction*Screen.width;
		sp.strength = 4f;

		sp.enabled = true;

		UIWidget[] widgets = GetComponentsInChildren<UIWidget>();


		if (SubMenu)
		{
			try
			{
				// UICenter.Panels["Panel_MainMenu"].GetComponent<GUI_Panel_MainMenu>().MoveDown();
			}
			catch
			{
			}
		}
	}

	private void SetColliders(bool value)
	{
		try
		{
			Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();

			foreach (Collider cldr in colliders)
			{
				cldr.enabled = value;
			}

			UIButton[] buttons = gameObject.GetComponentsInChildren<UIButton>();

			foreach (UIButton btn in buttons)
			{
				btn.isEnabled = true;
			}
		}
		catch
		{
		}
	}

	//private IEnumerator FreezeColliders(float t)
	//{
	//    Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();

	//    foreach (Collider cldr in colliders)
	//    {
	//        cldr.enabled = false;
	//    }

	//    if (t > 0)
	//    {
	//        yield return new WaitForSeconds(t);

	//        foreach (Collider cldr in colliders)
	//        {
	//            cldr.enabled = true;
	//        }
	//    }
	//}

	protected Vector3 DirectionToVector3(VectorDirection dir)
	{
		switch (dir)
		{
			case VectorDirection.Down:
				return Vector3.down;

			case VectorDirection.Up:
				return Vector3.up;

			case VectorDirection.Left:
				return Vector3.left;

			case VectorDirection.Right:
				return Vector3.right;


			case VectorDirection.None:
				return Vector3.forward;

			default:
				return Vector3.forward;
		}
	}

	public void CloseSubpanels()
	{
		foreach (GUIPanel panel in SubPanels)
		{
			try
			{
				panel.Close();
			}
			catch
			{
				Debug.LogError("Ошибка при закрытии вспомогательных панелей в " + Name);
			}
		}
	}

	protected virtual void OnDestroy()
	{
		UnsubscribeEvents();

		try
		{
			UICenter.Panels.Remove(this);
		}
		catch (Exception)
		{
		}
	}

	private void SubscribeEvents()
	{
	}

	private void UnsubscribeEvents()
	{
	}

	/// <summary>
	/// Получить координатную сетку для элементов, которые должны располагаться на панели.
	/// </summary>
	/// <param name="tPaddingX">Общий отступ от краев по горизонтали</param>
	/// <param name="tPaddingY">Общий отступ от краев по вертикали</param>
	/// <param name="paddingX">Горизонтальный отступ между элементами</param>
	/// <param name="paddingY">Вертикальный отступ между элементами</param>
	/// <param name="rows">Количество строк</param>
	/// <returns></returns>
	protected Vector3[] GetPointsGrid(float tPaddingX, float tPaddingY, float paddingX, float paddingY, int columns, int rows)
	{
		//Debug.Log(Screen.width + " : " + Screen.height);

		Transform background = transform.FindChild("Background");

		var tWidth = Screen.width - 2*tPaddingX; // Доступное по ширине пространство под элементы

		int tHorCount = Mathf.FloorToInt(tWidth/paddingX); // Столько элементов может расположиться по горизонтали
		tHorCount = (tHorCount > columns ? columns : tHorCount);

		float tHorSize = tHorCount*paddingX; // Расстояние между первым и последним элементом в строке

		Vector3 cursor = Vector3.zero; // Текущее положение курсора

		var grid = new Vector3[(int) (tHorCount*rows)];
		int i = 0;

		// cRow - Текущая строка
		for (int cRow = 0; cRow < rows; cRow++)
		{
			cursor = new Vector3(-(tHorSize - paddingX)/2f, background.transform.localScale.y / 2f - tPaddingY - cRow*paddingY + background.transform.localPosition.y, 0);

			// cCol - Текущий столбец
			for (int cCol = 0; cCol < tHorCount; cCol++)
			{
				grid[i++] = cursor + Vector3.right*paddingX*cCol;
			}
		}

		return grid;
	}


	///// <summary>
	///// Получить координатную сетку для элементов, которые должны располагаться на панели.
	///// </summary>
	///// <param name="tPaddingX">Общий отступ от краев по горизонтали</param>
	///// <param name="tPaddingY">Общий отступ от краев по вертикали</param>
	///// <param name="paddingX">Горизонтальный отступ между элементами</param>
	///// <param name="paddingY">Вертикальный отступ между элементами</param>
	///// <param name="rows">Количество строк</param>
	///// <returns></returns>
	//protected Vector3[] GetPointsGrid(float tPaddingX, float tPaddingY, float paddingX, float paddingY, float rows)
	//{
	//   //Debug.Log(Screen.width + " : " + Screen.height);

	//   Transform background = transform.FindChild("Background");

	//   var tWidth = Screen.width - 2 * tPaddingX; // Доступное по ширине пространство под элементы

	//   int tHorCount = Mathf.FloorToInt(tWidth / paddingX); // Столько элементов может расположиться по горизонтали
	//   float tHorSize = tHorCount * paddingX; // Расстояние между первым и последним элементом в строке

	//   Vector3 cursor = Vector3.zero; // Текущее положение курсора

	//   var grid = new Vector3[(int)(tHorCount * rows)];
	//   int i = 0;

	//   // cRow - Текущая строка
	//   for (int cRow = 0; cRow < rows; cRow++)
	//   {
	//      cursor = new Vector3(-(tHorSize - paddingX) / 2f, background.transform.localScale.y / 2f - tPaddingY - cRow * paddingY, 0);

	//      // cCol - Текущий столбец
	//      for (int cCol = 0; cCol < tHorCount; cCol++)
	//      {
	//         grid[i++] = cursor + Vector3.right * paddingX * cCol;
	//      }
	//   }

	//   return grid;
	//}
}
