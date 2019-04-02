using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelDataContainer", fileName = "LevelDataConfig",order = 2)]
public class LevelsDifficultyContainer:ScriptableObject
{
    [SerializeField] private List<LevelData> levelsData = new List<LevelData>();
    public static List<LevelData> LevelsData => Instance.levelsData;
    
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
