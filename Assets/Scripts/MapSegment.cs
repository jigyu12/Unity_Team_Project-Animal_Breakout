using UnityEngine;

public class MapSegment : MonoBehaviour
{
    public Transform exitPoint;
    public MapSpawn mapSpawn;

    bool triggered;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        mapSpawn.SpawnNextSegment();
        //mapSpawn.RemoveOldestSegment();
    }

}
