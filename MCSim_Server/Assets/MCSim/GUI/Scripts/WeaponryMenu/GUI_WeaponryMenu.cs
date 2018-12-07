using System;
using System.Linq;
using System.Reflection;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public class GUI_WeaponryMenu : MCSUIObject
{
    private enum WeaponryType
    {
        All, Air, Tank
    }

    public UIGrid WeaponryList;
    public GameObject WeaponryListItem;

    //public UICheckbox Checkbox_All;
    //public UICheckbox Checkbox_Air;
    //public UICheckbox Checkbox_Tank;
    //public UICheckbox Checkbox_AI;
	
    //public UIInput inputDistance;
    //public UIInput inputAsimut;
    //public UIInput inputHeight;
	
    //public GameObject targetAir;
    //public GameObject placeStrela;

    private bool _searchAI ;

	// Use this for initialization
	void Start ()
	{
        
        //LoadWeaponryList();

        //Show();

        //WeaponryList.Reposition();
        //WeaponryList.repositionNow = true;
        //placeStrela = GameObject.Find("Placeholder");
	}
	
    //public void LaunchTask()
    //{
    //    GameObject target = targetAir.GetComponent<Delegate>().target;
    //    GameObject point = targetAir.GetComponent<Delegate>().point;
    //    int height = Convert.ToInt32(inputHeight.text);
    //    point.transform.position = placeStrela.transform.position + Vector3.up*height;
    //    point.GetComponent<MCSPlaneWaypoint>().Height = height;
    //    int angle = Convert.ToInt32(inputAsimut.text);
    //    if((angle<30&&angle>=0)||(angle<360&&angle>330)){
    //        Debug.Log("Yeaaa");
    //        int distance = Convert.ToInt32(inputDistance.text);
    //        Debug.Log(distance);
    //        target.transform.position = placeStrela.transform.position + Vector3.forward*distance +Vector3.up*height;
    //    }
    //    else if(angle>=30&&angle<=70){
    //        Debug.Log("Yeaaa");
    //        int distance = Convert.ToInt32(inputDistance.text);
    //        Debug.Log(distance);
    //        target.transform.position = placeStrela.transform.position + Vector3.right*(distance - distance/5) - Vector3.forward*(distance/5) +Vector3.up*height;		
    //    }
    //    else if(angle>70&&angle<150){
    //        Debug.Log("Yeaaa");
    //        int distance = Convert.ToInt32(inputDistance.text);
    //        Debug.Log(distance);
    //        target.transform.position = placeStrela.transform.position + Vector3.right*distance +Vector3.up*height;	
    //    }
		
    //    else if(angle>=160&&angle<220){
    //        Debug.Log("Yeaaa");
    //        int distance = Convert.ToInt32(inputDistance.text);
    //        Debug.Log(distance);
    //        target.transform.position = placeStrela.transform.position - Vector3.forward*distance +Vector3.up*height;	
    //    }
		
    //    else if(angle>=220&&angle<300){
    //        Debug.Log("Yeaaa");
    //        int distance = Convert.ToInt32(inputDistance.text);
    //        Debug.Log(distance);
    //        target.transform.position = placeStrela.transform.position - Vector3.right*distance +Vector3.up*height;		
    //    }
		
    //    else if(angle>=300&&angle<=330){
    //        Debug.Log("Yeaaa");
    //        int distance = Convert.ToInt32(inputDistance.text);
    //        Debug.Log(distance);
    //        target.transform.position = placeStrela.transform.position - Vector3.right*(distance - distance/5) + Vector3.forward*(distance/5) +Vector3.up*height;		
    //    }
    //    else{
    //        Debug.Log("Some shit");
    //        return;
    //    }
    //    GameObject go = Instantiate(targetAir) as GameObject;	
    //}

    private GameObject weap_go;
    void LoadWeaponryList()
    {
        //ClearList();

        //Weaponry weaponry;
        //weap_go = new GameObject();
        //weap_go.name = "_WeaponryList";
               

        //Type baseType = null;

        //if(Checkbox_Air.isChecked)
        //    baseType = typeof(WeaponryPlane);

        //if(Checkbox_Tank.isChecked)
        //    baseType = typeof(WeaponryTank);

        //if (Checkbox_All.isChecked)
        //    baseType = typeof(Weaponry);


        //var types = Assembly.GetAssembly(baseType).GetTypes();
        //foreach (Type t in types.Where(t => t.IsSubclassOf(baseType)))
        //{
        //    if (t.IsSubclassOf(typeof(WeaponryProjectile))) continue;
        //    if (t.IsSubclassOf(typeof(WeaponryArms))) continue;

        //    // Пропускаем абстрактные классы
        //    if(t.IsAbstract) continue;

        //    // Если не реализует интерфейс управления AI
        //    if (Checkbox_AI.isChecked && !t.GetInterfaces().Contains(typeof(AIControllable)))
        //        continue;

        //    weaponry = weap_go.AddComponent(t.FullName) as Weaponry;// (Weaponry)Assembly.GetExecutingAssembly().CreateInstance(t.FullName);                    

        //    AddWeaponryListItem(weaponry);
        //}

        //Reposition = true;
    }

    void ClearList()
    {
        //foreach (Transform child in WeaponryList.GetComponentsInChildren<Transform>()) {
        //    if (child.gameObject == WeaponryList.gameObject) continue; 

        //    GameObject.Destroy(child.gameObject);
        //}

        //if (weap_go) Destroy(weap_go);
    }

    /// <summary>
    /// Добавление элемента в список вооружений
    /// </summary>
    /// <param name="weaponry"></param>
    void AddWeaponryListItem(Weaponry weaponry)
    {
        //GameObject list_item = GameObject.Instantiate(WeaponryListItem) as GameObject;
        //list_item.transform.parent = WeaponryList.transform;
        //list_item.transform.localScale = Vector3.one;
        //list_item.GetComponent<UIDragObject>().target = WeaponryList.transform;
        //list_item.GetComponent<UIButtonMessage>().target = this.gameObject;
        //WeaponryList.Reposition();

        //list_item.GetComponent<WeaponryList_Item>().SetWeaponry(weaponry);
    }


    private bool Reposition = false;
	// Update is called once per frame
	void FixedUpdate () {
        //if (Reposition)
        //{
        //    WeaponryList.repositionNow = true;
        //    Reposition = false;
        //}
	}

    public void RepositionList()
    {
        //try
        //{
        //    WeaponryList.GetComponent<SpringPosition>().enabled = true;
        //}
        //catch{}
    }

    #region MCSIUIObject Members

    public override void Hide()
    {
        Destroy(gameObject);
    }

    public override void Show()
    {
        Vector3 pos = MCSUICenter.GUICamera.ScreenToWorldPoint(new Vector3(Screen.width * 3f / 4f, Screen.height / 2f, 0));
        
        iTween.MoveTo(gameObject, new Vector3(pos.x, pos.y, 0), 0.5f);

        transform.localScale = Vector3.one * 0.88f;
    }

    public override void Close()
    {
        Destroy(gameObject);
    }

    public bool Visible
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
            throw new System.NotImplementedException();
        }
    }

    #endregion
}
