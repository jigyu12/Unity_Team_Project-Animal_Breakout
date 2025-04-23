using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField]
    private Image skillTypeIcon;

    [SerializeField]
    private Image skillIcon;

    //임시로 이렇게 작업함
    [SerializeField]
    private Sprite skillTypeAttackIcon;

    [SerializeField]
    private Sprite skillTypeSupportIcon;

    [SerializeField]
    private TextMeshProUGUI skillNameText;

    [SerializeField]
    private TextMeshProUGUI skillDescriptionText;

    public void UpdateSkillButton(SkillData skillData)
    {
        if(skillData.skillType==SkillType.Attack)
        {
            skillTypeIcon.sprite = skillTypeAttackIcon;
        }
        else if(skillData.skillType == SkillType.Support)
        {
            skillTypeIcon.sprite = skillTypeSupportIcon;
        }

        skillNameText.text = skillData.skillID.ToString();
    }

}
