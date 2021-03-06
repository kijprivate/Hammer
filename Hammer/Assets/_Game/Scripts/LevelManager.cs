﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject QuitPanel;
    [SerializeField] Button CancelQuitPanel;
    [SerializeField] Button PauseButton;
    [SerializeField] Button ResumeButton;
    [SerializeField] GameObject EndGamePanel;

    public bool isPaused { get; set; }
    public bool quitPanelActive { get; set; }

    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        PlayerPrefsManager.UnlockLevel(1);
        PlayerPrefsManager.UnlockHouse(1);
        PlayerPrefsManager.UnlockHammer(0);
    }

    private void OnApplicationPause(bool pause)
    {
        if(LevelContainer.MenuHided && !isPaused && !LevelContainer.GameOver)
        {
           // PausePanel.SetActive(true);
            PauseButton.onClick.Invoke();
            isPaused = true;
        }
    }

    private void Update()
    {
        if((Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Menu)) && LevelContainer.MenuHided && !isPaused && !LevelContainer.GameOver)
        {
          //  PausePanel.SetActive(true);
            PauseButton.onClick.Invoke();
            isPaused = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !LevelContainer.MenuHided && !quitPanelActive)
        {
            QuitPanel.SetActive(true);
            quitPanelActive = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !LevelContainer.MenuHided && quitPanelActive)
        {
            CancelQuitPanel.onClick.Invoke();
            quitPanelActive = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && LevelContainer.MenuHided && !isPaused && !LevelContainer.GameOver)
        {
           // PausePanel.SetActive(true);
            PauseButton.onClick.Invoke();
            isPaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && LevelContainer.MenuHided && isPaused && !LevelContainer.GameOver)
        {
          //  PausePanel.SetActive(false);
            ResumeButton.onClick.Invoke();
            isPaused = false;
        }
    }

    public void LoadNextLevel()
    {
        if (LevelsDifficultyContainer.Houses[LevelContainer.CurrentHouseIndex].levelsData.Count > LevelContainer.CurrentLevelIndex+1)
        {
            PlayerPrefsManager.ChooseLevel(PlayerPrefsManager.GetChosenLevelNumber()+1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            LevelContainer.MenuHided = true;
        } 
        else if(LevelsDifficultyContainer.Houses.Count > LevelContainer.CurrentHouseNumber)
        {
            PlayerPrefsManager.UnlockHouse(LevelContainer.CurrentHouseNumber + 1);
            PlayerPrefsManager.ChooseLevel(PlayerPrefsManager.GetChosenLevelNumber() + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            LevelContainer.MenuHided = true;
        }
        else
        {
            EndGamePanel.SetActive(true);
        }
    }
    public void ReloadLevel()
    {
        LevelContainer.MenuHided = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitRequest()
    {
        Application.Quit();
    }
    public void LoadLevel(int levelNumber)
    {
        PlayerPrefsManager.ChooseLevel(levelNumber);
        LevelContainer.MenuHided = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadLevelAndHideMenu(int levelNumber)
    {
        PlayerPrefsManager.ChooseLevel(levelNumber);
        LevelContainer.MenuHided = true;
        LevelContainer.GameStarted = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadMenuWithActiveLevel()
    {
        LevelContainer.MenuHided = false;
        LevelContainer.GameStarted = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LockAllLevels()
    {
        PlayerPrefsManager.LockAllLevels();

        for(int i=0;i< LevelsDifficultyContainer.Houses.Count; i++)
        {
            for (int j = 0; j < LevelsDifficultyContainer.Houses[i].levelsData.Count; j++)
            {
                PlayerPrefsManager.SetGainedStars(i, j, 0);
                PlayerPrefsManager.SetHighScore(i, j, 0);
            }
        }
    }
    
    public void LockAllHammers()
    {
        PlayerPrefsManager.LockAllHammers();
    }

    public void AddCoins()
    {
        PlayerPrefsManager.SetNumberOfCoins(10000);
    }

    public void UnlockAllLevels()
    {
        PlayerPrefsManager.UnlockAllLevels();
        GameObject[] lockedLevels = GameObject.FindGameObjectsWithTag("Locked");
        foreach(var locked in lockedLevels)
        {
            locked.GetComponent<Button>().enabled = false;
            locked.GetComponent<Image>().enabled = false;
        }
    }
}
