using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AnimalStatus
{
    public int AnimalID;
    public string Name;
    public string StringID;
    public int Grade;
    public float AttackPower;
    public float HP;
    public float MoveSpeed;
    public float JumpingPower;
    public GameObject Prefab;

    public AnimalStatus(int id, string name, string stringID, int grade, float attack, float hp, float speed, float jump, GameObject prefab)
    {
        AnimalID = id;
        Name = name;
        StringID = stringID;
        Grade = grade;
        AttackPower = attack;
        HP = hp;
        MoveSpeed = speed;
        JumpingPower = jump;
        Prefab = prefab;
    }
}
