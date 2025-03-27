using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesTest : MonoBehaviour
{
    public string testKey = "CharacterModel_10101"; // Addressables에서 테스트할 키

    void Start()
    {
        TestLoadModel();
    }

    void TestLoadModel()
    {
        Addressables.InstantiateAsync(testKey).Completed += (obj) =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"✅ Addressables 로드 성공: {testKey}");
            }
            else
            {
                Debug.LogError($"❌ Addressables 로드 실패: {testKey}");
            }
        };
    }
}
