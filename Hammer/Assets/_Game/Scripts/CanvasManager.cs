using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] GameObject GameplayUI;
    [SerializeField] GameObject MenuUI;
    [SerializeField] GameObject SummaryPanel;
    [SerializeField] GameObject star1,star2,star3;
    [SerializeField] GameObject PlayAgain, NextLevel;
    [SerializeField] GameObject Splash; // Image that displays splashes with hit rating 
    [SerializeField] Text ScoreGameplay;
    [SerializeField] Text ScoreSummary;
    [SerializeField] Text HammersLeft;
    [SerializeField] Text NailPocket;
    [SerializeField] Text LevelName;
    [SerializeField] Text Coins;
    [SerializeField] Sprite AwesomeSprite;  // sprite with Awesome! caption
    [SerializeField] Sprite PerfectSprite;  // sprite with Perfect! caption
    [SerializeField] Sprite ToohardSprite;  // sprite with Too hard! caption
    [SerializeField] Sprite HitagainSprite; // sprite with Hit again! caption

    private float cashed1Star;
    private float cashed2Stars;
    private float cashed3Stars;
    private RectTransform SplashRect;   // needed for changing scale when displaying Splash
    private Image SplashImage;
    private int numberofnails;// needed for changing Sprite depending on hit rating
    private void Start()
    {
        EventManager.EventGameOver += OnGameOver;
        EventManager.EventNoMoreNails += OnGameOver;
        EventManager.EventHammerHit += OnHammerHit;
        EventManager.EventNailPocket += OnNailPocket;
        EventManager.EventMenuHided += OnMenuHided;
        EventManager.EventShowSplash += OnShowSplash;
        EventManager.EventNailFinished += OnNailFinished;

        numberofnails = LevelContainer.NumberOfNails;

        HammersLeft.text = LevelContainer.HammerHits.ToString();
        NailPocket.text = numberofnails.ToString();
        LevelName.text = "Level"+LevelContainer.CurrentLevelNumber;
        Coins.text = "Coins: " + PlayerPrefsManager.GetNumberOfCoins();

        cashed1Star = ConstantDataContainer.PercentageValueFor1Star/100f;
        cashed2Stars = ConstantDataContainer.PercentageValueFor2Stars/100f;
        cashed3Stars = ConstantDataContainer.PercentageValueFor3Stars/100f;
        SplashRect = Splash.GetComponent<RectTransform>();
        SplashImage = Splash.GetComponent<Image>();

        StartCoroutine(DisplayMinPoints());
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
        StartCoroutine(DisplayPoints());
        Coins.text = "Coins: " + PlayerPrefsManager.GetNumberOfCoins();
        GameplayUI.SetActive(false);
        LevelName.gameObject.SetActive(false);
        Coins.gameObject.SetActive(false);

        if (LevelContainer.PercentageValueOfScore > cashed3Stars)
        {
            star3.SetActive(true);
            PlayAgain.SetActive(true);
            NextLevel.SetActive(true);
        }
        else if (LevelContainer.PercentageValueOfScore > cashed2Stars)
        {
            star2.SetActive(true);
            PlayAgain.SetActive(true);
            NextLevel.SetActive(true);
        }
        else if (LevelContainer.PercentageValueOfScore > cashed1Star)
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

    private IEnumerator DisplayPoints()
    {
        yield return new WaitForSeconds(0.1f);
        SummaryPanel.SetActive(true); 
        ScoreSummary.text = LevelContainer.Score.ToString();
    }
    
    private IEnumerator DisplayMinPoints()
    {
        yield return new WaitForSeconds(1.1f);
        ScoreGameplay.text = LevelContainer.Score.ToString() +"/"+LevelContainer.MaxAvailableScore*(ConstantDataContainer.PercentageValueFor1Star/100f);
    }
    
    private void OnHammerHit()
    {
        HammersLeft.text = LevelContainer.HammerHits.ToString();
    }

    private void OnNailPocket()
    {

        ScoreGameplay.text = LevelContainer.Score.ToString() +"/"+LevelContainer.MaxAvailableScore*(ConstantDataContainer.PercentageValueFor1Star/100f);
    }

    private void OnNailFinished()
    {
        numberofnails--;
        NailPocket.text = numberofnails.ToString();
    }
    

    private void OnMenuHided()
    {
        MenuUI.SetActive(false);
        GameplayUI.SetActive(true);
    }

    private void OnShowSplash(int splashId)
    {
        StartCoroutine(DisplaySplash(splashId));

    }

    private IEnumerator DisplaySplash(int splashId) // displays splash with hit rating
    {
        switch (splashId)   
        {
            case -1:    // not enough strength
                SplashImage.sprite = HitagainSprite;
                break;
            case 0: // perfect hit
                SplashImage.sprite = AwesomeSprite;
                break;
            case 1: // too much strength
                SplashImage.sprite = ToohardSprite;
                break;
        }
        Splash.SetActive(true);
        SplashRect.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.5f).SetEase(Ease.OutElastic);   // scales splash for display
        yield return new WaitForSeconds(1.0f);
        Splash.SetActive(false);    
        SplashRect.DOScale(new Vector3(0.01f, 0.01f, 1.0f), 0.01f); // scales bask after displaying
    }

    private void OnDestroy()
    {
        EventManager.EventHammerHit -= OnHammerHit;
        EventManager.EventNailPocket -= OnNailPocket;
        EventManager.EventGameOver -= OnGameOver;
        EventManager.EventNoMoreNails -= OnGameOver;
        EventManager.EventMenuHided -= OnMenuHided;
        EventManager.EventShowSplash -= OnShowSplash;
        EventManager.EventNailFinished -= OnNailFinished;
    }
}
