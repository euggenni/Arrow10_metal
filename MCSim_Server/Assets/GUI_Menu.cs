using System;
using System.Linq;
using System.Reflection;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public class GUI_Menu : MCSUIObject
{
    private enum WeaponryType
    {
        All, Air, Tank
    }

    public UIGrid WeaponryList;
    public GameObject WeaponryListItem;
    private bool _searchAI;

    // Use this for initialization
    void Start()
    {

        LoadWeaponryList();

        Show();

        WeaponryList.Reposition();
        WeaponryList.repositionNow = true;
    }

    private GameObject weap_go;
    void LoadWeaponryList()
    {
        ClearList();

        Weaponry weaponry;
        weap_go = new GameObject();
        weap_go.name = "_WeaponryList";


        Type baseType = null;


        var types = Assembly.GetAssembly(baseType).GetTypes();
        foreach (Type t in types.Where(t => t.IsSubclassOf(baseType)))
        {
            if (t.IsSubclassOf(typeof(WeaponryProjectile))) continue;
            if (t.IsSubclassOf(typeof(WeaponryArms))) continue;

            // Пропускаем абстрактные классы
            if (t.IsAbstract) continue;

            // Если не реализует интерфейс управления AI
           
            weaponry = weap_go.AddComponent(t.FullName) as Weaponry;// (Weaponry)Assembly.GetExecutingAssembly().CreateInstance(t.FullName);                    

            AddWeaponryListItem(weaponry);
        }

        Reposition = true;
    }

    void ClearList()
    {
        foreach (Transform child in WeaponryList.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject == WeaponryList.gameObject) continue;

            GameObject.Destroy(child.gameObject);
        }

        if (weap_go) Destroy(weap_go);
    }

    /// <summary>
    /// Добавление элемента в список вооружений
    /// </summary>
    /// <param name="weaponry"></param>
    void AddWeaponryListItem(Weaponry weaponry)
    {
        GameObject list_item = GameObject.Instantiate(WeaponryListItem) as GameObject;
        list_item.transform.parent = WeaponryList.transform;
        list_item.transform.localScale = Vector3.one;
        list_item.GetComponent<UIDragObject>().target = WeaponryList.transform;
        list_item.GetComponent<UIButtonMessage>().target = this.gameObject;
        WeaponryList.Reposition();

        list_item.GetComponent<WeaponryList_Item>().SetWeaponry(weaponry);
    }


    private bool Reposition = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Reposition)
        {
            WeaponryList.repositionNow = true;
            Reposition = false;
        }
    }

    public void RepositionList()
    {
        try
        {
            WeaponryList.GetComponent<SpringPosition>().enabled = true;
        }
        catch { }
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
