using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelLabel : MonoBehaviour
{
    [SerializeField] private LocalizedTextBinder levelBinder;

    private void Awake()
    {
        int initialLevel = 1;
        levelBinder.SetArgumentsAndRefresh(initialLevel);  // "Blizzard LV.{0}" → "Blizzard LV.1"
    }
}
