using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PlayerManager : MonoBehaviour
{
    public Transform modelContainer;
    public AnimalDatabase database;
    public int currentAnimalID;

    private GameObject currentModel;

    private void Start()
    {
        //  LoadCharacterModel(currentAnimalID);
    }

    public void LoadCharacterModel(int animalID, System.Action<PlayerStatus1> onLoaded = null)
{
    AnimalStatus character = database.GetAnimalByID(animalID);
    if (character == null) return;

    currentAnimalID = animalID;
    PlayerPrefs.SetInt("SelectedAnimalID", currentAnimalID);

    if (currentModel != null)
    {
        Addressables.ReleaseInstance(currentModel);
    }

    Addressables.InstantiateAsync(character.PrefabKey).Completed += (handle) =>
    {
        if (handle.Status != AsyncOperationStatus.Succeeded) return;

        currentModel = handle.Result;

        var status = currentModel.GetComponent<PlayerStatus1>();
        if (status != null)
        {
            status.Init(animalID, database);
            onLoaded?.Invoke(status); // ✅ 여기서 콜백 실행
        }

        var move = currentModel.GetComponent<PlayerMove>();
        if (move != null)
        {
            move.Init(
                FindObjectOfType<MapWay>(),
                FindObjectOfType<MapSpawn>()
            );
        }

        var attacker = currentModel.GetComponent<Attacker>();
        if (attacker != null && attacker.powerDisplay != null)
        {
            attacker.powerDisplay.Init(attacker);
        }

        var animator = currentModel.GetComponent<Animator>();
        if (animator != null)
            GetComponent<Animator>().runtimeAnimatorController = animator.runtimeAnimatorController;
    };
}

}
