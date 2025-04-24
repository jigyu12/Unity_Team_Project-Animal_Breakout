using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "AnimalStatData", menuName = "Game/AnimalStatData")]
public class AnimalStatData : ScriptableObject
{
    public int AnimalID;
    public string StringID;
    public int Grade;
    public int AttackPower;
    public float StartSpeed;
    public float MaxSpeed;
    public float Jump;

    public void SetData(AnimalDataTable.AnimalData rawData)
    {
        this.AnimalID = rawData.AnimalID;
        this.StringID = rawData.StringID;
        this.Grade = rawData.Grade;
        this.AttackPower = rawData.AttackPower;
        this.StartSpeed = rawData.StartSpeed;
        this.MaxSpeed = rawData.MaxSpeed;
        this.Jump = rawData.Jump;
    }
}
