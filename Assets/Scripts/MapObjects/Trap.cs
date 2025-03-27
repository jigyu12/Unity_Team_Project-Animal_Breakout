using UnityEngine;

public class Trap : CollidableMapObject
{
    public override ObjectType ObjectType { get; protected set; } = ObjectType.None;
    protected override ICollisionBehaviour CollisionBehaviour { get; set; }

    public TrapType TrapType { get; private set; } = TrapType.None;

    public void Init(TrapType trapType)
    {
        if (trapType == TrapType.None || trapType == TrapType.Count)
        {
            Debug.Assert(false, "Invalid TrapType");
            return;
        }

        if (trapType == TrapType.Bomb)
        {
            ObjectType = ObjectType.TrapBomb;
        }
        else if (trapType == TrapType.Hole)
        {
            ObjectType = ObjectType.TrapHole;
        }
        
        CollisionBehaviour = CollisionBehaviourFactory.GetTrapBehaviour(trapType);
        
        TrapType = trapType;
        
        // ToDo : Load Asset
        TryGetComponent(out MeshRenderer meshRenderer);
        switch (trapType)
        {
            case TrapType.Bomb:
                meshRenderer.material.color = Color.black;
                break;
            case TrapType.Hole:
                meshRenderer.material.color = Color.red;
                break;
        }

    }
}