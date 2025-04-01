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
        enabled = false;
        // var relayRunManager = GameObject.FindObjectOfType<RelayRunManager>();

        // relayRunManager.onLoadPlayer += (playerStatus) => enabled = true;
        // relayRunManager.onDiePlayer += (playerStatus) => enabled = false;
        var gameManager = GameObject.FindObjectOfType<GameManager>();
        gameManager.onPlayerSpawned += (playerStatus) => enabled = true;
        gameManager.onPlayerDied += (playerStatus) => enabled = false;
    }

    private void Update()
    {
        var nextPosition = transform.position + direction * Time.deltaTime * speed;
        transform.position = nextPosition;
    }

}
