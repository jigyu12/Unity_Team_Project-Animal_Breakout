using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillType
{
    BossTarget,
    Utill,
}

public enum SkillAttribute
{
    Fire,
    Ice,
    Electricity,
    None,
}


public class SkillData : ScriptableObject
{
    int skillID;

    SkillType skillType;

    SkillAttribute skillAttribute;

    int level;

    float damage;

    float coolDownTime;

}
