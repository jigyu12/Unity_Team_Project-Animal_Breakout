using System;
using UnityEngine;

public class GoldTokenSystem
{
   public long CurrentGolds
    {
        get;
        private set;
    }

    public const long minGold = 0;
    public const long maxGold = 99999999999;

    public static Action<long> onGoldChanged;

    public void SetInitialValue(int gold, int token)
    {
        CurrentGolds = gold;
    }


    public void AddGold(long value)
    {
        CurrentGolds += value;
        CurrentGolds = Math.Clamp(CurrentGolds, minGold, maxGold);

        Debug.Log($"Add gold : {value}");
        onGoldChanged?.Invoke(CurrentGolds);
    }

    public void PayGold(long value)
    {
        if(CurrentGolds<value)
        {
            Debug.Log($"Not enough gold! -> current gold : {CurrentGolds}, payment : {value} X");
            return;
        }

        CurrentGolds -= value;
        Debug.Log($"pay gold success! -> current gold : {CurrentGolds}");
        onGoldChanged?.Invoke(CurrentGolds);
    }


}
