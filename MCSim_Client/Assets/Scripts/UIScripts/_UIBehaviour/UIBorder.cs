using UnityEngine;
using System.Collections;

public class UIBorder : MonoBehaviour
{
	public string BorderName;

	public bool DefineOnce = true;
	
	// Update is called once per frame
	void Update ()
	{
		if (DefineOnce) enabled = false;

		UICenter.SetBorder(BorderName, transform.position);
	}
}
