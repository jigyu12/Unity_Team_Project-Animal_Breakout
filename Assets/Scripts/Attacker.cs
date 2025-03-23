using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Attacker : Power
{
    public PowerDisplay powerDisplay;
    
    public bool Attack(Power defender)
    {
        return defender.power < power;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Power defender))
        {
            bool result = Attack(defender);
            Debug.Log($"공격 성공 여부 : {result}");
        }
    }
}
