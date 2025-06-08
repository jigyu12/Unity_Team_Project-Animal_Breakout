using AmazingAssets.CurvedWorld;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedWorld : MonoBehaviour
{
    //public float curvature;
    //public float trimming;
    public StageManager stageManager;

    public float maxHorizonCurvatureValue;
    public float maxVirticalCurvatureValue;

    public AmazingAssets.CurvedWorld.CurvedWorldController curvedWorldController;



    private void Start()
    {
        //Shader.EnableKeyword("CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_Z_POSITIVE");
    }

    private Vector3 currentForward;
    private bool right;

    private void Update()
    {
        if (curvedWorldController.bendPivotPoint.forward != currentForward)
        {
            currentForward = curvedWorldController.bendPivotPoint.forward;
            UpdateHorizontalShaderValue(right, 1f);
            right = !right;
        }
        UpdateShaderValue();
    }


    private float startValue;
    private float endValue;
    private float durationTime = 0;
    private float lerpTime = -1;

    public void UpdateHorizontalShaderValue(bool r, float time)
    {
        startValue = curvedWorldController.bendHorizontalSize;
        endValue = r ? maxHorizonCurvatureValue : -maxHorizonCurvatureValue;
        durationTime = time;
        lerpTime = 0;
    }


    [ContextMenu("Update Shader Value")]
    public void UpdateShaderValue()
    {
        if (lerpTime < 0)
        {
            return;
        }

        lerpTime += Time.deltaTime;
        if (lerpTime > durationTime)
        {
            curvedWorldController.SetBendHorizontalSize(endValue);
            lerpTime = -1;
        }
        else
        {
            curvedWorldController.SetBendHorizontalSize(Mathf.Lerp(curvedWorldController.bendHorizontalSize, endValue, lerpTime / durationTime));
        }
    }

    //[ContextMenu("X+")]
    //public void SetPositiveX()
    //{
    //    Shader.DisableKeyword("CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_Z_POSITIVE");
    //    Shader.EnableKeyword("CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_X_POSITIVE");
    //}
    //[ContextMenu("Z+")]
    //public void SetPositiveZ()
    //{
    //    Shader.DisableKeyword("CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_X_POSITIVE");
    //    Shader.EnableKeyword("CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_Y_POSITIVE");
    //}
}
