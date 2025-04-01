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
    
    public void Initialize(RewardCoinItemType rewardCoinItemType)
    {
        if (rewardCoinItemType == RewardCoinItemType.None || rewardCoinItemType == RewardCoinItemType.Count)
        {
            Debug.Assert(false, "Invalid RewardCoinItemType");
            
            return;
        }
        
        ObjectType = ObjectType.Item;
        
        ItemType = ItemType.RewardCoin;
        
        RewardCoinItemType = rewardCoinItemType;
        
        CollisionBehaviour = CollisionBehaviourFactory.GetRewardCoinBehaviour(rewardCoinItemType);
        
        // ToDo : Load Asset & Table
        TryGetComponent(out MeshRenderer meshRenderer);
        switch (rewardCoinItemType)
        {
            case RewardCoinItemType.BronzeCoin:
            {
                meshRenderer.material.color = new Color(0.8f, 0.5f, 0.2f);
                CollisionBehaviour.SetScoreToAdd(ScoreToAddBronzeCoin);
            }
                break;
            case RewardCoinItemType.SilverCoin:
            {
                meshRenderer.material.color = Color.gray;
                CollisionBehaviour.SetScoreToAdd(ScoreToAddSilverCoin);
            }
                break;
            case RewardCoinItemType.GoldCoin:
            {
                meshRenderer.material.color = Color.yellow;
                CollisionBehaviour.SetScoreToAdd(ScoreToAddGoldCoin);
            }
                break;
            case RewardCoinItemType.PlatinumCoin:
            {
                meshRenderer.material.color = new Color(0.65f, 0.77f, 0.85f);
                CollisionBehaviour.SetScoreToAdd(ScoreToAddPlatinumCoin);
            }
                break;
            case RewardCoinItemType.DiamondCoin:
            {
                meshRenderer.material.color = new Color(0.6f, 1f, 1f);
                CollisionBehaviour.SetScoreToAdd(ScoreToAddDiamondCoin);
            }
                break;
        }
    }
}