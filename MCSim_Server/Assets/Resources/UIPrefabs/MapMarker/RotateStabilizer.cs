using UnityEngine;
using System.Collections;

public class RotateStabilizer : MonoBehaviour {

    //public Quaternion _initialRotation;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        try
        {
            transform.up = Vector3.zero;
        }
        catch { }
	}
}
