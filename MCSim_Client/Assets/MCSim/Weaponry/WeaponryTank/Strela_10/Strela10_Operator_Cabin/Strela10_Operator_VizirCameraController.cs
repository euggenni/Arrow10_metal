using UnityEngine;

/// <summary>
/// Класс управляет камерой, участвует в ее наведение на точку фокуса
/// </summary>
public class Strela10_Operator_VizirCameraController : MonoBehaviour {
    public GameObject Containers;

    public float speed = 35f;

    private VectorLine line;

    // Use this for initialization
    void Start() {
        camera.clearFlags = CameraClearFlags.Color;
        if (!camera || !Containers) Destroy(this);
    }

    // Update is called once per frame
    void LateUpdate() {
        if (Containers) {
            camera.transform.localRotation = Quaternion.Lerp(camera.transform.localRotation,
                Containers.transform.localRotation, Time.deltaTime * speed);
        }
    }
}