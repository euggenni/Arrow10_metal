using UnityEngine;
using System.Collections;

public class CameraActivator : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //camera.cullingMask = 0x1F;      // 0b11111
	}
	
	// Update is called once per frame
	void Update () {
        camera.cullingMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("TransparentFX")) | (1 << LayerMask.NameToLayer("Ignore Raycast")) | (1 << LayerMask.NameToLayer("Water")) | (1 << LayerMask.NameToLayer("Cabin"));
	}
}
