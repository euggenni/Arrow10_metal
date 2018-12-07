using UnityEngine;

public class rockettt : MonoBehaviour {
    // Use this for initialization
    void Start() {
        this.gameObject.rigidbody.AddForce(this.transform.forward * 10000);
    }

    // Update is called once per frame
    void Update() {
    }
}