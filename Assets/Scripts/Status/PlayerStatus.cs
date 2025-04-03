using UnityEngine;
using System.Collections;
using System;

public class PlayerStatus : MonoBehaviour
{
    public AnimalDatabase animalDB;
    public int currentAnimalID;
    private AnimalStatus currentAnimal;
    private bool isGameOver;
    private Animator animator;

    public Action<PlayerStatus> onDie;
    private bool isInvincible = false;
    private int defaultLayer;
    private int invincibleLayer;
    public bool IsInvincible => isInvincible;

    private void Start()
    {
        if (animalDB == null)
        {
            animalDB = FindObjectOfType<AnimalDatabase>();
            if (animalDB == null)
            {
                Debug.LogError("AnimalDatabase를 찾을 수 없습니다.");
                return;
            }
        }

        Init(currentAnimalID, animalDB);
    }


    public void Init(int animalID, AnimalDatabase database)
    {
        animalDB = database;
        animator = GetComponentInChildren<Animator>();
        SetAnimal(animalID);
        defaultLayer = LayerMask.NameToLayer("Player");
        invincibleLayer = LayerMask.NameToLayer("InvinciblePlayer");
    }

    public void SetAnimal(int animalID)
    {
        currentAnimal = animalDB.GetAnimalByID(animalID);
        currentAnimalID = animalID;

        if (currentAnimal != null)
        {
            Debug.Log($"선택한 캐릭터: {currentAnimal.Name} | 공격력: {currentAnimal.AttackPower} | HP: {currentAnimal.HP}");
        }
        else
        {
            Debug.LogError("해당 ID의 캐릭터를 찾을 수 없음!");
        }
    }

    public void SetInvincible(bool value)
    {
        isInvincible = value;
        gameObject.layer = isInvincible ? invincibleLayer : defaultLayer;
    }

    [ContextMenu("Toggle Invincible")]
    public void ToggleInvincible()
    {
        SetInvincible(!isInvincible);
    }

    public float GetMoveSpeed() => currentAnimal?.MoveSpeed ?? 0f;
    public float GetJumpPower() => currentAnimal?.JumpingPower ?? 0f;
    [ContextMenu("Damage +1")]
    public void ForceTakeDamage()
    {
        TakeDamage(1);
    }

    public void TakeDamage(int damage)
    {
        if (isGameOver || isInvincible) return;

        currentAnimal.HP -= damage;
        Debug.Log($"{currentAnimal.Name} took {damage} damage. Current HP: {currentAnimal.HP}");

        if (currentAnimal.HP <= 0) OnDie();
    }

    private void OnDie()
    {
        if (isGameOver) return;
        isGameOver = true;
        var move = GetComponent<PlayerMove>();
        move.DisableInput();

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        Debug.Log($"Player Died: {currentAnimal.Name}");
        StartCoroutine(DieAndSwitch());

    }

    IEnumerator DieAndSwitch()
    {
        var gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
        {
            gameManager.StopAllMovements();
        }

        yield return new WaitForSeconds(1.5f);

        var relayManager = FindObjectOfType<RelayRunManager>();

        if (relayManager != null)
        {
            if (gameManager != null)
            {
                gameManager.OnPlayerDied(this);
            }
            else
            {
                Debug.LogError("GameManager를 찾을 수 없습니다!");
            }
        }
        else
        {
            Debug.LogError("RelayRunManager를 찾을 수 없습니다!");
        }

        Destroy(gameObject);
    }



}
