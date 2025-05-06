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

    public void AddGold(long value)
    {
        Debug.Log($"Add gold : {value}");

        CurrentGolds += value;
        CurrentGolds = Math.Clamp(CurrentGolds, minGold, maxGold);
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
