using UnityEngine;
using System.Collections;
using MilitaryCombatSimulator;

public class WeaponryMarker : MonoBehaviour {

    Weaponry _weaponry;

	// Use this for initialization
	void Awake () {
        var weaponry = gameObject.GetComponentInParents<Weaponry>(true);

        GameObject marker = null;
//        if (weaponry is WeaponryTank)
//        {
//            marker = (GameObject)GameObject.Instantiate(Resources.Load("UIPrefabs/MapMarker/TankMarker"));          
//        }
//
//        if (weaponry is WeaponryPlane)
//        {
//            marker = (GameObject)GameObject.Instantiate(Resources.Load("UIPrefabs/MapMarker/PlaneMarker"));        
//        }

        if (marker)
        {
            marker.transform.parent = transform;
            marker.transform.localEulerAngles = Vector3.zero;
            marker.transform.localPosition = Vector3.zero;
        }

        Destroy(this);
	}
}
