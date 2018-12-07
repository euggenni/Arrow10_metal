using UnityEngine;
using System.Collections;
using UnityEditor;

#pragma warning disable 0414, 0108, 0618

[CustomEditor(typeof(IndicatorToolkit))]
public class IndicatorToolkitEditor : Editor {

      // ������ �� ���������
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
            // ������� ������ ������ ��� ����� ����������, ���� �� �����
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
        /* ���������� ��� �������� ���������� � ����� ������ �� ������ ������
        ��� ������ ����� ������, � ������ ��������� ������ ������� ������� ���������, � ����
        �� ��� ��� IndicatorToolkit - ������� ��� GameObject */

        // ���� ��� ������������ ������ - ������ �� ������
        if (indicator.getParentPanelScript() == null) return;

        // ���� �� �������� �� ����� ������� SwitcherToolkit - ������� ��� � ������
        if (indicator.getParentPanelScript().IndicatorScripts[indicator.IndicatorID] == null)
        {
            Debug.Log("Indicator [" + indicator.getParentPanelScript().GetIndicators()[indicator.IndicatorID].GetName() + "] was deleted from panel [" + indicator.getParentPanelScript().transform.name + "]");
            indicator.getParentPanelScript().RemoveTumbler(ControlType.Indicator, indicator.IndicatorID);
        }
    }
}
