using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FitScaleWithParent))]
public class FitScaleWithParentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); 

        FitScaleWithParent fitScaleScript = (FitScaleWithParent)target;

        if (GUILayout.Button("Fit Scale With Parent"))
        {
            fitScaleScript.FitScale();
        }
    }
}