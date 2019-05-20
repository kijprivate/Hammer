using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour
{
    const string LEVEL_KEY = "level_unlocked_";
    const string CHOSEN_LEVEL_KEY = "chosen_level_key_";

    const string HAMMER_KEY = "hammer_unlocked_";
    const string CHOSEN_HAMMER_KEY = "chosen_hammer_key_";

    const string SPECIAL_ITEM_KEY = "special_item_unlocked_";
    const string HOUSE_KEY = "house_unlocked_";

    const string NUMBER_COINS = "number of coins";

    const string HOUSE_PROGRESS = "house_progress_";
    const string LEVEL_PROGRESS = "_level_progress_";
    const string STARS_PROGRESS = "_stars";
    const string HIGHSCORE_PROGRESS = "_highscore";

    const string SOUND_ON = "sound_on";

    public static void SetGainedStars(int houseIndex,int levelIndex,int gainedStars)
    {
        PlayerPrefs.SetInt(HOUSE_PROGRESS + houseIndex.ToString() + LEVEL_PROGRESS + levelIndex.ToString() + STARS_PROGRESS, gainedStars);
    }
    public static int GetGainedStars(int houseIndex, int levelIndex)
    {
        return PlayerPrefs.GetInt(HOUSE_PROGRESS + houseIndex.ToString() + LEVEL_PROGRESS + levelIndex.ToString() + STARS_PROGRESS);
    }
    public static void SetHighScore(int houseIndex, int levelIndex, int highScore)
    {
        PlayerPrefs.SetInt(HOUSE_PROGRESS + houseIndex.ToString() + LEVEL_PROGRESS + levelIndex.ToString() + HIGHSCORE_PROGRESS, highScore);
    }
    public static int GetHighScore(int houseIndex, int levelIndex)
    {
       return PlayerPrefs.GetInt(HOUSE_PROGRESS + houseIndex.ToString() + LEVEL_PROGRESS + levelIndex.ToString() + HIGHSCORE_PROGRESS);
    }
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
    
    public static void UnlockHammer(int hammerNumber)
    {
        PlayerPrefs.SetInt(HAMMER_KEY + hammerNumber.ToString(), 1);
    }
    public static bool IsHammerUnlocked(int hammerNumber)
    {
        int hammerValue = PlayerPrefs.GetInt(HAMMER_KEY + hammerNumber.ToString());
        bool isHammerUnlocked = (hammerValue == 1);

        return isHammerUnlocked;
    }
    public static void ChooseHammer(int hammerNumber)
    {
        PlayerPrefs.SetInt(CHOSEN_HAMMER_KEY, hammerNumber);
    }
    public static int GetChosenHammer()
    {
        if (!PlayerPrefs.HasKey(CHOSEN_HAMMER_KEY))
        { return 0; }
        else
        { return PlayerPrefs.GetInt(CHOSEN_HAMMER_KEY); }
    }

    public static void UnlockSpecialItem(int index,int houseNumber)
    {
        PlayerPrefs.SetInt(SPECIAL_ITEM_KEY + index.ToString() + HOUSE_KEY + houseNumber.ToString(), 1);
    }
    public static bool IsSpecialItemUnlocked(int index,int houseNumber)
    {
        int itemValue = PlayerPrefs.GetInt(SPECIAL_ITEM_KEY + index.ToString() + HOUSE_KEY + houseNumber.ToString());
        bool isItemUnlocked = (itemValue == 1);

        return isItemUnlocked;
    }

    public static void LockAllLevels()
    {
        for (int i = 2; i < 1000; i++)
        {
            PlayerPrefs.SetInt(LEVEL_KEY + i.ToString(), 0);
            PlayerPrefs.SetInt(HOUSE_KEY + i.ToString(), 0);
        }
    }

    public static void LockAllHammers()
    {
        for (int i = 1; i < 30; i++)
        { PlayerPrefs.SetInt(HAMMER_KEY + i.ToString(), 0); }
    }

    public static void UnlockAllLevels()
    {
        for (int i = 2; i < 1000; i++)
        {
            PlayerPrefs.SetInt(LEVEL_KEY + i.ToString(), 1);
            PlayerPrefs.SetInt(HOUSE_KEY + i.ToString(), 1);
        }
    }

    public static void SetNumberOfCoins(int value)
    {
        PlayerPrefs.SetInt(NUMBER_COINS, value);
    }

    public static int GetNumberOfCoins()
    {
        return PlayerPrefs.GetInt(NUMBER_COINS);
    }
    
    public static void UnlockHouse(int houseNumber)
    {
        PlayerPrefs.SetInt(HOUSE_KEY + houseNumber.ToString(), 1);
    }
    public static bool IsHouseUnlocked(int houseNumber)
    {
        int houseValue = PlayerPrefs.GetInt(HOUSE_KEY + houseNumber.ToString());
        bool isHouseUnlocked = (houseValue == 1);

        return isHouseUnlocked;
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
        int get = PlayerPrefs.GetInt(SOUND_ON,1); //default sound on
        bool isSoundOn = (get == 1);

        return isSoundOn;
    }
}
