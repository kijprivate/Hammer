using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelDataContainer", fileName = "LevelDataConfig",order = 2)]
public class LevelsDifficultyContainer:ScriptableObject
{
    [SerializeField] private List<House> houses = new List<House>();
    public static List<House> Houses => Instance.houses;
    
    static LevelsDifficultyContainer instance;
    public static LevelsDifficultyContainer Instance
    {
        get
        {
            if (instance == null)
                instance = Resources.Load<LevelsDifficultyContainer>("LevelDataConfig");
            return instance;
        }
    }
}
