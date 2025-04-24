
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElement : MonoBehaviour
{
    protected GameUIManager gameUIManager;
    protected GameManager_new gameManager;

    public void SetUIManager(GameManager_new gameManager, GameUIManager uIManager)
    {
        gameUIManager = uIManager;
        this.gameManager = gameManager;
    }

    public virtual void Initialize()
    {


    }

    public virtual void Show()
    {

    }
}