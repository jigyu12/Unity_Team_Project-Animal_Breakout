using System;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionBehaviourFactory
{
    private static readonly Dictionary<TrapType, Func<ICollisionBehaviour>> TrapBehaviours =
        new()
        {
            { TrapType.Bomb, () => new BombCollisionBehaviour() },
            { TrapType.Hole, () => new HoleCollisionBehaviour() },
        };
    
    private static readonly Dictionary<HumanItemType, Func<ICollisionBehaviour>> ItemHumanBehaviours =
        new()
        {
            { HumanItemType.JuniorResearcher, () => new JuniorResearcherCollisionBehaviour() },
            { HumanItemType.Researcher, () => new ResearcherCollisionBehaviour() },
            { HumanItemType.SeniorResearcher, () => new SeniorResearcherCollisionBehaviour() },
        };
    
    private static readonly Dictionary<PenaltyCoinItemType, Func<ICollisionBehaviour>> ItemPenaltyCoinBehaviours =
        new()
        {
            { PenaltyCoinItemType.GhostCoin, () => new GhostCoinCollisionBehaviour() },
            { PenaltyCoinItemType.PoisonCoin, () => new PoisonCoinCollisionBehaviour() },
            { PenaltyCoinItemType.SkullCoin, () => new SkullCoinCollisionBehaviour() },
            { PenaltyCoinItemType.FireCoin, () => new FireCoinCollisionBehaviour() },
            { PenaltyCoinItemType.BlackHoleCoin, () => new BlackHoleCoinCollisionBehaviour() },
        };
    
    private static readonly Dictionary<RewardCoinItemType, Func<ICollisionBehaviour>> ItemRewardCoinBehaviours =
        new()
        {
            { RewardCoinItemType.BronzeCoin, () => new BronzeCoinCollisionBehaviour() },
            { RewardCoinItemType.SilverCoin, () => new SilverCoinCollisionBehaviour() },
            { RewardCoinItemType.GoldCoin, () => new GoldCoinCollisionBehaviour() },
            { RewardCoinItemType.PlatinumCoin, () => new PlatinumCoinCollisionBehaviour() },
            { RewardCoinItemType.DiamondCoin, () => new DiamondCoinCollisionBehaviour() },
        };

    public static ICollisionBehaviour GetTrapBehaviour(TrapType trapType)
    {
        if (TrapBehaviours.TryGetValue(trapType, out var behaviour))
        {
            return behaviour();
        }

        Debug.Assert(false, $"Cant find trapBehaviour in TrapType: {trapType}");
        
        return null;
    }
    
    public static ICollisionBehaviour GetHumanBehaviour(HumanItemType humanItemType)
    {
        if (ItemHumanBehaviours.TryGetValue(humanItemType, out var behaviour))
        {
            return behaviour();
        }

        Debug.Assert(false, $"Cant find ItemHumanBehaviour in HumanItemType: {humanItemType}");
        
        return null;
    }
    
    public static ICollisionBehaviour GetPenaltyCoinBehaviour(PenaltyCoinItemType penaltyCoinType)
    {
        if (ItemPenaltyCoinBehaviours.TryGetValue(penaltyCoinType, out var behaviour))
        {
            return behaviour();
        }

        Debug.Assert(false, $"Cant find ItemPenaltyCoinBehaviour in PenaltyCoinType: {penaltyCoinType}");
        
        return null;
    }
    
    public static ICollisionBehaviour GetRewardCoinBehaviour(RewardCoinItemType rewardCoinType)
    {
        if (ItemRewardCoinBehaviours.TryGetValue(rewardCoinType, out var behaviour))
        {
            return behaviour();
        }

        Debug.Assert(false, $"Cant find ItemRewardCoinBehaviour in RewardCoinType: {rewardCoinType}");
        
        return null;
    }
}