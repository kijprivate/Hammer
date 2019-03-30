using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlocker : MonoBehaviour
{
    [SerializeField] int levelNumber;

    private void Update()
    {
        //TODO remove to Awake, in Update only for testing
        if(PlayerPrefsManager.IsLevelUnlocked(levelNumber))
        {
            gameObject.GetComponent<Button>().enabled = false;
            gameObject.GetComponent<Image>().enabled = false;
        }
    }
}
