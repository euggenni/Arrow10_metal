using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {
    public int RotationSpeed = 150;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion rotation = Quaternion.AngleAxis(RotationSpeed * Time.deltaTime, Vector3.up);
        transform.rotation *= rotation;
	}
}
