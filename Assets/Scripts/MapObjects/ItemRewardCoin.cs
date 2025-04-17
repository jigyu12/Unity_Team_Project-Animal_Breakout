using UnityEngine;

public class ItemRewardCoin : ItemBase
{
    public override ObjectType ObjectType { get; protected set; }
    protected override BaseCollisionBehaviour CollisionBehaviour { get; set; }

    public override ItemType ItemType { get; protected set; }

    public RewardCoinItemType RewardCoinItemType { get; private set; } = RewardCoinItemType.None;

    private const long ScoreToAddBronzeCoin = 10;
    private const long ScoreToAddSilverCoin = 20;
    private const long ScoreToAddGoldCoin = 30;
    private const long ScoreToAddPlatinumCoin = 40;
    private const long ScoreToAddDiamondCoin = 50;

    [SerializeField] private int itemID;
    private ItemDataTable.ItemData itemStatData;

    public int Score => itemStatData?.Score ?? 0;

    public void Initialize(RewardCoinItemType rewardCoinItemType)
    {
        // if (rewardCoinItemType == RewardCoinItemType.None || rewardCoinItemType == RewardCoinItemType.Count)
        // {
        //     Debug.Assert(false, "Invalid RewardCoinItemType");

        //     return;
        // }

        ObjectType = ObjectType.Item;

        ItemType = ItemType.RewardCoin;

        // RewardCoinItemType = rewardCoinItemType;

        // CollisionBehaviour = CollisionBehaviourFactory.GetRewardCoinBehaviour(rewardCoinItemType);
        itemStatData = DataTableManager.itemDataTable.Get(itemID);
        RewardCoinItemType = (RewardCoinItemType)itemID;
        CollisionBehaviour = CollisionBehaviourFactory.GetRewardCoinBehaviour((RewardCoinItemType)itemID);
        CollisionBehaviour.SetScoreToAdd(itemStatData.Score);
        // switch (rewardCoinItemType)
        // {
        //     case RewardCoinItemType.BronzeCoin:
        //         {
        //             CollisionBehaviour.SetScoreToAdd(ScoreToAddBronzeCoin);
        //         }
        //         break;
        //     case RewardCoinItemType.SilverCoin:
        //         {
        //             CollisionBehaviour.SetScoreToAdd(ScoreToAddSilverCoin);
        //         }
        //         break;
        //     case RewardCoinItemType.GoldCoin:
        //         {
        //             CollisionBehaviour.SetScoreToAdd(ScoreToAddGoldCoin);
        //         }
        //         break;
        //     case RewardCoinItemType.PlatinumCoin:
        //         {
        //             CollisionBehaviour.SetScoreToAdd(ScoreToAddPlatinumCoin);
        //         }
        //         break;
        //     case RewardCoinItemType.DiamondCoin:
        //         {
        //             CollisionBehaviour.SetScoreToAdd(ScoreToAddDiamondCoin);
        //         }
        //         break;
        // }
    }
}