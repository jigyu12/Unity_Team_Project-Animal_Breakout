using UnityEngine;

[CreateAssetMenu(fileName = "AnimalStatData", menuName = "Game/AnimalStatData")]
public class AnimalStatData : ScriptableObject
{
    public int AnimalID;
    public string StringID;
    public int Grade;
    public int AttackPower;
    public int StartSpeed;
    public int MaxSpeed;
    public int Jump;
}
