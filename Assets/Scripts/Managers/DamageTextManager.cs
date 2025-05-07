using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;
public class DamageTextManager : InGameManager
{
    [SerializeField] private DamageNumber numberPrefab;
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private int initialPoolCount = 20;
    private Queue<DamageText> damageTextPool = new();
    List<DamageNumber> activeDamageNumbers = new List<DamageNumber>();

    // private void Awake()
    // {
    //     for (int i = 0; i < initialPoolCount; i++)
    //     {
    //         CreateDamageText();
    //     }
    // }

    // private void CreateDamageText()
    // {
    //     var obj = Instantiate(damageTextPrefab, transform);
    //     obj.SetActive(false);
    //     damageTextPool.Enqueue(obj.GetComponent<DamageText>());
    // }

    public void Register(DamageableStatus target, Color defaultColor)
    {
        if (target is BossStatus bossStatus)
        {
            bossStatus.onElementalDamaged += (damage, elemental) => ShowDamage(target.transform.position, damage, GetColorByElement(elemental));
        }
        else
        {
            target.onDamaged += (damage) => ShowDamage(target.transform.position, damage, defaultColor);
        }
    }

    // public void ShowDamage(Vector3 worldPosition, float damage, Color color)
    // {
    //     if (damageTextPool.Count == 0)
    //         CreateDamageText();

    //     var damageText = damageTextPool.Dequeue();
    //     damageText.gameObject.SetActive(true);

    //     Vector3 offset = Vector3.up * 2f + Camera.main.transform.forward * -1f;
    //     Vector3 randomSpread = new Vector3(
    //         Random.Range(-1.5f, 1.5f),
    //         0f,
    //         Random.Range(-1.5f, 1.5f)
    //     );

    //     damageText.transform.position = worldPosition + offset + randomSpread;
    //     damageText.Initialize(damage, ReturnToPool, color);
    // }
    public void ShowDamage(Vector3 worldPosition, float damage, Color color)
    {
        if (numberPrefab == null)
        {
            Debug.LogWarning("DamageNumber Prefab이 연결되어 있지 않습니다!");
            return;
        }

        Vector3 offset = Vector3.up * 2f + Camera.main.transform.forward * -1f;
        Vector3 randomSpread = new Vector3(
            Random.Range(-1.5f, 1.5f),
            0f,
            Random.Range(-1.5f, 1.5f)
        );

        Vector3 spawnPosition = worldPosition + offset + randomSpread;

        var damageNumber = numberPrefab.Spawn(spawnPosition, damage);
        damageNumber.SetColor(color);
    }

    // private void ReturnToPool(DamageText text)
    // {
    //     text.gameObject.SetActive(false);
    //     damageTextPool.Enqueue(text);
    // }
    private Color GetColorByElement(SkillElemental elemental)
    {
        return elemental switch
        {
            SkillElemental.Fire => Color.red,
            SkillElemental.Ice => Color.cyan,
            SkillElemental.Thunder => new Color(0.6f, 0f, 1f),
            _ => Color.white,
        };
    }
}
