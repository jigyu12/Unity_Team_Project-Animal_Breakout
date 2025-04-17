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

        //GUILayout.Label("");
        //Editor editor = Editor.CreateEditor(playerStatus.statData);

        //GUILayout.BeginHorizontal();
        //GUILayout.Label("Reset to CSV data");
        ////데이터 테이블 기존 값으로 
        //if (GUILayout.Button("Revert"))
        //{
        //    dataEditor.currentEnemyData.SetData(DataTableManager.EnemyTable.Get(dataEditor.currentEnemyData.Id));
        //}
        //GUILayout.EndHorizontal();
    }
}

