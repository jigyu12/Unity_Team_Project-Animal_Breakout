
using UnityEditor;
using UnityEngine;


public class GoogleSheetCSVWindow : EditorWindow
{
    private string fileName;
    private string sheetURL;

    [MenuItem("Window/GoogleSheet CSV Window")]
    public static void ShowWindow()
    {
        GetWindow<GoogleSheetCSVWindow>();
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("GoogleSheet URL");
        sheetURL = GUILayout.TextField(sheetURL);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Load"))
        {
            string path = EditorUtility.SaveFilePanel("Save GoogleSheet to CSV", "", "", "csv");

            if (!string.IsNullOrEmpty(path))
            {
                GoogleSheetManager.Load(sheetURL, path);
                AssetDatabase.Refresh();
            }
        }

        GUILayout.Label("");

        LoadDataTableGUI(animalDataTableURL, Utils.AnimalTableName);

        LoadDataTableGUI(itemDataTableURL, Utils.ItemTableName);
    }

    private void UpdateDataTableCSV(string url, string path)
    {
        GoogleSheetManager.Load(url, path);
        AssetDatabase.Refresh();
    }

    private string animalDataTableURL = "https://docs.google.com/spreadsheets/d/1lgeY8ZIuS4VGB0Ii2VdqcRd126eV1GDp4h0aw2hoVBA/edit?gid=1280379651#gid=1280379651";
    private string itemDataTableURL = "https://docs.google.com/spreadsheets/d/1lgeY8ZIuS4VGB0Ii2VdqcRd126eV1GDp4h0aw2hoVBA/edit?gid=100321918#gid=100321918";

    //private void UpdateAnimalDataTable()
    //{
    //    var path = System.IO.Path.Combine(Application.dataPath, "Resources/") + string.Format(DataTable.FormatPath, Utils.AnimalTableName) + ".csv";

    //    GUILayout.BeginHorizontal();
    //    GUILayout.Label(Utils.AnimalTableName);
    //    if (GUILayout.Button("Load"))
    //    {
    //        UpdateDataTableCSV(animalDataTableURL, path);
    //    }
    //    GUILayout.EndHorizontal();
    //}

    private void LoadDataTableGUI(string url, string dataTableFileName)
    {
        var path = System.IO.Path.Combine(Application.dataPath, "Resources/") + string.Format(DataTable.FormatPath, dataTableFileName) + ".csv";

        GUILayout.BeginHorizontal();
        GUILayout.Label(dataTableFileName);
        if (GUILayout.Button("Load"))
        {
            UpdateDataTableCSV(url, path);
        }
        GUILayout.EndHorizontal();
    }
}

