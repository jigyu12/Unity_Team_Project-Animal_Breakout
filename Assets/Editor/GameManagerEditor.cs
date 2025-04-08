using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager_new))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var gm = (GameManager_new)target;

        GUILayout.Label("Current : "+ gm.GetCurrentGameState().ToString());
        foreach(GameManager_new.GameState gameState in Enum.GetValues(typeof(GameManager_new.GameState)))
        {
            if(GUILayout.Button(gameState.ToString()))
            {
                gm.SetGameState(gameState);
            }
        }

        base.OnInspectorGUI();
    }
}
