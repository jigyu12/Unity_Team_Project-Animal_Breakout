using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRotator : MonoBehaviour
{
    private bool isRotating = false;
    public float rotateDuration = 5f;

    [ReadOnly]
    public float currentYRotation;

    private void Start()
    {
        currentYRotation = 0;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().onRotate += Rotate;
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

        Scroll scroll = GetComponent<Scroll>();
        if (scroll != null) scroll.enabled = false;
        float elapsed = 0f;
        currentYRotation += angle;

        float currentAngle = 0f; 
        while (elapsed < rotateDuration)
        {
            float deltaAngle = (angle / rotateDuration) * Time.deltaTime; 
            transform.RotateAround(pivot, Vector3.up, deltaAngle);
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
