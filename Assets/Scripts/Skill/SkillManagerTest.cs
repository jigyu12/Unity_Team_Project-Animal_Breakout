using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManagerTest : MonoBehaviour
{
    [SerializeField]
    private GameObject skillPrefab;

    [SerializeField]
    private SkillManager skillManager;

    private int priority = 0;

    [ContextMenu("AddSkill")]
    public void AddTempSkill()
    {
        var skill = Instantiate(skillPrefab).GetComponent<ProjectileSkill>();
        skillManager.AddSkill(priority++, skill);
    }

}
