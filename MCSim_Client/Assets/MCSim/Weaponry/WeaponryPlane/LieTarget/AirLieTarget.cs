using UnityEngine;

public class AirLieTarget : MonoBehaviour {
    public float timer = 3;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (timer < 0) {
            Destroy(this.gameObject);
        }
        timer -= Time.deltaTime;
    }
}