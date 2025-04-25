using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDebuffUIController : UIElement
{
    [SerializeField] private Transform debuffIconArea;
    [SerializeField] private GameObject debuffIconPrefab;

    [SerializeField] private List<DebuffIconInfo> debuffIcons = new(); // 인스펙터에서 등록

    private Dictionary<string, GameObject> activeIcons = new();
    private Dictionary<string, Sprite> debuffIconMap = new();

    private void Awake()
    {
        foreach (var info in debuffIcons)
        {
            if (!debuffIconMap.ContainsKey(info.debuffId))
            {
                debuffIconMap.Add(info.debuffId, info.icon);
            }
        }
    }

    public void AddDebuff(string debuffId)
    {
        if (activeIcons.ContainsKey(debuffId)) return;

        GameObject iconObj = Instantiate(debuffIconPrefab, debuffIconArea);

        if (debuffIconMap.TryGetValue(debuffId, out var icon))
        {
            iconObj.GetComponentInChildren<Image>().sprite = icon;
        }

        activeIcons[debuffId] = iconObj;
    }

    public void RemoveDebuff(string debuffId)
    {
        if (!activeIcons.TryGetValue(debuffId, out var icon)) return;

        Destroy(icon);
        activeIcons.Remove(debuffId);
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

[System.Serializable]
public class DebuffIconInfo
{
    public string debuffId;
    public Sprite icon;
}
