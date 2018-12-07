using UnityEngine;
using System.Collections;

/// <summary>
/// Делегат события изменения разрешения экрана
/// </summary>
/// <param name="width">Ширина экрана</param>
/// <param name="height">Высота экрана</param>
public delegate void OnResolutionChanged(float width, float height);

public class UIBehaviour : MonoBehaviour {

	void Awake()
	{
	}

	private static Vector2 _resolution = Vector2.zero;

	/// <summary>
	/// Текущее разрешение экрана
	/// </summary>
	public static Vector2 Resolution
	{
		//get { return (_resolution == Vector2.zero ? (_resolution = ProjectHelper.GetMainGameViewSize()) : _resolution); }

		get { return new Vector2(Screen.width, Screen.height); }

		set { _resolution = value; }
	}
}
