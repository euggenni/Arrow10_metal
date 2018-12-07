using UnityEngine;
using System.Collections;

public class PlayerList_Item : MonoBehaviour
{
    /// <summary>
    /// ������ ������
    /// </summary>
    [SerializeField]
    public MCSMarker PlayerMarker;

    /// <summary>
    /// ��� ������
    /// </summary>
    [SerializeField]
    UILabel PlayerName;

    private NetworkPlayer _player;
    /// <summary>
    /// ���������� ��� ������������� ������ ������� ��������
    /// </summary>
    public NetworkPlayer Player
	{
        get { return _player; }
        set { 
            PlayerName.text = MCSGlobalSimulation.Players.List[value].Account.FirstName + " " + value;
            //PlayerName.text = value.ipAddress;
            _player = value;
            PlayerMarker.Data = value;
        }
	}

    void Start()
    {
        NGUIPanel panel = GetComponent<NGUIPanel>();
        GameObject.Destroy(panel);

        transform.localScale *= MCSUICenter.sizeK;
    }
}
