using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectionUI : UIElement
{
    private int skillButtonCount = 3;

    [SerializeField]
    private GameObject skillButtonPrefab;

    [SerializeField]
    private GameObject skillListGameObject;

    private List<SkillButton> skillButtons=new();
    private List<SkillData> skillDatas=new();

    private int priority = 1;

    public override void Initialize()
    {
        base.Initialize();

        for (int i = 0; i < skillButtonCount; i++)
        {
            var skillButton = Instantiate(skillButtonPrefab, skillListGameObject.transform).GetComponent<SkillButton>();
            skillButtons.Add(skillButton);

            skillButton.InitializeButtonAction(() =>
            {
                int index = i;
                SelectSkill(index);
            });
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);    
    }

    public override void Show()
    {
        base.Show();

        gameObject.SetActive(true);
        UpdateRandomSkillDatas();
    }

    private void UpdateRandomSkillDatas()
    {
        skillDatas.Clear();
        gameManager.SkillManager.SkillSelectionSystem.GetRandomSkillDatas(skillButtonCount, skillDatas);

        for (int i = 0; i < skillButtonCount; i++)
        {
            skillButtons[i].UpdateSkillButton(skillDatas[i]);
        }
    }

    private void SelectSkill(int index)
    {
        gameManager.SkillManager.SkillSelectionSystem.AddSkill(priority++, skillDatas[index]);
    }
}
