using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimalPanel : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown animalStatDropDown;
    [SerializeField] private TMP_Text labelText;
    
    [SerializeField] private ScrollRect animalScrollRect;

    private void Awake()
    {
        GameDataManager.onLocaleChange += OnLanguageSettingTypeChangedHandler;
    }

    private void OnDestroy()
    {
        GameDataManager.onLocaleChange -= OnLanguageSettingTypeChangedHandler;
    }
    
    private void OnLanguageSettingTypeChangedHandler()
    {
        ChangeAnimalStatDropDownLanguage();
    }

    private void ChangeAnimalStatDropDownLanguage()
    {
        animalStatDropDown.options.Clear();
        
        animalStatDropDown.options.Add(new TMP_Dropdown.OptionData(LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalLevelSortDropDownStringKey)));
        animalStatDropDown.options.Add(new TMP_Dropdown.OptionData(LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalGradeSortDropDownStringKey)));
        
        labelText.text = animalStatDropDown.options[animalStatDropDown.value].text;
    }

    public void InActiveAnimalScrollView()
    {
        animalScrollRect.enabled = false;
    }
    
    public void ActiveAnimalScrollView()
    {
        animalScrollRect.enabled = true;
    }
}