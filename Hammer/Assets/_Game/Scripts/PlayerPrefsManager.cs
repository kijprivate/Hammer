using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour
{
    const string LEVEL_KEY = "level_unlocked_";
    const string CHOSEN_LEVEL_KEY = "chosen_level_key_";

    const string BALL_NEWKEY = "ball_unlocked_";
    const string CHOSEN_BALL_NEWKEY = "chosen_ball_key_";

    const string NUMBER_COINS = "number of coins";
    
    const string HIGH_SCORE = "high score";
    const string GAMES_PLAYED = "games played";
    const string REWARDED_PLAYED = "rewarded played";
    const string SOUND_ON = "sound_on";

    const string INTERSTITIAL_PREVIOUS = "interstitial shown in previous game";
    const string REMOVED_ADS = "removed ads";

    public static void UnlockLevel(int levelNumber)
    {
        PlayerPrefs.SetInt(LEVEL_KEY + levelNumber.ToString(), 1);
    }
    public static bool IsLevelUnlocked(int levelNumber)
    {
        int levelValue = PlayerPrefs.GetInt(LEVEL_KEY + levelNumber.ToString());
        bool isLevelUnlocked = (levelValue == 1);

        return isLevelUnlocked;
    }
    public static void ChooseLevel(int levelNumber)
    {
        PlayerPrefs.SetInt(CHOSEN_LEVEL_KEY, levelNumber);
    }
    public static int GetChosenLevelNumber()
    {
        if (!PlayerPrefs.HasKey(CHOSEN_LEVEL_KEY))
        { return 1;}
        else
        { return PlayerPrefs.GetInt(CHOSEN_LEVEL_KEY); }
    }

    public static void LockAllLevels()
    {
        for (int i = 2; i < 30; i++)
        {   PlayerPrefs.SetInt(LEVEL_KEY + i.ToString(), 0);}
    }

    public static void SetNumberOfCoins(int value)
    {
        PlayerPrefs.SetInt(NUMBER_COINS, value);
    }

    public static int GetNumberOfCoins()
    {
        return PlayerPrefs.GetInt(NUMBER_COINS);
    }

    public static void SetHighScore(float value)
    {
        PlayerPrefs.SetFloat(HIGH_SCORE, value);
    }

    public static float GetHighScore()
    {
        return PlayerPrefs.GetFloat(HIGH_SCORE);
    }
    public static void SetGamesPlayed(int value)
    {
        PlayerPrefs.SetInt(GAMES_PLAYED, value);
    }

    public static int GetGamesPlayed()
    {
        return PlayerPrefs.GetInt(GAMES_PLAYED);
    }

    public static void SetSoundOn()
    {
        PlayerPrefs.SetInt(SOUND_ON, 1);

    }
    public static void SetSoundOff()
    {
        PlayerPrefs.SetInt(SOUND_ON, 0);

    }
    public static bool IsSoundOn()
    {
        int get = PlayerPrefs.GetInt(SOUND_ON);
        bool isSoundOn = (get == 1);

        return isSoundOn;

    }
    //public static void LockAllBalls()
    //{
    //    for (int i = 2; i < 30; i++)
    //        PlayerPrefs.SetInt(BALL_KEY + i.ToString(), 0);

    //}
    public static void UnlockBall(int ballNumber)
    {
        PlayerPrefs.SetInt(BALL_NEWKEY + ballNumber.ToString(), 1);

    }
    public static bool IsBallUnlocked(int ballNumber)
    {
        int ballValue = PlayerPrefs.GetInt(BALL_NEWKEY + ballNumber.ToString());
        bool isBallUnlocked = (ballValue == 1);

        return isBallUnlocked;

    }
    public static void ChooseBall(int ballNumber)
    {
        PlayerPrefs.SetInt(CHOSEN_BALL_NEWKEY, ballNumber);
    }
    public static int GetChoosenBallNumber()
    {
       return PlayerPrefs.GetInt(CHOSEN_BALL_NEWKEY);
    }

    public static bool IsAdsRemoved()
    {
        int get = PlayerPrefs.GetInt(REMOVED_ADS);
        bool isRemoved = (get == 1);

        return isRemoved;
    }
    public static void RemoveAds()
    {
        PlayerPrefs.SetInt(REMOVED_ADS, 1);
    }
}
