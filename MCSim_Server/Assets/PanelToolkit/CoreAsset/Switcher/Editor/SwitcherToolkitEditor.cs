#pragma warning disable 0618

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;


[CustomEditor(typeof(SwitcherToolkit))]
public class SwitcherToolkitEditor : Editor
{
    // ссылка на компонент
    SwitcherToolkit switcher;

    void OnEnable()
    {
        switcher = target as SwitcherToolkit;
    }

    public override void OnInspectorGUI()
    {
        if (switcher.TumblerID == -1) return;

        if (switcher.getParentPanelScript() != null)
        {
            // Выводим объект панели для этого тумблера, если он задан
            EditorGUILayout.LabelField("Parent Panel: ", "");
            EditorGUILayout.ObjectField(switcher.getParentPanelScript().transform, typeof (Transform));

            EditorGUILayout.Space();
        }

        switcher.isButton = EditorGUILayout.Toggle("Is Button", switcher.isButton);

            if (switcher.isButton)
                switcher.isReversible = EditorGUILayout.Toggle("Reversible", switcher.isReversible);

            EditorGUILayout.Space();

            for (int i = 0; i < switcher.StatesList.Count; i++)
            {
                // switcher.Tumbler.GetStatesList().Length
                EditorGUILayout.Foldout(true, "State " + switcher.StatesList[i]);

                switcher.States_Transforms[i] = (Transform)EditorGUILayout.ObjectField("Transform:", switcher.States_Transforms[i], typeof(Transform));

                //Применить текущую позицию как состояние
                if (GUILayout.Button("Apply current rotation as transform"))
                {
                    // Пытаемся найти уже существующий каталог
                    GameObject directory = GameObject.Find(switcher.name + "(States)");

                    // Если его нет - создаем новый
                    if (directory == null)
                    {
                        directory = new GameObject();
                        directory.transform.parent = switcher.transform.parent.transform; //_positions[0].gameObject.transform.parent.gameObject.transform.parent.transform;
                        directory.name = switcher.name + "(States)";
                        //Debug.Log("name: " + switcher.transform.parent.transform.name);
                        directory.transform.position = switcher.transform.position;
                        directory.transform.localRotation = switcher.transform.localRotation;
                    }

                    GameObject newTransform = new GameObject();
                    newTransform.gameObject.name = "State_" + switcher.Tumbler.GetStatesList()[i];
                    newTransform.transform.parent = directory.transform;
                    newTransform.transform.position = switcher.transform.position;
                    switcher.States_Transforms[i] = newTransform.transform;
                    switcher.States_Transforms[i].localRotation = switcher.transform.localRotation;
                    switcher.States_Transforms[i].position = switcher.transform.position;
                }
            }
        

        if (GUI.changed)
        {
            EditorUtility.SetDirty(switcher);
        }
    }

    void OnDestroy()
    {
         /* Вызывается при удалении компонента и смене фокуса на другой объект
         при вызове этого метода, в списке тумблеров панели находим текущий тумблер, и если
         на нем нет SwitcherToolkit - удаляем его GameObject */

        // Если нет родительской панели - ничего не делаем
        if (switcher.getParentPanelScript() == null) return;
        
        // Если на тумблере не висит скрипта SwitcherToolkit - удаляем его с панели
        if (switcher.getParentPanelScript().SwitcherScripts[switcher.TumblerID] == null)
        {
            Debug.Log("Indicator [" + switcher.getParentPanelScript().GetTumblers()[switcher.TumblerID].GetName() + "] was deleted from panel [" + switcher.getParentPanelScript().transform.name + "]");
            switcher.getParentPanelScript().RemoveTumbler(ControlType.Tumbler, switcher.TumblerID);
        }
    }


    void Awake()
    {
        DontDestroyOnLoad(switcher.gameObject);
    }

}
