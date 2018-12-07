using UnityEditor;
using UnityEngine;
using System.Collections;

#pragma warning disable 0618
#pragma warning disable 0414

[CustomEditor(typeof(CoreToolkit))]
public class CoreToolkitEditor : Editor {

    [SerializeField]
    private CoreToolkit _core;

    void OnEnable() {
        _core = target as CoreToolkit;
    }

    public override void OnInspectorGUI()
    {
        if(_core.isVirtual) return;

        if (_core.Library == null)
        {
            _core.SetLibraryObject(EditorGUILayout.ObjectField("Libray:", _core.GetLibraryObject(), typeof(UnityEngine.Object)));
        }
        else
        {
            if (GUILayout.Button("Remove " + _core.Library.Name)) {
                _core.RemoveLibrary(); // Удаляем текущую библиотеку для это панели
                return;
            }

            for (int i = 0; i < _core.Panels.Count; i++)
            {
                EditorGUILayout.Separator();

                EditorGUILayout.BeginHorizontal();
                
                EditorGUILayout.LabelField("", "[" + _core.Library.GetPanelNames()[i] + "]");

                _core.SetPanelObject(i, (GameObject)EditorGUILayout.ObjectField(_core.GetPanelObject(i), typeof(GameObject)));


                if (_core.GetPanelObject(i) != null)
                    if (GUILayout.Button("Remove", GUILayout.MinHeight(15), GUILayout.MaxHeight(15)))
                    {
                        _core.RemovePanel(i);
                    }
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.Separator();
            
            EditorGUILayout.LabelField("", "CoreHandler for [" + _core.Library.Name + "]");
            _core.HandlerObject = (GameObject)EditorGUILayout.ObjectField(_core.HandlerObject, typeof(GameObject));
        }

    }

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
