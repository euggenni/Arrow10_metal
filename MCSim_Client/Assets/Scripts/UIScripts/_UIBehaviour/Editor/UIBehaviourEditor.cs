using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(UIBehaviour))]
public class UIBehaviourEditor : Editor {

	private UIBehaviour _controller;

	void Start()
	{
		Debug.Log("asdsad");
	}

	void OnEnable()
	{
		_controller = target as UIBehaviour;
	}
	
	public override void OnInspectorGUI()
	{
		EditorGUILayout.LabelField("Script is setting up behaviour of all ui elements in project");

		//if (Application.isEditor)
		//{
		//   Vector2 _res = ProjectHelper.GetMainGameViewSize();

		//   if (UIBehaviour.Resolution != _res)
		//   {
		//      Debug.Log("Resolution has been changed");

		//      UIBehaviour.Resolution = _res;
		//      _controller.InvokeOnResolutionChangedInChildrens(_res.x, _res.y);
		//   }
		//}

		if (GUILayout.Button("Update UI"))
		{
			UpdateUI();
		}
	}

	[MenuItem("UIBehaviour/Update UI #&r")]
	private void UpdateUI()
	{
		Vector2 _res = ProjectHelper.GetMainGameViewSize();

		UIBehaviour.Resolution = _res;
		_controller.InvokeOnResolutionChangedInChildrens(_res.x, _res.y);
	}
}
