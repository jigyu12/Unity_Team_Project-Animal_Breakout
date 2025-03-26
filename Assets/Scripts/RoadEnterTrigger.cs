using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadEnterTrigger : MonoBehaviour
{
    public Action onRoadEnter;
    private bool triggered;

    public void OnTriggerEnter(Collider other)
    {
        if (triggered)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        triggered = true;
        onRoadEnter?.Invoke();
    }

    public void Reset()
    {
        triggered = false;
        onRoadEnter = null;
    }
}
