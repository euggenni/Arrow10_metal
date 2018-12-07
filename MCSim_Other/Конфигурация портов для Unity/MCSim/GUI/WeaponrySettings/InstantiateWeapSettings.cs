using UnityEngine;
using System.Collections;

public class InstantiateWeapSettings : MonoBehaviour {

    public GameObject SettingsPanelObject;
    public GameObject Sender;

	// Use this for initialization
	void Start () {
        Instantiate(SettingsPanelObject);

        //Vector3 pos = MCSUICenter.GUICamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f - Screen.height / 8f, 0));
        //Vector3 newv = new Vector3(0.5f, -80, 208);
        //SettingsPanelObject.transform.position = pos;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
