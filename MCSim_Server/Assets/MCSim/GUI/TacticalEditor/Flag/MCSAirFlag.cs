using UnityEngine;
using System.Collections;

public class MCSAirFlag : MCSFlag
{
    public GameObject Base;

    public LayerMask groundMask;

    private GameObject _target;

    /// <summary>
    /// Устанавливает цель для флага
    /// </summary>
    public override void SetTarget(GameObject target)
    {
        if (target == this) return;

        _target = target;
        transform.parent = target.transform;
        transform.localPosition = Vector3.zero;

        line = Vector.SetLine3D(Color.gray, Base.transform.position, _target.transform.position);
        line.layer = LayerMask.NameToLayer("Satellite_GUI");
        line.lineWidth = 3f;
        line.smoothWidth = true;

        Vector.SetCamera(MCSUICenter.SatelliteCamera);
    }

    void OnDestroy()
    {
        Vector.DestroyLine(ref line);
    }

    private VectorLine line;

	// Use this for initialization
	void Awake () {
            groundMask = MCSUICenter.Satellite_HangMask;
	}

	private Ray r;
    private RaycastHit hit;
    private float distance;

	void Update () {

        if(!_target) return;

        r = new Ray(_target.transform.position, Vector3.down);

        if (Physics.Raycast(r, out hit, 5000f, groundMask))
        {
            float distance = _target.transform.position.y - hit.point.y;
            
            if(distance < 20f) { Hide(); }
            else Show();

            Base.transform.localPosition = Vector3.zero;
            Base.transform.Translate(0, -distance, 0, Space.World);
            Base.transform.rotation = Quaternion.identity;
        }
        else
        {
            float distance = _target.transform.position.y; if (distance < 20f) { Hide(); }
            else Show();
            Base.transform.position = _target.transform.position;
            Base.transform.Translate(0, -distance, 0, Space.World);
            Base.transform.rotation = Quaternion.identity;
        }

        line.points3[0] = Base.transform.position;
        line.points3[1] = _target.transform.position;
        Vector.DrawLine3D(line);
	}

    private bool isVisible = true;

    void Hide()
    {
        if (!isVisible) return;

        foreach (var VARIABLE in GetComponentsInChildren<Renderer>())
        {
            VARIABLE.enabled = false;
        }

        isVisible = false;
    }

    void Show()
    {
        if (isVisible) return;

        foreach (var VARIABLE in GetComponentsInChildren<Renderer>())
        {
            VARIABLE.enabled = true;
        }

        isVisible = true;
    }
}
