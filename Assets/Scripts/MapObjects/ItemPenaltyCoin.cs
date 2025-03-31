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

    public void Initialize(PenaltyCoinItemType penaltyCoinItemType)
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
        
        // ToDo : Load Asset & Table
        TryGetComponent(out MeshRenderer meshRenderer);
        switch (penaltyCoinItemType)
        {
            case PenaltyCoinItemType.GhostCoin:
            {
                meshRenderer.material.color = new Color(0.9f, 0.9f, 0.9f, 0.5f);
                CollisionBehaviour.SetScoreToAdd(ScoreToAddGhostCoin);
            }
                break;
            case PenaltyCoinItemType.PoisonCoin:
            {
                meshRenderer.material.color = Color.green;
                CollisionBehaviour.SetScoreToAdd(ScoreToAddPoisonCoin);
            }
                break;
            case PenaltyCoinItemType.SkullCoin:
            {
                meshRenderer.material.color = Color.gray;
                CollisionBehaviour.SetScoreToAdd(ScoreToAddSkullCoin);
            }
                break;
            case PenaltyCoinItemType.FireCoin:
            {
                meshRenderer.material.color = new Color(1f, 0.3f, 0f);
                CollisionBehaviour.SetScoreToAdd(ScoreToAddFireCoin);
            }
                break;
            case PenaltyCoinItemType.BlackHoleCoin:
            {
                meshRenderer.material.color = Color.black;
                CollisionBehaviour.SetScoreToAdd(ScoreToAddBlackHoleCoin);
            }
                break;
        }
    }
}