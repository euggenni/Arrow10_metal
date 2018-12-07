using MilitaryCombatSimulator;
using UnityEngine;
    using System.Collections;

    public class MouseOrbitAroundTarget : MonoBehaviour
    {
        public  Transform _target;

        public Transform target
        {
            get { return _target; }
            set
            {
                _target = value;

                transform.rotation = Quaternion.Euler(y, x, 0);
                transform.position = (Quaternion.Euler(y, x, 0))*new Vector3(0.0f, 0.0f, -distance) + target.position;
            }
        }

        public float distance = 10.0f;

        public float xSpeed = 200.0f;
        public float ySpeed = 120.0f;

        public float yMinLimit = -20;
        public float yMaxLimit = 80;

        public float x = 0.0f;
        public float y = 0.0f;

        void Start () 
        {
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            transform.rotation = Quaternion.Euler(y, x, 0);

            transform.position = (Quaternion.Euler(y, x, 0)) * new Vector3(0.0f, 0.0f, -distance) + target.position;

            // Make the rigid body not change rotation
            //if (rigidbody) 
            //    rigidbody.freezeRotation = true;
        }

        private bool dragging;


        void FixedUpdate()
        {
            if(!Input.GetKey(KeyCode.Mouse0)) return;

            distance += -(Input.GetAxis("Mouse ScrollWheel")) * 10;

            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            transform.rotation = Quaternion.Euler(y, x, 0);

            transform.position = (Quaternion.Euler(y, x, 0)) * new Vector3(0.0f, 0.0f, -distance) + target.position;
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