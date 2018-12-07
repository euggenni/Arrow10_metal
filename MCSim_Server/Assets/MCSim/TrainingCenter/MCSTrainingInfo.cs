using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MilitaryCombatSimulator;

public abstract class MCSTrainingInfo
{
    public abstract List<MCSTrainingOrder> GetAllOrders();
}