using MilitaryCombatSimulator;
using UnityEngine;

public class OperatorRudeVizirContriller : MonoBehaviour {
    public Strela10_Operator_Node OperatorNode;
    private Strela10_Operator_CrosshairController _crosshairController;

    /// <summary>
    /// Îêíî ïðèöåëèâàíèÿ
    /// </summary>
    public GameObject OperatorCrosshair;

    private bool VizirActivated;

    private GameObject gameObjectCrosshair;
    private Camera gameObjectVizir;
    private GameObject cameraGO;
    private bool bortFlag;
    private WeaponryRocket currentRocket;
    private Vector3 lastTrackerPos;

    public float a = 5f, b = 5f;

    enum Zoom {
        Close = 25,
        Far = 5
    }

    private Zoom zoom = Zoom.Close;
    private float speed = 10f;
    private bool keyState = false;


    void activeOfVizir() {
        if (VizirActivated) return;

        GameObject containersFrame = OperatorNode.Host.Containers;
        OperatorNode.Camera.enabled = false;

        // Ñîçäàåì ÃÓÈ ñ ïðèöåëîì
        gameObjectCrosshair = Instantiate(OperatorCrosshair) as GameObject;
        gameObjectCrosshair.transform.parent = UICenter.Panels.Anchor.transform;
        gameObjectCrosshair.transform.localScale = Vector3.one;

        // Ïîëó÷àåì ñ íåãî êîíòðîëëåð ïðèöåëà äëÿ óïðàâëåíèÿ ñòðåëêàìè, êîëüöîì çàõâàòà è ò.ï.
        // Ñîçäàåì êàìåðó âèçèðà
        cameraGO = new GameObject();
        cameraGO.name = "VizorCamera2";
        gameObjectVizir = cameraGO.AddComponent<Camera>();
        gameObjectVizir.transform.parent = OperatorNode.Host.Tower.transform;
        gameObjectVizir.nearClipPlane = 0.1f;
        gameObjectVizir.farClipPlane = 10000f;
        gameObjectVizir.transform.localPosition = new Vector3(0.3f, -0.4f, 0.15f);
        gameObjectVizir.transform.localEulerAngles = Vector3.zero;
        gameObjectVizir.fieldOfView = (int) Zoom.Close;

        // Àêòèâèðóåì ñêðèïò ñëåæåíèÿ âèçèðà çà òî÷êîé ôîêóñà
        Strela10_Operator_VizirCameraController cameraController =
            gameObjectVizir.gameObject.AddComponent<Strela10_Operator_VizirCameraController>();
        cameraController.Containers = OperatorNode.Host.Containers;

        VizirActivated = true;
    }


    void OnMouseDown() {
        activeOfVizir();
    }

    void Start() {
        switchIndicatorOff();
    }


    void destroyOfVizir() {
        OperatorNode.Camera.enabled = true;
        Destroy(cameraGO);
        Destroy(gameObjectCrosshair);
        VizirActivated = false;
    }


    void Update() {
        //Debug.Log(OperatorNode.Host.TowerHandler.TowerAngle);

        if (Input.GetKeyDown(KeyCode.Escape) && VizirActivated) {
            destroyOfVizir();
        }

        if (Input.GetKeyUp(KeyCode.Tab)) {
            if (!VizirActivated)
                activeOfVizir();
            else
                destroyOfVizir();
        }
    }

    void switchIndicatorOn() {
        if (_crosshairController != null) {
            _crosshairController.Indicator.GetComponent<UISprite>().color = new Color(255, 0, 0);
        }
    }

    void switchIndicatorOff() {
        if (_crosshairController != null) {
            _crosshairController.Indicator.GetComponent<UISprite>().color = new Color(89, 89, 89);
        }
    }
}