using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(UISlicedSpriteController))]
public class UISlicedSpriteControllerEditor : Editor
{
	private UISlicedSpriteController _controller;

	void OnEnable()
	{
		_controller = target as UISlicedSpriteController;
	}

	public override void OnInspectorGUI()
	{
		_controller.WidthPercent = EditorGUILayout.FloatField("Ширина %:", _controller.WidthPercent);
		_controller.HeightPercent = EditorGUILayout.FloatField("Высота %:", _controller.HeightPercent);

		_controller.HorizontalTransform = EditorGUILayout.Toggle("Ширина", _controller.HorizontalTransform);
		_controller.VerticalTransform = EditorGUILayout.Toggle("Высота", _controller.VerticalTransform);
	}
}
