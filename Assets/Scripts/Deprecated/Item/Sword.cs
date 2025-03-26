using TMPro;
using UnityEngine;

public class Sword : MonoBehaviour, ICollisionable
{
    private void Awake()
    {
        TryGetComponent(out MeshRenderer meshRenderer);
        meshRenderer.material.color = Color.blue;
    }
    
    public void OnCollision(Collider other)
    {
        Destroy(gameObject);
        
        other.gameObject.TryGetComponent(out Attacker attacker);
        attacker.power += 100;
        attacker.powerDisplay.TryGetComponent(out TMP_Text text);
        text.text = $"{attacker.power}";
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnCollision(other);
        }
    }
}