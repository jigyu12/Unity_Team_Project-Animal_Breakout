using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public Vector3 direction;
    public float speed;


    private void Awake()
    {
        direction.Normalize();
    }

    private void Update()
    {
        var nextPosition = transform.position + direction * Time.deltaTime * speed;
        transform.position = nextPosition;
    }
}
