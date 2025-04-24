using UnityEngine;
using UnityEditor;
using static Codice.Client.Commands.WkTree.WorkspaceTreeNode;

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

        //스크립터블 데이터 수정용 
        GUILayout.Label("Scriptable Data Edit");

        Editor editor = Editor.CreateEditor(playerStatus.statData);
        editor.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Reset to CSV data");
        //데이터 테이블 기존 값으로 
        if (GUILayout.Button("Revert"))
        {
            playerStatus.statData.SetData(DataTableManager.animalDataTable.Get(playerStatus.statData.AnimalID));
        }
        GUILayout.EndHorizontal();
    }
}

