using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using MilitaryCombatSimulator;

#pragma warning disable 0414, 0108, 0219
#pragma warning disable 0618, 0168

public class WindowCommanderRuntime : MCSUIObject
{
    /// <summary>
    /// Главное окно
    /// </summary>
    public GameObject Panel;

    /// <summary>
    /// Спрайт окна
    /// </summary>
    public GameObject Window;

    /// <summary>
    /// Контейнер для карты
    /// </summary>
    public GameObject MapContainer;

    /// <summary>
    /// Контейнер для игроков
    /// </summary>
    public GameObject PlayerContainer;

    /// <summary>
    /// Лист игроков
    /// </summary>
    public UIGrid PlayerList;

    /// <summary>
    /// Префаб игрока
    /// </summary>
    public GameObject PlayerListItem;

    /// <summary>
    /// Камера-спутник
    /// </summary>
    public Camera Satellite;

    public GameObject Map_Left_Top, Map_Right_Bottom;

    public NetworkPlayer networkPlayerLocateButton;
    private Rect CameraRect = new Rect(100, 100, 100, 100);


    private float border = 12;
    void Start()
    {
	    MCSNetworkCenter.OnParticipantConnected += AddPlayer;
	    MCSNetworkCenter.OnParticipantDisconnected += RemovePlayer;
        //if (Screen.width < 1280)
        //{
            
        //}

        Calibrate();
        FillPlayerList();

        Hide();

        float k = (Screen.width) / (Window.transform.localScale.x);

        if (Window.transform.localScale.x > Window.transform.localScale.x * k)
        {
            Panel.transform.localScale *= k;
            MCSUICenter.sizeK = k;

            PlayerList.cellHeight *= k;
        }
        else
        {
            Panel.transform.localScale *= 1.2f;
        }
    }

    /// <summary>
    /// Калибровка камеры и коллайдеров
    /// </summary>
    void Calibrate()
    {
        SatelliteCamera sc = MCSUICenter.SatelliteCamera.GetComponent<SatelliteCamera>();
        sc.leftTop = Map_Left_Top.transform;
        sc.rightBottom = Map_Right_Bottom.transform;
    }

    /// <summary>
    /// Загрузить список игроков
    /// </summary>
    void FillPlayerList()
    {
        foreach (NetworkPlayer player in MCSGlobalSimulation.Players.List.Keys)
        {
           AddPlayerToList(player);
        }
    }

    /// <summary>
    /// Добавляет игрока и информацию о нем в список игроков
    /// </summary>
    /// <param name="player"></param>
    void AddPlayerToList(NetworkPlayer player)
    {
        GameObject list_item = Instantiate(PlayerListItem) as GameObject;

        PlayerList_Item pli = list_item.gameObject.GetComponent<PlayerList_Item>();
        pli.Player = player;
        networkPlayerLocateButton = player;
        pli.PlayerMarker.Data = player;

        list_item.transform.parent = PlayerList.transform;
        //list_item.transform.localScale = Vector3.one;
        list_item.GetComponent<UIDragObject>().target = PlayerList.transform;
        list_item.GetComponent<UIButtonMessage>().target = this.gameObject;
        PlayerList.repositionNow = true;
    }

    /// <summary>
    /// Очистить список игроков
    /// </summary>
    void ClearPlayerList()
    {
        PlayerList_Item[] list_pli = PlayerList.GetComponentsInChildren<PlayerList_Item>();

        foreach (PlayerList_Item playerListItem in list_pli) {
            Destroy(playerListItem.gameObject);
        }
    }

    void AddPlayer(MCSPlayer player)
    {
        if (!_visible) return;

        try
        {
            AddPlayerToList(player.NetworkPlayer);
        }
        catch(Exception e) { Debug.LogWarning(e.Message);}
    }

	 void RemovePlayer(MCSPlayer player)
    {
        if (!_visible) return;
        try
        {
            foreach (PlayerList_Item playerListItem in PlayerList.GetComponentsInChildren<PlayerList_Item>())
            {
                //if (playerListItem.PlayerMarker.Data.Equals(player))
                if (!playerListItem.Player.guid.Equals(player.NetworkPlayer.guid)) continue;
                Destroy(playerListItem.gameObject);
                break;
            }

            PlayerList.repositionNow = true;
        }
        catch (Exception e) { Debug.LogWarning(e.Message); }
    }

	void OnDestroy()
	{
		MCSNetworkCenter.OnParticipantConnected -= AddPlayer;
		MCSNetworkCenter.OnParticipantDisconnected -= RemovePlayer;
	}
       
    #region MCSIUIObject Members

    public override void Hide()
    {
        MCSUICenter.SatelliteCamera.GetComponent<SatelliteCamera>().Freeze();
        _visible = false;

        ClearPlayerList();

        this.GetComponent<NGUIPanel>().enabled = false;


        MCSUICenter.Store.Windows["WindowCommander"] = null;
    }

    Weaponry InstantiateLocally()
    {
        GameObject weaponry = MCSGlobalFactory.InstantiateWeaponry("WeaponryTank_Strela10");
        weaponry.rigidbody.isKinematic = true;

        MCSFlagType flag = MCSFlagType.Ground;

        //Отключаем коллайдеры, которые не триггеры
        foreach (var collider in weaponry.GetComponentsInChildren<Collider>())
        {
            if (!collider.isTrigger) collider.enabled = false;
        }
        
        flag = MCSFlagType.Ground;

        //Устанавливаем получатель маркера и его обработчик
        MCSMarkerReciever markerReciever = weaponry.AddComponent<MCSMarkerReciever>();
        MarkerReciever_WeaponryHandler markerHandler = weaponry.AddComponent<MarkerReciever_WeaponryHandler>();

        markerReciever.trigger = MCSMarkerReciever.Trigger.OnRelease;
        markerReciever.target = markerHandler;

        weaponry.InstantiateFlag(flag);

        Transform placeholder = GameObject.Find("Placeholder").transform;
        weaponry.transform.position = placeholder.position;

        placeholder.position += Vector3.right * weaponry.collider.bounds.size.x * 2f;

        return weaponry.GetComponent<Weaponry>();
    }

    public void Locate()
    {
        Weaponry weaponry = InstantiateLocally();
        var iweaponry = weaponry as IWeaponryControl;
        
        //Debug.LogError("Locate");
        //MCSMarker Marker = new MCSMarker() ;
        //IWeaponryControl weaponry = MCSDummy_Weaponry.WeaponryState as IWeaponryControl;
        //Debug.LogError(weaponry);
        
        if (iweaponry != null)
        {
            iweaponry.Crew[networkPlayerLocateButton] = "Strela-10_Operator";

            Debug.Log("Задали роль для " + networkPlayerLocateButton + " - " + iweaponry.Crew[networkPlayerLocateButton]);

            InstantiateButtonClick(weaponry);
            //GameObject LieTarget = Resources.Load("LieTarget") as GameObject;
            //Instantiate(LieTarget);
        }

    }

    public void InstantiateButtonClick(Weaponry weaponry)
    {
        weaponry = MCSGlobalSimulation.Instantiate(weaponry.gameObject);

        // Удаляем лишний коллайдер
        if (weaponry.collider.isTrigger)
            Destroy(weaponry.collider);

        // Включаем физику
        weaponry.rigidbody.isKinematic = false;

        //Включаем коллайдеры, которые не триггеры
        foreach (var col in weaponry.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }

        // Удаляем вейпоинты, если размещаем без AI
        AIControllable ai = weaponry as AIControllable;

        if (ai != null)
        {
            ai.AIUnit.DestroyWaypoints();
        }

        // Эвент размещения 
        MCSGlobalSimulation.OnWeaponryInstantiatedEvent(weaponry);

        // Удаляем селектор
        BoxSelector selector = GetComponent<BoxSelector>();
        //Destroy(selector);

        //Destroy(this);
    }

    public override void Show()
    {
        Vector3 pos = MCSUICenter.GUICamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f - Screen.height / 8f, 0));
        iTween.MoveTo(gameObject, new Vector3(pos.x, pos.y, transform.position.z), 0.5f);

        if(_visible) {
            return;
        }

        Calibrate();

        _visible = true;

        //PlayerList.gameObject.SetActiveRecursively(true);
        FillPlayerList();
        this.GetComponent<NGUIPanel>().enabled = true;
        MCSUICenter.SatelliteCamera.GetComponent<SatelliteCamera>().Resume();

        MCSUICenter.Store.Windows["WindowCommander"] = this.gameObject;
    }

    public override void Close()
    {
        MCSUICenter.SatelliteCamera.GetComponent<SatelliteCamera>().Freeze();
        Destroy(this.gameObject);
    }

    public bool Visible
    {
        get { return _visible; }
        set { _visible = value; 
            if(value) Show();
            else Hide();
        }
    }

   
    #endregion

}
