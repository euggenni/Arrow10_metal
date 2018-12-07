using System;
using UnityEngine;

public class Boom : MonoBehaviour {
    public GameObject BoomFx;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (Convert.ToInt32(Vector3.Distance(this.transform.position,
                GameObject.Find("buildings_City").transform.position)) < 80) {
            GameObject boom =
                Instantiate(BoomFx, this.transform.position, this.transform.rotation) as GameObject;
            GetComponent<WeaponryPlane_Missile>().Destroy();
            Destroy(boom, 4f);
        }
    }
}