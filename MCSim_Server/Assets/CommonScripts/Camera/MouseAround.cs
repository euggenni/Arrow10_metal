using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Around")]
public class MouseAround : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -20F;
	public float maximumY = 80;

    public float rotationX = 0f;
    public float rotationY = 0F;

    public float MoveSpeed = 15f;
    public float ShiftMultiply = 2f;

    public Quaternion originalRotation;

    void OnEnable()
    {
        //originalRotation = transform.rotation;
        rotationX = transform.eulerAngles.y;
        rotationY = transform.eulerAngles.x;
    }

    private bool dragging;

	void Update ()
	{
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            dragging = true;

            GameObject go;
            if (go = MCSUICenter.MouseOverObject())
            {
                if (go.layer == LayerMask.NameToLayer("GUI"))
                {
                    dragging = false;
                }
            }

            if (MCSUICenter.SatelliteCamera.enabled && MCSUICenter.SatelliteCamera.pixelRect.Contains(Input.mousePosition))
            {
                dragging = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            dragging = false;
        }

        if (!dragging) return;

        if (axes == RotationAxes.MouseXAndY)
        {
                rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                rotationY -= Input.GetAxis("Mouse Y") * sensitivityY;
                //rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                rotationY = ClampAngle(rotationY, minimumY, maximumY);


                //Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                //Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);

                //transform.localRotation = originalRotation * xQuaternion * yQuaternion;
                transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
        }
        //else if (axes == RotationAxes.MouseX)
        //{
        //    transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        //}
        //else
        //{
        //    rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        //    rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
        //    transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        //}
	}


    void FixedUpdate()
    {
        float shift = MoveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
            shift *= ShiftMultiply;

        if (Input.GetKey(KeyCode.W)) transform.Translate(transform.forward * shift * Time.fixedDeltaTime, Space.World);
        if (Input.GetKey(KeyCode.A)) transform.Translate(-transform.right * shift * Time.fixedDeltaTime, Space.World);
        if (Input.GetKey(KeyCode.S)) transform.Translate(-transform.forward * shift * Time.fixedDeltaTime, Space.World);
        if (Input.GetKey(KeyCode.D)) transform.Translate(transform.right * shift * Time.fixedDeltaTime, Space.World);
    }
	
	void Start ()
    {
        originalRotation = transform.rotation;
        //// Make the rigid body not change rotation
        //if (rigidbody)
        //    rigidbody.freezeRotation = true;
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
}