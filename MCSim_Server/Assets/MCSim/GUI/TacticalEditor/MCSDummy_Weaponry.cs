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
    /// Старая позиция, используется для того, чтобы создавать вейпоинты при отдалении от старой позиции
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
            Data[0] = _weaponry.GetType().FullName; // Имя класса Weaponry
            isAi = _weaponry is AIControllable;
            //_weaponry.ImplementedBy<AIControllable>();
            //ai = _weaponry as AIControllable;
        }
        else {
            Debug.LogWarning("Необходим объект Weaponry.");
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
    /// Загрузка списка действий
    /// </summary>
    private void LoadActionList()
    {
        if(ListUi)
        {

            ListUi.items.Clear();

            ListUi.items.Add("Разместить");

            if(isAi)
                ListUi.items.Add("Разместить AI");

            ListUi.items.Add("Найстройки");
            ListUi.items.Add("Удалить");

            ListUi.OnClick();

            PopupMenuScript pms = ListUi.gameObject.GetComponent<PopupMenuScript>();

            pms.ShowHeader(false);
            //pms.label.text = "Меню";
        }
    }

  
    public override void MenuEvent(string command)
    {
        switch (command)
        {
            case "Разместить":
                Instantiate();
                break;

            case "Разместить AI":
                InstantiateAI();
                break;

            case "Удалить":
                Remove();
                break;

            default:
                return;
        }
        Destroy(ListGo);
    }


     /// <summary>
    /// Создание реального объекта на основе пустышки
    /// </summary>
    public override void Instantiate()
    {
        Weaponry weaponry = MCSGlobalSimulation.Instantiate(_weaponry.gameObject);


        // Удаляем лишний коллайдер
        if (_weaponry.collider.isTrigger) Destroy(_weaponry.collider);

        // Включаем физику
         _weaponry.rigidbody.isKinematic = false;

		 //Включаем коллайдеры, которые не триггеры
		 foreach (var col in _weaponry.GetComponentsInChildren<Collider>())
		 {
			 col.enabled = true;
		 }

        // Удаляем вейпоинты, если размещаем без AI
        AIControllable ai = _weaponry as AIControllable;

        if(ai != null) {
            ai.AIUnit.DestroyWaypoints();
        }

        // Эвент размещения 
        MCSGlobalSimulation.OnWeaponryInstantiatedEvent(weaponry);

        // Удаляем селектор
        BoxSelector selector = GetComponent<BoxSelector>();
        Destroy(selector);

        Destroy(this);
    }


    public void InstantiateAI()
    {
        MCSGlobalSimulation.Instantiate(_weaponry.gameObject);
        //IWeaponryControl iweaponry = weaponry as IWeaponryControl;

        AIControllable ai = _weaponry as AIControllable;

        // Удаляем лишний коллайдер
        if (_weaponry.collider.isTrigger) Destroy(_weaponry.collider);

        //Включаем коллайдеры, которые не триггеры
        foreach (var collider in _weaponry.GetComponentsInChildren<Collider>())
        {
            if (!collider.isTrigger) collider.enabled = true;
        }

        // Включаем физику
        _weaponry.rigidbody.isKinematic = false;

        ai.InitializeAI();

        // Эвент размещения 
        MCSGlobalSimulation.OnWeaponryInstantiatedEvent(_weaponry.GetComponent<Weaponry>());

        // Удаляем селектор
        BoxSelector selector = GetComponent<BoxSelector>();
        Destroy(selector);

        Destroy(this);
    }

    public void Remove()
    {
        // Удаляем вейпоинты, если размещаем без AI
        AIControllable ai = _weaponry as AIControllable;

        if (ai != null) {
            ai.AIUnit.DestroyWaypoints();
        }

        MCSGlobalSimulation.CommandCenter.UIHandler.ClearTargetIfCurrent(gameObject);
        Destroy(_weaponry.gameObject);
    }
}
