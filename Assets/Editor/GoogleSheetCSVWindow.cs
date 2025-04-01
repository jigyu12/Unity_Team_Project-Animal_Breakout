using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Test
{
    public int PrefabID { get; set; }
    public int Obj_Type { get; set; }
    public int Coor1 { get; set; }
    public int Coor2 { get; set; }
}

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

        if (GUILayout.Button("Load"))
        {
            string path = EditorUtility.SaveFilePanel("Save GoogleSheet to CSV", "", "", "csv");

            if (!string.IsNullOrEmpty(path))
            {
                GoogleSheetManager.Load<Test>(sheetURL, path);
            }
        }
        GUILayout.EndHorizontal();
    }
}

