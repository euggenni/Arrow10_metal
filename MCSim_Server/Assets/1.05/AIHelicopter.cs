 using UnityEngine;
using System.Collections;

public class AIHelicopter : MonoBehaviour
{
    public static float angle;
    public GameObject rotor;
   // public GameObject korpus;
    private Transform _thisTransform;
    private Transform _playerTransform;
    public static float turnSpeed = 90;
    public static float speed = 1f;
    //public GameObject player;
    public static bool boom = false;
    public static float speedrotor = 600f;
    public float speedturn = 2.0f;
    public static Quaternion rot;
  //  public CharacterController cs = new CharacterController();
    public static float distance;
    // Use this for initialization
    void Start()
    {
       // _thisTransform = transform;
        //player = GameObject.Find("GameObject");
        // player = GameObject.FindGameObjectWithTag("trigger");
        // �������� ��������� ������������� ������
        //Player player = (Player)FindObjectOfType(typeof(Player));

    }
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("trigger"))
    //    {
    //        other.tag = "trigger2";
    //        player = GameObject.FindGameObjectWithTag("trigger");

    //    }
    //}
    // Update is called once per frame
    void Update()
    {
       // korpus.gameObject.transform.Rotate(0, -90, -11);
        //if (!boom)
        //{
            Quaternion rotation1 = Quaternion.AngleAxis(speedrotor * Time.deltaTime, Vector3.up);
            //Quaternion rotationRight = Quaternion.AngleAxis(speedturn * Time.deltaTime, Vector3.right);
            //Quaternion rotationLeft = Quaternion.AngleAxis(speedturn * Time.deltaTime * (-1), Vector3.right);
            // ���������� ��������
                rotor.gameObject.transform.rotation *= rotation1;
            //    _playerTransform = player.transform;
            //    distance = Vector3.Distance(_playerTransform.position, _thisTransform.position);
            //    if ( distance< 4000.0f)
            //    {
            //        // ����������� �� ������
            //        Vector3 playerDirection = (_playerTransform.position - _thisTransform.position).normalized;

            //        // ���� �������� �� ������
            //        angle = Vector3.Angle(_thisTransform.forward, playerDirection);

            //        // ������������ ���� �������� �� ������� �����
            //        float maxAngle = turnSpeed * Time.deltaTime;

            //        // ��������� ������ ������� �� ������
            //        rot = Quaternion.LookRotation(_playerTransform.position - _thisTransform.position);

            //        // ������������ ����� �� ������ � ������ �������� ��������
            //        if (maxAngle < angle)
            //        {
            //            _thisTransform.rotation = Quaternion.Slerp(_thisTransform.rotation, rot, maxAngle / angle);
            //        }
            //        else
            //        {
            //            _thisTransform.rotation = rot;
            //        }

            //         Quaternion rotA = Quaternion.AngleAxis(speedturn * Time.deltaTime, Vector3.right);
            //       // if (angle >= 0)
            //       // {
            //            //if  korpus.gameObject.transform.rotation *= rotationRight;
            //            //((korpus.gameObject.transform.rotation.x > 0f)&&(korpus.gameObject.transform.rotation.x<340f)) korpus.gameObject.transform.rotation *= rotationLeft;
            //           // if()
            //           // else if ((angle < angle / 2) && (korpus.gameObject.transform.rotation.x > 1)) korpus.gameObject.transform.rotation *= rotationLeft;
            //       // }

            //        //if (angle < -1)
            //        //{
            //        //    if ((angle < -10) && (korpus.gameObject.transform.rotation.x > -25)) korpus.gameObject.transform.rotation *= rotationLeft;
            //        //    else if ((angle < 10) && (korpus.gameObject.transform.rotation.x < -1)) korpus.gameObject.transform.rotation *= rotationRight;
            //        //}
            //        //if ((angle > -1) && (angle < 1)) korpus.gameObject.transform.rotation.Set(0f, 0f, -10f,0f);
            //    }
            //    cs.Move(this.transform.forward * speed);
            //}
            //else
            //{
            //    cs.Move(this.transform.up * (-1));
            //    cs.Move(this.transform.forward * 0.5f);
            //
        //}
    }
}
