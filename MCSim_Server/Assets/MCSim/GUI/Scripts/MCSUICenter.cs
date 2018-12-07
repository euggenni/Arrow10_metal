using System;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;


public enum MCSFlagType
{
    Ground, Air
}

public class MCSUICenter: MonoBehaviour
{
    private static MCSMarker _marker;

    /// <summary>
    /// Àêòèâíûé ìàðêåð.
    /// </summary>
    public static MCSMarker Marker
    {
        get { return _marker; }
        set
        {
            _marker = value;
        }
    }

    /// <summary>
    /// Ñëîè, íà êîòîðûå îïèðàåòñÿ êóðñîð ïðè ïåðåìåùåíèè
    /// </summary>
    public static LayerMask Satellite_HangMask;

    public GameObject WindowCommander;

    private static Camera _guiCamera;

    /// <summary>
    /// Êàìåðà, îòðèñîâûâàþùàÿ GUI.
    /// </summary>
    public static Camera GUICamera
    {
        get
        {
            if (!_guiCamera)
                _guiCamera = GameObject.Find("GUI Camera").GetComponent<Camera>();

            return _guiCamera;
        }
    }

    private static Camera _satelliteCamera;

    /// <summary>
    /// Ñïóòíèêîâàÿ êàìåðà.
    /// </summary>
    public static Camera SatelliteCamera
    {
        get
        {
            if (!_satelliteCamera)
                _satelliteCamera = GameObject.Find("Satellite Camera").GetComponent<Camera>();

            return _satelliteCamera;
        }
    }

    private static Camera _mainCamera;

    /// <summary>
    /// Ãëàâíàÿ êàìåðà.
    /// </summary>
    public static Camera MainCamera
    {
        get
        {
            if (!_mainCamera)
                _mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

            return _mainCamera;
        }
    }

    /// <summary>
    /// Êîýôôèöèåíò ðàçðåøåíèÿ
    /// </summary>
    public static float sizeK = 1f;

    private static GameObject _lightHouse;

    /// <summary>
    /// Òî÷êà-ìàÿê
    /// </summary>
    public static GameObject LightHouse
    {
        get { return _lightHouse; }
        set { _lightHouse = value; }
    }

    /// <summary>
    /// Óðîâåíü GUI Ìàÿêà
    /// </summary>
    public static GameObject LightHouseGUI { get; private set; }

    /// <summary>
    /// Îáúåêò ñ ññûëêàìè íà êîíòåéíåðû è ïðåôàáû
    /// </summary>
    public static GUILibrary Store;

    void Awake()
    {
        MCSGlobalSimulation.WeaponryInstantiatedEvent += WeaponryInstantiated;
    }

    void WeaponryInstantiated(Weaponry weaponry)
    {
        weaponry.gameObject.AddComponent<WeaponryMarker>();
    }

    /// <summary>
    /// Âîçâðàùàåò GameObject, íàõîäÿùèéñÿ ïîä óêàçàòåëåì ìûøè. Íåîáõîäèì êîëëàéäåð íà ýòîì îáúåêòå, ÷òîáû îí îïðåäåëèëñÿ.
    /// </summary>
    /// <returns></returns>
    public static GameObject MouseOverObject()
    {
        Ray r1 = GUICamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit1;
        if (Physics.Raycast(r1, out hit1, 1000f))
        {
            if (hit1.transform.gameObject.GetComponent<Terrain>()) return null; // Åñëè îáúåêò òåððåéí òî âûõîäèì
            return hit1.transform.gameObject;
        }

        return null;
    }

    /// <summary>
    /// Âîçâðàùàåò GameObject, íàõîäÿùèéñÿ ïîä óêàçàòåëåì ìûøè â ñïóòíèêîâîì âèäå. Íåîáõîäèì êîëëàéäåð íà ýòîì îáúåêòå, ÷òîáû îí îïðåäåëèëñÿ.
    /// </summary>
    /// <returns></returns>
    public static GameObject SatelliteMouseOverObject(LayerMask layermask)
    {
        Ray r1 = SatelliteCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit1;
        if (Physics.Raycast(r1, out hit1, SatelliteCamera.farClipPlane - SatelliteCamera.nearClipPlane))
        {
            if (((layermask >> hit1.transform.gameObject.layer) & 1) == 1)
                return hit1.transform.gameObject;
        }

        Debug.Log("Íå íàéäåí îáúåêò ïîä ìûøîé");
        return null;
    }

    /// <summary>
    /// Âîçâðàùàåò GameObject, íàõîäÿùèéñÿ ïîä óêàçàòåëåì ìûøè â ñïóòíèêîâîì âèäå. Íåîáõîäèì êîëëàéäåð íà ýòîì îáúåêòå, ÷òîáû îí îïðåäåëèëñÿ.
    /// </summary>
    /// <returns></returns>
    public static GameObject SatelliteMouseOverObject(int layermask)
    {
        Ray r1 = SatelliteCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit1;
        if (Physics.Raycast(r1, out hit1, Mathf.Infinity))
        {
            if (layermask == hit1.transform.gameObject.layer)
                return hit1.transform.gameObject;
        }

        return null;
    }
	
	public GameObject targetAir;
	
	/**
	 * 	for launching target for label
	 * */
	void LaunchTask(){
		Debug.Log("Input");
		Debug.Log(GameObject.Find("UInput").GetComponent<UIInput>().label.text);
		/*
		G
		*/
			
	}
	
	
    /// <summary>
    /// Îòêðûòü îêíî ñ óêàçàííûì èìåíåì
    /// </summary>
    /// <param name="windowName"></param>
    void OpenWindow(String windowName)
    {
        GameObject go = null;

        // Åñëè óæå íàõîäèòñÿ â ïàìÿòè - âûâîäèì, öåíòðèðóåì
        try
        {
            switch (windowName)
            {
                case "WeaponryMenu":
                    (Store.Windows["Window_WeaponryMenu"].GetComponent<MCSUIObject>()).Show();

                    break;
                case "TaskMenu":
                    (Store.Windows["Window_TaskMenu"].GetComponent<MCSUIObject>()).Show();
                    break;
                case "Console":
                    (Store.Windows["Window_Console"].GetComponent<MCSUIObject>()).Show();
                    break;
                case "StatusInfo":
                    (Store.Windows["Window_StatusInfo"].GetComponent<MCSUIObject>()).Show();
                    break;

            }
            WindowCommander.GetComponent<WindowCommanderRuntime>().Hide();
            return;
        }
        catch { }

        switch (windowName)
        {
            case "WeaponryMenu":
                go = (GameObject)Instantiate(Store.Prefab_Window_WeaponryMenu);
                Store.Windows["Window_WeaponryMenu"] = go;
                break;
            case "TaskMenu":
                go = (GameObject)Instantiate(Store.Prefab_Window_TaskMenu);
                Store.Windows["Window_TaskMenu"] = go;
                break;
            case "Console":
                Debug.LogError("Console");
                go = (GameObject)Instantiate(Store.Prefab_Window_Console);
                Store.Windows["Window_Console"] = go;
                break;
            case "StatusInfo":
                go = (GameObject)Instantiate(Store.Prefab_Window_StatusInfo);
                Store.Windows["Window_StatusInfo"] = go;
                break;
            default:
                Debug.Log("Íå íàéäåíî îêíî ñ èìåíåì [" + windowName + "]");
                return;
        }

        if (go)
        {
            go.transform.parent = Store.Container_GUI_Main.transform;
            go.transform.localScale = Vector3.one;
            go.layer = LayerMask.NameToLayer("GUI");
        }
    }

    public GameObject AirTarget;
    public GameObject placeStrela;

    //void OnGUI()
    //{
    //    String str = "Àêòèâíûå öåëè:\n";
    //    String str2="";
    //    try
    //    {
    //        //   var obj=MCSGlobalSimulation.Weapons.List;
    //        //foreach(Key)
    //        GameObject[] findO = GameObject.FindGameObjectsWithTag("Weaponry");
    //        if (findO.Length != 0)
    //        {
    //            int i = 1;
    //            foreach (GameObject cnt in findO)
    //            {
    //                Boolean cc = cnt.name.Contains("strela_10");
    //                if (!cc)
    //                {
    //                    if(isAlive(cnt))
    //                    {
    //                        str += "\n\nÖåëü ¹ " + i;
    //                        i++;
    //                        str += "\nÍàçâàíèå: " + cnt.name.Substring(7, cnt.name.Length - 7);
    //                        str += "\nÐàññòîÿíèå äî öåëè: " + Convert.ToInt32((cnt.transform.position - placeStrela.transform.position).sqrMagnitude / 1000f) + " ì";
    //                        str += "\nÑêîðîñòü öåëè: " + Convert.ToInt32(cnt.rigidbody.velocity.sqrMagnitude / 100f) + " ì/ñ";
    //                    }
    //                    else
    //                    {
    //                        str2 += "\n\nÖåëü ¹ " + i;
    //                        i++;
    //                        str2 += "\nÍàçâàíèå: " + cnt.name.Substring(7, cnt.name.Length - 7);
    //                        str2 += "\nÐàññòîÿíèå äî öåëè: " + Convert.ToInt32((cnt.transform.position - placeStrela.transform.position).sqrMagnitude / 1000f) + " ì";
    //                        str2 += "\nÑêîðîñòü öåëè: " + Convert.ToInt32(cnt.rigidbody.velocity.sqrMagnitude / 100f) + " ì/ñ";
    //                        str2 += "\nÂðåìÿ ïîðàæåíèÿ: " + DateTime.Now.TimeOfDay;
    //                        TargetInfo.Text = TargetInfo.Text+str2;
    //                    }
    //                }

    //            }

    //        }
    //        else str += "Âñå öåëè ïîðàæåíû";
    //    }
    //    catch (Exception e1)
    //    {
    //        str = "Îøèáêà!";
    //    }
    //    GUI.Label(new Rect(Screen.width - 150, 100, 150, 500), str);
    //    GUI.Label(new Rect(Screen.width - 320, 100, 150, 500), TargetInfo.Text);
    //    if(GUI.Button(new Rect(Screen.width - 320, 50, 150, 20), "Î÷èñòèòü ñïèñîê"))
    //    {
    //        TargetInfo.Text = "Ïîðàæžííûå öåëè:\n";
    //    }

    //}

    private bool isAlive(GameObject weaponry)
    {
        if (weaponry.GetComponent<AirCraftAI>() == null)
        {
            weaponry.AddComponent<AirCraftAI>();
            return false;
        }
        else return true;
    }

    

    public GameObject[] targets;
    void PlaceTarget(String target)
    {
        for (int i = 0; i < targets.Length; i++)
            if (target.Equals(targets[i].name))
            {
                GameObject go = Instantiate(targets[i]) as GameObject;
                go.transform.position = Vector3.zero;
            }
    }
}
