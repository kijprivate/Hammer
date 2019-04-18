using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class House
{
    public int houseNumber;
    
    [SerializeField] public List<LevelData> levelsData = new List<LevelData>();
}
