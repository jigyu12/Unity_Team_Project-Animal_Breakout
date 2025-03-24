public enum WayType
{
    Straight = 0, 
    Left,
    Right,
    UnavoidableWall,
    
    Count
}

public enum RewardType
{
    TestRewardCoin = -1,
    
    BronzeCoin = 0,
    SilverCoin,
    GoldCoin,
    PlatinumCoin,
    DiamondCoin,
    Human,
    
    Count
}

public enum PenaltyType
{
    TestPenaltyCoin = -1,
    
    GhostCoin = 0,
    PoisonCoin,
    SkullCoin,
    FireCoin,
    BlackHoleCoin,
    
    Count
} 