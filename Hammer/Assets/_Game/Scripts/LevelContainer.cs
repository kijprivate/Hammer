using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelContainer : MonoBehaviour
{
    private int hammerHits;
    public static int HammerHits => Instance.hammerHits;

    private int currentLevelNumber;
    public static int CurrentLevelNumber => Instance.currentLevelNumber;
    
    private int currentLevelIndex;
    public static int CurrentLevelIndex => Instance.currentLevelIndex;

    private int numberOfNails;
    public static int NumberOfNails => Instance.numberOfNails;

    private float percentageValueOfScore;
    public static float PercentageValueOfScore => Instance.percentageValueOfScore;
    
    public static int Score { get; set; }
    public static int PocketNails { get; private set; }

    public static bool GameOver { get; private set; }
    public static bool GameStarted { get; set; }
    public static bool MenuHided { get; set; }

    private int cashed1Star;
    private int cashedScoreForMoves;
    private int cashedScoreForNails;
    void Awake()
    {
        currentLevelNumber = PlayerPrefsManager.GetChosenLevelNumber();
        currentLevelIndex = currentLevelNumber - 1;
        //DontDestroyOnLoad(gameObject);
        var data = LevelsDifficultyContainer.LevelsData[currentLevelIndex];

        numberOfNails = data.numberOfDefaultNails + data.numberOfRedNails;
        hammerHits = data.hammerHits;
        cashed1Star = ConstantDataContainer.PercentageValueFor1Star;
        cashedScoreForNails = ConstantDataContainer.ScoreBonusForPerfectHit;
        
        PocketNails = 0;
        GameOver = false;
        Score = 0;
        EventManager.EventGameOver += OnGameOver;
        EventManager.EventNailPocket += OnNailPocket;
        EventManager.EventNoMoreNails += OnGameOver;
        EventManager.EventGameStarted += OnGameStarted;
        EventManager.EventMenuHided += OnMenuHided;
        EventManager.EventHammerHit += OnHammerHit;
    }
    static LevelContainer instance;
    public static LevelContainer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelContainer>();
            }
            return instance;
        }
    }
    private void OnGameOver()
    {
        GameOver = true;

       // Score = Score + HammerHits * cashedScoreForMoves + PocketNails * cashedScoreForNails;
        percentageValueOfScore = (float)Score / NailsSpawner.MaxAvailableScore;
        
        if(PercentageValueOfScore > cashed1Star)
        { PlayerPrefsManager.UnlockLevel(SceneManager.GetActiveScene().buildIndex + 2); }
    }

    private void OnNailPocket()
    {
        PocketNails++;
    }

    private void OnMenuHided()
    {
        MenuHided = true;
    }

    private void OnGameStarted()
    {
        GameStarted = true;
    }

    private void OnHammerHit()
    {
        hammerHits--;
        if (hammerHits <= 0)
        { 
            EventManager.RaiseEventGameOver();
        }
    }

    private void OnDestroy()
    {
        EventManager.EventGameOver -= OnGameOver;
        EventManager.EventNailPocket -= OnNailPocket;
        EventManager.EventNoMoreNails -= OnGameOver;
        EventManager.EventGameStarted -= OnGameStarted;
        EventManager.EventMenuHided -= OnMenuHided;
        EventManager.EventHammerHit -= OnHammerHit;
        
    }
}
