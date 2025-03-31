public enum WayType
{
    Straight = 0,
    Left,
    Right,
    UnavoidableWall,
    
    Count
}

public enum ObjectType
{
    None = -1,
    
    Item = 0,
    TrapBomb,
    TrapHole,
    Wall,
    ItemTrapMixed,
    
    Count
}

public enum ItemType
{
    None = -1,
    
    RewardCoin = 0,
    PenaltyCoin,
    Human,
    
    Count
}

public enum RewardCoinItemType
{
    None = -1,
    
    BronzeCoin = 0,
    SilverCoin,
    GoldCoin,
    PlatinumCoin,
    DiamondCoin,
    
    Count
}

public enum HumanItemType
{
    None = -1,
    
    JuniorResearcher = 0,
    Researcher,
    SeniorResearcher,
    
    Count
}

public enum PenaltyCoinItemType
{
    None = -1,
    
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

public enum WallType
{
    None = -1,
    
    NormalWall = 0,
    ReinforcedWall,
    
    Count
}