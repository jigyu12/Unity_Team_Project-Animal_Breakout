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
        
        var actionMap = inputActions.FindActionMap("UIActions");
        touchAction = actionMap?.FindAction("TouchGacha2");
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();

        currentGachaDataIndex = 0;

        if (gachaDataList is not null)
        {
            SetItemNameText(GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(
                    gachaDataList[currentGachaDataIndex].AnimalID).AnimalStatData
                .StringID);
        }
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
            SetItemNameText(GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(
                    gachaDataList[currentGachaDataIndex].AnimalID).AnimalStatData
                .StringID);
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