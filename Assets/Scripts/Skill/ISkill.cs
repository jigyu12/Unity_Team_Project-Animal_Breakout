

using System;
using UnityEngine;

public interface ISkill
{
    public static readonly int maxLevel = 5;

    public SkillData SkillData
    {
        get;
    }
    public int Id
    {
        get;
    }

    public int Level
    {
        get;
    }

    public bool IsReady
    {
        get;
    }

    //public float CoolTime
    //{
    //    get;
    //}

    //public float CoolDownRemaining
    //{
    //    get;
    //}

    //public float CoolTimeRatio
    //{
    //    get;
    //}

    public void InitializeSkilManager(SkillManager skillManager);

    void Perform(Transform attackerTrs, Transform targetTrs, AttackPowerStatus attacker = null, DamageableStatus target = null);

    //public void OnReady();
    //public void AddOnReadyAction(Action onReady);
    public void UpgradeLevel();

    public void Update();


}
