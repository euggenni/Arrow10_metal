using UnityEngine;
using System.Collections;
using UnityEditor;

#pragma warning disable 0414, 0108, 0618

[CustomEditor(typeof(IndicatorToolkit))]
public class IndicatorToolkitEditor : Editor {

      // ссылка на компонент
    IndicatorToolkit indicator;

    void OnEnable()
    {
        indicator = target as IndicatorToolkit;
    }

    public override void OnInspectorGUI()
    {
        if (indicator.IndicatorID == -1) return;

        if (indicator.getParentPanelScript() != null)
        {
            // Выводим объект панели для этого индикатора, если он задан
            EditorGUILayout.LabelField("Parent Panel: ", "");
            EditorGUILayout.ObjectField(indicator.getParentPanelScript().transform, typeof(Transform));

            EditorGUILayout.Space();

            indicator.isManual = EditorGUILayout.Toggle("Manual", indicator.isManual);

            EditorGUILayout.Space();

            for (int i = 0; i < indicator.getParentPanelScript().GetIndicators()[indicator.IndicatorID].GetStatesList().Length; i++)
            {
                // switcher.Tumbler.GetStatesList().Length
                EditorGUILayout.Foldout(true, "State " + indicator.getParentPanelScript().GetIndicators()[indicator.IndicatorID].GetStatesList()[i]);

                EditorGUILayout.InspectorTitlebar(true, indicator.transform.renderer.sharedMaterial);


                //EditorGUILayout.LabelField("Material:", "");

                EditorGUILayout.BeginHorizontal();


                EditorGUILayout.LabelField("Material color:", "");
                indicator.SetColor(EditorGUILayout.ColorField(indicator.GetColors()[i]), i);

                EditorGUILayout.EndHorizontal();


                indicator.setGlowing(EditorGUILayout.Toggle("Glowing: ", indicator.getGlowing(i)), i);
            }
        }
        else
        {
            EditorGUILayout.TextArea("This object is not connected with parent,\nwhich have ControlPanelToolkit script.\nCreate new object with ControlPanelToolkit\nand add object with SwitcherToolkit\nscript to him as tumbler.", GUILayout.ExpandHeight(true));
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(indicator);
        }
    }

    void OnDestroy()
    {
        /* Вызывается при удалении компонента и смене фокуса на другой объект
        при вызове этого метода, в списке тумблеров панели находим текущий индикатор, и если
        на нем нет IndicatorToolkit - удаляем его GameObject */

        // Если нет родительской панели - ничего не делаем
        if (indicator.getParentPanelScript() == null) return;

        // Если на тумблере не висит скрипта SwitcherToolkit - удаляем его с панели
        if (indicator.getParentPanelScript().IndicatorScripts[indicator.IndicatorID] == null)
        {
            Debug.Log("Indicator [" + indicator.getParentPanelScript().GetIndicators()[indicator.IndicatorID].GetName() + "] was deleted from panel [" + indicator.getParentPanelScript().transform.name + "]");
            indicator.getParentPanelScript().RemoveTumbler(ControlType.Indicator, indicator.IndicatorID);
        }
    }
}
