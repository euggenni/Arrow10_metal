using UnityEngine;

public class LieTargetOut : MonoBehaviour {
    public GameObject placeStrela;
    public GameObject lieTarget;
    public int lieTargetCount = 3;
    public GameObject[] lieTargets;
    public float timer = 2f;

    public float forwardForce = 40f;

    // Use this for initialization
    void Start() {
        placeStrela = GameObject.Find("placeholder");
        lieTargets = new GameObject[lieTargetCount];
    }

    // Update is called once per frame
    void Update() {
        if (Vector3.Distance(this.transform.position, placeStrela.transform.position) < 3500) {
            //foreach (GameObject target in lieTargets)
            //{
            //    target = lieTarget;
            //}
            if (timer < 0) {
                for (int i = 0; i < lieTargetCount; i++) {
                    GameObject target = Instantiate(lieTarget.gameObject,
                        this.transform.position + this.transform.forward * forwardForce,
                        this.transform.rotation) as GameObject;
                    if (i < lieTargetCount / 3) {
                        Debug.Log("up");
                        target.rigidbody.AddForce(Vector3.up * 1700);
                    } else if (i < lieTargetCount * 2 / 3) {
                        target.rigidbody.AddForce(Vector3.right * 1000);
                        Debug.Log("right");
                    } else {
                        target.rigidbody.AddForce(Vector3.left * 1000);
                        Debug.Log("left");
                    }
                    timer = 2;
                }
            }
            timer -= Time.deltaTime;
        }
    }
}