using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsPanelManager : MonoBehaviour
{
    [SerializeField] GameObject CurrentHouse;
    [SerializeField] GameObject PreviousHouse;
    [SerializeField] GameObject NextHouse;

    [SerializeField] GameObject Previous;
    [SerializeField] GameObject Next;

    private Text[] levelNamesCurrentHouse;
    private Transform[] starsCurrentHouse;

    private Text[] levelNamesPreviousHouse;
    private Transform[] starsPreviousHouse;

    private Text[] levelNamesNextHouse;
    private Transform[] starsNextHouse;

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

        decimalNumberOfLevel = localHouseIndex * 10;
        for (int i = 0; i < levelNamesCurrentHouse.Length; i++)
        {
            var levelNumber = i + 1;
            levelNamesCurrentHouse[i].text = "LEVEL " + (i+1);
            levelNamesCurrentHouse[i].gameObject.GetComponent<Button>().onClick.AddListener(() => LoadLevel(decimalNumberOfLevel + levelNumber));

            if(!PlayerPrefsManager.IsLevelUnlocked(decimalNumberOfLevel + levelNumber))
            {
                levelNamesCurrentHouse[i].color = Color.grey;
                levelNamesCurrentHouse[i].gameObject.GetComponent<Button>().enabled = false;
            }

            data = LevelsDifficultyContainer.Houses[localHouseIndex].levelsData[i];
            switch(data.gainedStars)
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

        if(localHouseIndex+1==1)
        {
            levelNamesNextHouse = Finder.FindComponentsInChildrenWithTag<Text>(NextHouse, "LevelName");
            starsNextHouse = Finder.FindComponentsInChildrenWithTag<Transform>(NextHouse, "StarsLevelsPanel");
            print(levelNamesNextHouse);
            Previous.SetActive(false);
            //if(!PlayerPrefsManager.IsHouseUnlocked(localHouseNumber+1))
            //{
            //    Next.GetComponent<Image>().color = Color.grey;
            //    Next.GetComponent<Button>().enabled = false;
            //}

            decimalNumberOfLevel = (localHouseIndex+1) * 10;
            for (int i = 0; i < levelNamesNextHouse.Length; i++)
            {
                var levelNumber = i + 1;
                levelNamesNextHouse[i].text = "LEVEL " + (i + 1);
                levelNamesNextHouse[i].gameObject.GetComponent<Button>().onClick.AddListener(() => LoadLevel(decimalNumberOfLevel + levelNumber));

                if (!PlayerPrefsManager.IsLevelUnlocked(decimalNumberOfLevel + levelNumber))
                {
                    levelNamesNextHouse[i].color = Color.grey;
                    levelNamesNextHouse[i].gameObject.GetComponent<Button>().enabled = false;
                }

                data = LevelsDifficultyContainer.Houses[localHouseIndex+1].levelsData[i];
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
    }

    public void ToggleRight()
    {
        var pos = NextHouse.transform.position;
        NextHouse.transform.position = CurrentHouse.transform.position;
        CurrentHouse.transform.position = pos;

        var cur = CurrentHouse;
        CurrentHouse = NextHouse;
        NextHouse = cur;

        HandleNextHouse();
    }

    public void LoadLevel(int globalLevelNumber)
    {
        print("loading level: " + globalLevelNumber);
        levelManager.LoadLevel(globalLevelNumber);
    }

    void HandleNextHouse()
    {
        levelNamesNextHouse = Finder.FindComponentsInChildrenWithTag<Text>(NextHouse, "LevelName");
        starsNextHouse = Finder.FindComponentsInChildrenWithTag<Transform>(NextHouse, "StarsLevelsPanel");
        print(levelNamesNextHouse);

        Previous.SetActive(false);
        //if(!PlayerPrefsManager.IsHouseUnlocked(localHouseNumber+1))
        //{
        //    Next.GetComponent<Image>().color = Color.grey;
        //    Next.GetComponent<Button>().enabled = false;
        //}

        decimalNumberOfLevel = (localHouseIndex + 1) * 10;
        for (int i = 0; i < levelNamesNextHouse.Length; i++)
        {
            var levelNumber = i + 1;
            levelNamesNextHouse[i].text = "LEVEL " + (i + 1);
            levelNamesNextHouse[i].gameObject.GetComponent<Button>().onClick.AddListener(() => LoadLevel(decimalNumberOfLevel + levelNumber));

            if (!PlayerPrefsManager.IsLevelUnlocked(decimalNumberOfLevel + levelNumber))
            {
                levelNamesNextHouse[i].color = Color.grey;
                levelNamesNextHouse[i].gameObject.GetComponent<Button>().enabled = false;
            }

            data = LevelsDifficultyContainer.Houses[localHouseIndex + 1].levelsData[i];
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
}
