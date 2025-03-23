using UnityEngine;

public class PlacePrefabIntoChild : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private GameObject prefabInstance;

    private void OnEnable()
    {
        var newPrefab = Instantiate(prefab, transform);
        prefabInstance = newPrefab;
        
        Utils.SetChildScaleFitToParent(prefabInstance, gameObject);
    }

    private void OnDisable()
    {
        Destroy(prefabInstance);
    }
}