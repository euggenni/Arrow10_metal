using UnityEditor;
using UnityEngine;
using System.Collections;

#pragma warning disable 0414, 0168, 0618

[CustomEditor(typeof(AirCraftToolkit))]
public class AirCraftToolkitEditor : Editor
{
    private AirCraftToolkit _airCraft;

    void OnEnable() {
        _airCraft = target as AirCraftToolkit;
    }

    private int _turbineCount = 1;
    public override void OnInspectorGUI()
    {
        _airCraft.Center_Horiznontal = (GameObject)EditorGUILayout.ObjectField("��� �������:", _airCraft.Center_Horiznontal, typeof(GameObject));
        _airCraft.Center_Longitudinal = (GameObject)EditorGUILayout.ObjectField("��� ��������:", _airCraft.Center_Longitudinal, typeof(GameObject));
        EditorGUILayout.Space();


        _airCraft.MaxThrust = EditorGUILayout.FloatField("����. ����:", _airCraft.MaxThrust);
        for(int i = 0; i < _airCraft.TurbineCount; i++)
        {
            _airCraft.Turbine[i] = (Rigidbody)EditorGUILayout.ObjectField("������� " + (i+1), _airCraft.Turbine[i], typeof(Rigidbody));
        }

        EditorGUILayout.BeginHorizontal();
        _turbineCount = EditorGUILayout.IntField("���������� ������:", _turbineCount);

        if (GUILayout.Button("���������"))
        {
            _airCraft.TurbineCount = _turbineCount;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        _airCraft.MaxSpeed = EditorGUILayout.FloatField("����. �������� (�/�):", _airCraft.MaxSpeed);
        EditorGUILayout.Space();

        EditorGUILayout.Foldout(true, " -=������=-");

        EditorGUILayout.BeginHorizontal();
        _airCraft.WSA = EditorGUILayout.FloatField("�������:", _airCraft.WSA);
        _airCraft.WingSpan = EditorGUILayout.FloatField("������:", _airCraft.WingSpan);
        EditorGUILayout.EndHorizontal();
        _airCraft.SAH = EditorGUILayout.FloatField("���:", _airCraft.SAH);

        EditorGUILayout.BeginHorizontal();
        _airCraft.MinPitchAngle = EditorGUILayout.FloatField("���. ���� ��������:", _airCraft.MinPitchAngle);
        _airCraft.MaxPitchAngle = EditorGUILayout.FloatField("����. ���� ��������:", _airCraft.MaxPitchAngle);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.Foldout(true, " -=���������=-");
        EditorGUILayout.BeginHorizontal();
        _airCraft.ThrustMultiplier = EditorGUILayout.FloatField("���������� ����:", _airCraft.ThrustMultiplier);
        _airCraft.LiftForceMultiplier = EditorGUILayout.FloatField("��������� ����:", _airCraft.LiftForceMultiplier);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        _airCraft.PitchMultiplier = EditorGUILayout.FloatField("������:", _airCraft.PitchMultiplier);
        _airCraft.RollMultiplier = EditorGUILayout.FloatField("��������:", _airCraft.RollMultiplier);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        _airCraft.YawMultiplier = EditorGUILayout.FloatField("��������:", _airCraft.YawMultiplier);
        _airCraft.InertiaMultiplier = EditorGUILayout.FloatField("�����������:", _airCraft.InertiaMultiplier);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Foldout(true, " -=����������� ������� �������� �� ��������=-");
        EditorGUILayout.HelpBox("������������ ��� - 10���������. �������������� ��� - ��/�", MessageType.Info);
        _airCraft.SpeedToAngular = EditorGUILayout.CurveField("", _airCraft.SpeedToAngular, GUILayout.MinHeight(150));
       // Debug.Log(_airCraft.SpeedToAngular.Evaluate(360)/10);
    }
}
