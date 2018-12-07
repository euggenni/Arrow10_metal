using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public class Weaponry_InstantiateWindow : MonoBehaviour, MCSIUIObject
{
    /// <summary>
    /// Редактируемая боевая единица
    /// </summary>
    public Weaponry Weaponry;


    public UIGrid ArmsList;
    public GameObject ArmsListItem;

    public UISprite Icon;
    public UILabel Name;

	// Use this for initialization
	void Awake ()
	{
	    //Debug.Log(MCSProjectHelper.GetDescription(ArmsType.RocketLauncher));
        if (Weaponry)
            LoadArmsList();
	}

    void Start()
    {
        transform.Translate(0,0,-1f);
    }
	
    public void SetWeaponry(Weaponry weaponry)
    {
        Weaponry = weaponry;

        if (weaponry.Resources.ContainsKey("Icon_WeaponryList"))
        {
            Icon.spriteName = weaponry.Resources["Icon_WeaponryList"].ToString();
            Icon.color = new Color(200, 200, 200);
            Icon.UpdateUVs();
        }

        Name.text = weaponry.Name;

        LoadArmsList();

        transform.localScale = Vector3.one * MCSUICenter.sizeK;
    }

    void LoadArmsList()
    {
        ArmsList.ClearList();

        WeaponryArms[] arms = Weaponry.GetComponentsInChildren<WeaponryArms>();

        foreach (WeaponryArms arm in arms) {
            AddArmsListItem(arm);
        }
        
        ArmsList.repositionNow = true;
    }

    /// <summary>
    /// Добавление элемента в список вооружений
    /// </summary>
    /// <param name="weaponry"></param>
    void AddArmsListItem(WeaponryArms arms)
    {
        GameObject list_item = GameObject.Instantiate(ArmsListItem) as GameObject;

        //ArmsList_Item ali = list_item.GetComponent<ArmsList_Item>(); // Получаем компоненты

        list_item.transform.parent = ArmsList.transform;

        list_item.GetComponent<UIDragObject>().target = ArmsList.transform;
        list_item.GetComponent<ArmsList_Item>().SetArms(arms);

        ArmsList.Reposition();

    }

    public void Confirm()
    {
        Destroy(this.gameObject);
    }

    #region MCSIUIObject Members

    public void Hide()
    {
        throw new System.NotImplementedException();
    }

    public void Show()
    {
        throw new System.NotImplementedException();
    }

    public void Close()
    {
        if (Weaponry)
        {
            Destroy(Weaponry.gameObject);
        }

        Destroy(this.gameObject.transform.parent.gameObject);
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
