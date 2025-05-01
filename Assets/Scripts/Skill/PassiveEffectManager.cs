

using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

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

    public PassiveEffectDataTable.PassiveEffectData PassiveEffectData
    {
        get;
        private set;
    }

    public void InitializePassiveEffectData(PassiveType type, int grade, int level)
    {
        Debug.Log(type.ToString() + "grade : " + grade + ", level : " + level);

        var datas = DataTableManager.passiveEffectDataTable.Get((int)type, grade);
        foreach (var data in datas.passiveEffectDatas)
        {
            if (level >= data.Level)
            {
                PassiveEffectData = data;
            }
        }

        Perform();
    }

    public void Perform()
    {
        //switch ((PassiveType)PassiveEffectData.PassiveType)
        //{
        //    case PassiveType.SkillDamage:
        //        {
        //            PerformSkillDamage();
        //            break;
        //        }
        //    case PassiveType.AttackPower:
        //        {
        //            PerformAttackPowerUp();
        //            break;
        //        }
        //    case PassiveType.ItemValue:
        //        {
        //            PerformItemScoreUp();
        //            break;
        //        }

        //    case PassiveType.CoinValue:
        //        {
        //            PerformCoinGainUp();
        //            break;
        //        }
        //    case PassiveType.ResultScoreUp:
        //        {
        //            PerformResultScoreUp();
        //            break;
        //        }
        //}
    }


    private void PerformSkillDamage()
    {
        GameManager.PlayerManager.playerAttack.AddAdditionalSkillAttackPowerRateValue(PassiveEffectData.Value);
    }

    private void PerformAttackPowerUp()
    {
        GameManager.PlayerManager.playerAttack.AddAdditionalAttackPowerValue(PassiveEffectData.Value);
    }

    private void PerformItemScoreUp()
    {
        GameManager.PlayerManager.playerAttack.AddAdditionalItemValue((int)PassiveEffectData.Value);
    }
    private void PerformResultScoreUp()
    {

    }

    private void PerformCoinGainUp()
    {

    }

}
