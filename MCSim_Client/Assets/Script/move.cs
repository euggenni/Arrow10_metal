using UnityEngine;
using System.Collections;

public class move : MonoBehaviour {
    public float forceUp;
    public float forcegravity;
    public int RotationSpeed = 15;
    public GameObject rocket;
    private bool fire = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W) && forceUp < 50) forceUp += 0.01f;
        
        if (Input.GetKey(KeyCode.S) && forceUp >-10) forceUp -= 0.1f;
        if (!Input.GetKey(KeyCode.S)&&!Input.GetKey(KeyCode.W)) forceUp = forcegravity;
        this.transform.position += this.transform.up * forceUp;
        if (this.transform.position.y > 0.1f) this.transform.position -= Vector3.up * forcegravity;
        if (Input.GetKey(KeyCode.Q))
        {
            Quaternion rotation = Quaternion.AngleAxis(RotationSpeed * Time.deltaTime, Vector3.right);
            transform.rotation *= rotation;
        }
        if (Input.GetKey(KeyCode.E))
        {
            Quaternion rotation = Quaternion.AngleAxis(RotationSpeed * Time.deltaTime*(-1), Vector3.right);
            transform.rotation *= rotation;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Quaternion rotation = Quaternion.AngleAxis(RotationSpeed * Time.deltaTime * (-1), this.transform.up);
            transform.rotation *= rotation;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Quaternion rotation = Quaternion.AngleAxis(RotationSpeed * Time.deltaTime, this.transform.up);
            transform.rotation *= rotation;
        }
        if (Input.GetKey(KeyCode.Space)&&!fire)
        {
            GameObject rock = new GameObject();
            rock = Instantiate(rocket, this.transform.position, this.transform.rotation) as GameObject;
            rock.gameObject.rigidbody.AddForce(this.transform.forward * 1000f);
            fire = true;
	}
    }
}
