using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelContainer : MonoBehaviour
{
    private int hammerHits;
    public static int HammerHits => Instance.hammerHits;

    private int currentGlobalLevelNumber;
    public static int CurrentGlobalLevelNumber => Instance.currentGlobalLevelNumber;
    
    private int currentLevelIndex;
    public static int CurrentLevelIndex => Instance.currentLevelIndex;
    
    private int currentHouseNumber;
    public static int CurrentHouseNumber => Instance.currentHouseNumber;

    private int currentHouseIndex;
    public static int CurrentHouseIndex => Instance.currentHouseIndex;

    private int numberOfNails;
    public static int NumberOfNails => Instance.numberOfNails;

    private float percentageValueOfScore;
    public static float PercentageValueOfScore => Instance.percentageValueOfScore;
    
    private int starsForCurrentTry;

    public static int StarsForCurrentTry => Instance.starsForCurrentTry;

    private int maxAvailableScore;

    public static int MaxAvailableScore => Instance.maxAvailableScore;

    public static int Score { get; set; }
    public static int PocketNails { get; private set; }
    public static bool GameOver { get; private set; }
    public static bool GameStarted { get; set; }
    public static bool MenuHided { get; set; }

    private LevelData data;

    private int starsForPreviousTries;
    private int currentHighScore;
    private float cashed1Star;
    private float cashed2Stars;
    private float cashed3Stars;

    void Awake()
    {
        CountCurrentLevelIndexes();

        data = LevelsDifficultyContainer.Houses[currentHouseIndex].levelsData[currentLevelIndex];

        starsForPreviousTries = PlayerPrefsManager.GetGainedStars(currentHouseIndex, currentLevelIndex);
        currentHighScore = PlayerPrefsManager.GetHighScore(currentHouseIndex, currentLevelIndex);
        numberOfNails = data.numberOfDefaultNails + data.numberOfRedNails + data.movingDefaultNails + data.movingRedNails;
        hammerHits = data.hammerHits;

        cashed1Star = ConstantDataContainer.PercentageValueFor1Star / 100f;
        cashed2Stars = ConstantDataContainer.PercentageValueFor2Stars / 100f;
        cashed3Stars = ConstantDataContainer.PercentageValueFor3Stars / 100f;

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

    private void CountCurrentLevelIndexes()
    {
        currentGlobalLevelNumber = PlayerPrefsManager.GetChosenLevelNumber();

        int length = 0;
        for (int i = 0; i < LevelsDifficultyContainer.Houses.Count; i++)
        {
            length += LevelsDifficultyContainer.Houses[i].levelsData.Count;
            if (currentGlobalLevelNumber <= length)
            {
                currentHouseNumber = i + 1;
                break;
            }
        }

        int levNumber = currentGlobalLevelNumber;
        for (int i = 1; i < currentHouseNumber; i++)
        {
            if (currentHouseNumber > 1)
            {
                levNumber -= LevelsDifficultyContainer.Houses[i].levelsData.Count;
            }
        }

        currentLevelIndex = levNumber - 1;
        currentHouseIndex = currentHouseNumber - 1;
    }

    private void Start()
    {
        StartCoroutine(CountMaxPoints());
    }

    IEnumerator CountMaxPoints()
    {
        yield return new WaitForSeconds(0.2f);

        maxAvailableScore = (int)((data.numberOfDefaultNails * ConstantDataContainer.DefaultNail +
                             data.numberOfRedNails * ConstantDataContainer.RedNail +
                             data.movingDefaultNails * ConstantDataContainer.MovingDefaultNail +
                             data.movingRedNails * ConstantDataContainer.MovingRedNail) +
                             (data.numberOfDefaultNails * ConstantDataContainer.DefaultNail +
                             data.numberOfRedNails * ConstantDataContainer.RedNail +
                             data.movingDefaultNails * ConstantDataContainer.MovingDefaultNail +
                             data.movingRedNails * ConstantDataContainer.MovingRedNail) * ConstantDataContainer.PercentageBonusForPerfectHit);
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
        percentageValueOfScore = (float)Score / maxAvailableScore;
        GameOver = true;

        print(maxAvailableScore);

        StartCoroutine(CalculatePointsWithDelay());
    }

    private IEnumerator CalculatePointsWithDelay()
    {
        yield return new WaitForSeconds(0.3f);
       
        if (percentageValueOfScore >= cashed3Stars)
        {
            starsForCurrentTry = 3;
            PlayerPrefsManager.UnlockLevel(currentGlobalLevelNumber + 1);
            HandleCoins();
            HandleProgress();
        }
        else if (percentageValueOfScore >= cashed2Stars)
        {
            starsForCurrentTry = 2;
            PlayerPrefsManager.UnlockLevel(currentGlobalLevelNumber + 1);
            HandleCoins();
            HandleProgress();
        }
        else if (percentageValueOfScore >= cashed1Star)
        {
            starsForCurrentTry = 1;
            PlayerPrefsManager.UnlockLevel(currentGlobalLevelNumber + 1);
            HandleCoins();
            HandleProgress();
        }
        else
        {
            starsForCurrentTry = 0;
            HandleCoins();
            HandleProgress();
        }
    }

    private void HandleProgress()
    {
        if (Score > currentHighScore)
        {
            PlayerPrefsManager.SetHighScore(currentHouseIndex, currentLevelIndex, Score);
        }
        if (starsForCurrentTry > starsForPreviousTries)
        {
            PlayerPrefsManager.SetGainedStars(currentHouseIndex, currentLevelIndex, starsForCurrentTry);
        }
        if (LevelsDifficultyContainer.Houses[currentHouseIndex].levelsData.Count < currentLevelIndex + 1 &&
            LevelsDifficultyContainer.Houses.Count > currentHouseNumber && starsForCurrentTry > 0)
        {
            PlayerPrefsManager.UnlockHouse(currentHouseNumber + 1);
        }
    }
    private void HandleCoins()
    {
        if (Score > currentHighScore && starsForCurrentTry > 0)
        {
            PlayerPrefsManager.SetNumberOfCoins(PlayerPrefsManager.GetNumberOfCoins() + (Score - currentHighScore)/ConstantDataContainer.ScoreDivider);
        }
        else 
        {
            Debug.Log("No points added");
        }
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
            StartCoroutine(FinishLevelWithDelay());
        }
    }

    IEnumerator FinishLevelWithDelay()
    {
        yield return new WaitForSeconds(0.2f);
        EventManager.RaiseEventGameOver();
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
