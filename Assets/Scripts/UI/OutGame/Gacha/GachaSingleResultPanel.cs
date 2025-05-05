using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GachaSingleResultPanel : GachaPanelBase
{
    [SerializeField] private Image starImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    
    private int currentGachaDataIndex;

    protected override void Awake()
    {
        base.Awake();
        
        var actionMap = inputActions.FindActionMap("PlayerActions");
        touchAction = actionMap?.FindAction("TouchGacha2");
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();

        currentGachaDataIndex = 0;
        
        SetItemNameText((currentGachaDataIndex + 1).ToString());
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        currentGachaDataIndex = 0;
    }
    
    protected override void OnTouchPerformed(InputAction.CallbackContext context)
    {
        ++currentGachaDataIndex;
        
        if (currentGachaDataIndex == gachaDataList.Count)
        {
            gachaPanelController?.ShowNextGachaPanel();
        }
        else
        {
            SetItemNameText((currentGachaDataIndex + 1).ToString());
        }
    }
    
    public void SetStarImage(Sprite starImage)
    {
        this.starImage.sprite = starImage;
    }
    
    public void SetItemImage(Sprite itemImage)
    {
        this.itemImage.sprite = itemImage;
    }
    
    public void SetItemNameText(string itemNameText)
    {
        this.itemNameText.text = itemNameText;
    }
    
    public void SetItemDescriptionText(string itemDescriptionText)
    {
        this.itemDescriptionText.text = itemDescriptionText;
    }
}