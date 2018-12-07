using System.Collections.Generic;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public class MCSUIHandler : MonoBehaviour
{
    private GameObject _oldTarget;
    private GameObject TrackingObject;

    private VectorLine line;

    private List<string> Coroutines = new List<string>();

    private Vector3[] v3array;

	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Mouse0))
	    {
	        Ray r = MCSUICenter.SatelliteCamera.ScreenPointToRay(Input.mousePosition);
	        RaycastHit hit;
            //GameObject go = MCSUICenter.SatelliteMouseOverObject(LayerMask.NameToLayer("Default"));
            //SelectTarget(go);

            LayerMask mask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Satellite_GUI"));

            if (Physics.Raycast(r, out hit, Mathf.Infinity, mask))
            {
                SelectTarget(hit.transform.gameObject);
            }
	    }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ClearTarget();
        }
	}

    /// <summary>
    /// Очистка UI
    /// </summary>
    public void ClearTarget()
    {
        OnTargetChanged(null);
        TrackingObject = null;
    }

    /// <summary>
    /// Очистка UI, если текущая цель
    /// </summary>
    public void ClearTargetIfCurrent(GameObject target)
    {
        if (TrackingObject == target)
        {
            OnTargetChanged(null);
            TrackingObject = null;
        }
    }

    void OnTargetChanged(GameObject newTarget)
    {
        foreach (string coroutine in Coroutines) {
            StopCoroutine(coroutine);
            if(line != null)
                Vector.DestroyLine(ref line);
        }
        Coroutines.Clear();
        _oldTarget = newTarget;
    }

    void SelectTarget(GameObject target)
    {
        if (target == null) return;

        if(_oldTarget)
        {
            if (_oldTarget != target)
                OnTargetChanged(target);
            else return;
        }
        
        TrackingObject = target;
        Vector.DestroyLine(ref line);

        object data;
        //MCSDummy dummy;

        if ((data = target.GetComponent<Weaponry>()) != null)
        {
            if (data is AIControllable)
            {
                StartCoroutine(UpdateWeaponryWaypoints(data as Weaponry));
                Coroutines.Add("UpdateWeaponryWaypoints");
                return;
            }
        }

        //data = GetComponent<MCSDummy_Waypoint>();

        if ((data = target.GetComponent<MCSDummy_Waypoint>()) != null)
        {
            MCSDummy_Waypoint dummy = data as MCSDummy_Waypoint;
            SelectTarget(dummy.Weaponry.gameObject);
            //Debug.Log("Вейпоинт: " + dummy.Weaponry);
            //StartCoroutine(UpdateWeaponryWaypoints(dummy.Weaponry));
            //Coroutines.Add("UpdateWeaponryWaypoints");
            return;
        }
    }

    private Vector3[] GetWaypoints(Weaponry weaponry)
    {

        AIControllable ai = weaponry as AIControllable;

        if (ai.AIUnit.Waypoints.Count == 0) return new Vector3[0];

        int startPoint = 0; // Если еще не инициализирован, currentWaypoint = -1

        if (ai.AIUnit.CurrentWaypoint != -1)
        {
            startPoint = ai.AIUnit.CurrentWaypoint;
        }
        Vector3[] waypoints = new Vector3[ai.AIUnit.Waypoints.Count - startPoint + 1];

        waypoints[0] = weaponry.gameObject.transform.position;

        for (int i = startPoint; i < ai.AIUnit.Waypoints.Count; i++)
        {
            // Debug.Log(i + " из " + ai.AIUnit.Waypoints.Count + " равен " + ai.AIUnit.Waypoints[i].Point);
            waypoints[i + 1] = ai.AIUnit.Waypoints[i].Point;
        }

        return waypoints;
    }

    private IEnumerator UpdateWeaponryWaypoints(Weaponry weaponry)
    {
        while (weaponry && weaponry.GetComponent<AIUnit>())
        {
            if (TrackingObject == weaponry.gameObject)
            {
                v3array = GetWaypoints(weaponry);

                if (v3array.Length >= 2)
                {
                    if(line == null)
                    {
                        line = Vector.SetLine3D(Color.green, v3array);
                        line.lineWidth = 1.5f;
                        line.layer = LayerMask.NameToLayer("Satellite_GUI");
                    }
                    else
                    line.Resize(v3array);

                    Vector.DrawLine3D(line);
                }
                else
                {
                    if(line != null)
                        Vector.DestroyLine(ref line);
                }

            }

            yield return new WaitForFixedUpdate();
        }

        Vector.DestroyLine(ref line);
    }
}
