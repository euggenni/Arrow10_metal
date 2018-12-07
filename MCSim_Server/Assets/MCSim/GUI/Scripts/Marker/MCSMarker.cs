using System.Linq;
using UnityEngine;
using System.Collections;

/// <summary>
/// ����� ���������������� �������.
/// </summary>
[AddComponentMenu("MCS/Marker")]
public class MCSMarker : MonoBehaviour
{
    protected enum Container
    {
        None,
        GUI,
        Sattellite,
        Marker
    }

    /// <summary>
    /// ���������, � ������� ��������� ������
    /// </summary>
    protected Container _container = Container.None;

    /// <summary>
    /// �������� �������� ��� ��������������
    /// </summary>
    public bool HideOnDrag = false;

    /// <summary>
    /// ������-����� �������
    /// </summary>
    protected GameObject Cursor;

    /// <summary>
    /// ������ �� �������
    /// </summary>
    protected UISprite CursorSprite;

    /// <summary>
    /// ��������� ������� � ������
    /// </summary>
    protected Vector3 StartPos;

    /// <summary>
    /// ��������� �������
    /// </summary>
    protected Vector3 LocalStartPos;

    /// <summary>
    /// ��������� ������
    /// </summary>
    protected Vector3 LocalStartScale;

    /// <summary>
    /// ��������� ���������� ������
    /// </summary>
    protected Vector3 LocalStartScaleGlobal;

    /// <summary>
    /// ��������� �������
    /// </summary>
    protected Quaternion LocalStartRot;
     
    /// <summary>
    /// ������ ������, ������� �������� ������
    /// </summary>
    public object Data;

    /// <summary>
    /// True ���� ��� ����� Escape �� ����� Drag'�
    /// </summary>
    protected bool Canceled;

    public float SizeMultiplier = 1f;

    public void Start()
    {
        LocalStartPos = transform.localPosition;
        LocalStartRot = transform.localRotation;

        LocalStartScale = transform.localScale;
        LocalStartScaleGlobal = transform.lossyScale;
    }

    /// <summary>
    /// ���������� ����� �������-�������
    /// </summary>
    public GameObject InitializeCursor()
    {
        UISprite _sprite = GetComponent<UISprite>();

        if(_sprite)
        {
            GameObject cursor;

            CursorSprite = NGUITools.AddWidget<UISprite>(MCSUICenter.Store.Container_Marker);
            CursorSprite.transform.localScale = Vector3.zero;
            cursor = CursorSprite.gameObject;

            CursorSprite.name = CursorSprite.name + "Marker [" + cursor.layer + "]";

            CursorSprite.atlas = _sprite.atlas;
            CursorSprite.spriteName = _sprite.spriteName;
            CursorSprite.transform.localScale = _sprite.transform.lossyScale;
            CursorSprite.pivot = UIWidget.Pivot.Center;

            cursor.transform.localScale = LocalStartScaleGlobal;

            cursor.transform.position = MCSUICenter.SatelliteCamera.ScreenToWorldPoint(Input.mousePosition);

            CursorSprite.UpdateUVs();
            CursorSprite.enabled = false;
            return cursor;
        }

        return transform.gameObject;
    }

    public void OnMouseDown()
    {
        if (!Input.GetKey(KeyCode.Mouse0)) return;

        Canceled = false;

        // ���������� ������
        MCSUICenter.Marker = this;

        // ������� ������
        Cursor = InitializeCursor();

        if (HideOnDrag)
            GetComponent<UISprite>().enabled = false;

        // ��������� ����������
        gameObject.collider.enabled = false;

        ProcessContainer(Container.GUI);
    }

    public void OnMouseUp()
    {
        SendToBottom();

        Canceled = true;

        //collider.enabled = true;
        transform.localPosition = LocalStartPos;
        transform.localRotation = LocalStartRot;
        transform.localScale = LocalStartScale;

        // ���������� Marker Container � ���� ���
        try
        {
            MCSUICenter.Store.Container_Marker.layer = LayerMask.NameToLayer("GUI");
        } catch {/* ������ ��� ���������. */}

        if(Cursor) Destroy(Cursor);
       if (HideOnDrag)
           GetComponent<UISprite>().enabled = true;


       // ��������� ����������
       gameObject.collider.enabled = true;

       MCSUICenter.Marker = null;
    }

    protected void OnMouseDrag()
    {
        if (Canceled) return;

        // ���������� ����� �������
        Vector3 pos = MCSUICenter.GUICamera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = StartPos.z - 0.1f;
        Cursor.transform.position = pos;

        //���� ���� ��� ��������� ��� - �������
        GameObject go = MCSUICenter.MouseOverObject();
        if (go)
            if (go.layer == LayerMask.NameToLayer("GUI")) return;

        if (MCSUICenter.SatelliteCamera.camera.enabled && MCSUICenter.SatelliteCamera.pixelRect.Contains(Input.mousePosition)) // ����������� � ����������� ���
        {
            ProcessContainer(Container.Sattellite);

            Ray r = MCSUICenter.SatelliteCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, MCSUICenter.SatelliteCamera.farClipPlane - MCSUICenter.SatelliteCamera.nearClipPlane)){
                pos = hit.point;
            }

            // ���������� � ������
            Vector3 LocalCameraPos = MCSUICenter.SatelliteCamera.transform.InverseTransformPoint(pos);
            LocalCameraPos.z = MCSUICenter.SatelliteCamera.nearClipPlane + 10f;
            Cursor.transform.position = MCSUICenter.SatelliteCamera.transform.TransformPoint(LocalCameraPos);


            // �������� ������
            float dif = MCSUICenter.SatelliteCamera.orthographicSize / MCSUICenter.GUICamera.orthographicSize;
            Cursor.transform.localScale = LocalStartScaleGlobal * dif * SizeMultiplier; // ����������� �� ���������� ������
        }
        else
        {
            ProcessContainer(Container.GUI);
        }
    }

    /// <summary>
    /// ��������� ����������� �� ������ ���������� � ������
    /// </summary>
    private void ProcessContainer(Container container)
    {
        if (_container == container) return;
        _container = container;

        switch (container)
        {
            case Container.GUI:
                // ����� � ������
                Cursor.transform.localRotation = LocalStartRot;
                Cursor.transform.localScale = LocalStartScaleGlobal;
                MCSUICenter.Store.Container_Marker.layer = LayerMask.NameToLayer("GUI");
                break;

            case Container.Sattellite:
                // ����� � ������
                Cursor.transform.rotation = MCSUICenter.SatelliteCamera.transform.rotation;
                MCSUICenter.Store.Container_Marker.layer = LayerMask.NameToLayer("Satellite_GUI");
                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnMouseUp();
        }
    }

    /// <summary>
    /// �������� ������� Data �������, ���� �� ������� ���� ����������, ����������� ��������� IMarkerHandler
    /// </summary>
    void SendToBottom()
    {
        Camera camera = null;

        switch (_container)
        {
            case Container.GUI:
                camera = MCSUICenter.GUICamera;
                break;

            case Container.Sattellite:
                camera = MCSUICenter.SatelliteCamera;
                break;

            case Container.None:
                return;

            default:
                Debug.Log("�� ������� ���������� ������, ��� ������� �������� ���������� �� �������.");
                return;
        }

        // ���� ������� ��� �������� ���������� �� �������
        Ray r = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, MCSUICenter.SatelliteCamera.farClipPlane - MCSUICenter.SatelliteCamera.nearClipPlane))
        {
            Component[] scripts = hit.transform.gameObject.GetComponents<Component>();
            
            foreach (Component script in scripts)
            {
                if (script.GetType().GetInterfaces().Contains(typeof(IMarkerHandler)))
                {
                    (script as IMarkerHandler).SendMarker(this);

                    return;
                }
            }

            
        }
    }
}
