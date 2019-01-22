using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/// <summary>
/// Библиотека с ссылками на контейнеры хранения гуи, префабы и т.п.
/// </summary>
public class GUILibrary : MonoBehaviour {

    // GameObject's
    public GameObject Container_GUI;
    public GameObject Container_Marker;
    public GameObject Container_PopupList;
    public GameObject Container_Waypoints;
    public GameObject Container_GUI_Main;

    // Prefab's
    public GameObject Prefab_PopupList;


    /// <summary>
    /// Активное выпадающее меню
    /// </summary>
    public GameObject Active_PopupList;


    // Флаги
    public GameObject Flag_WeaponryPlane;
    public GameObject Flag_Sphere;

    // Окна (Префабы)
    public GameObject Prefab_Window_WeaponryMenu;
    public GameObject Prefab_Window_TaskMenu;
    public GameObject Prefab_Window_Console;
    public GameObject Prefab_Window_StatusInfo;
    public GameObject Prefab_Window_Students;

    public Dictionary<string,GameObject> Windows = new Dictionary<string, GameObject>();
}
