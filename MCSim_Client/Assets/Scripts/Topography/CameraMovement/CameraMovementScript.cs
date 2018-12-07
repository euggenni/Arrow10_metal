using UnityEngine;
using System.Collections;

public class CameraMovementScript : MonoBehaviour {

    private Vector3 moveVectorVertical;
    private Vector3 moveVectorHorizontal;
    private Vector3 curMousePosition;
    private Vector3 lastMousePosition;
    public float Speed = 0.001f;
    public GameObject plane;
    public float ZoomSpeed = 5f;

    private bool onPlane = true;
    private float planeSizeX;
    private float planeSizeZ;
    private Vector3 planePos;
    private Vector3 selfPos;
    private float CameraZoom;
    private float fieldOfView;
	// Use this for initialization
	void Start () {


        Reset();
	}

  
	
	void Update () {



        

        moveVectorVertical.z = Input.GetAxis("Mouse ScrollWheel");
       
        transform.Translate(moveVectorVertical * ZoomSpeed);
        
        selfPos = transform.position;
        Speed = selfPos.y * 0.9f;

      /* if (selfPos.y < planePos.y + 3f)
        {
            transform.position = new Vector3(selfPos.x, planePos.y+3f, selfPos.z);
        }

        if (selfPos.y > planePos.y + CameraZoom)
        {
            transform.position = new Vector3(selfPos.x, planePos.y + CameraZoom, selfPos.z);
        }
        */
        transform.position = new Vector3(selfPos.x, Mathf.Clamp(selfPos.y, planePos.y + 3f, planePos.y + CameraZoom) , selfPos.z);



        if (Input.GetMouseButtonDown(2))
        {
            lastMousePosition = Input.mousePosition;
        }

        if(Input.GetMouseButton(2))
        {
            moveVectorHorizontal = (lastMousePosition - Input.mousePosition).normalized * Speed;
            transform.Translate(moveVectorHorizontal * Time.deltaTime);
            lastMousePosition = Input.mousePosition;
            
        }

        if (selfPos.x > Mathf.Abs(planePos.x + planeSizeX / 2))
        {
            onSideGone("Right");
        }
        if (-selfPos.x > Mathf.Abs(planePos.x + planeSizeX / 2))
        {
            onSideGone("Left");
        }
        if (selfPos.z > Mathf.Abs(planePos.x + planeSizeZ / 2))
        {
            onSideGone("Up");
        }
        if (-selfPos.z > Mathf.Abs(planePos.x + planeSizeZ / 2))
        {
            onSideGone("Bottom");
        }
	}



    void onSideGone(string side)
    {
        switch (side) 
        {
            case "Right": transform.position = new Vector3(planeSizeX / 2, selfPos.y, selfPos.z);
                break;
            case "Left": transform.position = new Vector3(-planeSizeX / 2, selfPos.y, selfPos.z);
                break;
            case "Up": transform.position = new Vector3(selfPos.x, selfPos.y, planeSizeZ / 2);
                break;
            case "Bottom": transform.position = new Vector3(selfPos.x, selfPos.y, -planeSizeZ / 2);
                break;
        }

        
    }


    public void Reset()
    {
        planeSizeX = plane.renderer.bounds.size.x;
        planeSizeZ = plane.renderer.bounds.size.z;
        planePos = plane.transform.position;
        fieldOfView = this.GetComponent<Camera>().fieldOfView / 2;
        float b = Mathf.Max(planeSizeX, planeSizeZ) / 2;
        CameraZoom = b / Mathf.Tan(fieldOfView);
        transform.position = new Vector3(planePos.x, CameraZoom, planePos.z);
        ZoomSpeed = Mathf.Max(planeSizeZ, planeSizeX) / 5;
    }
}


