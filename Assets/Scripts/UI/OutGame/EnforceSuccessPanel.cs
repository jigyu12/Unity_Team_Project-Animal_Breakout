using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EnforceSuccessPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text enforceSuccessText;
    [SerializeField] private TMP_Text animalNameText;
    [SerializeField] private TMP_Text attackPowerText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text passiveText;
    
    [SerializeField] private Image starImage;
    [SerializeField] private Image animalImage;
    
    [SerializeField] protected InputActionAsset inputActions;
    private InputAction touchAction;
    
    protected OutGameUIManager outGameUIManager;
    
    private bool isFindOtherComponent = false;

    private void Awake()
    {
        var actionMap = inputActions.FindActionMap("UIActions");
        touchAction = actionMap?.FindAction("TouchEnforce");
        
        OutGameUIManager.onEnforceSuccessScreenActive += OnEnforceSuccessScreenActiveHandler;
    }

    private void OnDestroy()
    {
        OutGameUIManager.onEnforceSuccessScreenActive -= OnEnforceSuccessScreenActiveHandler;
    }
    
    protected virtual void OnEnable()
    {
        if (touchAction is not null)
        {
            touchAction.performed += OnTouchPerformed;
            touchAction.Enable();
        }
    }

    protected virtual void OnDisable()
    {
        if (touchAction is not null)
        {
            touchAction.performed -= OnTouchPerformed;
            touchAction.Disable();
        }
    }
    
    protected virtual void OnTouchPerformed(InputAction.CallbackContext context)
    {
        gameObject.SetActive(false);
        outGameUIManager.HideFullScreenPanel();
    }

    private void OnEnforceSuccessScreenActiveHandler(AnimalUserData animalUserData)
    {
        SetEnforceSuccessPanel(animalUserData);
        
        gameObject.SetActive(true);
    }
    
    public void SetEnforceSuccessPanel(AnimalUserData animalUserData)
    {
        if (!isFindOtherComponent)
        {
            GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
            outGameUIManager = outGameManager.OutGameUIManager;
        }
        
        SetEnforceSuccessText("강화 성공");
        SetAnimalNameText(animalUserData.AnimalStatData.StringID);
        SetAttackPowerText(animalUserData.AttackPower);
        SetLevelText(animalUserData.Level);
        SetPassiveText(animalUserData.AnimalStatData.passive.ToString());
        SetAnimalImage(animalUserData.AnimalStatData.iconImage);
        //SetStarImage();
    }

    public void SetEnforceSuccessText(string text)
    {
        enforceSuccessText.text = text;
    }
    
    public void SetAnimalNameText(string text)
    {
        animalNameText.text = text;
    }

    public void SetAttackPowerText(int attackPower)
    {
        attackPowerText.text = $"공격력          {attackPower}";
    }

    public void SetLevelText(int level)
    {
        levelText.text = $"레벨          Lv.{level}";
    }

    public void SetPassiveText(string text)
    {
        passiveText.text = $"보유 효과          {text}";
    }
    
    public void SetAnimalImage(Sprite animalImage)
    {
        this.animalImage.sprite = animalImage;
    }

    public void SetStarImage(Sprite starImage)
    {
        this.starImage.sprite = starImage;
    }
}