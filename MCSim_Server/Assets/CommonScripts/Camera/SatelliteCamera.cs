using UnityEngine;
using System.Collections;
using System;


#pragma warning disable 0414, 0108, 0219
#pragma warning disable 0618, 0168

[RequireComponent(typeof (Camera))]
public class SatelliteCamera : MonoBehaviour
{
    // Для двойного клика
    private float currentTime = 0;
    private float lastClickTime = 0;
    private float clickTime = 0.3F;

    //public Rect CameraRect;

    [SerializeField]
    public Transform leftTop, rightBottom;

    private Camera _camera;


    private Vector3 centerPoint = Vector3.zero;

    private void Start()
    {
        _camera = this.gameObject.GetComponent<Camera>();


        if (_camera)
        {
            //_camera.orthographic = true;
            //_camera.orthographicSize = 500; // Изначальный размер, для охвата территории 2000х2000
        }
        else
        {
            this.enabled = false;
        }

        gameObject.AddComponent<UICamera>();
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 lu, rb;
        lu = MCSUICenter.GUICamera.WorldToScreenPoint(leftTop.position);
        rb = MCSUICenter.GUICamera.WorldToScreenPoint(rightBottom.position);

        float MapWidth = Mathf.Abs(lu.x - rb.x);
        float MapHeight = Mathf.Abs(lu.y - rb.y);


        camera.pixelRect = new Rect(lu.x, lu.y - MapHeight, MapWidth, MapHeight);

    }

    /// <summary>
    /// Приостановить работу камеры.
    /// </summary>
    public void Freeze()
    {
        camera.enabled = false;
        this.enabled = false;
    }

    /// <summary>
    /// Возобновить работу камеры.
    /// </summary>
    public void Resume()
    {
        this.enabled = true;
        camera.enabled = true;
    }
    
}