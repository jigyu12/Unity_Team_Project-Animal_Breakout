using System;

[Serializable]
public class PlayerAccountDataSave
{
    public bool tutorialCompleted;

    public int gachaSingleAdsRemainCount;
    public DateTime gachaLastUpdate;
    
    public float bgmVolume;
    public float sfxVolume;
    public int frameRateIndex;
    public int languageSettingType;
}