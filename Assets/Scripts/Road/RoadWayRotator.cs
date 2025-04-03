using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoadWayRotator : MonoBehaviour
{
    public float rotateDuration = 5f;

    private TempleRunStyleRoadMaker roadManager;
    private PlayerMove playerMove;

    private bool isRotating = false;
    private Vector3 pivot;

    private void Awake()
    {
        roadManager = GetComponent<TempleRunStyleRoadMaker>();
    }

    private void Start()
    {
        enabled = false;

        var gameManager = GameObject.FindObjectOfType<GameManager>();

        gameManager.onPlayerSpawned += (playerStatus) => GetPlayer(playerStatus.gameObject.GetComponent<PlayerMove>());
        gameManager.onPlayerDied += (playerStatus) => enabled = false;
    }

    public void GetPlayer(PlayerMove playerMove)
    {
        this.playerMove = playerMove;
        playerMove.onRotate += Rotate;
        enabled = true;
    }

    public void Rotate(Vector3 pivot, float angle)
    {
        if (!isRotating)
        {
            this.pivot = pivot;
            StartCoroutine(RotateRoutine(pivot, angle));
        }
    }

    //private void RotateLinkedChunk(RoadWay roadChunk, Vector3 pivot, float angle)
    //{
    //    //roadChunk.RotateAround(pivot, angle);
    //    roadChunk.transform.RotateAround(pivot, Vector3.up, angle);
    //    foreach (var next in roadChunk.NextRoadChunks)
    //    {
    //        //next.RotateAround(pivot, angle);
    //        next.transform.RotateAround(pivot, Vector3.up, angle);
    //    }
    //}

    private IEnumerator RotateRoutine(Vector3 pivot, float angle)
    {
        isRotating = true;
        playerMove.enabled = false;
        MoveForward scroll = playerMove.transform.parent.GetComponent<MoveForward>();
        scroll.enabled = false;

        //StartCoroutine(playerMove.MoveTo(pivot));

        float elapsed = 0f;
        float currentAngle = 0f;
        while (elapsed < rotateDuration)
        {
            float deltaAngle = (elapsed / rotateDuration) * angle - currentAngle;

            //roadManager.transform.RotateAround(playerMove.transform.position, Vector3.up, deltaAngle);
            roadManager.transform.RotateAround(pivot, Vector3.up, deltaAngle);
            //RotateLinkedChunk(roadManager.currentRoadChunk, pivot, deltaAngle);

            currentAngle += deltaAngle;
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }


        float remainingAngle = angle - currentAngle;
        // RotateLinkedChunk(roadManager.currentRoadChunk, pivot, remainingAngle);
        roadManager.transform.RotateAround(pivot, Vector3.up, remainingAngle);
        //roadManager.transform.RotateAround(playerMove.transform.position, Vector3.up, remainingAngle);
        if (scroll != null) scroll.enabled = true;

        isRotating = false;
        playerMove.enabled = true;
    }

    private void OnDrawGizmos()
    {
        if (isRotating)
        {
            Gizmos.color = Color.red;
            var to = pivot;
            to.y = 5f;
            Gizmos.DrawLine(pivot, to);
        }
    }
}
