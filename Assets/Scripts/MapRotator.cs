using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRotator : MonoBehaviour
{
    private bool isRotating = false;
    private Transform player;
    public float currentYRotation;

    private void Start()
    {
        currentYRotation = 0;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().onRotate += Rotate;
    }

    public void Rotate(Vector3 pivot, float angle)
    {
        transform.RotateAround(pivot, Vector3.up, angle);
        currentYRotation += angle;
        //if (!isRotating)
        //    StartCoroutine(RotateRoutine(pivot, angle));
    }

    private void OnDrawGizmos()
    {
        
    }

    private IEnumerator RotateRoutine(Vector3 pivot, float angle)
    {
        isRotating = true;

        Scroll scroll = GetComponent<Scroll>();
        if (scroll != null) scroll.enabled = false;

        float duration =5f;
        float elapsed = 0f;
        currentYRotation += angle;

        Quaternion endRot = transform.rotation * Quaternion.Euler(0, angle, 0);

        while (elapsed < duration)
        {
            transform.RotateAround(pivot, Vector3.up, Mathf.Lerp(0, angle, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot;

        //// 기준점을 중심으로 한 회전 축 기준으로 초기 위치 계산
        //Quaternion startRot = transform.rotation;
        //Quaternion endRot = transform.rotation * Quaternion.Euler(0, angle, 0);

        //Vector3 startPos = transform.position;
        //Vector3 dir = startPos - pivot; // 회전 중심점과의 거리 벡터
        //Vector3 rotatedDir = endRot * dir; // 최종 회전 방향 계산
        //Vector3 endPos = pivot + rotatedDir; // 최종 위치 설정

        //while (elapsed < duration)
        //{
        //    float t = elapsed / duration;
        //    transform.rotation = Quaternion.Slerp(startRot, endRot, t); // 부드러운 회전
        //    transform.position = Vector3.Lerp(startPos, endPos, t); // 위치 보간
        //    elapsed += Time.deltaTime;
        //    yield return null;
        //}

        //// 최종 상태 확정
        //transform.rotation = endRot;
        //transform.position = endPos;

        if (scroll != null) scroll.enabled = true;

        isRotating = false;
    }

}
