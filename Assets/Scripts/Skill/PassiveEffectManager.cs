
using UnityEngine;

public enum PassiveType
{
    None,
    SkillDamage,
    AttackPower,
    ItemValue,
    ResultScoreUp,
    CoinValue,
}

public class PassiveEffectManager : InGameManager
{

    public void PerformGlobalPassiveValues()
    {
        foreach (var data in GameDataManager.Instance.AnimalUserDataList.AnimalUserDatas)
        {
            //보유중인 캐릭터만 패시브 발동
            if (data.IsUnlock)
            {
                PerformPassive(data.AnimalStatData.passive, data.AnimalStatData.Grade, data.Level);
            }
        }
    }

    public void PerformPassive(PassiveType type, int grade, int level)
    {

        PassiveEffectData passiveData = null;
        var datas = DataTableManager.passiveEffectDataTable.Get((int)type, grade);
        for (int i = datas.passiveEffectDatas.Count - 1; i >= 0; i--)
        {
            var data = datas.passiveEffectDatas[i];
            if (level >= data.Level)
            {
                passiveData = data;
                break;
            }
        }

        //기준치보다 낮기때문에 패시브 미발동
        if(passiveData==null)
        {
            return;
        }

        Debug.Log(type.ToString() + "grade : " + grade + ", level : " + passiveData.Level);
        switch (type)
        {
            case PassiveType.SkillDamage:
                {
                    PerformSkillDamage(passiveData);
                    break;
                }
            case PassiveType.AttackPower:
                {
                    PerformAttackPowerUp(passiveData);
                    break;
                }
            case PassiveType.ItemValue:
                {
                    PerformItemScoreUp(passiveData);
                    break;
                }
            case PassiveType.CoinValue:
                {
                    PerformCoinGainUp(passiveData);
                    break;
                }
            case PassiveType.ResultScoreUp:
                {
                    PerformResultScoreUp(passiveData);
                    break;
                }
        }
    }


    private void PerformSkillDamage(PassiveEffectData passiveEffectData)
    {
        GameManager.PlayerManager.playerAttack.AddAdditionalSkillAttackPowerRateValue(passiveEffectData.Value);
    }

    private void PerformAttackPowerUp(PassiveEffectData passiveEffectData)
    {
        GameManager.PlayerManager.playerAttack.AddAdditionalAttackPowerValue(passiveEffectData.Value);
    }

    private void PerformItemScoreUp(PassiveEffectData passiveEffectData)
    {
        GameManager.PlayerManager.playerAttack.AddAdditionalItemValue((int)passiveEffectData.Value);
    }
    private void PerformResultScoreUp(PassiveEffectData passiveEffectData)
    {
        GameManager.InGameCountManager.ScoreSystem.AddAdditionalFinalScoreRate(passiveEffectData.Value);
    }

    private void PerformCoinGainUp(PassiveEffectData passiveEffectData)
    {

    }

}
