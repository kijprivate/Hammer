using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialItemShop : MonoBehaviour
{
    [SerializeField] int specialItemIndex;
    [SerializeField] int houseNumber;
    [SerializeField] int cost;
    [SerializeField] Image childImage;
    [SerializeField] Text childText;
    [SerializeField] Sprite grayedSprite;

    private Sprite mySprite;

    private void Awake()
    {
        mySprite = GetComponent<Image>().sprite;

        if (!PlayerPrefsManager.IsHouseUnlocked(houseNumber))
        {
            gameObject.SetActive(false);
        }
        else if (!PlayerPrefsManager.IsSpecialItemUnlocked(specialItemIndex,houseNumber))
        {
            GetComponent<Image>().sprite = grayedSprite;
            if (childText)
            {
                childText.text = cost.ToString();
            }
        }
        else
        {
            GetComponent<Image>().color = Color.white;
            if (childText)
            {
                childText.enabled = false;
            }
            if (childImage)
            {
                childImage.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (PlayerPrefsManager.IsSpecialItemUnlocked(specialItemIndex, houseNumber))
        {
            GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public void OnClick()
    {
        if (!PlayerPrefsManager.IsSpecialItemUnlocked(specialItemIndex,houseNumber) && PlayerPrefsManager.GetNumberOfCoins() >= cost)
        {
            PlayerPrefsManager.UnlockSpecialItem(specialItemIndex,houseNumber);
            GetComponent<Image>().sprite = mySprite;
            PlayerPrefsManager.SetNumberOfCoins(PlayerPrefsManager.GetNumberOfCoins() - cost);
            if (childText)
            {
                childText.enabled = false;
            }
            if (childImage)
            {
                childImage.enabled = false;
            }
            EventManager.RaiseEventCoinsSubstracted();
        }
    }
}
