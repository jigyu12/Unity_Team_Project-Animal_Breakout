using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text settingText;
    
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private List<Toggle> frameToggles;
    [SerializeField] private ToggleGroup frameToggleGroup;

    
    [SerializeField] private TMP_Text languageText;
    [SerializeField] private Image countryImage;
    [SerializeField] private Button languageSwitchButton;
    [SerializeField] private Image languageSwitchImage;

    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Text closeText;

    public float bgmValue { get; private set; }
    public float sfxValue { get; private set; }
    public int frameRateIndex { get; private set; }
    public LanguageSettingType languageSettingType { get; private set; }
    
    [SerializeField] private CloseButtonType closeButtonType;

    private void Awake()
    {
        bgmSlider.onValueChanged.AddListener(OnBgmSliderValueChangedHandler);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderValueChanged);

        for (int i = 0; i < frameToggles.Count; ++i)
        {
            frameToggles[i].group = frameToggleGroup;
            frameToggles[i].onValueChanged.AddListener(OnFrameToggleValueChanged);
        }
        
        languageSwitchButton.onClick.AddListener(() =>
        {
            LanguageSettingType type = (LanguageSettingType)((int)languageSettingType + 1);

            if (type == LanguageSettingType.Count)
            {
                type = LanguageSettingType.Korean;
            }
            
            SetLanguageSettingType(type);
        });
    }
    
    private void OnDestroy()
    {
        bgmSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();
        
        for (int i = 0; i < frameToggles.Count; ++i)
        {
            frameToggles[i].onValueChanged.RemoveAllListeners();
        }
        
        languageSwitchButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            if (closeButtonType == CloseButtonType.OutGame)
            {
                GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
                outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
            }
            else
            {
                gameObject.SetActive(false);
            }
        });
        
        bgmValue = 0.5f;
        sfxValue = 0.2f;
        frameRateIndex = 2;
        languageSettingType = LanguageSettingType.Korean;
        
        SetSettingText("설정");
        SetBgmSliderValue(bgmValue);
        SetSfxSliderValue(sfxValue);
        SetFrameRateToggle(frameRateIndex);
        SetLanguageSettingType(languageSettingType);
        SetCloseButtonText("닫기");
    }
    
    public void SetSettingText(string settingText)
    {
        this.settingText.text = settingText;
    }
    
    public void SetBgmSliderValue(float value)
    {
        bgmSlider.value = value;
    }
    
    public void SetSfxSliderValue(float value)
    {
        sfxSlider.value = value;
    }
    
    public void SetFrameRateToggle(int index)
    {
        if (index < 0 || index >= frameToggles.Count)
        {
            Debug.Assert(false, "FrameRateToggle index is out of range.");
            
            return;
        }
        
        frameRateIndex = index;
        
        frameToggles[index].isOn = true;
    }
    
    public void SetLanguageSettingType(LanguageSettingType languageSettingType)
    {
        this.languageSettingType = languageSettingType;
        
        Debug.Log($"Language Setting Type : {languageSettingType}");

        switch (this.languageSettingType)
        {
            case LanguageSettingType.Korean:
                {
                    languageText.text = "한국어";
                    countryImage.sprite = null;

                }
                break;
            case LanguageSettingType.English:
                {
                    languageText.text = "English";
                    countryImage.sprite = null;
                }
                break;
            default:
                {
                    Debug.Assert(false, "LanguageSettingType is not defined.");
                }
                break;
        }
    }

    public void SetCloseButtonText(string closeText)
    {
        this.closeText.text = closeText;
    }
    
    private void OnBgmSliderValueChangedHandler(float value)
    {
        bgmValue = value;
        
        Debug.Log($"BGM Value : {bgmValue}");
    }
    
    private void OnSfxSliderValueChanged(float value)
    {
        sfxValue = value;
        
        Debug.Log($"SFX Value : {sfxValue}");
    }
    
    private void OnFrameToggleValueChanged(bool isOn)
    {
        for (int i = 0; i < frameToggles.Count; ++i)
        {
            if (frameToggles[i].isOn)
            {
                frameRateIndex = i;
                
                break;
            }
        }
        
        Debug.Log($"Frame Rate Index : {frameRateIndex}");
    }
}