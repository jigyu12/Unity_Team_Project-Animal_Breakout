using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour, IManager
{
    protected GameManager_new GameManager
    {
        get;
        private set;
    }

    virtual public void SetGameManager(GameManager_new gameManager)
    {
        GameManager = gameManager;
    }

    virtual public void Initialize()
    {

    }

    virtual public void Clear()
    {

    }

}
