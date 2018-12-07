using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public interface MCSIUIObject
{
    /// <summary>
    /// ������ �������
    /// </summary>
    void Hide();

    /// <summary>
    /// ���������� �������
    /// </summary>
    void Show();

    /// <summary>
    /// ������� �������
    /// </summary>
    void Close();
    

    /// <summary>
    /// ��������� �������
    /// </summary>
    bool Visible { get; set; }
}

public abstract class MCSUIObject : MonoBehaviour {

    protected bool _visible;

    /// <summary>
    /// ������ �������
    /// </summary>
    public abstract void Hide();

    /// <summary>
    /// ���������� �������
    /// </summary>
    public abstract void Show();

    /// <summary>
    /// ������� �������
    /// </summary>
    public abstract void Close();
    
    /// <summary>
    /// ��������� �������
    /// </summary>
    bool Visible { get; set; }
}
