using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.ResourceManagement.AsyncOperations;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;



public class StartPlayerModel : MonoBehaviour
{
    private GameObject startAnimalModel=null;
    public float rotationSpeed=5f;

    //[SerializeField]
    //private InputActionReference onTouch;

    private void Start()
    {
        //onTouch.action.performed += OnTouch;

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
                startAnimalModel.GetComponent<PlayerEyeExpressionController>().SetEyeExpression(PlayerEyeState.Excited);

            }
            else
            {
                Debug.LogError($"Failed to load character prefab for ID {addressableID}");
            }
        };
    }

    //public void OnTouch(InputAction.CallbackContext context)
    //{
    //    if(context.ReadValue<TouchState>().phase!=TouchPhase.Began)
    //    {
    //        return;
    //    }

    //    Ray ray;
    //    RaycastHit hit;

    //    var position = context.ReadValue<TouchState>().position;
    //    ray = Camera.main.ScreenPointToRay(position);
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        if(hit.collider.gameObject==gameObject)
    //        {
    //            Debug.Log("터취");
    //        }
    //    }
    //}
}
