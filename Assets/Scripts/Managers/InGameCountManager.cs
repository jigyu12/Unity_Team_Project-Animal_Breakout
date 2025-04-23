using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCountManager : InGameManager
{

    public int coinCount;

    private void Awake()
    {
        BaseCollisionBehaviour.OnCoinAcquired += OnCoinAcquiredHandler;
    }

    private void OnDestroy()
    {
        BaseCollisionBehaviour.OnCoinAcquired -= OnCoinAcquiredHandler;   
    }
    
    private void OnCoinAcquiredHandler(int amount)
    {
        coinCount += amount;
    }
}