using UnityEngine;

public class ShowInformation : MonoBehaviour {
    // Use this for initialization
    void Start() {
        try {
            var panel = UICenter.Panels.Instantiate("NewTaskMessage") as GUIPanel_Mode;
            panel.Show();
        } catch {
        }
        Invoke("setTextLabel", 1.5f);
    }

    // Update is called once per frame
    void Update() {
    }

    void setTextLabel() {
        GameObject.Find("NewTaskMessage(Clone)").GetComponent<GUI_NewTaskMessage>().onText(this.gameObject);
        Debug.LogWarning("SHOINFO");
    }
}