using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SupportSlotUI : UIElement
{
    [SerializeField] private List<Image> supportSlots;

    public void AddSupportSkill(Sprite skillIcon)
    {
        foreach (var slot in supportSlots)
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
        foreach (var slot in supportSlots)
        {
            slot.gameObject.SetActive(false);
        }
    }
}
