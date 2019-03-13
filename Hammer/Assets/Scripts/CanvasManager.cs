using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject SummaryPanel;
    [SerializeField] Text HammersLeft;
    [SerializeField] Text NailPocket;

    private void Awake()
    {
        EventManager.EventGameOver += OnGameOver;
        EventManager.EventNoMoreNails += OnGameOver;
    }

    private void Start()
    {
        EventManager.EventHammerHit += OnHammerHit;
        EventManager.EventNailPocket += OnNailPocket;

        HammersLeft.text = LevelContainer.HammerHits.ToString();
        NailPocket.text = LevelContainer.PocketNails.ToString();
    }

    private void OnGameOver()
    {
        SummaryPanel.SetActive(true); //TODO add delay
    }

    private void OnHammerHit()
    {
        HammersLeft.text = LevelContainer.HammerHits.ToString();
    }

    private void OnNailPocket()
    {
        NailPocket.text = LevelContainer.PocketNails.ToString();
    }

    private void OnDestroy()
    {
        EventManager.EventHammerHit -= OnHammerHit;
        EventManager.EventNailPocket -= OnNailPocket;
        EventManager.EventGameOver -= OnGameOver;
        EventManager.EventNoMoreNails -= OnGameOver;
    }
}
