using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManagerTest : MonoBehaviour
{
    public SkillData currentSkillData;
    public List<SkillData> currentSkillDatas = new();

    [SerializeField]
    private SkillManager skillManager;

    private int priority = 0;

    public void AddSkill(SkillData skillData)
    {
        skillManager.SkillSelectionSystem.AddSkill(priority++, skillData);
    }

}
