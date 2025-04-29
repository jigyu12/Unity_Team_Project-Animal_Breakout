using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackSkill : ISkill
{
    public SkillData SkillData
    {
        get => AttackSkillData;
    }

    public AttackSkillData AttackSkillData
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
        get => AttackSkillData.coolDownTime - AttackSkillData.coolDownTime * skillManager.GlobalCoolDownTimeRate;
    }

    #endregion

    protected Action onReady;

    public int Id
    {
        get => SkillData.skillID;
    }

    public string SkillGroup
    {
        get => SkillData.skillGroup;
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

    public abstract void Perform(AttackPowerStatus attacker, DamageableStatus target, Transform start = null, Transform destination = null);
    public abstract IEnumerator coPerform(AttackPowerStatus attacker, DamageableStatus target, Transform start = null, Transform destination = null);

    protected virtual void AttackDamage(float damage, DamageableStatus target)
    {
        target.OnDamage(damage, AttackSkillData.skillElemental);
    }

    protected void ApplyElementalEffect(AttackPowerStatus attacker, DamageableStatus target, SkillElemental elemental)
    {
        var ui = skillManager.gameManager.UIManager.bossDebuffUI;
        string debuffId = null;
        Color textColor = Color.white;
        switch (elemental)
        {

            case SkillElemental.Fire:
                {
                    var burn = target.gameObject.GetComponent<BurnStatusEffect>();
                    burn?.Perform(Id, attacker.GetElementalAdditionalAttackPower(SkillElemental.Fire));
                    textColor = Color.red;
                    burn?.SetDebuffUI(ui);
                    if (burn.IsPerforming)
                    {
                        debuffId = "Burn";
                    }
                    break;
                }
            case SkillElemental.Ice:
                {
                    var freeze = target.GetComponent<FrozenStatusEffect>();
                    freeze?.Perform(Id, attacker.GetElementalAdditionalAttackPower(SkillElemental.Ice));
                    freeze?.SetDebuffUI(ui);
                    textColor = Color.cyan;
                    debuffId = "Freeze";
                    break;
                }
            case SkillElemental.Thunder:
                {
                    var shock = target.GetComponent<ElectricShockStatusEffect>();
                    shock?.Perform(Id, attacker.GetElementalAdditionalAttackPower(SkillElemental.Thunder));
                    shock?.SetDebuffUI(ui);
                    textColor = new Color(0.6f, 0f, 1f);
                    debuffId = "Thunder";
                    break;
                }
        }
        if (!string.IsNullOrEmpty(debuffId))
        {
            ShowDebuffIcon(debuffId, SkillData.iconImage);
            skillManager.gameManager.DamageTextManager.ShowDamage(target.transform.position, attacker.GetElementalAdditionalAttackPower(elemental), textColor);
        }
    }

    private void ShowDebuffIcon(string debuffId, Sprite icon)
    {
        var gameManager = skillManager.gameManager;
        if (gameManager != null && gameManager.StageManager.IsPlayerInBossStage)
        {
            gameManager.UIManager.bossDebuffUI.AddDebuff(debuffId);
        }
    }



    public void Update()
    {
        UpdateCoolTime();
    }

    public void UpgradeLevel()
    {
        var nextSkillData = skillManager.SkillFactory.GetSkillData(SkillData.skillType, SkillData.skillGroup, Level + 1);
        AttackSkillData = nextSkillData as AttackSkillData;
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

    public void ApplyOnlyDamage(AttackPowerStatus attacker, DamageableStatus target, int count)
    {
        if (!skillManager.IsSkillTargetValid())
        {
            return;
        }

        for (int i = 0; i < count; i++)
        {
            AttackDamage(attacker.GetElementalAdditionalAttackPower(AttackSkillData.skillElemental) * AttackSkillData.damageRate, target);
        }
    }

    public void ApplyDamageAndElementalEffect(AttackPowerStatus attacker, DamageableStatus target, int count)
    {
        if (!skillManager.IsSkillTargetValid())
        {
            return;
        }

        ApplyElementalEffect(attacker, target, AttackSkillData.skillElemental);
        for (int i = 0; i < count; i++)
        {
            AttackDamage(attacker.GetElementalAdditionalAttackPower(AttackSkillData.skillElemental) * AttackSkillData.damageRate, target);
        }
    }
}
