using System;
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

    private void Start()
    {
        //진짜 개최악코드 절대 수정할것임 진짜 잠깐 보여주는 용도임 김희정이 진짜 눈물흘리면서 임시로 쓴거임 한번만 봐주셈
        Invoke("AddTempSkill", 5f);
    }

    [ContextMenu("AddSkill")]
    public void AddTempSkill()
    {
        var skill = new ProjectileSkill(tempProjectile);
        skillManager.AddSkill(priority++, skill);
    }

}
