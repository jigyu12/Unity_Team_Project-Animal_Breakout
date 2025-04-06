using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OutGameUIManager))]
public class OutGameUIManagerEditor : Editor
{
    private int addExpValue;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Add Experience Test", EditorStyles.boldLabel);
        
        addExpValue = EditorGUILayout.IntField("Experience Amount", addExpValue);

        if (GUILayout.Button("Add Exp"))
        {
            if (Application.isPlaying)
            {
                OutGameUIManager.onExpChanged?.Invoke(addExpValue);
                Debug.Log($"Added exp: {addExpValue}");
            }
            else
            {
                Debug.LogWarning("Exp can only be added while the game is running.");
            }
        }
    }
}