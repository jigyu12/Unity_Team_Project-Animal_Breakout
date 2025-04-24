using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(SafeAreaCanvas))]
public class SafeAreaCanvasEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var safeAreaCanvas = (SafeAreaCanvas)target;
        GUILayout.BeginHorizontal();
        GUILayout.Label("If Changed Simulator...");
        if (GUILayout.Button("Apply SafeArea"))
        {
            safeAreaCanvas.ApplySafeAreaCanvasAnchor();
        }
        if (GUILayout.Button("Log SafeArea position"))
        {
            Debug.Log($"SafeArea{safeAreaCanvas.transform.position}");
            Debug.Log($"SafeArea{safeAreaCanvas.rectTransform.anchoredPosition}");
            Debug.Log($"SafeArea{safeAreaCanvas.rectTransform.rect}");
            //Debug.Log($"SafeArea{Screen.safeArea);
        }
        GUILayout.EndHorizontal();

        base.OnInspectorGUI();
    }
}
