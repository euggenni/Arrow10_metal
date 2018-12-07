using UnityEngine;
using System.Collections;

public class AirLieTarget : MonoBehaviour
{
    public float timer = 3;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if (timer < 0)
	    {
	        var plane = GetComponent<WeaponryPlane_LieTarget>();

            if(plane)
                plane.Destroy();

	        Destroy(gameObject);
	    }
	    timer -= Time.deltaTime;

	}
}
