using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaResultSlotPanel : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Image starImage;
    [SerializeField] private TMP_Text itemName;
    
    public void SetItemImage(Sprite itemImage)
    {
        this.itemImage.sprite = itemImage;
    }
    
    public void SetStarImage(Sprite starImage)
    {
        this.starImage.sprite = starImage;
    }
    
    public void SetItemName(string itemName)
    {
        this.itemName.text = itemName;
    }
}