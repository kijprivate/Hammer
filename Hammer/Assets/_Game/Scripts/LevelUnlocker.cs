using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlocker : MonoBehaviour
{
    [SerializeField] int levelNumber;
    [SerializeField] private int houseNumber;

    private void Update()
    {
        //TODO remove to Awake, in Update only for testing
        if( levelNumber>0 && PlayerPrefsManager.IsLevelUnlocked(levelNumber))
        {
            gameObject.GetComponent<Button>().enabled = false;
            gameObject.GetComponent<Image>().enabled = false;
        }

        if (houseNumber > 0 && PlayerPrefsManager.IsHouseUnlocked(houseNumber))
        {
            gameObject.GetComponent<Image>().enabled = false;
        }
    }
}
