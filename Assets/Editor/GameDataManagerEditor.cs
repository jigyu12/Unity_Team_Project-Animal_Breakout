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
        var gameDataManager = (GameDataManager)target;
        if (GUILayout.Button($"Increase Stamina by {staminaAmountToAdd}"))
        {
            if (Application.isPlaying)
            {
                gameDataManager.StaminaSystem.AddStamina(staminaAmountToAdd);

                EditorUtility.SetDirty(gameDataManager);
            }
            else
            {
                Debug.LogWarning("Stamina can only be increased while the game is running.");
            }
        }
        if (Application.isPlaying)
        {
            GUILayout.Label($"Left Time : {gameDataManager.StaminaSystem.GetLeftTimeToGetNextStamina()}");
        }

        if (GUILayout.Button("Pay 1 Stamina"))
        {
            if (Application.isPlaying)
            {
                gameDataManager.StaminaSystem.PayStamina(1);

                EditorUtility.SetDirty(gameDataManager);
            }
            else
            {
                Debug.LogWarning("Stamina can only be increased while the game is running.");
            }
        }
    }
}