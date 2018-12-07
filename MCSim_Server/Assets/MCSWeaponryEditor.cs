using System;
using System.Collections.Generic;
using MilitaryCombatSimulator;
//using UnityEditor;
using UnityEngine;
using System.Collections;

public class MCSWeaponryEditor : MonoBehaviour
{
    public GameObject[] WeaporyList;
    public UIPopupList TargetList;
    public UIPopupList traceList;
    public UIPopupList NoiseList;
    public UIPopupList SkyList;

    public UICheckbox WarCheckbox;
    public UIInput InputChastota;
    public UICheckbox BelongingCheckbox;

    private Dictionary<String, GameObject> mapWeaponry = new Dictionary<string, GameObject>();
    public GameObject AtackPolPick;
    public GameObject AtackPickPramNavodkoi;
    public GameObject AtackPramo;
    public GameObject AtackKabrFix;
    public GameObject AtackGorPol;
    public GameObject AtackAaPodsk;
    public GameObject AtackAaGorPol;
    public GameObject AtackMissile;

    public GameObject CloudHight;
    public GameObject CloudLight;

    public GameObject WarElement;

    public UIInput InputDistance;
    public UIInput InputAsimut;
    public UIInput InputHeight;

    public GameObject PlaceStrela;

    private GameObject newWeaponry;

    private bool _searchAi;

    public String[] Tracelist =
	    {
	        "Атака с горизонтального полёта (общий)",
	        "Атака с пологого пикирования (Фронтовая Ав.)",
	        "Атака с пикирования прямой наводкой (Фронтовая Ав.)",
	        "Атака с кабрирования с фиксированным углом 110 (Ф.А.)",
	        "Атака с горизонтального полёта (Фронтовая Ав.)",
	        "Атака с подскока (с зависанием) (Армейская. Ав.)",
            "Атака с горизонтального полёта (Армейская Ав.)",
            "Атака крылатой ракеты"
	    };
    public String[] Noiselist =
	    {
	        "нет",
	        "ложные тепловые цели",
	        "аэрозольные",
	    };

    public String[] Skylist =
    {
        "ясно",
        "слабая облачность",
        "сильная облачность"
    };

	// Use this for initialization
	void Start () {
	    foreach (GameObject weaponry in WeaporyList)
	    {
	        //targetList.items.Add(weaponry.GetComponent<WeaponryPlane>().Name);
            Debug.LogWarning(weaponry.name);
            mapWeaponry.Add(weaponry.GetComponent<WeaponryPlane>().Name, weaponry);
	    }
        TargetList.items.AddRange(mapWeaponry.Keys);
        traceList.items.AddRange(Tracelist);
        NoiseList.items.AddRange(Noiselist);
        SkyList.items.AddRange(Skylist);
        PlaceStrela = GameObject.Find("Placeholder");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlaceTarget()
    {
        if (TargetList.selection.Equals("Выберите цель") || NoiseList.selection.Equals("Выберите тип помех") ||
            traceList.selection.Equals("Выберите траекторию движения"))
        {
            Debug.LogWarning("Не выбрано");
        }
        else
        {
            int height = Convert.ToInt32(InputHeight.text);
            double angle = Convert.ToDouble(InputAsimut.text);
            double distance = Convert.ToDouble(InputDistance.text);
            GameObject go = new GameObject();
            mapWeaponry.TryGetValue(TargetList.selection, out go);
            GameObject wp = new GameObject();
            newWeaponry = GameObject.Instantiate(go) as GameObject;
            switch (traceList.selection)
            {
                case "Атака с горизонтального полёта (общий)":
                {
                    wp = GameObject.Instantiate(AtackPramo) as GameObject;
                }
                    break;
                case "Атака с пологого пикирования (Фронтовая Ав.)":
                {
                    wp = GameObject.Instantiate(AtackPolPick) as GameObject;
                }
                    break;

                case "Атака с пикирования прямой наводкой (Фронтовая Ав.)":
                {
                    wp = GameObject.Instantiate(AtackPickPramNavodkoi) as GameObject;
                }
                    break;
                case "Атака с кабрирования с фиксированным углом 110 (Ф.А.)":
                {
                    wp = GameObject.Instantiate(AtackKabrFix) as GameObject;
                }
                    break;
                case "Атака с горизонтального полёта (Фронтовая Ав.)":
                    {
                        wp = GameObject.Instantiate(AtackGorPol) as GameObject;
                    }
                    break;
                case "Атака с горизонтального полёта (Армейская Ав.)":
                    {
                        wp = GameObject.Instantiate(AtackAaGorPol) as GameObject;
                    }
                    break;
                case "Атака с подскока (с зависанием) (Армейская. Ав.)":
                    {
                        wp = GameObject.Instantiate(AtackAaPodsk) as GameObject;
                        newWeaponry.AddComponent<KinematicDownScript>();
                    } break;
                case "Атака крылатой ракеты":
                    {
                        wp = GameObject.Instantiate(AtackMissile) as GameObject;
                    } break;


            }
            wp.transform.RotateAround(PlaceStrela.transform.position, PlaceStrela.transform.up, (float)angle);
                
                foreach (MCSWaypoint waypoint in wp.GetComponentsInChildren<MCSWaypoint>())
                {
                    newWeaponry.GetComponent<AirCraftAI>().Waypoints.Add(waypoint);
                }
                newWeaponry.transform.position = PlaceStrela.transform.position +
                                                    new Vector3((float)(Math.Sin(normalAngle(angle)) * distance), height,
                                                        (float)(Math.Cos(normalAngle(angle)) * distance));
                newWeaponry.AddComponent<MCSDummy_WeaponryForTask>();
            
            switch (NoiseList.selection)
            {
                case "ложные тепловые цели":
                {
                    newWeaponry.AddComponent<LieTargetOut>().lieTarget = Resources.Load("LieTarget") as GameObject;
                    try
                    {
                        float timer = (float) Convert.ToDouble(InputChastota.text);
                        newWeaponry.GetComponent<LieTargetOut>().timer = timer;
                    }   
                    catch(Exception)
                    {
                    }
                }break;
            }

            switch (SkyList.selection)
            {
                case "сильная облачность":
                {
                    if (GameObject.Find("Target_Cloud_Hight(Clone)") == null)
                    (GameObject.Instantiate(CloudHight) as GameObject).AddComponent<MCSDummy_WeaponryForTask>();
                } break;
                case "слабая облачность":
                    {
                        if (GameObject.Find("Target_Cloud_Light(Clone)") == null)
                        (GameObject.Instantiate(CloudLight) as GameObject).AddComponent<MCSDummy_WeaponryForTask>();
                    } break;

            }

            if(WarCheckbox.isChecked)
            {
                if (GameObject.Find("WarElement(Clone)") == null)
                 GameObject.Instantiate(WarElement);
            }

            if(BelongingCheckbox.isChecked)
            {
                Debug.Log("NRZ -> MCSWeaponryEditor - RUSSIA (0)");
               Invoke("call", 3f);
            }
        }
    }

    public double normalAngle(double angle)
    {
        return Math.PI * angle / 180;
    }

    public void call()
    {
        Debug.Log("NRZ -> MCSWeaponryEditor - RUSSIA (1). Id = " + newWeaponry.networkView.viewID);
        Debug.LogError(newWeaponry.networkView.viewID);
        GameObject.Find("Frame").GetComponent<Strela10_Arms>().BelongingTargetCall(newWeaponry.networkView.viewID);
    }
}
