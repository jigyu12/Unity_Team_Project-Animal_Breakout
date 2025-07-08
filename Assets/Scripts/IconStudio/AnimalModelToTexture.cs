using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteAlways]
public class AnimalModelToTexture : MonoBehaviour
{
    public Camera TargetCamera;
    public List<GameObject> GameObjectToTextureQueue = new();
    private Queue<GameObject> textureQueue = new();

    public string icontexturePath = "Assets/Resources/Textures/PlayerIcon/PlayerFace/{0}Face2";

    private bool isProcessing = false;

    public void PushToGameObjectToTextureQueue(GameObject gobj)
    {
        GameObjectToTextureQueue.Add(gobj);
    }

    public void StartCapture()
    {
        textureQueue.Clear();
        

        foreach (var gobj in GameObjectToTextureQueue)
        {
            var target = Instantiate(gobj);
            target.name = gobj.name;
            target.transform.position = Vector3.zero;
            target.SetActive(false);
            textureQueue.Enqueue(target);
        }

        CaptureRenderTexture();
    }

    private void ProcessQueue()
    {
        if (textureQueue.Count == 0)
        {
            isProcessing = false;
            EditorApplication.update -= ProcessQueue;
            AssetDatabase.Refresh();
            return;
        }

        var gobj = textureQueue.Dequeue();
        gobj.SetActive(true);

        SceneView.RepaintAll();


        TargetCamera.Render();
        SaveTextureToFileUtility.SaveRenderTextureToFile(TargetCamera.targetTexture, string.Format(icontexturePath, gobj.name));

        Debug.Log("Mug shot : " + gobj.name);
        gobj.SetActive(false);
        DestroyImmediate(gobj);
    }

    private GameObject currentObj = null;
    private int frameWait = 0;

    private void ProcessQueue2()
    {
        if (currentObj == null)
        {
            if (textureQueue.Count == 0)
            {
                isProcessing = false;
                EditorApplication.update -= ProcessQueue2;
                AssetDatabase.Refresh();
                return;
            }

            currentObj = textureQueue.Dequeue();
            currentObj.SetActive(true);
            currentObj.GetComponent<PlayerEyeExpressionController>().SetEyeExpression(PlayerEyeState.Excited);


            frameWait = 10;
            return;
        }

        if (frameWait > 0)
        {
            frameWait--;
            return;
        }

        SceneView.RepaintAll();

        TargetCamera.Render();
        SaveTextureToFileUtility.SaveRenderTextureToFile(TargetCamera.targetTexture, string.Format(icontexturePath, currentObj.name));

        Debug.Log("Mug shot : " + currentObj.name);
        currentObj.SetActive(false);
        DestroyImmediate(currentObj);
        currentObj = null;
    }

    private void CaptureRenderTexture()
    {
        while (textureQueue.Count != 0)
        {
            currentObj = textureQueue.Dequeue();
            currentObj.SetActive(true);

            SceneView.RepaintAll(); //씬뷰를 강제로 갱신해 확실하게 씬이 현재상태 반영하도록 함
            TargetCamera.Render(); //랜더타겟텍스처에 현재 카메라 화면 캡처
            SaveTextureToFileUtility.SaveRenderTextureToFile(TargetCamera.targetTexture, string.Format(icontexturePath, currentObj.name));

            currentObj.SetActive(false);
            DestroyImmediate(currentObj);
            currentObj = null;
        }

        AssetDatabase.Refresh();
    }

   

    public void Test()
    {
        SaveTextureToFileUtility.SaveRenderTextureToFile(TargetCamera.targetTexture, string.Format(icontexturePath, "temp"));
    }
}

#endif
