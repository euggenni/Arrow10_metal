using UnityEngine;
using System.Collections;

public enum BitStreamStatus
{
    Read, Write
}
public class NetworkInterpolatedRotation : MonoBehaviour
{
    // public bool X, Y, Z;

    // ��������� �������� ��� ������������ 
    public double interpolationBackTime = 0.05;

    internal struct State
    {
        internal double timestamp;
        internal Quaternion rot;
    }

    public Vector3 StartAngles = new Vector3();


    // ��������� 20 ��������� � ����������� "��� ���������������" 
    State[] m_BufferedState = new State[20];
    // ������� ��������� ������������ 
    int m_TimestampCount;

    void Start()
    {
        StartAngles = transform.localEulerAngles;
    }

    //public bool Write = false;
    private Quaternion _oldRotation;
    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        // ������ �������� ���������  
        if (stream.isWriting)
        {
            Quaternion rot = transform.localRotation;

            if (rot != _oldRotation)
            {
                _oldRotation = rot;
                stream.Serialize(ref rot);
            }
        }
        // ����� ��������� - ������������� ���������� 
        else
        {
            // ��������� ��������� ��������� 
            Quaternion rot = Quaternion.identity;
            stream.Serialize(ref rot);

            // ������� ������ � ������ �� 1 
            for (int i = m_BufferedState.Length - 1; i >= 1; i--)
            {
                m_BufferedState[i] = m_BufferedState[i - 1];
            }

            // ��������� ���������� ������ ��� ������� �������� 
            State state;
            state.timestamp = info.timestamp;
            state.rot = rot;
            m_BufferedState[0] = state;

            // ����������� ���������� ����������� ���������, �� �� ������ ���������� ������ 
            m_TimestampCount = Mathf.Min(m_TimestampCount + 1, m_BufferedState.Length);

            // ��������� ����������� ������ 
            for (int i = 0; i < m_TimestampCount - 1; i++)
            {
                if (m_BufferedState[i].timestamp < m_BufferedState[i + 1].timestamp)
                    Debug.Log("State inconsistent");
            }

            //Debug.Log("stamp: " + info.timestamp + "my time: " + Network.time + "delta: " + (Network.time - info.timestamp)); 
        }
    }

    // �������� ������ ����� ��������� �������, �.�. �� ��������� ����������� (server/clients) 
    void Update()
    {
        double currentTime = Network.time;
        double interpolationTime = currentTime - interpolationBackTime;
        // ���� ����������, ���������� �� ����������� ������� 
        if (m_BufferedState[0].timestamp > interpolationTime)
        {
            for (int i = 0; i < m_TimestampCount; i++)
            {
                if (m_BufferedState[i].timestamp <= interpolationTime || i == m_TimestampCount - 1)
                {
                    // ��� ���������, � ���������� ������� �������� ����� 
                    State lhs = m_BufferedState[i];

                    State rhs = m_BufferedState[Mathf.Max(i - 1, 0)];


                    // ���������� ������� ����� ����������� 
                    double length = rhs.timestamp - lhs.timestamp;
                    float t = 0.0F;
                    // ��������������� ���������� 
                    if (length > 0.0001)
                        t = (float)((interpolationTime - lhs.timestamp) / length);

                    // ������������� ������������� 

                    transform.localRotation = Quaternion.Slerp(lhs.rot, rhs.rot, t);

                    return;
                }
            }
        }
        // ��� ������������� ���������� ��������� ���������� �������� 
        else
        {
            State latest = m_BufferedState[0];

            transform.localRotation = latest.rot;
        }
    }
}