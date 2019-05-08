using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsPanelManager : MonoBehaviour
{
    [SerializeField] GameObject CurrentHouse;

    [SerializeField] GameObject Previous;
    [SerializeField] GameObject Next;

    private Text[] levelNamesCurrentHouse;
    private Transform[] starsCurrentHouse;

    private LevelManager levelManager;
    private LevelData data;
    private int localHouseIndex;
    private int decimalNumberOfLevel;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        levelNamesCurrentHouse = Finder.FindComponentsInChildrenWithTag<Text>(CurrentHouse, "LevelName");
        starsCurrentHouse = Finder.FindComponentsInChildrenWithTag<Transform>(CurrentHouse, "StarsLevelsPanel");

        localHouseIndex = LevelContainer.CurrentHouseIndex;

        HandleToggleButtons();

        DisplayingLevelsAndStarsLoop();
    }

    public void ToggleRight()
    {

        if (localHouseIndex < LevelsDifficultyContainer.Houses.Count - 1)
        { localHouseIndex++; }

        HandleToggleButtons();

        DisplayingLevelsAndStarsLoop();
    }

    public void ToggleLeft()
    {
        if (localHouseIndex == 0)
        { return; }

        localHouseIndex--;

        HandleToggleButtons();

        DisplayingLevelsAndStarsLoop();
    }

    private void DisplayingLevelsAndStarsLoop()
    {
        decimalNumberOfLevel = localHouseIndex * 10;

        for (int i = 0; i < levelNamesCurrentHouse.Length; i++)
        {
            var levelNumber = i + 1;
            levelNamesCurrentHouse[i].text = "LEVEL " + (i + 1);
            levelNamesCurrentHouse[i].gameObject.GetComponent<Button>().onClick.AddListener(() => LoadLevel(decimalNumberOfLevel + levelNumber));

            if (!PlayerPrefsManager.IsLevelUnlocked(decimalNumberOfLevel + levelNumber))
            {
                levelNamesCurrentHouse[i].color = Color.grey;
                levelNamesCurrentHouse[i].gameObject.GetComponent<Button>().enabled = false;
            }
            else
            {
                levelNamesCurrentHouse[i].color = Color.white;
                levelNamesCurrentHouse[i].gameObject.GetComponent<Button>().enabled = true;
            }

            data = LevelsDifficultyContainer.Houses[localHouseIndex].levelsData[i];

            starsCurrentHouse[i].GetChild(0).gameObject.SetActive(false);
            starsCurrentHouse[i].GetChild(1).gameObject.SetActive(false);
            starsCurrentHouse[i].GetChild(2).gameObject.SetActive(false);
            switch (data.gainedStars)
            {
                case 3:
                    starsCurrentHouse[i].GetChild(0).gameObject.SetActive(true);
                    break;
                case 2:
                    starsCurrentHouse[i].GetChild(1).gameObject.SetActive(true);
                    break;
                case 1:
                    starsCurrentHouse[i].GetChild(2).gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }

    private void HandleToggleButtons()
    {
        if (localHouseIndex == 0)
        {
            Previous.SetActive(false);
        }
        else
        {
            Previous.SetActive(true);
        }

        if (localHouseIndex == LevelsDifficultyContainer.Houses.Count - 1)
        {
            Next.SetActive(false);
        }
        else
        {
            Next.SetActive(true);
        }

        if (!PlayerPrefsManager.IsHouseUnlocked(localHouseIndex + 2))
        {
            Next.GetComponent<Image>().color = Color.grey;
            Next.GetComponent<Button>().enabled = false;
        }
        else
        {
            Next.GetComponent<Image>().color = Color.white;
            Next.GetComponent<Button>().enabled = true;
        }
    }

    public void LoadLevel(int globalLevelNumber)
    {
        levelManager.LoadLevel(globalLevelNumber);
    }
}
