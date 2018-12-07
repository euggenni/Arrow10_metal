using UnityEngine;
using System.Collections;
using System;


#pragma warning disable 0649, 0168

/// <summary>
/// ������� ������ � ����������� �����������. �������������
/// </summary>
[Serializable]
public class MCSAccountInfo
{
    public MCSAccountInfo(string firstName, string secondName, string nickname)
    {
        FirstName = firstName;
        SecondName = secondName;
        NickName = nickname;
    }

    /// <summary>
    /// ���
    /// </summary>
    public string FirstName { get; private set; }

    /// <summary>
    /// �������
    /// </summary>
    public string SecondName { get; private set; }

    /// <summary>
    /// ��������
    /// </summary>
    public string NickName { get; private set; }

    [SerializeField]
    private MCSRanks _rank;
    /// <summary>
    /// ������
    /// </summary>
    public MCSRanks Rank
    {
        get { return _rank; }
    }
}
