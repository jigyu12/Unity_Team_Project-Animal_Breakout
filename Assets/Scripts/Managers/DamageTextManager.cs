using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : InGameManager
{
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private int initialPoolCount = 20;
    private Queue<DamageText> damageTextPool = new();

    private void Awake()
    {
        for (int i = 0; i < initialPoolCount; i++)
        {
            CreateDamageText();
        }
    }

    private void CreateDamageText()
    {
        var obj = Instantiate(damageTextPrefab, transform);
        obj.SetActive(false);
        damageTextPool.Enqueue(obj.GetComponent<DamageText>());
    }

    public void Register(DamageableStatus target, Color color)
    {
        target.onDamaged += (damage) => ShowDamage(target.transform.position, damage, color);
    }

    public void ShowDamage(Vector3 worldPosition, float damage, Color color)
    {
        if (damageTextPool.Count == 0)
            CreateDamageText();

        var damageText = damageTextPool.Dequeue();
        damageText.gameObject.SetActive(true);

        Vector3 offset = Vector3.up * 2f + Camera.main.transform.forward * -1f;
        Vector3 randomSpread = new Vector3(
            Random.Range(-1.5f, 1.5f),
            0f,
            Random.Range(-1.5f, 1.5f)
        );

        damageText.transform.position = worldPosition + offset + randomSpread;
        damageText.Initialize(damage, ReturnToPool, color);
    }



    private void ReturnToPool(DamageText text)
    {
        text.gameObject.SetActive(false);
        damageTextPool.Enqueue(text);
    }
}
