using UnityEngine;

public class ItemRewardCoin : ItemBase
{
    public override ObjectType ObjectType { get; protected set; }
    protected override ICollisionBehaviour CollisionBehaviour { get; set; }
    
    public override ItemType ItemType { get; protected set; }
    
    public RewardCoinItemType RewardCoinItemType { get; private set; } = RewardCoinItemType.None;
    
    public void Init(RewardCoinItemType rewardCoinItemType)
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
        
        // ToDo : Load Asset
        TryGetComponent(out MeshRenderer meshRenderer);
        switch (rewardCoinItemType)
        {
            case RewardCoinItemType.BronzeCoin:
                meshRenderer.material.color = new Color(0.8f, 0.5f, 0.2f);
                break;
            case RewardCoinItemType.SilverCoin:
                meshRenderer.material.color = Color.gray;
                break;
            case RewardCoinItemType.GoldCoin:
                meshRenderer.material.color = Color.yellow;
                break;
            case RewardCoinItemType.PlatinumCoin:
                meshRenderer.material.color = new Color(0.65f, 0.77f, 0.85f);
                break;
            case RewardCoinItemType.DiamondCoin:
                meshRenderer.material.color = new Color(0.6f, 1f, 1f);
                break;
        }

    }
}