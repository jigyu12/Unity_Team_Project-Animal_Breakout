using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOverText;

    private bool isGameOver;

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("RunMinjae2");
        }
    }

    public void OnDie()
    {
        Time.timeScale = 0;

        Debug.Log("Game Over");

        gameOverText.gameObject.SetActive(true);

        isGameOver = true;
        GetComponent<Animator>()?.SetTrigger("Die");
    }
}