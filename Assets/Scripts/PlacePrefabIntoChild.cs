using UnityEngine;

public class PlacePrefabIntoChild : MonoBehaviour
{
    [SerializeField] private PrefabSelector prefabSelector;
    private GameObject prefab;

    private void OnEnable()
    {
        var newPrefab = Instantiate(prefabSelector.GetRandomPrefab(0, prefabSelector.PrefabsCount - 1), transform);
        prefab = newPrefab;
        prefab.transform.localScale = new Vector3(1f / transform.localScale.x, 1f / transform.localScale.y, 1f / transform.localScale.z);
    }

    private void OnDisable()
    {
        Destroy(prefab);
    }
}