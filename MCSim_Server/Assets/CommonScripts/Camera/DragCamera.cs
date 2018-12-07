using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DragCamera : MonoBehaviour
{
    public float speed = .5f;
    private Camera GUICamera;
    private Rect CameraRect;

    private Vector3 startHit = Vector3.zero;
    //private Vector3 currentHit = Vector3.zero;

    public LayerMask DragMask;

    public float zeroLevel = -500f;

    private Ray r1;
    private RaycastHit hit1;

    void Start()
    {
        MCSUICenter.Satellite_HangMask = DragMask;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!camera.enabled) return;

            objPos = transform.position;
            rigidbody.isKinematic = true;
            // ≈сли под курсором есть сторонний объект, или мышь не попадает в квадрат камеры

            if (MCSUICenter.MouseOverObject() != null ||  !camera.pixelRect.Contains(Input.mousePosition)) {
                //Debug.Log("return " + MCSUICenter.MouseOverObject());
                return;
            }

            startHit = Vector3.one;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            //rigidbody.isKinematic = false;
            //rigidbody.velocity = (startHit - currentHit) * 2f;

            _currIntersectPosition = Vector3.zero;
            _lastIntersectPosition = Vector3.zero;

            _translationDelta = Vector3.zero;
            objMovement = Vector3.zero;
            startHit = Vector3.zero;
            _oldMouse = Vector3.zero;
            //_olddistance = 0;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            // ≈сли за пределами контейнера и еще не нажимали в нем мышь - выходим
            if (!camera.pixelRect.Contains(Input.mousePosition) && startHit == Vector3.zero) return;
            else if (startHit == Vector3.zero) return; // ≈сли внутри контейнера и мышь не нажата - выходим


            MoveCamera();
        }

        if (!rigidbody.isKinematic)
            rigidbody.velocity = Vector3.Slerp(rigidbody.velocity, Vector3.zero, 0.05f);

    }

    public float distance = 15f;


    private Plane plane;
    private float hitDistance;
    Vector3 _currIntersectPosition = Vector3.zero, _lastIntersectPosition = Vector3.zero, 
        _translationDelta = Vector3.zero, objMovement = Vector3.zero,
        objPos = Vector3.zero;

    private Ray ray;

    private Vector3 _oldMouse = Vector3.zero;

    //private float _olddistance = 0f;
    void MoveCamera()
    {
        if (Input.mousePosition == _oldMouse) return;

        ray = camera.ScreenPointToRay(Input.mousePosition);


        plane = new Plane(Vector3.up, Vector3.zero);
        hitDistance = 0;
        if (plane.Raycast(ray, out hitDistance))
        {
            _currIntersectPosition = ray.direction * (int)hitDistance;

            if (_lastIntersectPosition != Vector3.zero)
            {
                _translationDelta = _currIntersectPosition - _lastIntersectPosition;

                objMovement = Vector3.forward*_translationDelta.z;
                objMovement += Vector3.right*_translationDelta.x;

                objPos -= objMovement;


                transform.position = Vector3.Lerp(transform.position, objPos, 0.8f);
                //objPos;
            } //if						
        }//if					

        _lastIntersectPosition = _currIntersectPosition;
        _oldMouse = Input.mousePosition;
    }

    //private Vector3 targetPos = Vector3.zero;
}
