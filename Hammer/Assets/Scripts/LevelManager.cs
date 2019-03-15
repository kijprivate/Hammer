using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    //private void Update()
    //{
    //    if(SceneManager.GetActiveScene().name=="Game" && Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        QuitRequest();
    //    }
    //}
    public void LoadNextLevel()
    {
        if (SceneManager.sceneCountInBuildSettings - 1 > SceneManager.GetActiveScene().buildIndex + 1)
        { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }
        else
        { Debug.LogWarning("This is last level"); }
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitRequest()
    {
        Application.Quit();
    }
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void LoadSplashScreenWithDelay()
    {
        Invoke("SplashScreen", 5f);
    }
    public void SplashScreen()
    {
        SceneManager.LoadScene("Game");
    }
}
