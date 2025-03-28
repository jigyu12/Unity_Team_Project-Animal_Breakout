using UnityEngine;
using System.Collections;
using System;

public class PlayerStatus : MonoBehaviour
{
    public AnimalDatabase animalDB;
    public int currentAnimalID;

    private AnimalStatus currentAnimal;
    private bool isGameOver;
    private bool isInvincible = false;
    private Animator animator;

    public Action<PlayerStatus> onDie;

    public void Init(int animalID, AnimalDatabase database)
    {
        animalDB = database;
        animator = GetComponentInChildren<Animator>();
        SetAnimal(animalID);
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
    }

    public float GetMoveSpeed() => currentAnimal?.MoveSpeed ?? 0f;
    public float GetJumpPower() => currentAnimal?.JumpingPower ?? 0f;

    public void TakeDamage(int damage)
    {
        if (isGameOver || isInvincible) return;

        currentAnimal.HP -= damage;
        Debug.Log($"{currentAnimal.Name} took {damage} damage. Current HP: {currentAnimal.HP}");

        if (currentAnimal.HP <= 0) OnDie();
    }

    [ContextMenu("Damage +1")]
    public void ForceDie() => TakeDamage(1);

    private void OnDie()
    {
        if (isGameOver) return;

        isGameOver = true;

        var move = GetComponent<PlayerMove>();
        if (move != null) move.enabled = false;

        animator?.SetTrigger("Die");
        StartCoroutine(DieAndSwitch());

        onDie?.Invoke(this);
    }
    IEnumerator DieAndSwitch()
    {
        yield return new WaitForSeconds(1.5f);

        var relayManager = RelayRunManager.Instance;

        if (relayManager != null)
        {
            if (relayManager.HasNextRunner())
            {
                RelayContinueUI.Instance.Show();
            }
            else
            {
                GameManager.Instance.GameOver();
            }
        }

        Destroy(gameObject);
    }


}
