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
        int nextLane = GetTurnAfterLaneIndex(moveForward, pivot, angle);
        if (nextLane != 1)
        {
            bool isPassed = IsPlayerPassedPivot(moveForward, pivot);
            if (isPassed)
            {
                moveForward.transform.position = pivot + moveForward.direction;
            }
            else
            {
                moveForward.transform.position = pivot - moveForward.direction;
            }
        }
        else
        {
            moveForward.transform.position = pivot;
        }
       
        //플레이어 로컬을 회전하는 연출
        float elapsed = 0f;
        float currentAngle = 0f;
        while (elapsed < rotateDuration)
        {
            float deltaAngle = (elapsed / rotateDuration) * angle - currentAngle;
            playerMove.transform.Rotate(new Vector3(0, deltaAngle, 0));
            currentAngle += deltaAngle;
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }


        //플레이어루트를 실제로 회전하고 연출용 회전은 리셋시킨다.
        moveForward.transform.RotateAround(pivot,Vector3.up, angle);
        playerMove.transform.localRotation = Quaternion.identity;

        //레인 위치를 보정해준다.
        playerMove.MoveLaneIndexImmediate(nextLane);

        if (moveForward != null)
        {
            moveForward.enabled = true;
        }
        isRotating = false;

       
        moveForward.RotateForwardDirection(angle);
        onRotationEnd?.Invoke();
    }

    private bool IsPlayerPassedPivot(MoveForward player, Vector3 pivot)
    {
        var playerToPivot = pivot - player.transform.position;
        bool isPlayerPassed = Vector3.Dot(playerToPivot, player.direction) < 0f;
        return isPlayerPassed;
    }

    private int GetTurnAfterLaneIndex(MoveForward player, Vector3 pivot, float angle)
    {
        //각 타일사이즈가 1,1이라는 가정하에 쓰임
        var playerToPivot = pivot - player.transform.position;
        if (Vector3.Magnitude(playerToPivot) <= 0.5f)
        {
            //플레이어와 피봇간 거리가 0.5 미만이면 1번 레인
            return 1;
        }
        else
        {
            //플레이어가 피봇을 지난 경우 0또는 2인데 회전 방향에따라 현재위치가 0인지 2인지 결정된다.
            bool isPassed = IsPlayerPassedPivot(player, pivot);
            if (isPassed)
            {
                return angle > 0f ? 0 : 2;
            }
            else
            {
                return angle > 0f ? 2 : 0;
            }
        }
    }
}
