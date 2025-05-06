using System;
using UnityEngine;

public class GoldAnimalTokenSystem
{
   public long CurrentGolds
    {
        get;
        private set;
    }

    public const long minGold = 0;
    public const long maxGold = 99999999999;

    public int CurrentBronzeToken
    {
        get;
        private set;
    }

    public int CurrentSliverToken
    {
        get;
        private set;
    }

    public int CurrentGoldToken
    {
        get;
        private set;
    }
    
    public const int minToken = 0;
    public const int maxToken = 9999;

    public static Action<long> onGoldChanged;
    public static Action<int> onBronzeTokenChanged;
    public static Action<int> onSliverTokenChanged;
    public static Action<int> onGoldTokenChanged;

    public void SetInitialValue(long gold, int bronzeToken, int sliverToken, int goldToken)
    {
        CurrentGolds = gold;
        CurrentBronzeToken = bronzeToken;
        CurrentSliverToken = sliverToken;
        CurrentGoldToken = goldToken;
    }
    
    public void AddGold(long value)
    {
        CurrentGolds += value;
        CurrentGolds = Math.Clamp(CurrentGolds, minGold, maxGold);

        Debug.Log($"Add gold : {value}, CurrentGolds : {CurrentGolds}");
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

    public void AddBronzeToken(int value)
    {
        CurrentBronzeToken += value;
        CurrentBronzeToken = Math.Clamp(CurrentBronzeToken, minToken, maxToken);
        
        Debug.Log($"Add BronzeToken : {value}, CurrentBronzeToken : {CurrentBronzeToken}");
        onBronzeTokenChanged?.Invoke(CurrentBronzeToken);
    }
    
    public void PayBronzeToken(int value)
    {
        if(CurrentBronzeToken < value)
        {
            Debug.Log($"Not enough bronze token! -> current bronze token : {CurrentBronzeToken}, payment : {value} X");
            return;
        }
        
        CurrentBronzeToken -= value;
        Debug.Log($"pay bronze token success! -> current bronze token : {CurrentBronzeToken}");
        onBronzeTokenChanged?.Invoke(CurrentBronzeToken);
    }
    
    public void AddSliverToken(int value)
    {
        CurrentSliverToken += value;
        CurrentSliverToken = Math.Clamp(CurrentSliverToken, minToken, maxToken);
        
        Debug.Log($"Add SliverToken : {value}, CurrentSliverToken : {CurrentSliverToken}");
        onSliverTokenChanged?.Invoke(CurrentSliverToken);
    }
    
    public void PaySliverToken(int value)
    {
        if(CurrentSliverToken < value)
        {
            Debug.Log($"Not enough sliver token! -> current sliver token : {CurrentSliverToken}, payment : {value} X");
            return;
        }
        
        CurrentSliverToken -= value;
        Debug.Log($"pay sliver token success! -> current sliver token : {CurrentSliverToken}");
        onSliverTokenChanged?.Invoke(CurrentSliverToken);
    }
    
    public void AddGoldToken(int value)
    {
        CurrentGoldToken += value;
        CurrentGoldToken = Math.Clamp(CurrentGoldToken, minToken, maxToken);
        
        Debug.Log($"Add GoldToken : {value}, CurrentGoldToken : {CurrentGoldToken}");
        onGoldTokenChanged?.Invoke(CurrentGoldToken);
    }

    public void PayGoldToken(int value)
    {
        if (CurrentGoldToken < value)
        {
            Debug.Log($"Not enough gold token! -> current gold token : {CurrentGoldToken}, payment : {value} X");
            return;
        }

        CurrentGoldToken -= value;
        Debug.Log($"pay gold token success! -> current gold token : {CurrentGoldToken}");
        onGoldTokenChanged?.Invoke(CurrentGoldToken);
    }
}