using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialItemCustomize : MonoBehaviour
{
    [SerializeField] int specialItemIndex;

    [SerializeField] LevelsPanelManager levelsPanelManager;

    private void Awake()
    {
        if(!PlayerPrefsManager.IsSpecialItemUnlocked(specialItemIndex))
        {
            GetComponent<Image>().enabled = false;
            GetComponent<Button>().enabled = false;
        }
        else if(PlayerPrefsManager.GetPlacedSpecialItem(specialItemIndex) == levelsPanelManager.GetLocalHouseIndex()+1)
        {
            var img = GetComponent<Image>();
            img.color = Color.white;
            img.enabled = true;
            GetComponent<Button>().enabled = true;
        }
        else
        {
            var img = GetComponent<Image>();
            img.color = new Color(1f, 1f, 1f, 100f / 255f);
            img.enabled = true;
            GetComponent<Button>().enabled = true;
        }
    }

    public void OnClick()
    {
        if (PlayerPrefsManager.GetPlacedSpecialItem(specialItemIndex) != levelsPanelManager.GetLocalHouseIndex() + 1)
        {
            //place
            PlayerPrefsManager.PlaceSpecialItem(specialItemIndex, levelsPanelManager.GetLocalHouseIndex() + 1);

            var img = GetComponent<Image>();
            img.color = Color.white;
            img.enabled = true;
            GetComponent<Button>().enabled = true;
        }
        else
        {
            //remove
            PlayerPrefsManager.PlaceSpecialItem(specialItemIndex, 0);

            var img = GetComponent<Image>();
            img.color = new Color(1f, 1f, 1f, 100f / 255f);
            img.enabled = true;
            GetComponent<Button>().enabled = true;
        }
    }
}
