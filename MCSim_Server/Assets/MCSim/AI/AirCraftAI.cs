using System;
using UnityEngine;
using System.Collections;

#pragma warning disable 0414, 0108, 0219
#pragma warning disable 0618, 0168

[RequireComponent(typeof(AirCraftToolkit))]
public class AirCraftAI : AIUnit
{
    public enum Quarter
    {
        I, II, III, IV
    }

    public enum Hemisphere
    {
        Front, Rear
    }

    public enum FlightType
    {
        Rotation, // Маневр с использованием лишь Roll
        Maneuver, // Маневр с использованием Pitch и Roll
        Pitching, // Маневр только с использованием Pitch'a
        Correction // Маневр-доводка с доводкой до цели
    }

    /// <summary>
    /// По достижении этого радиуса, начинается корректировка движения
    /// </summary>
    private float _correctionRadius = 15f;

    /// <summary>
    /// Скорость изменения Roll, Pitch
    /// </summary>
    public float WheelSpeed = 1f;

    /// <summary>
    /// В каких пределах нужно повернуться к цели
    /// </summary>
    public float AngleFault = 5f;

    /// <summary>
    /// Угол зоны, в которой позволено совершать тангаж
    /// </summary>
    public float PitchAreaAngle = 30f;

    private AirCraftToolkit _airCraft;

    /// <summary>
    /// Оптимальная скорость для поворотов
    /// </summary>
    private float _optimalSpeed = -1f;

    private Vector3 _up, _right, _forward;

	void Start ()
	{
        _airCraft = GetComponent<AirCraftToolkit>();
        _airCraft.isManual = false;

        // Оптимальная скорость
        _optimalSpeed = CalculateOptimalSpeed(_airCraft);

        _airCraft.SetSpeed(_optimalSpeed);

        _up = transform.InverseTransformDirection(transform.up);
        _right = transform.InverseTransformDirection(transform.right);
        _forward = transform.InverseTransformDirection(transform.forward);

        MoveToWaypoint(0);
	}
    
    private FlightType _flightType;
	new void Update ()
    {
        base.Update();

        if (Waypoints.Count == 0) return;

	    MCSWaypoint wp = Waypoints[CurrentWaypoint];
	    Debug.DrawLine(transform.position, wp.transform.position, Color.cyan);

        Vector3 to = wp.transform.position - transform.position;

        ///////////////////////////////

        _pos = Waypoints[CurrentWaypoint].transform.position;
        _localpos = transform.InverseTransformPoint(_pos).normalized;

        // Положение в координатных осях.
	    _signX = _localpos.x;
	    _signY = _localpos.y;
        _signZ = transform.up.z;

        _signX = _signX / Mathf.Abs(_signX);
        _signY = _signY / Mathf.Abs(_signY);
        _signZ = _signZ / Mathf.Abs(_signZ);

        _localX = Vector2.Angle(new Vector2(_up.x, _up.y),
                                      new Vector2(_localpos.x, _localpos.y));
        _localY = Vector2.Angle(new Vector2(_right.x, _right.y),
                                      new Vector2(_localpos.x, _localpos.y));

        _localPitch = 90 - Vector3.Angle(transform.up, to);
	}

    /// Параметры для цели
    private Vector3 _pos; // Позиция цели
    private Vector3 _localpos; // Локальная позиция цели

    private Quarter _quarter; // Четверть, в которой находится цель
    private float _signX, _signY, _signZ; // Знаки четвертей
    private float _localX, _localY, _localPitch; // Угол между transform.forward и целью

    private float _totalAngle;

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawSphere(_pos, 50);
    }

    /// <summary>
    /// Возвращает значение 0..1, которое показывает степень наличия значения maxAngleSpeed в axeAngle
    /// </summary>
    float GetAngleProportion(float maxAngleSpeed, float axeAngle)
    {
        if (maxAngleSpeed == 0) return 0f;
        if (maxAngleSpeed <= axeAngle) return 1f;

        return axeAngle/maxAngleSpeed;
    }

    /// <summary>
    /// Возвращает тип маневра, который нужно совершить для достижения цели
    /// </summary>
    FlightType GetFlightType()
    {
        FlightType type;

        // Если в зоне коррекци полета
        if (_totalAngle >= -AngleFault && _totalAngle <= AngleFault)
            return FlightType.Correction;

        // Если мы в боковых зонах - нужно повернуть
        if((_localX >= PitchAreaAngle && _localX <= 180 - PitchAreaAngle) || (_localX <= -PitchAreaAngle && _localX >= -180 + PitchAreaAngle)) {
            return FlightType.Rotation; 
        }
        
        // Если не в зоне коррекци полета и не поворачиваем - делаем маневр
        return FlightType.Maneuver;
    }

    /// <summary>
    /// Отклонение от оси Крена
    /// </summary>
    private float DeviationFromRollAxe
    {
        get { 
            return Mathf.Min(Mathf.Abs(_localX), Mathf.Abs(180 - Mathf.Abs(_localX)));
        }
    }

    private FlightType flightType;

    void FixedUpdate()
    {
        if (Waypoints.Count == 0) return;

        // Максимальный угол поворота в текущем кадре с текущей скорость
        float angSpeed = _airCraft.AngularSpeed;

        // Коэффициенты распределения поворота
        float pitchK = 0f, rollK = 0f;

        // pitchK + rollK = 1
        // Определяем насколько Pitch и Roll близки к целевому значению - 0
        //float anglePitch = 0f, angleRoll = 0f;

        // На сколько нужно повернуть в соотношении 0..1 от максимального угла поворота
        pitchK = GetAngleProportion(angSpeed, Mathf.Abs(_localPitch));
        rollK = GetAngleProportion(angSpeed, Mathf.Abs(_localX));

        _quarter = GetTargetQuarter(Waypoints[CurrentWaypoint].transform.position); // Определяем в какой четверти находится цель
        _totalAngle = Vector3.Angle(transform.forward, _pos - transform.position); // Определяем общий угол до цели

        flightType = GetFlightType(); // Определяем тип движения

        switch (flightType)
        {
            case FlightType.Rotation:
                _airCraft.SetSpeed(_optimalSpeed);
                if (_quarter == Quarter.I || _quarter == Quarter.IV)
                {
                    if (_airCraft.Roll < 0) _airCraft.Roll = 0;
                    _airCraft.Roll = Mathf.Lerp(_airCraft.Roll, _airCraft.MaxPitchAngle * rollK, Time.deltaTime);
                }

                if (_quarter == Quarter.II || _quarter == Quarter.III)
                {
                    if (_airCraft.Roll > 0) _airCraft.Roll = 0;
                    _airCraft.Roll = Mathf.Lerp(_airCraft.Roll, -_airCraft.MaxPitchAngle * rollK, Time.deltaTime);
                }
                break;

            case FlightType.Maneuver:
                _airCraft.SetSpeed(_optimalSpeed);
                if (DeviationFromRollAxe > AngleFault) // Определяем минимальный угол до цели
                {
                    if (_quarter == Quarter.I || _quarter == Quarter.III) // Поворот направо
                    {
                        if (_airCraft.Roll < 0) _airCraft.Roll = 0;
                        _airCraft.Roll = Mathf.Lerp(_airCraft.Roll, _airCraft.MaxPitchAngle * rollK, Time.deltaTime);
                    }

                    if (_quarter == Quarter.II || _quarter == Quarter.IV) // Поворот налево
                    {
                        if (_airCraft.Roll > 0) _airCraft.Roll = 0;
                        _airCraft.Roll = Mathf.Lerp(_airCraft.Roll, -_airCraft.MaxPitchAngle * rollK, Time.deltaTime);
                    }
                }
                else
                {
                    _airCraft.Roll = 0;
                    float angle = DeviationFromRollAxe;

                    if (_quarter == Quarter.III || _quarter == Quarter.IV)
                    {
                        // Движение вниз, если объект находится ниже самолета
                        angle = -angle;
                    }
                    //Debug.Log("Angle: " + angle);
                    try
                    {
                        transform.Rotate(0, 0, angle * Time.deltaTime);
                    }
                    catch { }
                }

                int pitchSign = 1;

                try
                {
                    pitchSign = (int)_localPitch / (int)Mathf.Abs(_localPitch);
                }
                catch { }
                _airCraft.Pitch = -_airCraft.MaxPitchAngle * pitchSign * pitchK;
                break;

            case FlightType.Correction:
                _airCraft.SetSpeed(_airCraft.MaxSpeed);
                _airCraft.Pitch = 0;
                _airCraft.Roll = 0;
                Vector3 result = Waypoints[CurrentWaypoint].transform.position - transform.position;
                  transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(result), Time.deltaTime);
                
                break;
        }
    }


    /// <summary>
    /// Подсчитать оптимальную скорость для маневров
    /// </summary>
    /// <param name="airCraft">Объект для подсчета</param>
    private float CalculateOptimalSpeed(AirCraftToolkit airCraft)
    {
        float dimension = _airCraft.MaxSpeed/50;

        float optimalSpeed = 0f, maxAngularSpeed = 0f;

        for(float speed = 0; speed <= airCraft.MaxSpeed * 3.6f; speed += dimension)
        {
            if(airCraft.SpeedToAngular.Evaluate(speed) > maxAngularSpeed)
            {
                maxAngularSpeed = airCraft.SpeedToAngular.Evaluate(speed);
                optimalSpeed = speed;
            }
        }
        // Переводим в м/с
        return Mathf.Round(optimalSpeed/3.6f);
    }

    /// <summary>
    /// Возвращает четверть, в которой располагается точка относительно transform.forward
    /// </summary>
    /// <param name="target">Координаты точки</param>
    public Quarter GetTargetQuarter(Vector3 target)
    {
        Quarter res = Quarter.I;
        Vector3 localpos = transform.InverseTransformPoint(target).normalized;
        
        if(localpos.x < 0 && localpos.y >= 0)
            return Quarter.II;

        if (localpos.x < 0 && localpos.y < 0)
            return Quarter.III;

        if (localpos.x >= 0 && localpos.y < 0)
            return Quarter.IV;

        return res;
    }

    /// <summary>
    /// Возвращает имя полусферы, в которой находится точка
    /// </summary>
    /// <param name="target">Координаты точки</param>
    /// <returns>Передняя/Задняя</returns>
    public Hemisphere GetTargetHemisphere(Vector3 target)
    {
        Quarter res = Quarter.I;
        Vector3 localpos = transform.InverseTransformPoint(target).normalized;

        float localPitch = -Vector3.Angle(new Vector3(0, _forward.y, _forward.z),
                                      new Vector3(0, localpos.y, localpos.z)) * localpos.z / Mathf.Abs(localpos.z);

        if(localPitch > -90 && localPitch < 90) return Hemisphere.Front;

        return Hemisphere.Rear;
    }
}
