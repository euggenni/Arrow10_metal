using UnityEngine;
using System.Collections;


// ����������� �� ������������ ����������
[System.Serializable]
public class MCSRanks {

    public enum Ranks
    {
        ������� = 0,
        ��������� = 1,
        ��������� = 2
    }

    public Ranks _rank;
    //public Ranks Rank
    //{
    //    get { return _rank; }
    //}

    /// <summary>
    /// ������
    /// </summary>
    public string Rank { get; set; }
}
