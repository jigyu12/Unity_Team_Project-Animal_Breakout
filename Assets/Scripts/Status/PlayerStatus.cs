using UnityEngine;
using System.Collections;
using System;

public class PlayerStatus : MonoBehaviour
{
    //1.데이터 테이블에서 바로 읽어오는 방식
    //2.스크립터블 오브젝트 채로 프리펩화 하는 방식
    //지금은 1번입니다. 2번으로 추후 변경토록 하세요
    //기존 방식인 애니멀데이터 전체를 스크립터블 오브젝트화하는 것은 일반적이지 않습니다
    //각 AnimalData를 스크립터블 오브젝트화 하여 런타임전에 미리 장착하는 방법을 추천합니다.
    public AnimalDataTable.AnimalData animalData;

    //public AnimalDatabase animalDB;
    //public int currentAnimalID;
    //private bool isGameOver;
    //private AnimalStatus currentAnimal;
    private Animator animator;

    public Action<PlayerStatus> onDie;
    private bool isInvincible = false;
    private int defaultLayer;
    private int invincibleLayer;
    public bool IsInvincible => isInvincible;

    private void Start()
    {
        //if (animalDB == null)
        //{
        //    animalDB = FindObjectOfType<AnimalDatabase>();
        //    if (animalDB == null)
        //    {
        //        Debug.LogError("AnimalDatabase를 찾을 수 없습니다.");
        //        return;
        //    }
        //}

        //Init(currentAnimalID, animalDB);
    }
    public void Initialize(int animalID)
    {
        animalData = DataTableManager.animalDataTable.Get(animalID);

        //animalDB = database;
        //animator = GetComponentInChildren<Animator>();
        //SetAnimal(animalID);
        defaultLayer = LayerMask.NameToLayer("Player");
        invincibleLayer = LayerMask.NameToLayer("InvinciblePlayer");
    }

    //public void SetAnimal(int animalID)
    //{
    //    currentAnimal = animalDB.GetAnimalByID(animalID);
    //    currentAnimalID = animalID;

    //    if (currentAnimal != null)
    //    {
    //        Debug.Log($"선택한 캐릭터: {currentAnimal.Name} | 공격력: {currentAnimal.AttackPower} | HP: {currentAnimal.HP}");
    //    }
    //    else
    //    {
    //        Debug.LogError("해당 ID의 캐릭터를 찾을 수 없음!");
    //    }
    //}

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

    //임시로 고쳐놓은 것이라 수정이 필요합니다.
    public float GetMoveSpeed() => animalData?.StartSpeed ?? 0f;
    public float GetJumpPower() => animalData?.Jump ?? 0f;

    [ContextMenu("Damage +1")]
    public void ForceTakeDamage()
    {
        TakeDamage(1);
    }

    public void TakeDamage(int damage)
    {
        //currentAnimal.HP -= damage;
        //Debug.Log($"{currentAnimal.Name} took {damage} damage. Current HP: {currentAnimal.HP}");

        //if (currentAnimal.HP <= 0) OnDie();
        
        if (isInvincible) return;
        OnDie();
    }

    private void OnDie()
    {
        //if (isGameOver) return;
        //isGameOver = true;
        //var move = GetComponent<PlayerMove>();
        //move.DisableInput();

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        onDie?.Invoke(this);

        Debug.Log($"Player Died");
       //StartCoroutine(DieAndSwitch());
    }

    //IEnumerator DieAndSwitch()
    //{
    //    var gameManager = FindObjectOfType<GameManager>();

    //    if (gameManager != null)
    //    {
    //        gameManager.StopAllMovements();
    //    }

    //    yield return new WaitForSeconds(1.5f);

    //    var relayManager = FindObjectOfType<RelayRunManager>();

    //    if (relayManager != null)
    //    {
    //        if (gameManager != null)
    //        {
    //            gameManager.OnPlayerDied(this);
    //        }
    //        else
    //        {
    //            Debug.LogError("GameManager를 찾을 수 없습니다!");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("RelayRunManager를 찾을 수 없습니다!");
    //    }

    //    Destroy(gameObject);
    //}

}
