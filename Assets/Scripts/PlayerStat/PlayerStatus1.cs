using UnityEngine;
using System.Collections;
public class PlayerStatus1 : MonoBehaviour
{
    public AnimalDatabase animalDB;
    public int currentAnimalID;

    private AnimalStatus currentAnimal;
    private bool isGameOver;
    private Animator animator;
    private bool isInvincible = false;
    private float invincibleDuration = 1f;
    private Renderer[] renderers;
    public void Init(int animalID, AnimalDatabase database)
    {
        animalDB = database;
        animator = GetComponentInChildren<Animator>();
        SetAnimal(animalID);
    }

    public void SetAnimal(int animalID)
    {
        currentAnimal = animalDB.GetAnimalByID(animalID);

        if (currentAnimal != null)
        {
            Debug.Log($"선택한 캐릭터: {currentAnimal.Name} | 공격력: {currentAnimal.AttackPower} | HP: {currentAnimal.HP}");
            currentAnimalID = animalID;
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
    [ContextMenu("Damage +1")]
    public void ForceDie()
    {
        TakeDamage(1);
    }

    private void OnDie()
    {
        if (isGameOver) return;
        isGameOver = true;

        var move = GetComponent<PlayerMove>();
        if (move != null) move.enabled = false;

        animator?.SetTrigger("Die");
        StartCoroutine(DieAndSwitch());
    }

    IEnumerator DieAndSwitch()
    {
        yield return new WaitForSeconds(1.5f);

        RelayRunManager relayManager = FindObjectOfType<RelayRunManager>();
        if (relayManager != null)
        {
            relayManager.LoadNextRunner();
        }
        Destroy(gameObject);
    }

}
