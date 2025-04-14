using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private Image fillTimerImage;

    private ISkill targetSkill;

    private void Update()
    {
        UpdateFillTimer(targetSkill.CoolTimeRatio);
    }

    public void SetTargetSkill(ISkill skill)
    {
        targetSkill = skill;
    }

    private void UpdateFillTimer(float ratio)
    {
        timerText.text = Mathf.FloorToInt(ratio * 100f).ToString();
        fillTimerImage.fillAmount = 1f - ratio;
    }
}
