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
        if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            LevelContainer.MenuHided = true;
        }
        else
        { Debug.LogWarning("This is last level"); }
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
    public void LoadLevel(string name)
    {
        LevelContainer.MenuHided = false;
        SceneManager.LoadScene(name);
    }
    public void LoadMenuWithActiveLevel()
    {
        LevelContainer.MenuHided = false;
        LevelContainer.GameStarted = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadSplashScreenWithDelay()
    {
        Invoke("SplashScreen", 5f);
    }
    public void SplashScreen()
    {
        SceneManager.LoadScene("Game");
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
    }
}
