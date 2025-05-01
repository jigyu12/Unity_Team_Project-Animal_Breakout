using UnityEngine;
using UnityEditor;

public class AnimalDataLoader : MonoBehaviour
{
    public string csvFileName = "Animal_Table";

    [ContextMenu("Load and Save Animal Data")]
    public void LoadAndSaveData()
    {
        var path = $"Tables/{csvFileName}";

        var textAsset = Resources.Load<TextAsset>(path);

        if (textAsset == null)
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다.");
            return;
        }

#if UNITY_EDITOR
        var animalList = AnimalDataTable.LoadCSV<AnimalDataTable.AnimalRawData>(textAsset.text);
        foreach (var animal in animalList)
        {
            // ScriptableObject 생성
            AnimalStatData asset = ScriptableObject.CreateInstance<AnimalStatData>();
            asset.AnimalID = animal.AnimalID;
            asset.StringID = animal.StringID;
            asset.Grade = animal.Grade;
            asset.AttackPower = animal.AttackPower;
            asset.StartSpeed = animal.StartSpeed;
            asset.MaxSpeed = animal.MaxSpeed;
            asset.Jump = animal.Jump;

            // 경로 설정
            string assetPath = $"Assets/Resources/Stats/Animal_{animal.AnimalID}.asset";
            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();

            Debug.Log($"AnimalStatData created: {assetPath}");
        }
#endif
    }
}
