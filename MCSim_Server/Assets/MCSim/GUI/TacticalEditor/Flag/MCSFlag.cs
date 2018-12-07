using UnityEngine;
using System.Collections;

public abstract class MCSFlag : MonoBehaviour
{
    /// <summary>
    /// Задание цели для флага
    /// </summary>
    /// <param name="target">Цель</param>
    public abstract void SetTarget(GameObject target);
}
