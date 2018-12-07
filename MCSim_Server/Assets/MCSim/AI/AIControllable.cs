using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public interface AIControllable {

    /// <summary>
    /// ������� AI-������ �� ������� �������
    /// </summary>
    void InitializeAI();

    /// <summary>
    /// AI-�����
    /// </summary>
    AIUnit AIUnit { get; }
}
