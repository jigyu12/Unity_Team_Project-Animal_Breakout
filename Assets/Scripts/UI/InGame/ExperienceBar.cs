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

        experienceStatus.InitializeValue();
    }

    private void UpdateExperienceValue(int value, int sum)
    {
        gageBar.SetValue(sum);

        if (experienceStatus.IsMaxLevel)
        {
            gageBar.SetMaxValue(1);
            gageBar.SetValue(1);
            gageBar.SetText("MAX");
        }
    }

    private void UpdateExperienceLevel(int level, int maxValue)
    {
        gageBar.SetMaxValue(maxValue);
    }

}
