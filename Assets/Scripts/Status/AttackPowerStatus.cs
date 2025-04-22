using System;
using UnityEngine;


public class AttackPowerStatus : MonoBehaviour, IAttacker, IItemTaker
{
    public int AttackPower
    {
        get;
        private set;
    } = 0;


    public Action<int> onAddValue;

    public void InitializeValue(int initialAttackPower)
    {
        AttackPower = initialAttackPower;
    }

    public void AddValue(int value)
    {
        AttackPower += value;
        onAddValue?.Invoke(value);
    }

    public void ApplyItem(int value)
    {
        AddValue(value);
    }
}
