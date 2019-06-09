using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] AppearanceData appearanceData;
    [SerializeField] GameObject GameplayUI;
    [SerializeField] GameObject MenuUI;
    [SerializeField] GameObject CF2CanvasInput;

    [Header("Gameplay")]
    [SerializeField] Text HammersLeft;
    [SerializeField] Text NailPocket;
    [SerializeField] Text LevelName;
    [SerializeField] GameObject Tutorial;
    [SerializeField] GameObject TutorialHand;
    [SerializeField] GameObject TutorialCircle;

    [Header("SummaryPanel")]
    [SerializeField] GameObject SummaryPanel;
    [SerializeField] GameObject star1,star2,star3;
    [SerializeField] GameObject PlayAgain, NextLevel;
    [SerializeField] GameObject LevelFailed;
    [SerializeField] Text ScoreSummary;
    [SerializeField] Text CoinsAdded;
    [SerializeField] Text HighScore;
    [SerializeField] GameObject ParticlesSummary;
    [SerializeField] GameObject LevelCompletedText;
    [SerializeField] GameObject HouseCompletedText;

    [Header("Shop")]
    [SerializeField] Text CoinsShop;

    [Header("Popups")]
    [SerializeField] GameObject Splash; // Image that displays splashes with hit rating 
    [SerializeField] GameObject ScoreEarned;    // Text displaying points earned with one hit
    [SerializeField] GameObject PerfectObject;    // Text displaying when perfect hit
    [SerializeField] GameObject BonusObject;    // Text displaying when perfect hit
    [SerializeField] Sprite AwesomeSprite;  // sprite with Awesome! caption
    [SerializeField] Sprite PerfectSprite;
    [SerializeField] Sprite ToohardSprite;  // sprite with Too hard! caption
    [SerializeField] Sprite HitagainSprite; // sprite with Hit again! caption
    [SerializeField] RectTransform PerfectTarget;
    [SerializeField] RectTransform SplashTarget;
    [SerializeField] RectTransform ScoreTarget;

    [Header("Sounds")]
    [SerializeField] AudioClip OkSound;
    [SerializeField] AudioClip PerfectSound;
    [SerializeField] AudioClip LevelSound;
    [SerializeField] AudioClip WoodBreakSound;
    [SerializeField] AudioClip PickSound;
    [SerializeField] AudioClip LevelFailedSound;
    [SerializeField] AudioSource MusicSource;

    private float cashed1Star;
    private float cashed2Stars;
    private float cashed3Stars;
    private LevelData data;
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
    private int numberOfNails;
    private AudioSource CanvasAudioSource;
    private int currentHighScore;
    private int score;

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

        numberOfNails = LevelContainer.NumberOfNails;
        HammersLeft.text = LevelContainer.HammerHits.ToString();
        NailPocket.text = numberOfNails.ToString();
        LevelName.text = "LEVEL "+(LevelContainer.CurrentLevelIndex+1);
        CoinsShop.text = PlayerPrefsManager.GetNumberOfCoins().ToString();

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
        CanvasAudioSource = GetComponent<AudioSource>();

        data = LevelsDifficultyContainer.Houses[LevelContainer.CurrentHouseIndex].levelsData[LevelContainer.CurrentLevelIndex];
        currentHighScore = PlayerPrefsManager.GetHighScore(LevelContainer.CurrentHouseIndex, LevelContainer.CurrentLevelIndex);

        if (!LevelContainer.MenuHided)
        {
            MenuUI.SetActive(true);
            GameplayUI.SetActive(false);
        }
        else
        {
            OnMenuHided();
        }
        Debug.Log("Canvas Manager started");
    }

    private void OnGameOver()
    {
        MusicSource.Stop();
        StartCoroutine(DisplayPoints());
        if (LevelContainer.Score > currentHighScore)
        {
            CoinsAdded.text = "+" + (LevelContainer.Score - currentHighScore) / ConstantDataContainer.ScoreDivider;
            HighScore.text = "BEST: " + LevelContainer.Score;
        }
        else
        {
            HighScore.text = "BEST: " + currentHighScore.ToString();
            CoinsAdded.text = "+0";
        }

        GameplayUI.SetActive(false);
        LevelName.gameObject.SetActive(false);
        CF2CanvasInput.SetActive(false);

        DisplayStars();
    }

    private void DisplayStars()
    {
        if (LevelContainer.PercentageValueOfScore >= cashed3Stars)
        {
            CanvasAudioSource.clip = LevelSound;
            CanvasAudioSource.Play();
            PlayAgain.SetActive(true);
            NextLevel.SetActive(true);
        }
        else if (LevelContainer.PercentageValueOfScore >= cashed2Stars)
        {
            CanvasAudioSource.clip = LevelSound;
            CanvasAudioSource.Play();
            PlayAgain.SetActive(true);
            NextLevel.SetActive(true);
        }
        else if (LevelContainer.PercentageValueOfScore >= cashed1Star)
        {
            CanvasAudioSource.clip = LevelSound;
            CanvasAudioSource.Play();
            PlayAgain.SetActive(true);
            NextLevel.SetActive(true);
        }
        else
        {
            CanvasAudioSource.clip = LevelFailedSound;
            CanvasAudioSource.Play();
            PlayAgain.SetActive(true);
            NextLevel.GetComponent<Button>().enabled = false;
            NextLevel.GetComponent<Image>().color = Color.grey;
            NextLevel.SetActive(true);
            CoinsAdded.transform.parent.gameObject.SetActive(false);
            LevelFailed.SetActive(true);
        }

        if (LevelContainer.CurrentLevelIndex == LevelsDifficultyContainer.Houses[LevelContainer.CurrentHouseIndex].levelsData.Count - 1
             && appearanceData.houses[LevelContainer.CurrentHouseIndex].NextHouseIcon)
        {
            NextLevel.GetComponent<Image>().sprite = appearanceData.houses[LevelContainer.CurrentHouseIndex].NextHouseIcon;
            LevelCompletedText.SetActive(false);
        }
    }

    private IEnumerator DisplayPoints()
    {
        yield return new WaitForSeconds(0.1f);
        SummaryPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f); // duration of sliding panel animation
        int toScore = LevelContainer.Score;
        score = 0;
        DOTween.To(() => score, x => score = x, toScore, 3).OnUpdate(UpdateUI).OnComplete(UpdateUI).OnComplete(OnCompleteScoreAnimation);

    }

    private void UpdateUI()
    {
        ScoreSummary.text = score.ToString();

        if ((float)score/LevelContainer.MaxAvailableScore >= cashed1Star)
        {
            star1.SetActive(true);
        }
        if ((float)score / LevelContainer.MaxAvailableScore >= cashed2Stars)
        {
            star2.SetActive(true);
        }
        if ((float)score / LevelContainer.MaxAvailableScore >= cashed3Stars)
        {
            star3.SetActive(true);
        }
    }

    private void OnCompleteScoreAnimation()
    {
        if ((float)score / LevelContainer.MaxAvailableScore >= cashed1Star &&
            LevelContainer.CurrentLevelIndex == LevelsDifficultyContainer.Houses[LevelContainer.CurrentHouseIndex].levelsData.Count - 1
            && appearanceData.houses[LevelContainer.CurrentHouseIndex].NextHouseIcon)
        {
            ParticlesSummary.SetActive(true);
            HouseCompletedText.SetActive(true);
        }
        else if ((float)score / LevelContainer.MaxAvailableScore >= cashed1Star)
        {
            LevelCompletedText.SetActive(true);
        }
    }
    
    private void OnHammerHit()
    {
        HammersLeft.text = LevelContainer.HammerHits.ToString();
    }

    private void OnNailPocket()
    {
        //ScoreGameplay.text = LevelContainer.Score.ToString();

        //if (LevelContainer.Score == LevelContainer.MaxAvailableScore)
        //{
        //    ScoreGameplay.text = LevelContainer.Score + "/" + LevelContainer.MaxAvailableScore; //TODO change if 3 stars != 100% max score
        //    StarsGameplay[0].SetActive(true);
        //    StarsGameplay[1].SetActive(true);
        //    StarsGameplay[2].SetActive(true);
        //}
        //else if (LevelContainer.Score + 5 >= LevelContainer.MaxAvailableScore * cashed2Stars)
        //{
        //    ScoreGameplay.text = LevelContainer.Score + "/" + LevelContainer.MaxAvailableScore; //TODO change if 3 stars != 100% max score
        //    StarsGameplay[0].SetActive(true);
        //    StarsGameplay[1].SetActive(true);
        //}
        //else if (LevelContainer.Score + 5 >= LevelContainer.MaxAvailableScore * cashed1Star)
        //{
        //    ScoreGameplay.text = LevelContainer.Score + "/" + (int)(LevelContainer.MaxAvailableScore * cashed2Stars);
        //    StarsGameplay[0].SetActive(true);
        //}
        //else if (LevelContainer.Score + 5 < LevelContainer.MaxAvailableScore * cashed1Star)
        //{
        //    ScoreGameplay.text = LevelContainer.Score + "/" + (int)(LevelContainer.MaxAvailableScore * cashed1Star);
        //}
    }

    private void OnNailFinished()
    {
        numberOfNails--;
        NailPocket.text = numberOfNails.ToString();
    }
    
    private void OnCoinsSubstracted()
    {
        CoinsShop.text = PlayerPrefsManager.GetNumberOfCoins().ToString();
    }

    private void OnMenuHided()
    {
        MenuUI.SetActive(false);
        GameplayUI.SetActive(true);

        if(!global::Tutorial.wasPlayed)
        {
            Tutorial.SetActive(true);
            global::Tutorial.wasPlayed = true;
            var sequence = DOTween.Sequence();
            sequence.Append(TutorialHand.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f))
                    .Join(TutorialCircle.transform.DOScale(new Vector3(0.6f, 0.35f, 1f), 0.4f))
                    .Append(TutorialHand.transform.DOScale(new Vector3(1f, 1f, 1f), 0.8f))
                    .Append(TutorialHand.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f))
                   // .Join(TutorialHand.GetComponent<RectTransform>().DOAnchorPos(new Vector3(34f, 34f, 0f), 0.5f))
                    .Join(TutorialCircle.transform.DOScale(new Vector3(0f, 0f, 0f), 0.4f))
                    .AppendInterval(0.5f)
                    .SetLoops(-1);
        }
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
                CanvasAudioSource.clip = OkSound;
                CanvasAudioSource.Play();
                break;
            case 1: // too much strength
                SplashImage.sprite = ToohardSprite;
                CanvasAudioSource.clip = WoodBreakSound;
                CanvasAudioSource.Play();
                break;
        }
        SplashRect.position = SplashPosition; // moves back before displaying
        SplashRect.localScale = SplashScale;  // scales back before displaying
        Splash.SetActive(true);
        SplashRect.DOScale(new Vector3(1.3f, 1.3f, 1.0f), 0.5f).SetEase(Ease.OutBounce);   // scales splash for display
        SplashRect.DOMove(SplashTarget.position, 0.5f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.7f);    
        Splash.SetActive(false);
    }

    private void OnShowEarnedScore(int points)
    {
        StartCoroutine(DisplayEarnedScore(points));
    }

    private IEnumerator DisplayEarnedScore(int earnedPoints)    // displays text with earned pointes
    {
        ScoreEarned.SetActive(true);
        EarnedScoreRect.position = EarnedScorePosition;
        EarnedScoreText.text = "+" + earnedPoints;
        Sequence earnedScoreSequence = DOTween.Sequence();
        earnedScoreSequence.Append(EarnedScoreText.DOColor(new Color(1.0f,1.0f,1.0f,1.0f), 0.4f));
        earnedScoreSequence.AppendInterval(0.2f);
        earnedScoreSequence.Append(EarnedScoreText.DOColor(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.4f));
        earnedScoreSequence.Insert(0, EarnedScoreRect.DOMove(ScoreTarget.position, 1.0f));
        yield return new WaitForSeconds(1.0f);
        ScoreEarned.SetActive(false);
    }

    private void OnPerfectHit()
    {
        StartCoroutine(DisplayPerfectHit());
    }

    private IEnumerator DisplayPerfectHit()    // displays text with earned pointes
    {
        CanvasAudioSource.clip = PerfectSound;
        CanvasAudioSource.Play();
        PerfectObject.SetActive(true);
        BonusObject.SetActive(true);
        PerfectRect.localScale = PerfectScale; // scales back before displaying
        PerfectRect.position = PerfectPosition; // moves back before displaying
        PerfectRect.DOScale(new Vector3(1.3f, 1.3f, 1.0f), 0.5f).SetEase(Ease.OutBounce);   // scales perfect for display
        PerfectRect.DOMove(PerfectTarget.position, 0.5f).SetEase(Ease.OutBounce);
        Sequence bonusSequence = DOTween.Sequence();
        bonusSequence.Append(BonusText.DOColor(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.4f));
        bonusSequence.AppendInterval(0.2f);
        bonusSequence.Append(BonusText.DOColor(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.4f));
        bonusSequence.Insert(0, BonusRect.DOMove(ScoreTarget.position - new Vector3(0.0f, 89.0f,0.0f), 1.0f));
        yield return new WaitForSeconds(1.0f);
        BonusRect.position = BonusPosition;
        PerfectObject.SetActive(false);
        BonusObject.SetActive(false);
    }

    public void HideMenuAndStartGame()
    {
        StartCoroutine(CoroutineHideMenuAndStartGame());
    }

    public void ItemPicked()
    {
        Debug.Log("Playing pick sound");
        CanvasAudioSource.clip = PickSound;
        CanvasAudioSource.Play();
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
