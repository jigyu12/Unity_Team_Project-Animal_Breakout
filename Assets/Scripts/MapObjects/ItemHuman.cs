using UnityEngine;

public class ItemHuman : ItemBase
{
    public override ObjectType ObjectType { get; protected set; } = ObjectType.None;
    protected override BaseCollisionBehaviour CollisionBehaviour { get; set; } //충돌 

    public override ItemType ItemType { get; protected set; } = ItemType.None; //아이템 타입

    public HumanItemType HumanItemType { get; private set; } = HumanItemType.None; // 인간 타입

    [SerializeField] private HumanItemType humanItemType;
    private ItemDataTable.ItemData itemStatData;

    public int Score => itemStatData?.Score ?? 0;

    public void Initialize()
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

        itemStatData = DataTableManager.itemDataTable.Get((int)this.humanItemType);
        HumanItemType = this.humanItemType;
        CollisionBehaviour = CollisionBehaviourFactory.GetHumanBehaviour(this.humanItemType);
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
