using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StaminaSystemSave
{
    public DateTime lastStaminaAddTime; //DateTime.UtcNow; 로 저장한다
    public int currentStamina;
}
