using System;
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

    private string skillNameFormat = "{0} LV{1}";

    [SerializeField]
    private TextMeshProUGUI skillDescriptionText;

    [SerializeField]
    private Button button;

    private Action onSelected;

    public void InitializeButtonAction(Action onClick)
    {
        onSelected = onClick;
        button.onClick.AddListener(OnSelected);
    }

    public void UpdateSkillButton(SkillData skillData)
    {
        if (skillData.skillType == SkillType.Attack)
        {
            skillTypeIcon.sprite = skillTypeAttackIcon;
        }
        else if (skillData.skillType == SkillType.Support)
        {
            skillTypeIcon.sprite = skillTypeSupportIcon;
        }

        skillNameText.text =   string.Format(skillNameFormat, skillData.skillID, skillData.level);
        skillIcon.sprite = skillData.iconImage;

        skillDescriptionText.text = LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, skillData.descriptionID);
    }

    private void OnSelected()
    {
        onSelected?.Invoke();
    }
}
