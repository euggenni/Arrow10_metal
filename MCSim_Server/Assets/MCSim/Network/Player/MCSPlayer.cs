using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using MilitaryCombatSimulator;

public class MCSSubordination
{
    private MCSPlayer _authority; // �������� 
    private List<MCSPlayer> _subordinates; // �����������
    private List<MCSPlayer> _crews; // ������

    public MCSSubordination()
    {
        _subordinates = new List<MCSPlayer>();
        _crews = new List<MCSPlayer>();
    }

    /// <summary>
    /// ��������
    /// </summary>
    public MCSPlayer Authority
    {
        get { return _authority; }
        set
        {
            if (_subordinates != null)
            {
                try
                {
                    _subordinates.Remove(value); // ������� ��������� �� ������ �����������, ���� �� ��� ����
                }
                catch
                {
                }
            }

            _authority = value;
        }
    }

    /// <summary>
    /// �����������
    /// </summary>
    public List<MCSPlayer> Subordinates
    {
        get { return _subordinates; }
    }

    /// <summary>
    /// ������
    /// </summary>
    public List<MCSPlayer> Crews
    {
        get { return _crews; }
    }

    public void AddSubordinate(MCSPlayer player)
    {
        if (_subordinates.Contains(player)) return;

        if (_authority != null)
        {
            if (_authority == player) _authority = null; // ������� ���������, ���� �������� ��� � �����������
        }
        _subordinates.Add(player);
    }
    public void RemoveSubordinate(MCSPlayer player)
    {
        _subordinates.Remove(player);
    }

    public void AddCrew(MCSPlayer player) {
        if (_crews.Contains(player)) return;

        _crews.Add(player);
    }
    public void RemoveCrew(MCSPlayer player) {
        _crews.Remove(player);
    }
}

public class MCSPlayer {
    
    public enum PlayerStatus
    {
        None = 0,
        Disconnected = 1,
        Connected = 2,
        WaitingForWeaponry = 3,
    }

    /// <summary>
    /// ������ ������
    /// </summary>
    public PlayerStatus Status = PlayerStatus.None;

    public MCSPlayer (NetworkPlayer player, MCSAccountInfo accountInfo)
    {
        Status = PlayerStatus.Connected;
        NetworkPlayer = player;
        Account = accountInfo;
    }
    
    /// <summary>
    /// NetworkPlayer ������
    /// </summary>
    public NetworkPlayer NetworkPlayer { get; set; }

    /// <summary>
    /// Weaponry, ��������� � ���� �������
    /// </summary>
    public Weaponry Weaponry { get; set; }

    /// <summary>
    /// ������ ��� �������� ������ �� MCSPlayer ����������, ����������� � �������
    /// </summary>
    public MCSSubordination Subordination { get; set; }

    /// <summary>
    /// ������ ��� �������� ���������� �� ��������. ���, ������ � �.�.
    /// </summary>
    public MCSAccountInfo Account { get; set; }

    /// <summary>
    /// ���� ������ � �������� ������� Weaponry
    /// </summary>
    public string Role { get; set; }

    // ������ ������, ������ �������� ������
}
