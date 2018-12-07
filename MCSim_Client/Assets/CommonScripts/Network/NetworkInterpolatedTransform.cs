using UnityEngine;

public class NetworkInterpolatedTransform : MonoBehaviour
{
    public bool localPosition;

    // временная задержка для интерполяции 
    public double interpolationBackTime = 0.1;

    public Vector3 _oldPosition;
    private Quaternion _oldRotation;

    internal struct State
    {
        internal double timestamp;
        internal Vector3 pos;
        internal Quaternion rot;
    }

    // Сохраняем 20 состояний с информацией "для воспроизведения" 
    State[] m_BufferedState = new State[20];
    // сколько состояний используется 
    int m_TimestampCount;

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        // Всегда отсылаем трансформ  
        if (stream.isWriting)
        {
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;

            if (localPosition)
            {
                pos = transform.localPosition;
                rot = transform.localRotation;
            }

            //if (pos != _oldPosition || rot != _oldRotation)
            if (Vector3.Distance(pos, _oldPosition) > 0.05f || Quaternion.Angle(rot, _oldRotation) > 1f)
            {
                stream.Serialize(ref pos);
                stream.Serialize(ref rot);

                _oldPosition = pos;
                _oldRotation = rot;
            }
        }
        // Когда принимаем - буфферизируем информацию 
        else
        {
            // Принимаем последнее состояние 
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);

            // Смещаем данные в буфере на 1 
            for (int i = m_BufferedState.Length - 1; i >= 1; i--)
            {
                m_BufferedState[i] = m_BufferedState[i - 1];
            }

            // сохраняем полученные данные под нулевым индексом 
            State state;
            state.timestamp = info.timestamp;
            state.pos = pos;
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

            //Type t = Type.GetControlType("asd");
            //Object obj = new Object();
            //NetworkInterpolatedTransform nit = (t) obj;
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
                    // два состояния, в промежуток которых попадает времяя 
                    State rhs = m_BufferedState[Mathf.Max(i - 1, 0)];

                    State lhs = m_BufferedState[i];

                    // промежуток времени между состояниями 
                    double length = rhs.timestamp - lhs.timestamp;
                    float t = 0.0F;
                    // интерполирующая переменная 
                    if (length > 0.0001)
                        t = (float)((interpolationTime - lhs.timestamp) / length);

                    // интерполируем трансформации 
                    if (!localPosition)
                    {
                        transform.position = Vector3.Lerp(lhs.pos, rhs.pos, t);
                        transform.rotation = Quaternion.Slerp(lhs.rot, rhs.rot, t);
                    }
                    else
                    {
                        transform.localPosition = Vector3.Lerp(lhs.pos, rhs.pos, t);
                        transform.localRotation = Quaternion.Slerp(lhs.rot, rhs.rot, t);
                    }
                    return;
                }
            }
        }
        // для экстраполяции используем последнее полученное значение 
        else
        {
            State latest = m_BufferedState[0];

            if (!localPosition)
            {
                transform.position = latest.pos;
                transform.rotation = latest.rot;
            }
            else
            {
                transform.localPosition = latest.pos;
                transform.localRotation = latest.rot;
            }
        }
    }
}