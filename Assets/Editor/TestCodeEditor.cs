using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestCode))]
public class TestCodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TestCode testCode = (TestCode)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("프리팹 인스턴스 추가", EditorStyles.boldLabel);

        if (GUILayout.Button("리스트1 프리팹 생성"))
        {
            testCode.InstantiatePrefab1();
        }

        if (GUILayout.Button("리스트2 프리팹 생성"))
        {
            testCode.InstantiatePrefab2();
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("랜덤 오브젝트 제거", EditorStyles.boldLabel);

        if (GUILayout.Button("리스트1에서 랜덤 제거"))
        {
            testCode.DestroyRandomObjFromList1();
        }

        if (GUILayout.Button("리스트2에서 랜덤 제거"))
        {
            testCode.DestroyRandomObjFromList2();
        }
    }
}