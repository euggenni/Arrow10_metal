using System;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public class WeaponryList_Item : MonoBehaviour
{
    public UISprite WeaponryIcon;

    public UILabel WeaponryName;
    public UILabel WeaponryType;


    public void SetWeaponry(Weaponry weaponry)
    {
        UIAtlas atlas = WeaponryIcon.atlas;

        if(atlas)
        {
            if (weaponry.Resources.ContainsKey("Icon_WeaponryList"))
            {
                WeaponryIcon.spriteName = weaponry.Resources["Icon_WeaponryList"].ToString();
                WeaponryIcon.color = new Color(200,200,200);
                WeaponryIcon.UpdateUVs();
            }

            //WeaponryName.text = "Name: " + weaponry.Name;
            WeaponryName.text = weaponry.Name;
            WeaponryType.text = "Type: " + weaponry.GetType().ToString().Split('_')[0].Replace("Weaponry","");
        }

        WeaponryIcon.gameObject.GetComponent<MCSMarker_Weaponry>().Data = weaponry.GetType().FullName;
    }


	// Use this for initialization
	void Start ()
	{
        NGUIPanel panel = GetComponent<NGUIPanel>();
        GameObject.Destroy(panel);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
