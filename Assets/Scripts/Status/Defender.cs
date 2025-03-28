using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : Power
{
    public PowerDisplay powerDisplay;
    void Start()
    {
        powerDisplay.Init(this);

        power = 300;
    }
}
