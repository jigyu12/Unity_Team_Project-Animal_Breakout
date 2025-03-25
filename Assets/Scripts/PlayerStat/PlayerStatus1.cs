using UnityEngine;

public class PlayerStatus1 : MonoBehaviour
{
    public AnimalDatabase animalDB;
    public int currentAnimalID;
    private AnimalStatus currentAnimal;

    private void Start()
    {
        SetAnimal(currentAnimalID);
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

    public void TakeDamage(float damage)
    {
        currentAnimal.HP -= damage;
        if (currentAnimal.HP <= 0)
        {
            Debug.Log($"{currentAnimal.Name}이(가) 죽었습니다!");
        }
    }
}
