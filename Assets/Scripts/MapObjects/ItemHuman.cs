using UnityEngine;

public class ItemHuman : ItemBase
{
    public override ObjectType ObjectType { get; protected set; } = ObjectType.None;
    protected override BaseCollisionBehaviour CollisionBehaviour { get; set; } //충돌 

    public override ItemType ItemType { get; protected set; } = ItemType.None; //아이템 타입

    public HumanItemType HumanItemType { get; private set; } = HumanItemType.None; // 인간 타입

    private const long ScoreToAddJuniorResearcher = 100;
    private const long ScoreToAddResearcher = 150;
    private const long ScoreToAddSeniorResearcher = 200;
    [SerializeField] private int itemID;
    private ItemDataTable.ItemData itemStatData;

    public int Score => itemStatData?.Score ?? 0;

    public void Initialize(HumanItemType humanItemType)
    {
        // if (humanItemType == HumanItemType.None || humanItemType == HumanItemType.Count)
        // {
        //     Debug.Assert(false, "Invalid HumanItemType");

        //     return;
        // }

        ObjectType = ObjectType.Item;

        ItemType = ItemType.Human;

        // HumanItemType = humanItemType;
        // CollisionBehaviour = CollisionBehaviourFactory.GetHumanBehaviour(humanItemType);

        itemStatData = DataTableManager.itemDataTable.Get(itemID);
        HumanItemType = (HumanItemType)itemID;
        CollisionBehaviour = CollisionBehaviourFactory.GetHumanBehaviour((HumanItemType)itemID);
        CollisionBehaviour.SetScoreToAdd(itemStatData.Score);


        // switch (humanItemType)
        // {
        //     case HumanItemType.JuniorResearcher:
        //         {
        //             CollisionBehaviour.SetScoreToAdd(ScoreToAddJuniorResearcher);
        //         }
        //         break;
        //     case HumanItemType.Researcher:
        //         {
        //             CollisionBehaviour.SetScoreToAdd(ScoreToAddResearcher);
        //         }
        //         break;
        //     case HumanItemType.SeniorResearcher:
        //         {
        //             CollisionBehaviour.SetScoreToAdd(ScoreToAddSeniorResearcher);
        //         }
        //         break;
        // }
    }
}
