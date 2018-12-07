using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public class SatelliteControl : MonoBehaviour {


	public LayerMask DragMask;
    // Для двойного клика
    private float currentTime = 0;
    private float lastClickTime = 0;
    private float clickTime = 0.3F;

    public float ZoomValue = 100f;
    public float MaxZoom = 1000f, MinZoom = 100f;


    private Camera _camera;

    // Координаты для вращения
    private float x = 0.0f;
    private float y = 0.0f;


    public float xSpeed = 200.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20;
    public float yMaxLimit = 80;

    private float _currentZoom = -1f;

    private Vector3 centerPoint = Vector3.zero;

    ////private float startHeight;

    public float reqHeight = float.NaN;
    public float hitDistance = 0f;
    // Use this for initialization
    void Start()
    {
        _camera = this.gameObject.GetComponent<Camera>();

        //startHeight = transform.position.y;

        var angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        if (_camera)
        {
            _currentZoom = _camera.fieldOfView;
        }
        else
        {
            this.enabled = false;
        }

        plane = new Plane(Vector3.up, Vector3.zero);
        CalculateParams();

        transform.position = new Vector3(transform.position.x, MaxZoom, transform.position.z);
        _currentZoom = transform.position.y;
    }

    /// <summary>
    /// Подсчет необходимой высоты
    /// </summary>
    private void CalculateParams()
    {
        Ray ray = new Ray(MCSUICenter.SatelliteCamera.transform.position, MCSUICenter.SatelliteCamera.transform.forward);
            //MCSUICenter.SatelliteCamera.ScreenPointToRay(MCSUICenter.SatelliteCamera.transform.forward);
        if (plane.Raycast(ray, out hitDistance))
        {
            reqHeight = Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.x) * hitDistance;
        }
    }

    private Plane plane;

    // Update is called once per frame
    void Update()
    {
        float zoom = _currentZoom - Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) / Input.GetAxis("Mouse ScrollWheel") * ZoomValue;
     
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            // Если под курсором есть сторонний объект, или мышь не попадает в квадрат камеры
	        if (!MCSUICenter.SatelliteMouseOverObject(DragMask) || MCSUICenter.MouseOverObject() != null) return;
			//if (MCSUICenter.MouseOverObject() || !camera.pixelRect.Contains(Input.mousePosition)) return;
            if (zoom >= MinZoom && zoom <= MaxZoom)
            {
                SetZoom(zoom);
            }
            else
            {
                if(zoom < MinZoom) SetZoom(MinZoom);
                if(zoom > MaxZoom) SetZoom(MaxZoom);
            }
            //CenterMap(Input.mousePosition);
        }

        // Центрирование камеры по клику
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Если под курсором есть сторонний объект, или мышь не попадает в квадрат камеры
            if (MCSUICenter.MouseOverObject() || !camera.pixelRect.Contains(Input.mousePosition)) return;
            
            // Отлов двойного клика
            currentTime = Time.time;
            if ((currentTime - lastClickTime) < clickTime)
            {
                CenterMap(Input.mousePosition);
            }
            lastClickTime = currentTime;
        }

        // Вращение камеры
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            // Если под курсором есть сторонний объект, или мышь не попадает в квадрат камеры
            if (MCSUICenter.MouseOverObject() || !camera.pixelRect.Contains(Input.mousePosition) || !camera.enabled) return;

            Ray r = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit0;
            if (Physics.Raycast(r, out hit0, _camera.farClipPlane - _camera.nearClipPlane))
            {
                centerPoint = hit0.point;
                //centerPoint.y = -350;
            }
            else centerPoint = Vector3.zero;

            //Vector3 screenPoint = camera.ScreenToWorldPoint(Input.mousePosition);
            //float height = Math.Abs(screenPoint.y - -351);
            //float hypotenuse = height / Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.x);

            //centerPoint = screenPoint + transform.forward * hypotenuse;
            //centerPoint.y = -351;
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (centerPoint != Vector3.zero)
            {
                x = Input.GetAxis("Mouse X") * xSpeed * Time.fixedDeltaTime;
                y = -Input.GetAxis("Mouse Y") * ySpeed * Time.fixedDeltaTime;


                transform.RotateAround(centerPoint, Vector3.up, x);

                if (transform.eulerAngles.x + y >= yMinLimit && transform.eulerAngles.x + y <= yMaxLimit)
                    transform.RotateAround(centerPoint, transform.right, y);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
          //  centerPoint = Vector3.zero;
        }

        CalculateParams();

        
        Vector3 reqShift = _camera.transform.position + _camera.transform.forward * (hitDistance - _currentZoom);
        if(reqShift != transform.position)
        {

            Ray ray = new Ray(transform.position, reqShift - transform.position);
            RaycastHit hit;

            if(!Physics.Raycast(ray, out hit, Vector3.Distance(transform.position, reqShift)))
            {
                //transform.position = reqShift;
                if (!ColliderActive)
                    //transform.position = reqShift;
                    iTween.MoveUpdate(gameObject, reqShift, 0.3f);
                //iTween.Vector3Update(transform.position, reqShift, 15);
                else
                {
                    _currentZoom = hitDistance;
                }
            }
            else
            {
                _currentZoom = hitDistance;
            }

            
        }
    }

    private bool ColliderActive;
    void OnTriggerStay(Collider collisionInfo)
    {
        ColliderActive = true;
        transform.Translate(0, 0, -250f * Time.deltaTime, Space.Self);
    }

    void OnTriggerExit(Collider collisionInfo)
    {
        ColliderActive = false;
    }

    void FixedUpdate()
    {

        //if(transform.position.y < 500)
        //    transform.Translate(0, 5f, 0, Space.World);


        //if (transform.position.y > startHeight)
        //    transform.position += Vector3.down * (transform.position.y - startHeight);
                //Translate(0, -5f, 0, Space.World);
    }


    public void SetZoom(float value)
    {
        _currentZoom = value;

        //if (Vector3.Distance(transform.position, reqShift) > ZoomValue)
        //    transform.position = iTween.Vector3Update(_camera.transform.position, reqShift, 10f);
    }

    /// <summary>
    /// Смотреть вниз
    /// </summary>
    /// <returns></returns>
    public IEnumerator LookDown()
    {
        x = transform.eulerAngles.x;
        y = transform.eulerAngles.y;

        Debug.Log("x " + x);
        while (true)
        {
            x = Mathf.Lerp(x, 90, 0.1f);
            y = Mathf.Lerp(y, 0, 0.1f);

            //x = iTween.FloatUpdate(transform.eulerAngles.x, 90, 10f);
            //y = iTween.FloatUpdate(transform.eulerAngles.y, 0, 5f);
            transform.rotation = Quaternion.Euler(x, y, 0);
       
            if (Mathf.Approximately(Mathf.RoundToInt(transform.eulerAngles.x), 90) && Mathf.Approximately(Mathf.RoundToInt(transform.eulerAngles.y), 0))
                break;

            yield return new WaitForFixedUpdate();
        }
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }

    private Vector3 _oldMousePosition = Vector3.one;
    private void CenterMap(Vector2 mousePosition)
    {
        //if (Vector3.Distance(_oldMousePosition, Input.mousePosition) < 50f) return;
        ////if (_oldMousePosition == Input.mousePosition) { Debug.Log("return"); return;}
        //else
        //{
        //    _oldMousePosition = Input.mousePosition;
        //}

        RaycastHit hit;
        Ray r = new Ray(_camera.transform.position, _camera.transform.forward);
        float distance;
        if (Physics.Raycast(r, out hit, 10000))
        {
            distance = Vector3.Distance(hit.point, _camera.transform.position);
            _currentZoom = distance;
            hitDistance = distance;
        }
        else
        {
            return;
        }

        r = _camera.ScreenPointToRay(new Vector3(mousePosition.x, mousePosition.y, 0f));
        
        if (Physics.Raycast(r, out hit, camera.farClipPlane - camera.nearClipPlane))
        {
            Vector3 targetPos = hit.point;

            Vector3 vector = -_camera.transform.forward*distance;
            //targetPos.y = transform.position.y;

            //iTween.MoveTo(_camera.gameObject, targetPos + vector, 0.1f);
            transform.position = targetPos + vector;
        }

        Vector3 screenPos = camera.WorldToScreenPoint(hit.point);
        //SetCursorPos((int)screenPos.x, (int)screenPos.y);
        //SetCursorPos((int)camera.pixelRect.xMin + (int)camera.pixelRect.width / 2, (int)camera.pixelRect.yMax - (int)camera.pixelRect.height / 2);
    }

    //[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    //public static extern int SetCursorPos(int x, int y);
}
