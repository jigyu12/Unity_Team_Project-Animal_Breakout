using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AnimalChooseButton : MonoBehaviour
{
    private Button animalChooseButton;
    [SerializeField] [ReadOnly] private int animalID;
    [SerializeField] private TMP_Text animalChooseText;
    
    bool isSameWithCurrentAnimalId;
    
    private void Awake()
    {
        TryGetComponent(out animalChooseButton);

        GameDataManager.onSetStartAnimalIDInGameDataManager += SetAnimalChooseText;
        UnlockedAnimalPanel.onSetStartAnimalIDInPanel += SetAnimalChooseText;
        GameDataManager.onLocaleChange += OnLocaleChangeHandler;

        isSameWithCurrentAnimalId = false;
    }

    private void OnDestroy()
    {
        GameDataManager.onSetStartAnimalIDInGameDataManager -= SetAnimalChooseText;
        UnlockedAnimalPanel.onSetStartAnimalIDInPanel -= SetAnimalChooseText;
        GameDataManager.onLocaleChange -= OnLocaleChangeHandler;
    }

    public void SetAnimalID(int id)
    {
        animalID = id;
    }

    private void SetAnimalChooseText(int id, int currentStamina, AnimalUserData animalUserData)
    {
        isSameWithCurrentAnimalId = id == animalID;
        animalChooseText.text = isSameWithCurrentAnimalId ? 
            LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalSelectedString) 
            : LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalSelectableString);
        animalChooseButton.interactable = !isSameWithCurrentAnimalId;
    }

    private void OnLocaleChangeHandler()
    {
        animalChooseText.text = isSameWithCurrentAnimalId ? 
            LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalSelectedString) 
            : LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalSelectableString);
    }
    
    public void SetAnimalChooseText(int id)
    {
        isSameWithCurrentAnimalId = id == animalID;
        animalChooseText.text = isSameWithCurrentAnimalId ? 
            LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalSelectedString) 
            : LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalSelectableString);
        animalChooseButton.interactable = !isSameWithCurrentAnimalId;
    }
}