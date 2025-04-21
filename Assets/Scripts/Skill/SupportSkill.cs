using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SupportSkill : ISkill
{
    public SkillData SkillData
    {
        get => SupportSkillData;
    }

    public SupportSkillData SupportSkillData
    {
        get;
        private set;
    }

    public bool IsReady
    {
        get => true;
    }

    public float CoolTime
    {
        get => 0f;
    }

    public float CoolDownRemaining
    {
        get => 0f;
    }

    public float CoolTimeRatio
    {
        get => 1f;
    }

    public int Id
    {
        get => SkillData.skillID;
    }

    public int Level
    {
        get => SkillData.level;
    }

    protected SkillManager skillManager;
    public void InitializeSkilManager(SkillManager skillManager)
    {
        this.skillManager = skillManager;
    }

    public SupportSkill(SupportSkillData supportSkillData)
    {
        SupportSkillData = supportSkillData;
    }

    public virtual void Perform(Transform attackerTrs, Transform targetTrs, IAttacker attacker = null, DamageableStatus target = null)
    {
        throw new NotImplementedException();
    }

    public virtual void Update()
    {
        
    }

    public virtual void UpgradeLevel()
    {
        
    }
}
