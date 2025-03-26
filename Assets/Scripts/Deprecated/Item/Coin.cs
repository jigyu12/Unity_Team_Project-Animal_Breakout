using UnityEngine;

public class Coin : MonoBehaviour, ICollisionable
{
    private void Awake()
    {
        TryGetComponent(out MeshRenderer meshRenderer);
        meshRenderer.material.color = Color.yellow;
    }
    
    public void OnCollision(Collider other)
    {
        Debug.Log("Acquire Coin");
        
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnCollision(other);
        }
    }
}