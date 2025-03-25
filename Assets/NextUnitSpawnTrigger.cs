using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextUnitSpawnTrigger : MonoBehaviour
{
    [SerializeField]
    private MapUnit ownerUnit;
    private bool triggered;

    void OnTriggerEnter(Collider other)
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
        ownerUnit.mapSpawner.SpawnNextTileChuck(ownerUnit);
    }

    private void OnDisable()
    {
        triggered = false;
    }
}
