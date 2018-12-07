using System;
using UnityEngine;

#pragma warning disable 0649, 0168

/// <summary>
/// Аккаунт игрока с необходимой информацией. Сериализуемый
/// </summary>
[Serializable]
public class MCSAccountInfo {
    public MCSAccountInfo(string firstName, string secondName, string nickname) {
        FirstName = firstName;
        SecondName = secondName;
        NickName = nickname;
    }

    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; private set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string SecondName { get; private set; }

    /// <summary>
    /// Позывной
    /// </summary>
    public string NickName { get; private set; }

    [SerializeField]
    private MCSRanks _rank;

    /// <summary>
    /// Звание
    /// </summary>
    public MCSRanks Rank {
        get { return _rank; }
    }
}