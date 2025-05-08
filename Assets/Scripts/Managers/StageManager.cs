using System;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : InGameManager
{
    public bool IsPlayerInBossStage
    {
        get;
        private set;
    }


    [ReadOnly]
    public int currentStageDataIndex = 0;

    public StageData CurrentStageData
    {
        get => stageDatas[currentStageDataIndex];
    }

    [SerializeField]
    private List<StageData> stageDatas = new List<StageData>();

    public Action onBossStageEnter;
    private bool isTrackingStarted = false;

    private int bossStageSetCount;
    public event Action<RunPhaseType> onBossStageSet;
    private bool isFirstBossSpawn;
    private int stagePlayCount = 0;

    private void Awake()
    {
        BossStatus.onBossDeathAnimationEnded += OnBossStageClear;

        onBossStageEnter += () => IsPlayerInBossStage = true;

        bossStageSetCount = 0;
        isFirstBossSpawn = true;
    }

    private void OnDestroy()
    {
        BossStatus.onBossDeathAnimationEnded -= OnBossStageClear;
    }

    public override void Initialize()
    {
        base.Initialize();
        GameManager.RoadMaker.onCurrentRoadWayEmpty += OnSetRoadMode;

    }

    public void SetInitialRoadMode()
    {
        GameManager.RoadMaker.PushNextStageRoadWayData(CurrentStageData);
    }


    //?뚯뒪?몄슜?쇰줈 臾댄븳 諛섎났?섍쾶 ?좉쾬?낅땲??
    private void OnSetRoadMode()
    {
        currentStageDataIndex++;
        //currentStageDataIndex %= stageDatas.Count;

        if (currentStageDataIndex == stageDatas.Count)
        {
            currentStageDataIndex -= 2;
        }

        if (CurrentStageData.isBossStage)
        {
            ++bossStageSetCount;
        }

        switch (bossStageSetCount)
        {
            case < 2:
                {
                    onBossStageSet?.Invoke(RunPhaseType.EarlyPhase);
                }
                break;
            case < 4:
                {
                    onBossStageSet?.Invoke(RunPhaseType.MiddlePhase);
                }
                break;
            case < 6:
                {
                    onBossStageSet?.Invoke(RunPhaseType.LateMiddlePhase);
                }
                break;
            case < Int32.MaxValue:
                {
                    onBossStageSet?.Invoke(RunPhaseType.LatePhase);
                }
                break;
        }

        if (isFirstBossSpawn && CurrentStageData.isBossStage)
        {
            //GameManager.UIManager.runStageUI.SetTotalByRoadWayCount(CurrentStageData.roadWayCount); //추가
            GameManager.RoadMaker.PushNextStageRoadWayData(CurrentStageData);

            currentStageDataIndex = -1;

            isFirstBossSpawn = false;

            return;
        }

        Debug.Log("Stage " + currentStageDataIndex + "is boss stage " + CurrentStageData.isBossStage);

        var currStageData = stageDatas[currentStageDataIndex];
        GameManager.RoadMaker.PushNextStageRoadWayData(CurrentStageData);
        // StartCoroutine(WaitForBossRoadwayAndTrack());
    }


    public void RoadWayDistanceTracking()
    {
        if (isTrackingStarted) return; // 이미 시작했으면 무시
        isTrackingStarted = true;

        GameManager.UIManager.runStageUI.Reset();
        // int nonBossCount = GameManager.RoadMaker.GetNonBossRoadWayCountFromStageData(CurrentStageData);
        // GameManager.UIManager.runStageUI.SetTotalByRoadWayCount(nonBossCount);
        GameManager.UIManager.runStageUI.SetTotalByRoadWayCount(CurrentStageData.roadWayCount, stagePlayCount);
        GameManager.UIManager.runStageUI.StartBossWayTracking();
        GameManager.UIManager.runStageUI.Show();
        GameManager.UIManager.bossWayUI.Hide();
    }


    [ContextMenu("Boss Stage Exit")]
    private void OnCurrentStageClear()
    {
        OnSetRoadMode();
    }

    public void OnBossStageEnter()
    {
        Debug.Log("Boss Stage Enter");
        GameManager.UIManager.runStageUI.StopBossWayTracking(); //추가
        GameManager.UIManager.runStageUI.Hide();
        GameManager.UIManager.bossWayUI.Show();
        GameManager.PlayerManager.StopAllMovements();
        GameManager.UIManager.bossTimeLimit.StartTimeOut();
        stagePlayCount++;
        onBossStageEnter?.Invoke();
    }

    public void OnBossStageClear()
    {
        IsPlayerInBossStage = false;
        GameManager.PlayerManager.ActivatePlayer();
        GameManager.UIManager.bossWayUI.Hide();
        GameManager.UIManager.runStageUI.Show();
        isTrackingStarted = false;
        RoadWayDistanceTracking();
        // GameManager.UIManager.runStageUI.total += 60f;
        GameManager.UIManager.bossTimeLimit.StopTimeOut();
        OnCurrentStageClear();
        GameManager.PlayerManager.moveForward.AddSpeed(1f);
    }
}