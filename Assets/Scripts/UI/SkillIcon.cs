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
        UpdateFillTimer(targetSkill.CoolTimeRatio, targetSkill.CoolTime);
    }

    public void SetTargetSkill(ISkill skill)
    {
        targetSkill = skill;
    }

    //private static readonly string timeFormatString = $"{}";
    private void UpdateFillTimer(float ratio, float time)
    {
        if (time <= 0)
        {
            //숫자 0일때는 text안뜨게함
            timerText.gameObject.SetActive(false);
        }
        else
        {
            timerText.gameObject.SetActive(true);
            timerText.text = time.ToString("F1");

        }
        fillTimerImage.fillAmount = ratio;
    }
}
