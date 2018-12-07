using UnityEngine;
using System.Collections;

public class createMap : MonoBehaviour {

	//public GameObject map;

    public GameObject[] maps;
    public CameraMovementScript cms;
    private GameObject createdMap;


	void OnSelectionChange (string val)
	{
        Destroy(createdMap);
        //if (map == null) map = gameObject.transform.Find("Map");
        switch (val)
        {
            case "Map 1": placeMap(0); break;
            case "Map 2": placeMap(1); break;
            case "Map 3": placeMap(2); break;
        }
        while (createdMap.renderer.bounds.size.x+5f < this.renderer.bounds.size.x)
        {
            transform.localScale -= new Vector3(1.0f, 0.0f, 0.0f);
        }
        while (createdMap.renderer.bounds.size.z+5f < this.renderer.bounds.size.z)
        {
            transform.localScale -= new Vector3(0.0f, 0.0f, 1.0f);
        }
        while (createdMap.renderer.bounds.size.x + 5f > this.renderer.bounds.size.x)
        {
            transform.localScale += new Vector3(1.0f, 0.0f, 0.0f);
        }
        while (createdMap.renderer.bounds.size.z + 5f > this.renderer.bounds.size.z)
        {
            transform.localScale += new Vector3(0.0f, 0.0f, 1.0f);
        }

        cms.Reset();

	}

    private void placeMap(int i)
    {
        createdMap = (GameObject)GameObject.Instantiate(maps[i], (this.transform.position + Vector3.up.normalized), Quaternion.identity);
    }
}

