using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public int wayIndex;

    public MapWay way;

    private void Update()
    {
        var nextPosition = transform.position;
        if (Input.GetKey(KeyCode.A))
        {
            nextPosition.x += -speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            nextPosition.x += +speed * Time.deltaTime;
        }

        nextPosition.x = Mathf.Clamp(nextPosition.x, way.minX, way.maxX);
        transform.position = nextPosition;

        wayIndex = way.PositionToWayIndex(transform.position);
    }

}
