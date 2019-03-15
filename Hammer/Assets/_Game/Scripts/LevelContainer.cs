using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelContainer : MonoBehaviour
{
    [SerializeField] int hammerHits = 12;
    //[SerializeField] int numberOfNails = 10;

    public static int Score { get; set; }
    public static int MaxAvailableScore { get; private set; }
    public static int HammerHits{ get; set; }
    public static int PocketNails { get; private set; }
    public static bool GameOver { get; private set; }
    public static bool GameStarted { get; set; }
    public static bool MenuHided { get; set; }
    //public static int NumberOfNails { get; set; }

    void Awake()
    {
        HammerHits = hammerHits;
        PocketNails = 0;
        GameOver = false;
        Score = 0;
        MaxAvailableScore = NailsSpawner.MAX_SCORE_FOR_NAILS + (HammerHits - NailsSpawner.MIN_HAMMER_HITS)*50 +NailsSpawner.NUMBER_OF_NAILS*100;
        print(MaxAvailableScore);
        EventManager.EventGameOver += OnGameOver;
        EventManager.EventNailPocket += OnNailPocket;
        EventManager.EventNoMoreNails += OnGameOver;
        EventManager.EventGameStarted += OnGameStarted;
        EventManager.EventMenuHided += OnMenuHided;
        //NumberOfNails = numberOfNails;
    }

    private void OnGameOver()
    {
        GameOver = true;
        Score = Score + HammerHits * 50 + PocketNails * 100;
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

    private void OnDestroy()
    {
        EventManager.EventGameOver -= OnGameOver;
        EventManager.EventNailPocket -= OnNailPocket;
        EventManager.EventNoMoreNails -= OnGameOver;
        EventManager.EventGameStarted -= OnGameStarted;
        EventManager.EventMenuHided -= OnMenuHided;

       // GameStarted = false;
       // MenuHided = false;
    }
}
