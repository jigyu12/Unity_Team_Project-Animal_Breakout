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
        gameManager.SkillManager.onSkillListUpdated += OnSkillListUpdated;
    }

    private void Start()
    {
        for (int i = 0; i < maxSkillCount; i++)
        {
            var skillIcon = Instantiate(skillIconPrefab, transform).GetComponent<SkillIcon>();
            skillIcon.gameObject.SetActive(false);
            skillIcons.Add(skillIcon);
        }
    }

    private void OnSkillListUpdated(List<SkillPriorityItem> skills)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            skillIcons[i].SetTargetSkill(skills[i].skill);
            skillIcons[i].gameObject.SetActive(true);
        }

        for (int i = skills.Count; i < maxSkillCount; i++)
        {
            skillIcons[i].gameObject.SetActive(false);
        }
    }
}
