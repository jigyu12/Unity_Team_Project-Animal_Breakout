using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OutGameUIManager))]
public class OutGameUIManagerEditor : Editor
{
    private int addExpValue;
    
    private SwitchableCanvasType switchableCanvasType;
    private bool isVisibleOtherCanvas;
    private bool isVisibleShowCanvasType = true; 
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Add Experience Test", EditorStyles.boldLabel);
        
        addExpValue = EditorGUILayout.IntField("Experience Amount", addExpValue);

        if (GUILayout.Button("Add Exp"))
        {
            if (Application.isPlaying)
            {
                OutGameUIManager.onExpChanged?.Invoke(addExpValue);
                Debug.Log($"Added exp: {addExpValue}");
            }
            else
            {
                Debug.LogWarning("Exp can only be added while the game is running.");
            }
        }
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Editor Canvas Visualization Test", EditorStyles.boldLabel);

        switchableCanvasType = (SwitchableCanvasType)EditorGUILayout.EnumPopup("Show Canvas Type", switchableCanvasType);
        isVisibleOtherCanvas = EditorGUILayout.Toggle("Is Visible Other Canvas", isVisibleOtherCanvas);
        isVisibleShowCanvasType = EditorGUILayout.Toggle("Is Visible Show Canvas Type", isVisibleShowCanvasType);

        if (GUILayout.Button("Invoke Visualize Canvas"))
        {
            InvokeVisualizeCanvasInEditor();
        }
    }
    
    private void InvokeVisualizeCanvasInEditor()
    {
        var allSwitchableCanvases = FindObjectsOfType<SwitchableCanvas>(true);

        foreach (var canvas in allSwitchableCanvases)
        {
            bool visualizeThisCanvas = canvas.switchableCanvasType == switchableCanvasType
                ? isVisibleShowCanvasType
                : isVisibleOtherCanvas;

            var canvasGroup = canvas.GetComponent<CanvasGroup>();

            canvasGroup.alpha = visualizeThisCanvas ? 1f : 0f;
            canvasGroup.interactable = visualizeThisCanvas;
            canvasGroup.blocksRaycasts = visualizeThisCanvas;

            EditorUtility.SetDirty(canvasGroup);
            EditorUtility.SetDirty(canvas);
        }
    }
}