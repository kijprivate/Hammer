using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelContainer : MonoBehaviour
{
    private int hammerHits;
    public static int HammerHits => Instance.hammerHits;

    private int currentLevelNumber; //TODO Load from playerprefs
    public static int CurrentLevelNumber => Instance.currentLevelNumber;

    private int numberOfNails;
    public static int NumberOfNails => Instance.numberOfNails;

    public static int Score { get; set; }
    public static int MaxAvailableScore { get; private set; }
    public static int PocketNails { get; private set; }
    public static float PercentageValueOfScore { get; private set; }
    public static bool GameOver { get; private set; }
    public static bool GameStarted { get; set; }
    public static bool MenuHided { get; set; }

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        var data = LevelsDifficultyContainer.LevelsData[SceneManager.GetActiveScene().buildIndex];
        currentLevelNumber = data.levelNumber;
        hammerHits = data.hammerHits;
        numberOfNails = data.numberOfDefaultNails + data.numberOfRedNails;
        
        PocketNails = 0;
        GameOver = false;
        Score = 0;
        MaxAvailableScore = NailsSpawner.MAX_SCORE_FOR_NAILS + (HammerHits - NailsSpawner.MIN_HAMMER_HITS)*50 +numberOfNails*100;
        print(MaxAvailableScore);
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
        Score = Score + HammerHits * 50 + PocketNails * 100;
        PercentageValueOfScore = (float)Score / MaxAvailableScore;
        if(PercentageValueOfScore > 0.6f)
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
