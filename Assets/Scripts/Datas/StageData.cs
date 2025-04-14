using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "InGameAssets/Stage Data")]
public class StageData : ScriptableObject
{
    public bool isBossStage;
    public RoadMakeMode roadMode;
    public ItemSetMode itemSetMode;
    public int roadWayCount;
}
