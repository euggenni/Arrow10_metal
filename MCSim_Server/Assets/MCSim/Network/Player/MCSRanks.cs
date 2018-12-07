using UnityEngine;
using System.Collections;


// Наследовать от графического интерфейса
[System.Serializable]
public class MCSRanks {

    public enum Ranks
    {
        Рядовой = 0,
        Еферейтор = 1,
        МлСержант = 2
    }

    public Ranks _rank;
    //public Ranks Rank
    //{
    //    get { return _rank; }
    //}

    /// <summary>
    /// Звание
    /// </summary>
    public string Rank { get; set; }
}
