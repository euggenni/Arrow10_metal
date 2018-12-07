using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/// <summary>
/// Класс для объектов-пустышек на тактической карте.
/// </summary>
public abstract class MCSDummy : MonoBehaviour
{
    /// <summary>
    /// Позиция пустышки
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// Поворот пустышки
    /// </summary>
    public Quaternion rotation;

    /// <summary>
    /// Информация о пустышке
    /// </summary>
    public object[] Data;
      
    public void Start()
    {
        position = transform.position;
        rotation = transform.rotation;
    }

    /// <summary>
    /// Отображение меню с доступными опциями
    /// </summary>
    public void ShowMenu()
    {
        // Останавливаем движение камеры
        //MCSUICenter.SatelliteCamera.rigidbody.velocity = Vector3.zero;

        if (ListGo) Destroy(ListGo);

        ListGo = Instantiate(MCSUICenter.Store.Prefab_PopupList) as GameObject;
        ListGo.transform.parent = MCSUICenter.Store.Container_PopupList.transform;

        ListGo.transform.position = MCSUICenter.GUICamera.ScreenToWorldPoint(Input.mousePosition);
        ListGo.transform.localPosition = new Vector3(ListGo.transform.localPosition.x,
                                                           ListGo.transform.localPosition.y, 0);
        ListGo.transform.localScale = Vector3.one;

        ListUi = ListGo.GetComponent<UIPopupList>();

        ListUi.eventReceiver = this.gameObject;
        ListUi.functionName = "MenuEvent";

        satelliteCoords = MCSUICenter.SatelliteCamera.transform.position;

        if (MCSUICenter.Store.Active_PopupList != this.gameObject) {
            Destroy(MCSUICenter.Store.Active_PopupList);
        }

        MCSUICenter.Store.Active_PopupList = ListGo;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ListGo)
            {
                Destroy(ListGo);
            }
        }

        if (satelliteCoords != Vector3.zero)
        {
            // Если камера сместилась - удаляем лист
            if (MCSUICenter.SatelliteCamera.transform.position != satelliteCoords)
            {
                Destroy(ListGo);
                satelliteCoords = Vector3.zero;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (ListGo && ListUi.selection.Equals("Menu Option 0"))
                Destroy(ListGo);
        }
    }

    /// <summary>
    /// Создание реального объекта на основе пустышки
    /// </summary>
    public abstract void Instantiate();

    /// <summary>
    /// Событие получения команды из выпадающего списка
    /// </summary>
    public abstract void MenuEvent(string command);


    protected GameObject ListGo;
    protected UIPopupList ListUi;

    private Vector3 satelliteCoords = Vector3.zero;
}
