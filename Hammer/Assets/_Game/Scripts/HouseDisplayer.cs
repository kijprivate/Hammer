using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class HouseDisplayer : MonoBehaviour
{
    public static bool LoadedFromHouseView = false;

    [SerializeField] HousePartsArray houseParts;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject house;
    [SerializeField] private GameObject specialItems;
    [SerializeField] private GameObject board;
    [SerializeField] private GameObject clouds;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject SummaryGround;

    private LevelData data;
    private Vector3 positionToAnimate;
    private Hammer hammer;
    private NailsSpawner nailsSpawner;
    private Sequence sequence;
    private float animationTime = 0.8f;
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

        starsForPreviousTries = PlayerPrefsManager.GetGainedStars(LevelContainer.CurrentHouseIndex, LevelContainer.CurrentLevelIndex);
    }

    IEnumerator SetupHouse()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i <= LevelContainer.CurrentLevelIndex; i++)
        {
            var part = Instantiate(houseParts.houseParts[LevelContainer.CurrentHouseIndex].parts[i], transform.position, Quaternion.identity);
            part.transform.SetParent(house.transform);
            part.transform.localPosition = Vector3.zero;
            part.transform.localRotation = Quaternion.Euler(Vector3.zero);
            part.SetActive(false);
        }
        for (int i = 0; i < houseParts.houseParts[LevelContainer.CurrentHouseIndex].specialItems.Count; i++)
        {
            var special = Instantiate(houseParts.houseParts[LevelContainer.CurrentHouseIndex].specialItems[i], transform.position, Quaternion.identity);
            special.transform.SetParent(specialItems.transform);
            special.transform.localPosition = Vector3.zero;
            special.transform.localRotation = Quaternion.Euler(Vector3.zero);
            special.SetActive(false);
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

        virtualCamera.m_Follow = null;
        //sequence = DOTween.Sequence();
        sequence.Append(board.transform.DOMove(board.transform.position + new Vector3(0f, -8f, 0f), animationTime))
            .Join(clouds.transform.DOMove(clouds.transform.position + new Vector3(0f, -8f, 0f), animationTime))
            .Join(background.transform.DOMove(background.transform.position + new Vector3(0f, -8f, 0f), animationTime))
            .Join(hammer.transform.DOMove(background.transform.position + new Vector3(0f, -8f, 0f), animationTime))
            .Join(nailsSpawner.transform.DOMove(background.transform.position + new Vector3(0f, -8f, 0f), animationTime));

        yield return new WaitForSeconds(0.3f);

        house.SetActive(true);
        for (int i = 0; i < index; i++)
        {
            house.transform.GetChild(i).gameObject.SetActive(true);
        }
        if (starsForPreviousTries > 0) //show if already unlocked
        { house.transform.GetChild(LevelContainer.CurrentLevelIndex).gameObject.SetActive(true); }

        specialItems.SetActive(true);
        for (int i = 0; i < specialItems.transform.childCount; i++)
        {
            if(PlayerPrefsManager.IsSpecialItemUnlocked(i,LevelContainer.CurrentHouseNumber))
            {
                specialItems.transform.GetChild(i).gameObject.SetActive(true);
            }
        }

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
        }
    }

    public void ShowHouse(int houseNumber)
    {
        StartCoroutine(HouseAnimations(houseNumber));
    }

    private IEnumerator HouseAnimations(int houseNumber)
    {
        SummaryGround.SetActive(true);
        virtualCamera.m_Follow = null;

        sequence.Append(board.transform.DOMove(board.transform.position + new Vector3(0f, -8f, 0f), animationTime))
            .Join(clouds.transform.DOMove(clouds.transform.position + new Vector3(0f, -8f, 0f), animationTime))
            .Join(background.transform.DOMove(background.transform.position + new Vector3(0f, -8f, 0f), animationTime))
            .Join(hammer.transform.DOMove(background.transform.position + new Vector3(0f, -8f, 0f), animationTime))
            .Join(nailsSpawner.transform.DOMove(background.transform.position + new Vector3(0f, -8f, 0f), animationTime));

        for (int i = 0; i < house.transform.childCount ; i++)
        {
            Destroy(house.transform.GetChild(i).gameObject);
        }

        foreach(Transform child in specialItems.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < houseParts.houseParts[houseNumber-1].parts.Count; i++)
        {
            var part = Instantiate(houseParts.houseParts[houseNumber-1].parts[i], transform.position, Quaternion.identity);
            part.transform.SetParent(house.transform);
            part.transform.localPosition = Vector3.zero;
            part.transform.localRotation = Quaternion.Euler(Vector3.zero);
            part.SetActive(false);
        }
        for (int i = 0; i < houseParts.houseParts[houseNumber-1].specialItems.Count; i++)
        {
            var special = Instantiate(houseParts.houseParts[houseNumber-1].specialItems[i], transform.position, Quaternion.identity);
            special.transform.SetParent(specialItems.transform);
            special.transform.localPosition = Vector3.zero;
            special.transform.localRotation = Quaternion.Euler(Vector3.zero);
            special.SetActive(false);
        }
        yield return new WaitForSeconds(0.3f);

        var decimalPart = (houseNumber - 1) * 10;
        house.SetActive(true);

        for (int i = 0; i < LevelsDifficultyContainer.Houses[houseNumber-1].levelsData.Count; i++)
        {
            if (PlayerPrefsManager.IsLevelUnlocked(decimalPart + (i+2)))
            {
                house.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        specialItems.SetActive(true);
        for (int i = 0; i < specialItems.transform.childCount; i++)
        {
            if (PlayerPrefsManager.IsSpecialItemUnlocked(i, houseNumber))
            {
                specialItems.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void BackFromHouseView()
    {
        LoadedFromHouseView = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
