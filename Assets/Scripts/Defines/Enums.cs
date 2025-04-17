public enum MapObjectCSVType
{
    Bomb = 1,
    Hole,
    Human,
    PenaltyCoin,
}

public enum RewardCoinPatternCSVType
{
    Straight = 1,
    Hill,
}

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

    BronzeCoin = 110101,
    SilverCoin,
    GoldCoin,
    PlatinumCoin,
    DiamondCoin,

    Count
}

public enum HumanItemType
{
    None = -1,

    JuniorResearcher = 110301,
    Researcher,
    SeniorResearcher,

    Count
}


public enum PenaltyCoinItemType
{
    None = -1,

    GhostCoin = 110201,
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

public enum DefaultCanvasType
{
    Shop = 0,
    Lobby,
    Animal,

    Menu,
}

public enum SwitchableCanvasType
{
    Shop = 0,
    Lobby,
    Animal,
}