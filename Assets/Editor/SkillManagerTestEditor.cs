using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillManagerTest))]
public class SkillManagerTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var dataEditor = (SkillManagerTest)target;
        if (dataEditor.currentSkillData != null)
        {
            //Editor editor = Editor.CreateEditor(dataEditor.currentSkillData);

            if (Application.isPlaying)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Spawn " + dataEditor.currentSkillData.skillGroup);
                if (GUILayout.Button("Spawn"))
                {
                    dataEditor.AddSkill(dataEditor.currentSkillData);
                }
                GUILayout.EndHorizontal();
            }
            //editor.OnInspectorGUI();
        }

        if (dataEditor.currentSkillDatas != null)
        {
            //Editor editor = Editor.CreateEditor(dataEditor.currentSkillData);

            if (Application.isPlaying)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("SpawnList"))
                {
                    foreach (var data in dataEditor.currentSkillDatas)
                    {
                        dataEditor.AddSkill(data);
                    }
                }
                GUILayout.EndHorizontal();
            }
            //editor.OnInspectorGUI();
        }
    }
}
