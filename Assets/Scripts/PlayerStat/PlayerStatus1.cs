using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus1 : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOverText;

    public AnimalDatabase animalDB;
    public int currentAnimalID;
    private AnimalStatus currentAnimal;
    private bool isGameOver;
    private Animator animator;

    private void Start()
    {
        Time.timeScale = 1;
        SetAnimal(currentAnimalID);
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void SetAnimal(int animalID)
    {
        currentAnimal = animalDB.GetAnimalByID(animalID);

        if (currentAnimal != null)
        {
            Debug.Log($"선택한 캐릭터: {currentAnimal.Name} | 공격력: {currentAnimal.AttackPower} | HP: {currentAnimal.HP}");
        }
        else
        {
            Debug.LogError("해당 ID의 캐릭터를 찾을 수 없음!");
        }
    }

    public float GetMoveSpeed()
    {
        return currentAnimal?.MoveSpeed ?? 0f;
    }

    public float GetJumpPower()
    {
        return currentAnimal?.JumpingPower ?? 0f;
    }

    public void TakeDamage(int damage)
    {
        if (isGameOver) return;

        currentAnimal.HP -= damage;
        Debug.Log($"{currentAnimal.Name} took {damage} damage. Current HP: {currentAnimal.HP}");

        if (currentAnimal.HP <= 0)
        {
            OnDie();
        }
    }

    private void OnDie()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0;
        animator?.SetTrigger("Die");
        GameManager.Instance.GameOver();
    }
}
