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
        _airCraft.Center_Horiznontal = (GameObject)EditorGUILayout.ObjectField("Ось тангажа:", _airCraft.Center_Horiznontal, typeof(GameObject));
        _airCraft.Center_Longitudinal = (GameObject)EditorGUILayout.ObjectField("Ось вращения:", _airCraft.Center_Longitudinal, typeof(GameObject));
        EditorGUILayout.Space();


        _airCraft.MaxThrust = EditorGUILayout.FloatField("Макс. тяга:", _airCraft.MaxThrust);
        for(int i = 0; i < _airCraft.TurbineCount; i++)
        {
            _airCraft.Turbine[i] = (Rigidbody)EditorGUILayout.ObjectField("Турбина " + (i+1), _airCraft.Turbine[i], typeof(Rigidbody));
        }

        EditorGUILayout.BeginHorizontal();
        _turbineCount = EditorGUILayout.IntField("Количество турбин:", _turbineCount);

        if (GUILayout.Button("Применить"))
        {
            _airCraft.TurbineCount = _turbineCount;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        _airCraft.MaxSpeed = EditorGUILayout.FloatField("Макс. скорость (м/с):", _airCraft.MaxSpeed);
        EditorGUILayout.Space();

        EditorGUILayout.Foldout(true, " -=Крылья=-");

        EditorGUILayout.BeginHorizontal();
        _airCraft.WSA = EditorGUILayout.FloatField("Площадь:", _airCraft.WSA);
        _airCraft.WingSpan = EditorGUILayout.FloatField("Размах:", _airCraft.WingSpan);
        EditorGUILayout.EndHorizontal();
        _airCraft.SAH = EditorGUILayout.FloatField("САХ:", _airCraft.SAH);

        EditorGUILayout.BeginHorizontal();
        _airCraft.MinPitchAngle = EditorGUILayout.FloatField("Мин. угол закрылок:", _airCraft.MinPitchAngle);
        _airCraft.MaxPitchAngle = EditorGUILayout.FloatField("Макс. угол закрылок:", _airCraft.MaxPitchAngle);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.Foldout(true, " -=Множители=-");
        EditorGUILayout.BeginHorizontal();
        _airCraft.ThrustMultiplier = EditorGUILayout.FloatField("Реактивная сила:", _airCraft.ThrustMultiplier);
        _airCraft.LiftForceMultiplier = EditorGUILayout.FloatField("Подъемная сила:", _airCraft.LiftForceMultiplier);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        _airCraft.PitchMultiplier = EditorGUILayout.FloatField("Тангаж:", _airCraft.PitchMultiplier);
        _airCraft.RollMultiplier = EditorGUILayout.FloatField("Вращение:", _airCraft.RollMultiplier);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        _airCraft.YawMultiplier = EditorGUILayout.FloatField("Рыскание:", _airCraft.YawMultiplier);
        _airCraft.InertiaMultiplier = EditorGUILayout.FloatField("Интертность:", _airCraft.InertiaMultiplier);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Foldout(true, " -=Зависимость Угловой скорости от Скорости=-");
        EditorGUILayout.HelpBox("Вертикальная ось - 10хГрадусов. Горизонтальная ось - Км/Ч", MessageType.Info);
        _airCraft.SpeedToAngular = EditorGUILayout.CurveField("", _airCraft.SpeedToAngular, GUILayout.MinHeight(150));
       // Debug.Log(_airCraft.SpeedToAngular.Evaluate(360)/10);
    }
}
