using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillType
{
    Attack,
    Support,
}

public enum SkillElemental
{
    Fire,
    Ice,
    Electricity,
    None,
}

public enum SupportSkillTarget
{
    AttackPower,
    ElementAttackPower,
    CoolDown,
    Experience,
}


public class SkillData : ScriptableObject
{
    public int skillID;
    public SkillType skillType;

    public int level;
    public float coolDownTime;

}

public class AttackSkillData : SkillData
{
    public SkillElemental skillElemental;
}

public class SupportSkillData : SkillData
{
    public SupportSkillTarget skillTarget;
    public float rate;


}