using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Cinemachine;

public class HouseDisplayer : MonoBehaviour
{
    [SerializeField] HousePartsArray houseParts;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject house;
    [SerializeField] private GameObject board;
    [SerializeField] private GameObject clouds;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject SummaryGround;

    private LevelData data;
    private Vector3 positionToAnimate;
    private Hammer hammer;
    private NailsSpawner nailsSpawner;
    private int index;
    private int starsForPreviousTries;

    void Start()
    {
        StartCoroutine(SetupHouse());

        hammer = FindObjectOfType<Hammer>();
        nailsSpawner = FindObjectOfType<NailsSpawner>();
        index = LevelContainer.CurrentLevelIndex;

        EventManager.EventGameOver += OnGameOver;
        EventManager.EventNoMoreNails += OnGameOver;
        
        data = LevelsDifficultyContainer.Houses[LevelContainer.CurrentHouseIndex].levelsData[index];

        starsForPreviousTries = data.gainedStars;
    }

    IEnumerator SetupHouse()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < houseParts.houseParts[LevelContainer.CurrentHouseIndex].parts.Count; i++)
        {
            var part = Instantiate(houseParts.houseParts[LevelContainer.CurrentHouseIndex].parts[i], transform.position, Quaternion.identity);
            part.transform.SetParent(house.transform);
            part.transform.localPosition = Vector3.zero;
            part.transform.localRotation = Quaternion.Euler(Vector3.zero);
            part.SetActive(false);
        }

        positionToAnimate = house.transform.GetChild(LevelContainer.CurrentLevelIndex).gameObject.transform.position;
    }
    void OnGameOver()
    {
        StartCoroutine(DisableGameplayItemsAndActiveHouse());
    }

    private void OnDestroy()
    {
        EventManager.EventGameOver -= OnGameOver;
        EventManager.EventNoMoreNails -= OnGameOver;
    }

    private IEnumerator DisableGameplayItemsAndActiveHouse()
    {
        yield return new WaitForSeconds(0.5f);

        SummaryGround.SetActive(true);
        var time = 0.8f;
        virtualCamera.m_Follow = null;
        var seqBoard = DOTween.Sequence();
        seqBoard.Append(board.transform.DOMove(board.transform.position + new Vector3(0f, -8f, 0f), time))
            // .Join(board.transform.DOScale(Vector3.zero, time))
            .Join(clouds.transform.DOMove(clouds.transform.position + new Vector3(0f, -8f, 0f), time))
            // .Join(clouds.transform.DOScale(Vector3.zero, time))
            .Join(background.transform.DOMove(background.transform.position + new Vector3(0f, -8f, 0f), 0.8f))
            //  .Join(background.transform.DOScale(Vector3.zero, time))
            .Join(hammer.transform.DOMove(background.transform.position + new Vector3(0f, -8f, 0f), 0.8f))
            // .Join(hammer.transform.DOScale(Vector3.zero, time))
            .Join(nailsSpawner.transform.DOMove(background.transform.position + new Vector3(0f, -8f, 0f), 0.8f));
         //   .Join(nailsSpawner.transform.DOScale(Vector3.zero, time));

        //board.transform.DOMove(board.transform.position + new Vector3(0f, -8f, 0f), 0.8f).;
        //clouds.transform.DOMove(clouds.transform.position + new Vector3(0f, -8f, 0f), 0.8f);
        //background.transform.DOMove(background.transform.position + new Vector3(0f, -8f, 0f), 0.8f);

        //hammer.transform.DOMove(background.transform.position + new Vector3(0f, -8f, 0f), 0.8f);
        //nailsSpawner.transform.DOMove(background.transform.position + new Vector3(0f, -8f, 0f), 0.8f);

        yield return new WaitForSeconds(0.3f);


        house.SetActive(true);
        for (int i = 0; i < index; i++)
        {
            house.transform.GetChild(i).gameObject.SetActive(true);
        }
        if (starsForPreviousTries > 0) //show if already unlocked
        { house.transform.GetChild(LevelContainer.CurrentLevelIndex).gameObject.SetActive(true); }

        yield return new WaitForSeconds(0.3f);

        StartCoroutine(ActiveAndAnimateLastPart());
    }

    private IEnumerator ActiveAndAnimateLastPart()
    {
        yield return new WaitForSeconds(1f);
        
        if (!(starsForPreviousTries > 0) && LevelContainer.StarsForCurrentTry>0) //unlock part and show
        {
            var unlockedPart = house.transform.GetChild(LevelContainer.CurrentLevelIndex).gameObject;
            unlockedPart.transform.position = positionToAnimate;
            unlockedPart.SetActive(true);
            unlockedPart.transform.DOMove(unlockedPart.transform.position + new Vector3(0f,-8f,0f), 1f).SetEase(Ease.OutBounce);
           // unlockedPart.GetComponent<DOTweenAnimation>().DOPlay();
        }
    }

    //public void ShowHouse(int houseNumber)
    //{
    //    hammer.gameObject.SetActive(false);
    //    nailsSpawner.gameObject.SetActive(false);
    //    board.SetActive(false);
        
    //    house.SetActive(true);
    //    houseParts.houseParts[LevelContainer.CurrentHouseIndex].parts[0].SetActive(true);
    //    for (int i = 1; i < LevelsDifficultyContainer.Houses[LevelContainer.CurrentHouseNumber-1].levelsData.Count; i++)
    //    {
    //        if (PlayerPrefsManager.IsLevelUnlocked(i))
    //        {
    //            houseParts.houseParts[LevelContainer.CurrentHouseIndex].parts[i].SetActive(true);
    //        }
    //    }
    //}

    //public void BackFromHouseView()
    //{
    //    hammer.gameObject.SetActive(true);
    //    nailsSpawner.gameObject.SetActive(true);
    //    board.SetActive(true);
        
    //    house.SetActive(false);
    //    houseParts.houseParts[LevelContainer.CurrentHouseIndex].parts[0].SetActive(false);
    //    for (int i = 1; i < LevelsDifficultyContainer.Houses[LevelContainer.CurrentHouseNumber-1].levelsData.Count; i++)
    //    {
    //        if (PlayerPrefsManager.IsLevelUnlocked(i))
    //        {
    //            houseParts.houseParts[LevelContainer.CurrentHouseIndex].parts[i].SetActive(false);
    //        }
    //    }
    //}
}
