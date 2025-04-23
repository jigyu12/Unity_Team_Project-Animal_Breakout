using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingTime : MonoBehaviour
{
    public float PlayTime { get; private set; }
    private bool isTracking = false;
    private void Awake()
    {
        StartTracking();
    }
    public void StartTracking()
    {
        PlayTime = 0f;
        isTracking = true;
    }

    public void StopTracking()
    {
        isTracking = false;
    }

    private void Update()
    {
        if (isTracking)
            PlayTime += Time.deltaTime;
    }
    // UI용 포맷 함수
    public string GetFormattedPlayTime()
    {
        int minutes = (int)(PlayTime / 60);
        int seconds = (int)(PlayTime % 60);
        return $"플레이 시간: {minutes}분 {seconds}초";
    }
}
