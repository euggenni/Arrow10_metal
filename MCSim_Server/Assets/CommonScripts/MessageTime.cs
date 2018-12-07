using UnityEngine;
using System.Collections;
using MilitaryCombatSimulator;

public class MessageTime : MonoBehaviour
{
    private UILabel label;

    void Start()
    {
        label = GetComponent<UILabel>();

        if (!label)
            Destroy(this);

        label.text = MCSGlobalEnvironment.UniSky.TIME.ToString();
    }


    void Send()
    {
        float time;

        if (float.TryParse(label.text, out time))
        {
            MCSGlobalEnvironment.UniSky.TIME = time;
            MCSGlobalEnvironment.SetEnvironmentTime(time);
            MCSGlobalSimulation.CommandCenter.Execute(new MCSCommand(MCSCommandType.Simulation, "WeatherPosted", true, time));
        }
        else
        {
            Debug.Log("Не удалось определить введенное время");
        }
    }
}
