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

	public float minimumY = -60F;
	public float maximumY = 60F;
    private bool stateOne = false;
    public Vector3 lampPosition;

    float rotationX = 0f;
	float rotationY = 0F;

    private bool isCameraMoveEnabled;

    Quaternion originalRotation;

	void Update ()
	{
        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            isCameraMoveEnabled = !isCameraMoveEnabled;
        }

        if (!isCameraMoveEnabled) {
            return;
        }

       /* Debug.Log(">>>>>>!!!!!"+gameObject.camera.fieldOfView);
        if (gameObject.camera.fieldOfView >= 55) { 
            gameObject.camera.fieldOfView = 55; Debug.Log("TRUE"); }*/
        if (gameObject.camera.fieldOfView < 0) { gameObject.camera.fieldOfView = 0; }
        if (gameObject.camera.fieldOfView >= 55)
        {

            gameObject.camera.fieldOfView = 55;
            
          
        }
            gameObject.camera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 25;
        

        if (!Input.GetKey(KeyCode.Mouse1)) return;

        if (axes == RotationAxes.MouseXAndY)
        {
            // Read the mouse input axis
                rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);

            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}

	    lampPosition =
	        Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_VizorPanel").GetControl(ControlType.Indicator,
                                                                                                "INDICATOR_U_115").transform.position;
        if (Vector3.Angle(transform.forward, lampPosition - transform.position)<=20f){
            if (!stateOne)
            {
                Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_VizorPanel").GetControl(
                    ControlType.Tumbler, "TUMBLER_LOSE").GetComponent<SwitcherToolkit>().ControlChanged();
                stateOne = true;
            }
        }
        else
        {
            stateOne = false;
        }
	}
	
	void Start ()
    {
        originalRotation = transform.localRotation;

		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
	}
}