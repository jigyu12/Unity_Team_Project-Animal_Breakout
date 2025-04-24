using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BossStatus))]
public class BossStatusEditor : Editor
{
    private float damageToApply = 10f;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BossStatus bossStatus = (BossStatus)target;

        GUILayout.Space(10);
        GUILayout.Label("Test Damage", EditorStyles.boldLabel);

        damageToApply = EditorGUILayout.FloatField("Damage Amount", damageToApply);
        
        if (GUILayout.Button("Apply Damage"))
        {
            if (Application.isPlaying)
            {
                bossStatus.OnDamage(damageToApply);
            }
            else
            {
                Debug.LogWarning("You must be in Play mode to apply damage!");
            }
        }
    }
}