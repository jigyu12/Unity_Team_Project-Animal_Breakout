using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillType
{
    Attack,
    Utility,
}

public enum SkillElemental
{
    Fire,
    Ice,
    Electricity,
    None,
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

public class UtilitySkillData : SkillData
{

}