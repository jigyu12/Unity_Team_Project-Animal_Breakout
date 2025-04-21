using System;
using UnityEngine;


public class AttackPowerStatus : MonoBehaviour, IAttacker, IItemTaker
{
    public int AttackPower
    {
        get => (int)(attackPower+ attackPower * additionalAttackPowerRate);
    }

    private float attackPower = 0;

    public Action<int> onAddValue;

    private float additionalAttackPowerRate = 0f;
    //private float additionalElementalAttackPowerRate;

    public void InitializeValue(int initialAttackPower)
    {
        attackPower = initialAttackPower;
    }

    public void SetAdditionalAttackPowerRateValue(float value)
    {
        additionalAttackPowerRate += value;
    }

    public void AddValue(int value)
    {
        attackPower += value;
        onAddValue?.Invoke(value);
    }

    public void ApplyItem(int value)
    {
        AddValue(value);
    }
}
