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
        for (int i = 0; i < skills.Count; i++)
        {
            skillIcons[i].SetTargetSkill(skills[i]);
            skillIcons[i].gameObject.SetActive(true);
        }

        for (int i = skills.Count; i < maxSkillCount; i++)
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
