using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManagerTest : MonoBehaviour
{
    [SerializeField]
    private ProjectileSkillData tempProjectile;

    [SerializeField]
    private SkillManager skillManager;

    private int priority = 0;

    [ContextMenu("AddSkill")]
    public void AddTempSkill()
    {
        var skill = new ProjectileSkill(tempProjectile);
        skillManager.AddSkill(priority++, skill);
    }

}
