using UnityEditor;
using UnityEngine;
using System.Collections;

#pragma warning disable 0414, 0108
#pragma warning disable 0618, 0168

[CustomEditor(typeof(JoystickToolkit))]
public class JoystickToolkitEditor : Editor
{
    private JoystickToolkit joystick;

    void OnEnable()
    {
        joystick = target as JoystickToolkit;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Set Start position"))
        {
            joystick.StartPosition = MCSimCoreToolkitHelper.SaveStateTransform(joystick.transform, "Position_Start");
        }

        if (joystick.StartPosition == null) return;

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Axes:");
                joystick.X = EditorGUILayout.Toggle("X:", joystick.X);
                joystick.Y = EditorGUILayout.Toggle("Y:", joystick.Y);
                joystick.Z = EditorGUILayout.Toggle("Z:", joystick.Z);
        EditorGUILayout.EndVertical();

        

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if(joystick.X)
        {
                EditorGUILayout.HelpBox("Axe 'X'", MessageType.None);

            EditorGUILayout.BeginHorizontal();
            joystick._state.X.Min = (short)EditorGUILayout.IntField("Minimal Value:", joystick._state.X.Min);
            joystick._state.X.Max = (short)EditorGUILayout.IntField("Maxmimum Value:", joystick._state.X.Max);
            EditorGUILayout.EndHorizontal();

            if (joystick._state.X.PositionMin != null && joystick._state.X.PositionMax != null)
            joystick._state.X.Value = EditorGUILayout.IntSlider(joystick._state.X.Value, joystick._state.X.Min, joystick._state.X.Max);

            EditorGUILayout.BeginHorizontal();
            joystick._state.X.PositionMin = (Transform)EditorGUILayout.ObjectField("", joystick._state.X.PositionMin, typeof(Transform));
            joystick._state.X.PositionMax = (Transform)EditorGUILayout.ObjectField("", joystick._state.X.PositionMax, typeof(Transform));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("Current as Min"))
            {
                joystick._state.X.PositionMin = MCSimCoreToolkitHelper.SaveStateTransform(joystick.transform, "Min_X");
            }

            if (GUILayout.Button("Current as Max"))
            {
                joystick._state.X.PositionMax = MCSimCoreToolkitHelper.SaveStateTransform(joystick.transform, "Max_X");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        if (joystick.Y)
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Axe 'Y'", MessageType.None);

            EditorGUILayout.BeginHorizontal();
            joystick._state.Y.Min = (short)EditorGUILayout.IntField("Minimal Value:", joystick._state.Y.Min);
            joystick._state.Y.Max = (short)EditorGUILayout.IntField("Maxmimum Value:", joystick._state.Y.Max);
            EditorGUILayout.EndHorizontal();


            if (joystick._state.Y.PositionMin != null && joystick._state.Y.PositionMax != null)
            joystick._state.Y.Value = EditorGUILayout.IntSlider(joystick._state.Y.Value, joystick._state.Y.Min, joystick._state.Y.Max);

            EditorGUILayout.BeginHorizontal();
            joystick._state.Y.PositionMin = (Transform)EditorGUILayout.ObjectField("", joystick._state.Y.PositionMin, typeof(Transform));
            joystick._state.Y.PositionMax = (Transform)EditorGUILayout.ObjectField("", joystick._state.Y.PositionMax, typeof(Transform));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Current as Min"))
            {
                joystick._state.Y.PositionMin = MCSimCoreToolkitHelper.SaveStateTransform(joystick.transform, "Min_Y");
            }

            if (GUILayout.Button("Current as Max"))
            {
                joystick._state.Y.PositionMax = MCSimCoreToolkitHelper.SaveStateTransform(joystick.transform, "Max_Y");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        if (joystick.Z)
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Axe 'Z'", MessageType.None);

            EditorGUILayout.BeginHorizontal();
            joystick._state.Z.Min = (short)EditorGUILayout.IntField("Minimal Value:", joystick._state.Z.Min);
            joystick._state.Z.Max = (short)EditorGUILayout.IntField("Maxmimum Value:", joystick._state.Z.Max);
            EditorGUILayout.EndHorizontal();


            if (joystick._state.Z.PositionMin != null && joystick._state.Z.PositionMax != null)
            joystick._state.Z.Value = EditorGUILayout.IntSlider(joystick._state.Z.Value, joystick._state.Z.Min, joystick._state.Z.Max);

            EditorGUILayout.BeginHorizontal();
            joystick._state.Z.PositionMin = (Transform)EditorGUILayout.ObjectField("", joystick._state.Z.PositionMin, typeof(Transform));
            joystick._state.Z.PositionMax = (Transform)EditorGUILayout.ObjectField("", joystick._state.Z.PositionMax, typeof(Transform));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Current as Min"))
            {
                joystick._state.Z.PositionMin = MCSimCoreToolkitHelper.SaveStateTransform(joystick.transform, "Min_Z");
            }

            if (GUILayout.Button("Current as Max"))
            {
                joystick._state.Z.PositionMax = MCSimCoreToolkitHelper.SaveStateTransform(joystick.transform, "Max_Z");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
