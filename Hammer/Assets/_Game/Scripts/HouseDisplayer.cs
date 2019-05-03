using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HouseDisplayer : MonoBehaviour
{
    [SerializeField] HousePartsArray houseParts;
    [SerializeField] private GameObject house;
    [SerializeField] private GameObject board;
    [SerializeField] private GameObject SummaryGround;

    private LevelData data;
    private Vector3 positionToAnimate;
    private Hammer hammer;
    private NailsSpawner nailsSpawner;
    private int index;
    private int starsForPreviousTries;

    void Start()
    {

        for (int i = 0; i < houseParts.houseParts[LevelContainer.CurrentHouseNumber - 1].parts.Count; i++)
        {
            var part = Instantiate(houseParts.houseParts[LevelContainer.CurrentHouseIndex].parts[i],transform.position,Quaternion.identity);
            part.transform.SetParent(house.transform);
            part.transform.localPosition = Vector3.zero;
            part.transform.localRotation = Quaternion.Euler(Vector3.zero);
            part.SetActive(false);
        }

        hammer = FindObjectOfType<Hammer>();
        nailsSpawner = FindObjectOfType<NailsSpawner>();
        index = LevelContainer.CurrentLevelIndex;

        EventManager.EventGameOver += OnGameOver;
        EventManager.EventNoMoreNails += OnGameOver;
        
        data = LevelsDifficultyContainer.Houses[LevelContainer.CurrentHouseIndex].levelsData[index];

        starsForPreviousTries = data.gainedStars;
    }

    void OnGameOver()
    {
        StartCoroutine(DisableGameplayItemsAndActiveHouse());
        StartCoroutine(ActiveAndAnimateLastPart());
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

        yield return new WaitForSeconds(0.3f);

        house.SetActive(true);
        for (int i = 0; i < index; i++)
        {
            house.transform.GetChild(i).gameObject.SetActive(true);
        }
        if (starsForPreviousTries > 0) //show if already unlocked
        { house.transform.GetChild(LevelContainer.CurrentLevelIndex).gameObject.SetActive(true); }

        yield return new WaitForSeconds(0.3f);
        hammer.gameObject.SetActive(false);
        nailsSpawner.gameObject.SetActive(false);
        board.SetActive(false);
    }

    private IEnumerator ActiveAndAnimateLastPart()
    {
        yield return new WaitForSeconds(1f);
        
        if (!(starsForPreviousTries > 0) && LevelContainer.StarsForCurrentTry>0) //unlock part and show
        {
            house.transform.GetChild(LevelContainer.CurrentLevelIndex).gameObject.SetActive(true);
        }
    }

    public void ShowHouse(int houseNumber)
    {
        hammer.gameObject.SetActive(false);
        nailsSpawner.gameObject.SetActive(false);
        board.SetActive(false);
        
        house.SetActive(true);
        houseParts.houseParts[LevelContainer.CurrentHouseIndex].parts[0].SetActive(true);
        for (int i = 1; i < LevelsDifficultyContainer.Houses[LevelContainer.CurrentHouseNumber-1].levelsData.Count; i++)
        {
            if (PlayerPrefsManager.IsLevelUnlocked(i))
            {
                houseParts.houseParts[LevelContainer.CurrentHouseIndex].parts[i].SetActive(true);
            }
        }
    }

    public void BackFromHouseView()
    {
        hammer.gameObject.SetActive(true);
        nailsSpawner.gameObject.SetActive(true);
        board.SetActive(true);
        
        house.SetActive(false);
        houseParts.houseParts[LevelContainer.CurrentHouseIndex].parts[0].SetActive(false);
        for (int i = 1; i < LevelsDifficultyContainer.Houses[LevelContainer.CurrentHouseNumber-1].levelsData.Count; i++)
        {
            if (PlayerPrefsManager.IsLevelUnlocked(i))
            {
                houseParts.houseParts[LevelContainer.CurrentHouseIndex].parts[i].SetActive(false);
            }
        }
    }
}
