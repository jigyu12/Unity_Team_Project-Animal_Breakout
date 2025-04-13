using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameDataManager))]
public class GameDataManagerEditor : Editor
{
    private int staminaAmountToAdd = 5;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Stamina Management Tool", EditorStyles.boldLabel);

        staminaAmountToAdd = EditorGUILayout.IntField("Stamina Amount to Add", staminaAmountToAdd);

        if (GUILayout.Button($"Increase Stamina by {staminaAmountToAdd}"))
        {
            if (Application.isPlaying)
            {
                var gameDataManager = (GameDataManager)target;
                gameDataManager.IncreaseStamina(staminaAmountToAdd);

                EditorUtility.SetDirty(gameDataManager);
            }
            else
            {
                Debug.LogWarning("Stamina can only be increased while the game is running.");
            }
        }
    }
}