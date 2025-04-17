using UnityEngine;

public class ItemPenaltyCoin : ItemBase
{
    public override ObjectType ObjectType { get; protected set; } = ObjectType.None;
    protected override BaseCollisionBehaviour CollisionBehaviour { get; set; }

    public override ItemType ItemType { get; protected set; } = ItemType.None;

    public PenaltyCoinItemType PenaltyCoinItemType { get; private set; } = PenaltyCoinItemType.None;

    private const long ScoreToAddGhostCoin = -100;
    private const long ScoreToAddPoisonCoin = -200;
    private const long ScoreToAddSkullCoin = -300;
    private const long ScoreToAddFireCoin = -400;
    private const long ScoreToAddBlackHoleCoin = -500;
    [SerializeField] private int itemID;
    private ItemDataTable.ItemData itemStatData;

    public int Score => itemStatData?.Score ?? 0;

    public void Initialize(PenaltyCoinItemType penaltyCoinItemType)
    {
        // if (penaltyCoinItemType == PenaltyCoinItemType.None || penaltyCoinItemType == PenaltyCoinItemType.Count)
        // {
        //     Debug.Assert(false, "Invalid PenaltyCoinItemType");

        //     return;
        // }

        ObjectType = ObjectType.Item;

        ItemType = ItemType.PenaltyCoin;

        // PenaltyCoinItemType = penaltyCoinItemType;

        // CollisionBehaviour = CollisionBehaviourFactory.GetPenaltyCoinBehaviour(penaltyCoinItemType);

        itemStatData = DataTableManager.itemDataTable.Get(itemID);
        PenaltyCoinItemType = (PenaltyCoinItemType)itemID;
        CollisionBehaviour = CollisionBehaviourFactory.GetPenaltyCoinBehaviour((PenaltyCoinItemType)itemID);
        CollisionBehaviour.SetScoreToAdd(itemStatData.Score);

        // switch (penaltyCoinItemType)
        // {
        //     case PenaltyCoinItemType.GhostCoin:
        //         {
        //             CollisionBehaviour.SetScoreToAdd(ScoreToAddGhostCoin);
        //         }
        //         break;
        //     case PenaltyCoinItemType.PoisonCoin:
        //         {
        //             CollisionBehaviour.SetScoreToAdd(ScoreToAddPoisonCoin);
        //         }
        //         break;
        //     case PenaltyCoinItemType.SkullCoin:
        //         {
        //             CollisionBehaviour.SetScoreToAdd(ScoreToAddSkullCoin);
        //         }
        //         break;
        //     case PenaltyCoinItemType.FireCoin:
        //         {
        //             CollisionBehaviour.SetScoreToAdd(ScoreToAddFireCoin);
        //         }
        //         break;
        //     case PenaltyCoinItemType.BlackHoleCoin:
        //         {
        //             CollisionBehaviour.SetScoreToAdd(ScoreToAddBlackHoleCoin);
        //         }
        //         break;
    }
}