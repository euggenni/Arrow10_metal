using UnityEngine;

public class GUI_MesagePanel : MonoBehaviour {
    // Use this for initialization
    public void exitWindow() {
        Destroy(this.gameObject);
    }

    public void OpenAndExit() {
        try {
            var panelStart2 = UICenter.Panels.Instantiate("MessageWindowPart2");
            panelStart2.Show();
        } catch {
        }
        Destroy(this.gameObject);
    }
}