using System;
using System.Collections.Generic;
using UnityEngine;


public class AttackPowerStatus : MonoBehaviour, IAttacker, IItemTaker
{
    public int AttackPower
    {
        get => (int)(attackPower + additionalAttackPower);
    }

    private float attackPower = 0;

    public Action<int> onAddValue;

    public float AdditionalSkillAttackPowerRate
    {
        get;
        private set;
    } = 0;

    private float additionalAttackPower = 0f;
    private int additionalItemValue=0;

    //public struct AdditionalElementalAttackPowerRate
    //{
    //    public SkillElemental type;
    //    public float additionalAttackPowerRate;
    //}
    //private List<AdditionalElementalAttackPowerRate> additionalElementalAttackPowerRates = new();

    private List<float> additionalElementalAttackPowerRates = new();
    public void InitializeValue(int initialAttackPower)
    {
        attackPower = initialAttackPower;

        foreach (SkillElemental elemental in Enum.GetValues(typeof(SkillElemental)))
        {
            //additionalElementalAttackPowerRates.Add(new AdditionalElementalAttackPowerRate { type = elemental, additionalAttackPowerRate = 0f });
            additionalElementalAttackPowerRates.Add(0f);
        }
    }

    public void AddAdditionalSkillAttackPowerRateValue(float value)
    {
        AdditionalSkillAttackPowerRate += value;
    }

    public void AddAdditionalAttackPowerValue(float value)
    {
        additionalAttackPower += value;
    }

    public void AddElementalAdditionalAttackPowerRateValue(SkillElemental elemental, float value)
    {
        additionalElementalAttackPowerRates[(int)elemental] += value;
    }
    public void AddAdditionalItemValue(int value)
    {
        additionalItemValue += value;
    }

    public int GetElementalAdditionalAttackPower(SkillElemental elemental)
    {
        return (int)(AttackPower + AttackPower*additionalElementalAttackPowerRates[(int)elemental]);
    }

    public void AddValue(int value)
    {
        attackPower += value+ additionalItemValue;
        onAddValue?.Invoke(value+ additionalItemValue);
    }

    public void ApplyItem(int value)
    {
        AddValue(value);
    }
}
