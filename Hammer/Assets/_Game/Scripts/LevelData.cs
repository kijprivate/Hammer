
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int levelNumber;
    
    [Header("Hammer")]
    [SerializeField] 
    public int hammerHits = 12;
    [SerializeField]
    public float rotationSpeed = 10f;

    [Header("Nails")] 
    [SerializeField] 
    public int numberOfDefaultNails=10;
    [SerializeField] 
    public int numberOfRedNails=2;
    [SerializeField] 
    public int movingDefaultNails=0;
    [SerializeField] 
    public int movingRedNails=0;
    

    [Header("Score")] 
    [SerializeField] 
    public int gainedStars = 0;
    [SerializeField] 
    public int highScore = 0;
}
