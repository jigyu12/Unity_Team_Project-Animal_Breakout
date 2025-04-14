using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIconListUI : MonoBehaviour
{
    //임시로 스킬매니저에서 받아온다
    [SerializeField]
    private SkillManager skillManager;

    private int maxSkillCount;

    [SerializeField]
    private GameObject skillIconPrefab;

    private List<SkillIcon> skillIcons = new();

    private void Start()
    {
        maxSkillCount = skillManager.MaxSkillCount;
        for (int i = 0; i < maxSkillCount; i++)
        {
            var skillIcon = Instantiate(skillIconPrefab, transform).GetComponent<SkillIcon>();
            skillIcon.gameObject.SetActive(false);
            skillIcons.Add(skillIcon);
        }

        skillManager.onSkillListUpdated += OnSkillListUpdated;
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
