using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIconListUI : UIElement
{
    private int maxSkillCount;

    [SerializeField]
    private GameObject skillIconPrefab;

    private List<SkillIcon> skillIcons = new();

    public override void Initialize()
    {
        base.Initialize();

        maxSkillCount = gameManager.SkillManager.MaxSkillCount;
        gameManager.SkillManager.SkillSelectionSystem.onSkillListUpdated += OnSkillListUpdated;

        for (int i = 0; i < maxSkillCount; i++)
        {
            var skillIcon = Instantiate(skillIconPrefab, transform).GetComponent<SkillIcon>();
            skillIcon.gameObject.SetActive(false);
            skillIcons.Add(skillIcon);
        }
    }

    private void OnSkillListUpdated(List<ISkill> skills)
    {
        gameUIManager.supportSlotUI.ClearSlots();
        gameUIManager.attackSlotUI.ClearSlots();

        int skillIconIndex = 0;

        for (int i = 0; i < skills.Count; i++)
        {
            var skill = skills[i];

            // 공격/서포트 둘 다 사이드바에 표시
            skillIcons[skillIconIndex].SetTargetSkill(skill);
            skillIcons[skillIconIndex].gameObject.SetActive(true);
            skillIconIndex++;
            if (skill.SkillData.skillType == SkillType.Attack)
            {
                gameUIManager.attackSlotUI.AddSupportSkill(skill.SkillData.iconImage);
            }
            if (skill.SkillData.skillType == SkillType.Support)
            {
                gameUIManager.supportSlotUI.AddSupportSkill(skill.SkillData.iconImage);
            }
        }

        // 남은 아이콘은 끄기
        for (int i = skillIconIndex; i < maxSkillCount; i++)
        {
            skillIcons[i].gameObject.SetActive(false);
        }
    }

    public bool TryGetIconSprite(int index, out Sprite sprite)
    {
        sprite = null;
        if (index >= 0 && index < skillIcons.Count && skillIcons[index] != null)
        {
            sprite = skillIcons[index].IconSprite;
            return true;
        }
        return false;
    }

}
