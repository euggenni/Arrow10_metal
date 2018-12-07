using UnityEngine;
using System.Collections;
using System;

public class IsRotate : MonoBehaviour {

    public float MinDist = 2.0f;
    public float MaxDist = 50.0f;
    private float RotationSpeed = 1500;
    private float MoveSpeed = 10.0f;
    private float ZoomSpeed = 15.3f;

    public static GameObject instanceGameObject;
	// Use this for initialization

    private static IsRotate _currentRotationObject;
    public static IsRotate CurrentRotationObject
    {
        get { return _currentRotationObject; }
        set
        {
            if (_currentRotationObject != null)
                _currentRotationObject.enabled = false;

            _currentRotationObject = value;
            _currentRotationObject.enabled = true;

            print("new rotaition object " + _currentRotationObject);
        }
    }

    public static void checkRotate() {
        if (instanceGameObject != null)
        {
            Debug.LogWarning("get");
            var transformObject = instanceGameObject.GetComponent<IsRotate>();

            if (transformObject)
                GameObject.Destroy(transformObject);

            instanceGameObject = null;
        }
        else
        {
            Debug.LogWarning("add");
            instanceGameObject.AddComponent<IsRotate>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.LogWarning("yes");
        Vector3 dir = transform.position - Camera.main.transform.position;
        float dist = Math.Abs(dir.magnitude);
        float dx = Input.GetAxis("Mouse X");
        float dy = Input.GetAxis("Mouse Y");

        Vector3 camDir = Camera.main.transform.forward;
        Vector3 camLeft = Vector3.Cross(camDir, Vector3.down);
        Vector3 camDown = Vector3.Cross(camDir, camLeft);
        if (Input.GetMouseButton(1))
        {
            //Debug.LogWarning(transform.name);
            transform.RotateAround(Vector3.down, dy * (RotationSpeed / 120) * Time.deltaTime);
        }
	}
}
