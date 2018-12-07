using System;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public class MCSDummy_Weaponry : MCSDummy
{
    private Weaponry _weaponry;

    private bool isAi;

    public static Weaponry WeaponryState;
    //private AIControllable ai;

    /// <summary>
    /// ������ �������, ������������ ��� ����, ����� ��������� ��������� ��� ��������� �� ������ �������
    /// </summary>
    //private Vector3 _oldPosition = Vector3.zero;

    //private GameObject waypointgo;

    // Use this for initialization
	new void Start () {
        base.Start();

        Data = new object[3];

	    _weaponry = GetComponent<Weaponry>();
        WeaponryState = _weaponry;
        if(_weaponry)
        {
            Data[0] = _weaponry.GetType().FullName; // ��� ������ Weaponry
            isAi = _weaponry is AIControllable;
            //_weaponry.ImplementedBy<AIControllable>();
            //ai = _weaponry as AIControllable;
        }
        else {
            Debug.LogWarning("��������� ������ Weaponry.");
            Destroy(this);
        }
	}

    new void ShowMenu()
    {
        base.ShowMenu();
        LoadActionList();
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) {
            ShowMenu();
        }
    }

    /// <summary>
    /// �������� ������ ��������
    /// </summary>
    private void LoadActionList()
    {
        if(ListUi)
        {

            ListUi.items.Clear();

            ListUi.items.Add("����������");

            if(isAi)
                ListUi.items.Add("���������� AI");

            ListUi.items.Add("����������");
            ListUi.items.Add("�������");

            ListUi.OnClick();

            PopupMenuScript pms = ListUi.gameObject.GetComponent<PopupMenuScript>();

            pms.ShowHeader(false);
            //pms.label.text = "����";
        }
    }

  
    public override void MenuEvent(string command)
    {
        switch (command)
        {
            case "����������":
                Instantiate();
                break;

            case "���������� AI":
                InstantiateAI();
                break;

            case "�������":
                Remove();
                break;

            default:
                return;
        }
        Destroy(ListGo);
    }


     /// <summary>
    /// �������� ��������� ������� �� ������ ��������
    /// </summary>
    public override void Instantiate()
    {
        Weaponry weaponry = MCSGlobalSimulation.Instantiate(_weaponry.gameObject);


        // ������� ������ ���������
        if (_weaponry.collider.isTrigger) Destroy(_weaponry.collider);

        // �������� ������
         _weaponry.rigidbody.isKinematic = false;

		 //�������� ����������, ������� �� ��������
		 foreach (var col in _weaponry.GetComponentsInChildren<Collider>())
		 {
			 col.enabled = true;
		 }

        // ������� ���������, ���� ��������� ��� AI
        AIControllable ai = _weaponry as AIControllable;

        if(ai != null) {
            ai.AIUnit.DestroyWaypoints();
        }

        // ����� ���������� 
        MCSGlobalSimulation.OnWeaponryInstantiatedEvent(weaponry);

        // ������� ��������
        BoxSelector selector = GetComponent<BoxSelector>();
        Destroy(selector);

        Destroy(this);
    }


    public void InstantiateAI()
    {
        MCSGlobalSimulation.Instantiate(_weaponry.gameObject);
        //IWeaponryControl iweaponry = weaponry as IWeaponryControl;

        AIControllable ai = _weaponry as AIControllable;

        // ������� ������ ���������
        if (_weaponry.collider.isTrigger) Destroy(_weaponry.collider);

        //�������� ����������, ������� �� ��������
        foreach (var collider in _weaponry.GetComponentsInChildren<Collider>())
        {
            if (!collider.isTrigger) collider.enabled = true;
        }

        // �������� ������
        _weaponry.rigidbody.isKinematic = false;

        ai.InitializeAI();

        // ����� ���������� 
        MCSGlobalSimulation.OnWeaponryInstantiatedEvent(_weaponry.GetComponent<Weaponry>());

        // ������� ��������
        BoxSelector selector = GetComponent<BoxSelector>();
        Destroy(selector);

        Destroy(this);
    }

    public void Remove()
    {
        // ������� ���������, ���� ��������� ��� AI
        AIControllable ai = _weaponry as AIControllable;

        if (ai != null) {
            ai.AIUnit.DestroyWaypoints();
        }

        MCSGlobalSimulation.CommandCenter.UIHandler.ClearTargetIfCurrent(gameObject);
        Destroy(_weaponry.gameObject);
    }
}
