using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

//작업중...
public class StageManager : InGameManager
{
    //더미
    public class StageData
    {
        //public bool isBossStage;
        public RoadMakeMode roadMode;
        public ItemSetMode itemSetMode;
        public int roadWayCount;
    }

    [ReadOnly]
    public int currentStageDataIndex=0;

    private List<StageData> stageDatas = new List<StageData>();

    private void SetDummyData()
    {
        stageDatas.Add(new StageData { roadMode = RoadMakeMode.RandomWay, itemSetMode = ItemSetMode.TrapAndReward, roadWayCount = 8 });
        stageDatas.Add(new StageData { roadMode = RoadMakeMode.Vertical, itemSetMode = ItemSetMode.None, roadWayCount = -1 });
    }

    public override void Initialize()
    {
        base.Initialize();

        SetDummyData();
        GameManager.RoadMaker.onCurrentRoadWayEmpty += OnSetRoadMode;
    }

    public void SetInitialRoadMode()
    {
        var currStageData = stageDatas[currentStageDataIndex];
        GameManager.RoadMaker.SetRoadMakeMode(currStageData.roadMode, currStageData.roadWayCount);
        GameManager.RoadMaker.SetMapObjectMakeMode(currStageData.itemSetMode);
    }

    private void OnSetRoadMode()
    {
        currentStageDataIndex++;

        var currStageData = stageDatas[currentStageDataIndex];
        GameManager.RoadMaker.SetRoadMakeMode(currStageData.roadMode, currStageData.roadWayCount);
        GameManager.RoadMaker.SetMapObjectMakeMode(currStageData.itemSetMode);
    }
}
