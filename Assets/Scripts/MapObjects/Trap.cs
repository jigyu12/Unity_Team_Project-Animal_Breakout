using UnityEngine;

public class Trap : MonoBehaviour
{
    private ICollisionBehaviour collisionBehaviour;

    public TrapType TrapType { get; private set; } = TrapType.None;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Utils.PlayerTag))
        {
            collisionBehaviour.OnCollision(gameObject, other);
        }
    }

    public void Init(TrapType trapType)
    {
        TrapType = trapType;
        
        collisionBehaviour = CollisionBehaviourFactory.GetBehaviour(trapType);
        
        TryGetComponent(out MeshRenderer meshRenderer);
        if (trapType == TrapType.Bomb)
        {
            meshRenderer.material.color = Color.black;
        }
        else if (trapType == TrapType.Hole)
        {
            meshRenderer.material.color = Color.red;
        }
    }
}