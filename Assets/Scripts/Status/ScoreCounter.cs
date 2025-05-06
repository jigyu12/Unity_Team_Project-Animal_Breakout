using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour, IItemTaker
{
    private ScoreSystem scoreSystem;

    public void Initialize(ScoreSystem scoreSystem)
    {
        this.scoreSystem = scoreSystem;
    }

    public void ApplyItem(int value)
    {
        scoreSystem.AddScore(value);
    }
}
