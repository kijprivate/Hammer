using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
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
            // DontDestroyOnLoad(gameObject);
        }

        //if(SceneManager.GetActiveScene().buildIndex==0)
        //{
        //    LoadSplashScreenWithDelay();
        //}
        PlayerPrefsManager.UnlockLevel(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitRequest();
        }
    }

    public void LoadNextLevel()
    {
        if (LevelsDifficultyContainer.Houses[LevelContainer.CurrentHouseNumber-1].levelsData.Count > LevelContainer.CurrentLevelIndex+1)
        {
            PlayerPrefsManager.ChooseLevel(PlayerPrefsManager.GetChosenLevelNumber()+1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            LevelContainer.MenuHided = true;
        } //TODO unlock next house
        else
        {
            Debug.Log("This was last level");
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
    public void LoadMenuWithActiveLevel()
    {
        LevelContainer.MenuHided = false;
        LevelContainer.GameStarted = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LockAllLevels()
    {
        PlayerPrefsManager.LockAllLevels();
        GameObject[] lockedLevels = GameObject.FindGameObjectsWithTag("Locked");
        foreach(var locked in lockedLevels)
        {
            locked.GetComponent<Button>().enabled = true;
            locked.GetComponent<Image>().enabled = true;
        }
        for(int i=0;i< LevelsDifficultyContainer.Houses.Count; i++)
        {
            for (int j = 0; j < LevelsDifficultyContainer.Houses[i].levelsData.Count; j++)
            {
                LevelsDifficultyContainer.Houses[i].levelsData[j].gainedStars=0;
                LevelsDifficultyContainer.Houses[i].levelsData[j].highScore=0;
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
