using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using MilitaryCombatSimulator;

public class MCSSubordination
{
    private MCSPlayer _authority; // Командир 
    private List<MCSPlayer> _subordinates; // Подчиненные
    private List<MCSPlayer> _crews; // Экипаж

    public MCSSubordination()
    {
        _subordinates = new List<MCSPlayer>();
        _crews = new List<MCSPlayer>();
    }

    /// <summary>
    /// Командир
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
                    _subordinates.Remove(value); // Удаляем командира из списка подчиненных, если он там есть
                }
                catch
                {
                }
            }

            _authority = value;
        }
    }

    /// <summary>
    /// Подчиненные
    /// </summary>
    public List<MCSPlayer> Subordinates
    {
        get { return _subordinates; }
    }

    /// <summary>
    /// Экипаж
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
            if (_authority == player) _authority = null; // Убираем командира, если помещаем его в подчиненные
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
    /// Статус игрока
    /// </summary>
    public PlayerStatus Status = PlayerStatus.None;

    public MCSPlayer (NetworkPlayer player, MCSAccountInfo accountInfo)
    {
        Status = PlayerStatus.Connected;
        NetworkPlayer = player;
        Account = accountInfo;
    }
    
    /// <summary>
    /// NetworkPlayer игрока
    /// </summary>
    public NetworkPlayer NetworkPlayer { get; set; }

    /// <summary>
    /// Weaponry, связанный с этим игроком
    /// </summary>
    public Weaponry Weaponry { get; set; }

    /// <summary>
    /// Объект для хранения ссылки на MCSPlayer Начальника, Подчиненных и Экипажа
    /// </summary>
    public MCSSubordination Subordination { get; set; }

    /// <summary>
    /// Объект для хранения информации об аккаунте. Имя, Звание и т.п.
    /// </summary>
    public MCSAccountInfo Account { get; set; }

    /// <summary>
    /// Роль игрока в качестве экипажа Weaponry
    /// </summary>
    public string Role { get; set; }

    // Боевая задача, список отданных команд
}
