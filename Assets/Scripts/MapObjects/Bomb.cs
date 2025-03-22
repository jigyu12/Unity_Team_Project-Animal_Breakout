using UnityEngine;

public class Bomb : MonoBehaviour, ICollisionable
{
    private void Awake()
    {
        TryGetComponent(out MeshRenderer meshRenderer);
        meshRenderer.material.color = Color.black;
    }

    public void OnCollision(Collider other)
    {
        Destroy(gameObject);

        other.gameObject.TryGetComponent(out PlayerStatus playerStatus);
        playerStatus.OnDie();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnCollision(other);
        }
    }
}