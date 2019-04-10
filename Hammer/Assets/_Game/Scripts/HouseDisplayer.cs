using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HouseDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject house;
    [SerializeField] private GameObject[] houseParts;
    [SerializeField] private GameObject board;

    private LevelData data;
    private Vector3 positionToAnimate;
    private Hammer hammer;
    private NailsSpawner nailsSpawner;
    private int index;
    private int starsForPreviousTries;
    void Start()
    {
        hammer = FindObjectOfType<Hammer>();
        nailsSpawner = FindObjectOfType<NailsSpawner>();
        index = LevelContainer.CurrentLevelIndex;
        positionToAnimate = houseParts[index].transform.position;
        EventManager.EventGameOver += OnGameOver;
        EventManager.EventNoMoreNails += OnGameOver;
        
        data = LevelsDifficultyContainer.LevelsData[index];

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
        
        house.SetActive(true);
        for (int i = 0; i < index; i++)
        {
            houseParts[i].SetActive(true);
        }
        if(starsForPreviousTries>0) //show if already unlocked
        { houseParts[index].SetActive(true);}
        
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
            houseParts[index].SetActive(true);
            houseParts[index].transform.position = positionToAnimate;
            houseParts[index].GetComponent<DOTweenAnimation>().DOPlay();
        }
    }
}
