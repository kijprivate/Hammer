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
    private float cashed1Star;
    private float cashed2Stars;
    private float cashed3Stars;

    void Awake()
    {
        currentLevelNumber = PlayerPrefsManager.GetChosenLevelNumber();
        
        int length=0;
        for (int i = 0; i < LevelsDifficultyContainer.Houses.Count; i++)
        {
            length += LevelsDifficultyContainer.Houses[i].levelsData.Count;
            if (currentLevelNumber <= length)
            {
                currentHouseNumber = i + 1;
                break;
            }
        }

        int levNumber = currentLevelNumber;
        for (int i = 1; i < currentHouseNumber; i++)
        {
            if (currentHouseNumber > 1)
            {
                levNumber -= LevelsDifficultyContainer.Houses[i].levelsData.Count;
            }
        }

        currentLevelIndex = levNumber - 1;
        currentHouseIndex = currentHouseNumber - 1;
        print(currentHouseNumber);
        print(currentLevelNumber);
        print(currentLevelIndex);
        //DontDestroyOnLoad(gameObject);
        data = LevelsDifficultyContainer.Houses[currentHouseNumber-1].levelsData[currentLevelIndex];

        starsForPreviousTries = data.gainedStars;
        numberOfNails = data.numberOfDefaultNails + data.numberOfRedNails + data.movingDefaultNails + data.movingRedNails;
        hammerHits = data.hammerHits;
        
        cashed1Star = ConstantDataContainer.PercentageValueFor1Star/100f;
        cashed2Stars = ConstantDataContainer.PercentageValueFor2Stars/100f;
        cashed3Stars = ConstantDataContainer.PercentageValueFor3Stars/100f;
        
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

    private void Start()
    {
        StartCoroutine(CountMaxPoints());
    }

    IEnumerator CountMaxPoints()
    {
        yield return new WaitForSeconds(1f);
        //maxAvailableScore = NailsSpawner.MaxScoreForNails + numberOfNails * ConstantDataContainer.ScoreBonusForPerfectHit;
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
        GameOver = true;

        //maxAvailableScore = NailsSpawner.MaxScoreForNails + numberOfNails * ConstantDataContainer.ScoreBonusForPerfectHit;
        maxAvailableScore = (int)((data.numberOfDefaultNails * ConstantDataContainer.DefaultNail +
                             data.numberOfRedNails * ConstantDataContainer.RedNail +
                             data.movingDefaultNails * ConstantDataContainer.MovingDefaultNail +
                             data.movingRedNails * ConstantDataContainer.MovingRedNail) +
                             (data.numberOfDefaultNails * ConstantDataContainer.DefaultNail +
                             data.numberOfRedNails * ConstantDataContainer.RedNail +
                             data.movingDefaultNails * ConstantDataContainer.MovingDefaultNail +
                             data.movingRedNails * ConstantDataContainer.MovingRedNail)*ConstantDataContainer.PercentageBonusForPerfectHit);

        print(maxAvailableScore);
        percentageValueOfScore = (float)Score / maxAvailableScore;

        StartCoroutine(CalculatePointsWithDelay());
    }

    private IEnumerator CalculatePointsWithDelay()
    {
        yield return new WaitForSeconds(0.3f);
       
        if (percentageValueOfScore >= cashed3Stars)
        {
            starsForCurrentTry = 3;
            PlayerPrefsManager.UnlockLevel(currentLevelNumber + 1);
            HandleCoins();
        }
        else if (percentageValueOfScore >= cashed2Stars)
        {
            starsForCurrentTry = 2;
            PlayerPrefsManager.UnlockLevel(currentLevelNumber + 1);
            HandleCoins();
        }
        else if (percentageValueOfScore >= cashed1Star)
        {
            starsForCurrentTry = 1;
            PlayerPrefsManager.UnlockLevel(currentLevelNumber + 1);
            HandleCoins();
        }
        else
        {
            starsForCurrentTry = 0;
            HandleCoins();
        }
    }

    private void HandleCoins()
    {
        if (Score > data.highScore)
        {
            PlayerPrefsManager.SetNumberOfCoins(PlayerPrefsManager.GetNumberOfCoins() + (Score - data.highScore)/ConstantDataContainer.ScoreDivider);

            data.highScore = Score;
            if(starsForCurrentTry > starsForPreviousTries)
            {
                data.gainedStars = starsForCurrentTry;
            }
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
