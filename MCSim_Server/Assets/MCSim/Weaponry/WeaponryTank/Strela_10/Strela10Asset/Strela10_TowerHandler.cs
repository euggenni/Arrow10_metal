using System;
using System.Text;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public class Strela10_TowerHandler : MonoBehaviour
{
    public Strela10_Operator_CoreHandler Handler;

    private WeaponryTank_Strela10 Strela10;
    private GameObject Tower, Containers;
    private CoreLibrary.Core Core;

    public GameObject[] TowerDetails;
    public GameObject RotationPivot;

    public WorkMode currentMode = WorkMode.Combat;

    private PanelControl controlPosition, indicatorStowed;

    // ���������� ����� �� ��������� ������ � ����� ������
    private bool stowed = false;

    public enum WorkMode
    {
        Marching = 0,
        Combat = 1
    }

    void Start()
    {
        if (Handler) {
            Core = Handler.Core;
            Strela10 = Handler.Strela10;

            //************//

            //Strela10 = gameObject.GetComponentInParents<WeaponryTank_Strela10>(false);
            //string roleName = new Strela10_Operator_PanelLibrary().GetName();

            //Strela10.Core[roleName].Virtualize();

            //CoreToolkit ct = Strela10.Core[roleName] as CoreToolkit;
            //ct.Handler.Weaponry = Strela10; // ����������� ���������� ������� ����
            //ct.Handler.Core = ct;

            //Core = Handler.Core;

            //************//

            Tower = Handler.Strela10.Tower;
            Containers = Handler.Strela10.Containers;

            Handler.ControlChangeCallEvent += OnControlChanged;

            _operatorJoystickHorizontal = Core.GetPanel("Strela10_GuidancePanel").GetControl(ControlType.Joystick, "OperatorJoystickHorizontal");
            _operatorJoystickVertical = Core.GetPanel("Strela10_GuidancePanel").GetControl(ControlType.Joystick, "OperatorJoystickVertical");
            controlPosition = Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_POSITION");
        }
        else {
            Debug.LogError("�� ���������� Strela10_Operator_CoreHandler");
        }

        //SwitchWorkMode(WorkMode.Marching, false);
    }

    void OnControlChanged(PanelControl control)
    {
        //[1]: ������� [TUMBLER_POSITION] �� ������ [Strela10_SupportPanel] ������ ��������� [BATTLE]
        if(controlPosition == null) {
            controlPosition = Core.GetPanel("Strela10_SupportPanel").GetControl(ControlType.Tumbler, "TUMBLER_POSITION");
        }
    }
    
	// Update is called once per frame
	void FixedUpdate () {
        try
        {
            if (Handler.Core.isVirtual)
            {
                if (!stowed) {
                    UpdateTower();
                }
            }
        }
        catch
        {
        }
	}

    private PanelControl _tumblerPosition, _operatorJoystickHorizontal, _operatorJoystickVertical;

    /// <summary>
    /// �������� ��������� �����
    /// </summary>
    void UpdateTower()
    {
        float verticalMax;

        int[] joystickHorizontal = (int[])_operatorJoystickHorizontal.State;
        float x = joystickHorizontal[1];

        int[] joystickVertical = (int[])_operatorJoystickVertical.State;
        float y = joystickVertical[0];

        if (!Mathf.Approximately(x, 0)) x = x / 35f;
        if (!Mathf.Approximately(y, 0)) y = y / 35f;

        Tower.transform.localEulerAngles = new Vector3(0f, Tower.transform.localEulerAngles.y + Strela10.TowerRotationSpeed.X * Time.fixedDeltaTime * x, 0f);

        // ��������� ������ ����� � ��������
        float clamp = ClampTowerAngle(Strela10.Containers.transform.localEulerAngles.x - Strela10.TowerRotationSpeed.Y * Time.fixedDeltaTime * y);
        
        // ���� ������ ����� �� 83, � ����� ������������� �� ������, �� ����� ����� ����� �� �����������

        if (controlPosition.State.ToString() == "BATTLE") { // ���� ������� � ������ ���������
        
            verticalMax = Strela10.TowerLimitAngle.Y.Max;
            
        }
        else {
            verticalMax = 83;
        }
        
        // ������� �����������
        if (clamp >= Strela10.TowerLimitAngle.Y.Min && clamp <= verticalMax)
            Strela10.Containers.transform.localEulerAngles =
                new Vector3(
                    Strela10.Containers.transform.localEulerAngles.x -
                    Strela10.TowerRotationSpeed.Y * Time.deltaTime * y, 0f, 0f);
    }

    /// <summary>
    /// �������� ���� �������� ����������� � ���� �� -180 �� 180
    /// </summary>
    float ClampTowerAngle(float angle) {
        if (angle > 0 && angle < 180)
            return -angle;

        if (angle > 180 && angle < 360)
            return 360 - angle;

        if (angle >= 360)
            return angle - 360;

        return angle;
    }

    
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (currentMode == WorkMode.Marching)
        //        SwitchWorkMode(WorkMode.Combat, false);
        //    else SwitchWorkMode(WorkMode.Marching, false);
        //}
    }

    /// <summary>
    /// ������� ����� ����� ��������.       
    /// </summary>
    /// <param name="mode">�����</param>
    /// <param name="showAnimation">���������� �� �������</param>
    public void SwitchWorkMode(WorkMode mode, bool showAnimation) {
        if (towerEvolution) return; // ���� ��� �������� ���������
        if (mode == currentMode) return; // ���� ������� ����� � ���� ���������

        Model.UCoroutine(this, switchWorkMode(mode, showAnimation), "switchWorkMode");
    }

    private bool towerEvolution = false;
    /// <summary>
    /// �������� ������������ ��������� �����
    /// </summary>
    /// <param name="mode">����� ��������/������</param>
    /// <param name="immediately">��� ��������</param>
    /// <returns></returns>
    private IEnumerator switchWorkMode(WorkMode mode, bool showAnimation)
    {
        float rotationSpeed = Time.fixedDeltaTime * 15f;

        switch (mode)
        {
            case WorkMode.Marching:
                towerEvolution = true; // � ������ ������ ���������� ��������� ��������� �����
                stowed = true; // ������ ����������� ������� �����

                Tower.transform.localEulerAngles = Vector3.zero;

                // ����������� ����������
                Containers.transform.localEulerAngles = new Vector3(-83f, 0, 0);

                while (true)
                {
                    foreach (GameObject towerDetail in TowerDetails) {
                        towerDetail.transform.RotateAround(RotationPivot.transform.position,
                                                           RotationPivot.transform.right, -rotationSpeed);
                    }

                    if (Containers.transform.localRotation.eulerAngles.x < rotationSpeed)
                    {
                        currentMode = WorkMode.Marching;
                        towerEvolution = false; // �������� ����� ���������
                        break;
                    }
                    if (showAnimation) // ���� ���������� ��������
                    yield return new WaitForFixedUpdate();
                }
                break;

            case WorkMode.Combat:
                towerEvolution = true; // � ������ ������ ���������� ��������� ��������� �����
                GameObject pivot = RotationPivot; // ����� ��������
                bool isVertical = false;
                while (true)
                {
                    foreach (GameObject towerDetail in TowerDetails)
                    {
                        if(towerDetail != Containers) // ���� �� ����������, �� ������� ������ �� ������������� ���������
                        {
                            if(Vector3.Angle(towerDetail.transform.forward, Tower.transform.up) < 1f) // ��������� ��������� ����������� ��� �����������
                            {
                                isVertical = true;
                                continue;
                            }
                        }

                        if(isVertical)
                        {
                            pivot = Containers; // ���� ����� �� ������������� ��������, �� ������������ ������ ��� �����������
                        }

                        towerDetail.transform.RotateAround(pivot.transform.position,
                                                           pivot.transform.right, rotationSpeed);
                    }

                    if(Vector3.Angle(Containers.transform.forward, Tower.transform.forward) < 25f)
                    {
                        currentMode = WorkMode.Combat;
                        towerEvolution = false; // �������� ����� ���������
                        stowed = false; // ������ ��������� ������� �����
                        break;
                    }

                    if (showAnimation) // ���� ���������� ��������
                    yield return new WaitForFixedUpdate();
                }
                break;
        }

    }

    public IEnumerator ACUBattle(GameObject target)
    {
        //StopCoroutine("AutoAimTarget"); // TODO: 

        Debug.Log(target.name);
           //����������� ���� ����� 
       

       
        //print(_target.name + ": " + x);
        //CallTargetAngleChanged(x < 0 ? 360 + x : x);

        while (true)
        {
            var vector = Strela10.transform.InverseTransformDirection(target.transform.position - Strela10.transform.position);
            vector.y = 0;
            float sign = Mathf.Sign(vector.x);
            //vector = Tower.transform.TransformDirection(vector);
                                   
            float x = Vector3.Angle(Strela10.transform.forward, vector) * sign;

            Vector3 hz = Vector3.zero;
            //Tower.transform.forward = Vector3.Lerp(Tower.transform.forward, vector, Strela10.TowerRotationSpeed.X * Time.deltaTime);
            Tower.transform.localEulerAngles = Vector3.Lerp(Tower.transform.localEulerAngles, new Vector3(0, x, 0), Strela10.TowerRotationSpeed.X * 0.01F);


            x = x < 0 ? 360 + x : x;
           // Debug.Log(x);

            Debug.DrawLine(Tower.transform.position, Tower.transform.position + Tower.transform.forward * 100f, Color.blue);
            Debug.DrawLine(Tower.transform.position, Tower.transform.position + vector, Color.red);

            //Tower.transform.localEulerAngles = new Vector3(0,x,0);
            yield return new WaitForEndOfFrame();
        }

        //  while (true)
        //{
        //    var vector = Tower.transform.InverseTransformDirection(target.transform.position - Tower.transform.position);
        //    vector.y = 0;
        //    float sign = Mathf.Sign(vector.x);
        //    //vector = Tower.transform.TransformDirection(vector);
            
                       
        //    float x = Vector3.Angle(Tower.transform.forward, vector) * sign;

        //    Vector3 hz = Vector3.zero;
        //    //Tower.transform.forward = Vector3.Lerp(Tower.transform.forward, vector, Strela10.TowerRotationSpeed.X * Time.deltaTime);
        //    Tower.transform.forward = Vector3.Lerp(Tower.transform.forward, vector, Strela10.TowerRotationSpeed.X * Time.deltaTime);


        //    x = x < 0 ? 360 + x : x;
        //    Debug.Log(x);

        //    Debug.DrawLine(Tower.transform.position, Tower.transform.position + Tower.transform.forward * 100f, Color.blue);
        //    Debug.DrawLine(Tower.transform.position, Tower.transform.position + vector, Color.red);

        //    //Tower.transform.localEulerAngles = new Vector3(0,x,0);
        //    yield return new WaitForEndOfFrame();
        //}

/*
        Vector3 point; // ����� �������� (� �����������)
        Vector3 startShift; // ����������� �������� ����� �������� ������������ �������-����

        float angleTower, angleContainers;
        float clamp;

        Plane plane = new Plane(); // ��������� ��� ��������

        Ray ray;

        // ���������� ��������� �������� � ����������� �����
        Vector3 localDirection;

        float startDistanceToTarget, distanceK = 1f; // ��������� ��������� �� ���� � ����������� ���������

        //// ����������� ��������� � ����������� ���������� ////

        // ������ ������� � ��������� ���������
        plane.SetNormalAndPosition(-Containers.transform.forward, target.transform.position);
        // ��������� ���������� �� ����, ��� �������� ������������ ���������
        startDistanceToTarget = Vector3.Distance(target.transform.position, Containers.transform.position);

        //Debug.DrawLine(Containers.transform.position, target.transform.position, Color.red);

        // ����������� �������� ����� ������� �� ����
        ray = new Ray(Containers.transform.position, Containers.transform.forward);
        float startDistanceToCrosshair;
        plane.Raycast(ray, out startDistanceToCrosshair); // ������ ������� �� ����� ������������
        startShift = (startDistanceToCrosshair * Containers.transform.forward.normalized + ray.GetPoint(startDistanceToCrosshair)) - target.transform.position;

        //Debug.DrawLine(Containers.transform.position,
        //               startDistanceToCrosshair*Containers.transform.forward.normalized +
        //               Containers.transform.position, Color.white);
        //Debug.DrawLine(target.transform.position, target.transform.position + startShift, Color.green);

        Vector3 localShift = Containers.transform.InverseTransformDirection(startShift);
        //Debug.LogWarning("SHIFT LOC "+localShift);
        // ��������� �������� � ������� �����������
        // x: 100 y: -100
        while (true)
        {
            // ����������� ���������
            distanceK = Vector3.Distance(target.transform.position, Containers.transform.position) / startDistanceToTarget;

            // ������� � ������� ���������
            plane.SetNormalAndPosition(-Containers.transform.forward, target.transform.position);

            point = Containers.transform.InverseTransformPoint(target.transform.position) + localShift * distanceK; // � ������� ����������� ������ �������� ������������ ����
            point = Containers.transform.TransformPoint(point); // �������� � ���������� ����������

            //Debug.DrawLine(Containers.transform.position, point, Color.yellow);

            #region ������� �����
            // ������ �� �������� � ��������� �����������
            localDirection = Tower.transform.InverseTransformDirection(point - Tower.transform.position);
            localDirection.y = 0;

            // ���� ����� �������� ������ ����� � �����
            angleTower = Vector3.Angle(Tower.transform.forward,
                                       Tower.transform.TransformDirection(localDirection));

            // ������� ����� - ���������� ����� ���� ��� ������ (Mathf.Sign), � �������� �������� �������� 
            // ����� 0 � ������������ ��������� ����� �� ����
            Tower.transform.localEulerAngles = new Vector3(0,
                                                           Tower.transform.localEulerAngles.y +
                                                           Mathf.Clamp(angleTower, 0,
                                                                       Strela10.TowerRotationSpeed.X * Time.fixedDeltaTime) *
                                                           Math.Sign(localDirection.x));
            #endregion

            #region ������� ����������

            localDirection =
                Containers.transform.InverseTransformDirection(point - Containers.transform.position);
            localDirection = new Vector3(0, localDirection.y, localDirection.z);

            //Debug.DrawRay(Containers.transform.position, Containers.transform.TransformDirection(localDirection),
            //              Color.yellow);

            angleContainers = Vector3.Angle(Containers.transform.forward,
                                            Containers.transform.TransformDirection(localDirection));


            clamp = ClampTowerAngle(Containers.transform.localEulerAngles.x + Mathf.Clamp(angleContainers, 0, Strela10.TowerRotationSpeed.Y * Time.fixedDeltaTime) *
                        -Mathf.Sign(localDirection.y));

            if (clamp >= Strela10.TowerLimitAngle.Y.Min && clamp <= Strela10.TowerLimitAngle.Y.Max)
            {
                Containers.transform.localEulerAngles = Vector3.Lerp(Containers.transform.localEulerAngles,
                                                                     new Vector3(
                                                                         Containers.transform.localEulerAngles.x +
                                                                         Mathf.Clamp(angleContainers, 0,
                                                                                     Strela10.TowerRotationSpeed.Y *
                                                                                     Time.fixedDeltaTime) *
                                                                         -Mathf.Sign(localDirection.y), 0, 0),
                                                                     0.25f);
            }

            #endregion
        
            yield return new WaitForFixedUpdate();
        }*/
    }

    /// <summary>
    /// �������������� �������� ����� �� �����
    /// </summary>
    /// <param name="target">����</param>
    /// <returns></returns>
    public IEnumerator AutoAimTarget(GameObject target)
    {
        //StopCoroutine("AutoAimTarget"); // TODO: 

        Vector3 point; // ����� �������� (� �����������)
        Vector3 startShift; // ����������� �������� ����� �������� ������������ �������-����

        float angleTower, angleContainers;
        float clamp;

        Plane plane = new Plane(); // ��������� ��� ��������

        Ray ray;

        // ���������� ��������� �������� � ����������� �����
        Vector3 localDirection;

        float startDistanceToTarget, distanceK = 1f; // ��������� ��������� �� ���� � ����������� ���������

        //// ����������� ��������� � ����������� ���������� ////

        // ������ ������� � ��������� ���������
        plane.SetNormalAndPosition(-Containers.transform.forward, target.transform.position);
        // ��������� ���������� �� ����, ��� �������� ������������ ���������
        startDistanceToTarget = Vector3.Distance(target.transform.position, Containers.transform.position);

        //Debug.DrawLine(Containers.transform.position, target.transform.position, Color.red);

        // ����������� �������� ����� ������� �� ����
        ray = new Ray(Containers.transform.position, Containers.transform.forward);
        float startDistanceToCrosshair;
        plane.Raycast(ray, out startDistanceToCrosshair); // ������ ������� �� ����� ������������
        startShift = (startDistanceToCrosshair * Containers.transform.forward.normalized + ray.GetPoint(startDistanceToCrosshair)) - target.transform.position;

        //Debug.DrawLine(Containers.transform.position,
        //               startDistanceToCrosshair*Containers.transform.forward.normalized +
        //               Containers.transform.position, Color.white);
        //Debug.DrawLine(target.transform.position, target.transform.position + startShift, Color.green);

        Vector3 localShift = Containers.transform.InverseTransformDirection(startShift);
        //Debug.LogWarning("SHIFT LOC "+localShift);
        // ��������� �������� � ������� �����������
        // x: 100 y: -100
        while (true)
        {
            // ����������� ���������
            distanceK = Vector3.Distance(target.transform.position, Containers.transform.position) / startDistanceToTarget;

            // ������� � ������� ���������
            plane.SetNormalAndPosition(-Containers.transform.forward, target.transform.position);

            point = Containers.transform.InverseTransformPoint(target.transform.position) + localShift * distanceK; // � ������� ����������� ������ �������� ������������ ����
            point = Containers.transform.TransformPoint(point); // �������� � ���������� ����������

            //Debug.DrawLine(Containers.transform.position, point, Color.yellow);

            #region ������� �����
            // ������ �� �������� � ��������� �����������
            localDirection = Tower.transform.InverseTransformDirection(point - Tower.transform.position);
            localDirection.y = 0;

            // ���� ����� �������� ������ ����� � �����
            angleTower = Vector3.Angle(Tower.transform.forward,
                                       Tower.transform.TransformDirection(localDirection));

            // ������� ����� - ���������� ����� ���� ��� ������ (Mathf.Sign), � �������� �������� �������� 
            // ����� 0 � ������������ ��������� ����� �� ����
            Tower.transform.localEulerAngles = new Vector3(0,
                                                           Tower.transform.localEulerAngles.y +
                                                           Mathf.Clamp(angleTower, 0,
                                                                       Strela10.TowerRotationSpeed.X * Time.fixedDeltaTime) *
                                                           Math.Sign(localDirection.x));
            #endregion

            #region ������� ����������

            localDirection =
                Containers.transform.InverseTransformDirection(point - Containers.transform.position);
            localDirection = new Vector3(0, localDirection.y, localDirection.z);

            //Debug.DrawRay(Containers.transform.position, Containers.transform.TransformDirection(localDirection),
            //              Color.yellow);

            angleContainers = Vector3.Angle(Containers.transform.forward,
                                            Containers.transform.TransformDirection(localDirection));


            clamp = ClampTowerAngle(Containers.transform.localEulerAngles.x + Mathf.Clamp(angleContainers, 0, Strela10.TowerRotationSpeed.Y * Time.fixedDeltaTime) *
                        -Mathf.Sign(localDirection.y));

            if (clamp >= Strela10.TowerLimitAngle.Y.Min && clamp <= Strela10.TowerLimitAngle.Y.Max)
            {
                Containers.transform.localEulerAngles = Vector3.Lerp(Containers.transform.localEulerAngles,
                                                                     new Vector3(
                                                                         Containers.transform.localEulerAngles.x +
                                                                         Mathf.Clamp(angleContainers, 0,
                                                                                     Strela10.TowerRotationSpeed.Y *
                                                                                     Time.fixedDeltaTime) *
                                                                         -Mathf.Sign(localDirection.y), 0, 0),
                                                                     0.25f);
            }

            #endregion

            yield return new WaitForFixedUpdate();
        }
    }

    /*public void ShootWithPrediction(GameObject target)
    {
        Vector3 point; // ����� �������� (� �����������)
        Vector3 startShift; // ����������� �������� ����� �������� ������������ �������-����
        Vector3 localDirection;
        float angleTower, angleContainers;
        float clamp;
        float startDistanceToTarget, distanceK = 1f; // ��������� ��������� �� ���� � ����������� ���������
        Plane plane = new Plane();
        startDistanceToTarget = Vector3.Distance(target.transform.position, Containers.transform.position);
       // float startDistanceToTarget, distanceK = 1f; 
        // ������������ ������ ����� (����� AutoAimTarget � ������)
        //Vector3 localShift = new Vector3(-100, 0, 0);
        var ray = new Ray(Containers.transform.position, Containers.transform.forward);
        float startDistanceToCrosshair;
        plane.Raycast(ray, out startDistanceToCrosshair); // ������ ������� �� ����� ������������
        startShift = (startDistanceToCrosshair * Containers.transform.forward.normalized + ray.GetPoint(startDistanceToCrosshair)) - target.transform.position;

        Vector3 localShift = Containers.transform.InverseTransformDirection(startShift);
        localShift.x += 200;
        // ����������� ���������
        distanceK = Vector3.Distance(target.transform.position, Containers.transform.position) / startDistanceToTarget;

        // ������� � ������� ���������
        plane.SetNormalAndPosition(-Containers.transform.forward, target.transform.position);

        point = Containers.transform.InverseTransformPoint(target.transform.position) + localShift * distanceK; // � ������� ����������� ������ �������� ������������ ����
        point = Containers.transform.TransformPoint(point); // �������� � ���������� ����������

        //Debug.DrawLine(Containers.transform.position, point, Color.yellow);

        // ������ �� �������� � ��������� �����������
        localDirection = Tower.transform.InverseTransformDirection(point - Tower.transform.position);
        localDirection.y = 0;

        // ���� ����� �������� ������ ����� � �����
        angleTower = Vector3.Angle(Tower.transform.forward,
                                   Tower.transform.TransformDirection(localDirection));

        // ������� ����� - ���������� ����� ���� ��� ������ (Mathf.Sign), � �������� �������� �������� 
        // ����� 0 � ������������ ��������� ����� �� ����
        Tower.transform.localEulerAngles = new Vector3(0,
                                                       Tower.transform.localEulerAngles.y +
                                                       Mathf.Clamp(angleTower, 0,
                                                                   Strela10.TowerRotationSpeed.X * Time.fixedDeltaTime) *
                                                       Math.Sign(localDirection.x));

        localDirection =
            Containers.transform.InverseTransformDirection(point - Containers.transform.position);
        localDirection = new Vector3(0, localDirection.y, localDirection.z);

        //Debug.DrawRay(Containers.transform.position, Containers.transform.TransformDirection(localDirection),
        //              Color.yellow);

        angleContainers = Vector3.Angle(Containers.transform.forward,
                                        Containers.transform.TransformDirection(localDirection));


        clamp = ClampTowerAngle(Containers.transform.localEulerAngles.x + Mathf.Clamp(angleContainers, 0, Strela10.TowerRotationSpeed.Y * Time.fixedDeltaTime) *
                    -Mathf.Sign(localDirection.y));

        if (clamp >= Strela10.TowerLimitAngle.Y.Min && clamp <= Strela10.TowerLimitAngle.Y.Max)
        {
            Containers.transform.localEulerAngles = Vector3.Lerp(Containers.transform.localEulerAngles,
                                                                 new Vector3(
                                                                     Containers.transform.localEulerAngles.x +
                                                                     Mathf.Clamp(angleContainers, 0,
                                                                                 Strela10.TowerRotationSpeed.Y *
                                                                                 Time.fixedDeltaTime) *
                                                                     -Mathf.Sign(localDirection.y), 0, 0),
                                                                 0.25f);
        }

  
    }
    */
    void OnGUI()
    {
       //GUI.Toggle(new Rect(Screen.width / 2 - 1f, Screen.height / 2 - 1f, 2f, 2f), true, "");
    }
}
