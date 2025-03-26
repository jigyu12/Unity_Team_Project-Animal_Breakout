using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RoadChunkRotator : MonoBehaviour
{
    public float rotateDuration = 5f;

    private RoadManager roadManager;
    private PlayerMove player;

    private bool isRotating = false;


    private void Awake()
    {
        roadManager = GetComponent<RoadManager>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        player.onRotate += Rotate;
    }

    public void Rotate(Vector3 pivot, float angle)
    {
        if (!isRotating)
        {
            StartCoroutine(RotateRoutine(pivot, angle));
        }
    }

    private void Update()
    {

    }

    private IEnumerator RotateRoutine(Vector3 pivot, float angle)
    {
        isRotating = true;

        Scroll scroll = player.transform.parent.GetComponent<Scroll>();
        if (scroll != null) scroll.enabled = false;
        float elapsed = 0f;

        float currentAngle = 0f;
        while (elapsed < rotateDuration)
        {
            float deltaAngle = (angle / rotateDuration) * Time.deltaTime;

            roadManager.currentRoadChunk.RotateAround(pivot, deltaAngle);
            foreach(var next in roadManager.currentRoadChunk.NextRoadChunks)
            {
                next.RotateAround(pivot, deltaAngle);
            }

            currentAngle += deltaAngle;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 오차 보정
        float remainingAngle = angle - currentAngle;
        transform.RotateAround(pivot, Vector3.up, remainingAngle);

        if (scroll != null) scroll.enabled = true;

        isRotating = false;
    }

}
