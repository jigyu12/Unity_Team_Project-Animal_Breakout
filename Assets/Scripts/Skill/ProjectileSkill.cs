using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileSkill : AttackSkill
{
    public ProjectileSkill(ProjectileSkillData data)
    {
        ProjectileSkillData = data;
        CoolTime = data.coolDownTime;
    }

    public override AttackSkillData AttackSkillData
    {
        get => ProjectileSkillData;
    }

    public ProjectileSkillData ProjectileSkillData
    {
        get;
        private set;
    }

    private float lastPerformedTime = 0;


    public override void Perform(Transform attackerTrs, Transform targetTrs, IAttacker attacker, DamageableStatus target)
    {
        //간격이라던지 그런건 일단 차치하고 작성
        for (int i = 0; i < ProjectileSkillData.projectileCount; i++)
        {
            var projectile = UnityEngine.Object.Instantiate(ProjectileSkillData.projectileBehaviourPrefab.gameObject, skillManager.transform).GetComponent<ProjectileBehaviour>();
            projectile.InitializeSkilManager(skillManager);
            if (i != ProjectileSkillData.elementalEffectAttackIndex)
            {
                projectile.onArrival += () => ApplyOnlyDamage(attacker, target, ProjectileSkillData.attackCount);
            }
            else
            {
                projectile.onArrival += () => ApplyDamageAndElementalEffect(attacker, target, ProjectileSkillData.attackCount);
            }
            projectile.Fire(attackerTrs, targetTrs, ProjectileSkillData.speed);
        }

        lastPerformedTime = Time.time;
        CoolTime = 0f;
    }

    public void ApplyOnlyDamage(IAttacker attacker, DamageableStatus target, int count)
    {
        if (!skillManager.IsSkillTargetValid())
        {
            return;
        }

        for (int i = 0; i < count; i++)
        {
            AttackDamage(attacker.AttackPower * ProjectileSkillData.damageRate, target);
        }
    }

    public void ApplyDamageAndElementalEffect(IAttacker attacker, DamageableStatus target, int count)
    {
        if (!skillManager.IsSkillTargetValid())
        {
            return;
        }

        ApplyElementalEffect(target, ProjectileSkillData.skillElemental);
        for (int i = 0; i < count; i++)
        {
            AttackDamage(attacker.AttackPower * ProjectileSkillData.damageRate, target);
        }
    }
}
