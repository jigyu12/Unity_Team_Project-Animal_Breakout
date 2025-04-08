using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraManagerExampleUsage))]
public class CameraManagerExampleUsageEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CameraManagerExampleUsage cameraManagerExampleUsage = (CameraManagerExampleUsage)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Virtual Camera Control", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Activate Cam 0"))
        {
            if (Application.isPlaying)
            {
                GameObject.FindGameObjectWithTag("GameManager").TryGetComponent(out GameManager_new gameManager);

                gameManager.CameraManager.ActivateCameraByIndex(0);
            }
            else
            {
                Debug.LogWarning("Camera can only be changed while the game is running.");
            }
        }

        if (GUILayout.Button("Activate Cam 1"))
        {
            if (Application.isPlaying)
            {
                GameObject.FindGameObjectWithTag("GameManager").TryGetComponent(out GameManager_new gameManager);

                gameManager.CameraManager.ActivateCameraByIndex(1);
            }
            else
            {
                Debug.LogWarning("Camera can only be changed while the game is running.");
            }
        }

        if (GUILayout.Button("Activate Cam 2"))
        {
            if (Application.isPlaying)
            {
                GameObject.FindGameObjectWithTag("GameManager").TryGetComponent(out GameManager_new gameManager);

                gameManager.CameraManager.ActivateCameraByIndex(2);
            }
            else
            {
                Debug.LogWarning("Camera can only be changed while the game is running.");
            }
        }

        EditorGUILayout.EndHorizontal();
    }
}