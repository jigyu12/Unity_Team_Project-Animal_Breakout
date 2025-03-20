using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Attacker : Power
{
    public bool Attack(Power defender)
    {
        return defender.power < power;
    }

    public void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.TryGetComponent<Power>(out Power defender))
        {
            bool result = Attack(defender);
            Debug.Log($"АјАн : {result}");
        }
    }
}
