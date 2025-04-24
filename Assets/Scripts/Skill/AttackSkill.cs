using System;
using UnityEngine;

public abstract class AttackSkill : ISkill
{
    public SkillData SkillData
    {
        get=> AttackSkillData;
    }

    public  AttackSkillData AttackSkillData
    {
        get;
        private set;
    }

    public bool IsReady
    {
        get => (CoolTime >= AttackSkillData.coolDownTime);
    }

    #region cooldownTime

    public float CoolTime
    {
        get;
        protected set;
    }

    public float CoolDownRemaining
    {
        get => CoolDownTime - CoolTime;
    }

    public float CoolTimeRatio
    {
        get => Mathf.Clamp01(CoolTime / CoolDownTime);
    }

    public float CoolDownTime
    {
        get=> AttackSkillData.coolDownTime - AttackSkillData.coolDownTime * skillManager.GlobalCoolDownTimeRate;
    }

    #endregion

    protected Action onReady;

    public int Id
    {
        get => SkillData.skillID;
    }

    public int Level
    {
        get => SkillData.level;
    }

    protected SkillManager skillManager;
    public void InitializeSkilManager(SkillManager skillManager)
    {
        this.skillManager = skillManager;
        CoolTime = CoolDownTime;
    }

    public AttackSkill(AttackSkillData attackSkillData)
    {
        AttackSkillData = attackSkillData;
    }

    public abstract void Perform(Transform attackerTrs, Transform targetTrs, AttackPowerStatus attacker, DamageableStatus target);

    protected virtual void AttackDamage(float damage, DamageableStatus target)
    {
        target.OnDamage(damage, AttackSkillData.skillElemental);
    }

    protected void ApplyElementalEffect(DamageableStatus target, SkillElemental elemental)
    {
        switch (elemental)
        {
            case SkillElemental.Fire:
                {
                    target.gameObject.GetComponent<BurnStatusEffect>().Perform(Id);
                    break;
                }
            case SkillElemental.Ice:
                {
                    target.gameObject.GetComponent<FrozenStatusEffect>().Perform(Id);
                    break;
                }
            case SkillElemental.Thunder:
                {
                    target.gameObject.GetComponent<ElectricShockStatusEffect>().Perform(Id);
                    break;
                }
        }
    }
    public void Update()
    {
        UpdateCoolTime();
    }

    public void UpgradeLevel()
    {
        //Data set을 교체해야함
    }

    private void UpdateCoolTime()
    {
        if (CoolTimeRatio < 1f)
        {
            CoolTime += Time.deltaTime;

            if (CoolTimeRatio >= 1f)
            {
                CoolTime = Mathf.Clamp(CoolTime, 0, AttackSkillData.coolDownTime);
                OnReady();
            }
        }
    }

    public virtual void AddOnReadyAction(Action onReady)
    {
        this.onReady += onReady;
    }

    public virtual void OnReady()
    {
        onReady?.Invoke();
    }
}
