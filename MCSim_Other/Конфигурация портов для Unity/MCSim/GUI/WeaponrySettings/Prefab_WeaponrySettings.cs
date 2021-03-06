using UnityEngine;
using System.Collections;

public class Prefab_WeaponrySettings : MCSUIObject
{
    Transform positionBut;
	// Use this for initialization
	void Start () {
	
	}

    void OnGUI()
    {
        GUI.TextField(new Rect(25, 100, 100, 30), "Конфигурация контроллера");
        //GUI.TextArea;
    }

    public override void Show()
    {
        positionBut = GameObject.Find("GearButton").GetComponent<Transform>();

        Vector3 pos = UICenter.GUICamera.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, positionBut.transform.position.z));
        iTween.MoveTo(gameObject, new Vector3(pos.x, pos.y, pos.z), 0.5f);

        if (_visible)
        {
            return;
        }

        _visible = true;

        //MCSUICenter.Store.Windows["WeaponrySettings"] = gameObject;
    }

    public override void Close()
    {
        Destroy(gameObject);
    }

    public override void Hide()
    {
        _visible = false;
        //MCSUICenter.Store.Windows["WeaponrySettings"] = null;
    }
}
