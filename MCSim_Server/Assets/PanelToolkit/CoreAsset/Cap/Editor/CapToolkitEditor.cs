using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 0618

[CustomEditor(typeof(CapToolkit))]
public class CapToolkitEditor : Editor {

    
    [SerializeField]
    CapToolkit cap;     // ������ ��������

    void OnEnable()
    {
        cap = target as CapToolkit;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        
        #region Rotations

        EditorGUILayout.Foldout(true, "Start Position");

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
            cap.StartRotation = (Transform)EditorGUILayout.ObjectField("Start:", cap.StartRotation, typeof(Transform));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        //��������� ������� ������� ��� ���������
        if (GUILayout.Button("Apply Current"))
        {
            // �������� ����� ��� ������������ �������
            GameObject directory = GameObject.Find(cap.name + "(States)");

            // ���� ��� ��� - ������� �����
            if (directory == null)
            {
                directory = new GameObject();
                directory.transform.parent = cap.transform.parent.transform;
                directory.name = cap.name + "(States)";
            }

            GameObject newTransform = new GameObject();
            newTransform.gameObject.name = "Position_Start";
            newTransform.transform.parent = directory.transform;
            newTransform.transform.localRotation = cap.transform.localRotation;
            newTransform.transform.rotation = cap.transform.rotation;
            cap.StartRotation = newTransform.transform;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();


        EditorGUILayout.Foldout(true, "End Position");

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        cap.EndRotation = (Transform)EditorGUILayout.ObjectField("Transform:", cap.EndRotation, typeof(Transform));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        //��������� ������� ������� ��� ���������
        if (GUILayout.Button("Apply Current"))
        {
            // �������� ����� ��� ������������ �������
            GameObject directory = GameObject.Find(cap.name + "(States)");

            // ���� ��� ��� - ������� �����
            if (directory == null)
            {
                directory = new GameObject();
                directory.transform.parent = cap.transform.parent.transform;
                directory.name = cap.name + "(States)";
            }

            GameObject newTransform = new GameObject();
            newTransform.gameObject.name = "Position_End";
            newTransform.transform.parent = directory.transform;
            newTransform.transform.localRotation = cap.transform.localRotation;
            newTransform.transform.rotation = cap.transform.rotation;
            cap.EndRotation = newTransform.transform;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();





        #endregion

        cap.Seconds = EditorGUILayout.FloatField("Seconds:", cap.Seconds);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
