using System.Collections.Generic;
using UnityEngine;

public class PrefabSelector : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;
    
    public int PrefabsCount => prefabs.Count;

    public GameObject GetPrefab(int index)
    {
        if (index < 0 || index >= prefabs.Count)
            return null;
        
        return prefabs[index];
    }
    
    public GameObject GetRandomPrefab()
    {
        return prefabs[Random.Range(0, prefabs.Count)];
    }
    
    public GameObject GetRandomPrefab(int minInclusive, int maxExclusive)
    {
        if (minInclusive < 0 || maxExclusive > prefabs.Count)
            return null;
        
        return prefabs[Random.Range(minInclusive, maxExclusive)];
    }
}