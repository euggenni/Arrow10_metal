using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public class MCSMarker_Weaponry : MCSMarker
{
    /// <summary>
    /// Объекты слоев, на которых возможно размещение Weaponry.
    /// </summary>
    public LayerMask Surface;

    /// <summary>
    /// Имя объекта Weaponry, который хранит данный маркер
    /// </summary>

    private GameObject _weaponry;

	new void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {

            if(_weaponry)
                Destroy(_weaponry);

            base.OnMouseUp();
        }
	}

    new void OnMouseDown()
    {
        base.OnMouseDown();
    }

    new void OnMouseUp()
    {
        base.OnMouseUp();

        if (_weaponry)
        {
            //GameObject instantiateWindow = GameObject.Instantiate(Resources.Load("GUI/GUI_WeaponryInstantiate")) as GameObject;

			//instantiateWindow.transform.parent = MCSUICenter.GUICamera.transform.FindChild("Anchor").transform;
			//instantiateWindow.transform.localScale = Vector3.one;
			//instantiateWindow.transform.localPosition = new Vector3(-0.5f, 0.5f, -137.4382f);

			//Weaponry_InstantiateWindow window = instantiateWindow.transform.FindChild("Window").GetComponent<Weaponry_InstantiateWindow>();
			//window.SetWeaponry(_weaponry.GetComponent<Weaponry>());

            _weaponry.AddComponent<MCSDummy_Weaponry>();

            // Пустышка для для создания вейпоинтов
             _weaponry.AddComponent<MCSDummy_Waypoint>();

            _weaponry.AddComponent<BoxSelector>();
        }

        _weaponry = null;
        _verticalDifference = float.NaN;
    }

    private float _verticalDifference = float.NaN;
    new void OnMouseDrag()
    {
        if (Canceled) return;

        Vector3 pos = MCSUICenter.GUICamera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = StartPos.z - 0.1f;
        Cursor.transform.position = pos;
        Cursor.transform.localRotation = LocalStartRot;

        Cursor.transform.localScale = LocalStartScaleGlobal;

        //Если мышь над элементом ГУИ - выходим
        GameObject go = MCSUICenter.MouseOverObject();
        if (go)
            if (go.layer == LayerMask.NameToLayer("GUI"))
            {
                Cursor.GetComponent<UISprite>().enabled = true;

                if (_weaponry)
                    Destroy(_weaponry);
                return;
            }


        if (MCSUICenter.SatelliteCamera.camera.enabled && MCSUICenter.SatelliteCamera.pixelRect.Contains(Input.mousePosition)) // Перемещение в спутниковые вид
        {
            Ray r = MCSUICenter.SatelliteCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            RaycastHit hit;

            // Если попали в объект-площадку
            if (Physics.Raycast(r, out hit, MCSUICenter.SatelliteCamera.farClipPlane - MCSUICenter.SatelliteCamera.nearClipPlane, Surface.value))
            {
                Cursor.GetComponent<UISprite>().enabled = false; // Отключаем иконку

                //Debug.Log(">Layer:" + LayerMask.LayerToName(hit.transform.gameObject.layer) + "GO:" + hit.transform.name);
                if(!_weaponry) { // Если не создан Weaponry 
                    try {
                        AddWeaponry();
                    }
                    catch{}
                }
                
                if(_weaponry)
                {
                    _weaponry.transform.position = hit.point - Vector3.up * _verticalDifference;

                    if (_weaponry.GetComponent<Weaponry>().Category == WeaponryCategory.Ground)
                    ProjectOnPlane(_weaponry);

                    if(!_weaponry.active)
                    _weaponry.active = true;
                }
            }
        }
    }

    /// <summary>
    /// Нормализует поворот объекта относительно земли
    /// </summary>
    /// <param name="weaponry">Объект</param>
    void ProjectOnPlane(GameObject weaponry)
    {
        Bounds bounds = weaponry.collider.bounds;
        Vector3 a, b, c;

        a = bounds.center + weaponry.transform.forward * bounds.extents.z;

        b = bounds.center - weaponry.transform.forward * bounds.extents.z;
        b += weaponry.transform.right*bounds.extents.x;

        c = b - 2*weaponry.transform.right*bounds.extents.x;

        Ray r;
        RaycastHit raycastHit;

        Physics.Raycast(a, -weaponry.transform.up, out raycastHit, Mathf.Infinity, MCSUICenter.Satellite_HangMask);
        a = raycastHit.point;

        Physics.Raycast(b, -weaponry.transform.up, out raycastHit, Mathf.Infinity, MCSUICenter.Satellite_HangMask);
        b = raycastHit.point;

        Physics.Raycast(c, -weaponry.transform.up, out raycastHit, Mathf.Infinity, MCSUICenter.Satellite_HangMask);
        c = raycastHit.point;

        Plane plane = new Plane(a, b, c);

        Vector3 up = plane.normal;

        if (a == Vector3.zero || b == Vector3.zero || c == Vector3.zero) return; 
        weaponry.transform.up = up;
    }

    void AddWeaponry()
    {
        _weaponry = MCSGlobalFactory.InstantiateWeaponry(Data.ToString());
        _weaponry.rigidbody.isKinematic = true;
        _verticalDifference = _weaponry.collider.bounds.min.y;

        MCSFlagType flag = MCSFlagType.Ground;

        //Отключаем коллайдеры, которые не триггеры
        foreach (var collider in _weaponry.GetComponentsInChildren<Collider>()) {
            if (!collider.isTrigger) collider.enabled = false;
        }

        if(_weaponry.GetComponent<Weaponry>() is WeaponryPlane)
        {
            flag = MCSFlagType.Air;
            _verticalDifference -= 100f;
        }

        if (_weaponry.GetComponent<Weaponry>() is WeaponryTank)
        {
            flag = MCSFlagType.Ground;
        }

        //Устанавливаем получатель маркера и его обработчик
        MCSMarkerReciever markerReciever = _weaponry.AddComponent<MCSMarkerReciever>();
        MarkerReciever_WeaponryHandler markerHandler = _weaponry.AddComponent<MarkerReciever_WeaponryHandler>();

        markerReciever.trigger = MCSMarkerReciever.Trigger.OnRelease;
        markerReciever.target = markerHandler;

        _weaponry.InstantiateFlag(flag);
    }
}
