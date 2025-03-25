using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalDatabase", menuName = "Game Data/Animal Database")]
public class AnimalDatabase : ScriptableObject
{
    public List<AnimalStatus> Animals = new List<AnimalStatus>();

    public AnimalStatus GetAnimalByID(int id)
    {
        return Animals.Find(animal => animal.AnimalID == id);
    }
}
