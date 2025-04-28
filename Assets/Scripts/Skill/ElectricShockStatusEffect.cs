using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricShockStatusEffect : StatusEffect
{
    private readonly int effectId = 1603;

    [SerializeField]
    private SkillData effectSkillData;
    private ISkill effectSkill;
    private DamageableStatus target;

    public int stackMaxCount = 6;
    private int currentStackCount;


    public override bool CanPerform
    {
        get => !isPerforming;
    }

    public override bool IsPerforming
    {
        get => isPerforming;
    }
    private bool isPerforming;

    private DebuffIcon debuffIcon;

    private void Start()
    {
        SetEffectData(effectId, SkillElemental.Thunder);
        SetDamagerableTarget(GetComponent<DamageableStatus>());
        isPerforming = false;
        currentStackCount = 0;
    }

    public override void InitializeSkillManager(SkillManager skillManager)
    {
        base.InitializeSkillManager(skillManager);
        effectSkill = skillManager.SkillFactory.CreateSkill(effectSkillData);
        effectSkill.InitializeSkilManager(skillManager);
    }

    //public void SetEffectSkill(ISkill skill)
    //{
    //    effectSkill = skill;
    //}

    public override void SetDamagerableTarget(DamageableStatus damageable)
    {
        target = damageable;
        target.onElementalDamaged += AddElectricStack;
    }

    //스택은 번개 속성 스킬을 맞췄을 때 1스택씩 쌓이고 6스택을 쌓으면 번개 속성 타겟팅 추가 스킬이 발동된다.
    public override void Perform(int skillID, int elementalAttackPower)
    {
        if (CanPerform)
        {
            isPerforming = true;
            currentStackCount = 0;
            // debuffIcon = debuffUI?.AddDebuff("Thunder");
            // debuffIcon?.UpdateCountText(currentStackCount);
        }
    }

    private void AddElectricStack(float damage, SkillElemental elemental)
    {
        if (elemental != Elemental || !IsPerforming)
        {
            return;
        }

        currentStackCount++;
        //  스택 올라갈 때마다 텍스트 갱신
        var gameManager = skillManager.gameManager;
        var debuffIcon = gameManager.UIManager.bossDebuffUI.AddDebuff("Thunder"); // 이미 있는 거 가져옴
        debuffIcon?.UpdateCountText(currentStackCount);
        Debug.Log($"전기스택 {currentStackCount}번째");

        if (currentStackCount >= stackMaxCount)
        {
            PerformEffectSkill();
        }
    }

    private void PerformEffectSkill()
    {
        isPerforming = false;
        skillManager.PerformSkill(effectSkill);

        debuffUI?.RemoveDebuff("Thunder");
    }

}
