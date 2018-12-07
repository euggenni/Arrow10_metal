using UnityEngine;

public class RudeVizirRotateController : MonoBehaviour {
    public GameObject frameTower;
    public GameObject thisVisir;
    public float x = 0;
    public float y = 0;
    public Vector3 thisRotate;
    public Vector3 frameRotate;

    public float x1 = 0;

    // Use this for initialization
    void Start() {
        frameTower = GameObject.Find("Frame");
        thisRotate = thisVisir.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update() {
        frameRotate = frameTower.transform.localEulerAngles;
        thisVisir.transform.localEulerAngles = new Vector3(thisRotate.x, -frameRotate.x + 50f, thisRotate.z);
        //this.transform.rotation.z = z;
    }
}