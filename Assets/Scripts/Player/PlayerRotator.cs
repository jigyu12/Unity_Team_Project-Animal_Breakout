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
    public void SetPlayerMove(PlayerMove playerMove)
    {
        this.playerMove = playerMove;
        this.playerMove.onRotate += Rotate;

        onRotationStart += playerMove.DisableInput;
        onRotationEnd += playerMove.EnableInput;
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
   
        moveForward.RotateForwardDirection(angle);
        onRotationEnd?.Invoke();
    }
}
