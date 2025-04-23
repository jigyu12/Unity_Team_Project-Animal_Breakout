using UnityEngine;

public class ItemRewardCoin : ItemBase
{
    public override ObjectType ObjectType { get; protected set; }
    protected override BaseCollisionBehaviour CollisionBehaviour { get; set; }

    public override ItemType ItemType { get; protected set; }

    public RewardCoinItemType RewardCoinItemType { get; private set; } = RewardCoinItemType.None;

    [SerializeField] private RewardCoinItemType rewardCoinItemType;
    private ItemDataTable.ItemData itemStatData;
    private InGameCountManager inGameCountManager;
    public int Score => itemStatData?.Score ?? 0;

    public void Initialize()
    {
        // if (rewardCoinItemType == RewardCoinItemType.None || rewardCoinItemType == RewardCoinItemType.Count)
        // {
        //     Debug.Assert(false, "Invalid RewardCoinItemType");

        //     return;
        // }

        ObjectType = ObjectType.Item;

        ItemType = ItemType.RewardCoin;

        itemStatData = DataTableManager.itemDataTable.Get((int)rewardCoinItemType);
        RewardCoinItemType = rewardCoinItemType;
        CollisionBehaviour = CollisionBehaviourFactory.GetRewardCoinBehaviour(rewardCoinItemType);
        CollisionBehaviour.SetScoreToAdd(itemStatData.Score);
    }
}