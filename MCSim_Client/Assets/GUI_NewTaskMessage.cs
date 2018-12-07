using System;
using MilitaryCombatSimulator;
using UnityEngine;

public class GUI_NewTaskMessage : MonoBehaviour {
    public UILabel TaskText;
    public float Timer = 5f;

    // Use this for initialization
    void Start() {
        Destroy(this.gameObject, Timer);
    }

    // Update is called once per frame
    void Update() {
    }

    public void onText(GameObject target) {
        Debug.LogWarning("INFO");
        TaskText.text = "Запущена новая задача\nЦель: " + target.GetComponent<WeaponryPlane>().Name + "\nДальность: " +
                        Convert.ToInt32(Vector3.Distance(target.transform.position,
                            GameObject.Find("placeholder").transform.position)) + " м";
    }
}