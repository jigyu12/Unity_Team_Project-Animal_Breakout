using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackSlotUI : UIElement
{
    [SerializeField] private List<Image> attackSlots;

    public void AddSupportSkill(Sprite skillIcon)
    {
        foreach (var slot in attackSlots)
        {
            if (!slot.gameObject.activeSelf)
            {
                slot.gameObject.SetActive(true);
                slot.sprite = skillIcon;
                return;
            }
        }
    }

    public void ClearSlots()
    {
        foreach (var slot in attackSlots)
        {
            slot.gameObject.SetActive(false);
        }
    }
}
