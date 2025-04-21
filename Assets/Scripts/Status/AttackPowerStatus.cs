using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class AttackPowerStatus : MonoBehaviour, IAttacker, IItemTaker
{
    public int AttackPower
    {
        get => (int)(attackPower + attackPower * additionalAttackPowerRate);
    }

    private float attackPower = 0;

    public Action<int> onAddValue;

    private float additionalAttackPowerRate = 0f;


    //public struct AdditionalElementalAttackPowerRate
    //{
    //    public SkillElemental type;
    //    public float additionalAttackPowerRate;
    //}
    //private List<AdditionalElementalAttackPowerRate> additionalElementalAttackPowerRates = new();

    private List<float> additionalElementalAttackPowerRates = new ();
    public void InitializeValue(int initialAttackPower)
    {
        attackPower = initialAttackPower;

        foreach (SkillElemental elemental in Enum.GetValues(typeof(SkillElemental)))
        {
            //additionalElementalAttackPowerRates.Add(new AdditionalElementalAttackPowerRate { type = elemental, additionalAttackPowerRate = 0f });
            additionalElementalAttackPowerRates.Add(0f);
        }
    }

    public void SetAdditionalAttackPowerRateValue(float value)
    {
        additionalAttackPowerRate += value;
    }

    public void SetElementalAdditionalAttackPowerRateValue(SkillElemental elemental,  float value)
    {
        additionalElementalAttackPowerRates[(int)elemental] += value;
    }

    public int GetElementalAdditionalAttackPower(SkillElemental elemental)
    {
       return (int)(attackPower + attackPower * (additionalAttackPowerRate+ additionalElementalAttackPowerRates[(int)elemental]));
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
