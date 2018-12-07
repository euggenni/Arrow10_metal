using UnityEngine;
using System.Collections;

public enum BitStreamStatus
{
    Read, Write
}
public class NetworkInterpolatedRotation : MonoBehaviour
{
    // public bool X, Y, Z;

    // временная задержка для интерполяции 
    public double interpolationBackTime = 0.05;

    internal struct State
    {
        internal double timestamp;
        internal Quaternion rot;
    }

    public Vector3 StartAngles = new Vector3();


    // Сохраняем 20 состояний с информацией "для воспроизведения" 
    State[] m_BufferedState = new State[20];
    // сколько состояний используется 
    int m_TimestampCount;

    void Start()
    {
        StartAngles = transform.localEulerAngles;
    }

    //public bool Write = false;
    private Quaternion _oldRotation;
    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        // Всегда отсылаем трансформ  
        if (stream.isWriting)
        {
            Quaternion rot = transform.localRotation;

            if (rot != _oldRotation)
            {
                _oldRotation = rot;
                stream.Serialize(ref rot);
            }
        }
        // Когда принимаем - буфферизируем информацию 
        else
        {
            // Принимаем последнее состояние 
            Quaternion rot = Quaternion.identity;
            stream.Serialize(ref rot);

            // Смещаем данные в буфере на 1 
            for (int i = m_BufferedState.Length - 1; i >= 1; i--)
            {
                m_BufferedState[i] = m_BufferedState[i - 1];
            }

            // сохраняем полученные данные под нулевым индексом 
            State state;
            state.timestamp = info.timestamp;
            state.rot = rot;
            m_BufferedState[0] = state;

            // увеличиваем количество сохраненных состояний, но не больше количества слотов 
            m_TimestampCount = Mathf.Min(m_TimestampCount + 1, m_BufferedState.Length);

            // проверяем целостность данных 
            for (int i = 0; i < m_TimestampCount - 1; i++)
            {
                if (m_BufferedState[i].timestamp < m_BufferedState[i + 1].timestamp)
                    Debug.Log("State inconsistent");
            }

            //Debug.Log("stamp: " + info.timestamp + "my time: " + Network.time + "delta: " + (Network.time - info.timestamp)); 
        }
    }

    // Работает только когда компонент включен, т.е. на удаленных соединениях (server/clients) 
    void Update()
    {
        double currentTime = Network.time;
        double interpolationTime = currentTime - interpolationBackTime;
        // Ищем промежуток, подходящий по отложенному времени 
        if (m_BufferedState[0].timestamp > interpolationTime)
        {
            for (int i = 0; i < m_TimestampCount; i++)
            {
                if (m_BufferedState[i].timestamp <= interpolationTime || i == m_TimestampCount - 1)
                {
                    // два состояния, в промежуток которых попадает время 
                    State lhs = m_BufferedState[i];

                    State rhs = m_BufferedState[Mathf.Max(i - 1, 0)];


                    // промежуток времени между состояниями 
                    double length = rhs.timestamp - lhs.timestamp;
                    float t = 0.0F;
                    // интерполирующая переменная 
                    if (length > 0.0001)
                        t = (float)((interpolationTime - lhs.timestamp) / length);

                    // интерполируем трансформации 

                    transform.localRotation = Quaternion.Slerp(lhs.rot, rhs.rot, t);

                    return;
                }
            }
        }
        // для экстраполяции используем последнее полученное значение 
        else
        {
            State latest = m_BufferedState[0];

            transform.localRotation = latest.rot;
        }
    }
}