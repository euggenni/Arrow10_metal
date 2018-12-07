using UnityEngine;
using System.Collections;

public class ControllerSupportEvent : MonoBehaviour {
    public delegate void UI(string name, string description, string[] statelist, string position);

    public event UI TumblerEvent;

    public void OnTumblerCreateEvent(string name, string description, string[] statelist, string position)
    {
        TumblerEvent(name, description, statelist, position);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
