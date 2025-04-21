using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManagerTest : MonoBehaviour
{
    [SerializeField]
    private ProjectileSkillData fire;

    [SerializeField]
    private ProjectileSkillData ice;

    [SerializeField]
    private ProjectileSkillData electicity;

    [SerializeField]
    private SkillManager skillManager;

    private int priority = 0;

    private void Start()
    {

        AddTempSkill();
  
    }

    [ContextMenu("AddSkill")]
    public void AddTempSkill()
    {
        var skill = new ProjectileSkill(fire);
        skillManager.SkillSelectionSystem.AddSkill(priority++, skill);

        var skill1 = new ProjectileSkill(ice);
        skillManager.SkillSelectionSystem.AddSkill(priority++, skill1);

        var skill2 = new ProjectileSkill(electicity);
        skillManager.SkillSelectionSystem.AddSkill(priority++, skill2);
    }

}
