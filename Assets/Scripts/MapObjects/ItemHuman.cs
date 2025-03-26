using UnityEngine;

public class ItemHuman : ItemBase
{
    public override ObjectType ObjectType { get; protected set; } = ObjectType.None;
    protected override ICollisionBehaviour CollisionBehaviour { get; set; }

    public override ItemType ItemType { get; protected set; } = ItemType.None;
    
    public HumanItemType HumanItemType { get; private set; } = HumanItemType.None;
    
    public void Init(HumanItemType humanItemType)
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
        
        // ToDo : Load Asset
        TryGetComponent(out MeshRenderer meshRenderer);
        switch (humanItemType)
        {
            case HumanItemType.JuniorResearcher:
                meshRenderer.material.color = Color.green;
                break;
            case HumanItemType.Researcher:
                meshRenderer.material.color = Color.blue;
                break;
            case HumanItemType.SeniorResearcher:
                meshRenderer.material.color = Color.magenta;
                break;
        }

    }
}