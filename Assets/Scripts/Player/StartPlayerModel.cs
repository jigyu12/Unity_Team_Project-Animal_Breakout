using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StartPlayerModel : MonoBehaviour
{
    private GameObject startAnimalModel=null;
    public float rotationSpeed=5f;

    private void Start()
    {
        GameDataManager.onSetStartAnimalIDInGameDataManager += SetStartPlayerModel;
        SetStartPlayerModel(0,0,GameDataManager.Instance.AnimalUserDataList.CurrentAnimalPlayer);
    }

    private void OnDestroy()
    {
        GameDataManager.onSetStartAnimalIDInGameDataManager -= SetStartPlayerModel;
    }

    private void Update()
    {
        if(startAnimalModel)
        {
            startAnimalModel.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }

    public void SetStartPlayerModel(int a, int b, AnimalUserData currentAnimalUserData)
    {
        var addressableID = currentAnimalUserData.AnimalStatData.Prefab;
        Addressables.LoadAssetAsync<GameObject>(addressableID).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject characterPrefab = handle.Result;
                if(startAnimalModel!=null)
                {
                    GameObject.Destroy(startAnimalModel);
                }

                startAnimalModel = Instantiate(characterPrefab, gameObject.transform);
                startAnimalModel.GetComponent<Animator>().SetTrigger("Idle");
                startAnimalModel.GetComponent<Animator>().SetTrigger("EyeExcited");
            }
            else
            {
                Debug.LogError($"Failed to load character prefab for ID {addressableID}");
            }
        };
    }
}
