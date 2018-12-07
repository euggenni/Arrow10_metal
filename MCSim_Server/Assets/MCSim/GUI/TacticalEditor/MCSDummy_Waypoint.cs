using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public class MCSDummy_Waypoint : MCSDummy
{
    public Weaponry Weaponry;
    public MCSWaypoint Waypoint;

    /// <summary>
    /// Старая позиция, используется для того, чтобы создавать вейпоинты при отдалении от старой позиции
    /// </summary>
    private Vector3 _oldPosition = Vector3.zero;

    private GameObject _nextWaypointGo;

    private AIControllable ai;

    void Awake()
    {
        Weaponry = GetComponent<Weaponry>();
        Waypoint = GetComponent<MCSWaypoint>();
    }

    new void Start()
    {
        base.Start();

        if (!Weaponry) return;

        ai = Weaponry as AIControllable;

        if (ai == null) Destroy(this);

        if (!Waypoint)
            Waypoint = gameObject.AddComponent<MCSWaypoint>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !Waypoint.NextPoint)
        {
            // Выключаем флаг на Weaponry, т.к. если мы постоянно меняем позишн, и на экране объект не двигается - флаг меняет позицию

            gameObject.GetComponentInChildren<MCSFlag>().enabled = false;
            _oldPosition = transform.position;
        }

        if (_oldPosition != Vector3.zero)
        {
            if (transform.position != _oldPosition)
            {
                AddWaypoint(transform.position);
            }
            transform.position = _oldPosition;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (_nextWaypointGo)
            {
                _nextWaypointGo.transform.position = GizmoController.Controller.transform.position;

                _nextWaypointGo.collider.enabled = true;
                _nextWaypointGo.gameObject.AddComponent<BoxSelector>();

                GizmoController.Controller.SetSelectedObject(_nextWaypointGo.transform);
                GizmoController.Controller.Show(GIZMO_MODE.TRANSLATE);

                // Добавляем пустышку-вейпоинт для создания новых
                MCSDummy_Waypoint dummy = _nextWaypointGo.AddComponent<MCSDummy_Waypoint>();
                dummy.Weaponry = Weaponry;

                // Включаем флаг на Weaponry
                gameObject.GetComponentInChildren<MCSFlag>().enabled = true;

                _nextWaypointGo = null;
            }

            _oldPosition = Vector3.zero;
        }

        if (_nextWaypointGo)
            _nextWaypointGo.transform.position = GizmoController.Controller.transform.position;
    }

    /// <summary>
    /// Создание точки следования в указанных координатах
    /// </summary>
    /// <param name="position">Координаты точки следования</param>
    void AddWaypoint(Vector3 position)
    {
        if (Waypoint.NextPoint) return;

        MCSWaypoint waypoint = null;

        _nextWaypointGo = GameObject.Instantiate(MCSUICenter.Store.Flag_Sphere) as GameObject;
        _nextWaypointGo.gameObject.layer = LayerMask.NameToLayer("Satellite_GUI");
        _nextWaypointGo.collider.isTrigger = true;
        _nextWaypointGo.collider.enabled = false;
        _nextWaypointGo.transform.localScale = Vector3.one * 10f;

        _nextWaypointGo.transform.parent = MCSUICenter.Store.Container_Waypoints.transform;
        _nextWaypointGo.transform.position = position;

        if (Weaponry is WeaponryPlane)
        {
            waypoint = _nextWaypointGo.AddComponent<MCSPlaneWaypoint>();
            ai.AIUnit.Waypoints.Add(waypoint);

            try
            {
                // Указываем предыдущей точке, что эта - следующая
                ai.AIUnit.Waypoints[ai.AIUnit.Waypoints.Count - 2].NextPoint = waypoint;
            }
            catch { /* Текущий вейпоинт - первый */ }

            _nextWaypointGo.InstantiateFlag(MCSFlagType.Air);
        }

        Waypoint.NextPoint = waypoint;
    }

    public override void Instantiate()
    {
    }

    public override void MenuEvent(string command)
    {
    }
}
