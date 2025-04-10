using UnityEngine;

public class ItemHuman : ItemBase
{
    public override ObjectType ObjectType { get; protected set; } = ObjectType.None;
    protected override BaseCollisionBehaviour CollisionBehaviour { get; set; }

    public override ItemType ItemType { get; protected set; } = ItemType.None;
    
    public HumanItemType HumanItemType { get; private set; } = HumanItemType.None;
    
    private const long ScoreToAddJuniorResearcher = 100;
    private const long ScoreToAddResearcher = 150;
    private const long ScoreToAddSeniorResearcher = 200;
    
    public void Initialize(HumanItemType humanItemType)
    {
        if (humanItemType == HumanItemType.None || humanItemType == HumanItemType.Count)
        {
            Debug.Assert(false, "Invalid HumanItemType");
            
            return;
        }
        
        ObjectType = ObjectType.Item;
        
        ItemType = ItemType.Human;
        
        HumanItemType = humanItemType;
        
        CollisionBehaviour = CollisionBehaviourFactory.GetHumanBehaviour(humanItemType);
        
        switch (humanItemType)
        {
            case HumanItemType.JuniorResearcher:
            {
                CollisionBehaviour.SetScoreToAdd(ScoreToAddJuniorResearcher);
            }
                break;
            case HumanItemType.Researcher:
            {
                CollisionBehaviour.SetScoreToAdd(ScoreToAddResearcher);
            }
                break;
            case HumanItemType.SeniorResearcher:
            {
                CollisionBehaviour.SetScoreToAdd(ScoreToAddSeniorResearcher);
            }
                break;
        }
    }
}