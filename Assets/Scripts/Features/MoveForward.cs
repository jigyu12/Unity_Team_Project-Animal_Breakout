using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public Vector3 direction;
    public float speed;

    private Lane lane;

    private void Awake()
    {
        direction.Normalize();
        lane = GetComponent<Lane>();
    }

    private void Start()
    {
        // enabled = false;
        // var gameManager = GameObject.FindObjectOfType<GameManager>();
        // gameManager.onPlayerSpawned += (playerStatus) =>
        // {
        //     if (playerStatus.gameObject == gameObject)
        //     {
        //         enabled = true;
        //     }
        // };
        // gameManager.onPlayerDied += (playerStatus) =>
        // {
        //     if (playerStatus.gameObject == gameObject)
        //     {
        //         enabled = false;
        //     }
        // };
    }

    

    public void RotateForwardDirection(float angle)
    {
        direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
    }

    private void Update()
    {
        var nextPosition = transform.position + direction * Time.deltaTime * speed;
        transform.position = nextPosition;
    }

}
