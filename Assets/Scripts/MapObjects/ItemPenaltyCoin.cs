using UnityEngine;

public class ItemPenaltyCoin : ItemBase
{
    public override ObjectType ObjectType { get; protected set; } = ObjectType.None;
    protected override ICollisionBehaviour CollisionBehaviour { get; set; }

    public override ItemType ItemType { get; protected set; } = ItemType.None;

    public PenaltyCoinItemType PenaltyCoinItemType { get; private set; } = PenaltyCoinItemType.None;

    public void Init(PenaltyCoinItemType penaltyCoinItemType)
    {
        if (penaltyCoinItemType == PenaltyCoinItemType.None || penaltyCoinItemType == PenaltyCoinItemType.Count)
        {
            Debug.Assert(false, "Invalid PenaltyCoinItemType");
            
            return;
        }
        
        ObjectType = ObjectType.Item;
        
        ItemType = ItemType.PenaltyCoin;
        
        PenaltyCoinItemType = penaltyCoinItemType;
        
        CollisionBehaviour = CollisionBehaviourFactory.GetPenaltyCoinBehaviour(penaltyCoinItemType);
        
        // ToDo : Load Asset
        TryGetComponent(out MeshRenderer meshRenderer);
        switch (penaltyCoinItemType)
        {
            case PenaltyCoinItemType.GhostCoin:
                meshRenderer.material.color = new Color(0.9f, 0.9f, 0.9f, 0.5f);
                break;
            case PenaltyCoinItemType.PoisonCoin:
                meshRenderer.material.color = Color.green;
                break;
            case PenaltyCoinItemType.SkullCoin:
                meshRenderer.material.color = Color.gray;
                break;
            case PenaltyCoinItemType.FireCoin:
                meshRenderer.material.color = new Color(1f, 0.3f, 0f);
                break;
            case PenaltyCoinItemType.BlackHoleCoin:
                meshRenderer.material.color = Color.black;
                break;
        }

    }
}