using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileSkill : ISkill
{
    public ProjectileSkill(ProjectileSkillData data)
    {
        ProjectileSkillData = data;
        CoolTime = data.coolDownTime;
    }

    public SkillData SkillData
    {
        get => ProjectileSkillData;
    }

    public ProjectileSkillData ProjectileSkillData
    {
        get;
        private set;
    }

    public bool IsReady
    {
        get => (CoolTime >= SkillData.coolDownTime);
    }


    public float CoolTime
    {
        //get => SkillData.coolDownTime - Mathf.Clamp((Time.time - lastPerformedTime), 0, SkillData.coolDownTime);
        get;
        private set;
    }
    public float CoolDownRemaining
    {
        get => SkillData.coolDownTime - CoolTime;
    }

    public float CoolTimeRatio
    {
        get => Mathf.Clamp01(CoolTime / SkillData.coolDownTime);
    }

    public int Id
    {
        get => SkillData.skillID;
    }
    public int Level
    {
        get => SkillData.level;
    }


    private float lastPerformedTime = 0;
    private Action onReady;

    private SkillManager skillManager;

    public void InitializeSkilManager(SkillManager skillManager)
    {
        this.skillManager = skillManager;
    }

    public void Perform(Transform attackerTrs, Transform targetTrs, IAttacker attacker, DamageableStatus target)
    {
        //DoSomeThing;
        var projectile = UnityEngine.Object.Instantiate(ProjectileSkillData.projectileBehaviourPrefab.gameObject, skillManager.transform).GetComponent<ProjectileBehaviour>();
        projectile.InitializeSkilManager(skillManager);

        projectile.onArrival += () => ApplyDamage(attacker, target);
        projectile.Fire(attackerTrs, targetTrs, ProjectileSkillData.speed);
       
        lastPerformedTime = Time.time;
        CoolTime = 0f;
    }

    public void ApplyDamage(IAttacker attacker, DamageableStatus target)
    {
        if (!skillManager.IsSkillTargetValid())
        {
            return;
        }
        ApplyElementalEffect(target, ProjectileSkillData.skillElemental);

        //임시
        target.OnDamage(attacker.AttackPower * ProjectileSkillData.damageRate, ProjectileSkillData.skillElemental);
    }

    public void ApplyElementalEffect(DamageableStatus target, SkillElemental elemental)
    {
        switch (elemental)
        {
            case SkillElemental.Fire:
                {
                    target.gameObject.GetComponent<BurnStatusEffect>().Perform();
                    break;
                }
            case SkillElemental.Ice:
                {
                    target.gameObject.GetComponent<FrozenStatusEffect>().Perform();
                    break;
                }
            case SkillElemental.Electricity:
                {
                    target.gameObject.GetComponent<ElectricStatusEffect>().Perform();
                    break;
                }
        }
    }

    public void UpgradeLevel()
    {
        // Level++;

        //?좎룞?쇿뜝?숈삕 clamp?좎뙥怨ㅼ삕
    }

    public void UpdateCoolTime()
    {
        if (CoolTimeRatio < 1f)
        {
            CoolTime += Time.deltaTime;
            CoolTime = Mathf.Clamp(CoolTime, 0, SkillData.coolDownTime);

            if (CoolTimeRatio >= 1f)
            {
                OnReady();
            }
        }
    }

    public void AddOnReadyAction(Action onReady)
    {
        this.onReady += onReady;
    }

    public void OnReady()
    {
        onReady?.Invoke();
    }
}
