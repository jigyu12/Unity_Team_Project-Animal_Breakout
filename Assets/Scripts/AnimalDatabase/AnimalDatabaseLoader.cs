using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AnimalDatabaseLoader : Singleton<AnimalDatabaseLoader>
{
    public TextAsset csvFile;
    public AnimalDatabase database;

    public void Awake()
    {
        LoadDataFromCSV();
        GameDataManager.Instance.SetAnimalDatabase(database);
        List<int> runnerIDs = new List<int>();
        foreach (AnimalStatus animal in database.Animals)
        {
            runnerIDs.Add(animal.AnimalID);
        }
        GameDataManager.Instance.SetRunnerIDs(runnerIDs);
    }
    // private void Start()
    // {
    //     LoadDataFromCSV();
    //     GameDataManager.Instance.SetAnimalDatabase(database);
    // }

    public void LoadDataFromCSV()
    {
        if (csvFile == null)
        {
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        if (lines.Length <= 1)
        {
            return;
        }

        database.Animals.Clear();

        for (int i = 2; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] values = line.Split(',');

            if (values.Length < 9)
                continue;

            try
            {
                int id = TryParseInt(values[1]);
                string name = values[0];
                string stringID = values[2];
                int grade = TryParseInt(values[3]);
                float attack = TryParseFloat(values[4]);
                float hp = TryParseFloat(values[5]);
                float speed = TryParseFloat(values[6]);
                float jump = TryParseFloat(values[7]);
                string prefabKey = values[8];

                AnimalStatus newAnimal = new AnimalStatus(id, name, stringID, grade, attack, hp, speed, jump, prefabKey);
                database.Animals.Add(newAnimal);
            }
            catch (System.Exception)
            {
                continue;
            }
        }
    }

    int TryParseInt(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0;
        if (int.TryParse(value, out int result))
            return result;
        return 0;
    }

    float TryParseFloat(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0f;
        if (float.TryParse(value, out float result))
            return result;
        return 0f;
    }
}
