using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public static event Action<float, float> onBgmVolumeChanged;
    public static event Action<int> onFrameRateIndexChanged;
    public static event Action<LanguageSettingType> onLanguageSettingTypeChanged;

    private void Awake()
    {
        bgmSlider.onValueChanged.AddListener(OnBgmSliderValueChangedHandler);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderValueChanged);

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
            if (SceneManager.GetActiveScene().name == "MainTitleScene")
            {
                GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
                outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
            }
            else
            {
                transform.parent.gameObject.SetActive(false);
            }
        });

        bgmValue = GameDataManager.Instance.PlayerAccountData.bgmVolume;
        sfxValue = GameDataManager.Instance.PlayerAccountData.sfxVolume;

        frameRateIndex = GameDataManager.Instance.frameRateIndex;

        languageSettingType = GameDataManager.Instance.languageSettingType;

        SetBgmSliderValue(bgmValue);
        SetSfxSliderValue(sfxValue);

        SetFrameRateToggle(frameRateIndex);
        for (int i = 0; i < frameToggles.Count; ++i)
        {
            int index = i;
            frameToggles[i].onValueChanged.AddListener((isOn) => OnFrameToggleValueChanged(index, isOn));
        }

        SetLanguageSettingType(languageSettingType);
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

        for (int i = 0; i < frameToggles.Count; ++i)
        {
            if (i == frameRateIndex)
            {
                frameToggles[frameRateIndex].isOn = true;
            }
            else
            {
                frameToggles[i].isOn = false;
            }
        }
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

                    return;
                }
        }

        onLanguageSettingTypeChanged?.Invoke(this.languageSettingType);

        countryImage.sprite =
            LocalizationUtility.GetLocalizeSprite(LocalizationUtility.defaultSpriteTableName,
                Utils.CountryIconSpriteKey);
    }

    public void SetCloseButtonText(string closeText)
    {
        this.closeText.text = closeText;
    }

    private void OnBgmSliderValueChangedHandler(float value)
    {
        bgmValue = value;

        onBgmVolumeChanged?.Invoke(bgmValue, sfxValue);
    }

    private void OnSfxSliderValueChanged(float value)
    {
        sfxValue = value;

        onBgmVolumeChanged?.Invoke(bgmValue, sfxValue);
    }

    private void OnFrameToggleValueChanged(int index, bool isOn)
    {
        if (isOn)
        {
            frameRateIndex = index;

            onFrameRateIndexChanged?.Invoke(frameRateIndex);
        }
    }
}