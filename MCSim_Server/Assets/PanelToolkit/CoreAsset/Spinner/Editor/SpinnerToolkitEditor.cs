using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 0414
#pragma warning disable 0618

[CustomEditor(typeof(SpinnerToolkit))]
public class SpinnerToolkitEditor : Editor {

    [SerializeField]
    SpinnerToolkit spinner;     // Объект тумблера

    private int min = 0, max = 100;

    void OnEnable()
    {
        spinner = target as SpinnerToolkit;
    }

    public override void OnInspectorGUI()
    {
        if (spinner.getParentPanelScript() != null) 
        {
            EditorGUILayout.LabelField("Parent Panel: ", "");
            EditorGUILayout.ObjectField(spinner.getParentPanelScript().transform, typeof(Transform));

            EditorGUILayout.Space();
            EditorGUILayout.Foldout(true, "Properties");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            spinner.isManual = EditorGUILayout.Toggle("Manual", spinner.isManual);
            spinner.isCyclical = EditorGUILayout.Toggle("Cyclical", spinner.isCyclical);
            spinner.Inverse = EditorGUILayout.Toggle("Inverse", spinner.Inverse);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            spinner.Axe = (OPTIONS) EditorGUILayout.EnumPopup("Axe to rotate:", spinner.Axe);
            EditorGUILayout.Space();
            EditorGUILayout.Space();


            EditorGUILayout.Foldout(true, "Visualisation");
            spinner.showSlider = EditorGUILayout.Toggle("Show Slider", spinner.showSlider);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            #region Rotations

            EditorGUILayout.Foldout(true, "Start Position");

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            spinner.StartRotation = (Transform)EditorGUILayout.ObjectField("Transform:", spinner.StartRotation, typeof(Transform));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            //Применить текущую позицию как состояние
            if (GUILayout.Button("Apply Current"))
            {
                // Пытаемся найти уже существующий каталог
                GameObject directory = GameObject.Find(spinner.name + "(States)");

                // Если его нет - создаем новый
                if (directory == null)
                {
                    directory = new GameObject();
                    directory.transform.parent = spinner.transform.parent.transform;
                    directory.name = spinner.name + "(States)";
                }

                GameObject newTransform = new GameObject();
                newTransform.gameObject.name = "Position_Start";
                newTransform.transform.parent = directory.transform;
                newTransform.transform.localRotation = spinner.transform.localRotation;
                newTransform.transform.rotation = spinner.transform.rotation;
                spinner.StartRotation = newTransform.transform;
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Foldout(true, "End Position");

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            spinner.EndRotation = (Transform)EditorGUILayout.ObjectField("Transform:", spinner.EndRotation, typeof(Transform));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            //Применить текущую позицию как состояние
            if (GUILayout.Button("Apply Current"))
            {
                // Пытаемся найти уже существующий каталог
                GameObject directory = GameObject.Find(spinner.name + "(States)");

                // Если его нет - создаем новый
                if (directory == null)
                {
                    directory = new GameObject();
                    directory.transform.parent = spinner.transform.parent.transform;
                    directory.name = spinner.name + "(States)";
                }

                GameObject newTransform = new GameObject();
                newTransform.gameObject.name = "Position_End";
                newTransform.transform.parent = directory.transform;
                newTransform.transform.localRotation = spinner.transform.localRotation;
                newTransform.transform.rotation = spinner.transform.rotation;
                spinner.EndRotation = newTransform.transform;
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();





            #endregion

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            min = EditorGUILayout.IntField("Minimal Value:", min);
            max = EditorGUILayout.IntField("Maximal Value:", max);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Apply"))
            {
                spinner.MinimalValue = min;
                spinner.MaximalValue = max;
                spinner.CalculateDimension();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();



            EditorGUILayout.Space();
            
            spinner.Value = EditorGUILayout.IntSlider(spinner.Value, spinner.MinimalValue, spinner.MaximalValue);
        }
        else
        {
            EditorGUILayout.TextArea("This object is not connected with parent,\nwhich have ControlPanelToolkit script.\nCreate new object with ControlPanelToolkit\nand add object with SpinnerToolkit\nscript to him as tumbler.", GUILayout.ExpandHeight(true), GUILayout.MaxHeight(100));
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(spinner);
        }

    }

    void OnDestroy()
    {
        /* Вызывается при удалении компонента и смене фокуса на другой объект
        при вызове этого метода, в списке тумблеров панели находим текущий индикатор, и если
        на нем нет SpinnerToolkit - удаляем его GameObject */

        // Если нет родительской панели - ничего не делаем
        if (spinner.getParentPanelScript() == null) return;
        try
        {
            // Если на тумблере не висит скрипта SwitcherToolkit - удаляем его с панели
            if (spinner.getParentPanelScript().SpinnerScripts[spinner.SpinnerID] == null)
            {
                Debug.Log("Spinner [" + spinner.getParentPanelScript().GetSpinners()[spinner.SpinnerID].GetName() + "] was deleted from panel [" + spinner.getParentPanelScript().transform.name + "]");
                spinner.getParentPanelScript().RemoveTumbler(ControlType.Spinner, spinner.SpinnerID);
            }
        }
        catch 
        {
           // Debug.Log("ParentPanel havent script with this ID");
        }
    }
}
