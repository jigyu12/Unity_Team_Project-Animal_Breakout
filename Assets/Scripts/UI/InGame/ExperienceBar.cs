using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceBar : MonoBehaviour
{
    public ExperienceStatus experienceStatus;

    private GageBar gageBar;

    private void Start()
    {
        gageBar = GetComponent<GageBar>();

        experienceStatus.onAddValue += UpdateExperienceValue;
        experienceStatus.onLevelChange += UpdateExperienceLevel;
    }

    private void UpdateExperienceValue(int value, int sum)
    {
        gageBar.SetValue(sum);
    }

    private void UpdateExperienceLevel(int level, int maxValue)
    {
        if (experienceStatus.IsMaxLevel)
        {
            gageBar.SetValue(maxValue);
            gageBar.SetMaxValue(maxValue);
            gageBar.SetText("MAX");
        }
        else
        {
            gageBar.SetMaxValue(maxValue);
        }
    }

}
