
using UnityEngine;
using System.Collections;
using System.Collections.Generic; //1

public class TankTrackController : MonoBehaviour
{

    public GameObject wheelCollider; //2

    public float wheelRadius = 0.15f; //3
    public float suspensionOffset = 0.05f; //4

    public float trackTextureSpeed = 2.5f; //5

    public GameObject leftTrack;  //6
    public Transform[] leftTrackUpperWheels; //7
    public Transform[] leftTrackWheels; //8
    //public Transform[] leftTrackBones; //9

    public GameObject rightTrack; //6
    public Transform[] rightTrackUpperWheels; //7
    public Transform[] rightTrackWheels; //8
    // public Transform[] rightTrackBones; //9

    public Transform COM;

    
    bool shaking;

    public float maxRpm = 45f;

    #region Äâèæåíèå

    public float forwardTorque = 500.0f; //1
    public float rotateOnMoveBrakeTorque = 400.0f; //2 
    public float minBrakeTorque = 0.0f; //3 
    public float minOnStayStiffness = 0.06f; //4 
    public float minOnMoveStiffness = 0.05f;  //5 
    public float rotateOnMoveMultiply = 2.0f; //6

    #endregion

    public class WheelData
    { //10
        public Transform wheelTransform; //11
        //public Transform boneTransform; //12
        public WheelCollider col; //13
        public Vector3 wheelStartPos; //14
        //public Vector3 boneStartPos; //15
        public float rotation = 0.0f; //16
        public Quaternion startWheelAngle;	//17
    }

    protected WheelData[] leftTrackWheelData; //18
    protected WheelData[] rightTrackWheelData; //18

    protected float leftTrackTextureOffset = 0.0f; //19
    protected float rightTrackTextureOffset = 0.0f; //19


    public float accelerate = 0;
    public float steer = 0;


    private Vector3 lastPosition;

    void Start()
    {
        rigidbody.centerOfMass = COM.localPosition;
    }

    void Awake()
    {
        rigidbody.centerOfMass = COM.transform.position;

        leftTrackWheelData = new WheelData[leftTrackWheels.Length]; //1 
        rightTrackWheelData = new WheelData[rightTrackWheels.Length]; //1 

        for (int i = 0; i < leftTrackWheels.Length; i++)
        {
            leftTrackWheelData[i] = SetupWheels(leftTrackWheels[i]);  //2             
        }

        for (int i = 0; i < rightTrackWheels.Length; i++)
        {
            rightTrackWheelData[i] = SetupWheels(rightTrackWheels[i]); //2  
        }

        Vector3 offset = transform.position; //3 
        offset.z += 0.01f;  //3 
        transform.position = offset; //3		 
    }

    WheelData SetupWheels(Transform wheel)
    {  //2 
        WheelData result = new WheelData();

        GameObject go = new GameObject("Collider_" + wheel.name); //4
        go.transform.parent = transform; //5 	
        go.transform.position = wheel.position; //6
        go.transform.localRotation = Quaternion.Euler(0, wheel.localRotation.y, 0); //7 

        WheelCollider col = (WheelCollider)go.AddComponent(typeof(WheelCollider));//8 
        WheelCollider colPref = this.wheelCollider.GetComponent<WheelCollider>();//9 

        col.mass = colPref.mass;//10
        col.center = colPref.center;//10
        col.radius = colPref.radius;//10
        col.suspensionDistance = colPref.suspensionDistance;//10
        col.suspensionSpring = colPref.suspensionSpring;//10
        col.forwardFriction = colPref.forwardFriction;//10
        col.sidewaysFriction = colPref.sidewaysFriction;//10 
        col.center = Vector3.zero;

        result.wheelTransform = wheel; //11
        //result.boneTransform = bone; //11
        result.col = col; //11
        result.wheelStartPos = wheel.transform.localPosition; //11
        //result.boneStartPos = bone.transform.localPosition; //11
        result.startWheelAngle = wheel.transform.localRotation; //11



        return result; //12
    }

    private float MaxSpeed = 64;
    private float prevMultiply = 1;
    void FixedUpdate()
    {
        //accelerate = Input.GetAxis("Vertical");  //4
        //steer = Input.GetAxis("Horizontal"); //4

        //Debug.Log(accelerate + "|" + steer);
            //rigidbody.AddRelativeTorque(transform.up * steer * forwardTorque * rotateOnMoveMultiply * rigidbody.mass/10);

        if ((accelerate == -1 && rigidbody.velocity.z > -0.3) || (accelerate == 1 && rigidbody.velocity.z < -0.3))
        {
            Debug.Log("I am in IF ");
            rigidbody.velocity = new Vector3(0, 0, 0);
            return;
        }

        float speed = transform.InverseTransformDirection(rigidbody.velocity).z * 3.6f;
        float speedMultiply = 1f - speed/MaxSpeed;

//        Debug.Log("Rigidbody.velocity = " + rigidbody.velocity);

//        Debug.Log("Speed Multiply = " + speedMultiply);
//        Debug.Log("Speed = " + speed);



        if (accelerate == -1 && speedMultiply >= 1)
        {
            accelerate = 0;
            Debug.Log("Should go back!");
        }
        else if (accelerate == 1 && speedMultiply < 1)
        {
            accelerate = 0;
            Debug.Log("Should go front!");
        }

        

        if (!Mathf.Approximately(accelerate, 0))
            transform.RotateAroundLocal(COM.up, steer * Time.fixedDeltaTime * rotateOnMoveMultiply * minOnMoveStiffness * speedMultiply);
        else
            transform.RotateAroundLocal(COM.up, steer*Time.fixedDeltaTime*rotateOnMoveMultiply*speedMultiply);

//        Debug.Log("Before Accelerate = " + accelerate + "; steer = " + steer);

        if (speed >= MaxSpeed) accelerate = 0;

//        Debug.Log("After Accelerate = " + accelerate + "; steer = " + steer);
        UpdateWheels(accelerate, steer); //5   
        ////UpdateWheels(accelerate, steer); //5  


        prevMultiply = speedMultiply;
        if (networkView.isMine)
        {
            //if (Input.GetKey(KeyCode.W)) accelerate = 1;
            //if (Input.GetKey(KeyCode.S)) accelerate = -1;
            //if (Input.GetKey(KeyCode.A)) steer = -1;
            //if (Input.GetKey(KeyCode.D)) steer = 1;
            //accelerate = Input.GetAxis("Vertical");  //4
            //steer = Input.GetAxis("Horizontal"); //4

            //UpdateWheels(accelerate, steer); //5      
        }
    }

    private float CalculateSmoothRpm(WheelData[] w)
    { //12 
        float rpm = 0.0f;

        List<int> grWheelsInd = new List<int>(); //13 

        for (int i = 0; i < w.Length; i++)
        { //14 
            if (w[i].col.isGrounded)
            {  //14 
                grWheelsInd.Add(i); //14 
            }
        }

        if (grWheelsInd.Count == 0)
        {  //15   
            foreach (WheelData wd in w)
            {  //15 
                rpm += wd.col.rpm;  //15				 
            }

            rpm /= w.Length; //15 

        }
        else
        {  //16 

            for (int i = 0; i < grWheelsInd.Count; i++)
            {  //16 
                rpm += w[grWheelsInd[i]].col.rpm; //16	 
            }

            rpm /= grWheelsInd.Count; //16 
        }

        if (rpm > maxRpm) rpm = maxRpm;
        return rpm; //17 
    }

    private Vector3 CalculateWheelPosition(Transform w, WheelCollider col, Vector3 startPos)
    {  //18 
        WheelHit hit;

        Vector3 lp = w.localPosition;
        if (col.GetGroundHit(out hit))
        {
            lp.y -= Vector3.Dot(w.position - hit.point, transform.up) - wheelRadius;

        }
        else
        {
            lp.y = startPos.y - suspensionOffset;

        }

        return lp;
    }


    /////////////////////////-------------- ÂÐÀÙÅÍÈÅ ÍÀ ÌÅÑÒÅ -----------------//////////////////////////
    #region Âðàùåíèå íà ìåñòå
    public float rotateOnStandTorque = 1500.0f; //1
    public float rotateOnStandBrakeTorque = 500.0f; //2
    public float maxBrakeTorque = 1000.0f; //3


    float RPM;
    public void UpdateWheels(float accel, float steer)
    { //5
        float delta = Time.fixedDeltaTime;

        float trackRpm = CalculateSmoothRpm(leftTrackWheelData);
        RPM = trackRpm;

        foreach (WheelData w in leftTrackWheelData)
        {
            w.wheelTransform.localPosition = CalculateWheelPosition(w.wheelTransform, w.col, w.wheelStartPos);

            w.rotation = Mathf.Repeat(w.rotation + delta * trackRpm * 360.0f / 60.0f, 360.0f);
            w.wheelTransform.localRotation = Quaternion.Euler(w.rotation, w.startWheelAngle.y, w.startWheelAngle.z);

            CalculateMotorForce(w.col, accel, steer);  //6 
        }


        leftTrackTextureOffset = Mathf.Repeat(leftTrackTextureOffset - delta * trackRpm * trackTextureSpeed / 60.0f, 1.0f);
        leftTrack.renderer.material.SetTextureOffset("_MainTex", new Vector2(0, -leftTrackTextureOffset));

        trackRpm = CalculateSmoothRpm(rightTrackWheelData);
        RPM = (RPM + trackRpm) * 0.5f;

        foreach (WheelData w in rightTrackWheelData)
        {
            w.wheelTransform.localPosition = CalculateWheelPosition(w.wheelTransform, w.col, w.wheelStartPos);

            w.rotation = Mathf.Repeat(w.rotation + delta * trackRpm * 360.0f / 60.0f, 360.0f);
            w.wheelTransform.localRotation = Quaternion.Euler(w.rotation, w.startWheelAngle.y, w.startWheelAngle.z);

            CalculateMotorForce(w.col, accel, -steer); //6 
        }

        rightTrackTextureOffset = Mathf.Repeat(rightTrackTextureOffset - delta * trackRpm * trackTextureSpeed / 60.0f, 1.0f);
        rightTrack.renderer.material.SetTextureOffset("_MainTex", new Vector2(0, -rightTrackTextureOffset));

        //for (int i = 0; i < leftTrackUpperWheels.Length; i++)
        //{
        //    leftTrackUpperWheels[i].localRotation = Quaternion.Euler(leftTrackWheelData[0].rotation, leftTrackWheelData[0].startWheelAngle.y, leftTrackWheelData[0].startWheelAngle.z);
        //}

        //for (int i = 0; i < rightTrackUpperWheels.Length; i++)
        //{
        //    rightTrackUpperWheels[i].localRotation = Quaternion.Euler(rightTrackWheelData[0].rotation, rightTrackWheelData[0].startWheelAngle.y, rightTrackWheelData[0].startWheelAngle.z);
        //}

        //StartCoroutine(ShakeCamera(RPM));
    }

    public void CalculateMotorForce(WheelCollider col, float accel, float steer)
    {
        WheelFrictionCurve fc = col.sidewaysFriction;  //7 

        if (accel == 0 && steer == 0)
        {
            col.brakeTorque = maxBrakeTorque;
        }
        else if (accel == 0.0f)
        {
            col.brakeTorque = rotateOnStandBrakeTorque;
            col.motorTorque = steer * rotateOnStandTorque;
            //fc.stiffness = 1.0f + minOnStayStiffness - Mathf.Abs(steer);

        }
        else
        { //8         

            col.brakeTorque = minBrakeTorque;  //9 
            col.motorTorque = accel * forwardTorque;  //10 

            //rigidbody.AddRelativeTorque(transform.right * forwardTorque * rotateOnMoveMultiply * steer);
            if (steer < 0)
            { //11 


                //transform.RotateAroundLocal(transform.up, steer);
                //col.brakeTorque = rotateOnMoveBrakeTorque; //12 
                //col.motorTorque = steer * forwardTorque * rotateOnMoveMultiply;//13 
                //fc.stiffness = 1.0f + minOnMoveStiffness - Mathf.Abs(steer);  //14 
            }

            if (steer > 0)
            { //15 
                //transform.RotateAroundLocal(transform.up, steer);
                //rigidbody.AddRelativeTorque(-transform.right * forwardTorque * rotateOnMoveMultiply);

                //col.brakeTorque = rotateOnMoveBrakeTorque; //12 
                //col.motorTorque = steer * forwardTorque * rotateOnMoveMultiply;//16 
                //fc.stiffness = 1.0f + minOnMoveStiffness - Mathf.Abs(steer); //17
            } 
         
        }

        if (fc.stiffness > 1.0f) fc.stiffness = 1.0f; //18		 
        col.sidewaysFriction = fc; //19

        if (col.rpm > 0 && accel < 0)
        { //20 
            col.brakeTorque = maxBrakeTorque;  //21
        }
        else if (col.rpm < 0 && accel > 0)
        { //22 
            col.brakeTorque = maxBrakeTorque; //23
        }

    }

    //float k;
    //float shakeForce;
    //IEnumerator ShakeCamera(float rpm)
    //{
    //    if (!shaking)
    //    {
    //        shaking = true;
    //        k = rpm / maxRpm;
    //        shakeForce = k * 0.01f;

    //        if (Camera.main != null)
    //            //iTween.PunchPosition(Camera.main.gameObject, new Vector3(shakeForce * (float)rnd.Next(-1, 1), shakeForce * (float)rnd.Next(-1, 1), shakeForce * (float)rnd.Next(-1, 1)), 0.1f);
    //            yield return new WaitForSeconds(0.11f);
    //        shaking = false;
    //    }
    //}

    #endregion

    void OnGUI()
    {
        // BLEAT BITCHES FUCKING FUCK OMG GUICLIPS ROFLMAO!!!

        //        windowstringcount = 0;
        //        windowRect = GUI.Window(0, windowRect, DoMyWindow, "Info Window");
        //		
        //		GUILayout.BeginArea(new Rect(200,200, 300,200));
        //		
        //		if (GUILayout.Button("Switch Roles"))
        //            {				
        //			 SwitchRoles();
        //            }
    }


}
