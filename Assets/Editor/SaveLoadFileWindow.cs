using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveLoadFileWindow : EditorWindow
{
    [MenuItem("Window/SaveLoad File Window")]
    public static void ShowWindow()
    {
        GetWindow<SaveLoadFileWindow>();
    }

    void OnGUI()
    {
        GUILayout.Label("Save File");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Remove CurrentSaveFile");
        if (GUILayout.Button("Remove"))
        {
            var path = Path.Combine(SaveLoadSystem.SavePathDirectory, SaveLoadSystem.CurrentSaveFileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Open CurrentSaveFile");
        if (GUILayout.Button("Open"))
        {
            var path = Path.Combine(SaveLoadSystem.SavePathDirectory, SaveLoadSystem.CurrentSaveFileName);
            if (File.Exists(path))
            {
                EditorUtility.OpenFilePanel("Save File Directory", SaveLoadSystem.SavePathDirectory, "json");
            }
        }
        GUILayout.EndHorizontal();
    }
}
