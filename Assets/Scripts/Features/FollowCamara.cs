using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamara : MonoBehaviour
{
    public Transform targetPivot;
    public float rotationSpeed = 5f;


    private void LateUpdate()
    {
        Vector3 targetPosition = targetPivot.position;
        Quaternion targetRotation = targetPivot.rotation;

        //transform.position = Vector3.Lerp(transform.position, targetPosition, rotationSpeed * Time.deltaTime);
        transform.position = targetPosition;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

}
