using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        enabled = false;
        var relayRunManager = GameObject.FindObjectOfType<RelayRunManager>();

        relayRunManager.onLoadPlayer += (playerStatus) => GetPlayer(playerStatus.gameObject.GetComponent<PlayerMove>());
        relayRunManager.onDiePlayer += (playerStatus) => enabled = false;
    }

    public void GetPlayer(PlayerMove player)
    {
        this.player = player;
        player.onRotate += Rotate;
        enabled = true;
    }

    public void Rotate(Vector3 pivot, float angle)
    {
        if (!isRotating)
        {
            StartCoroutine(RotateRoutine(pivot, angle));
        }
    }

    private void RotateLinkedChunk(RoadChunk roadChunk, Vector3 pivot, float angle)
    {
        //roadChunk.RotateAround(pivot, angle);
        roadChunk.transform.RotateAround(pivot, Vector3.up, angle);
        foreach (var next in roadChunk.NextRoadChunks)
        {
            //next.RotateAround(pivot, angle);
            next.transform.RotateAround(pivot, Vector3.up, angle);
        }
    }

    private IEnumerator RotateRoutine(Vector3 pivot, float angle)
    {
        isRotating = true;
        player.enabled = false;
        MoveFoward scroll = player.transform.parent.GetComponent<MoveFoward>();
        if (scroll != null) scroll.enabled = false;
        float elapsed = 0f;

        float currentAngle = 0f;
        while (elapsed < rotateDuration)
        {
            float deltaAngle = (elapsed / rotateDuration) * angle - currentAngle;

            RotateLinkedChunk(roadManager.currentRoadChunk, pivot, deltaAngle);

            currentAngle += deltaAngle;
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // ���� ����
        float remainingAngle = angle - currentAngle;
        RotateLinkedChunk(roadManager.currentRoadChunk, pivot, remainingAngle);

        if (scroll != null) scroll.enabled = true;

        isRotating = false;
        player.enabled = true;

    }

}
