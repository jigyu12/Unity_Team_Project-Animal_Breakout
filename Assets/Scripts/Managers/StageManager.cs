using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class StageManager : InGameManager
{

    [ReadOnly]
    public int currentStageDataIndex = 0;

    public StageData CurrentStageData
    {
        get => stageDatas[currentStageDataIndex];
    }

    [SerializeField]
    private List<StageData> stageDatas = new List<StageData>();

    public Action onBossStageEnter;

    private void Awake()
    {
        BossStatus.onBossDead += OnCurrentStageClear;
    }

    private void OnDestroy()
    {
        BossStatus.onBossDead -= OnCurrentStageClear;
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


    //테스트용으로 무한 반복하게 할것입니다.
    private void OnSetRoadMode()
    {

        currentStageDataIndex++;
        currentStageDataIndex %= stageDatas.Count;

        Debug.Log("Stage " + currentStageDataIndex + "is boss stage " + CurrentStageData.isBossStage);

        var currStageData = stageDatas[currentStageDataIndex];
        GameManager.RoadMaker.PushNextStageRoadWayData(CurrentStageData);
    }

    [ContextMenu("Boss Stage Exit")]
    private void OnCurrentStageClear()
    {
        OnSetRoadMode();
    }

    public void OnBossStageEnter()
    {
        Debug.Log("Boss Stage Enter");
        onBossStageEnter?.Invoke();
    }
}