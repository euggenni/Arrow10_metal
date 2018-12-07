using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

/// <summary>
/// ����������� ����� ��� ���������� ����������
/// </summary>
public class MCSGlobalEnvironment : MonoBehaviour
{
    /// <summary>
    /// ������ ��� ���������� ����������
    /// </summary>
    public static UniSkyAPI UniSky;

    /// <summary>
    /// ������������� ���������
    /// </summary>
    public static void InitializeEnvironment()
    {
        try
        {
            UniSky = GameObject.Find("UniSkyAPI").GetComponent("UniSkyAPI") as UniSkyAPI;

            // Initiate and create default UniSky 
            UniSky.InstantiateUniSky();


            // Set some initial states 
            UniSky.SetTime(12.0f);
            UniSky.SetAmbientLighting(new Color(0.1f, 0.1f, 0.1f, 0.1f));
            UniSky.SetStormCenter(new Vector3(0, 0, 0));
            UniSky.SetSunShadows(LightShadows.Hard);
        }
        catch {
            Debug.Log("������ ��� ������������� ���������.");
        }
    }

    public static void SetEnvironmentTime(float TIME)
    {
        if(Network.isServer)
            MCSGlobalSimulation.CommandCenter.Execute(new MCSCommand(MCSCommandType.Simulation, "SynhronizeEnvironmentTime", false, TIME));
        else
            Debug.Log("�������� ��������� ���������������� �����.");
    }
}
