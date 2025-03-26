using UnityEngine;

public interface ICollisionBehaviour
{
    void OnCollision(GameObject self, Collider other);
}