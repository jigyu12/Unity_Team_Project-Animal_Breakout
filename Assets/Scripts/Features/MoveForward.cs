using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public Vector3 direction;
    public float speed;

    private void Awake()
    {
        direction.Normalize();
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

    private void Update()
    {
        var nextPosition = transform.position + direction * Time.deltaTime * speed;
        transform.position = nextPosition;
    }

}
