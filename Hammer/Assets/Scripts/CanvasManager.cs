using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] GameObject GameplayUI;
    [SerializeField] GameObject MenuUI;
    [SerializeField] GameObject SummaryPanel;
    [SerializeField] GameObject star1,star2,star3;
    [SerializeField] Text ScoreGameplay;
    [SerializeField] Text ScoreSummary;
    [SerializeField] Text HammersLeft;
    [SerializeField] Text NailPocket;

    private void Start()
    {
        EventManager.EventGameOver += OnGameOver;
        EventManager.EventNoMoreNails += OnGameOver;
        EventManager.EventHammerHit += OnHammerHit;
        EventManager.EventNailPocket += OnNailPocket;
        EventManager.EventMenuHided += OnMenuHided;

        HammersLeft.text = LevelContainer.HammerHits.ToString();
        NailPocket.text = LevelContainer.PocketNails.ToString();

        if(!LevelContainer.MenuHided)
        {
            MenuUI.SetActive(true);
            GameplayUI.SetActive(false);
        }
    }

    private void OnGameOver()
    {
        SummaryPanel.SetActive(true); //TODO add delay
        ScoreSummary.text = LevelContainer.Score.ToString();

        if ((float)LevelContainer.Score / LevelContainer.MaxAvailableScore > 0.85f)
        { star3.SetActive(true); }
        else if ((float)LevelContainer.Score / LevelContainer.MaxAvailableScore > 0.75f)
        { star2.SetActive(true); }
        else if ((float)LevelContainer.Score / LevelContainer.MaxAvailableScore > 0.6f)
        { star1.SetActive(true); }
        else
        {
            //levelfailed
        }
    }

    private void OnHammerHit()
    {
        HammersLeft.text = LevelContainer.HammerHits.ToString();
    }

    private void OnNailPocket()
    {
        NailPocket.text = LevelContainer.PocketNails.ToString();
        ScoreGameplay.text = LevelContainer.Score.ToString();
    }

    private void OnMenuHided()
    {
        MenuUI.SetActive(false);
        GameplayUI.SetActive(true);
    }

    private void OnDestroy()
    {
        EventManager.EventHammerHit -= OnHammerHit;
        EventManager.EventNailPocket -= OnNailPocket;
        EventManager.EventGameOver -= OnGameOver;
        EventManager.EventNoMoreNails -= OnGameOver;
        EventManager.EventMenuHided -= OnMenuHided;
    }
}
