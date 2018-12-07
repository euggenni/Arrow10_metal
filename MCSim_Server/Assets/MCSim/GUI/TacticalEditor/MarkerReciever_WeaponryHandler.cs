using MilitaryCombatSimulator;
using UnityEngine;
using System.Linq;
using System.Collections;


/// <summary>
/// Обработчик входящих Marker сообщений для Weaponry
/// </summary>
public class MarkerReciever_WeaponryHandler : MonoBehaviour, IMarkerHandler
{
    private GameObject _popupList;
    private UIPopupList list;

    private Vector3 satelliteCoords = Vector3.zero;

    #region IMarkerHandler Members

    public MCSMarker Marker;

    private  Weaponry _weaponry;
    public static Weaponry WeaponryState;
    void Start()
    {
        _weaponry = GetComponent<Weaponry>();
        WeaponryState = _weaponry;
    }

    public void SendMarker(MCSMarker marker)
    {
        Marker = marker;
        Debug.LogError(marker);
        if(marker.Data is NetworkPlayer) {
            if (_weaponry is IWeaponryControl)
            {
                
                if (_popupList) Destroy(_popupList);

                _popupList = Instantiate(MCSUICenter.Store.Prefab_PopupList) as GameObject;
                _popupList.transform.parent = MCSUICenter.Store.Container_PopupList.transform;

                _popupList.transform.position = MCSUICenter.GUICamera.ScreenToWorldPoint(Input.mousePosition);
                _popupList.transform.localPosition = new Vector3(_popupList.transform.localPosition.x,
                                                                   _popupList.transform.localPosition.y, 0);
                _popupList.transform.localScale = Vector3.one;

                list = _popupList.GetComponent<UIPopupList>();
                satelliteCoords = MCSUICenter.SatelliteCamera.transform.position;
                LoadRolesList(list);
            }
        }
    }

    void LoadRolesList(UIPopupList list)
    {
        list.eventReceiver = this.gameObject;
        Debug.LogError(list.functionName);
        list.functionName = "RoleSelected";

        if (_weaponry)
        {
            list.items.Clear();
            foreach (CoreToolkit ct in gameObject.GetComponentsInChildren<CoreToolkit>())
            {
                try
                {
                    list.items.Add(ct.Library.GetRole());
                }
                catch { 
						 list.items.Add("-Нет-"); 
					 }
            }

            list.OnClick();
        }
    }

    void RoleSelected(string role)
    {
        foreach (CoreToolkit ct in gameObject.GetComponentsInChildren<CoreToolkit>().Where(ct => ct.Library != null && ct.Library.GetRole() == role))
        {
            SetRole(ct.Library.Name);
            Destroy(_popupList);
        }
    }

    void SetRole(string role)
    {
        IWeaponryControl weaponry = GetComponent<Weaponry>() as IWeaponryControl;
        Debug.LogError(weaponry);
        if(weaponry != null)
        {
            
            NetworkPlayer player = (NetworkPlayer) Marker.Data;

            //weaponry.Owners.Add(player);
            weaponry.Crew[player] = role;

            Debug.Log("Задали роль для " + player + " - " + weaponry.Crew[player]);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_popupList) {
                Destroy(_popupList);
            }

            Marker = null;
        }

        if(satelliteCoords != Vector3.zero)
        {
            // Если камера сместилась - удаляем лист
            if (MCSUICenter.SatelliteCamera.transform.position != satelliteCoords)
            {
                Destroy(_popupList);
                Marker = null;
                satelliteCoords = Vector3.zero;
            }
        }
    }

    public void SendMarkerData(object data)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
