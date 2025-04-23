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


    //임시로 확인용
    [SerializeField]
    private List<SkillData> skillDatas = new();

    public override void Initialize()
    {
        base.Initialize();

        for (int i = 0; i < skillButtonCount; i++)
        {
            var skillButton = Instantiate(skillButtonPrefab, skillListGameObject.transform).GetComponent<SkillButton>();
            skillButtons.Add(skillButton);
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);    
    }

    public void OnShowSkillSelectionPanel()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < skillButtons.Count; i++)
        {
            skillButtons[i].UpdateSkillButton(skillDatas[i]);
        }
    }
}
