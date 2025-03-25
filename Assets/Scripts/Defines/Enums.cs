public enum WayType
{
    Straight = 0,
    Left,
    Right,
    UnavoidableWall,
    
    Count
}

public enum ItemType
{
    RewardItem = 0,
    PenaltyItem,
    
    Count
}

public enum RewardItemType
{
    BronzeCoin = 0,
    SilverCoin,
    GoldCoin,
    PlatinumCoin,
    DiamondCoin,
    Human,
    
    Count
}

public enum PenaltyItemType
{
    GhostCoin = 0,
    PoisonCoin,
    SkullCoin,
    FireCoin,
    BlackHoleCoin,
    
    Count
}

public enum TrapType
{
    None = -1,
    
    Bomb = 0,
    Hole,
    
    Count
}