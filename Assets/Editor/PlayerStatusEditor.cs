using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerStatus))]
public class PlayerStatusEditor : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        PlayerStatus playerStatus = (PlayerStatus)target;

        GUILayout.Space(10);

        string buttonLabel = playerStatus.IsInvincible ? "Disable Invincibility" : "Enable Invincibility";

        if (GUILayout.Button("Take 1 Damage"))
        {
            playerStatus.TakeDamage(1);
        }
        if (GUILayout.Button(buttonLabel))
        {
            playerStatus.ToggleInvincible();
            Debug.Log($"무적 상태 토글: {(playerStatus.IsInvincible ? "ON" : "OFF")}");
        }
    }
}
