using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelContainer : MonoBehaviour
{
    [SerializeField] int hammerHits = 12;
    //[SerializeField] int numberOfNails = 10;

    public static int HammerHits{ get; set; }
    public static int PocketNails { get; private set; }
    public static bool GameOver { get; private set; }
    //public static int NumberOfNails { get; set; }

    void Awake()
    {
        HammerHits = hammerHits;
        PocketNails = 0;
        GameOver = false;
        EventManager.EventGameOver += OnGameOver;
        EventManager.EventNailPocket += OnNailPocket;
        EventManager.EventNoMoreNails += OnGameOver;
        //NumberOfNails = numberOfNails;
    }

    private void OnGameOver()
    {
        GameOver = true;
    }

    private void OnNailPocket()
    {
        PocketNails++;
    }

    private void OnDestroy()
    {
        EventManager.EventGameOver -= OnGameOver;
        EventManager.EventNailPocket -= OnNailPocket;
        EventManager.EventNoMoreNails -= OnGameOver;
    }
}
