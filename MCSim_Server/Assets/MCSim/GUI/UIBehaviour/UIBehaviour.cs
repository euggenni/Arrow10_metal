using UnityEngine;
using System.Collections;

/// <summary>
/// ������� ������� ��������� ���������� ������
/// </summary>
/// <param name="width">������ ������</param>
/// <param name="height">������ ������</param>
public delegate void OnResolutionChanged(float width, float height);

public class UIBehaviour : MonoBehaviour {

	void Awake()
	{
	}

	private static Vector2 _resolution = Vector2.zero;

	/// <summary>
	/// ������� ���������� ������
	/// </summary>
	public static Vector2 Resolution
	{
		//get { return (_resolution == Vector2.zero ? (_resolution = ProjectHelper.GetMainGameViewSize()) : _resolution); }

		get { return new Vector2(Screen.width, Screen.height); }

		set { _resolution = value; }
	}
}
