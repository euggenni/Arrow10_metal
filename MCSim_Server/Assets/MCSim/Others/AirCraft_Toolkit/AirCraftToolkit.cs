using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

#pragma warning disable 0414, 0168, 0618

public enum WeaponryStatus
{
    Active = 0,
    Disabled = 1
}

public class AirCraftToolkit : MonoBehaviour {

    public WeaponryStatus Status = WeaponryStatus.Active;

    public bool isManual = true;

    /// <summary>
    /// Максимальная скорость самолета
    /// </summary>
    public float MaxSpeed = 500f;

    // Горизонтальная ось вращения (крылья)
    public GameObject Center_Horiznontal;
    // Продольная ось вращения
    public GameObject Center_Longitudinal;

    /// <summary>
    /// Массив "зависимость показателей Скорость и Угловая скорость"
    /// </summary>
    //[SerializeField]
    //public float[] SpeedToAngular;


    /// <summary>
    /// Массив "зависимость показателей Скорость и Угловая скорость"
    /// </summary>
    [SerializeField]
    public AnimationCurve SpeedToAngular;

    // Турбина
    public int TurbineCount
    {
        get { return Turbine.Length; }
        set
        {
            List<Rigidbody> temp = new List<Rigidbody>();

            for (int i = 0; i < value; i++)
                temp.Add(null);

            for (int i = 0; i < value && i < Turbine.Length; i++ )
            {
                temp[i] = Turbine[i];
            }

            Turbine = temp.ToArray();
        }
    }
    [SerializeField]
    public Rigidbody[] Turbine = new Rigidbody[2];

    // Площадь крыльев
    [SerializeField]
    public float WSA = 62;
    [SerializeField]
    public float WingSpan = 15f; // Размах крыльев
    [SerializeField]
    public float SAH = 15f; // Средяя аэродинамическая хорда крыла

    // Максимальная тяга движка
    [SerializeField]
    public float MaxThrust = 8250;

    // Плотность воздуха
    private float _airDensity = 1f;

    // Давление воздуха
    private float _airPressure = 1f;

    // Текущая высота
    private float _currentHeight = 100f;

    /// <summary>
    /// Граничные углы закрылок
    /// </summary>
    [SerializeField]
    public float MinPitchAngle = -10;
    [SerializeField]
    public float MaxPitchAngle = 10;
    //private int PitchAngle = 0;

    private IEnumerator CalculateStats()
    {
        while (true)
        {
            _airPressure = 101325 * Mathf.Pow((1f - 0.000022f * _currentHeight), 5.37f);
            Debug.Log("На высоте [" + _currentHeight + "] давление [" + _airPressure + "]");

            _airDensity = (_airPressure * 29f) / (8.31f * 20f); // Òåìïåðàòóðó ìá âû÷èñëÿòü
            Debug.Log("На высоте [" + _currentHeight + "] плотность [" + _airDensity + "]");
            yield return new WaitForSeconds(5f);
        }
    }

    // Максимальный угол отклонения закрылок от оси
    private float Flap_Limit;

    // Множители
    [SerializeField]
    public float ThrustMultiplier = 10f; // Множитель реактивной силы
    [SerializeField]
    public float LiftForceMultiplier = 10f; // Множитель подъемной силы
    [SerializeField]
    public float PitchMultiplier = 1f; // Множитель тангажа
    [SerializeField]
    public float RollMultiplier = 1f;  // Множитель вращения
    [SerializeField]
    public float YawMultiplier = 1f; // Множитель тангажа
    [SerializeField]
    public float InertiaMultiplier = 0.65f;

    private float accelerate = 0f;

    // Коэффициент подъемной силы
    private float _kLift = 1f;
    // Коэффициент тангажа
    private float _kTangage = 1f;

    // Крен / Тангаж

    [SerializeField]
    private float _roll = 0f, _pitch = 0f, _yaw = 0f, _thrust = 0f;



    // Тангажная сила
    private float _tangageForce = 0f;
    // Подъемная сила
    private float _liftForce = 0f;

    private float _dimensionPitch = 0f, _dimensionRoll = 0f;

    /// <summary>
    /// Крен
    /// </summary>
    public float Roll
    {
        get { return _roll; }
        set
        {
            _roll = value;
            if (value > MaxPitchAngle) _roll = MaxPitchAngle;
            if (value < MinPitchAngle) _roll = MinPitchAngle;

            if (_roll > -0.01f && _roll < 0.01f)
            {
                _roll = 0;
            }
        }
    }

    /// <summary>
    /// Тангаж
    /// </summary>
    public float Pitch
    {
        get { return _pitch; }
        set
        {
            _pitch = value;
            if (value > MaxPitchAngle) _pitch = MaxPitchAngle;
            if (value < MinPitchAngle) _pitch = MinPitchAngle;

            if (_pitch > -0.01f && _pitch < 0.01f)
            {
                _pitch = 0;
            }
        }
    }

    /// <summary>
    /// Рыскание
    /// </summary>
    public float Yaw
    {
        get { return _yaw; }
        set
        {
            _yaw = value;
            if (value > MaxPitchAngle) _yaw = MaxPitchAngle;
            if (value < MinPitchAngle) _yaw = MinPitchAngle;

            if (_yaw > -0.01f && _yaw < 0.01f)
            {
                _yaw = 0;
            }
        }
    }

    /// <summary>
    /// Тяга
    /// </summary>
    public float Thrust
    {
        get { return _thrust; }
        set
        {
            _thrust = value;
            if (value > MaxThrust * ThrustMultiplier) _thrust = MaxThrust * ThrustMultiplier;

            if (_thrust > -0.01f && _thrust < 0.01f)
            {
                _thrust = 0;
            }
        }
    }


    /// <summary>
    /// Текущая скорость
    /// </summary>
    private float Speed = 0f;

    // Текущие углы вращения в секунду
    private float _pitchDegPerSec = 0f, _rollDegPerSec = 0f, _yawDegPerSec = 0f;

    // Необходимые углы вращения в секунду
    private float _pitchRequiredDegPerSec = 0f, _rollRequiredDegPerSec = 0f, _yawRequiredDegPerSec = 0f;

    /// <summary>
    /// Возвращает подъемный коэффициент
    /// </summary>
    /// <param name="wingAngle">Угол наклона крыла относительно направления движения</param>
    public float GetLiftK(float wingAngle)
    {
        float val = 0.2f + 1.3f * (wingAngle + 5f) / 15f;

        if (wingAngle > 10) return GetLiftK(10 - wingAngle);
        if (wingAngle < -10) return -0.22f;
        return val;
    }

    /// <summary>
    /// Возвращает угловую скорость
    /// </summary>
    /// <param name="speed">Текущая скорость</param>
    public float GetAngularSpeed(float speed)
    {
        if (speed < 1f && speed > -1f) return 0f; 
        return SpeedToAngular.Evaluate(speed*3.6f)/10f;
    }

    /// <summary>
    /// Возвращает максимально допустимую угловую скорость при текущей скорости
    /// </summary>
    public float AngularSpeed
    {
        get
        {
            if (Speed < 1f && Speed > -1f) return 0f;
            return SpeedToAngular.Evaluate(Speed * 3.6f) / 10f;
        }
    }


	// Use this for initialization
    void Start() {
        Flap_Limit = Mathf.Abs(MinPitchAngle - MaxPitchAngle) / 2;

        _dimensionPitch = 100 / Math.Abs(MinPitchAngle - MaxPitchAngle);
        _dimensionRoll = _dimensionPitch;
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log("Повернулись: " + Vector3.Angle(transform.up, _previousVector) * 1/Time.deltaTime);

        //Debug.DrawRay(Center_Longitudinal.transform.position, Center_Longitudinal.transform.TransformDirection(Vector3.right) * 20, Color.green);

        Debug.DrawRay(transform.position, transform.forward * 15, Color.blue);
        Debug.DrawRay(transform.position, transform.right * 15, Color.red);
        Debug.DrawRay(transform.position, transform.up * 15, Color.green);
        Debug.DrawRay(transform.position, transform.rigidbody.velocity * 1.1f, Color.yellow);

        

        // Вычисляем текущую скорость
	    Speed = transform.InverseTransformDirection(rigidbody.velocity).z;
	}

    private void FixedUpdate()
    {
        //controlTorque = new Vector3(Input.GetAxis("Vertical") * forward_Rotor_Torque_Multiplier, 1.0f, -Input.GetAxis("Horizontal") * sideways_Rotor_Torque_Multiplier);
        if (isManual) // Если контроллируется вручную а не с помощью AI
        {
            if (Input.GetAxis("Horizontal") != 0)
                Roll += Input.GetAxis("Horizontal")*_dimensionRoll;
            else
                Roll = 0;

            if (Input.GetAxis("Vertical") != 0)
                Pitch += Input.GetAxis("Vertical")*_dimensionPitch;
            else
                Pitch = 0;

            Yaw = 0;
            if (Input.GetKey(KeyCode.Q))
                Yaw -= YawMultiplier;
            if (Input.GetKey(KeyCode.E))
                Yaw += YawMultiplier;

            if (Input.GetKey(KeyCode.Space))
            {
                Thrust = MaxThrust * ThrustMultiplier * TurbineCount;
                //foreach (Rigidbody turbine in Turbine)
                //{
                //    //turbine.rigidbody.AddRelativeForce(0, 0, Thrust * ThrustMultiplier);
                //}
                //rigidbody.velocity = transform.forward*200;
            }
            else
            {
                Thrust = 0;
            }
        }

        _kLift = GetLiftK(Pitch);
        _liftForce = _kLift * _airDensity * Mathf.Pow(Speed, 2) * LiftForceMultiplier * WSA; // WSA на площадь крыла относительно земли

        // Если подъемная сила выше гравитационной, нормализуем
        float gravForce = Mathf.Abs(rigidbody.mass*Physics.gravity.y);
        
        if (_liftForce > gravForce + 0.1f*gravForce)
            _liftForce = gravForce + 0.1f*gravForce;

        if (_liftForce < 0)
            _liftForce = 0;

        gameObject.rigidbody.AddForce(0, _liftForce * Mathf.Abs(transform.up.y) / rigidbody.mass, 0, ForceMode.Acceleration);



        //_tangageForce = PitchMultiplier * _kTangage * _airDensity * Mathf.Pow(longVelocity, 2) * WSA * SAH / 2; 

        if (Roll != 0)
        {
            _rollRequiredDegPerSec = GetAngularSpeed(Speed); // Вычисляем на сколько в секунду должен поворачиваться
            if (Pitch != 0) {
                _rollRequiredDegPerSec *= 0.7f;
            }
            if (Roll > 0) _rollRequiredDegPerSec = -_rollRequiredDegPerSec;

            // Интерполируем текущую скорость поворота к необходимой
            _rollDegPerSec = Mathf.Lerp(_rollDegPerSec, _rollRequiredDegPerSec, 0.05f);
            _rollDegPerSec = _rollRequiredDegPerSec;
            try
            {
                transform.RotateAroundLocal(Center_Longitudinal.transform.forward, Time.deltaTime * _rollDegPerSec * 0.0144f * RollMultiplier); // Поворачиваем
            }
            catch { }
        }


        if (Pitch != 0)
        {
            _pitchRequiredDegPerSec = GetAngularSpeed(Speed); // Вычисляем на сколько в секунду должен поворачиваться
            if (Roll != 0){
                _pitchRequiredDegPerSec *= 0.7f;
            }
            if (Pitch < 0) _pitchRequiredDegPerSec = -_pitchRequiredDegPerSec;

            // Интерполируем текущую скорость поворота к необходимой
            _pitchDegPerSec = Mathf.Lerp(_pitchDegPerSec, _pitchRequiredDegPerSec, 0.1f);
            _pitchDegPerSec = _pitchRequiredDegPerSec;
            
                transform.RotateAroundLocal(Center_Horiznontal.transform.right, Time.deltaTime * _pitchDegPerSec * 0.0144f * PitchMultiplier); // Поворачиваем
           
        }

        if (Yaw != 0)
        {
            _yawRequiredDegPerSec = GetAngularSpeed(Speed); // Вычисляем на сколько в секунду должен поворачиваться
            if (Yaw < 0) _yawRequiredDegPerSec = -_yawRequiredDegPerSec;

            // Интерполируем текущую скорость поворота к необходимой
            _yawDegPerSec = Mathf.Lerp(_yawDegPerSec, _yawRequiredDegPerSec, 0.1f);
            transform.RotateAroundLocal(Center_Horiznontal.transform.up, Time.deltaTime * _yawDegPerSec * 0.0144f * YawMultiplier); // Поворачиваем
        }

       

        // Если нужно поддерживать какую то постоянную скорость
        if (!float.IsNaN(_requiredSpeed))
        {
            if(Speed < _requiredSpeed)
            rigidbody.AddRelativeForce(0, 0, MaxThrust*60f, ForceMode.Force);
        }
        else
        {
            rigidbody.AddRelativeForce(0, 0, Thrust, ForceMode.Force);
        }

       

        // Интерполяция
        try
        {
            gameObject.rigidbody.velocity = Vector3.Lerp(gameObject.rigidbody.velocity,
                                                     transform.forward * Mathf.RoundToInt(transform.rigidbody.velocity.magnitude),
                                                     Time.deltaTime * 1 / InertiaMultiplier);
        }
        catch (Exception)
        {}
        
    }

    

    //void OnCollisionEnter()
    //{
    //    if (Mathf.RoundToInt(transform.rigidbody.velocity.magnitude) > 100)
    //    {
    //        isManual = false;
    //        Debug.Log("Crashed");
    //    }
    //}
    
    /// <summary>
    /// Установить постоянное значение скорости
    /// </summary>
    /// <param name="speed">Скорость (м/c)</param>
    public void SetSpeed(float speed) {
        if (Mathf.RoundToInt(speed) == Mathf.RoundToInt(_requiredSpeed)) return;
        if (speed > 0) _requiredSpeed = speed;   
    }
    // Требуемая скорость
    private float _requiredSpeed = float.NaN;
    
    /// <summary>
    /// Сбросить постоянное значение скорости
    /// </summary>
    public void ResetSpeed()
    {
        _requiredSpeed = float.NaN;
    }

}
