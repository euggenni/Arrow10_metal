using UnityEngine;
using System.Collections;

public class KinematicDownScript : MonoBehaviour {
    public GameObject placeStrela;
    public float Timer = 15f;
    private bool Stoping = false;
    //public bool KinematicStatus = false;
    private Rigidbody rb;
	// Use this for initialization
	void Start () {
	placeStrela = GameObject.Find("Placeholder");
	    rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Vector3.Distance(this.transform.position, placeStrela.transform.position) < 2005 && !Stoping)
	    {
	        rb.isKinematic = true;
	        Stoping = true;
	        StartCoroutine(movingDelay(Timer));
	    }
	}

    IEnumerator movingDelay(float delay)
    {
        Debug.LogWarning("До задержки");
        yield return new WaitForSeconds(delay);
        Debug.LogWarning("После задержки");
        rb.isKinematic = false;
        Debug.LogWarning(rb.isKinematic);
    }
}
