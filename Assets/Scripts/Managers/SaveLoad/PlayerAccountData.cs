using System;
using UnityEngine;

public class PlayerAccountData : ISaveLoad
{
    public DataSourceType SaveDataSouceType
    {
        get => DataSourceType.Local;
    }

    public bool IsTutorialComplete
    {
        get;
        private set;
    }

    //일일 무료 가챠 정보
    public int GachaSingleAdsCount
    {
        get;
        private set;
    } = 1;

    public int GachaSingleAdsRemainCount
    {
        get;
        //private set;
        set;
    }

    private DateTime gachaLastUpdate;
    
    private float bgmVolume;
    public float BgmVolume
    {
        get => bgmVolume;
        set
        {
            bgmVolume = Mathf.Clamp(value, 0.0001f, 1f);
        }
    }

    private float sfxVolume;

    public float SfxVolume
    {
        get => sfxVolume;
        set
        {
            sfxVolume = Mathf.Clamp(value, 0.0001f, 1f);
        }
        
    }
    public int frameRateIndex { get; private set; }
    public LanguageSettingType languageSettingType { get; private set; }

    public PlayerAccountData()
    {
        SaveLoadSystem.Instance.RegisterOnSaveAction(this);
    }

    public void OnTutorialComplete()
    {
        IsTutorialComplete = true;
    }

    public void Save()
    {
        var saveData = SaveLoadSystem.Instance.CurrentSaveData.playerAccountDataSave = new();
        saveData.tutorialCompleted = IsTutorialComplete;

        saveData.gachaSingleAdsRemainCount = GachaSingleAdsRemainCount;
        saveData.gachaLastUpdate = DateTime.Now;
        
        saveData.bgmVolume = SoundManager.Instance.bgmVolume;
        saveData.sfxVolume = SoundManager.Instance.sfxVolume;
        saveData.frameRateIndex = GameDataManager.Instance.frameRateIndex;
        saveData.languageSettingType = (int)GameDataManager.Instance.languageSettingType;
    }

    public void Load()
    {
        GachaSingleAdsRemainCount = GachaSingleAdsCount;
        IsTutorialComplete = false;
    }

    public void Load(PlayerAccountDataSave saveData)
    {
        if (saveData == null)
        {
            Load();
            return;
        }

        IsTutorialComplete = saveData.tutorialCompleted;

        if (DateTime.Today > saveData.gachaLastUpdate)
        {
            Load();
        }
        else
        {
            GachaSingleAdsRemainCount = saveData.gachaSingleAdsRemainCount;
        }

        BgmVolume = saveData.bgmVolume;
        SoundManager.Instance.SetBgmVolume(BgmVolume);
        
        SfxVolume = saveData.sfxVolume;
        SoundManager.Instance.SetSfxVolume(SfxVolume);
        
        frameRateIndex = saveData.frameRateIndex;
        GameDataManager.Instance.SetFrameRateIndex(frameRateIndex);
        
        languageSettingType = (LanguageSettingType)saveData.languageSettingType;
        GameDataManager.Instance.SetLanguageSettingType(languageSettingType);
    }
}