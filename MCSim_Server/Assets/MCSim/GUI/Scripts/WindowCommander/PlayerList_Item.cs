using UnityEngine;
using System.Collections;

public class PlayerList_Item : MonoBehaviour
{
    /// <summary>
    /// Маркер игрока
    /// </summary>
    [SerializeField]
    public MCSMarker PlayerMarker;

    /// <summary>
    /// Имя игрока
    /// </summary>
    [SerializeField]
    UILabel PlayerName;

    private NetworkPlayer _player;
    /// <summary>
    /// Возвращает или устанавливает игрока данного контрола
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
