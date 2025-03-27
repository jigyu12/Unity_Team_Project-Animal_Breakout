using UnityEngine;

public class Hole : MonoBehaviour, ICollisionable
{
    private void Awake()
    {
        TryGetComponent(out MeshRenderer meshRenderer);
        meshRenderer.material.color = Color.red;
    }

    public void OnCollision(Collider other)
    {
        Destroy(gameObject);

        other.gameObject.TryGetComponent(out PlayerStatus1 playerStatus);
        //  playerStatus.OnDie();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnCollision(other);
        }
    }
}