using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestCode))]
public class TestCodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TestCode testCode = (TestCode)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("생성 및 파괴 컨트롤", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Prefab 1 생성"))
        {
            testCode.InstantiatePrefab1();
        }
        
        if (GUILayout.Button("Prefab 2 생성"))
        {
            testCode.InstantiatePrefab2();
        }
        
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("리스트 1에서 랜덤 오브젝트 파괴"))
        {
            testCode.DestroyRandomObjFromList1();
        }
        
        if (GUILayout.Button("리스트 2에서 랜덤 오브젝트 파괴"))
        {
            testCode.DestroyRandomObjFromList2();
        }
        
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField($"리스트1 오브젝트 수 : {testCode.objList1.Count}");
        EditorGUILayout.LabelField($"리스트2 오브젝트 수 : {testCode.objList2.Count}");
    }
}