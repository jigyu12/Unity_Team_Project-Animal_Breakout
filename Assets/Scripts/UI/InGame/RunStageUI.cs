using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RunStageUI : UIElement
{
    [SerializeField] private Slider BossWaySlider;

    [SerializeField] MoveForward moveForward;
    [SerializeField] private GameObject panel;
    public float total = 180f;
    private bool tracking = false;

    public void StartBossWayTracking()
    {

        BossWaySlider.value = 0f;
        tracking = true;
    }

    public void StopBossWayTracking()
    {
        tracking = false;
    }
    public void Reset()
    {
        moveForward.moveForwardSum = 0;
    }
    public override void Show()
    {
        panel.SetActive(true);
    }
    public void Hide()
    {
        panel.SetActive(false);
    }
    void Update()
    {
        if (!tracking || moveForward == null)
            return;

        float current = moveForward.moveForwardSum;

        float progress = Mathf.Clamp01(current / total);
        BossWaySlider.value = progress;

    }
    public void SetTotalByRoadWayCount(int roadWayCount)
    {
        total = roadWayCount * 60f + 60f;
    }

}
