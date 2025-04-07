using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamara : MonoBehaviour
{
    public GameObject target;
    public float rayLength;

    public LayerMask ignoreLayers;


    private void LateUpdate()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, target.transform.position, rayLength, ignoreLayers.value);
        foreach(var ignoreObj in hits)
        {
            //ignoreObj.collider.gameObject.SetActive(false);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, target.transform.position);
    }
}
