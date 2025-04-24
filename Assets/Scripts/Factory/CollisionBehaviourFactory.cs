using System;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionBehaviourFactory
{
    private static readonly Dictionary<WallType, Func<BaseCollisionBehaviour>> WallBehaviours =
        new()
        {
            { WallType.NormalWall, () => new NormalWallCollisionBehaviour() },
            { WallType.ReinforcedWall, () => new ReinforcedWallCollisionBehaviour() },
        };
    
    private static readonly Dictionary<TrapType, Func<BaseCollisionBehaviour>> TrapBehaviours =
        new()
        {
            { TrapType.Bomb, () => new BombCollisionBehaviour() },
            { TrapType.Hole, () => new HoleCollisionBehaviour() },
        };
    
    private static readonly Dictionary<HumanItemType, Func<BaseCollisionBehaviour>> ItemHumanBehaviours =
        new()
        {
            { HumanItemType.JuniorResearcher, () => new JuniorResearcherCollisionBehaviour() },
            { HumanItemType.Researcher, () => new ResearcherCollisionBehaviour() },
            { HumanItemType.SeniorResearcher, () => new SeniorResearcherCollisionBehaviour() },
        };
    
    private static readonly Dictionary<PenaltyCoinItemType, Func<BaseCollisionBehaviour>> ItemPenaltyCoinBehaviours =
        new()
        {
            { PenaltyCoinItemType.GhostCoin, () => new GhostCoinCollisionBehaviour() },
            { PenaltyCoinItemType.PoisonCoin, () => new PoisonCoinCollisionBehaviour() },
            { PenaltyCoinItemType.SkullCoin, () => new SkullCoinCollisionBehaviour() },
            { PenaltyCoinItemType.FireCoin, () => new FireCoinCollisionBehaviour() },
            { PenaltyCoinItemType.BlackHoleCoin, () => new BlackHoleCoinCollisionBehaviour() },
        };
    
    private static readonly Dictionary<RewardCoinItemType, Func<BaseCollisionBehaviour>> ItemRewardCoinBehaviours =
        new()
        {
            { RewardCoinItemType.BronzeCoin, () => new BronzeCoinCollisionBehaviour() },
            { RewardCoinItemType.SilverCoin, () => new SilverCoinCollisionBehaviour() },
            { RewardCoinItemType.GoldCoin, () => new GoldCoinCollisionBehaviour() },
            { RewardCoinItemType.PlatinumCoin, () => new PlatinumCoinCollisionBehaviour() },
            { RewardCoinItemType.DiamondCoin, () => new DiamondCoinCollisionBehaviour() },
        };

    public static BaseCollisionBehaviour GetTrapBehaviour(TrapType trapType)
    {
        if (TrapBehaviours.TryGetValue(trapType, out var behaviour))
        {
            return behaviour();
        }

        Debug.Assert(false, $"Cant find trapBehaviour in TrapType: {trapType}");
        
        return null;
    }
    
    public static BaseCollisionBehaviour GetHumanBehaviour(HumanItemType humanItemType)
    {
        if (ItemHumanBehaviours.TryGetValue(humanItemType, out var behaviour))
        {
            return behaviour();
        }

        Debug.Assert(false, $"Cant find ItemHumanBehaviour in HumanItemType: {humanItemType}");
        
        return null;
    }
    
    public static BaseCollisionBehaviour GetPenaltyCoinBehaviour(PenaltyCoinItemType penaltyCoinType)
    {
        if (ItemPenaltyCoinBehaviours.TryGetValue(penaltyCoinType, out var behaviour))
        {
            return behaviour();
        }

        Debug.Assert(false, $"Cant find ItemPenaltyCoinBehaviour in PenaltyCoinType: {penaltyCoinType}");
        
        return null;
    }
    
    public static BaseCollisionBehaviour GetRewardCoinBehaviour(RewardCoinItemType rewardCoinType)
    {
        if (ItemRewardCoinBehaviours.TryGetValue(rewardCoinType, out var behaviour))
        {
            return behaviour();
        }

        Debug.Assert(false, $"Cant find ItemRewardCoinBehaviour in RewardCoinType: {rewardCoinType}");
        
        return null;
    }
    
    public static BaseCollisionBehaviour GetWallBehaviour(WallType wallType)
    {
        if (WallBehaviours.TryGetValue(wallType, out var behaviour))
        {
            return behaviour();
        }

        Debug.Assert(false, $"Cant find wallBehaviour in WallType: {wallType}");
        
        return null;
    }
}