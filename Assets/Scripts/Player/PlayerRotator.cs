using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    public float rotateDuration = 5f;
    private PlayerMove playerMove;

    public Action onRotationStart;
    public Action onRotationEnd;

    private bool isRotating = false;
    public void SetPlayerMove(PlayerMove plauerMove)
    {
        this.playerMove = plauerMove;
        this.playerMove.onRotate += Rotate;
    }

    public void Rotate(Vector3 pivot, float angle)
    {
        if (!isRotating)
        {
            StartCoroutine(RotateRoutine(pivot, angle));
        }
    }

    private IEnumerator RotateRoutine(Vector3 pivot, float angle)
    {
        isRotating = true;
        onRotationStart?.Invoke();
        playerMove.enabled = false;
        MoveForward moveForward = playerMove.transform.parent.GetComponent<MoveForward>();
        moveForward.enabled = false;

        //임시 처리
        moveForward.transform.position = pivot;
        

        float elapsed = 0f;
        float currentAngle = 0f;
        while (elapsed < rotateDuration)
        {
            float deltaAngle = (elapsed / rotateDuration) * angle - currentAngle;

            //playerMove.transform.Rotate(new Vector3(0, deltaAngle, 0), Space.Self);
            moveForward.transform.Rotate(new Vector3(0, deltaAngle, 0));

            currentAngle += deltaAngle;
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }


        float remainingAngle = angle - currentAngle;
        moveForward.transform.Rotate(new Vector3(0, remainingAngle, 0));
        if (moveForward != null) moveForward.enabled = true;

        isRotating = false;
        playerMove.enabled = true;


        moveForward.RotateForwardDirection(angle);
        onRotationEnd?.Invoke();

    }
}
