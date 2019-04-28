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
    [SerializeField] GameObject ScoreEarned;    // Text displaying points earned with one hit
    [SerializeField] GameObject PerfectObject;    // Text displaying when perfect hit
    [SerializeField] GameObject BonusObject;    // Text displaying when perfect hit
    [SerializeField] Text ScoreGameplay;
    [SerializeField] Text ScoreSummary;
    [SerializeField] Text HammersLeft;
    [SerializeField] Text NailPocket;
    [SerializeField] Text LevelName;
    [SerializeField] Text Coins;
    [SerializeField] Sprite AwesomeSprite;  // sprite with Awesome! caption
    [SerializeField] Sprite PerfectSprite;
    [SerializeField] Sprite ToohardSprite;  // sprite with Too hard! caption
    [SerializeField] Sprite HitagainSprite; // sprite with Hit again! caption

    private float cashed1Star;
    private float cashed2Stars;
    private float cashed3Stars;
    private RectTransform SplashRect;   // needed for changing scale when displaying Splash
    private Image SplashImage;    // needed for changing Sprite depending on hit rating
    private Vector3 SplashPosition;
    private Vector3 SplashScale;
    private RectTransform EarnedScoreRect;  // needed for changing position of earned score text
    private Text EarnedScoreText;   // text displayed when points earned
    private Vector3 EarnedScorePosition;
    private RectTransform BonusRect;
    private Text BonusText;
    private Vector3 BonusPosition;
    private RectTransform PerfectRect;   // needed for changing scale when displaying Splash
    private Vector3 PerfectPosition;
    private Vector3 PerfectScale;
    private int numberofnails;
    private void Start()
    {
        EventManager.EventGameOver += OnGameOver;
        EventManager.EventNoMoreNails += OnGameOver;
        EventManager.EventHammerHit += OnHammerHit;
        EventManager.EventNailPocket += OnNailPocket;
        EventManager.EventMenuHided += OnMenuHided;
        EventManager.EventShowSplash += OnShowSplash;
        EventManager.EventNailFinished += OnNailFinished;
        EventManager.EventEarnScore += OnShowEarnedScore;
        EventManager.EventCoinsSubstracted += OnCoinsSubstracted;
        EventManager.EventPerfectHit += OnPerfectHit;

        numberofnails = LevelContainer.NumberOfNails;
        print(PlayerPrefsManager.GetNumberOfCoins());
        HammersLeft.text = LevelContainer.HammerHits.ToString();
        NailPocket.text = numberofnails.ToString();
        LevelName.text = "Level"+LevelContainer.CurrentLevelNumber;
        Coins.text = "Coins: " + PlayerPrefsManager.GetNumberOfCoins();

        cashed1Star = ConstantDataContainer.PercentageValueFor1Star/100f;
        cashed2Stars = ConstantDataContainer.PercentageValueFor2Stars/100f;
        cashed3Stars = ConstantDataContainer.PercentageValueFor3Stars/100f;
        SplashRect = Splash.GetComponent<RectTransform>();
        SplashImage = Splash.GetComponent<Image>();
        EarnedScoreText = ScoreEarned.GetComponent<Text>();
        EarnedScoreRect = ScoreEarned.GetComponent<RectTransform>();
        PerfectRect = PerfectObject.GetComponent<RectTransform>();
        BonusText = BonusObject.GetComponent<Text>();
        BonusRect = BonusObject.GetComponent<RectTransform>();
        SplashPosition = SplashRect.position;
        SplashScale = SplashRect.localScale;
        EarnedScorePosition = EarnedScoreRect.position; 
        BonusPosition = BonusRect.position;
        PerfectPosition = PerfectRect.position;
        PerfectScale = PerfectRect.localScale;


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
        ScoreGameplay.text = LevelContainer.Score.ToString();
       // ScoreGameplay.text = LevelContainer.Score +"/"+LevelContainer.MaxAvailableScore*(ConstantDataContainer.PercentageValueFor1Star/100f);
    }
    
    private void OnHammerHit()
    {
        HammersLeft.text = LevelContainer.HammerHits.ToString();
    }

    private void OnNailPocket()
    {
        ScoreGameplay.text = LevelContainer.Score.ToString();

        //if (LevelContainer.Score >
        //    LevelContainer.MaxAvailableScore * (ConstantDataContainer.PercentageValueFor3Stars / 100f) &&
        //    LevelContainer.Score >
        //    LevelsDifficultyContainer.Houses[LevelContainer.CurrentHouseNumber-1].levelsData[LevelContainer.CurrentLevelIndex].highScore)
        //{
        //    ScoreGameplay.text = LevelContainer.Score.ToString();
        //}
        //else if (LevelContainer.Score >
        //    LevelContainer.MaxAvailableScore * (ConstantDataContainer.PercentageValueFor3Stars / 100f) &&
        //    LevelContainer.Score < LevelsDifficultyContainer.Houses[LevelContainer.CurrentHouseNumber-1].levelsData[LevelContainer.CurrentLevelIndex].highScore)
        //{
        //    ScoreGameplay.text = LevelContainer.Score +"/"+LevelsDifficultyContainer.Houses[LevelContainer.CurrentHouseNumber-1].levelsData[LevelContainer.CurrentLevelIndex].highScore;
        //}
        //else if (LevelContainer.Score >
        //    LevelContainer.MaxAvailableScore * (ConstantDataContainer.PercentageValueFor2Stars / 100f))
        //{
        //    ScoreGameplay.text = LevelContainer.Score +"/"+LevelContainer.MaxAvailableScore*(ConstantDataContainer.PercentageValueFor3Stars/100f);
        //}
        //else if (LevelContainer.Score >
        //    LevelContainer.MaxAvailableScore * (ConstantDataContainer.PercentageValueFor1Star / 100f))
        //{
        //    ScoreGameplay.text = LevelContainer.Score +"/"+LevelContainer.MaxAvailableScore*(ConstantDataContainer.PercentageValueFor2Stars/100f);
        //}
        //else if (LevelContainer.Score <
        //    LevelContainer.MaxAvailableScore * (ConstantDataContainer.PercentageValueFor1Star / 100f))
        //{
        //    ScoreGameplay.text = LevelContainer.Score + "/" + LevelContainer.MaxAvailableScore *
        //                         (ConstantDataContainer.PercentageValueFor1Star / 100f);
        //}
    }

    private void OnNailFinished()
    {
        numberofnails--;
        NailPocket.text = numberofnails.ToString();
    }
    
    private void OnCoinsSubstracted()
    {
        Coins.text = "Coins: " + PlayerPrefsManager.GetNumberOfCoins();
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
            case 0: // ok hit
                SplashImage.sprite = AwesomeSprite;
                break;
            case 1: // too much strength
                SplashImage.sprite = ToohardSprite;
                break;
        }
        Splash.SetActive(true);
        SplashRect.DOScale(new Vector3(1.2f, 1.2f, 1.0f), 0.5f).SetEase(Ease.OutElastic);   // scales splash for display
        SplashRect.DOMove(SplashRect.position + new Vector3(400.0f, 400.0f, 0.0f), 0.5f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.7f);    
        SplashRect.position = SplashPosition; // moves back after displaying
        SplashRect.localScale = SplashScale;  // scales back after displaying
        Splash.SetActive(false);
    }

    private void OnShowEarnedScore(int points)
    {
        StartCoroutine(DisplayEarnedScore(points));
    }

    private IEnumerator DisplayEarnedScore(int earnedPoints)    // displays text with earned pointes
    {
        ScoreEarned.SetActive(true);
        EarnedScoreText.text = "+" + earnedPoints;
        Sequence earnedScoreSequence = DOTween.Sequence();
        earnedScoreSequence.Append(EarnedScoreText.DOColor(new Color(1.0f,1.0f,1.0f,1.0f), 0.6f));
        earnedScoreSequence.AppendInterval(0.2f);
        earnedScoreSequence.Append(EarnedScoreText.DOColor(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.6f));
        earnedScoreSequence.Insert(0, EarnedScoreRect.DOMove(EarnedScoreRect.position + new Vector3(0.0f, 300.0f, 0.0f), 1.4f));
        yield return new WaitForSeconds(1.4f);
        EarnedScoreRect.position = EarnedScorePosition;
        ScoreEarned.SetActive(false);
    }

    private void OnPerfectHit()
    {
        StartCoroutine(DisplayPerfectHit());
    }

    private IEnumerator DisplayPerfectHit()    // displays text with earned pointes
    {
        PerfectObject.SetActive(true);
        BonusObject.SetActive(true);
        PerfectRect.DOScale(new Vector3(1.5f, 1.5f, 1.0f), 0.5f).SetEase(Ease.OutElastic);   // scales perfect for display
        PerfectRect.DOMove(PerfectRect.position + new Vector3(-150.0f, 930.0f, 0.0f), 0.5f).SetEase(Ease.OutBounce);
        Sequence bonusSequence = DOTween.Sequence();
        bonusSequence.Append(BonusText.DOColor(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.6f));
        bonusSequence.AppendInterval(0.2f);
        bonusSequence.Append(BonusText.DOColor(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.6f));
        bonusSequence.Insert(0, BonusRect.DOMove(BonusRect.position + new Vector3(0.0f, 300.0f, 0.0f), 1.4f));
        yield return new WaitForSeconds(1.4f);
        BonusRect.position = BonusPosition;
        PerfectRect.localScale = PerfectScale; // scales back after displaying
        PerfectRect.position = PerfectPosition;
        PerfectObject.SetActive(false);
        BonusObject.SetActive(false);
    }

    public void HideMenuAndStartGame()
    {
        StartCoroutine(CoroutineHideMenuAndStartGame());
    }
    private IEnumerator CoroutineHideMenuAndStartGame()
    {
        EventManager.RaiseEventMenuHided();
        yield return new WaitForSeconds(0.1f);
        EventManager.RaiseEventGameStarted();
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
        EventManager.EventCoinsSubstracted -= OnCoinsSubstracted;
        EventManager.EventEarnScore -= OnShowEarnedScore;
        EventManager.EventPerfectHit -= OnPerfectHit;
    }
}
