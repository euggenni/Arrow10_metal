using UnityEngine;
using System.Collections;

public class AutoSize : MonoBehaviour {

    public float Multiplier = 1f;
	// Use this for initialization
	void Start () {
        transform.localScale *= MCSUICenter.sizeK * Multiplier;
	}
	
}
