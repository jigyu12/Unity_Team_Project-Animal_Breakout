using TMPro;
using UnityEngine;

public class AnimalPanel : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown animalStatDropDown;

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
    }
}