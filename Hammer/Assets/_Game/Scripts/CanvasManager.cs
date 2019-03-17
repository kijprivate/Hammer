using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] GameObject GameplayUI;
    [SerializeField] GameObject MenuUI;
    [SerializeField] GameObject SummaryPanel;
    [SerializeField] GameObject star1,star2,star3;
    [SerializeField] GameObject PlayAgain, NextLevel;
    [SerializeField] Text ScoreGameplay;
    [SerializeField] Text ScoreSummary;
    [SerializeField] Text HammersLeft;
    [SerializeField] Text NailPocket;
    [SerializeField] Text LevelName;

    private void Start()
    {
        EventManager.EventGameOver += OnGameOver;
        EventManager.EventNoMoreNails += OnGameOver;
        EventManager.EventHammerHit += OnHammerHit;
        EventManager.EventNailPocket += OnNailPocket;
        EventManager.EventMenuHided += OnMenuHided;

        HammersLeft.text = LevelContainer.HammerHits.ToString();
        NailPocket.text = LevelContainer.PocketNails.ToString();
        LevelName.text = SceneManager.GetActiveScene().name;

        if(!LevelContainer.MenuHided)
        {
            MenuUI.SetActive(true);
            GameplayUI.SetActive(false);
        }
        else
        {
            OnMenuHided();
        }
    }

    private void OnGameOver()
    {
        SummaryPanel.SetActive(true); //TODO add delay
        ScoreSummary.text = LevelContainer.Score.ToString();

        if (LevelContainer.PercentageValueOfScore > 0.85f)
        {
            star3.SetActive(true);
            PlayAgain.SetActive(true);
            NextLevel.SetActive(true);
        }
        else if (LevelContainer.PercentageValueOfScore > 0.75f)
        {
            star2.SetActive(true);
            PlayAgain.SetActive(true);
            NextLevel.SetActive(true);
        }
        else if (LevelContainer.PercentageValueOfScore > 0.6f)
        {
            star1.SetActive(true);
            PlayAgain.SetActive(true);
            NextLevel.SetActive(true);
        }
        else
        {
            PlayAgain.SetActive(true);
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
