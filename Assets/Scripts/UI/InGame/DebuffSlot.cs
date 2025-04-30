using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DebuffSlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    private string currentDebuffId;

    public bool IsOccupied => !string.IsNullOrEmpty(currentDebuffId);

    public void SetDebuff(string debuffId, Sprite icon)
    {
        currentDebuffId = debuffId;
        iconImage.sprite = icon;
        iconImage.enabled = true;
    }

    public void ClearDebuff()
    {
        currentDebuffId = null;
        iconImage.sprite = null;
        iconImage.enabled = false;
    }

    public string GetDebuffId()
    {
        return currentDebuffId;
    }
}
