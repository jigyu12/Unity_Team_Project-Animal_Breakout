using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDebuffUIController : UIElement
{
    [SerializeField] private Transform debuffIconArea;
    [SerializeField] private GameObject debuffIconPrefab;

    private Dictionary<SkillType, GameObject> activeIcons = new();

    public void AddDebuff(SkillType type, Sprite icon)
    {
        if (activeIcons.ContainsKey(type)) return;

        GameObject iconObj = Instantiate(debuffIconPrefab, debuffIconArea);
        iconObj.GetComponent<Image>().sprite = icon;
        activeIcons[type] = iconObj;
    }

    public void RemoveDebuff(SkillType type)
    {
        if (!activeIcons.TryGetValue(type, out var icon)) return;

        Destroy(icon);
        activeIcons.Remove(type);
    }

    public void ClearAllDebuffs()
    {
        foreach (var icon in activeIcons.Values)
        {
            Destroy(icon);
        }
        activeIcons.Clear();
    }
}
