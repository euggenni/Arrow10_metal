using UnityEngine;

public class VizirCameraRender : MonoBehaviour {
    public RenderTexture texture;
    public GameObject cameraGO;


    //private Zoom zoom =  Zoom.Close;

    // Use this for initialization
    void Start() {
        //GameObject containersFrame = OperatorNode.Host.Containers;
        cameraGO = GameObject.Find("_Camera");
        cameraGO.camera.targetTexture = texture;
    }

    // Update is called once per frame
    void Update() {
    }
}